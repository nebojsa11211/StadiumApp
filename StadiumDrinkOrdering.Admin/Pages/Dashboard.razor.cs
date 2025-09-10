using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Dashboard : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    private List<OrderDto>? orders;
    private string alertMessage = "";
    private string alertType = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
    }

    private IEnumerable<OrderDto> pendingOrders => orders?.Where(o => o.Status == OrderStatus.Pending) ?? Enumerable.Empty<OrderDto>();
    private IEnumerable<OrderDto> activeOrders => orders?.Where(o => o.Status == OrderStatus.Accepted || o.Status == OrderStatus.InPreparation || o.Status == OrderStatus.Ready) ?? Enumerable.Empty<OrderDto>();
    private decimal todaysRevenue => orders?.Where(o => o.CreatedAt.Date == DateTime.Today && o.Status == OrderStatus.Delivered).Sum(o => o.TotalAmount) ?? 0;

    private async Task LoadData()
    {
        orders = await ApiService.GetOrdersAsync();
    }

    private async Task AcceptOrder(int orderId)
    {
        var updateDto = new UpdateOrderStatusDto { Status = OrderStatus.Accepted };
        var success = await ApiService.UpdateOrderStatusAsync(orderId, updateDto);
        if (success)
        {
            await LoadData();
            ShowAlert("Order accepted", "success");
        }
    }

    private async Task MarkInPreparation(int orderId)
    {
        var updateDto = new UpdateOrderStatusDto { Status = OrderStatus.InPreparation };
        var success = await ApiService.UpdateOrderStatusAsync(orderId, updateDto);
        if (success)
        {
            await LoadData();
            ShowAlert("Order marked as in preparation", "success");
        }
    }

    private async Task MarkReady(int orderId)
    {
        var updateDto = new UpdateOrderStatusDto { Status = OrderStatus.Ready };
        var success = await ApiService.UpdateOrderStatusAsync(orderId, updateDto);
        if (success)
        {
            await LoadData();
            ShowAlert("Order marked as ready", "success");
        }
    }

    private async Task MarkDelivered(int orderId)
    {
        var updateDto = new UpdateOrderStatusDto { Status = OrderStatus.Delivered };
        var success = await ApiService.UpdateOrderStatusAsync(orderId, updateDto);
        if (success)
        {
            await LoadData();
            ShowAlert("Order marked as delivered", "success");
        }
    }

    private string GetStatusBadgeClass(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "bg-warning text-dark",
            OrderStatus.Accepted => "bg-info",
            OrderStatus.InPreparation => "bg-primary",
            OrderStatus.Ready => "bg-success",
            OrderStatus.Delivered => "bg-success",
            OrderStatus.Cancelled => "bg-danger",
            _ => "bg-secondary"
        };
    }

    private string GetStatusProgressClass(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "bg-warning",
            OrderStatus.Accepted => "bg-info",
            OrderStatus.InPreparation => "bg-primary",
            OrderStatus.Ready => "bg-success",
            OrderStatus.Delivered => "bg-success",
            OrderStatus.Cancelled => "bg-danger",
            _ => "bg-secondary"
        };
    }

    private string GetStatusText(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "Pending",
            OrderStatus.Accepted => "Accepted",
            OrderStatus.InPreparation => "Preparing",
            OrderStatus.Ready => "Ready",
            OrderStatus.Delivered => "Delivered",
            OrderStatus.Cancelled => "Cancelled",
            _ => status.ToString()
        };
    }

    private void ShowAlert(string message, string type)
    {
        alertMessage = message;
        alertType = type;
        StateHasChanged();
        
        // Auto-hide after 3 seconds
        _ = Task.Delay(3000).ContinueWith(_ => 
        {
            alertMessage = "";
            InvokeAsync(StateHasChanged);
        });
    }

    private void ClearAlert()
    {
        alertMessage = "";
    }
}
