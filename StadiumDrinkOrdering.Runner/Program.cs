using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Runner;
using StadiumDrinkOrdering.Runner.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Localization: English + Croatian. Resources live in SharedResources.*.resx at the project root.
builder.Services.AddLocalization();

// API endpoint comes from wwwroot/appsettings.json (fetched at runtime by the WASM host).
var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7010";
if (!apiBaseUrl.EndsWith('/')) apiBaseUrl += "/";

// Singleton (not scoped): IHttpClientFactory resolves BearerTokenHandler in its own handler
// scope, so a scoped RunnerAuthService would give the handler a DIFFERENT instance than the
// components — its Token stays null and every API call goes out unauthenticated (401 -> bounce
// to login). A singleton is shared across all scopes, and is correct for a single-user WASM app.
builder.Services.AddSingleton<RunnerAuthService>();
builder.Services.AddScoped<OutboxService>();
builder.Services.AddTransient<BearerTokenHandler>();

// Typed client: every RunnerApiService call goes to the API with the bearer token attached.
builder.Services.AddHttpClient<RunnerApiService>(client => client.BaseAddress = new Uri(apiBaseUrl))
    .AddHttpMessageHandler<BearerTokenHandler>();

var host = builder.Build();

// Apply the persisted UI culture (default Croatian) before the first render. The choice is stored
// in localStorage by blazorCulture.set (see wwwroot/js/culture.js) and read back here at startup.
var js = host.Services.GetRequiredService<IJSRuntime>();
var culture = await js.InvokeAsync<string?>("blazorCulture.get");
var cultureInfo = new CultureInfo(string.IsNullOrWhiteSpace(culture) ? "hr" : culture);
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

await host.RunAsync();
