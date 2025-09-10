using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class BartenderDashboard : ComponentBase, IDisposable
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private ISignalRService SignalRService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private StadiumLayoutDto? stadiumLayout;
    private List<OrderDto> pendingOrders = new();
    private List<OrderDto> activeOrders = new();
    private StadiumSeatDto? selectedSeat;
    private OrderDto? selectedOrder;

    protected override async Task OnInitializedAsync()
    {
        await LoadData();
        await InitializeSignalR();
    }

    private async Task LoadData()
    {
        stadiumLayout = await ApiService.GetStadiumLayoutAsync();
        var allOrders = await ApiService.GetOrdersAsync();
        pendingOrders = allOrders?.Where(o => o.Status == OrderStatus.Pending).ToList() ?? new();
        activeOrders = allOrders?.Where(o => o.Status == OrderStatus.Accepted || 
                                           o.Status == OrderStatus.InPreparation || 
                                           o.Status == OrderStatus.Ready).ToList() ?? new();
    }

    private async Task InitializeSignalR()
    {
        SignalRService.NewOrder += OnNewOrder;
        SignalRService.OrderUpdated += OnOrderUpdated;
        SignalRService.OrderStatusChanged += OnOrderStatusChanged;
        
        try
        {
            await SignalRService.StartAsync();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("No authentication token"))
        {
            Console.WriteLine("SignalR connection delayed: No authentication token available");
            // Token might not be available yet, retry after a short delay
            _ = Task.Run(async () =>
            {
                await Task.Delay(2000);
                try
                {
                    await SignalRService.StartAsync();
                    await InvokeAsync(StateHasChanged);
                }
                catch (Exception retryEx)
                {
                    Console.WriteLine($"SignalR connection failed on retry: {retryEx.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to initialize SignalR: {ex.Message}");
        }
    }

    private void OnNewOrder(OrderDto order)
    {
        if (order.Status == OrderStatus.Pending)
        {
            pendingOrders.Add(order);
            StateHasChanged();
        }
    }

    private void OnOrderUpdated(OrderDto order)
    {
        UpdateOrderInList(order);
        StateHasChanged();
    }

    private void OnOrderStatusChanged(int orderId, OrderStatus newStatus, string seatNumber)
    {
        var order = pendingOrders.FirstOrDefault(o => o.Id == orderId) ?? 
                   activeOrders.FirstOrDefault(o => o.Id == orderId);
        
        if (order != null)
        {
            order.Status = newStatus;
            UpdateOrderInList(order);
            StateHasChanged();
        }
    }

    private void UpdateOrderInList(OrderDto order)
    {
        pendingOrders.RemoveAll(o => o.Id == order.Id);
        activeOrders.RemoveAll(o => o.Id == order.Id);

        if (order.Status == OrderStatus.Pending)
        {
            pendingOrders.Add(order);
        }
        else if (order.Status == OrderStatus.Accepted || 
                 order.Status == OrderStatus.InPreparation || 
                 order.Status == OrderStatus.Ready)
        {
            activeOrders.Add(order);
        }
    }

    private Task SelectSeat(StadiumSeatDto seat)
    {
        selectedSeat = seat;
        if (seat.ActiveOrder != null)
        {
            selectedOrder = seat.ActiveOrder;
        }
        return Task.CompletedTask;
    }

    private Task SelectOrder(OrderDto order)
    {
        selectedOrder = order;
        return Task.CompletedTask;
    }

    private async Task UpdateOrderStatus(int orderId, OrderStatus newStatus)
    {
        var updateDto = new UpdateOrderStatusDto { Status = newStatus };
        var success = await ApiService.UpdateOrderStatusAsync(orderId, updateDto);
        if (success)
        {
            if (selectedOrder != null && selectedOrder.Id == orderId)
            {
                selectedOrder.Status = newStatus;
            }
            await LoadData();
            await SignalRService.SendOrderStatusChanged(orderId, newStatus, selectedOrder?.SeatNumber ?? "");
            StateHasChanged();
        }
    }

    private async Task RefreshData()
    {
        await LoadData();
        StateHasChanged();
    }

    private void CloseOrderDetails()
    {
        selectedOrder = null;
    }

    private string GetModernSeatColor(StadiumSeatDto seat)
    {
        if (!seat.HasActiveOrder) return "#22c55e";
        
        return seat.ActiveOrder?.Status switch
        {
            OrderStatus.Pending => "#fbbf24",
            OrderStatus.Accepted => "#fb923c",
            OrderStatus.InPreparation => "#3b82f6",
            OrderStatus.Ready => "#ef4444",
            _ => "#6b7280"
        };
    }
    
    private string GetModernSeatStroke(StadiumSeatDto seat)
    {
        if (!seat.HasActiveOrder) return "#16a34a";
        
        return seat.ActiveOrder?.Status switch
        {
            OrderStatus.Pending => "#f59e0b",
            OrderStatus.Accepted => "#ea580c",
            OrderStatus.InPreparation => "#2563eb",
            OrderStatus.Ready => "#dc2626",
            _ => "#4b5563"
        };
    }
    
    private string GetModernSeatClass(StadiumSeatDto seat)
    {
        if (!seat.HasActiveOrder) return "available";
        if (selectedSeat?.Id == seat.Id) return "selected";
        
        return seat.ActiveOrder?.Status switch
        {
            OrderStatus.Pending => "pending",
            OrderStatus.Accepted => "accepted",
            OrderStatus.InPreparation => "preparing",
            OrderStatus.Ready => "ready",
            _ => "occupied"
        };
    }
    
    private string GetSeatLabel(StadiumSeatDto seat)
    {
        // Only show labels for VIP or selected seats
        if (seat.Section == "VIP" || selectedSeat?.Id == seat.Id)
        {
            return seat.SeatNumber.ToString();
        }
        return "";
    }
    
    private string GetStatusClass(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "pending",
            OrderStatus.Accepted => "accepted",
            OrderStatus.InPreparation => "preparing",
            OrderStatus.Ready => "ready",
            OrderStatus.Delivered => "ready",
            OrderStatus.Cancelled => "occupied",
            _ => "occupied"
        };
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

    private string GetSeatClass(StadiumSeatDto seat)
    {
        return "stadium-seat" + (seat.HasActiveOrder ? " has-order" : "");
    }

    private string GetSeatStroke(StadiumSeatDto seat)
    {
        if (selectedSeat?.Id == seat.Id) return "#1f2937";
        if (seat.HasActiveOrder) return "#dc2626";
        return "#6b7280";
    }

    private int GetSeatStrokeWidth(StadiumSeatDto seat)
    {
        if (selectedSeat?.Id == seat.Id) return 3;
        if (seat.HasActiveOrder) return 2;
        return 1;
    }

    private string GetSectionLabelX(string sectionName)
    {
        return sectionName switch
        {
            "A" => "300",  // North-West
            "B" => "700",  // North-East
            "C" => "700",  // South-East
            "VIP" => "300", // South-West
            _ => "500"
        };
    }

    private string GetSectionLabelY(string sectionName)
    {
        return sectionName switch
        {
            "A" => "140",  // North-West
            "B" => "140",  // North-East
            "C" => "660",  // South-East
            "VIP" => "660", // South-West
            _ => "400"
        };
    }

    private (int X, int Y) GetModernSeatCoordinates(string sectionName, int seatIndex, int totalSeatsInSection)
    {
        var seatsPerRow = (int)Math.Ceiling(Math.Sqrt(totalSeatsInSection));
        var currentRow = seatIndex / seatsPerRow;
        var currentCol = seatIndex % seatsPerRow;

        // Map sections A, B, C, D to North, East, South, West stands
        var sectionBounds = sectionName switch
        {
            "A" => new { StartX = 180, StartY = 75, Width = 280, Height = 80 },      // North Stand (Left)
            "B" => new { StartX = 340, StartY = 75, Width = 280, Height = 80 },      // North Stand (Right)
            "C" => new { StartX = 695, StartY = 150, Width = 80, Height = 280 },     // East Stand
            "D" => new { StartX = 25, StartY = 150, Width = 80, Height = 280 },      // West Stand
            "VIP" => new { StartX = 180, StartY = 465, Width = 280, Height = 80 },   // South Stand
            _ => new { StartX = 300, StartY = 200, Width = 200, Height = 200 }
        };

        // For vertical sections (East/West), arrange seats vertically
        if (sectionName == "C" || sectionName == "D")
        {
            var verticalSeatsPerRow = Math.Max(1, (int)Math.Ceiling(totalSeatsInSection / 8.0)); // 8 rows vertically
            var verticalCurrentRow = seatIndex % 8;
            var verticalCurrentCol = seatIndex / 8;
            
            var x = sectionBounds.StartX + (verticalCurrentCol * (sectionBounds.Width / Math.Max(verticalSeatsPerRow, 1)));
            var y = sectionBounds.StartY + (verticalCurrentRow * (sectionBounds.Height / 8));
            return (x, y);
        }
        else
        {
            // Horizontal arrangement for North/South stands
            var seatSpacingX = sectionBounds.Width / Math.Max(seatsPerRow, 1);
            var seatSpacingY = sectionBounds.Height / Math.Max((totalSeatsInSection + seatsPerRow - 1) / seatsPerRow, 1);

            var x = sectionBounds.StartX + (currentCol * seatSpacingX);
            var y = sectionBounds.StartY + (currentRow * seatSpacingY);
            return (x, y);
        }
    }

    public void Dispose()
    {
        SignalRService.NewOrder -= OnNewOrder;
        SignalRService.OrderUpdated -= OnOrderUpdated;
        SignalRService.OrderStatusChanged -= OnOrderStatusChanged;
        _ = SignalRService.StopAsync();
    }
}