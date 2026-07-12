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

        // Analytics data — allEvents is the full set; availableEvents is the season-filtered view shown.
        private List<EventDto> allEvents = new();
        private IEnumerable<EventDto>? availableEvents;

        // Season scope (mirrors the dashboard/orders/tickets).
        private int? selectedSeasonId;
        private List<SeasonDto> seasons = new();
        private List<(int Id, string Name)> seasonOptions = new();

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
                allEvents = (await AdminApiService.GetEventsAsync())?.ToList() ?? new List<EventDto>();

                try { seasons = await AdminApiService.GetAsync<List<SeasonDto>>("seasons") ?? new(); }
                catch (Exception ex) { Console.WriteLine($"Failed to load seasons: {ex.Message}"); }

                BuildSeasonOptions();
                ApplyEventFilter();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to load events: {ex.Message}");
            }
        }

        private void BuildSeasonOptions()
        {
            // Only surface seasons that actually have events.
            var seasonIdsWithEvents = allEvents
                .Where(e => e.SeasonId.HasValue)
                .Select(e => e.SeasonId!.Value)
                .ToHashSet();

            seasonOptions = seasons
                .Where(s => seasonIdsWithEvents.Contains(s.Id))
                .OrderBy(s => s.StartDate)
                .ThenBy(s => s.Id)
                .Select(s => (s.Id, s.Name))
                .ToList();

            if (selectedSeasonId.HasValue && !seasonOptions.Any(s => s.Id == selectedSeasonId.Value))
                selectedSeasonId = null;
        }

        // Rebuilds the shown event list from the season selection, then applies the active sort.
        private void ApplyEventFilter()
        {
            IEnumerable<EventDto> query = allEvents;
            if (selectedSeasonId.HasValue)
                query = query.Where(e => e.SeasonId == selectedSeasonId.Value);

            availableEvents = query.ToList();
            ApplyEventSort();
        }

        private void OnSeasonChanged(ChangeEventArgs e)
        {
            var v = e.Value?.ToString();
            selectedSeasonId = string.IsNullOrEmpty(v) ? (int?)null
                : int.TryParse(v, out var sid) ? sid : (int?)null;
            ApplyEventFilter();
            StateHasChanged();
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