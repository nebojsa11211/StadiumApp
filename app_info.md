# Stadium Drink Ordering System — Application Information

> A complete, detailed reference describing **what this solution is, how it is structured, how each project works, and how the pieces fit together at runtime.**
>
> This document is generated from the actual source code of the solution and is intended as an onboarding / architecture reference. For operational rules (cleanup, HTTPS policy, troubleshooting), see `CLAUDE.md`.

---

## 1. What the system is

The **Stadium Drink Ordering System** is a microservices-style, **.NET 8.0** platform for a sports/event stadium. It lets customers buy event tickets and order drinks to their seat, lets staff (bartenders/waiters) fulfil those orders in real time, and lets administrators manage the stadium, events, catalog, users, and analytics.

It is composed of **one backend API** and **three Blazor Server front-end web apps**, all sharing **one class library** of models, DTOs, and cross-cutting services.

| # | Project | Type | Role |
|---|---------|------|------|
| 1 | `StadiumDrinkOrdering.API` | ASP.NET Core Web API | Backend: data, business logic, auth, SignalR, payments |
| 2 | `StadiumDrinkOrdering.Admin` | Blazor Server | Admin console (auth required everywhere) |
| 3 | `StadiumDrinkOrdering.Customer` | Blazor Server | Customer storefront (browse events, buy tickets, order drinks) |
| 4 | `StadiumDrinkOrdering.Staff` | Blazor Server | Staff order-fulfilment console |
| 5 | `StadiumDrinkOrdering.Shared` | Class library | Shared models, DTOs, auth framework, logging client |

**Core principles baked into the codebase**
- 🔒 **HTTPS-only** across all services (development and Docker).
- 🔑 **JWT authentication** with short-lived access tokens + long-lived refresh tokens, automatic injection and renewal.
- 🛡️ **Role-based + policy-based authorization** (Admin, Bartender, Waiter, Customer).
- ⚡ **Real-time updates** via SignalR (order status, seat highlights, notifications).
- 🗄️ **PostgreSQL** (Supabase in cloud, or a local Postgres container offline) via EF Core.
- 🌍 **Bilingual UI** (English `en` + Croatian `hr`, default Croatian).
- 📊 **Centralized logging** of user actions, errors, and business events from every app into the API database.

---

## 2. High-level architecture

```
                     ┌──────────────────────────────────────────────┐
                     │            StadiumDrinkOrdering.Shared        │
                     │  Models · DTOs · Auth framework · Logging     │
                     │  (referenced by ALL projects below)           │
                     └──────────────────────────────────────────────┘
                                        ▲
        ┌───────────────────────────────┼───────────────────────────────┐
        │                               │                               │
┌───────────────┐             ┌───────────────┐               ┌───────────────┐
│   Admin App   │             │ Customer App  │               │   Staff App   │
│ Blazor Server │             │ Blazor Server │               │ Blazor Server │
│  :7030 / 9030 │             │  :7020 / 9020 │               │  :7040 / 9040 │
└───────┬───────┘             └───────┬───────┘               └───────┬───────┘
        │   HTTPS REST (JWT) + SignalR (BartenderHub / CustomerHub)   │
        └───────────────────────────────┼───────────────────────────────┘
                                        ▼
                            ┌───────────────────────┐
                            │   StadiumDrinkOrdering │
                            │          .API          │
                            │   ASP.NET Core Web API  │
                            │     :7010 / 9010        │
                            └───────────┬────────────┘
                                        │ EF Core (Npgsql)
                                        ▼
                            ┌───────────────────────┐
                            │  PostgreSQL / Supabase │
                            │  (or local PG :5432)   │
                            └───────────────────────┘
```

- **Browsers never talk to the database.** Each Blazor Server app renders on the server and calls the API over HTTPS REST; the API is the only thing touching the database.
- **Real-time** flows over two SignalR hubs hosted by the API. Tokens for SignalR are passed as `access_token` query parameters (the API's JWT handler reads them for `/bartenderHub` and `/customerHub`).
- **The Shared library is the contract**: every model, DTO, and the entire authentication framework live there so all four apps agree on shapes and behaviour.

---

## 3. Ports & URLs

🔒 **All endpoints are HTTPS.** Two deployment shapes exist: local `dotnet run` and Docker.

| Service | Local dev (HTTPS) | Docker external (HTTPS) | Docker internal |
|---------|-------------------|--------------------------|-----------------|
| API     | `https://localhost:7010` | `https://localhost:9010` (→9011 HTTP) | `https://api:8443` / `http://api:8080` |
| Customer| `https://localhost:7020` | `https://localhost:9020` | `https://+:8444` |
| Admin   | `https://localhost:7030` | `https://localhost:9030` | `https://+:8445` |
| Staff   | `https://localhost:7040` | `https://localhost:9040` | `https://+:8446` |
| Postgres| — | `localhost:5432` (local PG container) | `postgres:5432` |

**Configuration note (real inconsistency in the codebase):** Admin and Customer `appsettings.json` set `ApiSettings:BaseUrl` to `:9010`, while their `Program.cs` fallback and the Staff app use `:7010`. The container-detection logic (`DOTNET_RUNNING_IN_CONTAINER`) swaps to the internal `api:` URL when running in Docker. Keep this in mind when an app appears to call the "wrong" port.

SSL certificate: `./certificates/aspnetcore-multi-hostname.pfx`, mounted read-only into containers at `/https/`, password supplied via `SSL_CERT_PASSWORD`.

---

## 4. The Backend — `StadiumDrinkOrdering.API`

ASP.NET Core Web API (`Microsoft.NET.Sdk.Web`, net8.0). This is the heart of the system: all persistence, business rules, authentication, payments, and real-time messaging.

### 4.1 Key dependencies
- **EF Core + Npgsql** (`Npgsql.EntityFrameworkCore.PostgreSQL` 8.0.8) — PostgreSQL provider (Sqlite package is referenced but **not** wired up at runtime).
- **JWT**: `Microsoft.AspNetCore.Authentication.JwtBearer`, `System.IdentityModel.Tokens.Jwt`.
- **SignalR** for real-time.
- **BCrypt.Net-Next** for password hashing.
- **QRCoder** for ticket QR codes.
- **Stripe.net** for payments.
- **AspNetCoreRateLimit** for rate limiting (configured but middleware currently disabled).
- **Swashbuckle** (Swagger, Development only at `/swagger`).
- **Supabase**, **HealthChecks.NpgSql**.

### 4.2 Data layer — `ApplicationDbContext`
`Data/ApplicationDbContext.cs`. PostgreSQL-targeted: `OnModelCreating` forces all `DateTime` columns to `timestamp with time zone` (UTC), configures indexes/precision/relationships, and seeds data (admin user, 8 drinks, a sample event, 4 sections, ~398 seats, 5 test tickets).

**Entities / DbSets:** Users, Drinks, Orders, OrderItems, Payments, Tickets, StadiumSeats, Events, StadiumSections, Seats, EventStaffAssignments, EventAnalytics, OrderSessions, Notifications, Tribunes, Rings, Sectors, StadiumSeatsNew, StadiumSectorOverlays, LogEntries, ShoppingCarts, CartItems, SeatReservations, TicketSessions, FailedAttempts, AccountLockouts, IPBans, RefreshTokens.

**Migrations** (applied automatically at startup via `InitializeDatabaseAsync` → `MigrateAsync`, with retry/backoff):
1. `InitialPostgreSQLMigrationFixed`
2. `AddRateLimitingEntities`
3. `AddRefreshTokenEntity`
4. `ExpandLogEntryFieldLimits`
5. `AddStadiumSectorOverlay`
6. `AddVariableSeatingSupport`
7. `AddCustomShapeSupport`

### 4.3 Controllers (31)
Grouped by concern:

**Auth & users**
- `AuthController` — login, register, refresh-token, revoke-token, get user.
- `UsersController` — user CRUD, search, change password, online stats (Admin policy).
- `DebugAuthController` — diagnostic password/user checks.

**Catalog & orders**
- `DrinksController` — drink catalog CRUD.
- `OrdersController` — order CRUD, my-orders, status updates, cancel, statistics (auth required).
- `CustomerOrdersController` (`customer/orders`) — customer order create + confirmation.
- `OrderSessionController` — seat/QR order session lifecycle + cart (add/remove/update/clear/checkout/extend/invalidate).
- `QuickOrderController` — lightweight "create simple order" helper.

**Payments & QR**
- `PaymentController` — Stripe: create-intent, confirm, get, refund, webhook.
- `QRCodeController` — QR validate/create-session/get-ticket/regenerate.

**Tickets & ticketing store**
- `TicketsController` — ticket list, validate, today stats (auth).
- `TicketAuthController` — ticket-session auth (validate, session, logout, cleanup).
- `TicketSalesController` — sold tickets, seat status, simulate-sales (Admin/Staff).
- `CustomerTicketingController` (`customer/ticketing`) — public event list/detail, seat availability.
- `ShoppingCartController` (`customer/cart`) — cart get/add/remove/clear/summary/availability.

**Events**
- `EventController` (`events`) — event CRUD, active/upcoming/past lists, analytics, activate/deactivate.

**Stadium structure & visualization**
- `StadiumController` — stadium layout, section seats, seat-orders.
- `StadiumStructureController` — JSON/CSV import/export of tribune→ring→sector structure, generate-seats, summary (Admin).
- `StadiumViewerController` — overview, sector seats, event seat-status, search-seat.
- `StadiumSvgController` (`stadium-svg`) — SVG layout generation incl. HNK Rijeka layout (auth).
- `StadiumSectorOverlayController` — overlay CRUD for the admin drawing tool.

**Analytics**
- `AnalyticsController` — dashboard, event analytics, trends, drink popularity, revenue, staff performance (Admin/Staff).
- `CustomerAnalyticsController` — customer search/details/summary/export/top-customers/retention.

**System / admin / logging / security**
- `LogsController` — centralized logging: search, summary, log-action, log-batch, recent activity/errors, clear (auth, role-mixed).
- `SecurityController` — brute-force/rate-limit admin: stats, lift IP ban, lift lockout, config (Admin).
- `DataGridController` — generic DB explorer: list tables, table-data, export, generate-data (Admin).
- `DataImportController` — bulk ticketing data import.
- `DemoDataController` / `TestController` / `TestDataController` / `TestLoggingController` — demo/seed/diagnostic endpoints.

### 4.4 Services (business logic, `Services/`)
`AuthService`, `OrderService`, `QRCodeService`, `StripePaymentService` (`IPaymentService`), `NotificationService`, `OrderSessionService`, `DemoDataService`, `AnalyticsService`, `CustomerAnalyticsService`, `EventService`, `StadiumStructureService`, `SeatMappingService`, `LoggingService`, `ShoppingCartService`, `TicketAuthService`, `TicketingDataImportService`, `StadiumLayoutService` (`IStadiumLayoutService`, memory-cached), `HNKRijekaLayoutGenerator` (`IStadiumLayoutGenerator`), `BruteForceProtectionService`, `DatabaseHealthCheck`, `StadiumValidator`.

> **Background services exist but are currently disabled in `Program.cs`** (noted as causing connection-pool exhaustion): `LogRetentionBackgroundService`, `RateLimitCleanupService`, `TicketSessionCleanupService`.

### 4.5 Middleware (`Middleware/`)
- `SecurityHeadersMiddleware` — adds HSTS/CSP/NOSNIFF/Frame/Permissions headers, strips `Server`/`X-Powered-By`. **Currently commented out.**
- `GlobalExceptionMiddleware` — global try/catch → JSON 500 + fire-and-forget centralized logging. **Currently commented out.**

### 4.6 Startup pipeline (`Program.cs`)
Order: Swagger (dev) → `UseHttpsRedirection` → `UseCors("AllowAll")` → *(SecurityHeaders / IpRateLimiting / GlobalException — disabled)* → `UseAuthentication` → `UseAuthorization` → `MapControllers` → map hubs → map `/health`.

- **JSON:** `ReferenceHandler.IgnoreCycles`, ignore nulls, camelCase, indented, `MaxDepth = 64`.
- **JWT:** secret from `JWT_SECRET_KEY` (≥32 chars enforced), HS256 only, validates issuer/audience/lifetime/signing key, 1-min clock skew, custom lifetime validator, reads `access_token` query for SignalR hubs.
- **CORS:** `AllowAll` policy — origins from `CORS_ALLOWED_ORIGINS` env or dev defaults (7010–7040, plus Docker hostnames), AllowCredentials.
- **Health checks** at `/health` (custom DB check + Npgsql check).
- **Connection string** resolved from `ConnectionStrings__DefaultConnection`, else assembled from `DB_*` env vars, else config.
- **Centralized logging** registered for source "API".
- **Startup DB init** seeds/repairs admin (`admin@stadium.com` / `admin123`) and customer (`customer@stadium.com` / `customer123`) in Development.

### 4.7 Authorization (`Authorization/`)
Defined in `AuthorizationPolicies.ConfigurePolicies`. Roles from `UserRole`: **Admin, Bartender, Waiter, Customer**.

- **Role policies:** `RequireAdminRole`, `RequireStaffRole` (Admin/Bartender/Waiter), `RequireCustomerRole`, `RequireAuthenticatedUser`.
- **Order policies:** `CanReadOrders`, `CanCreateOrders`, `CanUpdateOrders`, `CanDeleteOrders`.
- **Ownership policies** (custom requirements + handlers): `CanAccessOwnOrders` (`OrderOwnershipHandler`), `CanAccessOwnProfile` (`UserOwnershipHandler`), `CanAccessOwnTickets` (`TicketOwnershipHandler`).
- **Admin policies:** `CanManageUsers`, `CanManageDrinks`, `CanManageEvents`, `CanViewAnalytics`, `CanManageSystem`, `CanAccessLogs`, `CanManageStadiumStructure`, `CanProcessPayments`, `CanManageQRCodes`.
- **Hub policies:** `CanAccessBartenderHub`, `CanAccessCustomerHub`.

### 4.8 SignalR hubs (`Hubs/`)
- **`BartenderHub`** → `/bartenderHub` (`[Authorize(CanAccessBartenderHub)]`). Staff/admin join event/section/role groups; broadcasts `OrderUpdated`, `NewOrder`, `OrderStatusChanged`, `StaffAssigned`, `OrderAssigned`, `DeliveryEstimateUpdated`, `NotificationReceived`, `SeatHighlight`, plus connect/disconnect presence events.
- **`CustomerHub`** → `/customerHub` (anonymous allowed; sensitive methods gated). Customers join per-order/per-event groups; emits `OrderStatusUpdated`, `OrderReady`, `NotificationReceived`, `Connected`. Ownership-checked so a customer can only join their own order group.

---

## 5. The Shared library — `StadiumDrinkOrdering.Shared`

The contract layer referenced by every other project. Targets net8.0; pulls in DI/HTTP/JWT/JSInterop abstractions.

### 5.1 Models (`Models/`) — namespace `...Shared.Models`
**Domain:** `Drink`, `User`, `Event`, `EventAnalytics`, `EventStaffAssignment`, `Order`, `OrderItem`, `OrderSession`, `Notification`, `Payment`, `Ticket`, `TicketSession`, `RefreshToken`.

**Stadium structure:** `StadiumSeat` (legacy flat), and the newer hierarchy `Tribune → Ring → Sector → StadiumSeatNew`; plus `StadiumSection`/`Seat`, and the drawing-tool `StadiumSectorOverlay` (+ `RowPattern`, `VertexCoordinate`) supporting custom shapes and variable seating.

**Shopping:** `ShoppingCart`, `CartItem`, `SeatReservation`.

**Logging:** `LogEntry` (rich — request context, exception data, and business-event fields like monetary amount, quantity, status-before/after).

**Enums:** `DrinkCategory`, `EventType`, `EventStatus`, `OrderStatus` (Pending→Accepted→InPreparation→Ready→OutForDelivery→Delivered→Cancelled), `PaymentMethod`, `PaymentStatus`, `UserRole`, `ReservationStatus`, `SectorShapeType` (Rectangle/Triangle/Rhombus/CircularSector/CustomPolygon), `CustomLogLevel`, `LogCategory`.

### 5.2 DTOs (`DTOs/`) — namespace `...Shared.DTOs`
Wire models grouped by domain: auth (`LoginDto`, `RegisterDto`, `RefreshTokenRequestDto`, `EnhancedLoginResponseDto`, …), users, drinks, orders, payments, tickets (incl. QR ticket-auth flow), ticket orders/checkout, shopping cart, customer analytics, stadium (layout/structure/import/SVG/viewer), and logging (`LogEntryDto`, `LogFilterDto`, `BusinessEventDto`, `LogUserActionRequest`, plus `BusinessEventCategories`/`BusinessEventActions` constants).

### 5.3 Authentication framework (`Authentication/`)
A complete, reusable client auth framework consumed by all three Blazor apps.

- **Interfaces:** `IAuthenticationStateService` (auth state + login/logout/refresh/role checks), `ITokenStorageService` (secure token storage + expiry), `ISecureApiService` (authenticated typed HTTP calls), `ITokenRefreshService`.
- **Middleware:** `AuthenticationHandler` (`DelegatingHandler`) — injects Bearer token, proactively refreshes near-expiry (2-min window), on 401 refreshes and retries; `TokenRefreshResult`.
- **Models:** `AuthenticationConfiguration` (per-app: `ForAdmin/ForCustomer/ForStaff`), `AuthenticationResult`, `AuthenticationState`, `TokenInfo` (JWT parsing).
- **Services:** `TokenRefreshService`, `BackgroundTokenRefreshService` (hosted, checks every minute).
- **Utilities:** `JwtTokenValidator`, `PasswordValidator` (+ `PasswordPolicy`), `SecurityUtilities` (random/PBKDF2/SHA256/TOTP/constant-time compare), custom `ValidationAttributes`.
- **Extensions:** `AddClientAuthentication(apiBaseUrl, context, enableBackgroundRefresh)` is the main entry point (registers authenticated HttpClient + handler + optional background refresh). Token storage is registered per app.
- **Constants:** `AuthenticationConstants` — contexts, per-app storage keys, token timings (access 15 min, refresh 7 days, 2-min refresh window), security limits (HS256/384/512, ≥32-char secret).

### 5.4 Shared services (`Services/`)
- **`CentralizedLoggingClient`** (`ICentralizedLoggingClient`, singleton) — fire-and-forget logging client used by every app. Batches (size 10 / 5 s), with aggressive 2–3 s timeouts so logging never blocks the user; posts to `/logs/log-action` and `/logs/log-batch`; typed business helpers (`LogTicketPurchaseAsync`, `LogOrderStatusChangeAsync`, …). Registered via `AddCentralizedLogging(apiBaseUrl, source)`.
- **`AuthorizationHelperService`** (`IAuthorizationHelperService`) — front-end UI authorization helper reading roles from Blazor's `AuthenticationStateProvider`; role/capability checks and UI helpers (role CSS class, badge text, display name).
- **`IStadiumLayoutGenerator`** — interface for generating stadium SVG layouts (HNK Rijeka / oval / rectangular / horseshoe / bowl); implemented in the API.
- **`SvgPathGenerator`** (static) — builds SVG path `d` strings for overlay shapes (rect, polygon, circular-sector arc).

### 5.5 Constants
- **`HNKRijekaStadiumConstants`** — exact HNK Rijeka stadium coordinate data (sector coordinates, field rectangle, view-box 1200×900, tribune CSS classes, default seat counts, total capacity) used to render the real stadium layout.

---

## 6. The front-ends (Blazor Server)

All three are server-side Blazor (`AddServerSideBlazor`, `MapBlazorHub`, `MapFallbackToPage("/_Host")`), reference the Shared library, call the API over HTTPS, support `en`/`hr` localization (default `hr`, cookie-persisted via `LanguageSwitcher`), and bypass SSL cert validation only in Dev/Docker. Each uses the shared auth framework plus a per-app "legacy" API client.

### 6.1 Admin — `StadiumDrinkOrdering.Admin` (`:7030` / `9030`)
The most elaborate app. Auth is **required on every route** except `/login`.

- **Extra infrastructure:** server-side **session** (token bridge, 2 h idle, Secure/HttpOnly/SameSite=Strict), `AddControllers` (has `Controllers/AuthSessionController.cs`), `AddRazorPages`. Named HttpClients: `ApiClient`, `AuthenticatedClient` (via `AddClientAuthentication(... "Admin" ...)`), `AdminSecureApi`.
- **Composite API client:** `AdminApiService` (`IAdminApiService`) exposes sub-services as properties — `Orders, Users, Drinks, Tickets, Auth, Logs, Analytics, Stadium, Events, Http` (each its own interface+impl under `Services/<Area>/`, sharing `Base/BaseApiService`).
- **Auth:** `AuthStateService` (as both `IAuthStateService` and `IAuthenticationStateService`), `HybridTokenStorageService` (server + client), `SecureApiService`.
- **Real-time:** `SignalRService` — join/leave section, order-status changes, seat highlight.
- **Error handling:** `ThrottlingService`, `ErrorNotificationService`, `ApiResponse`/`ErrorMessageMappings`; console logging toggle services; `StadiumSvgService` (typed client for dynamic stadium SVG).
- **Route guard:** `AuthRoute.razor` wraps every non-login route in `App.razor`; redirects unauthenticated users to `/login?returnUrl=…`.

**Pages / routes:** `/` & `/dashboard` (dashboard), `/login`, `/orders`, `/drinks`, `/events`, `/tickets`, `/users`, `/analytics`, `/customer-analytics`, `/bartender` (bartender screen), `/logs`, `/datagrid` (Database Explorer), and **Stadium Management**: `/admin/stadium-structure`, `/admin/stadium-overview`, `/admin/stadium-drawing-tool`, `/admin/structure-help`. Plus test/diagnostic pages. Stadium components live in `Components/Stadium/` (`StadiumSvgRenderer`, `SectorDetailModal`, `SectorEditModal`, …).

### 6.2 Customer — `StadiumDrinkOrdering.Customer` (`:7020` / `9020`)
The public storefront. A PWA (install prompt, theme switcher, offline docs). Guarding is **per-page**, not global.

- **Services:** `ApiService` (`IApiService`, large monolithic client — also defines many customer DTOs inline), `CartService` (`ICartService`, in-memory cart), `CustomerAuthStateService`, `CustomerTokenStorageService`, `CustomerSecureApiService` (named client `CustomerSecureApi`). Registered via `AddSharedAuthentication(apiBaseUrl, "Customer")` + `AddCentralizedLogging(... "Customer")`.
- **Two route guards:** `CustomerAuthRoute` (shows a "Please sign in / create account" card for protected features like purchasing) and `TicketProtectedRoute` (validates a **ticket session token** from param/`sessionStorage` via `ApiService.GetTicketSessionAsync`; redirects to `/scan` on expiry). `App.razor` wraps routes in `AuthenticationInitializer` (state init), not a hard global guard.

**Pages — two locations:**
- `Pages/` (ticketing store): `/` (home), `/login`, `/register`, `/events`, `/events/{EventId}` (event detail + seat selection), `/menu` (drinks), `/checkout`, `/orders`, `/order-confirmation/{orderId}`, `/stadium-map`, `/logs`.
- `Components/Pages/` (QR ticket-session ordering): `/scan` & `/scan/{qrToken}` (QR scanner), `/order/{qrToken}` (order drinks for a scanned ticket), `/checkout/{sessionToken}` (ticket-session checkout), `/stadium-viewer`.

> The customer journey for **buying tickets**: Browse Events → Event Details → select seats (15-min seat reservation) → Cart → Checkout (customer info + payment) → Order Confirmation (tickets with QR codes). The **drinks-to-seat journey**: scan seat QR → order drinks → checkout → real-time status via SignalR.

### 6.3 Staff — `StadiumDrinkOrdering.Staff` (`:7040` / `9040`)
The fulfilment console. Auth required on every route (separate `AuthLayout` for the login page). The only front-end that directly references the JWT NuGet packages.

- **Services:** `StaffApiService` (`IStaffApiService`) — the order workflow client: `GetActiveOrders/GetOrderQueue/GetAssignedOrders`, `AssignOrder`, and the status transitions `AcceptOrder → StartPreparation → MarkReady → StartDelivery → ConfirmDelivery`; plus stadium layout/seats and login. `StaffSecureApiService`, `AuthStateService`, `StaffTokenStorageService`. `SignalRService` (join staff hub, order-status changes, order-assigned). `DashboardService` (`IDashboardService`, memory-cached aggregation). Registered via the generic `AddSharedAuthentication<AuthStateService, StaffTokenStorageService, StaffSecureApiService>("Staff", apiBaseUrl)`.
- **Localization:** the only app with a real `Resources/` directory (`ResourcesPath = "Resources"`).
- **Route guard:** `AuthRoute.razor` wraps all non-login routes; shows an "Authentication Required" card when unauthenticated.

**Pages:** `/` (dashboard with widgets in `Components/Dashboard/`: `DashboardCard`, `ConnectionStatusCard`, `QuickActionsPanel`, `RecentOrdersList`), `/login`, `/order-queue` (live queue), `/my-orders` (assigned to logged-in staff), `/stadium-map`, `/logs`, plus a newer `Components/Pages/OrderQueue.razor` at `/orders` & `/orders/queue`.

---

## 7. Key cross-cutting flows

### 7.1 Authentication & token lifecycle
1. User posts credentials to `AuthController` → API issues a **JWT access token (15 min)** + **refresh token (7 days)**.
2. The Blazor app stores both via its `ITokenStorageService`.
3. Every outgoing API call passes through the shared `AuthenticationHandler`, which injects `Authorization: Bearer …`, and **proactively refreshes** when within 2 minutes of expiry.
4. On a `401`, the handler calls `/auth/refresh-token`, stores the new pair, and retries the original request once.
5. `BackgroundTokenRefreshService` independently renews tokens on a 1-minute cadence; on refresh failure it clears tokens and the route guard redirects to login.

### 7.2 Order workflow (states)
`Pending → Accepted → In Preparation → Ready → Out for Delivery → Delivered` (or `Cancelled`). Staff drive transitions through `StaffApiService`; each change is broadcast over `BartenderHub`/`CustomerHub` so staff queues and customer screens update live.

### 7.3 Centralized logging
Every app holds a singleton `CentralizedLoggingClient` that batches user actions / errors / business events and POSTs them to the API's `LogsController`, which persists them to the `LogEntries` table. Admins browse and filter them at `/logs` (and analytics summaries via the API).

### 7.4 Stadium structure & visualization
- Admins import a stadium as JSON: `Stadium → Tribunes (N/S/E/W) → Rings (1–5) → Sectors → auto-generated Seats`. Seat codes follow `[Tribune][Ring][Sector]-R[Row]S[Seat]`.
- The **drawing tool** stores `StadiumSectorOverlay` shapes (rectangle/triangle/rhombus/circular-sector/custom-polygon, with variable seating) rendered as SVG via `SvgPathGenerator` and `IStadiumLayoutGenerator` (HNK Rijeka layout uses the exact coordinates in `HNKRijekaStadiumConstants`).
- The **Stadium Overview** colour-codes sectors by occupancy for a selected event (green <50%, orange 50–89%, red ≥90%).

---

## 8. Persistence & configuration

- **Database:** PostgreSQL via EF Core (Npgsql). Cloud = Supabase; offline = local Postgres container (`docker-compose` `postgres` service, db `stadiumdb`, port 5432). See memory note: the SQLite fallback does **not** work with this EF model — use a local PG container offline.
- **Connection string:** `ConnectionStrings__DefaultConnection` (or assembled from `DB_*` env vars).
- **JWT:** `JwtSettings__SecretKey` (≥32 chars), `JwtSettings__Issuer`, `JwtSettings__Audience`.
- **Front-end → API:** `ApiSettings__BaseUrl`.
- **Timezone:** all containers set `TZ=Europe/Zagreb`.
- **Default credentials (Development seed):** admin `admin@stadium.com` / `admin123`, customer `customer@stadium.com` / `customer123`.

---

## 9. Running the system

### Local development (recommended for iteration — instant hot reload)
```bash
# API
cd StadiumDrinkOrdering.API && dotnet run --launch-profile https      # https://localhost:7010
# Admin
cd StadiumDrinkOrdering.Admin && dotnet run --launch-profile https    # https://localhost:7030
# Customer
cd StadiumDrinkOrdering.Customer && dotnet run --launch-profile https # https://localhost:7020
# Staff
cd StadiumDrinkOrdering.Staff && dotnet run --launch-profile https    # https://localhost:7040
```
> ⚠️ Per `CLAUDE.md`: **kill all `dotnet` processes before and after any change** (`taskkill //F //IM dotnet.exe` or `cleanup-for-vs-debug.ps1`) to avoid port/SSL/connection-pool issues.

### Docker (production-like)
```bash
.\generate-dev-certs.ps1            # one-time: SSL certs
docker-compose up --build -d        # API:9010, Customer:9020, Admin:9030, Staff:9040
docker-compose logs -f [service]
docker-compose down
```

### Database
```bash
dotnet ef migrations add <Name> -p StadiumDrinkOrdering.API
dotnet ef database update -p StadiumDrinkOrdering.API
```
> Migrations also apply automatically at API startup.

### Tests
```bash
dotnet test                 # unit/integration
npx playwright test         # UI end-to-end (use separate BrowserContexts per user/role)
```

---

## 10. Notable quirks & gotchas (from the actual code)

1. **Disabled-by-default infrastructure in the API:** `SecurityHeadersMiddleware`, `GlobalExceptionMiddleware`, `UseIpRateLimiting`, and the three background services are all present but **commented out** in `Program.cs` (the background ones caused connection-pool exhaustion). Re-enable deliberately.
2. **API base-URL inconsistency:** Admin/Customer `appsettings` use `:9010`; their `Program.cs` fallback and Staff use `:7010`. Container detection overrides both with internal `api:` URLs.
3. **Duplicate page files:** Customer and Staff each have a `Checkout`/`OrderQueue` in **both** `Pages/` and `Components/Pages/` — they serve **different routes** (the `Components/Pages` versions are the newer QR/ticket-session and `/orders/queue` flows). Don't assume they're duplicates to delete.
4. **Two stadium models coexist:** the legacy flat `StadiumSeat`/`StadiumSection`/`Seat` and the newer `Tribune→Ring→Sector→StadiumSeatNew` hierarchy plus `StadiumSectorOverlay` for the visual editor.
5. **SQLite is referenced but not used** — the model is PostgreSQL-only (UTC `timestamp with time zone` everywhere).
6. **HTTPS is mandatory** project-wide; cert validation is only bypassed in Dev/Docker client HttpClients.
```
