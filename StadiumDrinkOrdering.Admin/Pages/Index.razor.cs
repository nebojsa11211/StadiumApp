using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
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
    
    protected override async Task OnInitializedAsync()
    {
        await LoadDashboardData();
        
        // Set up auto-refresh every 30 seconds
        refreshTimer = new System.Threading.Timer(
            async _ => await InvokeAsync(async () =>
            {
                await LoadDashboardData();
                StateHasChanged();
            }),
            null,
            TimeSpan.FromSeconds(30),
            TimeSpan.FromSeconds(30)
        );
    }
    
    private async Task LoadDashboardData()
    {
        try
        {
            // Load today's revenue
            try
            {
                var revenueData = await ApiService.GetAsync<RevenueData>("analytics/revenue/today");
                if (revenueData != null)
                {
                    todayRevenue = revenueData.TodayRevenue;
                    revenueChange = revenueData.ChangePercentage;
                }
            }
            catch { /* Keep existing value on error */ }
            
            // Load order statistics
            try
            {
                var orderStats = await ApiService.GetAsync<OrderStatistics>("orders/statistics");
                if (orderStats != null)
                {
                    activeOrders = orderStats.Active;
                    pendingOrders = orderStats.Pending;
                    inPreparationOrders = orderStats.InPreparation;
                }
            }
            catch { /* Keep existing value on error */ }
            
            // Load ticket statistics
            try
            {
                var ticketStats = await ApiService.GetAsync<TicketStatistics>("tickets/statistics/today");
                if (ticketStats != null)
                {
                    ticketsSoldToday = ticketStats.SoldToday;
                    currentEventName = ticketStats.CurrentEventName ?? "No active event";
                }
            }
            catch { /* Keep existing value on error */ }
            
            // Load user statistics
            try
            {
                var userStats = await ApiService.GetAsync<UserStatistics>("users/online-statistics");
                if (userStats != null)
                {
                    onlineUsers = userStats.TotalOnline;
                    staffOnline = userStats.StaffOnline;
                    customersOnline = userStats.CustomersOnline;
                }
            }
            catch { /* Keep existing value on error */ }
            
            // Load recent activity
            try
            {
                var activities = await ApiService.GetAsync<List<ActivityItem>>("logs/recent-activity?limit=10");
                if (activities != null)
                {
                    recentActivities = activities;
                }
            }
            catch { /* Keep existing value on error */ }
            
            // Load recent errors count
            try
            {
                var errorCount = await ApiService.GetAsync<int>("logs/recent-errors-count");
                recentErrors = errorCount;
            }
            catch { /* Keep existing value on error */ }
        }
        catch (Exception)
        {
            // Only load mock data on initial load if all calls fail
            if (isFirstLoad && todayRevenue == 0 && activeOrders == 0 && ticketsSoldToday == 0)
            {
                LoadMockData();
            }
        }
        
        // Mark that first load is complete
        isFirstLoad = false;
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
            Console.WriteLine("🔍 ShowOnlineUsers method called");

            loadingModalData = true;
            StateHasChanged();

            try
            {
                Console.WriteLine("🔍 Loading online users from API...");
                // Load detailed online users list
                var users = await ApiService.GetAsync<List<OnlineUser>>("users/online-detailed");
                if (users != null)
                {
                    onlineUsersList = users;
                    Console.WriteLine($"✅ Loaded {users.Count} users from API");
                }
                else
                {
                    Console.WriteLine("⚠️ API returned null, using mock data");
                    // Mock data for demonstration
                    onlineUsersList = new List<OnlineUser>
                    {
                        new() { Name = "John Smith", Email = "john@stadium.com", Role = "Staff", LastActivity = DateTime.Now.AddMinutes(-2) },
                        new() { Name = "Sarah Johnson", Email = "sarah@stadium.com", Role = "Staff", LastActivity = DateTime.Now.AddMinutes(-5) },
                        new() { Name = "Mike Wilson", Email = "mike@example.com", Role = "Customer", LastActivity = DateTime.Now.AddMinutes(-1) },
                        new() { Name = "Emma Davis", Email = "emma@example.com", Role = "Customer", LastActivity = DateTime.Now.AddMinutes(-3) }
                    };
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
    
    public void Dispose()
    {
        refreshTimer?.Dispose();
    }
    
    // Data models
    private class ActivityItem
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }
    
    private class RevenueData
    {
        public decimal TodayRevenue { get; set; }
        public decimal ChangePercentage { get; set; }
    }
    
    private class OrderStatistics
    {
        public int Active { get; set; }
        public int Pending { get; set; }
        public int InPreparation { get; set; }
    }
    
    private class TicketStatistics
    {
        public int SoldToday { get; set; }
        public string? CurrentEventName { get; set; }
    }
    
    private class UserStatistics
    {
        public int TotalOnline { get; set; }
        public int StaffOnline { get; set; }
        public int CustomersOnline { get; set; }
    }

    private class OnlineUser
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime LastActivity { get; set; }
    }

    private class ActiveOrder
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    private class RevenueDetails
    {
        public decimal DrinkRevenue { get; set; }
        public decimal TicketRevenue { get; set; }
        public decimal ServiceFees { get; set; }
        public decimal YesterdayTotal { get; set; }
        public decimal LastWeekTotal { get; set; }
        public decimal LastMonthTotal { get; set; }
        public List<TopItem> TopItems { get; set; } = new();
    }

    private class TopItem
    {
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    private class TicketDetails
    {
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public int TotalCapacity { get; set; }
        public int SoldToday { get; set; }
        public int TotalSold { get; set; }
        public int Available { get; set; }
        public List<SectionSale> SectionSales { get; set; } = new();
    }

    private class SectionSale
    {
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public int Sold { get; set; }
    }
}