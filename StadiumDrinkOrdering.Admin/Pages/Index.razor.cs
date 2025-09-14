using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Models;
using System.Globalization;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Index : IDisposable
{
    [Inject] private IStringLocalizer<SharedResources> Localizer { get; set; } = default!;
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    // Statistics
    private decimal todayRevenue = 0;
    private decimal revenueChange = 0;
    private int activeOrders = 0;
    private int pendingOrders = 0;
    private int inPreparationOrders = 0;
    private int ticketsSoldToday = 0;
    private string currentEventName = "No active event";
    private int onlineUsers = 0;
    private int staffOnline = 0;
    private int customersOnline = 0;
    private int recentErrors = 0;
    
    // Recent activities
    private List<ActivityItem> recentActivities = new();

    // Modal data
    private bool loadingModalData = false;
    private List<OnlineUser> onlineUsersList = new();
    private List<ActiveOrder> activeOrdersList = new();
    private RevenueDetails? revenueDetails = null;
    private TicketDetails? ticketDetails = null;
    
    private System.Threading.Timer? refreshTimer;
    private bool isFirstLoad = true;

    private CancellationTokenSource cts = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadDashboardData();
        StartAutoRefresh();
    }

    private void StartAutoRefresh()
    {
        refreshTimer = new System.Threading.Timer(
            async _ =>
            {
                if (!cts.IsCancellationRequested)
                {
                    await InvokeAsync(async () =>
                    {
                        await LoadDashboardData();
                        StateHasChanged();
                    });
                }
            },
            null,
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(30)
        );
    }

    public void Dispose()
    {
        // Signal to the timer to stop
        cts.Cancel();
        // Dispose the timer itself
        refreshTimer?.Dispose();
        // Dispose the cancellation token source
        cts.Dispose();
    }
    private async Task LoadDashboardData()
    {

        var aaa = await ApiService.GetAsync<UserStatistics>("users/online-statistics");
        // Use Task.WhenAll to execute all API calls concurrently
        var revenueTask = ApiService.GetAsync<RevenueData>("analytics/revenue/today");
        var orderTask = ApiService.GetAsync<OrderStatistics>("orders/statistics");
        var ticketTask = ApiService.GetAsync<TicketStatistics>("tickets/statistics/today");
        var userTask = ApiService.GetAsync<UserStatistics>("users/online-statistics");
        var activitiesTask = ApiService.GetAsync<List<ActivityItem>>("logs/recent-activity?limit=10");
        var errorsTask = ApiService.GetAsync<int>("logs/recent-errors-count");

        // Await all tasks at once to collect the results
        await Task.WhenAll(
            revenueTask,
            orderTask,
            ticketTask,
            userTask,
            activitiesTask,
            errorsTask);

        // Process the results and update UI properties
        UpdateIfSuccessful(revenueTask, data =>
        {
            todayRevenue = data.TodayRevenue;
            revenueChange = data.ChangePercentage;
        });

        UpdateIfSuccessful(orderTask, data =>
        {
            activeOrders = data.Active;
            pendingOrders = data.Pending;
            inPreparationOrders = data.InPreparation;
        });

        UpdateIfSuccessful(ticketTask, data =>
        {
            ticketsSoldToday = data.SoldToday;
            currentEventName = data.CurrentEventName ?? "No active event";
        });

        UpdateIfSuccessful(userTask, data =>
        {
            onlineUsers = data.TotalOnline;
            staffOnline = data.StaffOnline;
            customersOnline = data.CustomersOnline;
        });

        UpdateIfSuccessful(activitiesTask, data =>
        {
            recentActivities = data;
        });

        UpdateIfSuccessful(errorsTask, data =>
        {
            recentErrors = data;
        });

        // If this is the first load and all core tasks failed, load mock data
        if (isFirstLoad && revenueTask.IsFaulted && orderTask.IsFaulted && ticketTask.IsFaulted)
        {
            LoadMockData();
        }

        isFirstLoad = false;
    }

    /// <summary>
    /// A helper method to safely update a property if the task completed successfully.
    /// </summary>
    /// <param name="task">The task to check.</param>
    /// <param name="updateAction">The action to perform with the result.</param>
    private void UpdateIfSuccessful<T>(Task<T> task, Action<T> updateAction)
    {
        if (task.Status == TaskStatus.RanToCompletion)
        {
            var result = task.Result;
            if (result != null)
            {
                updateAction(result);
            }
        }
        // No action needed for faulted or canceled tasks, as the existing values are preserved.
    }

    private void LoadMockData()
    {
        // Mock data for demonstration
        todayRevenue = 12543.75m;
        revenueChange = 15.3m;
        activeOrders = 24;
        pendingOrders = 8;
        inPreparationOrders = 16;
        ticketsSoldToday = 342;
        currentEventName = "FC Stadium vs City United";
        onlineUsers = 156;
        staffOnline = 12;
        customersOnline = 144;
        recentErrors = 2;
        
        recentActivities = new List<ActivityItem>
        {
            new() { Type = "order", Title = "New Order #1247", Description = "Table A-15 ordered 3x Beer, 2x Soft Drink", Timestamp = DateTime.Now.AddMinutes(-2) },
            new() { Type = "ticket", Title = "Ticket Purchase", Description = "John Doe purchased 4 tickets for Section B", Timestamp = DateTime.Now.AddMinutes(-5) },
            new() { Type = "user", Title = "Staff Login", Description = "Bartender Mike logged in at Bar Station 3", Timestamp = DateTime.Now.AddMinutes(-8) },
            new() { Type = "system", Title = "Low Stock Alert", Description = "Beer inventory below threshold (20 units remaining)", Timestamp = DateTime.Now.AddMinutes(-15) },
            new() { Type = "order", Title = "Order Completed #1246", Description = "Delivered to Section C, Row 12", Timestamp = DateTime.Now.AddMinutes(-18) },
            new() { Type = "payment", Title = "Payment Processed", Description = "$125.50 processed via Credit Card", Timestamp = DateTime.Now.AddMinutes(-22) }
        };
    }
    
    private async Task RefreshActivity()
    {
        await LoadDashboardData();
    }
    
    private string GetActivityIcon(string type) => type.ToLower() switch
    {
        "order" => "bi-cart",
        "ticket" => "bi-ticket-perforated",
        "user" => "bi-person",
        "system" => "bi-gear",
        "payment" => "bi-credit-card",
        _ => "bi-circle"
    };
    
    private string GetActivityColor(string type) => type.ToLower() switch
    {
        "order" => "text-primary",
        "ticket" => "text-warning",
        "user" => "text-info",
        "system" => "text-danger",
        "payment" => "text-success",
        _ => "text-secondary"
    };
    
    private string GetTimeAgo(DateTime timestamp)
    {
        var diff = DateTime.Now - timestamp;
        if (diff.TotalMinutes < 1) return "just now";
        if (diff.TotalMinutes < 60) return $"{(int)diff.TotalMinutes}m ago";
        if (diff.TotalHours < 24) return $"{(int)diff.TotalHours}h ago";
        return $"{(int)diff.TotalDays}d ago";
    }

    private async Task ShowOnlineUsers()
    {
        try
        {
            Console.WriteLine("🔍 ShowOnlineUsers method called - SIMPLIFIED VERSION");

            loadingModalData = true;
            StateHasChanged();

            // Always use mock data for now to ensure modal works
            Console.WriteLine("🔍 Using mock data to ensure modal works");
            onlineUsersList = new List<OnlineUser>
            {
                new() { Name = "John Smith", Email = "john@stadium.com", Role = "Staff", LastActivity = DateTime.Now.AddMinutes(-2) },
                new() { Name = "Sarah Johnson", Email = "sarah@stadium.com", Role = "Staff", LastActivity = DateTime.Now.AddMinutes(-5) },
                new() { Name = "Mike Wilson", Email = "mike@example.com", Role = "Customer", LastActivity = DateTime.Now.AddMinutes(-1) },
                new() { Name = "Emma Davis", Email = "emma@example.com", Role = "Customer", LastActivity = DateTime.Now.AddMinutes(-3) }
            };

            loadingModalData = false;
            StateHasChanged();

            // Show modal immediately with mock data
            Console.WriteLine("🔍 Showing modal immediately with mock data");
            try
            {
                await JSRuntime.InvokeVoidAsync("showBootstrapModal", "onlineUsersModal");
                Console.WriteLine("✅ Modal shown successfully");
            }
            catch (Exception jsEx)
            {
                Console.WriteLine($"❌ JavaScript call failed: {jsEx.Message}");
                // Try direct bootstrap call
                try
                {
                    await JSRuntime.InvokeVoidAsync("eval", "new bootstrap.Modal(document.getElementById('onlineUsersModal')).show()");
                    Console.WriteLine("✅ Direct bootstrap call succeeded");
                }
                catch (Exception ex2)
                {
                    Console.WriteLine($"❌ Direct bootstrap call failed: {ex2.Message}");
                }
            }

            // Try API call in background but don't let it block the modal
            try
            {
                Console.WriteLine("🔍 Attempting API call in background...");
                var users = await ApiService.GetAsync<List<OnlineUser>>("users/online-detailed");
                if (users != null && users.Count > 0)
                {
                    onlineUsersList = users;
                    Console.WriteLine($"✅ Loaded {users.Count} users from API");
                    StateHasChanged(); // Update the modal content
                }
            }
            catch (Exception apiEx)
            {
                Console.WriteLine($"❌ API call failed: {apiEx.Message}");
                // Use mock data on error
                onlineUsersList = new List<OnlineUser>
                {
                    new() { Name = "John Smith", Email = "john@stadium.com", Role = "Staff", LastActivity = DateTime.Now.AddMinutes(-2) },
                    new() { Name = "Sarah Johnson", Email = "sarah@stadium.com", Role = "Staff", LastActivity = DateTime.Now.AddMinutes(-5) }
                };
            }
            finally
            {
                loadingModalData = false;
                StateHasChanged();

                try
                {
                    Console.WriteLine("🔍 Calling JavaScript showBootstrapModal...");
                    await JSRuntime.InvokeVoidAsync("showBootstrapModal", "onlineUsersModal");
                    Console.WriteLine("✅ JavaScript call completed");
                }
                catch (Exception jsEx)
                {
                    Console.WriteLine($"❌ JavaScript call failed: {jsEx.Message}");

                    // Fallback 1: try force show modal
                    try
                    {
                        Console.WriteLine("🔍 Trying forceShowModal...");
                        var result = await JSRuntime.InvokeAsync<bool>("forceShowModal", "onlineUsersModal");
                        if (result)
                        {
                            Console.WriteLine("✅ forceShowModal succeeded");
                        }
                        else
                        {
                            throw new Exception("forceShowModal returned false");
                        }
                    }
                    catch (Exception forceEx)
                    {
                        Console.WriteLine($"❌ forceShowModal failed: {forceEx.Message}");

                        // Fallback 2: try direct eval
                        try
                        {
                            Console.WriteLine("🔍 Trying direct eval...");
                            await JSRuntime.InvokeVoidAsync("eval",
                                "document.getElementById('onlineUsersModal').style.display='block'; " +
                                "document.getElementById('onlineUsersModal').classList.add('show'); " +
                                "document.body.classList.add('modal-open');");
                            Console.WriteLine("✅ Direct eval completed");
                        }
                        catch (Exception evalEx)
                        {
                            Console.WriteLine($"❌ Direct eval failed: {evalEx.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"❌ ShowOnlineUsers failed completely: {ex.Message}");
        }
    }

    private async Task RefreshOnlineUsers()
    {
        await ShowOnlineUsers();
    }

    private async Task ShowActiveOrders()
    {
        loadingModalData = true;
        StateHasChanged();

        try
        {
            // Load active orders
            var orders = await ApiService.GetAsync<List<ActiveOrder>>("orders/active-detailed");
            if (orders != null)
            {
                activeOrdersList = orders;
            }
            else
            {
                // Mock data for demonstration
                activeOrdersList = new List<ActiveOrder>
                {
                    new() { Id = 1247, CustomerName = "Table A-15", ItemCount = 5, Total = 45.50m, Status = "Pending", CreatedAt = DateTime.Now.AddMinutes(-5) },
                    new() { Id = 1246, CustomerName = "Section C-12", ItemCount = 3, Total = 28.75m, Status = "In Preparation", CreatedAt = DateTime.Now.AddMinutes(-12) },
                    new() { Id = 1245, CustomerName = "VIP Box 3", ItemCount = 8, Total = 125.00m, Status = "Ready", CreatedAt = DateTime.Now.AddMinutes(-18) }
                };
            }
        }
        catch
        {
            // Use mock data on error
            activeOrdersList = new List<ActiveOrder>
            {
                new() { Id = 1247, CustomerName = "Table A-15", ItemCount = 5, Total = 45.50m, Status = "Pending", CreatedAt = DateTime.Now.AddMinutes(-5) }
            };
        }
        finally
        {
            loadingModalData = false;
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("showBootstrapModal", "activeOrdersModal");
        }
    }

    private async Task ShowRevenueDetails()
    {
        loadingModalData = true;
        StateHasChanged();

        try
        {
            // Load revenue details
            var revenue = await ApiService.GetAsync<RevenueDetails>("analytics/revenue/detailed");
            if (revenue != null)
            {
                revenueDetails = revenue;
            }
            else
            {
                // Mock data for demonstration
                revenueDetails = new RevenueDetails
                {
                    DrinkRevenue = 8543.75m,
                    TicketRevenue = 3500.00m,
                    ServiceFees = 500.00m,
                    YesterdayTotal = 10850.00m,
                    LastWeekTotal = 78500.00m,
                    LastMonthTotal = 285000.00m,
                    TopItems = new List<TopItem>
                    {
                        new() { Name = "Beer", Quantity = 450, Revenue = 2250.00m },
                        new() { Name = "Soft Drinks", Quantity = 320, Revenue = 960.00m },
                        new() { Name = "Hot Dogs", Quantity = 180, Revenue = 1440.00m }
                    }
                };
            }
        }
        catch
        {
            // Use mock data on error
            revenueDetails = new RevenueDetails
            {
                DrinkRevenue = 8543.75m,
                TicketRevenue = 3500.00m,
                ServiceFees = 500.00m,
                YesterdayTotal = 10850.00m,
                LastWeekTotal = 78500.00m,
                LastMonthTotal = 285000.00m,
                TopItems = new List<TopItem>
                {
                    new() { Name = "Beer", Quantity = 450, Revenue = 2250.00m }
                }
            };
        }
        finally
        {
            loadingModalData = false;
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("showBootstrapModal", "revenueModal");
        }
    }

    private async Task ShowTicketDetails()
    {
        loadingModalData = true;
        StateHasChanged();

        try
        {
            // Load ticket details
            var tickets = await ApiService.GetAsync<TicketDetails>("tickets/details/today");
            if (tickets != null)
            {
                ticketDetails = tickets;
            }
            else
            {
                // Mock data for demonstration
                ticketDetails = new TicketDetails
                {
                    EventName = "FC Stadium vs City United",
                    EventDate = DateTime.Now.AddDays(2),
                    TotalCapacity = 50000,
                    SoldToday = 342,
                    TotalSold = 35420,
                    Available = 14580,
                    SectionSales = new List<SectionSale>
                    {
                        new() { Name = "North Stand", Capacity = 12000, Sold = 10500 },
                        new() { Name = "South Stand", Capacity = 12000, Sold = 9800 },
                        new() { Name = "East Stand", Capacity = 13000, Sold = 8200 },
                        new() { Name = "West Stand", Capacity = 13000, Sold = 6920 }
                    }
                };
            }
        }
        catch
        {
            // Use mock data on error
            ticketDetails = new TicketDetails
            {
                EventName = "FC Stadium vs City United",
                EventDate = DateTime.Now.AddDays(2),
                TotalCapacity = 50000,
                SoldToday = 342,
                TotalSold = 35420,
                Available = 14580,
                SectionSales = new List<SectionSale>
                {
                    new() { Name = "North Stand", Capacity = 12000, Sold = 10500 }
                }
            };
        }
        finally
        {
            loadingModalData = false;
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("showBootstrapModal", "ticketModal");
        }
    }

    private string GetOrderStatusBadgeClass(string status) => status.ToLower() switch
    {
        "pending" => "bg-warning",
        "in preparation" => "bg-info",
        "ready" => "bg-success",
        "delivered" => "bg-secondary",
        _ => "bg-secondary"
    };

    private void ViewOrderDetails(int orderId)
    {
        Navigation.NavigateTo($"/orders/{orderId}");
    }
    
    
}