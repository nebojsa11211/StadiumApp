using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using StadiumDrinkOrdering.Staff.Services;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Extensions;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs for Docker environment
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8446");
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

// Add localization services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "hr" };
    options.DefaultRequestCulture = new RequestCulture("hr");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();

    // Add cookie request culture provider
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider());
});

// Determine API base URL
var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
var apiBaseUrl = containerEnv == "true"
    ? "https://api:8443"
    : builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://localhost:7010";

// Configure HttpClient for legacy StaffApiService
builder.Services.AddHttpClient<IStaffApiService, StaffApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl + "/");
    client.Timeout = TimeSpan.FromSeconds(30);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// ✅ Add standardized shared authentication services with refresh token support
builder.Services.AddSharedAuthentication<AuthStateService, StaffTokenStorageService, StaffSecureApiService>(
    "Staff",
    apiBaseUrl);

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
    client.Timeout = TimeSpan.FromSeconds(30);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Add legacy services
builder.Services.AddScoped<IStaffApiService, StaffApiService>();
builder.Services.AddScoped<ISignalRService, SignalRService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();

// Register legacy interfaces for backward compatibility
builder.Services.AddScoped<IAuthStateService>(provider =>
    (IAuthStateService)provider.GetRequiredService<IAuthenticationStateService>());

// Add centralized logging client
builder.Services.AddCentralizedLogging(apiBaseUrl, "Staff");

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
