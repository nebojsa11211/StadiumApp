using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.JSInterop;

namespace StadiumDrinkOrdering.Admin.Components.Shared;

public partial class SignalRConnectionBase : ComponentBase, IAsyncDisposable
{
    [Inject] protected IJSRuntime JSRuntime { get; set; } = default!;

    private HubConnection? hubConnection;
    protected string _connectionStatus = "Disconnected";
    protected int _activeConnections = 0;
    
    [Parameter] public EventCallback<string> OnNewOrder { get; set; }
    [Parameter] public EventCallback<string> OnOrderStatusChanged { get; set; }
    [Parameter] public EventCallback<string> OnNotificationReceived { get; set; }
    [Parameter] public EventCallback<string> OnSystemAlert { get; set; }
    [Parameter] public string ApiBaseUrl { get; set; } = "http://localhost:9000";

    protected override async Task OnInitializedAsync()
    {
        await InitializeConnection();
        StateHasChanged();
    }

    protected async Task InitializeConnection()
    {
        try
        {
            _connectionStatus = "Connecting";
            StateHasChanged();

            hubConnection = new HubConnectionBuilder()
                .WithUrl($"{ApiBaseUrl}/bartenderHub")
                .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
                .Build();

            // Handle connection events
            hubConnection.Reconnecting += async (error) =>
            {
                _connectionStatus = "Reconnecting";
                await InvokeAsync(StateHasChanged);
                await ShowNotification("Connection lost, attempting to reconnect...", "warning");
            };

            hubConnection.Reconnected += async (connectionId) =>
            {
                _connectionStatus = "Connected";
                await InvokeAsync(StateHasChanged);
                await ShowNotification("Connection restored!", "success");
            };

            hubConnection.Closed += async (error) =>
            {
                _connectionStatus = "Disconnected";
                await InvokeAsync(StateHasChanged);
                if (error != null)
                {
                    await ShowNotification("Connection lost. Please refresh the page if issues persist.", "error");
                }
            };

            // Listen for new orders
            hubConnection.On<int, string, string>("NewOrder", async (orderId, customerName, seatNumber) =>
            {
                await OnNewOrder.InvokeAsync($"{orderId}:{customerName}:{seatNumber}");
                await ShowNotification($"📋 New order #{orderId} from {customerName} (Seat {seatNumber})", "info");
                await InvokeAsync(StateHasChanged);
            });

            // Listen for order status updates
            hubConnection.On<int, string>("OrderStatusUpdated", async (orderId, status) =>
            {
                await OnOrderStatusChanged.InvokeAsync($"{orderId}:{status}");
                await InvokeAsync(StateHasChanged);
            });

            // Listen for system alerts
            hubConnection.On<string, string>("SystemAlert", async (message, severity) =>
            {
                await OnSystemAlert.InvokeAsync($"{severity}:{message}");
                await ShowNotification($"🚨 SYSTEM: {message}", severity);
                await InvokeAsync(StateHasChanged);
            });

            // Listen for connection count updates
            hubConnection.On<int>("ConnectionCountUpdate", async (count) =>
            {
                _activeConnections = count;
                await InvokeAsync(StateHasChanged);
            });

            // Listen for general notifications
            hubConnection.On<string, string>("NotificationReceived", async (message, type) =>
            {
                await OnNotificationReceived.InvokeAsync($"{type}:{message}");
                await ShowNotification(message, type);
                await InvokeAsync(StateHasChanged);
            });

            // Listen for performance metrics
            hubConnection.On<string>("PerformanceUpdate", async (metricsJson) =>
            {
                await OnNotificationReceived.InvokeAsync($"metrics:{metricsJson}");
                await InvokeAsync(StateHasChanged);
            });

            // Start the connection
            await hubConnection.StartAsync();
            _connectionStatus = "Connected";
            await ShowNotification("Connected to system monitoring!", "success");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            _connectionStatus = "Failed";
            await ShowNotification($"Failed to connect: {ex.Message}", "error");
            StateHasChanged();
        }
    }

    protected async Task ShowNotification(string message, string type)
    {
        try
        {
            await JSRuntime.InvokeVoidAsync("showToast", message, type, 5000);
        }
        catch
        {
            // Ignore JS interop errors
        }
    }

    public async Task SendMessage(string method, params object[] args)
    {
        if (hubConnection?.State == HubConnectionState.Connected)
        {
            try
            {
                await hubConnection.SendAsync(method, args);
            }
            catch (Exception ex)
            {
                await ShowNotification($"Failed to send message: {ex.Message}", "error");
            }
        }
    }

    public async Task BroadcastSystemAlert(string message, string severity)
    {
        await SendMessage("BroadcastSystemAlert", message, severity);
    }

    public async Task RequestPerformanceUpdate()
    {
        await SendMessage("RequestPerformanceUpdate");
    }

    public async ValueTask DisposeAsync()
    {
        if (hubConnection is not null)
        {
            await hubConnection.DisposeAsync();
        }
    }
}