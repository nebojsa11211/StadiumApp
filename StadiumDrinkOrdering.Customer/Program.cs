using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Localization;
using StadiumDrinkOrdering.Customer.Data;
using StadiumDrinkOrdering.Customer.Services;
using StadiumDrinkOrdering.Shared.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configure URLs for Docker environment
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8444;http://+:8081");
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
builder.Services.AddHttpClient<IApiService, ApiService>(client =>
{
    var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
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
}).ConfigurePrimaryHttpMessageHandler(() =>
{
    var handler = new HttpClientHandler();
    handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true;
    return handler;
});

// Add custom services
builder.Services.AddScoped<ICartService, CartService>();

// Add authentication services
builder.Services.AddSingleton<ICustomerTokenStorageService, CustomerTokenStorageService>();
builder.Services.AddScoped<ICustomerAuthStateService, CustomerAuthStateService>();

// Add centralized logging client
var containerEnv = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER");
var apiBaseUrl = containerEnv == "true" 
    ? "https://api:8443" 
    : builder.Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://localhost:7010";
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
