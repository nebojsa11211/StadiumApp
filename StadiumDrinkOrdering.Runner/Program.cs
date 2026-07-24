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

// API endpoint. This code runs in the BROWSER, so a hardcoded "localhost" would point at the
// *device* — fine on the dev machine, broken on a phone hitting the LAN address. So we derive the
// API host from whatever origin actually served the app and just swap in the API port. Loading the
// Runner from https://localhost:7060 talks to https://localhost:7010; loading it from
// https://192.168.178.32:7060 talks to https://192.168.178.32:7010, with no config change.
//
// wwwroot/appsettings.json still wins when it names an explicit non-loopback host, so pointing the
// Runner at a deployed/staging API stays a config-only change.
var configuredApiUrl = builder.Configuration["ApiSettings:BaseUrl"];
var apiBaseUrl = ResolveApiBaseUrl(configuredApiUrl, builder.HostEnvironment.BaseAddress);
if (!apiBaseUrl.EndsWith('/')) apiBaseUrl += "/";

static string ResolveApiBaseUrl(string? configured, string hostBaseAddress)
{
    const int ApiPort = 7010;

    // An explicit, non-loopback configured value is authoritative (staging, Docker, a real host).
    if (!string.IsNullOrWhiteSpace(configured) &&
        Uri.TryCreate(configured, UriKind.Absolute, out var configuredUri) &&
        !configuredUri.IsLoopback)
    {
        return configured;
    }

    // Otherwise follow the origin that served this app, so LAN access works from any device.
    if (Uri.TryCreate(hostBaseAddress, UriKind.Absolute, out var origin))
    {
        return new UriBuilder(origin.Scheme, origin.Host, ApiPort).Uri.ToString();
    }

    return configured ?? $"https://localhost:{ApiPort}";
}

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

// Typed client for the small "which database" diagnostic badge (anonymous /api/system/info).
builder.Services.AddHttpClient<StadiumDrinkOrdering.UI.SystemInfoClient>(client => client.BaseAddress = new Uri(apiBaseUrl));

var host = builder.Build();

// Apply the persisted UI culture (default Croatian) before the first render. The choice is stored
// in localStorage by blazorCulture.set (see wwwroot/js/culture.js) and read back here at startup.
var js = host.Services.GetRequiredService<IJSRuntime>();
var culture = await js.InvokeAsync<string?>("blazorCulture.get");
var cultureInfo = new CultureInfo(string.IsNullOrWhiteSpace(culture) ? "hr" : culture);
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

await host.RunAsync();
