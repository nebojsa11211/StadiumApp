using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Bar.Models;
using StadiumDrinkOrdering.Bar.Services;
using Microsoft.Extensions.Localization;

namespace StadiumDrinkOrdering.Bar.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardDataAsync();
    Task<List<OrderDto>> GetActiveOrdersAsync();
}

public class DashboardService : IDashboardService
{
    // No IMemoryCache here on purpose: AddMemoryCache() is a process-wide singleton, so a cached
    // board was shared by EVERY Bar circuit. One tab whose fetch failed (expired token, no live
    // event yet) cached an empty list and every other bar dashboard showed 0 orders while the
    // Order Queue (which never cached) showed them. The service is scoped per circuit and the page
    // only reloads on SignalR events / every 30 s, so fetching directly is cheap.
    private readonly IStaffApiService _apiService;
    private readonly ILogger<DashboardService> _logger;
    private readonly IStringLocalizer<SharedResources> _localizer;

    public DashboardService(
        IStaffApiService apiService,
        ILogger<DashboardService> logger,
        IStringLocalizer<SharedResources> localizer)
    {
        _apiService = apiService;
        _logger = logger;
        _localizer = localizer;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        // null = the API call failed (vs. an empty list = genuinely no orders). Surface it rather
        // than rendering a misleading all-zero board.
        var orders = await _apiService.GetActiveOrdersAsync();
        if (orders == null)
            throw new InvalidOperationException("Active orders could not be loaded from the API.");

        var currentEvent = await _apiService.GetCurrentEventAsync();
        return BuildDashboardViewModel(orders, currentEvent);
    }

    public async Task<List<OrderDto>> GetActiveOrdersAsync()
        => await _apiService.GetActiveOrdersAsync() ?? new List<OrderDto>();

    private DashboardViewModel BuildDashboardViewModel(List<OrderDto> orders, EventDto? currentEvent)
    {
        var recentOrders = orders.OrderByDescending(o => o.OrderDate).ToList();

        var pendingOrders = orders.Where(o => o.Status == OrderStatus.Pending).ToList();
        var acceptedOrders = orders.Where(o => o.Status == OrderStatus.Accepted).ToList();
        var inPreparationOrders = orders.Where(o => o.Status == OrderStatus.InPreparation).ToList();
        var readyOrders = orders.Where(o => o.Status == OrderStatus.Ready).ToList();
        var outForDeliveryOrders = orders.Where(o => o.Status == OrderStatus.OutForDelivery).ToList();
        var deliveredOrders = orders.Where(o => o.Status == OrderStatus.Delivered).ToList();

        var quickActions = new List<QuickAction>
        {
            QuickAction.CreateLink(
                title: _localizer["QuickAction_ViewOrderQueue"],
                url: "order-queue",
                iconClass: "oi oi-list",
                buttonClass: "btn-primary",
                badgeCount: pendingOrders.Count
            ),
            QuickAction.CreateLink(
                title: _localizer["QuickAction_MyAssignedOrders"],
                url: "my-orders",
                iconClass: "oi oi-clipboard",
                buttonClass: "btn-info",
                badgeCount: acceptedOrders.Count
            )
        };

        return new DashboardViewModel
        {
            // Order Metrics
            PendingOrdersCount = pendingOrders.Count,
            AcceptedOrdersCount = acceptedOrders.Count,
            InPreparationOrdersCount = inPreparationOrders.Count,
            ReadyOrdersCount = readyOrders.Count,
            OutForDeliveryOrdersCount = outForDeliveryOrders.Count,
            DeliveredOrdersCount = deliveredOrders.Count,

            // Order Lists
            RecentOrders = recentOrders,
            PendingOrders = pendingOrders,
            AcceptedOrders = acceptedOrders,
            InPreparationOrders = inPreparationOrders,
            ReadyOrders = readyOrders,
            OutForDeliveryOrders = outForDeliveryOrders,
            DeliveredOrders = deliveredOrders,

            // Current event context
            CurrentEvent = currentEvent,

            // UI Elements
            QuickActions = quickActions,

            // Metadata
            LastUpdated = DateTime.Now,
            TotalOrdersToday = orders.Count(o => o.OrderDate.Date == DateTime.Today)
        };
    }
}