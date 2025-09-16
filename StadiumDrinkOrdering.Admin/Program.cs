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
using System.Globalization;

Console.WriteLine("=== ADMIN PROGRAM START ===");
Console.WriteLine($"DOTNET_RUNNING_IN_CONTAINER: {Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER")}");
Console.WriteLine($"ASPNETCORE_URLS: {Environment.GetEnvironmentVariable("ASPNETCORE_URLS")}");

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

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
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

// Helper function to configure HttpClient for API services
static void ConfigureApiHttpClient(IServiceCollection services, string clientName)
{
    var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");

    services.AddHttpClient(clientName, client =>
    {
        string apiBaseUrl;
        if (containerEnv == "true")
        {
            // Running in Docker container - use Docker networking (HTTPS in Docker)
            apiBaseUrl = "https://api:8443/";
        }
        else
        {
            // Running locally - use localhost
            apiBaseUrl = "https://localhost:7010/";
        }

        client.BaseAddress = new Uri(apiBaseUrl);
        client.Timeout = TimeSpan.FromSeconds(10);
        client.DefaultRequestVersion = new Version(1, 1);
        client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
    }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        var handler = new HttpClientHandler();
        handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
        return handler;
    });
}

// Register HTTP clients for each service
ConfigureApiHttpClient(builder.Services, "OrderService");
ConfigureApiHttpClient(builder.Services, "UserService");
ConfigureApiHttpClient(builder.Services, "DrinkService");
ConfigureApiHttpClient(builder.Services, "TicketService");
ConfigureApiHttpClient(builder.Services, "AuthService");
ConfigureApiHttpClient(builder.Services, "LogService");
ConfigureApiHttpClient(builder.Services, "AnalyticsService");
ConfigureApiHttpClient(builder.Services, "StadiumService");
ConfigureApiHttpClient(builder.Services, "EventService");
ConfigureApiHttpClient(builder.Services, "HttpService");

// Register throttling service as singleton for persistent state
builder.Services.AddSingleton<IThrottlingService, ThrottlingService>();

// Register error notification service as scoped (depends on IJSRuntime which is scoped)
builder.Services.AddScoped<IErrorNotificationService, ErrorNotificationService>();

// Register specialized services
builder.Services.AddScoped<IOrderService>(provider =>
    new OrderService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("OrderService"),
                     provider.GetRequiredService<ICentralizedLoggingClient>(),
                     provider.GetRequiredService<IErrorNotificationService>()));

builder.Services.AddScoped<IUserService>(provider =>
    new UserService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("UserService"),
                    provider.GetRequiredService<ICentralizedLoggingClient>(),
                    provider.GetRequiredService<IErrorNotificationService>(),
                    provider.GetRequiredService<ITokenStorageService>()));

builder.Services.AddScoped<IDrinkService>(provider =>
    new DrinkService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("DrinkService"),
                     provider.GetRequiredService<ICentralizedLoggingClient>(),
                     provider.GetRequiredService<IErrorNotificationService>()));

builder.Services.AddScoped<ITicketService>(provider =>
    new TicketService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("TicketService"),
                      provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<IAuthService>(provider =>
    new AuthService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AuthService"),
                    provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<ILogService>(provider =>
    new LogService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("LogService"),
                   provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<IAnalyticsService>(provider =>
    new AnalyticsService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("AnalyticsService"),
                         provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<IStadiumService>(provider =>
    new StadiumService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("StadiumService"),
                       provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<IEventService>(provider =>
    new EventService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("EventService"),
                     provider.GetRequiredService<ICentralizedLoggingClient>()));

builder.Services.AddScoped<IHttpService>(provider =>
    new HttpService(provider.GetRequiredService<IHttpClientFactory>().CreateClient("HttpService"),
                    provider.GetRequiredService<ICentralizedLoggingClient>(),
                    provider.GetRequiredService<IErrorNotificationService>(),
                    provider.GetRequiredService<ITokenStorageService>()));

// Register composite AdminApiService
builder.Services.AddScoped<IAdminApiService, AdminApiService>();

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

// Determine API base URL for authentication services
string apiBaseUrl;
if (containerEnv == "true")
{
    apiBaseUrl = "https://api:8443";
}
else
{
    apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://localhost:7010";
}

// ✅ Add standardized shared authentication services with refresh token support
// Note: SecureApiService is registered manually below due to custom constructor requirements
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<IAuthStateService>(provider => provider.GetRequiredService<AuthStateService>());
builder.Services.AddScoped<IAuthenticationStateService>(provider => provider.GetRequiredService<AuthStateService>());
builder.Services.AddScoped<ITokenStorageService, TokenStorageService>();
builder.Services.AddClientAuthentication(apiBaseUrl, "Admin", enableBackgroundRefresh: true);

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
    client.Timeout = TimeSpan.FromSeconds(30);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
    return handler;
});

// Legacy interface registration is now handled by the shared authentication services
// The AuthStateService class implements both IAuthenticationStateService and IAuthStateService

// Add SignalR service
builder.Services.AddScoped<ISignalRService, SignalRService>();

// Add centralized logging client
builder.Services.AddCentralizedLogging(apiBaseUrl, "Admin");

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
    string apiBaseUrl;
    if (containerEnv == "true")
    {
        apiBaseUrl = "https://api:8443/";
    }
    else
    {
        apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7010/";
    }
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
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

// Add localization middleware
app.UseRequestLocalization();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
