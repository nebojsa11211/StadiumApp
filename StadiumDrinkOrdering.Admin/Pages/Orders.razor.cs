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

    // Order detail modal (opened by the row "view" button)
    private OrderDto? viewOrder;
    private bool isLoadingDetail;

    // Summary data
    private int totalOrders = 0;
    private int pendingOrders = 0;
    private int completedToday = 0;
    private decimal revenueToday = 0;

    // Statistics — recomputed on every filter change so they describe the current selection
    private int statOrderCount = 0;
    private decimal statRevenue = 0;
    private decimal statAvgOrder = 0;
    private int statItemsSold = 0;
    private int statUniqueCustomers = 0;
    private List<StatusSlice> statusBreakdown = new();
    private List<DrinkStat> topDrinks = new();

    private record StatusSlice(OrderStatus Status, int Count, double Percent);
    private record DrinkStat(string Name, int Quantity);

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
        CalculateStatistics();
        StateHasChanged();
    }

    // Statistics describe the currently filtered set, so picking an event / date range / status
    // narrows the figures, the status distribution and the top-drinks ranking accordingly.
    private void CalculateStatistics()
    {
        statOrderCount = filteredOrders.Count;

        // Revenue and item counts exclude cancelled orders (no money changes hands on those).
        var billable = filteredOrders.Where(o => o.Status != OrderStatus.Cancelled).ToList();
        statRevenue = billable.Sum(o => o.TotalAmount);
        statAvgOrder = billable.Count > 0 ? statRevenue / billable.Count : 0m;
        statItemsSold = billable.Sum(o => o.OrderItems.Sum(i => i.Quantity));
        statUniqueCustomers = filteredOrders
            .Where(o => !string.IsNullOrWhiteSpace(o.CustomerName))
            .Select(o => o.CustomerName.Trim().ToLowerInvariant())
            .Distinct()
            .Count();

        statusBreakdown = filteredOrders
            .GroupBy(o => o.Status)
            .Select(g => new StatusSlice(
                g.Key,
                g.Count(),
                statOrderCount > 0 ? (double)g.Count() / statOrderCount * 100 : 0))
            .OrderByDescending(s => s.Count)
            .ToList();

        topDrinks = filteredOrders
            .SelectMany(o => o.OrderItems)
            .Where(i => !string.IsNullOrWhiteSpace(i.DrinkName))
            .GroupBy(i => i.DrinkName)
            .Select(g => new DrinkStat(g.Key, g.Sum(i => i.Quantity)))
            .OrderByDescending(d => d.Quantity)
            .Take(5)
            .ToList();
    }

    private string GetStatusLabel(OrderStatus status) => status switch
    {
        OrderStatus.Pending => L["Orders_PendingOption"].Value,
        OrderStatus.Accepted => L["Orders_AcceptedOption"].Value,
        OrderStatus.InPreparation => L["Orders_InPreparationOption"].Value,
        OrderStatus.Ready => L["Orders_ReadyOption"].Value,
        OrderStatus.OutForDelivery => L["Orders_OutForDeliveryOption"].Value,
        OrderStatus.Delivered => L["Orders_DeliveredOption"].Value,
        OrderStatus.Cancelled => L["Orders_CancelledOption"].Value,
        _ => status.ToString()
    };

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

    private async Task ViewOrder(int orderId)
    {
        // Show immediately with the row data we already have, then enrich from the API
        // (full detail carries the status timeline, payment and notes the list query omits).
        viewOrder = allOrders.FirstOrDefault(o => o.Id == orderId);
        if (viewOrder == null)
            return;

        isLoadingDetail = true;
        StateHasChanged();

        try
        {
            var detail = await AdminApiService.GetOrderAsync(orderId);
            if (detail != null)
                viewOrder = detail;
        }
        catch (Exception)
        {
            // Keep the row-level data already on screen if the detail fetch fails.
        }
        finally
        {
            isLoadingDetail = false;
            StateHasChanged();
        }
    }

    private void CloseOrderModal()
    {
        viewOrder = null;
        isLoadingDetail = false;
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