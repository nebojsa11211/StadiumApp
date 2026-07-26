# Reusable Cups — Design

Status: **Design / not yet built**
Owner: nebojsa11211
Related: [wallet-design.md](wallet-design.md), [delivery-exception-design.md](delivery-exception-design.md), drink inventory (StockMovement) ledger, venue branding / Clubs.

---

## 1. Goal

Offer drinks in **reusable, club-branded cups** with three independently-toggleable modes, plus support for a customer's **own personal cup** identified by a scannable QR. Everything is configured per venue in the Admin app.

The three cup modes:

1. **Deposit rental** — customer pays a refundable deposit; refunded when the cup is returned.
2. **Honor system** — no money changes hands; the venue trusts the customer to return the cup. Value is branding + sustainability; cost is shrinkage.
3. **Bring-your-own-cup (BYOC)** — customer uses their own reusable cup carrying a scannable QR.

### Confirmed product decisions

- **Deposit binding — all combinations configurable.** The venue can enable any/all of: *ticket/wallet-bound*, *physical return token*, *cup-QR bound*. Not a single forced choice — the Cups settings tab exposes each as a toggle and they can be mixed.
- **BYOC QR serves all three purposes at once:** (a) *approved-cup / hygiene* gate — only registered, approved cups with a known volume are accepted; (b) *loyalty / attribution* — links the serving to an owner and powers sustainability stats ("cups saved"); (c) *discount anti-fraud* — the QR is required to claim the BYOC discount.
- **Default refund path = wallet credit** (instant, keeps money in the ecosystem, reuses existing cash-out). Original-method / cash refunds are configurable alternates.
- **Scope: all three modes**, built on a shared foundation, phased so the foundation lands first.

---

## 2. Key architectural insight

Two facts from the codebase shape the whole design:

1. **A cup deposit is economically identical to a wallet debit + refund, and the money rails already support it with no schema change.**
   - Take deposit → `WalletService.TryDebitAsync` / `TryDebitTicketWalletAsync` with a new `referenceType = "CupDeposit"`, plus a `Payment { Direction = In }` recorded atomically via the ledger `afterInsert` hook.
   - Return cup → `WalletService.RefundAsync(...)` posts a `WalletTransactionType.Refund` credit (idempotency key `cup-refund-{depositId}`), exactly the cancelled-order refund pattern.
   - Forfeit (cup never returned) → simply never post the refund; the debit stands and later converts to breakage revenue.
   - All flows are idempotency-keyed against the `WalletTransaction.IdempotencyKey` unique index and audited through `PaymentMethodParser`.

2. **Cups are inventory, but a *loop*, not a *drain*.** A drink `Sale` permanently decrements `Drink.StockQuantity`; a cup rotates issued → in-the-wild → returned → reissued. So cups get their **own append-only ledger** (`CupMovement`), mirroring the `StockMovement` pattern (signed `Delta`, `QuantityAfter` snapshot, inline append inside the same `SaveChanges`) — **not** folded into `StockMovement`.

### Fungible vs. identified cups

- **Deposit & Honor** use a **fungible pool** — thousands of identical logo cups tracked as *counts and deposit liability*, never scanned individually (bar throughput). Integrity for deposits comes from **binding**, not cup identity.
- **BYOC (and optional premium club cups)** require **identity** — a unique QR in a `RegisteredCup` registry.

---

## 3. Data model

### 3.1 `Venue` settings (new columns on the singleton row)

Follows the existing payment-toggle pattern (`WalletPaymentEnabled` etc.), each with `.HasDefaultValue(...)` so existing rows keep working. Because this is a large distinct group, expose it via a **dedicated `CupsSettingsDto` + `GET/PUT api/venue/cups-settings`** (mirroring the *email* slice), not by bloating `AppSettingsDto`.

| Column | Type | Default | Meaning |
|---|---|---|---|
| `CupsEnabled` | bool | false | Master switch for the whole feature |
| `CupDepositModeEnabled` | bool | false | Mode 1 available |
| `CupHonorModeEnabled` | bool | false | Mode 2 available |
| `CupByocEnabled` | bool | false | Mode 3 available |
| `CupDepositAmount` | decimal | 2.00 | Deposit charged per cup |
| `CupDepositBindTicketWallet` | bool | true | Binding combo: deposit recorded against ticket/wallet |
| `CupDepositBindReturnToken` | bool | false | Binding combo: printed return-token QR is proof |
| `CupDepositBindCupQr` | bool | false | Binding combo: individually QR'd deposit cups |
| `CupRefundToWallet` | bool | true | Default refund path = wallet credit |
| `CupRefundToOriginalMethod` | bool | false | Alternate refund path (card async / cash) |
| `CupRefundWindow` | enum/int | EndOfEvent | When an unreturned deposit forfeits → breakage |
| `CupByocDiscountAmount` | decimal | 0.00 | Per-serving discount for BYOC |
| `CupByocRequireApprovedCup` | bool | true | Only registered/approved cups accepted |
| `CupLogo` / `CupLogoContentType` | byte[]/string | null | In-DB cup artwork asset (reuse club-logo upload endpoints) |

**Invariant (enforced server-side, like `HasAnyPaymentMethod`):** if `CupsEnabled`, at least one mode must be on; if `CupDepositModeEnabled`, at least one *binding* combo and one *refund path* must be on. The Save button is disabled and the API rejects otherwise.

### 3.2 `CupType`  *(catalog — supports individually-tracked / premium cups)*

`Id`, `Name`, `VolumeMl`, `UnitCost` (replacement cost for shrinkage accounting), `LogoAssetRef`, `IsActive`. For v1 the fungible pool can run off a single default `CupType`; the entity exists so premium/multi-design cups and BYOC volume standards have a home.

### 3.3 `CupMovement`  *(append-only ledger — mirrors `StockMovement`)*

`Id`, `CupTypeId` (FK), `Delta` (signed — +1 issued into the wild? or model as separate counters; see note), `QuantityAfter` (outstanding-in-wild snapshot), `Type` (`CupMovementType`), `Mode` (`CupMode`), `OrderId?` (FK, `SetNull`), `TicketId?`, `DepositId?`, `Note`, `UserId`/`UserEmail` (actor), `CreatedAt`. Index `{ CupTypeId, CreatedAt }`. Written inline in the same `SaveChanges` as the cup state change.

`enum CupMovementType { Issued, Returned, DepositCharged, DepositRefunded, Forfeited, Shrinkage }`

> Note: "outstanding in the wild" = running sum of Issued(+1)/Returned(−1). Deposit liability is tracked on the deposit records (3.5), not derived from this ledger, so accounting and physical-count reconcile independently.

### 3.4 `RegisteredCup`  *(BYOC + optional premium club cups)*

`Id`, `QrToken` (unique index — mirror `Ticket.QRCodeToken`), `CupTypeId` (approved model → known volume), polymorphic owner (`OwnerType` User/Ticket + nullable `UserId`/`TicketId`, mirroring the `Wallet` owner pattern + its one-owner check constraint), `Status` (Active/Retired/Lost), `IsApproved`, `RegisteredAt`. Resolved at scan time by mirroring `BarTopupController.Resolve`'s `FirstOrDefaultAsync(x => x.QrToken == query)`.

### 3.5 Deposit records

A cup deposit that must be individually refundable/forfeitable is tracked as a lightweight `CupDeposit` row (`Id`, `OrderId?`, `TicketId?`/`WalletId?`, `CupTypeId`, `Amount`, `Status` Held/Refunded/Forfeited, `ChargeTxnId`, `RefundTxnId?`, `ReturnTokenQr?`, `CreatedAt`). The money itself lives in `WalletTransaction`/`Payment`; this row is the *link + state* so a return can find "N outstanding deposits on this ticket" and refund up to that count.

### 3.6 `OrderItem` — per-line cup fields

Add to `OrderItem` (per-line is correct: a round of 4 beers = 4 cup deposits):

```
CupMode        : enum None | Deposit | HonorSystem | ByocQr   (default None)
CupTypeId      : int?    (FK, null for None/BYOC-unregistered)
CupDepositAmount : decimal  (0 unless Deposit)
RegisteredCupId  : int?   (BYOC scanned cup)
```

`enum CupMode { None = 0, Deposit = 1, HonorSystem = 2, ByocQr = 3 }` in Shared.

---

## 4. Money & accounting

- **Deposit is a liability, not revenue.** It must be segregated from drink-sales analytics. `referenceType = "CupDeposit"` on the wallet txn + a dedicated `CupDeposit` row keep it identifiable and nettable.
- **Refund** on return: `RefundAsync` credit (wallet default) or original-method/cash per settings.
- **Breakage:** at `CupRefundWindow` expiry, unreturned `CupDeposit` rows flip Held → Forfeited and their amount is recognized as breakage revenue (reported separately). A background sweep (mirror `LogRetentionBackgroundService`) handles the flip at the window boundary.
- **BYOC** takes no deposit; applies `CupByocDiscountAmount` as a negative line adjustment (only when a valid approved cup QR is present — the discount anti-fraud requirement).
- **Toggle coupling:** deposit charge/refund must respect enabled payment methods (can't refund to wallet if wallet payment is disabled), re-checked server-side exactly like the order-time payment re-check.

Totals: extend `OrderService.CreateOrderAsync` (the `foreach` at ~lines 205–239) so `order.TotalAmount` includes `+ CupDepositAmount` per line and `− byoc discount`, and append the `CupMovement` `Issued`/`DepositCharged` rows in the same `SaveChanges` (same pattern as the `StockMovement` `Sale` append).

---

## 5. Flows

### 5.1 Issue (order time) — Customer app
`DrinkSheet`/`CartBar` gain a per-drink cup選択: None / Deposit / Honor / "Scan my cup" (BYOC). BYOC opens the camera scanner (reuse the `barTopupScanner` JS interop + `OnQrDecoded` pattern from `TicketTopup.razor`), resolves the `RegisteredCup`, validates approved + volume, applies discount. `Checkout` shows deposits as a separate refundable line. Submit → `CustomerSessionOrdersController` → `OrderService.CreateOrderAsync`.

### 5.2 Return / refund — Bar app (new `Cup Returns` page)
Mirror `TicketTopup.razor`: scan the customer's **ticket QR** (or return-token QR, or cup QR — whichever binding combos are enabled) → `BarTopupController.Resolve`-style lookup → show outstanding `CupDeposit` count for that ticket/owner → staff confirms *N* cups returned → for each: `RefundAsync` (or original-method) + flip `CupDeposit` Held → Refunded + append `CupMovement` Returned/DepositRefunded. Honor-mode return: no money, just append `Returned` for shrinkage stats.

### 5.3 BYOC registration
One-time: customer/staff registers a personal cup → creates `RegisteredCup` (QR token, approved `CupType`/volume, owner). Random un-registered cups are rejected when `CupByocRequireApprovedCup` is on.

### 5.4 Offline (Runner) — deferred
Runner deliveries are offline-first (outbox). Deposit *money* offline is risky; v1 keeps deposit charge/refund to online points (Customer checkout + Bar return station) and defers offline cup handling to a later phase.

---

## 6. Admin UI

New **"Cups"** tab in `/admin/settings` (mirror the Payments tab block + a dedicated `SaveCupsAsync`/`cups-settings` endpoint): master switch, per-mode toggles, deposit amount, the three binding-combo toggles, refund-path toggles + window, BYOC discount + approved-cup requirement, cup-logo upload (reuse `club-logo` endpoints), and a **cup dashboard**: outstanding deposit **liability** (money owed back), **return rate**, **shrinkage cost** (unreturned × `UnitCost`), and sustainability ("cups saved"). Localize all strings in `SharedResources.en/hr.resx`.

---

## 7. Phased build

**Phase 0 — Foundation (shared):** `CupMode` enum + `OrderItem` fields; `CupType`, `CupMovement`(+enum), `CupDeposit`, `RegisteredCup` entities; DbContext config + one hand-trimmed migration; `Venue` cup columns + `CupsSettingsDto` + `api/venue/cups-settings` + invariant enforcement; admin Cups **settings** tab.

**Phase 1 — Deposit + Honor (fungible):** issue at Customer checkout (totals + `CupMovement` + `CupDeposit` + wallet debit/Payment); Bar **Cup Returns** page (scan → refund/return); server-side toggle re-checks; breakage sweep; cup dashboard.

**Phase 2 — BYOC:** `RegisteredCup` registration UI, cup-QR scan in Customer checkout, approved-cup + volume enforcement, discount + anti-fraud, sustainability stats.

**Phase 3 — Advanced (later):** individually-tracked premium club cups (cup-QR deposit binding end-to-end), cross-event deposits, offline Runner cup handling, richer liability/shrinkage dashboards.

---

## 8. Open questions / risks

- **Return-token printing:** if `CupDepositBindReturnToken` is enabled, where is the token QR printed (receipt) and scanned? Needs a print surface — confirm the receipt/printing path exists.
- **Hygiene/liability policy** for BYOC pours (staff never touches tap to cup; empty/clean cup) — operational, document for staff.
- **Cash refunds at return** require a cash-drawer/reconciliation surface in the Bar app — heavier than wallet credit; that's why wallet is the default.
- **Guest / OIB-only tickets:** deposit binding for accountless guests rides the anonymous-ticket-wallet; confirm those tickets always have a ticket wallet to refund into.
