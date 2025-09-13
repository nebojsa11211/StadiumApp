using Microsoft.AspNetCore.SignalR.Client;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Services;

public interface ISignalRService
{
    Task StartAsync();
    Task StopAsync();
    Task JoinSection(string section);
    Task LeaveSection(string section);
    Task SendOrderStatusChanged(int orderId, OrderStatus newStatus, string seatNumber);
    event Action<OrderDto>? OrderUpdated;
    event Action<OrderDto>? NewOrder;
    event Action<int, OrderStatus, string>? OrderStatusChanged;
    event Action<string, bool>? SeatHighlight;
    bool IsConnected { get; }
}

public class SignalRService : ISignalRService
{
    private HubConnection? _hubConnection;
    private readonly IAdminApiService _apiService;
    private readonly IConfiguration _configuration;

    public event Action<OrderDto>? OrderUpdated;
    public event Action<OrderDto>? NewOrder;
    public event Action<int, OrderStatus, string>? OrderStatusChanged;
    public event Action<string, bool>? SeatHighlight;

    public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

    public SignalRService(IAdminApiService apiService, IConfiguration configuration)
    {
        _apiService = apiService;
        _configuration = configuration;
    }

    public async Task StartAsync()
    {
        if (_hubConnection != null)
            return;

        if (string.IsNullOrEmpty(_apiService.Token))
        {
            throw new InvalidOperationException("Cannot start SignalR connection: No authentication token available");
        }

        var apiBaseUrl = _configuration.GetValue<string>("ApiSettings:BaseUrl") ?? "https://api:8443/";
        var hubUrl = $"{apiBaseUrl.TrimEnd('/')}/bartenderHub";

        _hubConnection = new HubConnectionBuilder()
            .WithUrl(hubUrl, options =>
            {
                options.AccessTokenProvider = () => Task.FromResult(_apiService.Token);
                options.Headers.Add("Authorization", $"Bearer {_apiService.Token}");
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

        _hubConnection.On<int, OrderStatus, string>("OrderStatusChanged", (orderId, newStatus, seatNumber) =>
        {
            OrderStatusChanged?.Invoke(orderId, newStatus, seatNumber);
        });

        _hubConnection.On<string, bool>("SeatHighlight", (seatNumber, highlight) =>
        {
            SeatHighlight?.Invoke(seatNumber, highlight);
        });

        _hubConnection.Closed += async (error) =>
        {
            Console.WriteLine($"SignalR connection closed: {error?.Message}");
            await Task.Delay(new Random().Next(0, 5) * 1000);
        };

        _hubConnection.Reconnecting += async (error) =>
        {
            Console.WriteLine($"SignalR reconnecting: {error?.Message}");
            await Task.CompletedTask;
        };

        _hubConnection.Reconnected += async (connectionId) =>
        {
            Console.WriteLine($"SignalR reconnected: {connectionId}");
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
            throw;
        }
    }

    public async Task StopAsync()
    {
        if (_hubConnection != null)
        {
            await _hubConnection.StopAsync();
            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }

    public async Task JoinSection(string section)
    {
        if (_hubConnection != null && IsConnected)
        {
            await _hubConnection.InvokeAsync("JoinSection", section);
        }
    }

    public async Task LeaveSection(string section)
    {
        if (_hubConnection != null && IsConnected)
        {
            await _hubConnection.InvokeAsync("LeaveSection", section);
        }
    }

    public async Task SendOrderStatusChanged(int orderId, OrderStatus newStatus, string seatNumber)
    {
        if (_hubConnection != null && IsConnected)
        {
            await _hubConnection.InvokeAsync("SendOrderStatusChanged", orderId, newStatus, seatNumber);
        }
    }
}
