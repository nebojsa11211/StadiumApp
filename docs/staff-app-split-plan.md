# Staff App Split — Implementation Plan

**Goal:** Split the single `StadiumDrinkOrdering.Staff` (Blazor Server) app into two purpose-built frontends over the **same** API + `Shared` library:

- **`StadiumDrinkOrdering.Bar`** — Blazor **Server**, big fixed screen at the bar/prep station (the "sled"). Monitors incoming orders, prepares them. Role: **Bartender**.
- **`StadiumDrinkOrdering.Runner`** — Blazor **WASM PWA**, phone-carried, delivers orders around the stadium. Role: **Waiter**. Must survive flaky network (offline-tolerant).

Prepaid only — runners do **not** charge customers, which keeps the Runner app small.

---

## 1. Why this shape (recap of the decision)

| Driver | Answer | Consequence |
|---|---|---|
| Network for roaming phones | Flaky / dead zones | Runner **cannot** be Blazor Server (persistent circuit dies in dead zones). Must be client-side WASM + offline outbox. |
| Charge on delivery | No, prepaid only | Runner app is tiny: "my deliveries → tap Delivered". No terminal/cash/PCI. |
| Team capacity | Can run two apps | Clean two-app split is worth doing now. |

The bar screen is wired and stationary, so Blazor Server's persistent SignalR circuit is an **asset** there (instant live order flow, zero install).

---

## 2. What already exists (leverage, don't rebuild)

The current Staff app already contains both roles as separate pages — the seam is real:

- `Pages/OrderQueue.razor` → **Bar/prep** logic (`Accept → StartPreparation → MarkReady → StartDelivery`, live SignalR grid). This is the Bar app's core.
- `Pages/MyOrders.razor` → **Runner** logic (my assigned orders, section-grouped route, `Mark Delivered`, batch delivery). This is the Runner app's core.
- `Pages/Dashboard.razor` → overview; belongs to Bar.

**Roles already model the split** (`Shared/Models/User.cs`):
```
Customer = 1, Admin = 2, Bartender = 3, Waiter = 4
```
→ Bar app gates to **Bartender**, Runner app gates to **Waiter**.

**The hub already supports the split** (`API/Hubs/BartenderHub.cs`):
- `OnConnectedAsync` auto-joins each connection to `staff-all` **and** `staff-{role}` from the JWT role claim.
- `SendNewOrder` already pushes to `staff-waiter` (runners) and section groups.
- Group targeting exists: `event-{id}`, `section-{X}`, `staff-{role}`, `Clients.User(staffId)`.
- ⚠️ The current client (`Services/SignalRService.cs`) calls `JoinStaffHub`/`LeaveStaffHub`, which **do not exist** on the hub (it has `JoinStaffRole`/`JoinSection`/`JoinEvent`). Those invokes silently fail today; role-join already happens automatically on connect. Clean this up during the split.

**Auth ports cleanly to WASM:** token storage (`StaffTokenStorageService`) is `localStorage` via JS interop, and the shared `AuthenticationHandler` is an `HttpClient` message handler — both work in WASM.

---

## 3. Target architecture

```
                    ┌─────────────────────────────┐
                    │   StadiumDrinkOrdering.API   │  (unchanged core)
                    │   REST + BartenderHub (WS)   │
                    └──────────────┬──────────────┘
                          ┌────────┴────────┐
             (Server-rendered)          (browser HTTP + WS, CORS)
                          │                 │
        ┌─────────────────▼──────┐   ┌──────▼───────────────────────┐
        │ StadiumDrinkOrdering.Bar│   │ StadiumDrinkOrdering.Runner   │
        │ Blazor SERVER           │   │ Blazor WASM PWA               │
        │ big screen, Bartender   │   │ phone, Waiter, offline outbox │
        └─────────────────────────┘   └──────────────────────────────┘
                          \                 /
                     ┌─────▼─────────────────▼─────┐
                     │ StadiumDrinkOrdering.Shared  │  (DTOs, Models, Auth) — shared as-is
                     └──────────────────────────────┘
```

Proposed ports (extend the existing scheme):

| App | Local HTTPS | Docker HTTPS |
|---|---|---|
| Bar | 7040 (reuse Staff slot) | 9040 |
| Runner | 7060 (new) | 9060 |

> ⚠️ Runner uses **7060**, not 7050 — 7050 is already taken by the **TicketingSimulator**. They were originally both on 7050, which made the Runner fail to start (`address already in use`) whenever the Simulator was running.

---

## 4. Backend changes (small, additive)

The API stays the source of truth. Only these changes are needed, mostly for the WASM Runner:

1. **CORS for the Runner origin.** WASM makes cross-origin browser calls. Add the Runner's origin(s) (`https://localhost:7060`, `https://localhost:9060`) to the CORS policy with `AllowCredentials` + `WithHeaders(Authorization)`. (Bar is server-rendered and needs no CORS.)

2. **Browser-trusted TLS for the Runner path.** The Server apps bypass cert validation with `ServerCertificateCustomValidationCallback => true`. **WASM cannot bypass TLS** — the phone browser enforces it. The API endpoint the Runner calls must present a cert the phone trusts (proper cert / trusted CA, or the dev cert installed on the test device). Track as a deployment prerequisite.

3. **`GET /api/orders/mine`** (or reuse `orders/staff/{id}` but derive id server-side). Today `MyOrders.razor` hardcodes `currentStaffId = 1`. The Runner must scope to the logged-in Waiter from the **JWT user-id claim**, not a hardcoded/route id. Prefer a token-derived endpoint so a runner can't fetch another runner's queue.

4. **Idempotent status update + client action id (enables offline safety).** Add an optional `clientActionId` (GUID) to `UpdateOrderStatusDto` / the status endpoints. Server dedupes replays and treats "set Delivered when already Delivered" as success (idempotent), so the offline outbox can retry safely without double-effects. This is the one backend change that the offline design depends on.

5. **Token lifetime for a roaming runner.** Access tokens are 15 min (per CLAUDE.md) and refresh is currently a no-op (`RefreshTokenAsync` just re-validates). A runner offline in a dead zone can't refresh and will get bounced to login mid-shift. Decide one of: (a) implement real refresh tokens end-to-end, or (b) issue a longer-lived token for the Waiter role. Recommend (a) long-term, (b) as a fast unblock.

No changes needed to: order workflow, hub group logic, DTOs/models, the prep→delivery state machine.

---

## 5. App 1 — `StadiumDrinkOrdering.Bar` (Blazor Server)

Lowest risk; mostly a rename + focus of the existing Staff app.

**Steps**
1. Scaffold `StadiumDrinkOrdering.Bar` as a Blazor Server project (copy Staff's `Program.cs` wiring: shared auth, `AddClientAuthentication(apiBaseUrl, "Bar", ...)`, centralized logging source `"Bar"`, localization en/hr, dev-cert bypass handler, hub client).
2. Move in `Dashboard.razor`, `OrderQueue.razor`, `StadiumMap.razor`, `Logs.razor`, dashboard components, `SignalRService`, `StaffApiService` (rename to `BarApiService` or keep interface), auth services, `LanguageSwitcher`, layouts.
3. **Gate to Bartender + Admin** (route guard / `AuthRoute`), shared-station login (one login per screen, long session).
4. Promote `OrderQueue` into a **KDS mode**: large cards, high-contrast, auto-refresh already present; add new-order **sound + flash** and column-per-status (Pending / Accepted / In-Prep / Ready) for glanceability across the room.
5. Fix the SignalR client: drop `JoinStaffHub`/`LeaveStaffHub` (nonexistent server methods); rely on automatic `staff-bartender` join. Optionally `JoinEvent(eventId)` for the active event.
6. Ports 7040/9040; Dockerfile + docker-compose service mirroring the current Staff entry.

**Checkpoint:** Bar shows live order flow on a desktop browser, new orders pop with sound, prep buttons drive the state machine. Ship this first — it's immediately useful.

---

## 6. App 2 — `StadiumDrinkOrdering.Runner` (Blazor WASM PWA)

The real engineering. Keep it deliberately minimal: **login → my deliveries → route → Delivered**, resilient to network loss.

### 6.1 Project setup
1. Scaffold `StadiumDrinkOrdering.Runner` as **Blazor WebAssembly** with the **PWA** option (`--pwa`): gives `manifest.json` + `service-worker.js` (installable, app-shell cached).
2. Reference `StadiumDrinkOrdering.Shared`.
3. Config comes from `wwwroot/appsettings.json` fetched at runtime (not server config). Put `ApiSettings:BaseUrl` there.
4. Mobile-first layout: portrait, large touch targets, one-handed reach, bottom action bar.

### 6.2 Auth in WASM (differences from Server)
- Reuse `Shared` token storage pattern (`localStorage` via JS interop works in WASM) and `AuthenticationHandler` message handler on the API `HttpClient`.
- **No prerendering** — the prerender try/catch guards become no-ops (harmless).
- **`BackgroundTokenRefreshService` is an `IHostedService`** — the WASM host does not run hosted services the way the Server host does. Replace with a client-side timer/service started at app init (or a root-component `OnInitializedAsync`). Tie to the §4.5 token-lifetime decision.
- **No cert bypass** (see §4.2).
- Per-runner login; the token's user-id claim scopes `orders/mine`.

### 6.3 Pages (port from `MyOrders.razor`, simplified)
- **My Deliveries** — orders assigned to me in `Ready` / `OutForDelivery`, section-grouped route (logic already in `GetOptimizedRoute()`).
- **Order detail** — items, seat, big **Delivered** button (and optional **Picked up** → `OutForDelivery`).
- **Connection/sync status** — online/offline indicator + pending-sync count.

### 6.4 Live updates
- SignalR from WASM to `bartenderHub` works; runner auto-joins `staff-waiter` on connect and already receives `NewOrder`.
- Treat pushes as **best-effort**: on reconnect, always **pull** `orders/mine` to reconcile (don't trust that every push arrived while offline).

---

## 7. The offline outbox (heart of the Runner)

The flaky-network answer makes this the single most important component. A runner must be able to complete a delivery while offline and have it sync later.

**Design**
- **Store:** IndexedDB via a tiny JS interop module (durable across reloads/app kill), with a JSON-serialized fallback. Keep it small — the only runner mutations are status changes.
- **Queued action record:**
  ```
  { clientActionId: GUID, orderId, targetStatus, occurredAtUtc, retryCount }
  ```
- **Optimistic UI:** tapping **Delivered** updates the local list immediately and shows a "pending sync" badge; the action is enqueued.
- **Flush triggers:** immediately after enqueue if online; on `online` event (JS `navigator.onLine` + `window.addEventListener('online')` bridged to .NET); on app resume/visibility; and on a periodic timer (e.g. 15–30 s) as a backstop.
- **Idempotent send:** POST the status change with `clientActionId`. Server dedupes replays and treats already-in-target-state as success (§4.4). On success, remove from outbox and clear the badge. On transient failure, increment `retryCount` and keep queued (capped backoff).
- **Reconcile on reconnect:** after flushing, pull `orders/mine` so the UI matches server truth (handles actions that actually landed before the response was lost).

**Why idempotency matters:** without it, a "delivered" request whose response is lost gets retried and could double-apply or error. `clientActionId` + idempotent server = safe retries, which is the whole point of the outbox.

**Test it early against simulated loss** (Chrome DevTools offline throttling, or toggle the SW). This is where bugs hide; build it in week one of the Runner, not last.

---

## 8. Cross-cutting

- **Config:** Bar uses server config (`AppConfiguration.ResolveApiBaseUrl`); Runner uses `wwwroot/appsettings.json`.
- **HTTPS/cert:** Bar keeps dev-cert bypass; Runner needs a browser-trusted cert on the API (§4.2).
- **CORS:** required only for Runner (§4.1).
- **Localization:** both keep en/hr (`IStringLocalizer<SharedResources>`, culture cookie). WASM localization uses satellite assemblies + `Blazor.start` culture — slightly different setup than Server; budget a small task.
- **Centralized logging:** register with distinct sources `"Bar"` and `"Runner"`.
- **Docker:** two services + Dockerfiles; Runner is static WASM (can be served by its own ASP.NET host or any static host) — follow existing HTTPS/compose conventions and `TZ=Europe/Zagreb`.
- **Solution:** add both projects to `StadiumDrinkOrdering.sln`; retire `StadiumDrinkOrdering.Staff` once both are live (or repurpose it as Bar).

---

## 9. Sequencing (phased, each phase shippable)

**Phase 0 — Backend prep (small)**
- CORS for Runner origin; `orders/mine` (token-scoped); `clientActionId` + idempotent status update; token-lifetime decision. Verify with existing apps still green.

**Phase 1 — Bar app (low risk, high value)**
- Scaffold + move bar/prep pages, gate to Bartender, KDS polish (sound/flash/columns), fix SignalR joins, ports 7040/9040, Docker. **Ship.**

**Phase 2 — Runner shell (WASM PWA)**
- Scaffold WASM PWA, reuse Shared, WASM auth wiring, mobile layout, My Deliveries + Delivered (online-only first), gate to Waiter, live pull/reconcile. Ports 7060/9060.

**Phase 3 — Offline outbox (the core)**
- IndexedDB store, optimistic UI + pending badge, flush triggers, idempotent send, reconcile. **Test against simulated network loss.**

**Phase 4 — Cutover & retire**
- Point runners/bartenders at the new apps; retire/repurpose old Staff project; docs + CLAUDE.md update.

---

## 10. Risks & gotchas (pre-identified from the code)

- **WASM TLS**: self-signed dev cert won't be trusted by a phone browser — the Runner won't reach the API until the cert is trusted. Plan this before device testing.
- **Token expiry offline**: 15-min access token + no-op refresh will log runners out mid-shift/offline. Resolve in §4.5 before Phase 3.
- **Hardcoded `currentStaffId = 1`** in `MyOrders.razor`: must become the JWT user id, or runners see the wrong queue. Fixed via `orders/mine`.
- **Hub `JoinStaffHub` is a no-op** (method doesn't exist server-side): remove; rely on auto role-join.
- **Hosted-service refresh** doesn't run in WASM: replace with client-side timer.
- **Double-apply on retry** without idempotency: mitigated by `clientActionId` (§4.4).

---

## 11. Testing strategy

- **Playwright multi-context** (per CLAUDE.md): Bartender context preps an order → Waiter context sees it in My Deliveries → marks Delivered → verify server state + Bar screen update.
- **Offline simulation** for the outbox: go offline, mark delivered, confirm optimistic UI + queued badge; go online, confirm single idempotent sync and reconciled state.
- **PWA install** smoke test on a real phone (installable, launches, app-shell loads offline).
- Existing `dotnet test` + `npx playwright test` after each phase (mandatory testing protocol).

---

## 12. Open items / decisions

1. **Runner token lifetime — RESOLVED:** moderate ~4h Waiter token + re-login (refresh can't happen offline anyway). Implemented as role-based expiry in `AuthService` (Waiter 4h, others 24h; tunable via `JwtSettings:WaiterAccessTokenHours` / `JwtSettings:AccessTokenHours`).
2. **Cert/hosting for the Runner path — STILL OPEN:** WASM needs a browser-trusted cert for the API on the runner's device (§4.2). Must be sorted before on-device testing in Phase 2/3.
3. **How runners get orders — RESOLVED:** shared pool + self-claim. `GET /orders/available-for-delivery` (Ready + unassigned) lists the pool; `POST /orders/{id}/claim` atomically assigns to the caller and moves to OutForDelivery (first claimer wins → 409; re-claim by holder is idempotent). No section scoping for MVP.
4. **Bar project origin — RESOLVED (implicitly):** copied Staff → new `StadiumDrinkOrdering.Bar`, old Staff left intact for now, retire later.

### Phase 2 backend enablers (done)
- Role-based token expiry (decision 1).
- `GET /orders/available-for-delivery` + `POST /orders/{id}/claim` with `ClaimOutcome` (decision 3).
- `GET /orders/mine`, idempotent status updates, `ClientActionId`, Runner CORS (from Phase 0).
Remaining for Phase 2: scaffold the Runner WASM PWA (auth in WASM, pool + claim + my-deliveries + Delivered), then Phase 3 offline outbox.
