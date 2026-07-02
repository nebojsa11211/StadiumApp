using StadiumDrinkOrdering.TicketingSimulator.Services;

var builder = WebApplication.CreateBuilder(args);

// Container support (HTTPS-only project convention)
if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
{
    builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("ASPNETCORE_URLS") ?? "https://+:8447");
}

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

var apiBaseUrl = (builder.Configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7010").TrimEnd('/');
Console.WriteLine($"🌐 Stadium API base URL: {apiBaseUrl}");

builder.Services.AddHttpClient<SimulatorApiClient>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl + "/");
    client.Timeout = TimeSpan.FromSeconds(60);
}).ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    // Trust the ASP.NET dev certificate in development.
    ServerCertificateCustomValidationCallback = (_, _, _, _) => true
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
