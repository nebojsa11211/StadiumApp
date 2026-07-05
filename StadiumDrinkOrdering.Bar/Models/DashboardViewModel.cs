using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using Microsoft.Extensions.Localization;

namespace StadiumDrinkOrdering.Bar.Models;

public class DashboardViewModel
{
    // Order Metrics
    public int PendingOrdersCount { get; set; }
    public int AcceptedOrdersCount { get; set; }
    public int InPreparationOrdersCount { get; set; }
    public int ReadyOrdersCount { get; set; }
    public int OutForDeliveryOrdersCount { get; set; }
    public int DeliveredOrdersCount { get; set; }
    public int TotalOrdersToday { get; set; }

    // Order Collections
    public List<OrderDto> RecentOrders { get; set; } = new();
    public List<OrderDto> PendingOrders { get; set; } = new();
    public List<OrderDto> AcceptedOrders { get; set; } = new();
    public List<OrderDto> InPreparationOrders { get; set; } = new();
    public List<OrderDto> ReadyOrders { get; set; } = new();
    public List<OrderDto> OutForDeliveryOrders { get; set; } = new();
    public List<OrderDto> DeliveredOrders { get; set; } = new();

    // Current event context (the event bartenders are currently serving)
    public EventDto? CurrentEvent { get; set; }

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

    // Dashboard Cards Data. Titles/subtitles are localized via the injected localizer, while each
    // card keeps a stable English Slug so element IDs stay constant across cultures.
    public List<DashboardCardData> GetDashboardCards(IStringLocalizer localizer)
    {
        return new List<DashboardCardData>
        {
            new DashboardCardData
            {
                Slug = "pending-orders",
                Title = localizer["Dashboard_CardPendingTitle"],
                Value = PendingOrdersCount.ToString(),
                IconClass = "oi oi-clock",
                BackgroundClass = "bg-primary",
                TextColorClass = "text-white",
                Subtitle = localizer["Dashboard_CardPendingSub"]
            },
            new DashboardCardData
            {
                Slug = "in-preparation",
                Title = localizer["Dashboard_CardInPreparationTitle"],
                Value = InPreparationOrdersCount.ToString(),
                IconClass = "oi oi-wrench",
                BackgroundClass = "bg-warning",
                TextColorClass = "text-dark",
                Subtitle = localizer["Dashboard_CardInPreparationSub"]
            },
            new DashboardCardData
            {
                Slug = "ready-for-delivery",
                Title = localizer["Dashboard_CardReadyTitle"],
                Value = ReadyOrdersCount.ToString(),
                IconClass = "oi oi-box",
                BackgroundClass = "bg-success",
                TextColorClass = "text-white",
                Subtitle = localizer["Dashboard_CardReadySub"]
            },
            new DashboardCardData
            {
                Slug = "picked-up",
                Title = localizer["Dashboard_CardPickedUpTitle"],
                Value = OutForDeliveryOrdersCount.ToString(),
                IconClass = "oi oi-transfer",
                BackgroundClass = "bg-info",
                TextColorClass = "text-white",
                Subtitle = localizer["Dashboard_CardPickedUpSub"]
            },
            new DashboardCardData
            {
                Slug = "delivered",
                Title = localizer["Dashboard_CardDeliveredTitle"],
                Value = DeliveredOrdersCount.ToString(),
                IconClass = "oi oi-circle-check",
                BackgroundClass = "bg-secondary",
                TextColorClass = "text-white",
                Subtitle = localizer["Dashboard_CardDeliveredSub"]
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
    // Stable, language-independent identifier used for element IDs (decoupled from the localized Title).
    public string? Slug { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Value { get; set; } = "0";
    public string IconClass { get; set; } = "oi oi-info";
    public string BackgroundClass { get; set; } = "bg-primary";
    public string TextColorClass { get; set; } = "text-white";
    public string? Subtitle { get; set; }
    public bool Clickable { get; set; }
    public string? ClickUrl { get; set; }
}