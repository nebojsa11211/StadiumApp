# Delivery Exception ("Couldn't Deliver") Design

## Problem

A runner claims a prepared order and walks it to the seat, but **the fan isn't there** ("not
on the chair"). Today the delivery card (`Runner/Pages/MyDeliveries.razor`) offers exactly one
action — **✓ Mark delivered** — so the runner has no honest way to offload a paid, already-poured
drink. The order either sits `OutForDelivery` clogging their list, or gets falsely marked
`Delivered` (money kept, drink dumped, no audit trail).

The scenarios this must cover:

1. **Fan temporarily away** (toilet, concourse) — will be back.
2. **Seat empty / wrong seat** — fan never there, moved, or bad seat data.
3. **Fan refuses** — wrong drink, changed mind, too drunk to serve.

## Decisions (confirmed)

- **Runner action:** one **"Couldn't deliver"** button that opens a quick reason sheet
  (Not at seat / Refused / Wrong seat / Other). Fastest to tap mid-shift, single code path.
- **Resolution:** the **Bar** app triages. The returned drink physically comes back to the bar,
  so the bartender decides: **Retry** (→ back to the pool) or **Cancel & refund**
  (reuses the existing wallet-refund + restock path). No auto-refund, no admin bottleneck.

## State machine

```
Ready → (claim) → OutForDelivery → Delivered                      (happy path, unchanged)
                       │
                       └→ DeliveryFailed(reason, attempt#, byRunner)   ← NEW
                              │
                              ├→ Ready       (Bar: Retry → returns to shared pool)
                              └→ Cancelled   (Bar: Cancel & refund → existing CancelOrder path)
```

- **New `OrderStatus.DeliveryFailed = 8`** — a distinct, auditable exception state, NOT reuse of
  `Cancelled`. We must retain *why* it failed and *how many* attempts were made.
- **No auto-bounce to the pool.** If the fan isn't at the seat, the next runner won't fare better;
  re-pooling would fail-loop. The order parks in a triage queue until a human decides.

## Data model (`Shared/Models/Order.cs`)

Add:

```csharp
public int DeliveryAttempts { get; set; }              // incremented on each failed attempt
public DeliveryFailureReason? LastDeliveryFailureReason { get; set; }
public DateTime? LastDeliveryAttemptAt { get; set; }
public int? LastDeliveryAttemptByUserId { get; set; }  // runner who reported the failure
```

```csharp
public enum DeliveryFailureReason
{
    CustomerNotAtSeat = 1,
    CustomerRefused = 2,
    WrongSeat = 3,
    Other = 99
}
```

`OrderStatus` gains `DeliveryFailed = 8`.

Migration: one enum value + four columns. Hand-trim the scaffolded seed-timestamp churn to the
real ops only (see [[ef-migration-seed-churn]]).

## API

**Runner → report failure** (offline-outbox compatible, idempotent via `ClientActionId`):

```
POST /orders/{id}/delivery-failed
body: { reason, notes?, clientActionId }
```

- Guard: order must be `OutForDelivery` and assigned to the calling runner.
- Effect: `Status = DeliveryFailed`, increment `DeliveryAttempts`, stamp reason/time/user,
  **clear `AssignedStaffId`** (it leaves the runner's list; history is kept on the attempt fields).
- Idempotent: a replay with the same `ClientActionId` (or an order already `DeliveryFailed`) is a
  no-op success — mirrors the existing `Delivered` outbox retry semantics.

**Bar → resolve:**

- Retry: reuse `PUT /orders/{id}/status` → `Ready` (re-enters the pool via the existing
  `GetAvailableForDeliveryAsync` filter: `Status == Ready && AssignedStaffId == null`).
- Cancel & refund: reuse the existing cancel path. NOTE `CancelOrderAsync` currently guards
  `Status == Pending`; it must also accept `DeliveryFailed` so refund + restock run for a
  returned drink.

**Pool query unchanged** — `DeliveryFailed` orders are excluded from the runner pool by design;
they only appear in the Bar returns tab.

## Runner UI (`MyDeliveries.razor` + `OutboxService`)

- Second button on each active delivery card: **"Couldn't deliver"** → reason sheet (bottom sheet
  on mobile). Selecting a reason enqueues an outbox action and optimistically drops the card.
- Extend `OutboxAction.Type` with `"delivery-failed"` carrying `Reason` (+ optional note); the
  flush switch calls a new `RunnerApiService.ReportDeliveryFailedAsync`. Same `ClientActionId`
  idempotency as `deliver`.
- Localize new strings in `Runner/Resources/SharedResources.{en,hr}.resx` (see [[i18n-all-apps-localized]]).

## Bar UI

- A **"Returns / Needs attention"** section (new tab or a band on the order queue) listing
  `DeliveryFailed` orders with seat, items, reason, attempt count, and the runner who reported it.
- Two buttons per row: **Retry delivery** and **Cancel & refund**. Localize EN/HR.

## Out of scope (v1)

- Runner "snooze / try again" that keeps a *fan-away* order in the runner's own list for one more
  loop before it becomes a formal failure. Nice for scenario 1; pure polish.
- "Hold for pickup at bar" as a third resolution.
- Attempt-count auto-escalation (e.g. force-cancel after N failures).
