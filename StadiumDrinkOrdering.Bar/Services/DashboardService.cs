using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Bar.Models;
using StadiumDrinkOrdering.Bar.Services;
using Microsoft.Extensions.Caching.Memory;

namespace StadiumDrinkOrdering.Bar.Services;

public interface IDashboardService
{
    Task<DashboardViewModel> GetDashboardDataAsync();
    Task<List<OrderDto>> GetActiveOrdersAsync();
    Task RefreshDataAsync();
    void InvalidateCache();
}

public class DashboardService : IDashboardService
{
    private readonly IStaffApiService _apiService;
    private readonly IMemoryCache _cache;
    private readonly ILogger<DashboardService> _logger;

    private const string DASHBOARD_CACHE_KEY = "dashboard_data";
    private const string ORDERS_CACHE_KEY = "active_orders";
    private const string CURRENT_EVENT_CACHE_KEY = "current_event";
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromSeconds(30);

    public DashboardService(
        IStaffApiService apiService,
        IMemoryCache cache,
        ILogger<DashboardService> logger)
    {
        _apiService = apiService;
        _cache = cache;
        _logger = logger;
    }

    public async Task<DashboardViewModel> GetDashboardDataAsync()
    {
        var cacheKey = $"{DASHBOARD_CACHE_KEY}_{DateTime.UtcNow:yyyyMMddHHmm}";

        if (_cache.TryGetValue(cacheKey, out DashboardViewModel? cachedData) && cachedData != null)
        {
            _logger.LogDebug("Returning cached dashboard data");
            return cachedData;
        }

        try
        {
            _logger.LogDebug("Fetching fresh dashboard data from API");

            var orders = await GetActiveOrdersAsync();
            var currentEvent = await GetCurrentEventAsync();
            var dashboardData = BuildDashboardViewModel(orders, currentEvent);

            _cache.Set(cacheKey, dashboardData, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = CacheExpiration,
                SlidingExpiration = TimeSpan.FromSeconds(15),
                Priority = CacheItemPriority.High
            });

            return dashboardData;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching dashboard data");

            // Return cached data if available, even if expired
            if (_cache.TryGetValue(DASHBOARD_CACHE_KEY, out DashboardViewModel? fallbackData) && fallbackData != null)
            {
                _logger.LogWarning("Returning stale cached data due to error");
                return fallbackData;
            }

            // Return empty dashboard if no cache available
            return new DashboardViewModel();
        }
    }

    public async Task<List<OrderDto>> GetActiveOrdersAsync()
    {
        if (_cache.TryGetValue(ORDERS_CACHE_KEY, out List<OrderDto>? cachedOrders) && cachedOrders != null)
        {
            return cachedOrders;
        }

        try
        {
            var orders = await _apiService.GetActiveOrdersAsync() ?? new List<OrderDto>();

            _cache.Set(ORDERS_CACHE_KEY, orders, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(15),
                Priority = CacheItemPriority.High
            });

            return orders;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching active orders");
            return new List<OrderDto>();
        }
    }

    public async Task RefreshDataAsync()
    {
        _logger.LogDebug("Refreshing dashboard data");
        InvalidateCache();
        await GetDashboardDataAsync();
    }

    private async Task<EventDto?> GetCurrentEventAsync()
    {
        if (_cache.TryGetValue(CURRENT_EVENT_CACHE_KEY, out EventDto? cachedEvent))
        {
            return cachedEvent;
        }

        try
        {
            var currentEvent = await _apiService.GetCurrentEventAsync();

            _cache.Set(CURRENT_EVENT_CACHE_KEY, currentEvent, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(30),
                Priority = CacheItemPriority.High
            });

            return currentEvent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching current event");
            return null;
        }
    }

    public void InvalidateCache()
    {
        _cache.Remove(DASHBOARD_CACHE_KEY);
        _cache.Remove(ORDERS_CACHE_KEY);
        _cache.Remove(CURRENT_EVENT_CACHE_KEY);
        _logger.LogDebug("Dashboard cache invalidated");
    }

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
                title: "View Order Queue",
                url: "/order-queue",
                iconClass: "oi oi-list",
                buttonClass: "btn-primary",
                badgeCount: pendingOrders.Count
            ),
            QuickAction.CreateLink(
                title: "My Assigned Orders",
                url: "/my-orders",
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