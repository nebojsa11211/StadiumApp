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
}