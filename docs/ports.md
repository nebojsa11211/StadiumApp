# Port Configuration Documentation

## Overview
This document defines the **permanent and fixed port assignments** for all Stadium Drink Ordering System projects. These ports are consistent across all run configurations (dotnet run, IIS Express, Visual Studio, Docker).

## ✅ Fixed Port Assignments

### StadiumDrinkOrdering.API
**Description**: ASP.NET Core Web API Backend with Swagger documentation
- **HTTPS**: `7010`
- **HTTP**: `7011` 
- **Docker HTTPS**: `9010` → Internal: `8443`
- **Docker HTTP**: `9011` → Internal: `8080`
- **URLs**:
  - Development: https://localhost:7010
  - Docker: https://localhost:9010
  - Swagger: https://localhost:7010/swagger | https://localhost:9010/swagger (Docker)

### StadiumDrinkOrdering.Customer  
**Description**: Blazor Server Customer Frontend Application
- **HTTPS**: `7020`
- **HTTP**: `7021`
- **Docker HTTPS**: `9020` → Internal: `8444`
- **Docker HTTP**: `9021` → Internal: `8081`
- **URLs**:
  - Development: https://localhost:7020
  - Docker: https://localhost:9020

### StadiumDrinkOrdering.Admin
**Description**: Blazor Server Admin Management Application  
- **HTTPS**: `7030`
- **HTTP**: `7031`
- **Docker HTTPS**: `9030` → Internal: `8445`
- **Docker HTTP**: `9031` → Internal: `8082`
- **URLs**:
  - Development: https://localhost:7030
  - Docker: https://localhost:9030

### StadiumDrinkOrdering.Staff
**Description**: Blazor Server Staff Operations Application
- **HTTPS**: `7040` 
- **HTTP**: `7041`
- **Docker HTTPS**: `9040` → Internal: `8446`
- **Docker HTTP**: `9041` → Internal: `8083`
- **URLs**:
  - Development: https://localhost:7040
  - Docker: https://localhost:9040

## LAN Access (testing from a phone / tablet)

Every `commandName: Project` launch profile binds **two** endpoints:

```
https://192.168.178.44:<port>;https://localhost:<port>
```

**Order matters.** Visual Studio opens the browser at the **first** URL, so the LAN address is
listed first — that is what makes F5 land on `https://192.168.178.44:7020/...` instead of
`localhost`. Swap the order to go back to a localhost address bar. `localhost` keeps working
either way; both endpoints are always bound.

> ⚠️ Because the LAN address is the F5 target, the desktop browser shows a certificate warning on
> every launch (`ERR_CERT_COMMON_NAME_INVALID`) unless the LAN certificate is installed — the ASP.NET
> dev cert is only valid for the name `localhost`. See "The LAN certificate" below.

> Switching origin also means a **fresh `localStorage`**: JWTs saved under `https://localhost:7030`
> are not visible to `https://192.168.178.44:7030`, so you will be asked to log in again.

> **Exception — the Runner binds the LAN address ONLY**, and loads the LAN certificate explicitly via
> `Kestrel__Certificates__Default__*` in its launch profile. It is the phone app, so it is served over
> the LAN or not at all. The trade-off: its `inspectUri` (`wss://{url.hostname}:{url.port}/_framework/
> debug/ws-proxy`) was removed, because the Blazor WASM debug proxy expects a loopback origin — VS
> otherwise fails with *"Failed to launch debug adapter … Could not open wss://…:7060/_framework/debug/
> ws-proxy"*. To debug the Runner's C# in VS, temporarily point `applicationUrl` back at
> `https://localhost:7060`, drop the two `Kestrel__Certificates__*` variables, and restore `inspectUri`.
>
> Because the Runner is LAN-only, its profile has a single URL with no `;` — `set-dev-host.ps1`
> handles that shape separately from the two-endpoint pair the other five apps use.

> **Gotcha:** `Customer` and `Bar` also pin a `"Urls"` key in their `appsettings.Development.json`.
> `WebApplication` applies that key **over** the host URLs from `applicationUrl`, so it must list the
> LAN address too — otherwise those two apps bind loopback only and silently ignore the launch
> profile. The other four apps have no `Urls` key. `set-dev-host.ps1` keeps both places in sync.

### One-time setup
1. **Open the firewall** (run as Administrator): `.\open-lan-dev-ports.ps1`
   Adds an inbound TCP rule for 7010–7060 on the **Private** profile only.
   Undo with `.\open-lan-dev-ports.ps1 -Remove`.
2. Start the apps from Visual Studio as usual.
3. On the phone, browse to `https://192.168.178.44:<port>`.

### 🔐 The LAN certificate
The ASP.NET dev certificate is only valid for the name `localhost`, so hitting an app by IP produces
a certificate warning. `certificates\stadium-lan.pfx` is a self-signed cert whose SAN list carries the
**LAN IP** (plus `localhost`, `api`, and the machine name), which is what makes the IP origin
trustable. The Runner loads it via `Kestrel__Certificates__Default__Path` in its launch profile.

```powershell
.\generate-lan-cert.ps1                          # auto-detect the current LAN IP
.\generate-lan-cert.ps1 -IpAddress 192.168.1.50  # or set it explicitly
.\generate-lan-cert.ps1 -Trust                   # also add it to Cert:\CurrentUser\Root
```

**Run this once per machine, and again whenever the LAN IP changes** — the IP is baked into the
certificate, so `set-dev-host.ps1` alone is not enough. The `.pfx` is gitignored (`*.pfx`), so a fresh
clone has only the public `.cer` and the Runner fails to start with
`FileNotFoundException: Could not find file '…\certificates\stadium-lan.pfx'` until you run it.

- **Desktop**: pass `-Trust`, or double-click `stadium-lan.cer` → Install Certificate → Trusted Root
  Certification Authorities.
- **Phone**: transfer `stadium-lan.cer` and install it as a trusted credential. Until you do,
  **service workers will not register**, so the Runner PWA cannot be installed and offline mode
  cannot be tested over the LAN.
- Re-running mints a **new** cert, so every device that trusted the old `.cer` must install the new one.
- Server-to-server calls are unaffected — every client app already sets
  `ServerCertificateCustomValidationCallback => true`.

### ⚠️ The LAN address is a DHCP lease
`192.168.178.44` is baked into all six launch profiles. When the lease changes, re-stamp them:

```powershell
.\set-dev-host.ps1                          # auto-detect the current LAN IP
.\set-dev-host.ps1 -IpAddress 192.168.1.50  # or set it explicitly
```

A **DHCP reservation** on the router avoids this entirely and is the recommended fix.

### How the apps find the API over the LAN
- **Server-rendered apps** (Admin, Customer, Bar, Simulator) call the API from the *server*, so
  their `ApiSettings:BaseUrl` of `https://localhost:7010` stays correct regardless of how the
  browser reached them. No change needed.
- **Runner (WASM)** runs in the *browser*, where `localhost` means the phone. It therefore derives
  the API host from the origin that served it (see `Runner/Program.cs`) — loaded from
  `https://192.168.178.44:7060`, it calls `https://192.168.178.44:7010`. An explicit non-loopback
  `ApiSettings:BaseUrl` in `wwwroot/appsettings.json` still overrides this, so pointing the Runner
  at a staging API remains config-only.
- **API CORS**: in **Development only**, the API accepts any loopback or RFC1918 private-range
  origin, so the Runner's LAN origin is allowed without re-pinning the IP. Non-development
  behaviour is unchanged and still driven by `CORS_ALLOWED_ORIGINS`.

## Port Assignment Strategy

### Development Ports (7000-7099)
- **7010-7019**: API services
- **7020-7029**: Customer applications
- **7030-7039**: Admin applications  
- **7040-7049**: Staff / Bar applications
- **7050**: TicketingSimulator (external ticketing dev tool)
- **7060**: Runner (Waiter WASM PWA) — moved off 7050 to avoid colliding with the Simulator
- **7070-7099**: Reserved for future services

### Docker External Ports (9000-9099) 
- **9010-9019**: API services
- **9020-9029**: Customer applications
- **9030-9039**: Admin applications
- **9040-9049**: Staff applications
- **9050-9099**: Reserved for future services

### Internal Container Ports
- **8443/8080**: API internal HTTPS/HTTP ports
- **8444/8081**: Customer internal HTTPS/HTTP ports
- **8445/8082**: Admin internal HTTPS/HTTP ports
- **8446/8083**: Staff internal HTTPS/HTTP ports

## Configuration Files Updated

### launchSettings.json Files
All projects have standardized launch profiles:
- **http**: HTTP-only profile
- **https**: HTTPS + HTTP profile  
- **[ProjectName]**: Main project profile (HTTPS + HTTP)
- **IIS Express**: Uses same ports as direct run

### Docker Compose
Updated `docker-compose.yml` with fixed external port mappings:
```yaml
ports:
  - "9010:8443"  # API HTTPS
  - "9011:8080"  # API HTTP
  - "9020:8444"  # Customer HTTPS
  - "9021:8081"  # Customer HTTP
  - "9030:8445"  # Admin HTTPS
  - "9031:8082"  # Admin HTTP
  - "9040:8446"  # Staff HTTPS
  - "9041:8083"  # Staff HTTP
```

## Running the Applications

### Individual Development (dotnet run)
```bash
# API Backend
cd StadiumDrinkOrdering.API
dotnet run --launch-profile https
# → https://localhost:7010

# Customer App
cd StadiumDrinkOrdering.Customer
dotnet run --launch-profile https
# → https://localhost:7020

# Admin App
cd StadiumDrinkOrdering.Admin  
dotnet run --launch-profile https
# → https://localhost:7030

# Staff App
cd StadiumDrinkOrdering.Staff
dotnet run --launch-profile https  
# → https://localhost:7040
```

### Visual Studio / IIS Express
Projects use the same fixed ports as dotnet run. No more random port assignments.

### Docker Compose
```bash
docker-compose up --build -d

# Access applications (HTTPS):
# API: https://localhost:9010
# Customer: https://localhost:9020
# Admin: https://localhost:9030
# Staff: https://localhost:9040

# Alternative HTTP access:
# API: http://localhost:9011
# Customer: http://localhost:9021
# Admin: http://localhost:9031
# Staff: http://localhost:9041
```

## Network Communication

### Inter-Service Communication (Docker)
Services communicate using internal Docker network:
```
Customer → API: https://api:8443
Admin → API: https://api:8443
Staff → API: https://api:8443
```

### External Access
Host machine accesses via external ports:
```
Host → API: https://localhost:9010
Host → Customer: https://localhost:9020
Host → Admin: https://localhost:9030
Host → Staff: https://localhost:9040
```

## Testing & Validation

### Health Check Endpoints
- **API Health**: https://localhost:7010/health | https://localhost:9010/health
- **API Swagger**: https://localhost:7010/swagger | https://localhost:9010/swagger

### Port Availability Testing
```bash
# Check if ports are available
netstat -an | findstr :7010
netstat -an | findstr :7020  
netstat -an | findstr :7030
netstat -an | findstr :7040
```

### Playwright Test Configuration
Update test configurations to use fixed ports:
```typescript
const API_BASE_URL = 'https://localhost:9010';
const CUSTOMER_BASE_URL = 'https://localhost:9020';
const ADMIN_BASE_URL = 'https://localhost:9030';
const STAFF_BASE_URL = 'https://localhost:9040';
```

## Troubleshooting

### Port Conflicts
If any port is already in use:
1. Stop the conflicting process
2. Or modify the specific port in both launchSettings.json and docker-compose.yml
3. Update this documentation

### Service Discovery Issues
Ensure Docker services use internal hostnames:
- API service: `api:8080` (not localhost)
- Database: `sqlserver:1433` (if using SQL Server container)

## Migration Notes

### Previous → Current Port Mapping
| Service | Old Development | Old Docker | New Development | New Docker |
|---------|----------------|------------|-----------------|------------|
| API | 7000/7001 | 9000 | 7010/7011 | 9010 |
| Customer | 7002/7003 | 9001 | 7020/7021 | 9020 |
| Admin | 7004/7005 | 9002→9004 | 7030/7031 | 9030 |  
| Staff | 7006/7007 | 9003 | 7040/7041 | 9040 |

### Breaking Changes
- All previous bookmarks and hardcoded URLs must be updated
- CI/CD pipelines need port updates
- Environment-specific configurations require updates
- Test configurations need port adjustments

---

## Maintenance

**Last Updated**: 2025-09-15  
**Next Review**: When adding new services or major infrastructure changes

**Validation Status**: ✅ All projects tested and verified with fixed ports

**Contact**: Development Team for port assignment requests or conflicts