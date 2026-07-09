using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Common;
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

        // Sorting
        private readonly TableSortState sortState = new();
        private static readonly Dictionary<string, Func<EventDto, object?>> SortSelectors = new()
        {
            ["name"] = e => e.Name,
            ["date"] = e => e.Date,
            ["status"] = e => e.IsActive,
        };

        private void SortBy(string column)
        {
            sortState.Toggle(column);
            ApplyEventSort();
            StateHasChanged();
        }

        private void ApplyEventSort()
        {
            if (availableEvents == null || sortState.Column is null)
                return;

            availableEvents = sortState.Apply(availableEvents, SortSelectors).ToList();
        }

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
                ApplyEventSort();
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