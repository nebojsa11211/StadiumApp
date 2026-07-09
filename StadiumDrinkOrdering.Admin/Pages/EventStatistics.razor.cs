using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class EventStatistics : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;

    /// <summary>Event id from the route: /admin/event-statistics/{EventId}.</summary>
    [Parameter] public int EventId { get; set; }

    private EventStatisticsDto? stats;
    private bool loadFailed;
    private string loadError = "";

    protected override async Task OnParametersSetAsync()
    {
        // Reload when navigating between events without leaving the page.
        if (stats == null || stats.EventId != EventId)
        {
            await LoadStats();
        }
    }

    private async Task LoadStats()
    {
        loadFailed = false;
        loadError = "";
        stats = null;

        try
        {
            stats = await ApiService.GetAsync<EventStatisticsDto>($"events/{EventId}/statistics");
            if (stats == null)
            {
                loadFailed = true;
                loadError = "Statistics could not be loaded for this event.";
            }
        }
        catch (Exception ex)
        {
            loadFailed = true;
            loadError = $"An error occurred while loading statistics: {ex.Message}";
        }
    }
}
