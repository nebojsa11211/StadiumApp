using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using StadiumDrinkOrdering.Admin.Data;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.Services;
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
    var urls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8445;http://+:8082";
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

// Add HTTP client
builder.Services.AddHttpClient<IAdminApiService, AdminApiService>(client =>
{
    var environment = builder.Environment.EnvironmentName;
    var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
    var aspnetUrls = Environment.GetEnvironmentVariable("ASPNETCORE_URLS");
    
    // Determine API base URL based on environment
    string apiBaseUrl;
    if (containerEnv == "true")
    {
        // Running in Docker container - use Docker networking (HTTPS in Docker)
        apiBaseUrl = "https://api:8443/";
    }
    else
    {
        // Running locally - use localhost
        apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://localhost:7010/";
    }
    
    Console.WriteLine($"=== ADMIN CONFIGURATION DEBUG ===");
    Console.WriteLine($"Environment: {environment}");
    Console.WriteLine($"Container Env: {containerEnv}");
    Console.WriteLine($"ASPNETCORE_URLS: {aspnetUrls}");
    Console.WriteLine($"Final API BaseUrl: {apiBaseUrl}");
    Console.WriteLine($"===================================");
    
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(10); // Shorter timeout to fail fast
    // Force HTTP/1.1 to avoid HTTP/2 issues
    client.DefaultRequestVersion = new Version(1, 1);
    client.DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower;
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    
    // Simplified HTTPS configuration for development
    handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
    
    return handler;
});

// Add token storage service as singleton to persist across scopes
builder.Services.AddSingleton<ITokenStorageService, TokenStorageService>();

// Add SignalR service
builder.Services.AddScoped<ISignalRService, SignalRService>();

// Add authentication state service
builder.Services.AddScoped<IAuthStateService, AuthStateService>();

// Add centralized logging client
string apiBaseUrl;
if (containerEnv == "true")
{
    apiBaseUrl = "https://api:8443";
}
else
{
    apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://localhost:7010";
}

builder.Services.AddCentralizedLogging(apiBaseUrl, "Admin");

// Add console logging services
builder.Services.AddScoped<IConsoleLoggingService, ConsoleLoggingService>();
builder.Services.AddScoped<IConsoleLoggingToggleService, ConsoleLoggingToggleService>();

// Add stadium SVG services for dynamic stadium rendering
builder.Services.AddMemoryCache(); // Required for layout generator caching

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

// builder.Services.AddScoped<IStadiumLayoutGenerator, HNKRijekaLayoutGenerator>(); // Commented out - missing using statement

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
