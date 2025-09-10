using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Data;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class FetchData
{
    [Inject] private WeatherForecastService ForecastService { get; set; } = default!;
    
    private WeatherForecast[]? forecasts;

    protected override async Task OnInitializedAsync()
    {
        forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
    }
}