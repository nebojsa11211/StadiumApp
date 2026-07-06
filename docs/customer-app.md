# Customer App — What It Should Do

**Project:** `StadiumDrinkOrdering.Customer` · Blazor Server (mobile-first PWA) · **HTTPS only**
**Local:** https://localhost:7020 · **Docker:** https://localhost:9020

---

## 1. Purpose in one sentence

The Customer app is the **fan-facing phone app** for a stadium. A spectator scans the QR code on
their ticket, gets bound to their exact seat, orders drinks from that seat during the match, pays,
and watches the order come to them — **without ever leaving their seat or standing in a queue.**

Its brand/product name is **HALFTIME** ("NEMA REDOVA. SAMO PIĆE." — *No queues. Just drinks.*).
The primary language is **Croatian (hr)**, with English (en) available via the language switcher.

---

## 2. Who uses it

| Fan type | Entry point | What they get |
|----------|-------------|---------------|
| **Single-match ticket holder** | Scan QR → no login needed | Seat-bound ordering for that match only |
| **Season-pass member** | Log in | A "My Season" home, saved seat, wallet, order history |
| **Anonymous browser** | Lands on `/welcome` | Sees the next fixture, can scan or log in |

The defining principle: **ordering is anchored to a validated ticket/seat, not to an account.**
A guest never has to register to buy a drink — scanning the ticket is the identity.

---

## 3. The two core journeys

### A. Scan-to-order (the main flow — no account required)

```
/welcome  →  /scan  →  /t/{token}  →  /order  →  (pay)  →  /track/{orderId}
 landing     camera    validate &      browse &   wallet    live status
             or code   bind seat       add drinks  or card    to seat
```

1. **`/welcome`** — Anonymous landing. Shows the current season + next fixture. Big CTA: *"Skeniraj ulaznicu"* (Scan ticket).
2. **`/scan`** — Camera reticle to scan the ticket QR, or a manual code entry fallback. Routes to `/t/{code}`.
3. **`/t/{token}` (`Resolve.razor`)** — Validates the ticket against the API, establishes an **order session**
   bound to the event + seat, persists the session token to `sessionStorage` (so a hard reload survives),
   and auto-forwards into ordering.
4. **`/order`** — The drink menu for that seat. Category tabs, add to cart. Guards:
   - No session → prompt to scan.
   - Session exists but match not live → *"Ordering only open while the match is on."*
5. **Pay** — Wallet (season members) or card. Insufficient wallet funds prompts an inline top-up.
6. **`/track/{orderId}`** — Live order timeline (Confirmed → In preparation → Ready → Out for delivery → Delivered),
   ETA in minutes, seat text, item list. Polls the API every ~8s until a terminal state.

### B. Season member (authenticated)

```
/  →  /home  →  /order (via saved seat)  →  /wallet, /orders
```

- **`/`** — Splash that routes: authenticated → `/home`, otherwise → `/welcome`.
- **`/home`** — "My Season": greeting, season pass, saved seat, quick actions. No pass on the account → falls back to scan.
- **`/wallet`** — Stored-value wallet: balance, top-up, transaction history. Used to pay for orders in one tap.
- **`/orders`** — Order history.

---

## 4. What each screen is responsible for

| Route | Layout | Responsibility |
|-------|--------|----------------|
| `/` | MobileShell | Splash + auth-based redirect |
| `/welcome` | MobileShell | Anonymous landing, next fixture, scan/login CTAs |
| `/scan` | MobileShell | QR scan + manual code entry |
| `/t/{token}` | MobileShell | Validate ticket, bind seat, open order session |
| `/order` | MobileShell | Seat-bound drink menu + cart + place order |
| `/track/{orderId}` | MobileShell | Live order-status tracking to the seat |
| `/home` | MobileShell | Season member home (pass, seat, quick actions) |
| `/wallet` | (guarded) | Wallet balance, top-up, history |
| `/orders` | | Order history |
| `/menu` | | Legacy ticket-number drink menu (older desktop-style flow) |
| `/events`, `/event-details/{id}`, `/cart`, `/checkout`, `/order-confirmation/{id}` | | Legacy event-browsing/seat-purchase flow |
| `/login`, `/register` | | Season-member auth |

> **Note — two generations of UI:** The current product direction is the **mobile-first HALFTIME
> flow** (`MobileShell` layout: `/welcome`, `/scan`, `/t/{token}`, `/order`, `/track`, `/home`).
> The older Bootstrap desktop pages (`/events`, `/event-details`, `/cart`, `/checkout`, `/menu`)
> predate it. New work should target the HALFTIME flow; the legacy pages are candidates for
> retirement or consolidation.

---

## 5. Key behaviours the app must guarantee

- **Seat-bound ordering.** Every drink order carries the validated ticket session, so the runner
  knows exactly which seat to deliver to. Ordering without a valid session is not allowed.
- **No-login guest path.** A single-match fan completes an entire order with only their ticket QR.
- **Session survives reload.** The order-session token is stored in `sessionStorage`; `/order` and
  `/track` recover it after a hard refresh.
- **Live-match gating.** Drink ordering is only open while the match is on (`CanOrderDrinks`).
- **Real-time-ish tracking.** `/track` polls order status until Delivered/Cancelled (SignalR is the
  planned upgrade from polling).
- **Wallet payment.** Season members can pay from a stored-value balance in one tap, with inline
  top-up when funds are short. See the fan-wallet feature.
- **Mobile-first, installable.** Chrome-less `MobileShell` (each screen owns its header), centred at
  phone width on desktop; PWA install prompt supported.
- **Bilingual.** Croatian default, English available; culture persists via cookie.
- **HTTPS everywhere.** No HTTP endpoints anywhere in the flow.

---

## 6. How it talks to the backend

- All data goes through **`IApiService`** to the API (`ApiSettings:BaseUrl`) — events, tickets,
  ticket-session validation, drinks, orders, wallet.
- **`OrderSessionState`** holds the active seat-bound session (event, seat, capabilities, token).
- **`ICartService`** holds the in-progress drink cart.
- **`ICustomerAuthStateService`** holds season-member auth state (JWT via the shared auth library).
- Order status updates come from the API today by polling; SignalR (`/bartenderHub`) is the intended
  push channel.

---

## 7. Success criteria (what "working" means)

A fan should be able to:

1. Sit down, open the app, scan the ticket, and reach the drink menu in **seconds**, no typing.
2. Add drinks, pay (wallet or card), and get a confirmation **without leaving their seat**.
3. Watch the order progress to their seat with a clear ETA.
4. (Season members) log in once and reuse their seat, wallet, and history across matches.

If any of those requires standing up, queueing, or creating an account for a single match, the app
is not meeting its purpose.
