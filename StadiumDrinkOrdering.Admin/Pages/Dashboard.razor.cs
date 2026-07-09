using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Admin.Common;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Dashboard : ComponentBase, IDisposable
{
    [Inject] private IAdminApiService AdminApiService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private bool isLoading = false;
    private DateTime? lastUpdated;
    private System.Timers.Timer? autoRefreshTimer;
    private string loadingMessage = "Initializing dashboard...";

    // Progressive loading states
    private bool isLoadingMetrics = false;
    private bool isLoadingOrders = false;
    private bool isLoadingSystemStatus = false;
    private int loadingProgress = 0; // 0-100 for progress indication

    // Dashboard data
    private int totalOrders = 0;
    private int activeOrders = 0;
    private decimal todaysRevenue = 0;
    private string systemHealth = "Healthy";
    private string systemLoad = "Normal load";
    private decimal ordersTrend = 0;
    private decimal revenueTrend = 0;
    private List<OrderDto>? recentOrders;

    // Sorting
    private readonly TableSortState sortState = new();
    private static readonly Dictionary<string, Func<OrderDto, object?>> SortSelectors = new()
    {
        ["order"] = o => o.Id,
        ["customer"] = o => o.CustomerName,
        ["amount"] = o => o.TotalAmount,
        ["status"] = o => o.Status,
        ["time"] = o => o.CreatedAt,
    };

    // Displayed orders: keep today's default order (newest-first) until a column is picked.
    private IEnumerable<OrderDto> DisplayedRecentOrders =>
        recentOrders == null
            ? Enumerable.Empty<OrderDto>()
            : sortState.Column is null
                ? recentOrders
                : sortState.Apply(recentOrders, SortSelectors);

    private void SortBy(string column)
    {
        sortState.Toggle(column);
        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadDashboardData();
        SetupAutoRefresh();
    }

    private async Task LoadDashboardData()
    {
        isLoading = true;
        loadingProgress = 0;
        loadingMessage = "Initializing dashboard...";
        StateHasChanged();

        try
        {
            // Stage 1: Load core metrics (0-40% progress)
            await LoadMetricsData();

            // Stage 2: Load order data (40-80% progress)
            await LoadOrdersData();

            // Stage 3: Load system status (80-100% progress)
            await LoadSystemStatus();

            lastUpdated = DateTime.Now;
            loadingMessage = "Dashboard loaded successfully";
            loadingProgress = 100;
        }
        catch (Exception ex)
        {
            loadingMessage = "Failed to load dashboard data";
            await JSRuntime.InvokeVoidAsync("console.error", "Failed to load dashboard data:", ex.Message);
        }
        finally
        {
            isLoading = false;
            isLoadingMetrics = false;
            isLoadingOrders = false;
            isLoadingSystemStatus = false;
            StateHasChanged();
        }
    }

    private async Task LoadMetricsData()
    {
        isLoadingMetrics = true;
        loadingMessage = "Loading key metrics...";
        loadingProgress = 10;
        StateHasChanged();

        // Simulate network delay for better UX feedback
        await Task.Delay(300);

        try
        {
            var orders = await AdminApiService.GetOrdersAsync();
            if (orders != null)
            {
                totalOrders = orders.Count();
                activeOrders = orders.Count(o => o.Status == OrderStatus.Pending || o.Status == OrderStatus.InPreparation);
                todaysRevenue = orders.Where(o => o.CreatedAt.Date == DateTime.Today).Sum(o => o.TotalAmount);

                // Mock trend data
                ordersTrend = 15.2m;
                revenueTrend = 8.7m;
            }

            loadingProgress = 40;
            loadingMessage = "Metrics loaded";
        }
        finally
        {
            isLoadingMetrics = false;
            StateHasChanged();
        }
    }

    private async Task LoadOrdersData()
    {
        isLoadingOrders = true;
        loadingMessage = "Loading recent orders...";
        loadingProgress = 50;
        StateHasChanged();

        // Simulate network delay for better UX feedback
        await Task.Delay(400);

        try
        {
            var orders = await AdminApiService.GetOrdersAsync();
            if (orders != null)
            {
                recentOrders = orders.OrderByDescending(o => o.CreatedAt).Take(10).ToList();
            }

            loadingProgress = 80;
            loadingMessage = "Orders loaded";
        }
        finally
        {
            isLoadingOrders = false;
            StateHasChanged();
        }
    }

    private async Task LoadSystemStatus()
    {
        isLoadingSystemStatus = true;
        loadingMessage = "Checking system status...";
        loadingProgress = 85;
        StateHasChanged();

        // Simulate network delay for better UX feedback
        await Task.Delay(200);

        try
        {
            // Mock system health check
            systemHealth = "Healthy";
            systemLoad = "Normal load";

            loadingProgress = 95;
            loadingMessage = "System status updated";
        }
        finally
        {
            isLoadingSystemStatus = false;
            StateHasChanged();
        }
    }

    private async Task RefreshData()
    {
        await LoadDashboardData();
    }

    private async Task ToggleTheme()
    {
        await JSRuntime.InvokeVoidAsync("toggleTheme");
    }

    private void SetupAutoRefresh()
    {
        autoRefreshTimer = new System.Timers.Timer(30000); // 30 seconds
        autoRefreshTimer.Elapsed += async (sender, e) =>
        {
            await InvokeAsync(async () =>
            {
                await LoadDashboardData();
            });
        };
        autoRefreshTimer.Start();
    }

    private string GetHealthCardClass()
    {
        return systemHealth.ToLower() switch
        {
            "healthy" => "bg-success",
            "warning" => "bg-warning",
            "critical" => "bg-danger",
            _ => "bg-secondary"
        };
    }

    private string GetHealthIcon()
    {
        return systemHealth.ToLower() switch
        {
            "healthy" => "oi oi-circle-check",
            "warning" => "oi oi-warning",
            "critical" => "oi oi-circle-x",
            _ => "oi oi-question-mark"
        };
    }

    private string GetHealthIconModifier()
    {
        return systemHealth.ToLower() switch
        {
            "healthy" => "is-ok",
            "warning" => "is-warn",
            "critical" => "is-crit",
            _ => "is-neutral"
        };
    }

    private string GetHealthDotModifier()
    {
        return systemHealth.ToLower() switch
        {
            "healthy" => "is-ok",
            "warning" => "is-warn",
            "critical" => "is-crit",
            _ => "is-neutral"
        };
    }

    private string GetStatusBadgeClass(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "bg-warning",
            OrderStatus.Accepted => "bg-primary",
            OrderStatus.InPreparation => "bg-info",
            OrderStatus.Ready => "bg-success",
            OrderStatus.Delivered => "bg-success",
            OrderStatus.Cancelled => "bg-danger",
            _ => "bg-secondary"
        };
    }

    // Console status pill modifier (dot + label) for the redesigned orders table
    private string GetStatusModifier(OrderStatus status)
    {
        return status switch
        {
            OrderStatus.Pending => "is-pending",
            OrderStatus.Accepted => "is-accepted",
            OrderStatus.InPreparation => "is-preparing",
            OrderStatus.Ready => "is-ready",
            OrderStatus.Delivered => "is-delivered",
            OrderStatus.Cancelled => "is-cancelled",
            _ => "is-neutral"
        };
    }

    // Navigation methods
    private void NavigateToOrders() => Navigation.NavigateTo("/orders");
    private void NavigateToUsers() => Navigation.NavigateTo("/users");
    private void NavigateToReports() => Navigation.NavigateTo("/reports");
    private void NavigateToLogs() => Navigation.NavigateTo("/logs");
    private void NavigateToOrder(int orderId) => Navigation.NavigateTo($"/orders/{orderId}");

    public void Dispose()
    {
        try
        {
            autoRefreshTimer?.Stop();
            autoRefreshTimer?.Dispose();
        }
        catch (ObjectDisposedException)
        {
            // Timer may already be disposed, safe to ignore
            // This happens during normal application shutdown
        }
        catch (Exception)
        {
            // Any other disposal exceptions should be ignored
            // This prevents disposal issues from propagating
        }
    }
}
