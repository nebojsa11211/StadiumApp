using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Text.Json;

namespace StadiumDrinkOrdering.Admin.Components.Pages;

public partial class Analytics
{
    [Inject] private HttpClient Http { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private bool _isLoading = true;
    private string _selectedPeriod = "today";
    private AnalyticsData _analytics = new();
    private List<RealtimeActivity> _realtimeActivities = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadAnalytics();
        await LoadCharts();
        StartRealtimeUpdates();
    }

    private async Task LoadAnalytics()
    {
        try
        {
            _isLoading = true;
            StateHasChanged();

            // Simulate loading delay
            await Task.Delay(1000);

            // Mock analytics data - replace with actual API call
            _analytics = new AnalyticsData
            {
                TotalOrders = 1247,
                OrdersGrowth = 12.5,
                TotalRevenue = 18420.50m,
                RevenueGrowth = 8.3,
                AvgOrderValue = 14.78m,
                AovGrowth = -2.1,
                AvgFulfillmentTime = "8m 32s",
                FulfillmentImprovement = 15.2,
                CustomerSatisfaction = 92.5,
                OrderAccuracy = 96.8,
                OnTimeDelivery = 89.2,
                PopularItems = new List<PopularItem>
                {
                    new() { Name = "Premium Lager", OrderCount = 245 },
                    new() { Name = "Craft IPA", OrderCount = 198 },
                    new() { Name = "Soft Drinks", OrderCount = 167 },
                    new() { Name = "Water", OrderCount = 134 },
                    new() { Name = "Premium Wine", OrderCount = 89 },
                    new() { Name = "Hot Coffee", OrderCount = 67 }
                },
                SectionActivity = Enumerable.Range(1, 20).Select(i => new SectionActivity 
                { 
                    SectionNumber = i, 
                    ActivityLevel = Random.Shared.Next(10, 100) 
                }).ToList()
            };

            _isLoading = false;
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _isLoading = false;
            StateHasChanged();
            await JSRuntime.InvokeVoidAsync("showToast", $"Error loading analytics: {ex.Message}", "error", 5000);
        }
    }

    private async Task LoadCharts()
    {
        try
        {
            // Initialize Chart.js charts
            await JSRuntime.InvokeVoidAsync("initializeAnalyticsCharts", _analytics);
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("showToast", $"Error loading charts: {ex.Message}", "warning", 3000);
        }
    }

    private void StartRealtimeUpdates()
    {
        // Simulate real-time activity updates
        var timer = new Timer(async _ =>
        {
            var activities = new[]
            {
                "New order placed in Section 12",
                "Order #1234 completed",
                "Payment processed for Order #5678",
                "Staff member checked in at Bar Station 3",
                "Inventory alert: Premium Lager running low",
                "Customer rated service 5 stars",
                "Order #9876 ready for pickup"
            };

            var activity = new RealtimeActivity
            {
                Timestamp = DateTime.Now,
                Message = activities[Random.Shared.Next(activities.Length)],
                Type = new[] { "Order", "Payment", "Staff", "Inventory", "Rating" }[Random.Shared.Next(5)]
            };

            _realtimeActivities.Insert(0, activity);
            if (_realtimeActivities.Count > 20)
                _realtimeActivities.RemoveAt(_realtimeActivities.Count - 1);

            await InvokeAsync(StateHasChanged);
        }, null, TimeSpan.FromSeconds(3), TimeSpan.FromSeconds(5));
    }

    public class AnalyticsData
    {
        public int TotalOrders { get; set; }
        public double OrdersGrowth { get; set; }
        public decimal TotalRevenue { get; set; }
        public double RevenueGrowth { get; set; }
        public decimal AvgOrderValue { get; set; }
        public double AovGrowth { get; set; }
        public string AvgFulfillmentTime { get; set; } = "";
        public double FulfillmentImprovement { get; set; }
        public double CustomerSatisfaction { get; set; }
        public double OrderAccuracy { get; set; }
        public double OnTimeDelivery { get; set; }
        public List<PopularItem> PopularItems { get; set; } = new();
        public List<SectionActivity> SectionActivity { get; set; } = new();
    }

    public class PopularItem
    {
        public string Name { get; set; } = "";
        public int OrderCount { get; set; }
    }

    public class SectionActivity
    {
        public int SectionNumber { get; set; }
        public int ActivityLevel { get; set; }
    }

    public class RealtimeActivity
    {
        public DateTime Timestamp { get; set; }
        public string Message { get; set; } = "";
        public string Type { get; set; } = "";
    }
}