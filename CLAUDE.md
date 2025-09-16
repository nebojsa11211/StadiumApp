# CLAUDE.md

## 🔐 **MANDATORY HTTPS-ONLY PROJECT** 🔐

⚠️ **CRITICAL SECURITY REQUIREMENT**: This entire project operates on **HTTPS EXCLUSIVELY**. **NO HTTP PROTOCOLS ARE PERMITTED** anywhere in the codebase, configuration, or deployment. All services, applications, APIs, and communications must use HTTPS with proper SSL certificates.

---

## Project Overview
**Stadium Drink Ordering System** – A microservices-based **.NET 8.0** application for ordering beverages in a stadium environment. It consists of:

- **API Backend** (ASP.NET Core Web API) - **HTTPS ONLY**
- **Customer Blazor Server App** - **HTTPS ONLY**
- **Admin Blazor Server App** - **HTTPS ONLY**
- **Staff Blazor Server App** - **HTTPS ONLY**

### Key Features
- **🔒 HTTPS-ONLY COMMUNICATION** - All services and endpoints use HTTPS exclusively
- PostgreSQL/Supabase database for all environments
- JWT authentication with role-based access (Admin, Staff, Customer)
- **🔒 MANDATORY ADMIN AUTHENTICATION** - All admin pages require login
- SignalR (BartenderHub) for real-time updates - **HTTPS ONLY**
- Dockerized services with docker-compose orchestration - **HTTPS ONLY**
- Entity Framework Core with migrations support
- Stadium structure management with JSON import/export
- Interactive stadium seating map visualization
- **Complete Customer Ticketing System** with event browsing, seat selection, and checkout
- Real-time shopping cart with seat reservations and timeout handling
- Comprehensive order processing with payment simulation and ticket generation

---

## Development Commands

### Docker Development (Recommended)
- **Start services:** `docker-compose up --build -d`
- **Stop services:** `docker-compose down`
- **View logs:** `docker-compose logs -f [service-name]`
- **Restart service:** `docker-compose restart [service-name]`
- **Rebuild after code changes:** `docker-compose up --build -d [service-name]`

🔒 **HTTPS-ONLY**: All Docker containers use HTTPS exclusively for both internal and external communication. All mapped ports (9010-9040) use HTTPS with SSL certificates.

### Local .NET Development
- **Build solution:** `dotnet build StadiumDrinkOrdering.sln`
- **Run API:** `cd StadiumDrinkOrdering.API && dotnet run --launch-profile https`
- **Run Customer App:** `cd StadiumDrinkOrdering.Customer && dotnet run --launch-profile https`
- **Run Admin App:** `cd StadiumDrinkOrdering.Admin && dotnet run --launch-profile https`
- **Run Staff App:** `cd StadiumDrinkOrdering.Staff && dotnet run --launch-profile https`

---

## Database Management
- **Add migration:** `dotnet ef migrations add <MigrationName> -p StadiumDrinkOrdering.API`
- **Update database:** `dotnet ef database update -p StadiumDrinkOrdering.API`
- **Generate script:** `dotnet ef migrations script -p StadiumDrinkOrdering.API`

### Database Contexts
- **Current**: PostgreSQL/Supabase for all environments
- **Production**: PostgreSQL/Supabase (`ConnectionStrings__DefaultConnection`)

### 🚨 **PostgreSQL/Supabase Migration Status**
**Status**: PostgreSQL configuration active

**✅ Current Configuration:**
1. API configured to use PostgreSQL/Supabase
2. Connection strings configured in appsettings.json
3. Npgsql Entity Framework provider installed
4. Database migrations ready for PostgreSQL
5. **Docker Networking**: Fixed SSL communication between containers (HTTP internally, HTTPS externally)

**Required Steps:**
1. **Supabase Project**: Active Supabase project configured
2. **Connection String**: PostgreSQL connection string in appsettings.json
3. **Apply Migrations**: `dotnet ef database update -p StadiumDrinkOrdering.API`
4. **Test Connection**: Verify all functionality with PostgreSQL

---

## Testing

### Testing Protocol

- **MANDATORY**: After every file modification or code generation task is completed, you MUST run a full suite of Playwright tests.
- **Goal**: Ensure that no new changes have introduced regressions or broken existing functionality.
- **Test Command**: Use the command `npx playwright test` to run all tests.
- **Debugging**: If any test fails, you must analyze the failure logs, find the root cause, and propose a fix.
- **Verification**: After applying a fix, you must re-run the tests to confirm the fix is successful.

### Test Commands
- **Run all tests:** `dotnet test`
- **Run Playwright tests:** `npx playwright test`
- **Run specific test project:** `dotnet test StadiumDrinkOrdering.Tests`
- **Run Playwright tests in headed mode:** `npx playwright test --headed`
- **Run specific Playwright test:** `npx playwright test tests/login.spec.ts`

### Playwright Multi-Context Testing
Use **separate BrowserContext objects** for different users (Admin, Customer) to prevent state overlap.

Example: Admin creates a product → Customer purchases it → Validate order.

---

## Authentication & Security ✅ ENTERPRISE-GRADE SYSTEM

### 🔐 Comprehensive Authentication Architecture
**ENTERPRISE-GRADE**: Complete JWT-based authentication system with refresh tokens, automatic token injection, policy-based authorization, and comprehensive security middleware.

#### Core Authentication Features
- **🔑 JWT Refresh Tokens**: 15-minute access tokens + 7-day refresh tokens
- **🔄 Automatic Token Injection**: HTTP client middleware handles token management
- **🛡️ Policy-Based Authorization**: Fine-grained access control with custom requirements
- **⚡ Background Token Refresh**: Automatic token renewal before expiration
- **🔒 Brute Force Protection**: Rate limiting and progressive delays
- **📊 Security Headers**: Comprehensive security headers on all API responses
- **🎯 Role-Based Access**: Admin, Staff, Customer roles with appropriate permissions

#### Shared Authentication Library (`StladiumDrinkOrdering.Shared/Authentication/`)
**Unified Authentication Framework** across all applications:

```csharp
// Automatic service registration
builder.Services.AddClientAuthentication(apiBaseUrl, "Admin", enableBackgroundRefresh: true);

// Usage in components
@inject IAuthenticationStateService AuthState
@inject ITokenStorageService TokenStorage
```

**Key Interfaces:**
- **`IAuthenticationStateService`**: Unified auth state management
- **`ITokenStorageService`**: Secure token storage with expiration handling
- **`ITokenRefreshService`**: Automatic JWT refresh token management
- **`ISecureApiService`**: Authenticated API calls with retry logic

#### HTTP Client Middleware (`AuthenticationHandler`)
**Automatic Token Injection** with intelligent retry logic:
```csharp
// Automatically handles:
// 1. Token injection on all API calls
// 2. 401 detection and refresh token attempt
// 3. Retry original request with new token
// 4. Fallback to login redirect on refresh failure
```

#### JWT Security Implementation
**Enhanced Security Features:**
- **Algorithm Restrictions**: Only HS256 allowed, prevents algorithm confusion attacks
- **Token Validation**: Comprehensive validation with audience, issuer, expiration checks
- **Refresh Token Rotation**: New refresh token issued on each use
- **Secure Storage**: Browser localStorage with encryption considerations
- **Background Monitoring**: Automatic expiration detection and renewal

#### Policy-Based Authorization (`API/Authorization/`)
**Fine-Grained Access Control:**
```csharp
[Authorize(Policy = "AdminOnly")]
[Authorize(Policy = "StaffOrAdmin")]
[Authorize(Policy = "CustomerAccess")]

// Custom requirements:
// - AdminRequirement: Admin role verification
// - StaffRequirement: Staff role verification
// - UserRequirement: Authenticated user verification
```

#### Security Middleware Stack
**Comprehensive Protection:**
1. **Security Headers Middleware**: HSTS, CSP, NOSNIFF, Frame protection
2. **Rate Limiting Middleware**: IP-based and endpoint-specific limits
3. **Brute Force Protection**: Progressive delays and account lockouts
4. **Global Exception Handling**: Secure error responses without information leakage
5. **CORS Configuration**: Proper cross-origin resource sharing

#### Admin Authentication Requirements
**CRITICAL RULE**: All admin pages require authentication except `/login`

**Authentication Flow:**
1. **Route Protection**: `AuthRoute` component wraps all protected routes
2. **Silent Redirect**: Invalid tokens redirect to login without JavaScript alerts
3. **Session Persistence**: Tokens stored securely with automatic renewal
4. **Return URL Support**: Users redirected to intended page after login

**Default Admin Credentials:**
- **Email**: `admin@stadium.com`
- **Password**: `admin123`

#### Rate Limiting & Brute Force Protection
**API Protection Configuration:**
```json
{
  "RateLimiting": {
    "Authentication": {
      "LoginAttemptsPerMinute": 5,
      "RegisterAttemptsPerHour": 3
    },
    "BruteForce": {
      "MaxFailedAttempts": 5,
      "LockoutDurationMinutes": 15,
      "ProgressiveDelay": {
        "Enabled": true,
        "BaseDelayMs": 1000,
        "MaxDelayMs": 30000,
        "Multiplier": 2.0
      }
    }
  }
}
```

#### Security Headers Configuration
**Comprehensive Protection:**
- **X-Frame-Options**: DENY (prevent clickjacking)
- **X-Content-Type-Options**: nosniff (prevent MIME confusion)
- **X-XSS-Protection**: 1; mode=block (XSS protection)
- **Strict-Transport-Security**: HSTS with includeSubDomains
- **Content-Security-Policy**: Restrictive CSP configuration
- **Referrer-Policy**: strict-origin-when-cross-origin
- **Permissions-Policy**: Disabled dangerous features

#### Implementation Files
**Key Components:**
- **`AuthenticationHandler.cs`**: HTTP client middleware for token injection
- **`TokenRefreshService.cs`**: Background token renewal service
- **`SecurityHeadersMiddleware.cs`**: Comprehensive security headers
- **`BruteForceProtectionService.cs`**: Rate limiting and protection
- **`AuthStateService.cs`**: Unified authentication state management
- **`TokenStorageService.cs`**: Secure token storage per application
- **`AuthorizationPolicies.cs`**: Policy-based authorization configuration

#### Testing & Verification
**Authentication System Status:**
- ✅ All applications start without dependency injection errors
- ✅ JWT refresh tokens implemented and tested
- ✅ HTTP client middleware working correctly
- ✅ Policy-based authorization active
- ✅ Security headers applied to all responses
- ✅ Rate limiting and brute force protection enabled
- ✅ Background token refresh service operational

---

## Authentication Troubleshooting Guide 🔧

### Common Authentication Issues & Solutions

#### 1. Dependency Injection Errors
**Issue**: `InvalidOperationException: Cannot provide a value for property 'AuthStateService'`
```
Error: There is no registered service of type 'IAuthStateService'
```

**Solution**: Ensure proper service registration in `Program.cs`:
```csharp
// Register authentication services
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<IAuthStateService>(provider => provider.GetRequiredService<AuthStateService>());
builder.Services.AddScoped<IAuthenticationStateService>(provider => provider.GetRequiredService<AuthStateService>());
builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();

// Add client authentication
builder.Services.AddClientAuthentication(apiBaseUrl, "Admin", enableBackgroundRefresh: true);
```

**Root Cause**: Missing interface registration for `IAuthStateService` in DI container.

#### 2. Repeated Authentication Alerts
**Issue**: JavaScript alerts appearing repeatedly after clicking "OK"
```
Alert: "Unauthorized access!" appears continuously
```

**Solution**: Remove JavaScript alerts, implement silent redirect:
```csharp
// In AuthRoute.razor - REMOVE this:
// await JSRuntime.InvokeVoidAsync("alert", "Unauthorized access!");

// REPLACE with silent redirect:
NavigationManager.NavigateTo("/login", forceLoad: true);
```

**Root Cause**: JavaScript alert created infinite loop when user clicked OK.

#### 3. Token Storage Issues
**Issue**: Authentication state not persisting across page reloads
```
Error: Token is null after page refresh
```

**Solution**: Verify `TokenStorageService` implementation:
```csharp
// Check localStorage operations in TokenStorageService
public async Task<string?> GetTokenAsync()
{
    try
    {
        var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem",
            AuthenticationConstants.StorageKeys.Admin.Token);
        return token;
    }
    catch (Exception)
    {
        return null; // Handle prerendering gracefully
    }
}
```

**Root Cause**: JSRuntime errors during prerendering or localStorage access issues.

#### 4. HTTP Client Authentication Failures
**Issue**: API calls return 401 Unauthorized despite having valid tokens
```
Error: 401 Unauthorized on API requests
```

**Solution**: Verify `AuthenticationHandler` registration:
```csharp
// In Program.cs
builder.Services.AddClientAuthentication(apiBaseUrl, "Admin", enableBackgroundRefresh: true);

// Check HttpClient configuration
builder.Services.AddHttpClient("AuthenticatedClient")
    .AddHttpMessageHandler<AuthenticationHandler>();
```

**Root Cause**: Missing authentication handler on HTTP client or incorrect service registration.

#### 5. Refresh Token Failures
**Issue**: Background token refresh not working
```
Error: Refresh token expired or invalid
```

**Solution**: Check `BackgroundTokenRefreshService` logs:
```csharp
// Enable debugging in appsettings.json
"Logging": {
  "LogLevel": {
    "StadiumDrinkOrdering.Shared.Authentication": "Debug"
  }
}
```

**Root Cause**: Refresh token expired, invalid, or refresh service not registered properly.

#### 6. CORS Issues with Authentication
**Issue**: CORS errors when making authenticated API calls
```
Error: CORS policy blocked request with authentication headers
```

**Solution**: Configure CORS in API `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("https://localhost:9030", "https://localhost:9020")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});
```

**Root Cause**: CORS policy doesn't allow authentication headers or credentials.

#### 7. Docker Container Authentication Issues
**Issue**: Authentication works locally but fails in Docker
```
Error: Cannot connect to API from container
```

**Solution**: Check Docker networking configuration:
```yaml
# In docker-compose.yml
services:
  admin:
    environment:
      - ApiSettings__BaseUrl=https://api:8443
  api:
    environment:
      - JwtSettings__SecretKey=${JWT_SECRET_KEY}
```

**Root Cause**: Incorrect API base URL or missing environment variables in containers.

### Debugging Tools & Commands

#### Check Container Logs
```bash
# View authentication-related logs
docker logs stadium-admin | grep -i "auth\|token\|login"
docker logs stadium-api | grep -i "auth\|token\|401\|403"
```

#### Test API Authentication
```bash
# Test API accessibility
curl -k -I https://localhost:9010/api/drinks

# Test with authentication
TOKEN="your-jwt-token"
curl -k -H "Authorization: Bearer $TOKEN" https://localhost:9010/api/orders
```

#### Verify Service Registration
Add debugging to `Program.cs`:
```csharp
Console.WriteLine("✅ Registered authentication services:");
Console.WriteLine($"   - AuthStateService: {services.Any(s => s.ServiceType == typeof(AuthStateService))}");
Console.WriteLine($"   - IAuthStateService: {services.Any(s => s.ServiceType == typeof(IAuthStateService))}");
Console.WriteLine($"   - ITokenStorageService: {services.Any(s => s.ServiceType == typeof(ITokenStorageService))}");
```

### Performance Monitoring

#### Monitor Token Refresh
```csharp
// Add logging to BackgroundTokenRefreshService
private readonly ILogger<BackgroundTokenRefreshService> _logger;

protected override async Task ExecuteAsync(CancellationToken stoppingToken)
{
    _logger.LogInformation("Background token refresh service started");
    // ... implementation
}
```

#### Check Memory Usage
```bash
# Monitor container memory usage
docker stats stadium-admin stadium-api --no-stream
```

### Security Verification

#### Verify JWT Configuration
```csharp
// Check JWT settings in API startup
var jwtKey = configuration["JwtSettings:SecretKey"];
if (string.IsNullOrEmpty(jwtKey) || jwtKey.Length < 32)
{
    throw new InvalidOperationException("JWT secret key must be at least 32 characters");
}
```

#### Test Security Headers
```bash
# Verify security headers are applied
curl -k -I https://localhost:9010/api/auth/login | grep -E "(X-Frame-Options|X-Content-Type-Options|Strict-Transport-Security)"
```

### Known Issues & Workarounds

#### Issue: SQLite vs PostgreSQL Configuration
**Problem**: EF migrations fail with "Tenant or user not found"
**Workaround**: Update `appsettings.Development.json` connection string:
```json
{
  "UseSqliteFallback": false,
  "ConnectionStrings": {
    "DefaultConnection": "Host=your-db-host;Port=5432;Database=postgres;Username=user;Password=pass;Ssl Mode=Require"
  }
}
```

#### Issue: Container SSL Certificate Problems
**Problem**: SSL connection errors between containers
**Workaround**: Use HTTP for internal container communication:
```csharp
if (containerEnv == "true")
{
    apiBaseUrl = "http://api:8080"; // Internal HTTP
}
else
{
    apiBaseUrl = "https://localhost:7010"; // External HTTPS
}
```

#### Issue: localStorage Access During Prerendering
**Problem**: JSRuntime errors during server-side prerendering
**Workaround**: Add try-catch blocks in TokenStorageService:
```csharp
try
{
    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
}
catch (Exception)
{
    // Handle prerendering gracefully - localStorage not available
}
```

### Support & Documentation

#### Internal Documentation
- **CLAUDE.md**: Complete project documentation with authentication details
- **API Controllers**: Check `AuthController.cs` for authentication endpoints
- **Service Interfaces**: Review `IAuthenticationStateService` for contract details

#### External Resources
- **JWT.io**: Token inspection and debugging
- **ASP.NET Core Authentication**: Microsoft official documentation
- **Blazor Authentication**: Component-based authentication patterns

---

## Docker Timezone Configuration

### Overview
All Docker containers are configured to use **Europe/Zagreb** timezone (Croatia) for consistent time handling across the system.

### Implementation
- **docker-compose.yml**: Added `TZ=Europe/Zagreb` environment variable to all services (api, customer, admin, staff)
- **Dockerfiles**: Installed `tzdata` package and configured timezone with:
  - `ENV TZ=Europe/Zagreb`
  - `RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone`

### Impact
- Container system time displays Zagreb/Croatia time
- Log timestamps use Croatia timezone
- Application DateTime operations respect the configured timezone
- PostgreSQL UTC handling remains compatible (UTC stored, Zagreb displayed)

---

## Docker HTTPS Configuration ✅

### Overview
All Docker containers now support **secure HTTPS access** with proper SSL certificates, providing production-grade security for web applications and APIs.

### Implementation
- **🔐 SSL Certificates**: ASP.NET Core development certificates auto-generated and trusted on host machine
- **📦 Certificate Management**: Certificates stored in `./certificates/` directory and mounted read-only in containers
- **🔄 Dual Protocol Support**: Both HTTPS and HTTP endpoints available for maximum compatibility
- **🛡️ Security**: Certificate validation properly configured for development environment

### Key Features
- **External HTTPS Access**: All services accessible via https://localhost:903X ports
- **Internal HTTPS Communication**: Services communicate via HTTPS within Docker network
- **Certificate Automation**: One-command certificate generation with `generate-dev-certs.ps1`
- **Health Checks**: Container health checks support both HTTPS and HTTP endpoints
- **Development Friendly**: Self-signed certificates with proper trust configuration

### Port Configuration
| Service | HTTPS Port | HTTP Port | Status |
|---------|------------|-----------|---------|
| API | 9010 | 9011 | ✅ Active |
| Customer | 9020 | 9021 | ✅ Active |
| Admin | 9030 | 9031 | ✅ Active |
| Staff | 9040 | 9041 | ✅ Active |

### Verification
Test HTTPS endpoints:
```bash
curl -k -I https://localhost:9030  # Admin (200 OK)
curl -k -I https://localhost:9020  # Customer (200 OK)  
curl -k -I https://localhost:9040  # Staff (200 OK)
curl -k -I https://localhost:9010/api/drinks  # API (405 Method Not Allowed - Expected)
```

---

## Architecture
- **StadiumDrinkOrdering.API** → ASP.NET Core Web API with JWT, EF Core, SignalR
- **StadiumDrinkOrdering.Customer** → Blazor Server app for customers
- **StadiumDrinkOrdering.Admin** → Blazor Server app for admins (🔒 **AUTHENTICATION REQUIRED**)
- **StadiumDrinkOrdering.Staff** → Blazor Server app for staff
- **StadiumDrinkOrdering.Shared** → Shared models/DTOs

### Entity Models
Located in `StadiumDrinkOrdering.Shared/Models/`: User, Order, OrderItem, Drink, Ticket, Payment, StadiumSeat, Event, ShoppingCart, CartItem, SeatReservation

### API Controllers
- `AuthController`: login/register
- `OrdersController`: CRUD for orders
- `DrinksController`: catalog management
- `TicketsController`: ticket validation
- `StadiumStructureController`: stadium management and JSON import/export
- `UsersController`: user management with role-based access
- `AnalyticsController`: reporting and analytics data
- `EventController`: event management and activation
- **`CustomerTicketingController`**: customer event browsing and seat availability
- **`CustomerOrdersController`**: ticket purchase and order processing
- **`ShoppingCartController`**: shopping cart management with seat reservations
- **`LogsController`**: centralized logging with batch processing and search capabilities

### Order Workflow
`Pending → Accepted → In Preparation → Ready → Out for Delivery → Delivered`

---

## Centralized Logging System

### Overview
Enterprise-grade centralized logging system that captures, processes, and analyzes logs from all applications (API, Admin, Customer, Staff) in a unified location. Features automatic exception capture, batch processing, and comprehensive audit trails.

### Key Features
- **Automatic Exception Capture**: Global middleware captures all unhandled exceptions
- **High-Performance Batch Processing**: Queues logs and processes in 5-second intervals (10 entries per batch)
- **Real-time Logging**: Critical logs (errors, security events) sent immediately
- **Automated Log Retention**: Background service with configurable retention periods
- **Admin UI Integration**: Searchable logs interface in Admin application 
- **Cross-Application Support**: Works across all .NET projects in the ecosystem
- **PostgreSQL Storage**: All logs stored in PostgreSQL database for enterprise-grade data management

### Architecture Components

#### 1. CentralizedLoggingClient (`StadiumDrinkOrdering.Shared/Services/`)
High-performance logging client with intelligent batching:
```csharp
// Immediate logging for critical events
await _loggingClient.LogErrorAsync(exception, "UserAuthentication", "Security");

// Batched logging for high-volume events  
await _loggingClient.LogUserActionAsync("PageView", "UserAction", userId, userEmail);

// Bulk logging for analytics
await _loggingClient.LogBatchAsync(logEntries);
```

**Features:**
- Timer-based batch processing (5-second intervals)
- Priority handling (errors and security events bypass queue)
- Automatic fallback to individual logging if batch fails
- Thread-safe queue operations with SemaphoreSlim

#### 2. GlobalExceptionMiddleware (`StadiumDrinkOrdering.API/Middleware/`)
Captures all unhandled exceptions across API endpoints:
```csharp
app.UseMiddleware<GlobalExceptionMiddleware>();
```

**Capabilities:**
- Automatic exception logging with full context
- User information extraction from JWT claims
- IP address and user agent capture
- Clean error responses without exposing sensitive details

#### 3. LogRetentionBackgroundService (`StadiumDrinkOrdering.API/Services/`)
Automated maintenance and cleanup:
```csharp
services.AddHostedService<LogRetentionBackgroundService>();
```

**Functions:**
- Daily log cleanup (configurable retention period)
- Critical log archiving before deletion
- Database optimization with PostgreSQL maintenance operations
- Configurable via `LogRetention:RetentionDays` setting

#### 4. Enhanced LogsController (`StadiumDrinkOrdering.API/Controllers/`)
RESTful API for log management:

```http
POST /api/logs/search          # Paginated log search with filters
GET  /api/logs/summary         # Log statistics and analytics
POST /api/logs/log-action      # Single log entry submission
POST /api/logs/log-batch       # Bulk log processing (max 100 entries)
DELETE /api/logs/clear-old     # Manual cleanup (Admin only)
```

### Integration Guide

#### 1. Service Registration
Add to each application's `Program.cs`:
```csharp
// Register centralized logging
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://api:8080";
builder.Services.AddCentralizedLogging(apiBaseUrl, "Customer"); // Source: Customer/Admin/Staff
```

#### 2. Usage Patterns
```csharp
public class OrderController : ControllerBase
{
    private readonly ICentralizedLoggingClient _loggingClient;
    
    public async Task<IActionResult> CreateOrder(CreateOrderDto order)
    {
        try
        {
            // Business logic
            var result = await _orderService.CreateOrderAsync(order);
            
            // Log successful action
            await _loggingClient.LogUserActionAsync(
                action: "CreateOrder",
                category: "UserAction", 
                userId: User.GetUserIdFromClaims(),
                details: $"Order created: {result.OrderId}"
            );
            
            return Ok(result);
        }
        catch (Exception ex)
        {
            // Automatic error logging via GlobalExceptionMiddleware
            // Manual logging for additional context if needed
            await _loggingClient.LogErrorAsync(ex, "CreateOrder", "SystemError");
            throw;
        }
    }
}
```

### Log Categories and Actions

#### Standard Categories
- **`UserAction`**: User-initiated activities (login, purchase, navigation)
- **`SystemError`**: Application exceptions and errors
- **`Security`**: Authentication, authorization, and security events
- **`Performance`**: Timing and performance metrics
- **`Audit`**: Administrative actions and data changes

#### Common Actions
- **Authentication**: `Login`, `Logout`, `TokenRefresh`, `PasswordChange`
- **Orders**: `CreateOrder`, `UpdateOrder`, `CancelOrder`, `ViewOrders`
- **Admin**: `UserManagement`, `SystemConfiguration`, `DataExport`
- **Security**: `UnauthorizedAccess`, `InvalidToken`, `SuspiciousActivity`

### Admin Interface Features

#### Log Search & Filtering (`/admin/logs`)
- **Time Range**: Filter by date/time periods
- **Log Level**: Error, Warning, Info, Debug filtering
- **Category**: Filter by log categories
- **User**: Search by specific users or roles
- **Full-Text Search**: Search in log messages and details
- **Pagination**: Efficient handling of large log volumes

#### Log Analytics Dashboard
- **Real-time Statistics**: Current error rates and system health
- **Trend Analysis**: Historical log patterns and metrics
- **User Activity**: Top users, actions, and usage patterns
- **Error Analysis**: Most frequent errors and affected components
- **System Performance**: Response times and throughput metrics

### Configuration Settings

#### Application Settings
```json
{
  "ApiSettings": {
    "BaseUrl": "https://api:8080"
  },
  "LogRetention": {
    "RetentionDays": 30,
    "ArchiveCriticalLogs": true,
    "EnableVacuum": true
  },
  "CentralizedLogging": {
    "BatchSize": 10,
    "BatchIntervalMs": 5000,
    "MaxRetries": 3,
    "TimeoutMs": 10000
  }
}
```

#### Docker Environment Variables
```yaml
environment:
  - LOG_RETENTION_DAYS=30
  - LOG_BATCH_SIZE=10 
  - LOG_BATCH_INTERVAL=5000
  - ASPNETCORE_ENVIRONMENT=Development
```

### Performance Considerations

#### Batching Strategy
- **Queue Threshold**: Process immediately when queue reaches 10 entries
- **Time Threshold**: Process every 5 seconds regardless of queue size
- **Priority Override**: Errors and security events bypass batching
- **Fallback Mechanism**: Individual logging if batch processing fails

#### Database Optimization
- **Indexed Columns**: Timestamp, UserId, Category, Level for fast queries
- **Retention Management**: Automatic cleanup prevents database bloat
- **PostgreSQL Optimization**: Connection pooling and query optimization
- **Async Operations**: Non-blocking logging operations

#### Memory Management
- **Thread-Safe Queuing**: SemaphoreSlim for concurrent access control
- **Bounded Queues**: Prevent memory leaks from excessive queueing
- **Disposal Pattern**: Proper resource cleanup for timers and semaphores

### Security Features

#### Data Protection
- **Sensitive Data Filtering**: Automatic removal of passwords and tokens from logs
- **PII Compliance**: Configurable anonymization of personal information
- **Access Control**: Role-based access to log viewing and management
- **Audit Trail**: All log access and management actions are logged

#### Network Security
- **HTTPS Only**: Encrypted log transmission in production
- **Authentication**: JWT-based API authentication for log endpoints
- **Rate Limiting**: Protection against log flooding attacks
- **Input Validation**: Comprehensive validation of log entry data

### Monitoring and Alerting

#### Health Checks
- **Log Processing Health**: Monitor batch processing success rates
- **Database Health**: Track log storage and retention operations
- **API Availability**: Ensure logging endpoints are responsive
- **Queue Monitoring**: Track queue depths and processing times

#### Alert Conditions
- **High Error Rates**: Automated alerts for spike in error logs
- **Processing Failures**: Notification when batch processing fails
- **Storage Issues**: Alerts for database storage or retention problems
- **Security Events**: Immediate notifications for security-related logs

---

## Real-time Updates
- **SignalR Hub:** `/bartenderHub`
- Provides order status changes to staff and customers

---

## Service Ports - 🔒 **HTTPS-ONLY MANDATORY** 🔒

🚨 **NO HTTP ALLOWED**: All services run exclusively on HTTPS with SSL certificates. HTTP protocols have been completely removed from the project.

### Development Ports (Local) - **HTTPS ONLY**
- **API**: `7010` (HTTPS) → Docker `9010` (HTTPS) 🔒
- **Customer**: `7020` (HTTPS) → Docker `9020` (HTTPS) 🔒
- **Admin**: `7030` (HTTPS) → Docker `9030` (HTTPS) 🔒
- **Staff**: `7040` (HTTPS) → Docker `9040` (HTTPS) 🔒
- **Database**: PostgreSQL/Supabase (cloud/remote)

### Docker HTTPS Configuration 🔒
- **🔐 SSL Certificates**: Development certificates auto-generated and trusted
- **📁 Certificate Location**: `./certificates/aspnetcore.pfx`
- **🐳 Container Mount**: `/https/aspnetcore.pfx` (read-only)
- **🔑 Certificate Password**: `StadiumDev123!`
- **🔒 HTTPS-ONLY**: NO HTTP protocols - HTTPS exclusively for security

### Access URLs
**Local Development (HTTPS)**
- **API Dev**: https://localhost:7010
- **Customer Dev**: https://localhost:7020
- **Admin Dev**: https://localhost:7030
- **Staff Dev**: https://localhost:7040

**Docker Production (HTTPS)**
- **API Docker**: https://localhost:9010 ✅
- **Customer Docker**: https://localhost:9020 ✅
- **Admin Docker**: https://localhost:9030 ✅  
- **Staff Docker**: https://localhost:9040 ✅

**Docker Development (HTTPS)**
- **API Dev Docker**: https://localhost:5001
- **Customer Dev Docker**: https://localhost:5002
- **Admin Dev Docker**: https://localhost:5003
- **Staff Dev Docker**: https://localhost:5004

### HTTPS Setup Instructions
1. **Generate Certificates**: Run `.\generate-dev-certs.ps1`
2. **Start Services**: `docker-compose up --build -d`  
3. **Verify HTTPS**: Access https://localhost:9030 (Admin)

📋 **Complete Guide**: See `docs/docker-https-setup.md` for detailed HTTPS configuration.

---

## Configuration

### Development & Production
- **Database**: PostgreSQL/Supabase
- **Connection**: Supabase PostgreSQL connection string
- **Configuration**: All environments use PostgreSQL/Supabase for enterprise-grade data management

### Docker / Production
- Configured in `docker-compose.yml`:
  - `ConnectionStrings__DefaultConnection` (PostgreSQL connection string)
  - `JwtSettings__SecretKey` (min 32 chars)
  - `ASPNETCORE_ENVIRONMENT`
  - `TZ=Europe/Zagreb` (Croatia timezone for all containers)

### Frontend Configuration
- Apps connect to API via `ApiSettings__BaseUrl`

---

## Sample Data
- Default drinks: Beer, Soft Drinks, Water, Coffee
- Test tickets: `TICKET001`–`TICKET005`

---

## Stadium Structure Management

### Overview
Complete stadium structure management system with JSON import/export capabilities, comprehensive validation, and interactive documentation. The system supports multi-level stadium hierarchies with tribunes, rings, sectors, and automated seat generation.

### Access Points
- **Admin Panel:** `/admin/stadium-structure` - Main structure management interface
- **Help Guide:** `/admin/structure-help` - Comprehensive import documentation with samples
- **Navigation:** Admin → Stadium Management → Structure Management

### Stadium Hierarchy
```
STADIUM (Root)
├── TRIBUNES (N, S, E, W)
│   └── RINGS (1-5 levels)
│       └── SECTORS (A-Z codes)
│           └── SEATS (Auto-generated: rows × seatsPerRow)
```

**Seat Naming Convention:** `[Tribune][Ring][Sector]-R[Row]S[Seat]`  
Example: `N1A-R5S12` = North Tribune, Ring 1, Sector A, Row 5, Seat 12

### JSON Import/Export

#### Import Process
1. **Access:** Navigate to Admin → Structure Management
2. **Help:** Click "Import Guide & Samples" for detailed instructions
3. **Upload:** Select JSON file (max 10MB)
4. **Validate:** System automatically validates structure
5. **Import:** System creates tribunes, rings, sectors, and seats
6. **Verify:** Review structure summary and seat counts

#### Complete JSON Schema
```json
{
  "name": "Stadium Name",           // Required: Display name (3-100 chars)
  "description": "Description",     // Optional: Stadium description (max 500 chars)
  "capacity": 50000,                // Optional: Total capacity (auto-calculated)
  "tribunes": [                     // Required: Array of 1-4 tribunes
    {
      "code": "N",                  // Required: N, S, E, or W only
      "name": "North Tribune",      // Required: Display name (3-100 chars)
      "description": "Main stand",  // Optional: Tribune description
      "rings": [                     // Required: Array of 1-5 rings
        {
          "number": 1,               // Required: Ring level (1-5)
          "name": "Lower Ring",      // Required: Display name
          "priceMultiplier": 1.0,   // Optional: Price adjustment (0.5-3.0)
          "sectors": [               // Required: Array of sectors
            {
              "code": "NA",          // Required: Unique code (2-10 chars)
              "name": "Sector A",    // Required: Display name
              "type": "standard",    // Optional: standard/vip/wheelchair
              "rows": 25,            // Required: Number of rows (1-100)
              "seatsPerRow": 20,     // Required: Seats per row (1-100)
              "startRow": 1,         // Optional: First row number (default: 1)
              "startSeat": 1,        // Optional: First seat number (default: 1)
              "priceCategory": "A"   // Optional: Pricing category
            }
          ]
        }
      ]
    }
  ],
  "metadata": {                     // Optional: Additional information
    "version": "1.0",
    "createdDate": "2024-01-01",
    "lastModified": "2024-01-01"
  }
}
```

#### Sample Files
Pre-built template files available in `stadium-samples/`:
- **`minimal-stadium.json`** - Basic single-tribune structure (200 seats)
- **`standard-stadium.json`** - Four-tribune professional stadium (20,000 seats)
- **`complex-stadium.json`** - Multi-ring with VIP sections (80,000 seats)

#### Validation Rules
**Required Fields:**
- `name`: Stadium name (3-100 characters)
- `tribunes[]`: At least one tribune
- `tribune.code`: Must be N, S, E, or W (unique)
- `tribune.rings[]`: At least one ring per tribune
- `ring.number`: Integer 1-5 (sequential recommended)
- `ring.sectors[]`: At least one sector per ring
- `sector.code`: Unique across entire stadium (2-10 chars)
- `sector.rows` & `sector.seatsPerRow`: Both required (1-100 each)

**Constraints:**
- Maximum 4 tribunes, 5 rings per tribune, unlimited sectors
- Tribune codes must be unique (can't have two "N" tribunes)
- Sector codes must be unique across entire stadium
- File size limit: 10MB
- Ring numbers should be sequential for best practices

#### Common Import Errors & Solutions
| Error | Cause | Solution |
|-------|-------|----------|
| "Stadium name is required" | Missing/empty name | Add `"name": "Your Stadium"` |
| "Invalid tribune code" | Wrong code format | Use only N, S, E, or W |
| "Duplicate sector code" | Same code used twice | Make all sector codes unique |
| "Invalid JSON format" | Syntax errors | Validate JSON with online tools |
| "File too large" | File > 10MB | Reduce sectors or split import |

### Features
- **Interactive Help System:** Built-in documentation with visual guides
- **JSON Validator Tool:** Real-time validation with error reporting
- **Sample Downloads:** One-click download of template files
- **Automatic Seat Generation:** Creates all seats based on row/seat specifications
- **Database Transaction Safety:** Rollback on errors, maintains data integrity
- **Export Functionality:** Download existing structure as JSON
- **Clear Structure:** Safe deletion with confirmation dialogs
- **Real-time Feedback:** Progress indicators and detailed success/error messages

### Technical Implementation
- **Backend API:** `/api/stadium-structure` endpoints for CRUD operations
- **Admin Service:** `IAdminApiService` handles all structure operations
- **Validation:** Server-side validation with detailed error messages
- **File Processing:** Stream-based JSON processing with 10MB limit
- **Database:** Entity Framework Core with PostgreSQL/Supabase
- **UI Components:** Blazor Server with Bootstrap styling
- **Error Handling:** Global exception handling with user-friendly messages

---

## Stadium Overview (Admin)

### Overview
Visual stadium management interface providing administrators with real-time stadium visualization, occupancy monitoring, and event management capabilities. Combines SVG-based stadium rendering with administrative controls for comprehensive stadium oversight.

### Access Points
- **Admin Panel:** `/admin/stadium-overview` - Main visual stadium interface
- **Navigation:** Admin → Stadium Management → Stadium Overview

### Key Features

#### Visual Stadium Map
- **SVG Visualization**: Interactive stadium map with clickable sectors
- **Real-time Rendering**: Dynamic stadium layout based on imported structure
- **Color-coded Occupancy**: Visual representation of seat availability and sales
- **Responsive Design**: Scales to different screen sizes and devices

#### Event Management Integration
- **Event Selection**: Dropdown to select active events for occupancy view
- **Real-time Data**: Live seat status updates from database
- **Ticket Sales Simulation**: Admin tool to simulate ticket sales for testing
- **Occupancy Analytics**: Visual percentage display for each sector

#### Interactive Controls
- **Seat Search**: Find specific seats using stadium naming convention
- **Legend Toggle**: Show/hide color coding legend for occupancy status
- **Sector Tooltips**: Hover information showing seat counts and availability
- **Detail Modals**: Click sectors to view detailed seat layouts

#### Administrative Functions
- **Sales Simulation**: Generate test ticket sales data for events
- **Occupancy Monitoring**: Real-time visualization of seat utilization
- **Event Analytics**: Visual representation of event performance
- **Stadium Health Check**: Verify stadium structure integrity

### Color Coding System
- **🟢 Green (Available)**: 0-49% occupied - plenty of seats available
- **🟡 Orange (Partial)**: 50-89% occupied - limited availability
- **🔴 Red (Full)**: 90-100% occupied - nearly or completely sold out
- **⚫ Gray (No Event)**: No event selected - neutral state

### Integration Points
- **Stadium Structure**: Uses imported JSON structure for rendering
- **Event System**: Integrates with event management for occupancy data
- **Ticketing System**: Displays real ticket sales and reservations
- **Analytics**: Provides visual data for administrative reporting

### Technical Implementation
- **SVG Rendering**: Dynamic SVG generation based on stadium coordinates
- **Real-time Updates**: SignalR integration for live occupancy changes
- **Canvas Integration**: HTML5 Canvas for detailed seat-level visualization
- **API Integration**: RESTful endpoints for stadium viewer data
- **Responsive UI**: Bootstrap-based responsive design

### API Endpoints
```
GET  /api/stadium-viewer/overview           # Stadium structure with coordinates
GET  /api/stadium-viewer/event/{id}/status  # Event-specific seat status
POST /api/stadium-viewer/search-seat        # Find seat by code
GET  /api/stadium-viewer/sector/{id}/seats  # Detailed sector seat layout
```

### Usage Workflow
1. **Stadium Setup**: Import stadium structure via Structure Management
2. **Event Creation**: Create events through Event Management
3. **Visual Monitoring**: Use Stadium Overview to monitor occupancy
4. **Sales Analysis**: Utilize color coding and tooltips for insights
5. **Testing**: Use sales simulation for system validation

---

## Customer Ticketing System

### Overview
Complete end-to-end ticket booking system allowing customers to browse events, select seats, and purchase tickets with a professional checkout experience.

### Customer Journey Flow
```
Browse Events → Select Event → Choose Seats → Add to Cart → Checkout → Payment → Confirmation
```

### Key Features

#### 1. Event Discovery (`/events`)
- **Event Filtering**: Date range, event type, price range filters
- **Event Cards**: Show availability, pricing, and venue information
- **Responsive Design**: Mobile-optimized event browsing

#### 2. Event Details & Seat Selection (`/event-details/{eventId}`)
- **Event Information**: Complete event details with pricing tiers
- **Stadium Section Overview**: Visual representation of stadium sections
- **Seat Selection Modal**: Interactive seat picker with real-time availability
- **Multi-seat Selection**: Select multiple seats with price calculation
- **Cart Integration**: Add selected seats to shopping cart

#### 3. Shopping Cart System
- **Session-based Carts**: Guest checkout without registration required
- **Seat Reservations**: 15-minute timeout to hold selected seats
- **Real-time Updates**: Automatic cart expiration and seat release
- **Price Calculation**: Dynamic pricing with service fees

#### 4. Checkout Process (`/checkout`)
- **Customer Information**: Name, email, phone collection
- **Payment Form**: Credit card details with validation
- **Order Summary**: Complete breakdown of costs and seat details
- **Terms & Conditions**: Legal agreement requirements

#### 5. Order Confirmation (`/order-confirmation/{orderId}`)
- **Order Details**: Complete purchase confirmation
- **Ticket Display**: Individual tickets with QR codes
- **Action Buttons**: Print, download, email options
- **Customer Support**: Help and contact information

### API Endpoints

#### Customer Ticketing API (`/api/customer/ticketing/`)
```
GET  /events                              # Browse available events
GET  /events/{eventId}                    # Get event details
GET  /events/{eventId}/sections/{sectionId}/availability  # Get seat availability
```

#### Shopping Cart API (`/api/customer/cart/`)
```
GET    ?sessionId={sessionId}             # Get cart contents
POST   /add                               # Add seat to cart
DELETE /remove                            # Remove seat from cart
DELETE /clear?sessionId={sessionId}       # Clear entire cart
GET    /summary?sessionId={sessionId}     # Get cart summary
GET    /seat-availability                 # Check individual seat availability
```

#### Customer Orders API (`/api/customer/orders/`)
```
POST /create                              # Process ticket order
GET  /{orderId}/confirmation              # Get order confirmation
GET  /my-orders?email={email}             # Get customer order history
```

### Database Schema Extensions

#### New Tables
- **Events**: Event management with dates, pricing, and capacity
- **ShoppingCarts**: Session-based cart storage
- **CartItems**: Individual cart line items with seat details
- **SeatReservations**: Temporary seat holds with expiration
- **Tickets**: Generated tickets with QR codes and validation

#### Key Relationships
- `Event` → `Tickets` (one-to-many)
- `ShoppingCart` → `CartItems` (one-to-many)
- `Order` → `Tickets` (one-to-many)
- `CartItem` → `Event` + `Sector` (seat mapping)

### Services Architecture

#### Core Services
- **`ShoppingCartService`**: Cart management with automatic cleanup
- **`SeatMappingService`**: Stadium structure to seat mapping
- **`CustomerTicketingController`**: Event and availability APIs
- **`CustomerOrdersController`**: Order processing and confirmation

#### Payment Processing
- **Mock Implementation**: Simulated payment processing for development
- **Extensible Design**: Ready for Stripe/PayPal integration
- **Transaction Safety**: Database transactions for order integrity

### Customer Pages

#### Blazor Components
```
/events                    # Event browsing with filters
/event-details/{eventId}   # Event details with seat selection
/checkout                  # Customer information and payment
/order-confirmation/{orderId} # Purchase confirmation
```

### Security & Validation
- **Input Validation**: Comprehensive form validation with data annotations
- **Seat Availability**: Real-time validation to prevent double-booking
- **Session Management**: Secure session-based cart handling
- **Error Handling**: User-friendly error messages and recovery

### Mobile Responsiveness
- **Bootstrap Integration**: Mobile-first responsive design
- **Touch-friendly**: Optimized for mobile seat selection
- **Adaptive Layout**: Stadium map adapts to screen size
- **Print Support**: Print-friendly ticket display

---

## Multilingual Support (i18n)

### Overview
Complete internationalization implementation supporting **English (en)** and **Croatian (hr)** languages across all three applications (Customer, Admin, Staff) using .NET's built-in localization framework.

### Architecture
- **Technology**: ASP.NET Core's `IStringLocalizer<T>` with `.resx` resource files
- **Default Language**: Croatian (hr)
- **Supported Languages**: English (en), Croatian (hr)
- **Culture Persistence**: Cookie-based using `.AspNetCore.Culture`

### Resource File Structure
```
Application/
├── Resources/
│   ├── SharedResources.en.resx     # Common English translations
│   ├── SharedResources.hr.resx     # Common Croatian translations
│   ├── SharedResources.cs          # Marker class for resources
│   └── Pages/
│       ├── IndexResources.en.resx  # Page-specific English
│       ├── IndexResources.hr.resx  # Page-specific Croatian
│       └── IndexResources.cs       # Page resource marker
```

### Implementation Components

#### 1. Language Switcher Component
Located in `Shared/LanguageSwitcher.razor`:
- Dropdown selector in top navigation bar
- Persists selection via cookies
- Forces page reload to apply new culture

#### 2. Culture Persistence
JavaScript helper in `wwwroot/js/culture.js`:
- Manages `.AspNetCore.Culture` cookie
- Supports localStorage fallback
- Maintains selection across sessions

#### 3. Localization Configuration
In `Program.cs` for each app:
```csharp
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options => {
    var supportedCultures = new[] { "en", "hr" };
    options.DefaultRequestCulture = new RequestCulture("hr");
    // ... culture configuration
});
```

### Usage in Razor Components

#### Basic Text Localization
```razor
@using Microsoft.Extensions.Localization
@using YourApp.Resources.Pages
@inject IStringLocalizer<IndexResources> Localizer

<h1>@Localizer["WelcomeTitle"]</h1>
<p>@Localizer["WelcomeSubtitle"]</p>
```

#### Shared Resources
```razor
@inject IStringLocalizer<SharedResources> Shared

<button>@Shared["Login"]</button>
<span>@Shared["Loading"]</span>
```

#### Dynamic Text with Parameters
```razor
@* In resource file: "ItemCount" = "You have {0} items" *@
<p>@Localizer["ItemCount", cartItems.Count]</p>
```

### Resource Keys Convention
- **Page Titles**: `PageTitle`, `SectionTitle`
- **Buttons**: `SubmitButton`, `CancelButton`, `SaveButton`
- **Messages**: `SuccessMessage`, `ErrorMessage`, `ValidationMessage`
- **Labels**: `EmailLabel`, `PasswordLabel`, `NameLabel`
- **Navigation**: `Home`, `Menu`, `Orders`, `Events`
- **Status**: `OrderStatus_Pending`, `OrderStatus_Delivered`

### Adding New Translations

#### 1. Create Resource Files
For new page `MyPage.razor`:
1. Create `Resources/Pages/MyPageResources.cs` (marker class)
2. Create `Resources/Pages/MyPageResources.en.resx`
3. Create `Resources/Pages/MyPageResources.hr.resx`

#### 2. Add Translations
In `.resx` files, add key-value pairs:
```xml
<data name="Title" xml:space="preserve">
  <value>My Page Title</value>
</data>
```

#### 3. Use in Component
```razor
@inject IStringLocalizer<MyPageResources> Localizer
<h1>@Localizer["Title"]</h1>
```

### Testing Localization

#### Manual Testing
1. Run application: `dotnet run`
2. Default language should be Croatian
3. Use language switcher to change to English
4. Verify all text updates correctly
5. Refresh page - language preference should persist

#### Automated Testing
Run Playwright tests: `npx playwright test localization.spec.ts`

Tests verify:
- Language switcher functionality
- Text translation accuracy
- Cookie persistence
- Multi-user language preferences
- All three applications (Customer, Admin, Staff)

### Common Resource Keys Reference

#### Shared Actions
- `Login` / `Logout` / `Register`
- `Submit` / `Cancel` / `Save` / `Delete`
- `Add` / `Edit` / `Search` / `Filter`

#### Navigation
- `Home` / `Menu` / `Orders` / `Events`
- `Dashboard` / `Profile` / `Settings`

#### Messages
- `Loading` / `PleaseWait`
- `Success` / `Failed` / `ErrorOccurred`
- `NoDataAvailable`

#### Order Statuses
- `OrderStatus_Pending` → "Pending" / "Na čekanju"
- `OrderStatus_InPreparation` → "In Preparation" / "U pripremi"
- `OrderStatus_Ready` → "Ready" / "Spremno"
- `OrderStatus_Delivered` → "Delivered" / "Dostavljeno"

### Troubleshooting

#### Issue: Text not updating after language change
**Solution**: Ensure page reload after culture change:
```javascript
NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
```

#### Issue: Resource not found exception
**Solution**: Verify:
1. Resource file naming matches class name
2. Namespace is correct in marker class
3. Build action is set to "Embedded Resource"

#### Issue: Cookie not persisting
**Solution**: Check cookie configuration:
```csharp
options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
```

---

## UI/UX Improvements (Latest Updates)

### Navigation Bar Enhancement
**Update:** Login and Sign-up buttons moved to top-right corner in Customer app
- Removed authentication buttons from sidebar
- Added user dropdown menu in MainLayout.razor
- Implemented responsive dropdown with profile options for authenticated users
- Clean sign-out functionality with proper state management

### Responsive Form Layouts
**Update:** Fixed narrow column display issues across all authentication pages
- **Previous Issue:** Forms used `col-md-6` (50% width) which was too narrow
- **Solution:** Implemented responsive column classes:
  - `col-12`: Full width on mobile (<576px)
  - `col-sm-10`: 83% width on small tablets (≥576px)
  - `col-md-8`: 66% width on medium screens (≥768px)  
  - `col-lg-6`: 50% width on large screens (≥992px)
  - `col-xl-4`: 33% width on extra-large screens (≥1200px)
- **Applied to:** Login and Registration pages in Customer, Admin, and Staff projects

### Custom Authentication Styling
**Update:** Created dedicated auth.css files for each project
- **Customer auth.css:**
  - Blue gradient headers (#2563eb to #1d4ed8)
  - Rounded corners (12px) with shadow effects
  - Enhanced form controls with focus states
  - Animated hover effects on buttons
- **Admin auth.css:**
  - Dark gradient headers (#1e293b to #0f172a)
  - Professional styling for administrative interface
- **Staff auth.css:**
  - Green gradient headers (#10b981 to #059669)
  - Mobile-optimized for on-the-go staff usage

### File Structure Updates
- Added `wwwroot/css/auth.css` to all three projects
- Updated `_Layout.cshtml` (Customer/Admin) to include auth.css
- Updated `_Host.cshtml` (Staff) to include auth.css
- Modified MainLayout.razor and NavMenu.razor in Customer project

## Known Issues & Fixes

### PostgreSQL DateTime UTC Issue (FIXED ✅)
**Issue:** Admin logs page failed with error: "Cannot write DateTime with Kind=Local to PostgreSQL type 'timestamp with time zone', only UTC is supported."
**Root Cause:** PostgreSQL requires UTC DateTime values for timestamp with time zone columns, but the code was using `DateTime.Now` (local time).
**Solution:** Changed all date filtering code to use `DateTime.UtcNow` instead of `DateTime.Now`. The dates are stored as UTC in the database and converted to local time for display.

### Docker Container SSL Communication (FIXED ✅)
**Issue:** Admin, Customer, and Staff containers failed to connect to API with SSL errors: "The SSL connection could not be established" and "Cannot determine the frame size or a corrupted frame was received."
**Root Cause:** Protocol mismatch - applications had hardcoded `https://api:8080/` URLs in Program.cs when detecting Docker environment, but API was configured for HTTP-only (`ASPNETCORE_URLS=http://+:8080`).
**Solution:** Changed all hardcoded HTTPS references to HTTP in container environment detection logic:
- Updated Admin, Customer, and Staff Program.cs files
- Fixed SignalRService.cs fallback URLs
- Container-to-container communication now uses HTTP (internal)
- External access via Docker ports (9010, 9020, 9030, 9040) works correctly

### Authentication Token Persistence
**Issue:** Admin API service token was always null after page navigation.
**Root Cause:** AdminApiService was scoped, creating new instances per request.
**Solution:** Implemented `TokenStorageService` as singleton to persist tokens across scoped instances.

### File Upload Issues  
**Issue:** Imported JSON files had length=0 causing validation errors.
**Root Cause:** Stream position wasn't reset before HTTP upload.
**Solution:** Added `fileStream.Position = 0` before multipart form submission.

### JSON Deserialization
**Issue:** "Stadium name is required" errors during import of valid JSON.
**Root Cause:** JSON had camelCase properties but deserializer expected PascalCase.
**Solution:** Added `PropertyNamingPolicy.CamelCase` to JsonSerializer options.

### Customer Ticketing Implementation Issues

#### CSS Media Query Compilation Error
**Issue:** `@media` queries in Razor files caused compilation errors.
**Root Cause:** Blazor interprets `@` symbol as Razor syntax.
**Solution:** Escaped media queries as `@@media` in Razor files.

#### DataAnnotations Missing Reference
**Issue:** Using statement for `System.ComponentModel.DataAnnotations` placed incorrectly caused compilation errors.
**Root Cause:** Using statements must precede namespace declarations.
**Solution:** Moved using statement to proper location at top of file.

#### CartItem Type Conflicts
**Issue:** Multiple `CartItem` classes in different namespaces caused ambiguous references.
**Root Cause:** Naming conflicts between existing and new cart implementations.
**Solution:** Used fully qualified type names `StladiumDrinkOrdering.Shared.Models.CartItem` where needed.

#### Event Date Filter Issues
**Issue:** No events returned because default event dates were in the past.
**Root Cause:** Event date filtering excluded past events by default.
**Solution:** Modified query to use `IsActive` flag for testing, allowing past-dated events.

#### Seat Availability Race Conditions
**Issue:** Multiple users could select the same seats simultaneously.
**Root Cause:** Lack of real-time seat locking mechanism.
**Solution:** Implemented 15-minute seat reservation system with automatic timeout cleanup.

---

## UI Element Identification System

### Overview
All buttons, main divs, and interactive elements across the Customer and Admin applications have standardized ID attributes for easy automation, testing, and debugging. This comprehensive ID system ensures consistent element identification across all pages and components throughout the entire Stadium Drink Ordering system.

### ID Naming Convention
All IDs follow the pattern: `{application}-{page/component}-{element-type}-{specific-identifier}`

**Formats:**
- **Customer App**: `customer-{context}-{element}-{suffix}`
- **Admin App**: `admin-{context}-{element}-{suffix}`
- **Staff App**: `staff-{context}-{element}-{suffix}`

**Parameters:**
- **Application**: `customer`, `admin`, or `staff` (identifies the specific app)
- **Context**: Page name, component name, or section identifier
- **Element**: Button (`btn`), container (`container`), form (`form`), input (`input`), etc.
- **Suffix**: Specific descriptor or action

### Main Layout & Navigation IDs

#### MainLayout.razor
- `customer-layout-page` - Main page container div
- `customer-layout-sidebar` - Sidebar navigation container
- `customer-layout-main` - Main content area
- `customer-layout-top-row` - Top navigation row
- `customer-layout-content` - Article content area
- `customer-layout-about-link` - Help link
- `customer-layout-sign-in-btn` - Sign In button (unauthenticated)
- `customer-layout-sign-up-btn` - Sign Up button (unauthenticated)
- `customer-layout-user-dropdown` - User dropdown container (authenticated)
- `customer-layout-profile-link` - My Profile dropdown link
- `customer-layout-orders-link` - My Orders dropdown link
- `customer-layout-logout-btn` - Sign Out button

#### NavMenu.razor
- `customer-nav-top-row` - Top navigation bar
- `customer-nav-container` - Navigation container
- `customer-nav-brand` - Brand/logo link
- `customer-nav-menu-toggle-btn` - Mobile menu toggle button
- `customer-nav-menu` - Navigation menu container
- `customer-nav-items` - Navigation items list
- `customer-nav-home` - Home navigation item
- `customer-nav-home-link` - Home navigation link
- `customer-nav-events` - Events navigation item
- `customer-nav-events-link` - Events navigation link
- `customer-nav-menu-item` - Menu navigation item
- `customer-nav-menu-link` - Menu navigation link
- `customer-nav-orders` - Orders navigation item (authenticated)
- `customer-nav-orders-link` - Orders navigation link (authenticated)
- `customer-nav-logs` - Activity navigation item (authenticated)
- `customer-nav-logs-link` - Activity navigation link (authenticated)

### Page-Specific IDs

#### Index Page (Homepage)
- `customer-index-hero` - Hero section container
- `customer-index-hero-container` - Hero content container
- `customer-index-welcome-title` - Main welcome title
- `customer-index-welcome-subtitle` - Welcome subtitle
- `customer-index-order-drinks-btn` - Order Drinks CTA button
- `customer-index-my-orders-btn` - My Orders CTA button
- `customer-index-features-container` - Features section container
- `customer-index-features-row` - Features row
- `customer-index-feature-1` - First feature card (Wide Selection)
- `customer-index-feature-2` - Second feature card (Fast Delivery)
- `customer-index-feature-3` - Third feature card (Easy Ordering)
- `customer-index-how-it-works-row` - How It Works section row
- `customer-index-how-it-works` - How It Works container
- `customer-index-how-it-works-title` - How It Works title
- `customer-index-steps-list` - Steps list container

#### Login Page
- `customer-login-container` - Main login container
- `customer-login-row` - Bootstrap row wrapper
- `customer-login-col` - Bootstrap column wrapper
- `customer-login-card` - Login form card
- `customer-login-header` - Card header
- `customer-login-title` - Login title
- `customer-login-body` - Card body
- `customer-login-error` - Error message alert
- `customer-login-form` - Login form
- `customer-login-email-group` - Email form group
- `customer-login-email-input` - Email input field
- `customer-login-password-group` - Password form group
- `customer-login-password-input` - Password input field
- `customer-login-remember-group` - Remember me checkbox group
- `customer-login-remember-input` - Remember me checkbox
- `customer-login-button-group` - Submit button container
- `customer-login-submit-btn` - Login submit button
- `customer-login-spinner` - Loading spinner
- `customer-login-loading-text` - Loading text span
- `customer-login-button-text` - Button text span
- `customer-login-register-section` - Register link section
- `customer-login-register-link` - Register link button
- `customer-login-demo-info` - Demo credentials section
- `customer-login-demo-text` - Demo credentials text

#### Register Page
- `customer-register-container` - Main register container
- `customer-register-row` - Bootstrap row wrapper
- `customer-register-col` - Bootstrap column wrapper
- `customer-register-card` - Register form card
- `customer-register-header` - Card header
- `customer-register-title` - Register title
- `customer-register-body` - Card body
- `customer-register-error` - Error message alert
- `customer-register-success` - Success message alert
- `customer-register-form` - Register form
- `customer-register-username-group` - Username form group
- `customer-register-username-input` - Username input field
- `customer-register-email-group` - Email form group
- `customer-register-email-input` - Email input field
- `customer-register-password-group` - Password form group
- `customer-register-password-input` - Password input field
- `customer-register-confirm-password-group` - Confirm password form group
- `customer-register-confirm-password-input` - Confirm password input field
- `customer-register-terms-group` - Terms checkbox group
- `customer-register-terms-input` - Terms checkbox
- `customer-register-button-group` - Submit button container
- `customer-register-submit-btn` - Register submit button
- `customer-register-spinner` - Loading spinner
- `customer-register-loading-text` - Loading text span
- `customer-register-button-text` - Button text span
- `customer-register-login-section` - Login link section
- `customer-register-login-link` - Login link button

#### Events Page
- `customer-events-main-container` - Main events container
- `customer-events-title` - Events page title
- `customer-events-filters-card` - Filters card container
- `customer-events-filters-title` - Filters card title
- `customer-events-type-filter` - Event type filter dropdown
- `customer-events-date-from` - From date filter input
- `customer-events-date-to` - To date filter input
- `customer-events-min-price` - Minimum price filter input
- `customer-events-max-price` - Maximum price filter input
- `customer-events-apply-filters-btn` - Apply filters button
- `customer-events-clear-filters-btn` - Clear filters button
- `customer-events-loading` - Loading state container
- `customer-events-no-results` - No results alert
- `customer-events-list` - Events list container
- `customer-event-card-{id}` - Individual event card (where {id} is the event ID)
- `customer-events-buy-tickets-btn-{id}` - Buy tickets button for specific event
- `customer-events-sold-out-btn-{id}` - Sold out button for specific event

#### Admin Events Page
- `admin-events-main-container` - Main events container
- `admin-events-header` - Page header with title and create button
- `admin-events-title` - Events page title
- `admin-events-create-btn` - Create new event button
- `admin-events-loading` - Loading state container
- `admin-events-error` - Error state container
- `admin-events-retry-btn` - Retry loading button
- `admin-events-empty` - No events found container
- `admin-events-first-event-btn` - Create first event button
- `admin-events-list` - Events list container
- `admin-event-col-{id}` - Individual event column (where {id} is the event ID)
- `admin-event-card-{id}` - Individual event card
- `admin-event-header-{id}` - Event card header
- `admin-event-header-content-{id}` - Event card header content
- `admin-event-name-{id}` - Event name title
- `admin-event-active-badge-{id}` - Active status badge
- `admin-event-inactive-badge-{id}` - Inactive status badge
- `admin-event-body-{id}` - Event card body
- `admin-event-footer-{id}` - Event card footer
- `admin-event-buttons-{id}` - Event action buttons group
- `admin-event-edit-btn-{id}` - Edit event button
- `admin-event-activate-btn-{id}` - Activate event button
- `admin-event-deactivate-btn-{id}` - Deactivate event button
- `admin-event-delete-btn-{id}` - Delete event button

#### Admin Event Modal (Create/Edit)
- `admin-event-modal` - Modal container
- `admin-event-modal-dialog` - Modal dialog
- `admin-event-modal-content` - Modal content
- `admin-event-modal-header` - Modal header
- `admin-event-modal-title` - Modal title
- `admin-event-modal-close` - Modal close button
- `admin-event-modal-body` - Modal body
- `admin-event-form-name` - Event name input
- `admin-event-form-location` - Event location input
- `admin-event-form-description` - Event description textarea
- `admin-event-form-date` - Event date input
- `admin-event-form-capacity` - Event capacity input
- `admin-event-form-price` - Event base price input
- `admin-event-form-active` - Event active status checkbox
- `admin-event-modal-footer` - Modal footer
- `admin-event-modal-cancel-btn` - Modal cancel button
- `admin-event-modal-save-btn` - Modal save button

#### Admin Event Alerts
- `admin-event-alert-container` - Alert notification container
- `admin-event-alert` - Alert message
- `admin-event-alert-close` - Alert close button

### Authentication & Form IDs
All authentication forms (Login, Register, Profile) follow consistent patterns:
- Container: `{page}-container`
- Card: `{page}-card`
- Form: `{page}-form`
- Inputs: `{page}-{field}-input`
- Buttons: `{page}-{action}-btn`
- Messages: `{page}-{type}` (error, success, loading)

### Dynamic IDs
Some elements use dynamic IDs with placeholders:
- Event cards: `customer-event-card-{eventId}`
- Event actions: `customer-events-{action}-btn-{eventId}`
- Order items: `customer-order-item-{orderId}`
- Ticket displays: `customer-ticket-{ticketId}`

### Testing & Automation Usage

#### Playwright Example
```javascript
// Navigate to events and apply filters
await page.goto('/events');
await page.click('#customer-events-type-filter');
await page.selectOption('#customer-events-type-filter', 'Football');
await page.click('#customer-events-apply-filters-btn');

// Buy tickets for first available event
await page.click('#customer-events-buy-tickets-btn-1');
```

#### CSS Selector Examples
```css
/* All customer app buttons */
[id^="customer-"][id$="-btn"] { }

/* All form inputs */
[id^="customer-"][id*="-input"] { }

/* All main page containers */
[id^="customer-"][id*="-container"] { }
```

### Best Practices
1. **Consistent Naming**: Always follow the `customer-{context}-{element}-{suffix}` pattern
2. **Descriptive Suffixes**: Use clear, action-oriented suffixes (`-btn`, `-input`, `-container`)
3. **Unique IDs**: Ensure all IDs are unique across the entire application
4. **Dynamic IDs**: Include entity IDs for dynamic content (`-{eventId}`, `-{orderId}`)
5. **Semantic Structure**: Group related elements with consistent prefixes

### Maintenance
When adding new UI elements:
1. Follow the established naming convention
2. Add IDs to all interactive elements (buttons, inputs, links)
3. Add IDs to main structural divs and containers
4. Update this documentation with new ID patterns
5. Test automation scenarios with new IDs

---

## Claude MCP Setup
This project integrates **Claude Code MCP servers** for docs and automation:

```jsonc
{
  "mcpServers": {
    "context7": {
      "type": "sse",
      "url": "https://mcp.context7.com/sse"
      // To avoid rate limits, add an API key:
      // "headers": { "CONTEXT7_API_KEY": "sk-xxxx" }
    },
    "playwright": {
      "command": "npx",
      "args": ["-y", "@playwright/mcp@latest"]
    }
  }
}
```

### Usage Examples
- **Docs (Context7):**  
  `use context7 → How do I secure a Blazor Server app with JWT?`

- **Browser Automation (Playwright):**  
  `use playwright → Open https://localhost:7020, log in as a customer, and place an order.`
