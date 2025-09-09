using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using StadiumDrinkOrdering.Admin.Data;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Disable launch settings in Docker environment
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "http://+:8082");
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
        // Running in Docker container - use Docker networking
        apiBaseUrl = "http://api:8080/";
    }
    else
    {
        // Running locally - use localhost
        apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "http://localhost:7000/";
    }
    
    Console.WriteLine($"=== ADMIN CONFIGURATION DEBUG ===");
    Console.WriteLine($"Environment: {environment}");
    Console.WriteLine($"Container Env: {containerEnv}");
    Console.WriteLine($"ASPNETCORE_URLS: {aspnetUrls}");
    Console.WriteLine($"Final API BaseUrl: {apiBaseUrl}");
    Console.WriteLine($"===================================");
    
    client.BaseAddress = new Uri(apiBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30); // Reduce timeout to 30 seconds
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Add token storage service as singleton to persist across scopes
builder.Services.AddSingleton<ITokenStorageService, TokenStorageService>();

// Add SignalR service
builder.Services.AddScoped<ISignalRService, SignalRService>();

// Add authentication state service
builder.Services.AddScoped<IAuthStateService, AuthStateService>();

// Add centralized logging client
var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
string apiBaseUrl;
if (containerEnv == "true")
{
    apiBaseUrl = "http://api:8080";
}
else
{
    apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "http://localhost:7000";
}

builder.Services.AddCentralizedLogging(apiBaseUrl, "Admin");

// Add console logging services
builder.Services.AddScoped<IConsoleLoggingService, ConsoleLoggingService>();
builder.Services.AddScoped<IConsoleLoggingToggleService, ConsoleLoggingToggleService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Only use HTTPS redirection when not in Docker
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") != "true")
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

// Add localization middleware
app.UseRequestLocalization();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
