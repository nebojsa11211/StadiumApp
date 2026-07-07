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

    private const int PageSize = 50;

    private bool isLoading = false;
    private List<OrderDto> allOrders = new();
    private List<OrderDto> filteredOrders = new();
    private HashSet<int> selectedOrderIds = new();
    private int displayCount = PageSize;

    // Summary data
    private int totalOrders = 0;
    private int pendingOrders = 0;
    private int completedToday = 0;
    private decimal revenueToday = 0;

    // Filters
    private string selectedStatus = "";
    private string selectedEventId = "";
    private string customerSearch = "";
    private DateTime? fromDate;
    private DateTime? toDate;

    // Event filter options, derived from the loaded orders (only events that have orders)
    private List<(int Id, string Name)> eventOptions = new();

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
                BuildEventOptions();

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

    private void BuildEventOptions()
    {
        eventOptions = allOrders
            .Where(o => o.EventId.HasValue)
            .GroupBy(o => o.EventId!.Value)
            .Select(g => (
                Id: g.Key,
                Name: g.Select(o => o.Event?.EventName).FirstOrDefault(n => !string.IsNullOrWhiteSpace(n))
                      ?? $"Event #{g.Key}"))
            .OrderBy(e => e.Name)
            .ToList();

        // Drop a stale selection if that event no longer has orders after a refresh
        if (!string.IsNullOrEmpty(selectedEventId) &&
            !eventOptions.Any(e => e.Id.ToString() == selectedEventId))
        {
            selectedEventId = "";
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

        if (!string.IsNullOrWhiteSpace(selectedEventId) && int.TryParse(selectedEventId, out var eventId))
        {
            query = query.Where(o => o.EventId == eventId);
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
        displayCount = PageSize;
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
        selectedEventId = "";
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
            foreach (var order in filteredOrders.Take(displayCount))
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
        var targets = selectedOrderIds
            .Select(id => allOrders.FirstOrDefault(o => o.Id == id))
            .Where(o => o != null && o!.Status == OrderStatus.Pending)
            .Select(o => o!.Id)
            .ToList();

        int succeeded = 0;
        try
        {
            foreach (var id in targets)
            {
                if (await AdminApiService.UpdateOrderStatusAsync(id, OrderStatus.Accepted))
                    succeeded++;
            }
        }
        catch (Exception)
        {
            // fall through to report whatever succeeded
        }

        if (succeeded > 0)
            await JSRuntime.InvokeVoidAsync("showToast", $"Accepted {succeeded} order(s)", "success");
        if (succeeded < targets.Count)
            await JSRuntime.InvokeVoidAsync("showToast", $"Failed to accept {targets.Count - succeeded} order(s)", "error");

        selectedOrderIds.Clear();
        await LoadOrders();
    }

    private async Task BulkCancel()
    {
        var targets = selectedOrderIds
            .Select(id => allOrders.FirstOrDefault(o => o.Id == id))
            .Where(o => o != null && o!.Status != OrderStatus.Delivered && o!.Status != OrderStatus.Cancelled)
            .Select(o => o!.Id)
            .ToList();

        int succeeded = 0;
        try
        {
            foreach (var id in targets)
            {
                if (await AdminApiService.UpdateOrderStatusAsync(id, OrderStatus.Cancelled))
                    succeeded++;
            }
        }
        catch (Exception)
        {
            // fall through to report whatever succeeded
        }

        if (succeeded > 0)
            await JSRuntime.InvokeVoidAsync("showToast", $"Cancelled {succeeded} order(s)", "warning");
        if (succeeded < targets.Count)
            await JSRuntime.InvokeVoidAsync("showToast", $"Failed to cancel {targets.Count - succeeded} order(s)", "error");

        selectedOrderIds.Clear();
        await LoadOrders();
    }

    private async Task AcceptOrder(int orderId)
    {
        try
        {
            var order = allOrders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return;

            if (await AdminApiService.UpdateOrderStatusAsync(orderId, OrderStatus.Accepted))
            {
                await JSRuntime.InvokeVoidAsync("showToast", $"Order #{orderId} accepted", "success");
                await LoadOrders();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to accept order", "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to accept order", "error");
        }
    }

    private async Task AdvanceOrderStatus(int orderId)
    {
        try
        {
            var order = allOrders.FirstOrDefault(o => o.Id == orderId);
            if (order == null)
                return;

            var nextStatus = GetNextStatus(order.Status);
            if (nextStatus == order.Status)
            {
                await JSRuntime.InvokeVoidAsync("showToast", $"Order #{orderId} cannot be advanced further", "warning");
                return;
            }

            if (await AdminApiService.UpdateOrderStatusAsync(orderId, nextStatus))
            {
                await JSRuntime.InvokeVoidAsync("showToast", $"Order #{orderId} advanced to {nextStatus}", "success");
                await LoadOrders();
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("showToast", "Failed to update order status", "error");
            }
        }
        catch (Exception)
        {
            await JSRuntime.InvokeVoidAsync("showToast", "Failed to update order status", "error");
        }
    }

    // Order fulfillment workflow: Pending -> Accepted -> InPreparation -> Ready -> OutForDelivery -> Delivered.
    // Terminal states (Delivered, Cancelled) return themselves.
    private static OrderStatus GetNextStatus(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => OrderStatus.Accepted,
            OrderStatus.Accepted => OrderStatus.InPreparation,
            OrderStatus.InPreparation => OrderStatus.Ready,
            OrderStatus.Ready => OrderStatus.OutForDelivery,
            OrderStatus.OutForDelivery => OrderStatus.Delivered,
            _ => status
        };
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
        displayCount = Math.Min(displayCount + PageSize, filteredOrders.Count);
        StateHasChanged();
    }

    // Console status pill modifier (dot + label) matching the dashboard design
    private string GetStatusModifier(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "is-pending",
            OrderStatus.Accepted => "is-accepted",
            OrderStatus.InPreparation => "is-preparing",
            OrderStatus.OutForDelivery => "is-preparing",
            OrderStatus.Ready => "is-ready",
            OrderStatus.Delivered => "is-delivered",
            OrderStatus.Cancelled => "is-cancelled",
            _ => "is-neutral"
        };
    }
}