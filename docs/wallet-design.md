# Fan Digital Wallet — Design

> **Goal:** Every fan who holds a purchased season ticket gets a personal digital wallet.
> They can deposit real funds and spend that balance on in-stadium purchases (drinks today,
> anything payable later). HTTPS-only, EUR, PostgreSQL — consistent with the rest of the system.

> **v1 scope (decided):**
> - **Season-ticket → user linking:** auto-match by email (`HolderEmail == User.Email`) on register/login.
> - **Spend scope:** drinks only — wallet pays for in-stadium drink orders via `CreateOrderAsync`.
> - **Withdrawals:** admin-only refund-to-source; no self-service payout in v1 (no payout gateway needed yet).

---

## 1. How this grounds into the existing codebase

Four existing facts drive every decision below:

1. **`SeasonTicket` is not linked to a `User`.** It carries `HolderName/HolderEmail/HolderPhone`
   and can originate externally (TicketingSimulator / ingestion). "Fan with a season ticket" is
   therefore *not* a first-class relationship yet — we must create it.
2. **Drink orders currently charge nothing.** `OrderService.CreateOrderAsync` validates a ticket,
   decrements stock and writes an `Order` — it never creates a `Payment`. The wallet becomes the
   **first real in-stadium payment rail**, so we wire it into that method rather than bolting on a
   parallel flow.
3. **The schema already half-anticipates this:** `PaymentMethod.DigitalWallet = 3` exists and
   `Payment.OrderId` is already nullable.
4. **No optimistic-concurrency token exists anywhere in the model.** Money debits need one added
   deliberately — this is the single most important correctness concern.

We also mirror two established patterns: the **idempotent offline-outbox retry** discipline in
`OrderService` (replayed operations must be no-ops) and the **hosted background-service** pattern
(`LogRetentionBackgroundService`) for the reconciliation job.

---

## 2. Ownership model — wallet belongs to the *fan*, not the *pass*

- **One wallet per `User`** (not per season ticket). A family may hold several season tickets; a
  season ticket expires each year; deposited money must not. So the wallet is owned by the User and
  the season ticket is only the **eligibility gate to create one**.
- We add a nullable **`SeasonTicket.UserId`** FK. It's populated by a **link step**: on
  register/login, match any `SeasonTicket` rows by `HolderEmail == User.Email` (email already
  indexed) and attach them to the user. The FK — not live email matching — becomes the source of
  truth thereafter (email can change; shared/typo'd emails shouldn't silently grant access).
  - Guard: only link when the email is verified / the login succeeded for that account. An explicit
    "claim my season ticket" action (enter season-ticket number) is the stronger variant for v2.
- **Eligibility rule:** `user has ≥1 active SeasonTicket ⇒ may hold a wallet`.
- **Lifecycle:** eligibility gates *creation only*. Once created, the wallet persists and the
  balance is the fan's money even after the season ends — we never confiscate stored value (this is
  a legal/ethical requirement, not a nicety). A fan whose pass lapsed can still spend down or be
  refunded their balance; re-gating deposits on active status is a business toggle, defaulted off.

---

## 3. Data model

Two new tables. **The ledger is the source of truth; the wallet's `Balance` is a cached projection**
updated in the same transaction as every ledger append.

### `Wallet` (account)
| Field | Type | Notes |
|---|---|---|
| `Id` | int PK | |
| `UserId` | int FK → User | **unique** — one wallet per user |
| `Balance` | decimal(10,2) | cached projection of the ledger; never written outside a guarded mutation |
| `Currency` | char(3) | `"EUR"` |
| `Status` | string(20) | `Active` / `Frozen` / `Closed` |
| `Version` | `xmin` | PostgreSQL system column mapped as concurrency token (`UseXminAsConcurrencyToken`) |
| `CreatedAt` / `UpdatedAt` | timestamptz (UTC) | |

### `WalletTransaction` (ledger — append-only, immutable)
| Field | Type | Notes |
|---|---|---|
| `Id` | long PK | |
| `WalletId` | int FK → Wallet | indexed |
| `Type` | enum | `Deposit`, `Payment`, `Refund`, `Reversal`, `Adjustment` |
| `Amount` | decimal(10,2) | **signed**: credits +, debits − |
| `BalanceAfter` | decimal(10,2) | running balance snapshot → cheap audit + reconciliation |
| `Currency` | char(3) | must equal wallet currency |
| `Status` | string(20) | `Pending` (deposits mid-gateway) / `Completed` / `Failed` |
| `IdempotencyKey` | string(100) | **unique** — the anti-double-charge guard |
| `ReferenceType` / `ReferenceId` | string / int? | e.g. `Order` + OrderId, `Payment` + PaymentId |
| `Description` | string(500) | |
| `CreatedByUserId` | int? | fan or admin who caused it |
| `CreatedAt` | timestamptz (UTC) | |

### Alterations to existing tables
- `SeasonTicket`: **add** `UserId int? FK → User` (nullable, indexed).
- `Payment`: **add** `WalletTransactionId long? FK` so a `Payment` can fund either an order
  (`OrderId`) or a wallet **deposit** (`WalletTransactionId`). Both already/now nullable.

### Invariants (enforced in code, checked by a job)
1. `Wallet.Balance == Σ Amount` over that wallet's `Completed` transactions.
2. Balance never negative (guaranteed by the guarded debit in §4).
3. A `WalletTransaction` row is **never updated or deleted** once `Completed` — corrections are new
   `Reversal`/`Adjustment` rows. (Deposit rows may transition `Pending → Completed/Failed` only.)

---

## 4. Concurrency & correctness — the core of the design

Two concurrent drink orders must never overdraw the balance, and no retry may double-charge.

### Atomic guarded debit (authoritative)
Every debit is a **single conditional UPDATE**, not a read-modify-write:

```sql
UPDATE "Wallets"
SET "Balance" = "Balance" - @amount, "UpdatedAt" = @now
WHERE "Id" = @walletId AND "Status" = 'Active' AND "Balance" >= @amount;
```

- **0 rows affected ⇒ insufficient funds or lost race ⇒ fail** (surface `InsufficientFunds`).
- **1 row affected ⇒ debit won**; append the ledger row (`BalanceAfter` read back) in the **same DB
  transaction**. Commit atomically.

This eliminates the race at the database level regardless of isolation level. `xmin` concurrency
token on the EF-tracked `Wallet` is defense-in-depth for any code path that goes through the
change-tracker instead of the raw UPDATE.

### Idempotency
Every mutation carries a caller-supplied `IdempotencyKey` with a **unique index**. Before mutating,
look up the key: if a transaction already exists, **return the original result** (no second ledger
row). This makes deposit webhooks and the existing order-retry/offline-outbox behavior safe. Debit
key is order-scoped (e.g. `order-{orderId}` or a client GUID from the order request); deposit key is
the gateway `TransactionId`.

### Transaction boundaries
All balance changes run inside an explicit `IDbContextTransaction`. For the order-spend path the
order insert, stock decrement, wallet debit, `Payment` insert and ledger append **share one
transaction** — any failure rolls back everything, so a failed order never leaves money debited.

---

## 5. Flows

### Deposit (add funds)
1. `POST /api/wallet/me/deposits { amount, method, idempotencyKey }` → create `Payment`
   (`WalletTransactionId` set, no `OrderId`) + a `Pending` `Deposit` transaction. Return the
   gateway intent (redirect/clientSecret). *Balance unchanged.*
2. Gateway processes (mock now via `IWalletPaymentGateway`, Stripe/PayPal later).
3. Success callback/webhook → idempotent on gateway `TransactionId`: mark `Payment` Completed,
   **credit** the wallet (append ledger, bump cached balance atomically), flip the transaction to
   `Completed`.
4. Failure → mark `Failed`, no balance change.

Validation: amount > 0, ≤ configured per-deposit cap; wallet `Active`; currency EUR.

### Spend (pay for a drink order with the wallet)
Wire into `OrderService.CreateOrderAsync` with a new `PaymentMethod` argument:
1. Existing validation (ticket/session, drinks, stock) + compute `TotalAmount`.
2. Open transaction → insert order+items, decrement stock.
3. **Guarded debit** wallet by `TotalAmount`, key `order-{clientOrderGuid}`.
   - 0 rows ⇒ throw `InsufficientFunds` ⇒ rollback ⇒ order not created (surface an inline top-up
     prompt to the fan).
4. Insert `Payment` (`DigitalWallet`, `Completed`, `TransactionId` = ledger ref) + `Payment`-type
   ledger row (Reference = `Order`/OrderId).
5. Commit.

### Refund on cancel
`OrderService.CancelOrderAsync` already restores stock. Extend it: if the order was wallet-paid,
**credit** the wallet back (`Refund`, Reference = OrderId, idempotency key `refund-order-{orderId}`
so a double-cancel can't double-refund). Refund amount ≤ captured amount (guard).

### Admin adjustments
Never edit `Balance` directly. Admin freeze/unfreeze toggles `Status`; corrections/comps post
`Adjustment` (±) or `Reversal` ledger rows with a mandatory reason and `CreatedByUserId`.

---

## 6. API surface

**Customer** (`[Authorize]`, role Customer; wallet is *always* derived from the JWT `userId`
server-side — client never supplies a walletId/userId):
- `GET  /api/wallet/me` — balance, currency, status, eligibility
- `GET  /api/wallet/me/transactions?page=&pageSize=` — paginated ledger
- `POST /api/wallet/me/deposits` — initiate deposit
- `POST /api/wallet/deposits/webhook` — gateway confirmation (idempotent; signature-verified)

**Admin** (`[Authorize]` role Admin):
- `GET  /api/admin/wallets`, `GET /api/admin/wallets/{id}/transactions`
- `POST /api/admin/wallets/{id}/adjust { amount, reason }`
- `POST /api/admin/wallets/{id}/freeze` · `/unfreeze`
- `POST /api/admin/wallets/{id}/refund { amount, reason }`

Spend has **no dedicated endpoint** — it happens implicitly when an order is placed with
`PaymentMethod = DigitalWallet`.

---

## 7. Services

- **`IWalletService`** — `GetOrCreateForUserAsync`, `GetSummaryAsync`, `InitiateDepositAsync`,
  `ConfirmDepositAsync`, `TryDebitAsync` (guarded, for orders), `RefundAsync`, `AdjustAsync`,
  `FreezeAsync`. Every mutation is idempotent + transactional.
- **`IWalletPaymentGateway`** — `MockWalletPaymentGateway` now (mirrors the existing simulated
  payment approach); real provider later. One seam, swap the impl.
- **`WalletReconciliationBackgroundService`** — periodic (e.g. nightly) recompute of
  `Σ ledger == Balance` per wallet; log drift as a `Security`/`Audit` centralized-log event and
  alert. Same shape as `LogRetentionBackgroundService`.
- **Season-ticket linker** — on register/login, attach matching `SeasonTicket`s to the `User`.

All balance mutations emit centralized-log entries (`Audit`/`Security`). Deposit endpoint sits
behind the existing rate-limiting middleware.

---

## 8. UI

**Customer Blazor app** (HTTPS, EN/HR resx, `customer-wallet-*` element IDs per the project's ID
convention):
- `/wallet` page — balance card, deposit (amount + method), paginated transaction history.
- Ordering/Checkout — add a **"Pay with Wallet (€X available)"** option; on `InsufficientFunds`
  show an inline top-up CTA.

**Admin Blazor app** — Wallet management under a new nav item: search wallets, view ledger,
freeze/adjust/refund with reason. `admin-wallet-*` IDs.

---

## 9. Migrations

- New tables `Wallets`, `WalletTransactions`; add `SeasonTicket.UserId`, `Payment.WalletTransactionId`.
- Configure `xmin` concurrency token on `Wallet`; unique indexes on `Wallet.UserId` and
  `WalletTransaction.IdempotencyKey`; index `WalletTransaction.WalletId`.
- **Known gotcha:** scaffolded migrations in this repo carry ~5,800 lines of seed-timestamp churn —
  hand-trim the generated migration down to the real DDL (and fix enum `defaultValue`s) before
  committing, per the project's migration hygiene practice.

---

## 10. Edge cases (explicitly handled)

| Case | Handling |
|---|---|
| Concurrent debits | Atomic guarded UPDATE — at most one wins |
| Deposit webhook fires twice | Idempotency key = gateway TransactionId (unique) |
| Order creation fails after debit | Single transaction → rollback → no debit |
| Double cancel | Refund idempotency key `refund-order-{id}` |
| Email collision on link | FK is source of truth; require verified email / explicit claim |
| Lapsed season ticket w/ balance | Never confiscated; spend/refund still allowed |
| Refund > captured | Guard: refund ≤ captured amount |
| Frozen wallet | Debit UPDATE requires `Status='Active'`; deposits blocked |
| Zero/negative deposit | Validation rejects |
| Currency mismatch | Rejected; single-currency EUR |
| Rounding | `decimal(10,2)` throughout — no floating point |

---

## 11. Phasing

1. **Foundation** — models, DbContext config, migration, `IWalletService` (deposit via mock +
   balance + ledger + idempotency + guarded concurrency), season-ticket→user linker.
2. **Spend** — wallet payment inside `CreateOrderAsync`; refund-on-cancel.
3. **Customer UI** — `/wallet` page + checkout integration + EN/HR + element IDs.
4. **Admin & ops** — admin management page + reconciliation background service + refund-to-source.
5. **Real gateway** — replace `MockWalletPaymentGateway` with Stripe/PayPal.

Each phase is independently shippable and testable (Playwright per project protocol).
