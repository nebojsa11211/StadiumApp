using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Orders : ComponentBase, IDisposable
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    private List<OrderDto>? orders;
    private OrderDto? selectedOrder;
    private TicketDto? selectedTicket;
    private string? ticketErrorMessage = null;
    private string selectedStatus = "";
    private string searchTerm = "";
    private string sortBy = "newest";
    private string viewMode = "table";
    private string alertMessage = "";
    private string alertType = "";
    private bool isLoadingOrders = false;
    private CancellationTokenSource? loadOrdersCts;
    private static int instanceCounter = 0;
    private int instanceId;

    public Orders()
    {
        instanceId = ++instanceCounter;
        Console.WriteLine($"Orders Component Constructor - Instance #{instanceId} created at {DateTime.Now:HH:mm:ss.fff}");
    }

    protected override async Task OnInitializedAsync()
    {
        Console.WriteLine($"Orders #{instanceId} - OnInitializedAsync called at {DateTime.Now:HH:mm:ss.fff}");
        await LoadOrders();
    }

    private async Task LoadOrders()
    {
        var callId = Guid.NewGuid().ToString()[..8];
        var stackTrace = Environment.StackTrace;
        Console.WriteLine($"🔍 LoadOrders ENTRY #{callId} at {DateTime.Now:HH:mm:ss.fff}");
        Console.WriteLine($"🔍 Call Stack (first 3 lines): {string.Join(" -> ", stackTrace.Split('\n').Take(3).Select(line => line.Trim()))}");
        
        // Prevent concurrent API calls
        if (isLoadingOrders)
        {
            Console.WriteLine($"🔍 LoadOrders #{callId}: Already loading, skipping duplicate call");
            return;
        }

        // Cancel any pending load operation
        loadOrdersCts?.Cancel();
        loadOrdersCts = new CancellationTokenSource();
        var token = loadOrdersCts.Token;

        try
        {
            isLoadingOrders = true;
            Console.WriteLine($"🔍 LoadOrders #{callId}: Setting isLoadingOrders=true, starting API call");
            
            var loadedOrders = await ApiService.GetOrdersAsync();
            
            // Check if this operation was cancelled
            if (token.IsCancellationRequested)
            {
                Console.WriteLine($"🔍 LoadOrders #{callId}: Operation was cancelled");
                return;
            }
            
            orders = loadedOrders;
            Console.WriteLine($"🔍 LoadOrders #{callId}: Completed successfully, loaded {orders?.Count ?? 0} orders");
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("LoadOrders: Operation cancelled");
        }
        catch (UnauthorizedAccessException ex)
        {
            Console.WriteLine($"LoadOrders: Authentication error - {ex.Message}");
            ShowAlert("Please log in to view orders", "warning");
            NavigationManager.NavigateTo("/login");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"LoadOrders: Error - {ex.Message}");
            ShowAlert($"Failed to load orders: {ex.Message}", "danger");
        }
        finally
        {
            isLoadingOrders = false;
            Console.WriteLine($"🔍 LoadOrders #{callId}: Setting isLoadingOrders=false in finally block");
        }
    }

    private async Task RefreshOrders()
    {
        Console.WriteLine($"🔍 RefreshOrders: User clicked refresh button at {DateTime.Now:HH:mm:ss.fff}");
        
        // Don't refresh if already loading
        if (isLoadingOrders)
        {
            Console.WriteLine("🔍 RefreshOrders: Already loading, skipping");
            return;
        }

        Console.WriteLine("🔍 RefreshOrders: Calling LoadOrders()");
        await LoadOrders();
        Console.WriteLine("🔍 RefreshOrders: LoadOrders() completed");
        
        // Only show success if orders were actually loaded
        if (orders != null && !isLoadingOrders)
        {
            ShowAlert("Orders refreshed", "success");
        }
    }

    private IEnumerable<OrderDto> filteredOrders
    {
        get
        {
            if (orders == null) return Enumerable.Empty<OrderDto>();

            var filtered = orders.AsEnumerable();

            // Status filter
            if (!string.IsNullOrEmpty(selectedStatus) && Enum.TryParse<OrderStatus>(selectedStatus, out var status))
            {
                filtered = filtered.Where(o => o.Status == status);
            }

            // Search filter
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var search = searchTerm.ToLower();
                filtered = filtered.Where(o => 
                    o.Id.ToString().Contains(search) ||
                    o.TicketNumber.ToLower().Contains(search) ||
                    o.SeatNumber.ToLower().Contains(search) ||
                    o.OrderItems.Any(i => i.DrinkName.ToLower().Contains(search)));
            }

            // Sort
            filtered = sortBy switch
            {
                "oldest" => filtered.OrderBy(o => o.CreatedAt),
                "amount" => filtered.OrderByDescending(o => o.TotalAmount),
                "status" => filtered.OrderBy(o => o.Status),
                _ => filtered.OrderByDescending(o => o.CreatedAt)
            };

            return filtered;
        }
    }

    private void FilterOrders()
    {
        // No need for async or StateHasChanged
        // The filteredOrders computed property will automatically update
        // and Blazor will detect the change and re-render
        Console.WriteLine($"FilterOrders: Filtering with status='{selectedStatus}', search='{searchTerm}', sort='{sortBy}'");
    }

    private async void ShowOrderDetails(OrderDto order)
    {
        try
        {
            Console.WriteLine($"ShowOrderDetails called for order #{order?.Id}");
            if (order == null)
            {
                Console.WriteLine("ERROR: Order is null in ShowOrderDetails");
                return;
            }
            
            selectedOrder = order;
            Console.WriteLine($"selectedOrder set to order #{selectedOrder.Id}");
            
            await InvokeAsync(() =>
            {
                StateHasChanged();
                Console.WriteLine("StateHasChanged called via InvokeAsync - modal should be visible now");
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERROR in ShowOrderDetails: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
        }
    }

    private async Task ShowTicketDetails(string ticketNumber)
    {
        Console.WriteLine($"ShowTicketDetails called for ticket: {ticketNumber}");
        
        if (string.IsNullOrEmpty(ticketNumber))
        {
            Console.WriteLine("Ticket number is empty");
            ticketErrorMessage = "Invalid ticket number";
            return;
        }

        try
        {
            ticketErrorMessage = null;
            Console.WriteLine($"Fetching ticket details for: {ticketNumber}");
            selectedTicket = await ApiService.GetTicketByNumberAsync(ticketNumber);
            
            if (selectedTicket == null)
            {
                Console.WriteLine($"Ticket not found: {ticketNumber}");
                ticketErrorMessage = $"Ticket '{ticketNumber}' not found in the database. This may be a legacy ticket number or the ticket may have been deleted.";
                selectedTicket = new TicketDto { TicketNumber = ticketNumber };
            }
            else
            {
                Console.WriteLine($"Ticket found: {ticketNumber}");
            }
        }
        catch (Exception ex)
        {
            ticketErrorMessage = $"Error loading ticket details: {ex.Message}";
            selectedTicket = new TicketDto { TicketNumber = ticketNumber };
            Console.WriteLine($"Error loading ticket details: {ex}");
        }
        
        Console.WriteLine($"selectedTicket set, calling StateHasChanged");
        await InvokeAsync(StateHasChanged);
    }

    private async Task AdvanceOrderStatus(OrderDto order)
    {
        var nextStatus = GetNextStatus(order.Status);
        if (nextStatus.HasValue)
        {
            var updateDto = new UpdateOrderStatusDto { Status = nextStatus.Value };
            var success = await ApiService.UpdateOrderStatusAsync(order.Id, updateDto);
            
            if (success)
            {
                await LoadOrders();
                selectedOrder = null;
                ShowAlert($"Order #{order.Id} updated to {GetStatusText(nextStatus.Value)}", "success");
            }
            else
            {
                ShowAlert("Failed to update order status", "danger");
            }
        }
    }

    private bool CanUpdateStatus(OrderStatus status)
    {
        return status == OrderStatus.Pending || 
               status == OrderStatus.Accepted || 
               status == OrderStatus.InPreparation || 
               status == OrderStatus.Ready;
    }

    private OrderStatus? GetNextStatus(OrderStatus currentStatus)
    {
        return currentStatus switch
        {
            OrderStatus.Pending => OrderStatus.Accepted,
            OrderStatus.Accepted => OrderStatus.InPreparation,
            OrderStatus.InPreparation => OrderStatus.Ready,
            OrderStatus.Ready => OrderStatus.Delivered,
            _ => null
        };
    }

    private string GetNextStatusAction(OrderStatus currentStatus)
    {
        return currentStatus switch
        {
            OrderStatus.Pending => "Accept",
            OrderStatus.Accepted => "Start Preparing",
            OrderStatus.InPreparation => "Mark Ready",
            OrderStatus.Ready => "Mark Delivered",
            _ => ""
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

    private void CloseOrderModal()
    {
        Console.WriteLine("CloseOrderModal called");
        selectedOrder = null;
        StateHasChanged();
    }

    private void CloseTicketModal()
    {
        Console.WriteLine("CloseTicketModal called");
        selectedTicket = null;
        ticketErrorMessage = null;
        StateHasChanged();
    }

    private async Task CreateTestOrder()
    {
        try
        {
            Console.WriteLine("Creating test order...");
            
            // Create a dummy test order to verify the details button works
            var testOrder = new OrderDto
            {
                Id = 63,
                TicketNumber = "TK005",
                SeatNumber = "A-1-5",
                TotalAmount = 25.50m,
                Status = OrderStatus.Pending,
                CreatedAt = DateTime.Now,
                CustomerNotes = "Test order for debugging",
                OrderItems = new List<OrderItemDto>
                {
                    new OrderItemDto
                    {
                        Id = 1,
                        DrinkName = "Test Beer",
                        Quantity = 2,
                        UnitPrice = 12.00m,
                        TotalPrice = 24.00m,
                        SpecialInstructions = "Cold please"
                    },
                    new OrderItemDto
                    {
                        Id = 2,
                        DrinkName = "Water",
                        Quantity = 1,
                        UnitPrice = 1.50m,
                        TotalPrice = 1.50m
                    }
                }
            };
            
            // Add to orders list if it doesn't exist
            if (orders == null)
            {
                orders = new List<OrderDto>();
            }
            
            // Remove any existing order with ID 63
            orders.RemoveAll(o => o.Id == 63);
            
            // Add the test order
            orders.Add(testOrder);
            
            ShowAlert("Test order #63 created successfully. You can now test the details button.", "success");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating test order: {ex.Message}");
            ShowAlert($"Failed to create test order: {ex.Message}", "danger");
        }
    }

    public void Dispose()
    {
        loadOrdersCts?.Cancel();
        loadOrdersCts?.Dispose();
    }
}
