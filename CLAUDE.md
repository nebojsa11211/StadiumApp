# CLAUDE.md

## Project Overview
**Stadium Drink Ordering System** – A microservices-based **.NET 8.0** application for ordering beverages in a stadium environment. It consists of:

- **API Backend** (ASP.NET Core Web API)
- **Customer Blazor Server App**
- **Admin Blazor Server App**
- **Staff Blazor Server App**

### Key Features
- PostgreSQL/Supabase database for all environments
- JWT authentication with role-based access (Admin, Staff, Customer)
- SignalR (BartenderHub) for real-time updates
- Dockerized services with docker-compose orchestration
- Entity Framework Core with migrations support
- Stadium structure management with JSON import/export
- Interactive stadium seating map visualization
- **Complete Customer Ticketing System** with event browsing, seat selection, and checkout
- Real-time shopping cart with seat reservations and timeout handling
- Comprehensive order processing with payment simulation and ticket generation

---

## Development Commands

### Docker Development (Recommended)
- **Start services:** `./scripts/start-dev.sh` (Linux/macOS) or `docker-compose up --build -d`
- **Stop services:** `docker-compose down`
- **View logs:** `docker-compose logs -f [service-name]`
- **Restart service:** `docker-compose restart [service-name]`

### Local .NET Development
- **Build solution:** `dotnet build StadiumDrinkOrdering.sln`
- **Run API:** `cd StadiumDrinkOrdering.API && dotnet run --urls "https://localhost:7000;http://localhost:7001"`
- **Run Customer App:** `cd StadiumDrinkOrdering.Customer && dotnet run --urls "https://localhost:7002;http://localhost:7003"`
- **Run Admin App:** `cd StadiumDrinkOrdering.Admin && dotnet run --urls "https://localhost:7004;http://localhost:7005"`
- **Run Staff App:** `cd StadiumDrinkOrdering.Staff && dotnet run --urls "https://localhost:7006;http://localhost:7007"`

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

## Architecture
- **StadiumDrinkOrdering.API** → ASP.NET Core Web API with JWT, EF Core, SignalR
- **StadiumDrinkOrdering.Customer** → Blazor Server app for customers
- **StadiumDrinkOrdering.Admin** → Blazor Server app for admins
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
var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "http://api:8080";
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
    "BaseUrl": "http://api:8080"
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

## Service Ports
- API: Dev 7000/7001 → Docker 9000
- Customer: Dev 7002/7003 → Docker 9001
- Admin: Dev 7004/7005 → Docker 9002
- Staff: Dev 7006/7007 → Docker 9003
- Database: PostgreSQL/Supabase (cloud/remote)

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

### Frontend Configuration
- Apps connect to API via `ApiSettings__BaseUrl`

---

## Sample Data
- Default drinks: Beer, Soft Drinks, Water, Coffee
- Test tickets: `TICKET001`–`TICKET005`

---

## Stadium Structure Management

### JSON Import/Export
The system supports importing and exporting stadium structures via JSON files:

**Import Process:**
1. Navigate to Admin → Structure Management
2. Select JSON file with stadium structure (see `stadium-structure.json` example)
3. Click Import Structure (replaces existing structure)
4. System validates and creates tribunes, rings, sectors, and seats

**JSON Structure:**
```json
{
  "name": "Stadium Name",
  "description": "Description",
  "tribunes": [
    {
      "code": "N|S|E|W",
      "name": "Tribune Name", 
      "description": "Description",
      "rings": [
        {
          "number": 1,
          "name": "Ring Name",
          "sectors": [
            {
              "code": "SECTOR_CODE",
              "name": "Sector Name",
              "rows": 25,
              "seatsPerRow": 20,
              "startRow": 1,
              "startSeat": 1
            }
          ]
        }
      ]
    }
  ]
}
```

**Features:**
- Automatic seat generation for all sectors
- Validation of tribune codes (N, S, E, W only)
- Database transaction safety (rollback on errors)
- Export existing structure as JSON

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
  `use playwright → Open http://localhost:7003, log in as a customer, and place an order.`
