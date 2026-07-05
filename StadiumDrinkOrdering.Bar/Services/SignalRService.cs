using Microsoft.AspNetCore.SignalR.Client;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Bar.Services;

public interface ISignalRService
{
    Task StartAsync();
    Task StopAsync();
    Task SendOrderStatusChanged(int orderId, OrderStatus newStatus, string seatNumber);
    Task SendOrderAssignedToStaff(int orderId, int staffId);
    event Action<OrderDto>? OrderUpdated;
    event Action<OrderDto>? NewOrder;
    event Action<OrderStatusChangedNotification>? OrderStatusChanged;
    event Action<int, int>? OrderAssignedToStaff;
    event Action? ConnectionStateChanged;
    bool IsConnected { get; }
}

public class SignalRService : ISignalRService
{
    private HubConnection? _hubConnection;
    // Source the token from the SCOPED, circuit-shared token store — NOT from IStaffApiService, which
    // is registered as a transient typed-HttpClient client (AddHttpClient<IStaffApiService,...>). A
    // transient means SignalRService gets a different StaffApiService instance than the one login set
    // the token on, so its .Token is always null → "No authentication token available".
    private readonly ITokenStorageService _tokenStorage;
    private readonly IConfiguration _configuration;

    public event Action<OrderDto>? OrderUpdated;
    public event Action<OrderDto>? NewOrder;
    public event Action<OrderStatusChangedNotification>? OrderStatusChanged;
    public event Action<int, int>? OrderAssignedToStaff;
    public event Action? ConnectionStateChanged;

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public SignalRService(ITokenStorageService tokenStorage, IConfiguration configuration)
    {
        _tokenStorage = tokenStorage;
        _configuration = configuration;
    }

    public async Task StartAsync()
    {
        if (_hubConnection != null)
            return;

        var token = await _tokenStorage.GetTokenAsync();
        if (string.IsNullOrEmpty(token))
        {
            throw new InvalidOperationException("Cannot start SignalR connection: No authentication token available");
        }

        var apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://api:8443/";
        var hubUrl = $"{apiBaseUrl.TrimEnd('/')}/bartenderHub";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                // Re-fetch on each (re)connect so token refreshes are picked up automatically.
                options.AccessTokenProvider = async () => await _tokenStorage.GetTokenAsync();
                options.Headers.Add("Authorization", $"Bearer {token}");
            })
            .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30) })
            .Build();

        _hubConnection.On<OrderDto>("OrderUpdated", order =>
        {
            OrderUpdated?.Invoke(order);
        });

        _hubConnection.On<OrderDto>("NewOrder", order =>
        {
            NewOrder?.Invoke(order);
        });

        _hubConnection.On<OrderStatusChangedNotification>("OrderStatusChanged", notification =>
        {
            OrderStatusChanged?.Invoke(notification);
        });

        _hubConnection.On<int, int>("OrderAssignedToStaff", (orderId, staffId) =>
        {
            OrderAssignedToStaff?.Invoke(orderId, staffId);
        });

        _hubConnection.Closed += async (error) =>
        {
            Console.WriteLine($"SignalR connection closed: {error?.Message}");
            ConnectionStateChanged?.Invoke();
            await Task.Delay(new Random().Next(0, 5) * 1000);
        };

        _hubConnection.Reconnecting += async (error) =>
        {
            Console.WriteLine($"SignalR reconnecting: {error?.Message}");
            ConnectionStateChanged?.Invoke();
            await Task.CompletedTask;
        };

        _hubConnection.Reconnected += async (connectionId) =>
        {
            Console.WriteLine($"SignalR reconnected: {connectionId}");
            ConnectionStateChanged?.Invoke();
            await Task.CompletedTask;
        };

        try
        {
            await _hubConnection.StartAsync();
            Console.WriteLine("SignalR connection started successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to start SignalR connection: {ex.Message}");
            // Automatic reconnect only kicks in AFTER a successful initial connect, so a failed
            // start would otherwise leave a dead, non-null connection that StartAsync's early-return
            // skips forever. Dispose it and reset to null so a later retry can rebuild from scratch.
            try { await _hubConnection.DisposeAsync(); } catch { /* best-effort */ }
            _hubConnection = null;
            throw;
        }
        finally
        {
            ConnectionStateChanged?.Invoke();
        }
    }

    public async Task StopAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
            ConnectionStateChanged?.Invoke();
        }
    }

    public async Task SendOrderStatusChanged(int orderId, OrderStatus newStatus, string seatNumber)
    {
        if (_hubConnection != null && IsConnected)
        {
            await _hubConnection.InvokeAsync("SendOrderStatusChanged", orderId, newStatus, seatNumber);
        }
    }

    public async Task SendOrderAssignedToStaff(int orderId, int staffId)
    {
        if (_hubConnection != null && IsConnected)
        {
            await _hubConnection.InvokeAsync("SendOrderAssignedToStaff", orderId, staffId);
        }
    }
}