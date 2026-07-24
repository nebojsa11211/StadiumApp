using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using StadiumDrinkOrdering.Bar.Services;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Extensions;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Configuration;
using System.Globalization;

// Load the gitignored .env file (NoClobber) before building configuration.
AppConfiguration.LoadDotEnvFile();

var builder = WebApplication.CreateBuilder(args);

// Configure URLs for Docker environment
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8446");
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add localization services.
// NOTE: Do NOT set ResourcesPath here. The .resx files are compiled with the SDK
// "DependentUpon" convention, which names the embedded resource by the marker class's
// namespace (StadiumDrinkOrdering.Bar.SharedResources) and ignores the physical Resources/
// folder. Setting ResourcesPath="Resources" would make the localizer look for
// "StadiumDrinkOrdering.Bar.Resources.SharedResources" instead, which does not exist, and
// silently disable all translations. This mirrors the working Admin app.
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

// Single source of truth for the API endpoint (see AppConfiguration.ResolveApiBaseUrl).
var apiBaseUrl = AppConfiguration.ResolveApiBaseUrl(builder.Configuration);
Console.WriteLine($"🌐 API base URL: {apiBaseUrl}");

// Configure HttpClient for legacy StaffApiService
builder.Services.AddHttpClient<IStaffApiService, StaffApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl + "/");
    client.Timeout = TimeSpan.FromSeconds(120);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Typed client for the small "which database" diagnostic badge (anonymous /api/system/info).
builder.Services.AddHttpClient<StadiumDrinkOrdering.UI.SystemInfoClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl + "/");
    client.Timeout = TimeSpan.FromSeconds(15);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// ✅ Add standardized shared authentication services with refresh token support.
// Registered explicitly (rather than via the 3-generic AddSharedAuthentication overload) because
// StaffSecureApiService needs a string apiBaseUrl constructor argument and is registered through the
// factory below. The generic overload's AddScoped<StaffSecureApiService>() registers it by
// implementation type, which cannot supply that string and fails DI validation at startup.
builder.Services.AddScoped<AuthStateService>();
builder.Services.AddScoped<ITokenStorageService, StaffTokenStorageService>();
builder.Services.AddClientAuthentication(apiBaseUrl, "Bar", enableBackgroundRefresh: true);

// Note: Enhanced refresh token services will be implemented in future iterations

// Register StaffSecureApiService with its dependencies
builder.Services.AddScoped<StaffSecureApiService>(provider =>
{
    var staffApiService = provider.GetRequiredService<IStaffApiService>();
    var tokenStorage = provider.GetRequiredService<ITokenStorageService>();
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("StaffSecureApi");

    return new StaffSecureApiService(staffApiService, tokenStorage, httpClient, apiBaseUrl);
});

// Configure HTTP client for StaffSecureApiService
builder.Services.AddHttpClient("StaffSecureApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(120);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Add legacy services
// NOTE: IStaffApiService is intentionally NOT re-registered here. It is already registered
// above via AddHttpClient<IStaffApiService, StaffApiService> (line ~44), which supplies the
// configured HttpClient (correct BaseAddress + dev-cert bypass handler). A plain
// AddScoped<IStaffApiService, StaffApiService> would shadow that typed client with an
// unconfigured HttpClient, causing every API call (including login) to fail silently.
builder.Services.AddScoped<ISignalRService, SignalRService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Register legacy interfaces for backward compatibility
// AuthStateService implements both IAuthenticationStateService and IAuthStateService
builder.Services.AddScoped<IAuthenticationStateService>(provider => provider.GetRequiredService<AuthStateService>());
builder.Services.AddScoped<IAuthStateService>(provider => provider.GetRequiredService<AuthStateService>());

// Add centralized logging client
builder.Services.AddCentralizedLogging(apiBaseUrl, "Bar");

// Add memory cache for dashboard service
builder.Services.AddMemoryCache();

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
