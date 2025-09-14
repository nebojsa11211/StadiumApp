using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
// using StadiumDrinkOrdering.Admin.Components; - Components removed
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class CustomerAnalytics : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    // private CustomerDetailModal detailModal = default!; - Component removed
    private List<CustomerAnalyticsDto>? customers;
    private CustomerAnalyticsSummaryDto? summary;
    private PagedCustomerAnalyticsDto? pagedResult;
    private CustomerAnalyticsFilterDto currentFilter = new();
    private bool isLoading = false;
    private string? errorMessage = null;

    protected override async Task OnInitializedAsync()
    {
        currentFilter = new CustomerAnalyticsFilterDto
        {
            Page = 1,
            PageSize = 20,
            SortBy = CustomerSortBy.TotalSpent,
            SortDescending = true
        };

        await LoadCustomerAnalytics();
        await LoadSummary();
    }

    private async Task LoadCustomerAnalytics()
    {
        isLoading = true;
        errorMessage = null;
        StateHasChanged();

        try
        {
            pagedResult = await ApiService.GetCustomerAnalyticsAsync(currentFilter);
            customers = pagedResult?.Customers?.ToList();

            if (customers?.Any() != true)
            {
                customers = new List<CustomerAnalyticsDto>();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error loading customer analytics: {ex.Message}";
            customers = new List<CustomerAnalyticsDto>();
            pagedResult = null;
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private async Task LoadSummary()
    {
        try
        {
            summary = await ApiService.GetCustomerAnalyticsSummaryAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading summary: {ex.Message}");
        }
    }

    private async Task ApplyFilters()
    {
        currentFilter.Page = 1; // Reset to first page when applying filters
        await LoadCustomerAnalytics();
    }

    private async Task ClearFilters()
    {
        currentFilter = new CustomerAnalyticsFilterDto
        {
            Page = 1,
            PageSize = currentFilter.PageSize,
            SortBy = CustomerSortBy.TotalSpent,
            SortDescending = true
        };
        await LoadCustomerAnalytics();
    }

    private async Task OnSearchKeyPress(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            await ApplyFilters();
        }
    }

    private async Task OnPageSizeChanged(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out int newPageSize))
        {
            currentFilter.PageSize = newPageSize;
            currentFilter.Page = 1;
            await LoadCustomerAnalytics();
        }
    }

    private async Task GoToPage(int page)
    {
        if (page >= 1 && page <= (pagedResult?.TotalPages ?? 1) && page != currentFilter.Page)
        {
            currentFilter.Page = page;
            await LoadCustomerAnalytics();
        }
    }

    private async Task ViewCustomerDetails(string customerEmail)
    {
        // await detailModal.ShowCustomerDetails(customerEmail); - Component removed
    }

    private async Task ExportData()
    {
        if (customers?.Any() != true)
            return;

        try
        {
            var response = await ApiService.ExportCustomerAnalyticsAsync(currentFilter);
            if (response != null && response.IsSuccessStatusCode)
            {
                var csvContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(csvContent))
                {
                    var fileName = $"customer-analytics-{DateTime.Now:yyyyMMdd-HHmmss}.csv";
                    await JSRuntime.InvokeVoidAsync("downloadFile", fileName, csvContent);
                }
                else
                {
                    errorMessage = "No data available for export.";
                    StateHasChanged();
                }
            }
            else
            {
                errorMessage = "Failed to export data.";
                StateHasChanged();
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Error exporting data: {ex.Message}";
            StateHasChanged();
        }
    }
}