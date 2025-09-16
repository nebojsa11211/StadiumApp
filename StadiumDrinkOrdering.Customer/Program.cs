using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using StadiumDrinkOrdering.Customer.Data;
using StadiumDrinkOrdering.Customer.Services;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Extensions;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs for Docker environment
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8444");
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

// Determine API base URL
var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
var apiBaseUrl = containerEnv == "true"
    ? "https://api:8443"
    : builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://localhost:7010";

// Add HTTP client for legacy ApiService
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl + "/");
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Add custom services
builder.Services.AddScoped<ICartService, CartService>();

// ✅ Add standardized shared authentication services with refresh token support
builder.Services.AddSharedAuthentication<CustomerAuthStateService, CustomerTokenStorageService, CustomerSecureApiService>(
    "Customer",
    apiBaseUrl);

// Note: Enhanced refresh token services will be implemented in future iterations

// Register CustomerSecureApiService with its dependencies
builder.Services.AddScoped<CustomerSecureApiService>(provider =>
{
    var apiService = provider.GetRequiredService<IApiService>();
    var tokenStorage = provider.GetRequiredService<ITokenStorageService>();
    var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
    var httpClient = httpClientFactory.CreateClient("CustomerSecureApi");

    return new CustomerSecureApiService(apiService, tokenStorage, httpClient, apiBaseUrl);
});

// Configure HTTP client for CustomerSecureApiService
builder.Services.AddHttpClient("CustomerSecureApi", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl.TrimEnd('/') + "/");
    client.Timeout = TimeSpan.FromSeconds(30);
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Legacy interface registration is now handled by the shared authentication services
// The CustomerAuthStateService and CustomerTokenStorageService classes implement both interfaces

// Add centralized logging client
builder.Services.AddCentralizedLogging(apiBaseUrl, "Customer");

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
