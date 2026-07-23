using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using StadiumDrinkOrdering.Admin.Data;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Services.Orders;
using StadiumDrinkOrdering.Admin.Services.Users;
using StadiumDrinkOrdering.Admin.Services.Drinks;
using StadiumDrinkOrdering.Admin.Services.Tickets;
using StadiumDrinkOrdering.Admin.Services.Auth;
using StadiumDrinkOrdering.Admin.Services.Logs;
using StadiumDrinkOrdering.Admin.Services.Analytics;
using StadiumDrinkOrdering.Admin.Services.Stadium;
using StadiumDrinkOrdering.Admin.Services.Events;
using StadiumDrinkOrdering.Admin.Services.Http;
using StadiumDrinkOrdering.Admin.Services.ErrorHandling;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Extensions;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Configuration;
using System.Globalization;

Console.WriteLine("=== ADMIN PROGRAM START ===");
Console.WriteLine($"DOTNET_RUNNING_IN_CONTAINER: {Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")}");
Console.WriteLine($"ASPNETCORE_URLS: {Environment.GetEnvironmentVariable("ASPNETCORE_URLS")}");

// Load the gitignored .env file (NoClobber) before building configuration, so ApiSettings__BaseUrl
// and other variables can be configured there for local development.
AppConfiguration.LoadDotEnvFile();

var builder = WebApplication.CreateBuilder(args);

// Docker configuration - support both HTTP and HTTPS when running in container
var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
Console.WriteLine($"Container environment check: '{containerEnv}' == 'true' = {containerEnv == "true"}");

if (containerEnv == "true")
{
    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8445";
    Console.WriteLine($"🐳 Docker container detected - using URLs: {urls}");
    builder.WebHost.UseUrls(urls);
}
else
{
    Console.WriteLine("💻 Local development mode - using default configuration");
}

// Single source of truth for the API endpoint (see AppConfiguration.ResolveApiBaseUrl).
var apiBaseUrl = AppConfiguration.ResolveApiBaseUrl(builder.Configuration);
Console.WriteLine($"🌐 API base URL: {apiBaseUrl}");

// Add session state for authentication token bridge
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromHours(2);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // HTTPS only
    options.Cookie.SameSite = SameSiteMode.Strict;
});

// Add HttpContextAccessor for session access in services
builder.Services.AddHttpContextAccessor();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Add MVC controllers for API endpoints
builder.Services.AddServerSideBlazor(options =>
{
    // Configure SignalR for Docker environment
    if (builder.Environment.IsDevelopment() || containerEnv == "true")
    {
        options.DetailedErrors = true;
        options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromMinutes(3);
        options.DisconnectedCircuitMaxRetained = 100;
        options.JSInteropDefaultCallTimeout = TimeSpan.FromMinutes(1);
    }
})
.AddHubOptions(options =>
{
    // The event-poster flow downscales a generated image in the browser (canvas) and returns the
    // base64 thumbnail through JS interop. That return trip is client→server, so it is bounded by
    // MaximumReceiveMessageSize — whose 32 KB default is far below a ~150 KB thumbnail and silently
    // tears down the circuit mid-call. 1 MB leaves generous headroom without inviting large uploads
    // over the hub. (The poster itself never crosses this boundary: it is fetched server-side.)
    options.MaximumReceiveMessageSize = 1024 * 1024;
});
builder.Services.AddSingleton<WeatherForecastService>();

// Add localization services
builder.Services.AddLocalization();
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "hr" };
    options.DefaultRequestCulture = new RequestCulture("hr");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    
    // Add cookie request culture provider
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

// Register a single, centralized HttpClient for all API communication
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl + "/");
    client.Timeout = TimeSpan.FromSeconds(120); // Extended timeout for database operations
    client.DefaultRequestVersion = new Version(1, 1);
    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();

    // In development, bypass SSL certificate validation for self-signed certificates
    if (builder.Environment.IsDevelopment() || containerEnv == "true")
    {
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
        Console.WriteLine("⚠️ SSL certificate validation bypassed for development");
    }

    return handler;
});

// Register throttling service as singleton for persistent state
builder.Services.AddSingleton<IThrottlingService, ThrottlingService>();

// Register error notification service as scoped (depends on IJSRuntime which is scoped)
builder.Services.AddScoped<IErrorNotificationService, ErrorNotificationService>();

// Register specialized services to use the authenticated HTTP client
builder.Services.AddScoped<IOrderService>(provider =>
    new OrderService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                     provider.GetRequiredService<ICentralizedLoggingClient>(),
                     provider.GetRequiredService<IErrorNotificationService>(),
                     provider.GetRequiredService<ITokenStorageService>()));

builder.Services.AddScoped<IUserService>(provider =>
    new UserService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                    provider.GetRequiredService<ICentralizedLoggingClient>(),
                    provider.GetRequiredService<IErrorNotificationService>(),
                    provider.GetRequiredService<ITokenStorageService>()));

builder.Services.AddScoped<IDrinkService>(provider =>
    new DrinkService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                     provider.GetRequiredService<ICentralizedLoggingClient>(),
                     provider.GetRequiredService<IErrorNotificationService>(),
                     provider.GetRequiredService<ITokenStorageService>()));

builder.Services.AddScoped<ITicketService>(provider =>
    new TicketService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                      provider.GetRequiredService<ICentralizedLoggingClient>(),
                      provider.GetRequiredService<ITokenStorageService>()));

builder.Services.AddScoped<IAuthService>(provider =>
    new AuthService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("ApiClient"),
                    provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<ILogService>(provider =>
    new LogService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                   provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<IAnalyticsService>(provider =>
    new AnalyticsService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                         provider.GetRequiredService<ICentralizedLoggingClient>(),
                         provider.GetRequiredService<ITokenStorageService>()));

builder.Services.AddScoped<IStadiumService>(provider =>
    new StadiumService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                       provider.GetRequiredService<ICentralizedLoggingClient>(),
                       provider.GetRequiredService<ITokenStorageService>()));

builder.Services.AddScoped<IEventService>(provider =>
    new EventService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                     provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<IHttpService>(provider =>
    new HttpService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthenticatedClient"),
                    provider.GetRequiredService<ICentralizedLoggingClient>(),
                    provider.GetRequiredService<IErrorNotificationService>(),
                    provider.GetRequiredService<ITokenStorageService>()));

// Register composite AdminApiService
builder.Services.AddScoped<IAdminApiService, AdminApiService>();

// Shell-wide season scope shown by the banner in DashboardLayout. Scoped = one per circuit,
// so the selected season is shared by every admin page and survives navigation.
builder.Services.AddScoped<SeasonStateService>();

// Tracks the live (game-day) event so the shell can show a read-only event bar on every page.
builder.Services.AddScoped<LiveEventService>();

// First-run setup readiness, backing the setup banner in DashboardLayout and the /admin/setup page.
builder.Services.AddScoped<SetupStatusService>();

Console.WriteLine("✅ Registered specialized Admin API services:");
Console.WriteLine("   - OrderService: Order management operations");
Console.WriteLine("   - UserService: User management operations");
Console.WriteLine("   - DrinkService: Drink catalog operations");
Console.WriteLine("   - TicketService: Ticket validation operations");
Console.WriteLine("   - AuthService: Authentication operations");
Console.WriteLine("   - LogService: Logging and audit operations");
Console.WriteLine("   - AnalyticsService: Customer analytics operations");
Console.WriteLine("   - StadiumService: Stadium structure operations");
Console.WriteLine("   - EventService: Event management operations");
Console.WriteLine("   - HttpService: Generic HTTP operations");
Console.WriteLine("   - AdminApiService: Composite service (backward compatibility)");

// API base URL for authentication services resolved once above (apiBaseUrl).

// ✅ Add standardized shared authentication services with refresh token support
// Note: SecureApiService is registered manually below due to custom constructor requirements
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<IAuthStateService>(provider => provider.GetRequiredService<AuthStateService>());
builder.Services.AddScoped<IAuthenticationStateService>(provider => provider.GetRequiredService<AuthStateService>());
// Use hybrid token storage for both client-side and server-side authentication
builder.Services.AddScoped<ITokenStorageService, HybridTokenStorageService>();
// Background refresh is DISABLED for Admin: this app stores only an access token (no
// refresh token), so the shared BackgroundTokenRefreshService can never refresh - it just
// reports "RequiresReauthentication" and CLEARS the still-valid token every minute, which
// bounced the user between dashboard and /login. Token expiry is still handled by the
// HybridTokenStorageService expiration timer (OnTokenExpired -> logout).
builder.Services.AddClientAuthentication(apiBaseUrl, "Admin", enableBackgroundRefresh: false);

// Note: AuthenticatedClient is configured by AddClientAuthentication() above with proper authentication handler
// Configure SSL certificate bypass for development/container environments
builder.Services.ConfigureHttpClientDefaults(clientBuilder =>
{
    if (builder.Environment.IsDevelopment() || containerEnv == "true")
    {
        clientBuilder.ConfigurePrimaryHttpMessageHandler(() =>
        {
            var handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
            Console.WriteLine("⚠️ SSL certificate validation bypassed for development");
            return handler;
        });
    }
});

// Note: Enhanced refresh token services will be implemented in future iterations

// Register SecureApiService with its dependencies
builder.Services.AddScoped<SecureApiService>(provider =>
{
    var adminApiService = provider.GetRequiredService<IAdminApiService>();
    var tokenStorage = provider.GetRequiredService<ITokenStorageService>();
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("AdminSecureApi");

    return new SecureApiService(adminApiService, tokenStorage, httpClient, apiBaseUrl);
});

// Configure HTTP client for SecureApiService
builder.Services.AddHttpClient("AdminSecureApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(120); // Extended timeout for database operations
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();

    // In development, bypass SSL certificate validation for self-signed certificates
    if (builder.Environment.IsDevelopment() || containerEnv == "true")
    {
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    }

    return handler;
});

// Legacy interface registration is now handled by the shared authentication services
// The AuthStateService class implements both IAuthenticationStateService and IAuthStateService

// Add SignalR service
builder.Services.AddScoped<ISignalRService, SignalRService>();

// Add centralized logging client
builder.Services.AddCentralizedLogging(apiBaseUrl, "Admin");

// Register the external image-generation API client. Base URL is configurable via
// ImageGeneration:BaseUrl (appsettings / env var / .env), defaulting to https://localhost:7285.
// Dev-cert validation is bypassed only for local/container development (the API uses a self-signed
// dev cert); trust it once with `dotnet dev-certs https --trust` to avoid the bypass.
var imageGenBaseUrl = builder.Configuration["ImageGeneration:BaseUrl"]?.TrimEnd('/') ?? "https://localhost:7285";
Console.WriteLine($"🎨 Image generation API base URL: {imageGenBaseUrl}");
builder.Services.AddImageGeneration(imageGenBaseUrl,
    bypassDevCertValidation: builder.Environment.IsDevelopment() || containerEnv == "true");

// Add console logging services
builder.Services.AddScoped<IConsoleLoggingService, ConsoleLoggingService>();
builder.Services.AddScoped<IConsoleLoggingToggleService, ConsoleLoggingToggleService>();

// === COMPREHENSIVE ADMIN SERVICE LAYER ===
// Add memory cache for all caching operations
builder.Services.AddMemoryCache();

// Core Services - TODO: Add services when they are implemented

Console.WriteLine("✅ Registered comprehensive Admin Service Layer:");
Console.WriteLine("   - AdminCacheService (Scoped): Centralized caching with intelligent invalidation");
Console.WriteLine("   - AdminDashboardService (Scoped): Consolidated dashboard data with 30s caching");
Console.WriteLine("   - AdminOrderService (Scoped): Advanced order management with filtering & bulk ops");
Console.WriteLine("   - AdminUserService (Scoped): User management with role-based access control");
Console.WriteLine("   - AdminAnalyticsService (Scoped): Analytics with forecasting & insights");
Console.WriteLine("   - AdminNotificationService (Singleton): Real-time notifications with SignalR");

// Add stadium SVG services for dynamic stadium rendering
// Memory cache already added above in service layer registration

// Configure StadiumSvgService with the same API base URL as AdminApiService
builder.Services.AddHttpClient<IStadiumSvgService, StadiumSvgService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl + "/");
    client.Timeout = TimeSpan.FromSeconds(120); // Extended timeout for database operations
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();

    // In development, bypass SSL certificate validation for self-signed certificates
    if (builder.Environment.IsDevelopment() || containerEnv == "true")
    {
        handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    }

    return handler;
});

// Stadium layout generator service (commented out - requires additional implementation)
// builder.Services.AddScoped<IStadiumLayoutGenerator, HNKRijekaLayoutGenerator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Use HTTPS redirection in all environments
app.UseHttpsRedirection();

app.UseStaticFiles();

// Add session middleware BEFORE localization and routing
app.UseSession();

// Add localization middleware
app.UseRequestLocalization();

app.UseRouting();

// Map MVC controllers for API endpoints
app.MapControllers();

app.MapBlazorHub(options =>
{
    // Configure SignalR hub for Docker environment
    if (builder.Environment.IsDevelopment() || containerEnv == "true")
    {
        options.ApplicationMaxBufferSize = 65536;
        options.TransportMaxBufferSize = 65536;
    }
});
app.MapFallbackToPage("/_Host");

app.Run();
