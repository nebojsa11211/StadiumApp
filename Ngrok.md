# Running Stadium through a free ngrok tunnel

How the six Stadium applications are put on the public internet from a developer workstation with
a **free** ngrok account, what each moving part does, why it is built the way it is, and what goes
wrong when one of the parts is missing.

For reaching the apps from the *local network* only (a phone on the same Wi-Fi) the normal `https`
profiles already bind the LAN address — see `CLAUDE.md`. This document is about the `ngrok`
profiles, which reach the whole internet.

---

## 1. What problem this solves

All six apps run on `localhost` during development:

| App | What it is | Port |
|---|---|---|
| `StadiumDrinkOrdering.API` | REST API + SignalR hubs | 7010 |
| `StadiumDrinkOrdering.Customer` | Blazor Server customer app | 7020 |
| `StadiumDrinkOrdering.Admin` | Blazor Server admin app | 7030 |
| `StadiumDrinkOrdering.Bar` | Blazor Server bartender app | 7040 |
| `StadiumDrinkOrdering.TicketingSimulator` | Blazor Server external-ticketing stand-in | 7050 |
| `StadiumDrinkOrdering.Runner` | Blazor **WebAssembly** PWA for waiters | 7060 |

To show the product to somebody elsewhere, open a set-password mail on a phone on mobile data, or
scan a ticket QR code from outside the Wi-Fi, the apps need a **public HTTPS address**. ngrok gives
one by opening an outbound connection from this machine to ngrok's edge and forwarding everything
that arrives at the public hostname back down it. No port forwarding, no firewall rule, no public IP.

## 2. The constraint that shapes everything: the free plan

- **One static domain** per account: `lustrous-unfrozen-legacy.ngrok-free.dev` (the same account
  as the CRM project). It survives restarts, which is what lets it be written into configuration.
- **One agent online at a time.** A second tunnel — a second `ngrok http …`, the CRM's tunnel, or
  the same account on another computer — fails with **`ERR_NGROK_334`**. **The CRM and Stadium
  tunnels cannot be up at the same time.**
- **A browser warning page** on the first visit per browser (see §9).
- **Usage limits** and a ~5-minute cut on a single request (see §9).

So one tunnel per app is impossible. All six apps share **one hostname**: ngrok tunnels a single
local **reverse proxy**, which picks the app by the first segment of the path.

## 3. The shape of it

```
   Browser / phone anywhere on the internet
                    │  https://lustrous-unfrozen-legacy.ngrok-free.dev/admin/login
                    ▼
   ┌──────────────────────────────────┐
   │        ngrok edge (cloud)        │   public TLS with ngrok's own certificate
   └──────────────────────────────────┘
                    │  outbound tunnel opened by the agent
                    ▼
   ┌──────────────────────────────────┐
   │  ngrok agent (ngrok.exe)         │   ngrok http https://localhost:7000 --url=…
   └──────────────────────────────────┘   (its own inspector: 127.0.0.1:4040)
                    │  HTTPS, loopback
                    ▼
   ┌──────────────────────────────────┐
   │  Stadium proxy (Node)            │   tools/proxy/proxy.js
   │  https://127.0.0.1:7000          │   Express + http-proxy-middleware
   └──────────────────────────────────┘
      │         │         │        │       │        │      prefix is NOT stripped
   /backend /customer  /admin    /bar    /sim   /runner     HTTPS, loopback
      ▼         ▼         ▼        ▼       ▼        ▼
     API    Customer   Admin     Bar   Simulator Runner
    :7010    :7020     :7030    :7040   :7050    :7060     ("ngrok" launch profile)
```

Every hop is HTTPS (project rule). The proxy uses an exported copy of the machine's trusted ASP.NET
Core dev certificate; ngrok does not verify that local certificate, and the proxy does not verify
the apps' (`secure: false`) — every local hop is loopback.

| Public address | App |
|---|---|
| `https://lustrous-unfrozen-legacy.ngrok-free.dev/backend/swagger` | API (Swagger UI) |
| `https://lustrous-unfrozen-legacy.ngrok-free.dev/customer/` | Customer |
| `https://lustrous-unfrozen-legacy.ngrok-free.dev/admin/` | Admin |
| `https://lustrous-unfrozen-legacy.ngrok-free.dev/bar/` | Bar |
| `https://lustrous-unfrozen-legacy.ngrok-free.dev/sim/` | Ticketing Simulator |
| `https://lustrous-unfrozen-legacy.ngrok-free.dev/runner/` | Runner |

Four pieces make this work:

1. **The proxy** — `tools/proxy/proxy.js` (+ `package.json`).
2. **The tunnel script** — `start-ngrok.bat` in the repository root.
3. **The automatic launcher + proxy support** — `StadiumDrinkOrdering.Shared/Hosting/`
   (`TunnelLauncher.cs`, `ReverseProxySetup.cs`), called from each server app's `Program.cs`.
4. **The `ngrok` launch profile** of each app (`Properties/launchSettings.json`) and the `ngrok`
   multi-start entry in `StadiumDrinkOrdering.slnLaunch.user`.

## 4. The proxy (`tools/proxy/proxy.js`)

An Express 5 server on **https://127.0.0.1:7000** (`PORT` overrides), using
`http-proxy-middleware` 4. Requires Node ≥ 22.15. `node_modules` is gitignored and installed by the
batch on first run.

### 4.1 The map

```js
const apps = {
    '/backend':  { name: 'API',                 port: 7010, landing: '/backend/swagger' },
    '/customer': { name: 'Customer',            port: 7020, landing: '/customer/' },
    '/admin':    { name: 'Admin',               port: 7030, landing: '/admin/' },
    '/bar':      { name: 'Bar',                 port: 7040, landing: '/bar/' },
    '/sim':      { name: 'Ticketing Simulator', port: 7050, landing: '/sim/' },
    '/runner':   { name: 'Runner',              port: 7060, landing: '/runner/' },
};
```

The API is `/backend`, not `/api`, so public API URLs read `/backend/api/...` instead of the
confusing `/api/api/...`.

### 4.2 The prefix is forwarded, not stripped

A request for `/admin/login` reaches Admin as **`/admin/login`**. Each middleware owns its prefix
through `pathFilter` (not `app.use('/admin', …)`, which would make Express strip it), and there is
no `pathRewrite`.

Why: an app that thinks it lives at the root generates root links (`/login`, `/_blazor`,
`/css/site.css`) which point at the root of the public hostname — nobody's. Instead each app is
*told* its prefix (`Proxy__Prefix`, §7) and strips it itself with `UsePathBase`, so everything it
generates afterwards carries the prefix.

`/admin` (no trailing slash) is redirected (308) to `/admin/` so relative URLs resolve inside the app.

### 4.3 Forwarded headers

`changeOrigin: true` rewrites `Host` to `localhost:70x0`. The proxy therefore sets single-valued
`X-Forwarded-Proto` (from ngrok, else `https`), `X-Forwarded-Host` (from ngrok, else `Host`) and
`X-Forwarded-For`. The apps trust them only under `Proxy__Enabled=true` (§7). That is what makes
login redirects, the Customer "Open on mobile" QR code etc. use the public hostname.

### 4.4 WebSockets (Blazor Server)

Admin, Customer, Bar and the Simulator are Blazor Server: every click travels over a WebSocket at
`<prefix>/_blazor`. Upgrades never pass through Express middleware, so the proxy handles
`server.on('upgrade')` itself and hands the socket to the middleware owning that prefix (the
Referer picks the app when the URL has no prefix). Without this, pages render and **no button does
anything**. The proxy window logs every `WebSocket /admin/_blazor…` it routes.

### 4.5 Requests that arrive without a prefix

Root-absolute URLs still appear: a returnUrl such as `/orders`, VS's injected
`/_framework/aspnetcore-browser-refresh.js`, a hand-typed address. In order, the proxy:

1. Uses the **Referer** — a request from a page under `/admin` belongs to `/admin`. This includes
   `GET /` from inside an app (e.g. "go home" after login), which goes back to that app's home.
2. **Probes** (GET/HEAD only) each app for the path. Apps that answer everything (Blazor fallback
   pages, API 401s) are detected once with `/__stadium-proxy-probe-9f3a2b` and excluded. Only a
   single unambiguous match counts; answers are cached (500 paths).
3. Redirects **307** to the prefixed path (keeps method and body — POSTs survive).
4. Otherwise shows a small "which app did you mean?" page.

`GET /` without an app Referer shows the map itself.

**Visual Studio dev tooling is the exception.** `/_framework/aspnetcore-browser-refresh.js`,
`/_framework/blazor-hotreload.js` and `/_vs/*` are served by middleware VS injects *ahead of* the
app's `UsePathBase` (and ahead of the Runner DevServer's `pathbase`), so it answers only the
**unprefixed** path. The proxy forwards these to the Referer's app **without** adding the prefix (and
strips it if one arrives prefixed). Redirecting them instead gives 404s, and a failed
`blazor-hotreload.js` import throws inside the Runner's WASM entry point. The browser-refresh
WebSocket itself goes straight to VS on `localhost`, so live reload works only on this machine.

Root-absolute links in the Blazor apps were converted to base-relative (`NavigateTo("orders")`,
`href="events"`), which is the same URL under `<base href="/">` but stays inside the prefix behind
the proxy — so this recovery is a safety net, not the normal path. **Write new links base-relative**
(no leading `/`), or they cost a full page reload (and a new Blazor circuit) behind the tunnel.

### 4.6 `/open/:app` — opening the public address from Visual Studio

Every `ngrok` profile has `launchUrl: https://localhost:7000/open/<name>`. The proxy asks the
ngrok agent (`http://127.0.0.1:4040/api/tunnels`, 2 s timeout) for its public URL and redirects
(302, `no-store`) to `<public URL><landing>`. If ngrok does not answer, it redirects to the same
path on the local proxy instead — so F5 never lands on an ngrok "endpoint offline" page.

### 4.7 Certificate

On start the proxy exports the current trusted dev certificate with
`dotnet dev-certs https --export-path %LOCALAPPDATA%\StadiumApp\ngrok-proxy-cert.pfx` and re-exports
when that copy is older than 30 days. `certificates/aspnetcore.pfx` is **not** used (it is the
Docker certificate and has its own expiry). Override with `PROXY_PFX` / `PROXY_PFX_PASSWORD`.

## 5. The tunnel script (`start-ngrok.bat`)

- **Interactive** — double-click or run it in a terminal.
- **Automatic** — `start-ngrok.bat auto`, as started by `TunnelLauncher` (§6): same work, no
  questions, app checks skipped (the apps are still starting).

In order:

1. Checks `ngrok` is on `PATH`.
2. *(Interactive)* Checks ports 7010–7060 are listening; asks before continuing without some
   (their paths answer 502).
3. Starts the proxy if 7000 is free: `npm install` next to `proxy.js` when `node_modules` is
   missing, then a window titled **"Stadium proxy (7000)"** (`cmd /k`, so a crash stays readable),
   waiting up to 20 s for the port.
4. *(Interactive)* Checks each Blazor app is on the `ngrok` profile: its page must render
   `<base href="/admin/">`, not `<base href="/">`.
5. Kills any `ngrok.exe` on this machine (one agent per account — it cannot kill one on another PC).
6. Starts `ngrok http https://localhost:7000 --url=https://lustrous-unfrozen-legacy.ngrok-free.dev`.
   `--url` is mandatory: without it ngrok picks a random hostname and the written-down address
   answers `ERR_NGROK_3200`.

**CTRL+C or closing the "ngrok tunnel" window takes the public address down.**

## 6. The automatic launcher (`Shared/Hosting/TunnelLauncher.cs`)

So that **F5 is the whole procedure**. Called through `builder.AddReverseProxySupport()` in the
API, Admin, Customer, Bar and Simulator `Program.cs`. (The Runner is WebAssembly and cannot launch
processes; any other app in the multi-start does it.)

### 6.1 When it acts

All of: Windows; `Development`; `STADIUM_TUNNEL` is not `0`; `Proxy:Enabled` parses as `true`
(only the `ngrok` profiles set it). The launch-profile *name* is deliberately not read —
`DOTNET_LAUNCH_PROFILE` is not set on every VS path. It then walks up from the content root looking
for `start-ngrok.bat`; a container has no batch file, so it stops there.

### 6.2 Only one app does it

The API and the Blazor apps start in the same second; two batch runs would kill each other's agent.
A named mutex `Local\Stadium-ngrok-tunnel-launcher` is held for the lifetime of the process; the
others print `ngrok: another Stadium app is taking care of the tunnel`. An abandoned mutex (holder
killed by Stop Debugging) counts as acquired.

### 6.3 What it checks

Both halves, 500 ms TCP connect each: agent `:4040` and proxy `:7000`. Both up →
`ngrok: tunnel already running`. Anything down → runs the batch (it restarts whatever is missing).
Checking only ngrok is not enough: ngrok outlives Stop Debugging, and with the proxy window closed
every path would answer 502.

### 6.4 How it starts the batch

`cmd /c start "ngrok tunnel" "<repo>\start-ngrok.bat" auto` — its own window, which **survives the
debugging session**, so the next F5 reuses the tunnel. Exceptions are printed, never thrown.

## 7. What the apps do differently under the `ngrok` profile

No code difference between profiles — only configuration, read by
`ReverseProxySetup` (`Shared/Hosting/ReverseProxySetup.cs`):

### 7.1 `Proxy__Enabled=true`

`UseForwardedHeaders` for `X-Forwarded-For/-Proto/-Host` with `KnownNetworks`/`KnownProxies`
cleared, run first in the pipeline. The request's scheme and host become the public ones. Parsed
with `bool.TryParse`, so an empty value means off instead of a startup crash.
`UseHttpsRedirection` stays on — the whole chain is HTTPS, so it never fires.

**Security:** forwarded headers are trusted from any caller. That is safe only because the
`ngrok` profiles bind **localhost only** (Customer and Bar also override the LAN `Urls` from their
`appsettings.Development.json`) and the proxy binds 127.0.0.1. Never combine these settings with a
LAN/`0.0.0.0` binding.

### 7.2 `Proxy__Prefix=/admin`

`UsePathBase("/admin")`: `/admin/login` becomes path `/login` with `PathBase=/admin`; links,
`<base href="~/">`, redirects, the Blazor circuit and the Swagger UI all include it. Unprefixed
requests pass through, so `https://localhost:7030/` keeps working directly too.

### 7.3 Per-app settings

| App | `ngrok` profile settings | Notes |
|---|---|---|
| API | `Prefix=/backend`, `CustomerApp__BaseUrl=https://<domain>/customer` | Ticket/season-pass QR deep links and set-password mails point at the tunnel. Swagger endpoint is relative (`v1/swagger.json`). |
| Customer | `Prefix=/customer`, `ApiSettings__PublicBaseUrl=/backend`, `Urls` | Stadium images are loaded by the browser via `/backend`. |
| Admin | `Prefix=/admin`, `ApiSettings__PublicBaseUrl=/backend` | Team crests, venue logos, stadium images via `/backend`. |
| Bar | `Prefix=/bar`, `ApiSettings__PublicBaseUrl=/backend`, `Urls` | |
| Simulator | `Prefix=/sim` | |
| Runner | env `pathbase=/runner` (no `Proxy__*`) | See §7.5. |

Server-side calls (HttpClient, the .NET SignalR clients) keep using `ApiSettings:BaseUrl` =
`https://localhost:7010` over loopback; they never go through the tunnel.

### 7.4 `ApiSettings:PublicBaseUrl` — URLs the browser fetches

`AppConfiguration.ResolvePublicApiBaseUrl()` returns `ApiSettings:PublicBaseUrl` when set, else the
normal API base URL. Use it for anything the **browser** loads from the API (`<img src>`, URLs
handed to JavaScript). The `ngrok` profiles set it to the root-relative `/backend`, so a remote
browser loads through the proxy instead of from `https://localhost:7010`, which only exists here.

### 7.5 The Runner (WebAssembly)

The Runner is served by the WASM DevServer, which cannot run our middleware, and its code runs in
the browser:

- The DevServer's own `pathbase` setting (env var `pathbase=/runner` in the profile) makes it serve
  under `/runner`. With it set, the DevServer answers **only** prefixed requests — open the Runner
  through the proxy under this profile.
- `wwwroot/index.html` switches `<base href>` to `/runner/` with a tiny inline script when the page
  is loaded under `/runner/`, before any relative URL is resolved; direct LAN access at the root is
  unchanged. The service worker then registers with scope `/runner/`.
- `Program.cs` picks the API: served under a path prefix ⇒ behind the proxy ⇒ API is the sibling
  `<origin>/backend/` (same origin, no CORS). Otherwise it keeps the old "same host, port 7010" rule.
- The ngrok profile binds `https://localhost:7060` with the default dev certificate; the LAN-only
  `https` profile is untouched.

## 8. Day-to-day

### 8.1 Once per machine

1. ngrok on `PATH` (installed via winget here) and signed in: `ngrok config add-authtoken <token>`
   with the account that owns the domain. The token is a password — never in this repo.
2. Node.js ≥ 22.15 with npm. The batch installs the proxy's packages itself.
3. A trusted dev certificate (`dotnet dev-certs https --trust`), which the apps need anyway.
4. The `ngrok` multi-start entry in `StadiumDrinkOrdering.slnLaunch.user`. That file is gitignored
   (`*.user`), so on a new machine create it in VS: **Solution → Configure Startup Projects → New
   profile "ngrok"**, all six projects on Start with launch profile `ngrok`.

### 8.2 Every time

1. No Stadium app already running (cleanup command in `CLAUDE.md`).
2. The local Postgres container up (`.\ensure-localdb.ps1`).
3. Make sure the **CRM tunnel is not running** (same account, §2).
4. In Visual Studio pick **`ngrok`** in the startup dropdown, **F5**. The first app up opens the
   **"ngrok tunnel"** window, which opens **"Stadium proxy (7000)"**. Paths answer 502 for the few
   seconds until each app finishes starting.
5. The browsers open `https://localhost:7000/open/<app>` and land on the public address.
6. Share `https://lustrous-unfrozen-legacy.ngrok-free.dev/<app>/…`.

Without VS: `dotnet run --launch-profile ngrok` in each project folder, then `start-ngrok.bat` if no
app started it.

### 8.3 Stopping

- **Stop Debugging stops the apps only.** The tunnel and proxy windows stay so the next F5 reuses
  them.
- Close **"ngrok tunnel"** (or CTRL+C) to take the public address down; close
  **"Stadium proxy (7000)"** to free port 7000.
- Full cleanup including the tunnel:
  ```powershell
  Get-Process StadiumDrinkOrdering.*,ngrok -ErrorAction SilentlyContinue | Stop-Process -Force
  Get-NetTCPConnection -LocalPort 7000,7060 -State Listen -ErrorAction SilentlyContinue |
    Select-Object -Expand OwningProcess -Unique | ForEach-Object { Stop-Process -Id $_ -Force }
  ```

## 9. Free-plan behaviour you will run into

| Symptom | Cause | What to do |
|---|---|---|
| `ERR_NGROK_334`, ngrok window closes | Another agent on the account is online — the CRM tunnel, or another computer. | Stop it there. One tunnel per account at a time. |
| `ERR_NGROK_3200` (endpoint offline) | No agent online, or started without `--url`. | F5 on an `ngrok` profile, or run `start-ngrok.bat`. |
| `ERR_NGROK_3004` after a long wait | ngrok cut a request at ~5 minutes. | Long work must not live inside one request. |
| "You are about to visit…" page | Free-plan browser warning on `*.ngrok-free.dev`. | Click **Visit Site** once per browser (ngrok then sets a cookie). Scripts/tools can send the `ngrok-skip-browser-warning` header. Until clicked, background fetches (e.g. the Runner service worker) get that HTML page. Requests sent **without cookies** get it even after clicking — that is why the Runner and Customer `<link rel="manifest">` carry `crossorigin="use-credentials"` (otherwise: "Manifest: Line 1, column 1, Syntax error"). |
| Tunnel stops late in the month | Monthly quota. | ngrok dashboard. |

## 10. Troubleshooting

| Symptom | Likely cause | Check |
|---|---|---|
| `/<app>/` answers **502** | App not running or still starting. | Its port listening? The proxy window logs `Error (/admin -> :7030)…`. |
| Every path 502 although apps run | Proxy not running behind a live agent. | Port 7000 listening? F5 again — the launcher repairs both halves. |
| Page without CSS, links go to the hostname root | App on its `https` profile (no path base), or `Proxy__Prefix` ≠ proxy map. | Select `ngrok`. The interactive batch warns per Blazor app. |
| Page renders, no button works | WebSocket upgrade not reaching the app. | Proxy window should log `WebSocket /<app>/_blazor…`. |
| Images (crests, stadium map) broken remotely | A browser-facing URL built from `ApiSettings:BaseUrl`. | Use `AppConfiguration.ResolvePublicApiBaseUrl` (§7.4). |
| Navigation reloads the page / logs Customer out | A root-absolute link (`"/orders"`) left the prefix and was recovered by the proxy. | Make it base-relative (`"orders"`). |
| Mail / QR code points at `localhost` | API not on its `ngrok` profile. | Restart the API on `ngrok`. |
| Runner says "configured only to handle request URIs within the PathBase '/runner'" | Runner opened directly at `https://localhost:7060/` under the `ngrok` profile. | Open it via `/runner/` (or use its `https` profile). |
| F5 opens "cannot connect" to `localhost:7000` | Nobody started the proxy (only the Runner started, or `STADIUM_TUNNEL=0`). | Run `start-ngrok.bat`. |
| No `ngrok:` line in an app's console | A launcher gate failed (§6.1). | Development? `STADIUM_TUNNEL`? `Proxy__Enabled`? batch above the content root? |

## 11. Security — read before sharing a link

The tunnel puts this machine on the **internet**, not just the Wi-Fi.

- **All six apps are public** under the `ngrok` multi-start, including Admin and the API with
  Swagger. The API must be public for the Runner (it calls it from the browser); Admin, Bar and
  the Simulator are exposed only for convenience — leave them out of the multi-start when you only
  need to show the Customer app.
- Forwarded headers are trusted from any caller under this profile — safe only with the localhost
  bindings described in §7.1.
- The ngrok authtoken is a credential. Never commit it.
- Starting the API applies pending EF migrations, including uncommitted ones in your working tree.

## 12. Changing things

| To change… | Edit |
|---|---|
| The ngrok domain | `DOMAIN` in `start-ngrok.bat`; `CustomerApp__BaseUrl` in the API's `ngrok` profile. |
| Which path maps to which app | `apps` in `proxy.js` **and** `Proxy__Prefix` + `launchUrl` in that app's `ngrok` profile (Runner: `pathbase` env + the prefix in `wwwroot/index.html`; API prefix also `ProxiedApiPath` in `Runner/Program.cs` and `ApiSettings__PublicBaseUrl` in the profiles). |
| The proxy port (7000) | `PORT` in `proxy.js`, `PROXY_PORT` in the batch, `ProxyPort` in `TunnelLauncher.cs`, every `launchUrl`. |
| Turn the automatic tunnel off | `STADIUM_TUNNEL=0` in the environment. |
| Add an app | Map entry in `proxy.js`; an `ngrok` profile with `Proxy__Enabled`/`Proxy__Prefix` binding localhost; `builder.AddReverseProxySupport()` + `app.UseReverseProxySupport()` in its `Program.cs`; add it to the `ngrok` multi-start entry and the batch's checks. |

## 13. File index

| File | Role |
|---|---|
| `start-ngrok.bat` | Starts the proxy (if needed) and the ngrok agent on the reserved domain. |
| `tools/proxy/proxy.js`, `package.json`, `package-lock.json` | Path-based HTTPS reverse proxy on :7000; WebSockets; `/open/<app>`; unprefixed-path recovery. |
| `StadiumDrinkOrdering.Shared/Hosting/ReverseProxySetup.cs` | `AddReverseProxySupport` / `UseReverseProxySupport`: forwarded headers + path base from `Proxy:*`. |
| `StadiumDrinkOrdering.Shared/Hosting/TunnelLauncher.cs` | Runs the batch on F5 when an app comes up on the `ngrok` profile. |
| `StadiumDrinkOrdering.Shared/Configuration/AppConfiguration.cs` | `ResolvePublicApiBaseUrl` for browser-facing API URLs. |
| `*/Properties/launchSettings.json` → `ngrok` | Per-app `Proxy__*`, `launchUrl`, extras. |
| `StadiumDrinkOrdering.slnLaunch.user` | The `ngrok` multi-start entry (local, gitignored). |
| `StadiumDrinkOrdering.Runner/Program.cs`, `wwwroot/index.html` | Runner API resolution and `<base href>` under `/runner/`. |
