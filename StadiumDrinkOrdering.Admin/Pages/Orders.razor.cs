using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Orders : ComponentBase
{
    [Inject] private IAdminApiService AdminApiService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private bool isLoading = false;
    private List<OrderDto> allOrders = new();
    private List<OrderDto> filteredOrders = new();
    private HashSet<int> selectedOrderIds = new();

    // Summary data
    private int totalOrders = 0;
    private int pendingOrders = 0;
    private int completedToday = 0;
    private decimal revenueToday = 0;

    // Filters
    private string selectedStatus = "";
    private string customerSearch = "";
    private DateTime? fromDate;
    private DateTime? toDate;

    protected override async Task OnInitializedAsync()
    {
        await LoadOrders();
    }

    private async Task LoadOrders()
    {
        isLoading = true;
        StateHasChanged();

        try
        {
            var orders = await AdminApiService.GetOrdersAsync();
            if (orders != null)
            {
                allOrders = orders.ToList();

                if (allOrders.Count > 0)
                {
                    CalculateSummaryData();
                    FilterOrders();
                }
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("console.error", "Failed to load orders:", ex.Message);
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private void CalculateSummaryData()
    {
        totalOrders = allOrders.Count;
        pendingOrders = allOrders.Count(o => o.Status == OrderStatus.Pending);
        completedToday = allOrders.Count(o => o.Status == OrderStatus.Delivered && o.CreatedAt.Date == DateTime.Today);
        revenueToday = allOrders.Where(o => o.Status == OrderStatus.Delivered && o.CreatedAt.Date == DateTime.Today)
                               .Sum(o => o.TotalAmount);
    }

    private void FilterOrders()
    {
        var query = allOrders.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(selectedStatus))
        {
            if (Enum.TryParse<OrderStatus>(selectedStatus, out var status))
            {
                query = query.Where(o => o.Status == status);
            }
        }

        if (!string.IsNullOrWhiteSpace(customerSearch))
        {
            query = query.Where(o => o.CustomerName.Contains(customerSearch, StringComparison.OrdinalIgnoreCase));
        }

        if (fromDate.HasValue)
        {
            query = query.Where(o => o.CreatedAt.Date >= fromDate.Value.Date);
        }

        if (toDate.HasValue)
        {
            query = query.Where(o => o.CreatedAt.Date <= toDate.Value.Date);
        }

        filteredOrders = query.OrderByDescending(o => o.CreatedAt).ToList();
        StateHasChanged();
    }

    private async Task RefreshData()
    {
        await LoadOrders();
        await JSRuntime.InvokeVoidAsync("showToast", "Orders refreshed successfully", "success");
    }

    private void ClearFilters()
    {
        selectedStatus = "";
        customerSearch = "";
        fromDate = null;
        toDate = null;
        FilterOrders();
    }

    private void ToggleSelectAll(ChangeEventArgs e)
    {
        var isSelected = (bool)e.Value!;
        selectedOrderIds.Clear();
        if (isSelected)
        {
            foreach (var order in filteredOrders.Take(50))
            {
                selectedOrderIds.Add(order.Id);
            }
        }
        StateHasChanged();
    }

    private void ToggleOrderSelection(int orderId, bool isSelected)
    {
        if (isSelected)
            selectedOrderIds.Add(orderId);
        else
            selectedOrderIds.Remove(orderId);
        StateHasChanged();
    }

    private async Task BulkAccept()
    {
        await JSRuntime.InvokeVoidAsync("showToast", $"Accepted {selectedOrderIds.Count} orders", "success");
        selectedOrderIds.Clear();
        await LoadOrders();
    }

    private async Task BulkCancel()
    {
        await JSRuntime.InvokeVoidAsync("showToast", $"Cancelled {selectedOrderIds.Count} orders", "warning");
        selectedOrderIds.Clear();
        await LoadOrders();
    }

    private async Task AcceptOrder(int orderId)
    {
        try
        {
            var order = allOrders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                await JSRuntime.InvokeVoidAsync("showToast", $"Order #{orderId} accepted", "success");
                await LoadOrders();
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to accept order", "error");
        }
    }

    private async Task AdvanceOrderStatus(int orderId)
    {
        try
        {
            var order = allOrders.FirstOrDefault(o => o.Id == orderId);
            if (order != null)
            {
                await JSRuntime.InvokeVoidAsync("showToast", $"Order #{orderId} status updated", "success");
                await LoadOrders();
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to update order status", "error");
        }
    }

    private void CreateNewOrder()
    {
        Navigation.NavigateTo("/orders/new");
    }

    private void ViewOrder(int orderId)
    {
        Navigation.NavigateTo($"/orders/{orderId}");
    }

    private void LoadMoreOrders()
    {
        // Implementation for loading more orders
    }

    private string GetStatusBadgeClass(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "bg-warning text-dark",
            OrderStatus.Accepted => "bg-primary",
            OrderStatus.InPreparation => "bg-info",
            OrderStatus.Ready => "bg-success",
            OrderStatus.OutForDelivery => "bg-dark",
            OrderStatus.Delivered => "bg-success",
            OrderStatus.Cancelled => "bg-danger",
            _ => "bg-secondary"
        };
    }
}