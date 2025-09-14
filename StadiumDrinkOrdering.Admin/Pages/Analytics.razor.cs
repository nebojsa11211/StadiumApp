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

        protected override async Task OnInitializedAsync()
        {
            await LoadAnalyticsData();
        }

        private async Task LoadAnalyticsData()
        {
            isLoading = true;
            errorMessage = null;
            StateHasChanged();

            try
            {
                await LoadAvailableEvents();
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