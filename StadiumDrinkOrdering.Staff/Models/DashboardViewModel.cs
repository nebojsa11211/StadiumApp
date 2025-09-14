using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Staff.Models;

public class DashboardViewModel
{
    // Order Metrics
    public int PendingOrdersCount { get; set; }
    public int AcceptedOrdersCount { get; set; }
    public int InPreparationOrdersCount { get; set; }
    public int ReadyOrdersCount { get; set; }
    public int TotalOrdersToday { get; set; }

    // Order Collections
    public List<OrderDto> RecentOrders { get; set; } = new();
    public List<OrderDto> PendingOrders { get; set; } = new();
    public List<OrderDto> AcceptedOrders { get; set; } = new();
    public List<OrderDto> InPreparationOrders { get; set; } = new();
    public List<OrderDto> ReadyOrders { get; set; } = new();

    // UI Components Data
    public List<QuickAction> QuickActions { get; set; } = new();

    // Metadata
    public DateTime LastUpdated { get; set; }
    public bool IsLoading { get; set; }
    public string? ErrorMessage { get; set; }

    // Helper Properties
    public int TotalActiveOrders => PendingOrdersCount + AcceptedOrdersCount + InPreparationOrdersCount + ReadyOrdersCount;
    public bool HasOrders => TotalActiveOrders > 0;
    public bool HasRecentOrders => RecentOrders.Any();

    // Dashboard Cards Data
    public List<DashboardCardData> GetDashboardCards()
    {
        return new List<DashboardCardData>
        {
            new DashboardCardData
            {
                Title = "Pending Orders",
                Value = PendingOrdersCount.ToString(),
                IconClass = "oi oi-clock",
                BackgroundClass = "bg-primary",
                TextColorClass = "text-white",
                Subtitle = PendingOrdersCount == 1 ? "order awaiting acceptance" : "orders awaiting acceptance"
            },
            new DashboardCardData
            {
                Title = "Accepted Orders",
                Value = AcceptedOrdersCount.ToString(),
                IconClass = "oi oi-check",
                BackgroundClass = "bg-info",
                TextColorClass = "text-white",
                Subtitle = AcceptedOrdersCount == 1 ? "order in progress" : "orders in progress"
            },
            new DashboardCardData
            {
                Title = "In Preparation",
                Value = InPreparationOrdersCount.ToString(),
                IconClass = "oi oi-wrench",
                BackgroundClass = "bg-warning",
                TextColorClass = "text-dark",
                Subtitle = InPreparationOrdersCount == 1 ? "order being prepared" : "orders being prepared"
            },
            new DashboardCardData
            {
                Title = "Ready for Delivery",
                Value = ReadyOrdersCount.ToString(),
                IconClass = "oi oi-box",
                BackgroundClass = "bg-success",
                TextColorClass = "text-white",
                Subtitle = ReadyOrdersCount == 1 ? "order ready" : "orders ready"
            }
        };
    }

    // Performance metrics
    public double GetCompletionRate()
    {
        if (TotalActiveOrders == 0) return 0;
        return (double)(ReadyOrdersCount) / TotalActiveOrders * 100;
    }

    public string GetCompletionRateText()
    {
        var rate = GetCompletionRate();
        return rate switch
        {
            >= 80 => "Excellent",
            >= 60 => "Good",
            >= 40 => "Fair",
            _ => "Needs Attention"
        };
    }
}

public class DashboardCardData
{
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = "0";
    public string IconClass { get; set; } = "oi oi-info";
    public string BackgroundClass { get; set; } = "bg-primary";
    public string TextColorClass { get; set; } = "text-white";
    public string? Subtitle { get; set; }
    public bool Clickable { get; set; }
    public string? ClickUrl { get; set; }
}