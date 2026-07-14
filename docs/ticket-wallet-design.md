# Anonymous Ticket Wallet — Design

> **Goal:** Let a guest with *no account* store value on their ticket. They pay cash/card at a
> counter to load a balance onto the ticket, spend it on drinks during the match, and reclaim any
> remainder afterward. HTTPS-only, EUR, PostgreSQL — and it reuses the existing
> [Fan Digital Wallet](./wallet-design.md) ledger machinery rather than standing up a second one.
>
> **Decisions (confirmed 2026-07-13):**
> - **Balance cap:** €100 per ticket, **closed-loop** (spendable only in-venue) — keeps us clear of
>   e-money / AML thresholds.
> - **Leftover policy:** **claim-by-email first, cash-out as fallback.** Nudge fans to attach an
>   email to keep the balance (skips the full-time refund queue); cash-out for those who won't.
> - **Spend surface:** **Bar (Server) *and* Runner, strictly online-only.** Ticket-wallet debits
>   never enter the Runner offline outbox and fail closed when offline.

---

## 0. What this *is* — a bearer instrument

The balance lives on the **ticket**, not on a person. Whoever physically holds the ticket controls
the money — like cash. Two consequences drive the whole design:

1. **Lost ticket = lost balance.** With no owner on record there is nothing to restore *to*. The
   policy must be stated at top-up time ("this is like cash — guard your ticket") and the claim-by-
   email path (§5.4) is the *only* recovery mechanism, and only if the fan opts in beforehand.
2. **Double-spend is the primary threat.** Two terminals debiting the same ticket, or an
   optimistically-accepted offline debit, both lose real money. Every debit is therefore
   server-authoritative and online-only.

---

## 1. How this grounds into the existing codebase

The registered wallet (`docs/wallet-design.md`) is **built and shipped**. Four facts about it drive
this design:

1. **The ledger and the atomic guard already exist and are proven.** `WalletService.ApplyLedgerEntryAsync`
   does the conditional `UPDATE "Wallets" SET "Balance" = "Balance" + @amt WHERE "Id" = @id AND
   "Status" = 'Active' [AND "Balance" + @amt >= 0]` + ledger insert, inside one
   `CreateExecutionStrategy()` unit (required because the API enables `EnableRetryOnFailure`). We
   reuse this verbatim — it is the correctness core and it is already battle-tested (20-way
   concurrent-debit harness, exactly-N winners, no overdraft).
2. **But it is keyed to a `User`.** `ApplyLedgerEntryAsync` resolves the wallet with
   `w.UserId == userId` (`WalletService.cs:534`), and `Wallet.UserId` is `[Required]`
   (`Wallet.cs:21`). **This is the one real refactor:** the helper must key by *wallet identity*,
   not by user, so a ticket-owned wallet can use the same guarded path. See §4.
3. **`WalletStatus.Closed` already exists** (`Wallet.cs:60`) — the terminal "cashed-out" state needs
   no new enum value.
4. **The ticket is already a scannable token.** `Ticket` has `TicketNumber` and a unique
   `QRCodeToken` (`Ticket.cs:18,31`) and `Status` (Active/Used/Cancelled). The Bar `TicketTopup.razor`
   page already resolves a scanned code → identity. We extend that resolve step to also mint/return a
   ticket wallet.

We also mirror the same two established patterns the fan wallet used: **idempotent replay** (every
mutation carries a unique `IdempotencyKey`) and the **reconciliation background service** shape.

---

## 2. Ownership model — one wallet concept, two kinds of owner

Rather than a parallel `TicketWallet` table (which would duplicate the ledger, the guard, and the
reconciliation job), we make the existing `Wallet` **polymorphic in its owner**. A wallet is owned by
**exactly one** of:

- a **`User`** (the existing registered fan wallet), or
- a **`Ticket`** (the new anonymous bearer wallet).

```
Wallet
  OwnerType : { User, Ticket }        -- NEW discriminator
  UserId    : int?                    -- was [Required]; now nullable
  TicketId  : int?  FK → Ticket       -- NEW
  -- CHECK ((UserId IS NULL) <> (TicketId IS NULL))   exactly one owner
```

The append-only `WalletTransaction` ledger hangs off `WalletId` and is **unchanged in shape**, so
running-balance, the `BalanceAfter` snapshot, admin ledger views, and the reconciliation invariant
(`Balance == Σ Completed amounts`) all work for ticket wallets for free.

**Why polymorphic-owner beats a separate table:** the claim-by-email upgrade (§5.4) becomes a
one-row ownership flip instead of a cross-table data migration, and there is exactly one guarded
debit path to keep correct.

---

## 3. Data model

### `Wallet` — alterations (existing table)
| Field | Change | Notes |
|---|---|---|
| `OwnerType` | **add** `int`/enum | `User` (0) / `Ticket` (1). Backfill existing rows to `User`. |
| `UserId` | **relax** to `int?` | drop `[Required]`; keep FK. |
| `TicketId` | **add** `int?` FK → Ticket | one wallet per ticket. |
| `Status` | unchanged | reuse `WalletStatus.Closed` for a cashed-out ticket wallet. |

Constraints / indexes:
- `CHECK ((UserId IS NULL) <> (TicketId IS NULL))` — exactly one owner.
- Existing unique index on `UserId` → make it a **filtered unique** index `WHERE UserId IS NOT NULL`.
- **New filtered unique** index on `TicketId` `WHERE TicketId IS NOT NULL` — one wallet per ticket.

### `WalletTransaction` — new `Type` values (existing table, shape unchanged)
`WalletTransactionType` today is `Deposit=0, Payment=1, Refund=2, Reversal=3, Adjustment=4`
(`WalletTransaction.cs:72`). Add:

| Value | Meaning | Sign |
|---|---|---|
| `CashOut = 5` | Balance handed back as cash at the counter (terminal). | − (debit) |
| `Claim = 6` | Balance moved to a registered user wallet via email claim. | −/+ pair |

Top-ups reuse `Deposit`; drink spends reuse `Payment`. `CashOut` is deliberately distinct from
`Refund` so end-of-match cash payouts are separable from per-order cancellations in reporting.

### No new fields for the cap
The €100 cap and closed-loop rule are **config**, not columns: `TicketWallet:MaxBalance = 100`,
enforced in the top-up validation. Keeping it out of the schema lets ops tune it without a migration.

### Invariants (unchanged from fan wallet, now also for ticket wallets)
1. `Wallet.Balance == Σ Amount` over that wallet's `Completed` transactions.
2. Balance never negative (guaranteed by the guarded debit).
3. Ledger rows are immutable once `Completed`; corrections are new rows.

---

## 4. Concurrency & correctness — the required refactor

The **only** structural change to proven code: `ApplyLedgerEntryAsync` must key by wallet, not user.

**Today (`WalletService.cs:523,534`):**
```csharp
private async Task<...> ApplyLedgerEntryAsync(int userId, WalletTransactionType type, ...)
{
    ...
    var wallet = await _context.Wallets.AsNoTracking()
        .FirstOrDefaultAsync(w => w.UserId == userId);   // ← user-bound
```

**Proposed:** split the wallet *resolution* from the *guarded mutation*.
```csharp
// Core: operates on an already-resolved wallet id. Guard SQL is unchanged.
private async Task<...> ApplyLedgerEntryToWalletAsync(int walletId, WalletTransactionType type, ...)

// Thin resolvers keep every existing caller working:
//   user path   → resolve by UserId   → ApplyLedgerEntryToWalletAsync(wallet.Id, ...)
//   ticket path → resolve by TicketId → ApplyLedgerEntryToWalletAsync(wallet.Id, ...)
```

The guarded `UPDATE ... WHERE "Id" = @id AND "Status" = 'Active' [AND "Balance" + @amt >= 0]` and
the `CreateExecutionStrategy()` wrapper are **untouched** — they already key on `wallet.Id`. This
refactor is pure extraction; the fan-wallet callers call the same core through the user resolver.

Everything else — idempotency-key unique index, row-lock serialisation of concurrent writers,
`xmin` defense-in-depth — carries over unchanged.

---

## 5. Flows

### 5.1 Top-up (load funds onto a ticket)
```
Bartender scans ticket QR → ResolveTicketWalletAsync(codeOrToken)
  ├─ ticket not found / Cancelled       → refuse
  ├─ wallet exists, Status = Closed      → refuse ("ticket already cashed out")
  ├─ wallet exists, Status = Active      → return current balance
  └─ no wallet                           → create { OwnerType=Ticket, TicketId, Balance=0, Active }
Fan pays €30 cash/card → bartender enters amount, confirms
  ↓  POST /api/bar/ticket-wallet/topup { ticketId, amount, tender, idempotencyKey }
     validate: amount > 0  AND  balance + amount <= TicketWallet:MaxBalance (€100)
     ApplyLedgerEntryToWallet(walletId, Deposit, +amount, key)     ← atomic UPDATE + ledger insert
     write Payment { Method, Amount=+amount, WalletTransaction=txn } (records the tender)
  ↓  return { newBalance }  → receipt: "Balance €30 — guard this ticket, it's like cash"
```
This is ~90% the existing `TicketTopup.razor` submit; the delta is "resolve to a **ticket** wallet,
creating one if absent" instead of "resolve to the linked user account."

### 5.2 Spend (pay for a drink order) — Bar and Runner, online-only
Reuses the fan-wallet spend path in `OrderService.CreateOrderAsync` with the ticket wallet as payer:
```
Fan orders, presents ticket → POS scans QR
  ↓  CreateOrder { …, PaymentMethod = DigitalWallet, ticketId }
     order saved first (existing pattern)
     TryDebit ticket wallet: ApplyLedgerEntryToWallet(walletId, Payment, -total,
                                                       key="order-{orderId}", requireFunds:true)
       → UPDATE ... WHERE Status='Active' AND Balance - total >= 0
       → 0 rows ⇒ HTTP 402 ⇒ CompensateUnpaidOrderAsync (void order, restore stock)
```

**Runner (WASM PWA) online-only enforcement — the guardrail for decision #3:**
- Ticket-wallet debits **never** enter the offline outbox. The outbox keeps its current order types;
  a ticket-wallet payment takes a separate path requiring a live `200`/`402` from the debit endpoint
  before the order is presented as accepted.
- If the Runner is offline at spend time it **fails closed**: "connection required to charge a
  ticket balance" — it does **not** optimistically accept and reconcile later. Optimistic + bearer =
  double-spend across two waiters on one ticket.
- The server atomic guard remains the real backstop; the Runner rule only avoids ever *showing* a
  spend as accepted when the server didn't confirm it.

### 5.3 Cash-out (reclaim remainder) — the highest-fraud moment
All-or-nothing, single-shot, server-authoritative:
```
Fan returns ticket after match → bartender scans
  ↓  POST /api/bar/ticket-wallet/cashout { ticketId, idempotencyKey = "cashout-{walletId}" }
     one CreateExecutionStrategy unit:
       UPDATE "Wallets" SET "Balance" = 0, "Status" = 'Closed', "UpdatedAt" = @now
       WHERE "Id" = @id AND "Status" = 'Active'
       RETURNING (old) "Balance"                    -- = amount to hand back
       insert WalletTransaction { Amount = -oldBalance, Type = CashOut, key="cashout-{walletId}" }
       write Payment { Method = CashPayout, Amount = -oldBalance, WalletTransaction = txn }
  ↓  0 rows ⇒ already Closed ⇒ idempotent no-op (return "already cashed out", pay nothing)
     1 row  ⇒ return { refundAmount = oldBalance } → bartender hands over cash
```
Three properties fall out:
- **No double refund** — fixed key `cashout-{walletId}` + `WHERE Status='Active'`: a replay/re-scan
  affects 0 rows and pays nothing.
- **Spend vs cash-out race** — the atomic guard serialises them; the loser sees the changed status.
- **Refund the *server's* number**, via `RETURNING Balance` — never a figure off a printed receipt,
  so a doctored receipt buys nothing.

### 5.4 Claim-by-email (the preferred leftover path)
Lets a fan attach an email *any time* to make the balance recoverable and skip the cash-out queue:
```
Fan gives email → ClaimTicketWallet(ticketId, email)
  find-or-create the User (email-only, lightweight) and their wallet
  ┌ user has NO wallet yet: flip ownership in place (ledger history stays attached)
  │    UPDATE "Wallets" SET "OwnerType"=User, "UserId"=@u, "TicketId"=NULL
  │    WHERE "Id"=@id AND "Status"='Active'
  └ user ALREADY has a wallet: transfer
       CashOut ticket wallet (Type=CashOut, key) → credit user wallet (Type=Claim, same key)
```
After claiming, the balance lives on the account, survives the match, and there is nothing to refund
by hand — directly attacking the "stadium's worth of small balances at full time" problem.

---

## 6. API surface

**Bar / Staff** (`[Authorize]`, Bartender/Staff role — the counter operates the ticket wallet on the
guest's behalf; the client supplies the resolved `ticketId`, never a `walletId`):
- `POST /api/bar/ticket-wallet/resolve` — scan code/token → `{ ticketId, balance, status }` (creates on first top-up, not on resolve)
- `POST /api/bar/ticket-wallet/topup` — load funds `{ ticketId, amount, tender, idempotencyKey }`
- `POST /api/bar/ticket-wallet/cashout` — reclaim remainder `{ ticketId, idempotencyKey }`
- `POST /api/bar/ticket-wallet/claim` — attach email `{ ticketId, email }`

**Spend has no dedicated endpoint** — it happens implicitly when an order is placed with
`PaymentMethod = DigitalWallet` + a `ticketId` payer, via `CreateOrderAsync` (Bar and Runner).

**Admin** — the ticket wallets appear in the **existing** `/api/admin/wallets` list/ledger/
freeze/refund surface (they're just `Wallet` rows with a `Ticket` owner); no new admin endpoints,
only an owner-type column/filter.

---

## 7. Services

Extend the existing `IWalletService` rather than adding a parallel service — it already owns the
guarded ledger:
- `ResolveTicketWalletAsync(codeOrToken)` — ticket lookup (`TicketNumber`/`QRCodeToken`) → summary.
- `GetOrCreateTicketWalletAsync(ticketId)` — mint an Active, zero-balance ticket wallet.
- `TopUpTicketWalletAsync(ticketId, amount, tender, key)` — cap-checked `Deposit` credit + `Payment`.
- `CashOutTicketWalletAsync(ticketId, key)` — the atomic zero-and-close in §5.3.
- `ClaimTicketWalletAsync(ticketId, email)` — the ownership flip / transfer in §5.4.
- Internally: the extracted `ApplyLedgerEntryToWalletAsync(walletId, …)` core (§4).

`WalletReconciliationBackgroundService` needs **no change** — it already checks `Balance == Σ ledger`
per wallet id and is self-gated on `WalletReconciliation:Enabled` (default off) per the repo's
hosted-service posture.

All mutations emit centralized-log `Audit`/`Security` entries (who topped up / cashed out which
ticket, amount, station).

---

## 8. UI

**Bar (`StadiumDrinkOrdering.Bar`, Server)** — extend the existing `Pages/TicketTopup.razor`:
- Resolve step already scans a code. Add the **anonymous branch**: when the ticket has no linked
  account, offer "Load funds on this ticket" (create + top up) instead of only the current
  "no account — ask fan to register" dead end.
- New **cash-out** action on a resolved ticket wallet (shows server balance, confirm → pay out).
- Prominent "this balance is like cash — guard the ticket" notice at top-up. `bar-ticket-wallet-*`
  element IDs, EN/HR resx keys per project convention.

**Runner (`StadiumDrinkOrdering.Runner`, WASM PWA)** — ticket-wallet spend at the point of order,
with the online-only guard from §5.2 surfaced as a clear "connection required" state when offline.

**Admin** — owner-type column + filter on the existing `/admin/wallets` page so ticket wallets are
visible/manageable alongside user wallets.

---

## 9. Migrations

One migration:
- `Wallet`: add `OwnerType`, add `TicketId` (FK → Ticket), relax `UserId` to nullable; backfill
  existing rows `OwnerType = User`.
- Swap the `UserId` unique index for a **filtered** unique index (`WHERE UserId IS NOT NULL`); add a
  filtered unique index on `TicketId`; add the exactly-one-owner `CHECK`.
- Extend `WalletTransactionType` with `CashOut`, `Claim` (enum-only; no DDL if stored as int).
- **Hand-trim the scaffolded migration** down to real DDL (the repo's generated migrations carry
  ~5,800 lines of seed-timestamp churn) and fix enum `defaultValue`s — per the project's migration
  hygiene practice. Use `Host=127.0.0.1` for `dotnet ef` to dodge the IPv6/Docker-proxy reset.

**Backward-compat risk:** the `UserId`-nullable change and the index swap touch the live fan-wallet
table. The backfill + filtered-index rewrite must run as one migration; verify against the local PG
container before anything else.

---

## 10. Regulatory & ops guardrails

- **€100 cap, closed-loop.** Low value + single venue + spendable only in-venue keeps this out of
  EU e-money / AML territory. Do **not** let it become a general reloadable cash card: no
  cross-venue spend, no external payout beyond the original cash/card cash-out.
- **Cash-out queue mitigation.** Claim-by-email is offered *first* at every leftover touchpoint.
  (Optional future: auto-forfeit balances under a small threshold — not enabled now.)
- **Audit trail.** Every top-up, spend, cash-out and claim is a signed ledger row + a `Payment` row
  + a centralized-log entry, so any counter's cash position reconciles to the ledger.

---

## 11. Edge cases (explicitly handled)

| Case | Handling |
|---|---|
| Lost/stolen ticket | Bearer instrument — balance not recoverable unless previously claimed-by-email. Stated at top-up. |
| Concurrent debits on one ticket | Atomic guarded UPDATE — at most one wins (existing proven guard). |
| Offline waiter (Runner) tries to charge | Fails closed; never queued in the offline outbox. |
| Double / replayed cash-out | Fixed key `cashout-{walletId}` + `WHERE Status='Active'` ⇒ 0 rows, pays nothing. |
| Spend racing a cash-out | Serialised by the row lock; loser sees `Status=Closed`. |
| Top-up over the €100 cap | Validation rejects (`balance + amount <= MaxBalance`). |
| Top-up on a cashed-out ticket | Refused — `Status=Closed` is terminal. |
| Doctored receipt at cash-out | Refund uses `RETURNING Balance` (server truth), not the printed figure. |
| Claim when user already has a wallet | Transfer (CashOut ticket → Claim credit), not an ownership flip. |
| Rounding | `decimal(10,2)` throughout. |

---

## 12. Phasing

1. **Refactor** — extract `ApplyLedgerEntryToWalletAsync(walletId, …)` from the user-keyed helper;
   fan-wallet regression harness must stay green (no behaviour change).
2. **Model + migration** — polymorphic `Wallet` owner, new `Type` values, indexes/CHECK; verify on
   local PG.
3. **Top-up + cash-out** — `IWalletService` methods + `/api/bar/ticket-wallet/*` endpoints; extend
   `TicketTopup.razor` (anonymous branch + cash-out). Ship this first — it's the counter MVP.
4. **Spend** — `DigitalWallet` payment by ticket in `CreateOrderAsync`; Bar first, then Runner with
   the online-only guard.
5. **Claim-by-email** — ownership flip / transfer + minimal email-only user creation.
6. **Admin surfacing** — owner-type column/filter on `/admin/wallets`.

Each phase is independently shippable and Playwright-testable per the project protocol.
