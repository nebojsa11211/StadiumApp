using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using StadiumDrinkOrdering.Admin.Data;
using StadiumDrinkOrdering.Admin.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();

// Add HTTP client
builder.Services.AddHttpClient<IAdminApiService, AdminApiService>(client =>
{
    var apiBaseUrl = builder.Configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "http://api:8080/";
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Add SignalR service
builder.Services.AddScoped<ISignalRService, SignalRService>();

// Add authentication state service
builder.Services.AddScoped<IAuthStateService, AuthStateService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
