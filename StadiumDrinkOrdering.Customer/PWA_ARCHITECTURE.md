# PWA Architecture Diagram
## Stadium Drink Ordering - Technical Flow

```
┌──────────────────────────────────────────────────────────────────────┐
│                        USER'S DEVICE                                  │
│                                                                       │
│  ┌────────────────────────────────────────────────────────────────┐ │
│  │                     BROWSER (Chrome/Safari/Edge)                │ │
│  │                                                                 │ │
│  │  ┌──────────────────────────────────────────────────────────┐ │ │
│  │  │          BLAZOR SERVER APPLICATION                       │ │ │
│  │  │                                                          │ │ │
│  │  │  ┌────────────┐  ┌────────────┐  ┌─────────────────┐  │ │ │
│  │  │  │  _Layout   │  │ MainLayout │  │  InstallPrompt  │  │ │ │
│  │  │  │  .cshtml   │  │  .razor    │  │     .razor      │  │ │ │
│  │  │  └──────┬─────┘  └─────┬──────┘  └────────┬────────┘  │ │ │
│  │  │         │                │                  │           │ │ │
│  │  │         │ manifest.json  │ Component        │ Trigger   │ │ │
│  │  │         │ meta tags      │ rendering        │ install   │ │ │
│  │  │         └────────┬───────┴──────────────────┴───────────┤ │ │
│  │  │                  │                                       │ │ │
│  │  └──────────────────┼───────────────────────────────────────┘ │ │
│  │                     │                                         │ │
│  │  ┌──────────────────▼──────────────────────────────────────┐ │ │
│  │  │           JAVASCRIPT LAYER                              │ │ │
│  │  │                                                          │ │ │
│  │  │  ┌───────────────┐  ┌──────────────────────────────┐  │ │ │
│  │  │  │ pwa-install.js│  │   Browser APIs               │  │ │ │
│  │  │  │               │  │                              │  │ │ │
│  │  │  │ • Detect      │──┤ • beforeinstallprompt       │  │ │ │
│  │  │  │ • Install     │  │ • navigator.serviceWorker   │  │ │ │
│  │  │  │ • Track       │  │ • window.matchMedia()       │  │ │ │
│  │  │  │ • Persist     │  │ • localStorage              │  │ │ │
│  │  │  └───────┬───────┘  └──────────────────────────────┘  │ │ │
│  │  │          │                                             │ │ │
│  │  └──────────┼─────────────────────────────────────────────┘ │ │
│  │             │                                               │ │
│  │  ┌──────────▼──────────────────────────────────────────────┐ │ │
│  │  │        SERVICE WORKER (service-worker.js)               │ │ │
│  │  │                                                          │ │ │
│  │  │  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐ │ │ │
│  │  │  │   Install    │  │   Activate   │  │    Fetch     │ │ │ │
│  │  │  │   Event      │  │    Event     │  │    Event     │ │ │ │
│  │  │  │              │  │              │  │              │ │ │ │
│  │  │  │ Precache     │  │ Clean old    │  │ Intercept    │ │ │ │
│  │  │  │ static       │  │ caches       │  │ requests     │ │ │ │
│  │  │  │ assets       │  │              │  │              │ │ │ │
│  │  │  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘ │ │ │
│  │  │         │                  │                  │         │ │ │
│  │  │         │                  │                  │         │ │ │
│  │  │  ┌──────▼──────────────────▼──────────────────▼───────┐ │ │ │
│  │  │  │            CACHE STORAGE (Browser Cache)           │ │ │ │
│  │  │  │                                                     │ │ │ │
│  │  │  │ ┌─────────────┐ ┌──────────────┐ ┌─────────────┐ │ │ │ │
│  │  │  │ │   Static    │ │   Dynamic    │ │   Images    │ │ │ │ │
│  │  │  │ │   Cache     │ │    Cache     │ │    Cache    │ │ │ │ │
│  │  │  │ │             │ │              │ │             │ │ │ │ │
│  │  │  │ │ • CSS       │ │ • HTML Pages │ │ • Icons     │ │ │ │ │
│  │  │  │ │ • JS        │ │ • Components │ │ • Images    │ │ │ │ │
│  │  │  │ │ • Fonts     │ │ • API Calls  │ │ • Logos     │ │ │ │ │
│  │  │  │ └─────────────┘ └──────────────┘ └─────────────┘ │ │ │ │
│  │  │  │                                                     │ │ │ │
│  │  │  │ ┌─────────────────────────────────────────────────┤ │ │ │
│  │  │  │ │         API Cache (Network Responses)           │ │ │ │
│  │  │  │ │                                                  │ │ │ │
│  │  │  │ │ • /api/drinks • /api/events • /api/orders       │ │ │ │
│  │  │  │ └─────────────────────────────────────────────────┘ │ │ │
│  │  │  └─────────────────────────────────────────────────────┘ │ │ │
│  │  │                                                          │ │ │
│  │  │  ┌──────────────────────────────────────────────────┐  │ │ │
│  │  │  │    Background Sync & Push (Infrastructure)       │  │ │ │
│  │  │  │                                                   │  │ │ │
│  │  │  │  • sync event listener                           │  │ │ │
│  │  │  │  • push event listener                           │  │ │ │
│  │  │  │  • notificationclick listener                    │  │ │ │
│  │  │  └──────────────────────────────────────────────────┘  │ │ │
│  │  └──────────────────────────────────────────────────────────┘ │ │
│  │                                                                 │ │
│  │  ┌──────────────────────────────────────────────────────────┐ │ │
│  │  │              OFFLINE FALLBACK (offline.html)             │ │ │
│  │  │                                                          │ │ │
│  │  │  • Branded offline page                                 │ │ │
│  │  │  • Check connection button                              │ │ │
│  │  │  • Cached pages links                                   │ │ │
│  │  │  • Auto-reconnect on online                             │ │ │
│  │  └──────────────────────────────────────────────────────────┘ │ │
│  │                                                                 │ │
│  └─────────────────────────────────────────────────────────────────┘ │
│                                                                       │
│  ┌─────────────────────────────────────────────────────────────────┐ │
│  │                LOCAL STORAGE                                     │ │
│  │                                                                  │ │
│  │  • pwa-install-dismissed (7-day expiry)                         │ │
│  │  • pwa-installations (analytics data)                           │ │
│  │  • auth tokens (JWT)                                            │ │
│  └─────────────────────────────────────────────────────────────────┘ │
│                                                                       │
└───────────────────────────────────────────────────────────────────────┘

                                   ▲
                                   │ HTTPS Only
                                   │ (Required for PWA)
                                   │
┌──────────────────────────────────┴────────────────────────────────────┐
│                         SERVER INFRASTRUCTURE                          │
│                                                                        │
│  ┌──────────────────────────────────────────────────────────────────┐ │
│  │           ASP.NET Core API (HTTPS: 7010/9010)                    │ │
│  │                                                                   │ │
│  │  • Authentication (JWT)                                          │ │
│  │  • Drinks API                                                    │ │
│  │  • Events API                                                    │ │
│  │  • Orders API                                                    │ │
│  │  • Customer Ticketing API                                        │ │
│  │  • Shopping Cart API                                             │ │
│  │  • Analytics API (PWA installs)                                  │ │
│  └──────────────────────────────────────────────────────────────────┘ │
│                                                                        │
│  ┌──────────────────────────────────────────────────────────────────┐ │
│  │           PostgreSQL/Supabase Database                           │ │
│  │                                                                   │ │
│  │  • Users • Drinks • Orders • Events                              │ │
│  │  • Tickets • ShoppingCarts • SeatReservations                    │ │
│  └──────────────────────────────────────────────────────────────────┘ │
│                                                                        │
│  ┌──────────────────────────────────────────────────────────────────┐ │
│  │           SignalR Hub (Real-time Updates)                        │ │
│  │                                                                   │ │
│  │  • Order status updates                                          │ │
│  │  • Notifications                                                 │ │
│  │  • Cart updates                                                  │ │
│  └──────────────────────────────────────────────────────────────────┘ │
│                                                                        │
└────────────────────────────────────────────────────────────────────────┘
```

---

## Request Flow Diagrams

### Installation Flow

```
User Visits Site (HTTPS Required)
         │
         ▼
Service Worker Registration
         │
         ├─── Register /service-worker.js
         │
         ▼
Service Worker Install Event
         │
         ├─── Cache static assets
         ├─── Cache offline.html
         └─── Cache manifest icons
         │
         ▼
Service Worker Activate Event
         │
         ├─── Clean old caches
         └─── Take control of pages
         │
         ▼
beforeinstallprompt Event Fires
         │
         ├─── Browser detects PWA eligibility
         │    • Valid manifest.json
         │    • Service worker registered
         │    • HTTPS enabled
         │    • Icons present
         │
         ▼
pwa-install.js Captures Event
         │
         ├─── Check localStorage for dismissal
         ├─── Check if already installed
         │
         ▼
InstallPrompt.razor Shows Banner
         │
         ├─── Display custom install UI
         ├─── "Install" button
         ├─── "Not Now" button
         │
         ▼
User Clicks "Install"
         │
         ▼
Browser Native Install Dialog
         │
         ├─── Show app name
         ├─── Show app icon
         ├─── "Install" / "Cancel"
         │
         ▼
App Installed to Home Screen
         │
         ├─── Icon added to device
         ├─── appinstalled event fires
         └─── Analytics tracked
         │
         ▼
User Launches Installed App
         │
         ├─── Opens in standalone mode
         ├─── No browser UI visible
         └─── Full-screen experience
```

### Request Caching Flow

```
User Navigates to Page
         │
         ▼
Service Worker Fetch Event
         │
         ├─── Intercept network request
         │
         ▼
Determine Resource Type
         │
         ├─── Static Asset?
         │    │
         │    ├─── YES: Cache-First Strategy
         │    │         │
         │    │         ├─── Check cache
         │    │         ├─── If found: Return cached
         │    │         └─── If not: Fetch from network → Cache → Return
         │    │
         │    └─── NO: Continue
         │
         ├─── API Request?
         │    │
         │    ├─── YES: Network-First Strategy
         │    │         │
         │    │         ├─── Try network request
         │    │         ├─── If success: Cache response → Return
         │    │         └─── If fail: Return cached (if available)
         │    │
         │    └─── NO: Continue
         │
         ├─── Navigation Request?
         │    │
         │    ├─── YES: Network-First with Offline Fallback
         │    │         │
         │    │         ├─── Try network request
         │    │         └─── If fail: Return offline.html
         │    │
         │    └─── NO: Default fetch
         │
         ▼
Response Returned to Browser
         │
         ├─── Page renders
         └─── User sees content
```

### Offline Mode Flow

```
User Loses Internet Connection
         │
         ▼
User Navigates to Page
         │
         ▼
Service Worker Fetch Event
         │
         ├─── Network request fails
         │
         ▼
Check Cache for Resource
         │
         ├─── Cached?
         │    │
         │    ├─── YES: Return cached version
         │    │         │
         │    │         └─── Page loads from cache
         │    │
         │    └─── NO: Return offline.html
         │              │
         │              ├─── Show branded offline page
         │              ├─── Display cached page links
         │              └─── "Check Connection" button
         │
         ▼
User Clicks "Check Connection"
         │
         ├─── JavaScript checks navigator.onLine
         │
         ├─── Online?
         │    │
         │    ├─── YES: Reload page
         │    │         │
         │    │         └─── Normal operation resumes
         │    │
         │    └─── NO: Show "Still offline" message
         │
         ▼
Connection Restored (window.online event)
         │
         ├─── Automatically reload page
         └─── Resume normal operation
```

---

## Component Interaction Diagram

```
┌─────────────────────────────────────────────────────────────────┐
│                    Browser Window                                │
│                                                                  │
│  ┌────────────────────────────────────────────────────────────┐ │
│  │             _Layout.cshtml (HTML Shell)                    │ │
│  │                                                             │ │
│  │  <head>                                                     │ │
│  │    • PWA meta tags                                          │ │
│  │    • <link rel="manifest" href="/manifest.json">            │ │
│  │    • Apple touch icon links                                 │ │
│  │    • Favicon links                                          │ │
│  │  </head>                                                    │ │
│  │                                                             │ │
│  │  <body>                                                     │ │
│  │    • Blazor root component                                  │ │
│  │    • <script src="js/pwa-install.js">                       │ │
│  │  </body>                                                    │ │
│  └─────────────────┬───────────────────────────────────────────┘ │
│                    │                                             │
│  ┌─────────────────▼───────────────────────────────────────────┐ │
│  │           MainLayout.razor (Blazor Layout)                  │ │
│  │                                                             │ │
│  │  • Navigation bar                                           │ │
│  │  • User menu                                                │ │
│  │  • SignalR connection                                       │ │
│  │  • <InstallPrompt /> ◄────────────────┐                    │ │
│  │  • @Body (page content)                │                    │ │
│  │  • Footer                              │                    │ │
│  └────────────────────────────────────────┼────────────────────┘ │
│                                           │                      │
│  ┌────────────────────────────────────────▼────────────────────┐ │
│  │       InstallPrompt.razor (Blazor Component)                │ │
│  │                                                             │ │
│  │  @code {                                                    │ │
│  │    private bool showInstallBanner = false;                  │ │
│  │    private DotNetObjectReference dotNetRef;                 │ │
│  │                                                             │ │
│  │    protected override OnAfterRenderAsync() {                │ │
│  │      dotNetRef = DotNetObjectReference.Create(this);        │ │
│  │      JSRuntime.InvokeVoidAsync(                             │ │
│  │        "initializePwaInstall", dotNetRef                    │ │
│  │      );                                ┌────────────────────┤ │
│  │    }                                   │                    │ │
│  │                                        │                    │ │
│  │    [JSInvokable]                       │                    │ │
│  │    public void ShowInstallPrompt() {   │                    │ │
│  │      showInstallBanner = true; ────────┤                    │ │
│  │    }                                   │                    │ │
│  │                                        │                    │ │
│  │    private async Task InstallApp() {   │                    │ │
│  │      JSRuntime.InvokeAsync<bool>(      │                    │ │
│  │        "installPwa" ────────────────────┼────┐              │ │
│  │      );                                 │    │              │ │
│  │    }                                    │    │              │ │
│  │  }                                      │    │              │ │
│  │                                         │    │              │ │
│  │  @if (showInstallBanner) {              │    │              │ │
│  │    <div class="install-prompt">         │    │              │ │
│  │      <button @onclick="InstallApp">     │    │              │ │
│  │        Install                          │    │              │ │
│  │      </button>                          │    │              │ │
│  │    </div>                               │    │              │ │
│  │  }                                      │    │              │ │
│  └─────────────────────────────────────────┼────┼──────────────┘ │
│                                            │    │                │
│  ┌────────────────────────────────────────▼────▼──────────────┐ │
│  │            pwa-install.js (JavaScript)                      │ │
│  │                                                             │ │
│  │  let deferredPrompt = null;                                │ │
│  │  let dotNetReference = null;                               │ │
│  │                                                             │ │
│  │  window.initializePwaInstall = (dotNetRef) => {            │ │
│  │    dotNetReference = dotNetRef;                            │ │
│  │                                                             │ │
│  │    window.addEventListener('beforeinstallprompt', (e) => { │ │
│  │      e.preventDefault();                                   │ │
│  │      deferredPrompt = e;                                   │ │
│  │                                                             │ │
│  │      if (!isInstallPromptDismissed()) {                    │ │
│  │        dotNetReference.invokeMethodAsync( ─────────────────┤ │
│  │          'ShowInstallPrompt'                               │ │
│  │        ); ──────────────────────┐                          │ │
│  │      }                          │                          │ │
│  │    });                          │                          │ │
│  │  };                             │                          │ │
│  │                                 │                          │ │
│  │  window.installPwa = async () => {                         │ │
│  │    if (deferredPrompt) {                                   │ │
│  │      deferredPrompt.prompt(); ◄──────────────────────────┐ │ │
│  │      const result = await deferredPrompt.userChoice;      │ │ │
│  │      return result.outcome === 'accepted';                │ │ │
│  │    }                                                       │ │ │
│  │  };                                                        │ │ │
│  └────────────────────────────────────────────────────────────┼─┘ │
│                                                               │   │
└───────────────────────────────────────────────────────────────┼───┘
                                                                │
                          Triggers ─────────────────────────────┘

                        ┌───────────────────────┐
                        │  Browser Native       │
                        │  Install Dialog       │
                        │                       │
                        │  "Add Stadium Orders  │
                        │   to Home screen?"    │
                        │                       │
                        │  [Install] [Cancel]   │
                        └───────────────────────┘
```

---

## Cache Strategy Decision Tree

```
                    Request Intercepted
                            │
                            ▼
                    ┌───────────────┐
                    │ Is Static     │
                    │ Asset?        │
                    │ (.css/.js)    │
                    └───┬───────┬───┘
                        │       │
                    YES │       │ NO
                        │       │
                        ▼       ▼
                ┌───────────┐   ┌────────────┐
                │ CACHE     │   │ Is Image?  │
                │ FIRST     │   │ (.png/.jpg)│
                │           │   └──┬─────┬───┘
                │ 1. Cache  │      │     │
                │ 2. Network│  YES │     │ NO
                │ 3. Return │      │     │
                └───────────┘      ▼     ▼
                                ┌───────────┐ ┌──────────────┐
                                │ CACHE     │ │ Is API Call? │
                                │ FIRST     │ │ (/api/*)     │
                                │           │ └──┬───────┬───┘
                                │ 1. Cache  │    │       │
                                │ 2. Network│YES │       │ NO
                                │ 3. Return │    │       │
                                └───────────┘    ▼       ▼
                                        ┌──────────────┐ ┌────────────────┐
                                        │ NETWORK      │ │ Is Navigation? │
                                        │ FIRST        │ │ (HTML page)    │
                                        │              │ └──┬───────┬─────┘
                                        │ 1. Network   │    │       │
                                        │ 2. Cache     │YES │       │ NO
                                        │ 3. Cache hit │    │       │
                                        └──────────────┘    ▼       ▼
                                                    ┌──────────────┐ ┌────────┐
                                                    │ NETWORK      │ │ DEFAULT│
                                                    │ FIRST        │ │ FETCH  │
                                                    │ + OFFLINE    │ │        │
                                                    │              │ │ Network│
                                                    │ 1. Network   │ │ only   │
                                                    │ 2. offline.  │ └────────┘
                                                    │    html      │
                                                    └──────────────┘
```

---

## File Size & Performance Budget

```
┌─────────────────────────────────────────────────────────┐
│                    Asset Sizes                          │
├─────────────────────────────────────────────────────────┤
│                                                         │
│  manifest.json               │ ~2 KB   │ ████░░░░░░░  │
│  service-worker.js           │ ~15 KB  │ ████████░░░  │
│  offline.html                │ ~5 KB   │ ██████░░░░░  │
│  pwa-install.js              │ ~8 KB   │ ███████░░░░  │
│  InstallPrompt.razor         │ ~3 KB   │ █████░░░░░░  │
│  InstallPrompt.razor.css     │ ~4 KB   │ █████░░░░░░  │
│                                                         │
│  icon-72x72.png              │ ~3 KB   │ █████░░░░░░  │
│  icon-96x96.png              │ ~4 KB   │ █████░░░░░░  │
│  icon-128x128.png            │ ~5 KB   │ ██████░░░░░  │
│  icon-144x144.png            │ ~6 KB   │ ██████░░░░░  │
│  icon-152x152.png            │ ~7 KB   │ ███████░░░░  │
│  icon-192x192.png            │ ~10 KB  │ ████████░░░  │
│  icon-384x384.png            │ ~18 KB  │ █████████░░  │
│  icon-512x512.png            │ ~25 KB  │ ██████████░  │
│                                                         │
│  ─────────────────────────────────────────────────────  │
│  TOTAL PWA OVERHEAD          │ ~115 KB │ Initial      │
│  TOTAL WITH CACHING          │ ~5 MB   │ After visits │
│                                                         │
└─────────────────────────────────────────────────────────┘

Performance Budget Targets:
┌──────────────────────────────────────────────┐
│ First Contentful Paint (FCP) │ < 1.8s │ ✅  │
│ Largest Contentful Paint     │ < 2.5s │ ✅  │
│ Time to Interactive (TTI)    │ < 3.8s │ ✅  │
│ Total Blocking Time (TBT)    │ < 200ms│ ✅  │
│ Cumulative Layout Shift      │ < 0.1  │ ✅  │
└──────────────────────────────────────────────┘
```

---

**Last Updated**: 2025-10-11
**Version**: 1.0
