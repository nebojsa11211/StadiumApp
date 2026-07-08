using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Pages
{
    public partial class Analytics : ComponentBase
    {
        [Inject] private IAdminApiService AdminApiService { get; set; } = default!;
        [Inject] private NavigationManager NavigationManager { get; set; } = default!;

        private bool isLoading = false;
        private string? errorMessage = null;

        // Analytics data
        private IEnumerable<EventDto>? availableEvents;

        /// <summary>The configured venue — its address is the shared location shown for every event.</summary>
        private VenueDto? venue;

        protected override async Task OnInitializedAsync()
        {
            await LoadAnalyticsData();
        }

        /// <summary>
        /// The location to display for an event: its own stored value if present, otherwise the
        /// shared venue location (name plus city). Every event is held in the one stadium.
        /// </summary>
        private string DisplayLocation(EventDto evt)
        {
            if (!string.IsNullOrWhiteSpace(evt.Location)) return evt.Location!;
            var name = venue?.Name;
            if (string.IsNullOrWhiteSpace(name)) return "—";
            return string.IsNullOrWhiteSpace(venue?.City) ? name : $"{name} · {venue!.City}";
        }

        private async Task LoadAnalyticsData()
        {
            isLoading = true;
            errorMessage = null;
            StateHasChanged();

            try
            {
                await LoadAvailableEvents();
                await LoadVenue();
            }
            catch (Exception ex)
            {
                errorMessage = $"Failed to load analytics data: {ex.Message}";
            }
            finally
            {
                isLoading = false;
                StateHasChanged();
            }
        }

        private async Task LoadAvailableEvents()
        {
            try
            {
                availableEvents = await AdminApiService.GetEventsAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load events: {ex.Message}");
            }
        }

        /// <summary>Loads the venue so the events table can show its address as the shared location. Never throws.</summary>
        private async Task LoadVenue()
        {
            try
            {
                venue = await AdminApiService.GetAsync<VenueDto>("venue");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load venue: {ex.Message}");
            }
        }

        private void NavigateToCustomerAnalytics()
        {
            NavigationManager.NavigateTo("/customer-analytics");
        }

        private void NavigateToEvents()
        {
            NavigationManager.NavigateTo("/events");
        }

        private void NavigateToOrders()
        {
            NavigationManager.NavigateTo("/orders");
        }

        private void NavigateToLogs()
        {
            NavigationManager.NavigateTo("/logs");
        }
    }
}