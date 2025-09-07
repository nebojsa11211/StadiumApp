using Microsoft.AspNetCore.SignalR;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.API.Data;
using Microsoft.EntityFrameworkCore;

namespace StadiumDrinkOrdering.API.Hubs;

public class CustomerHub : Hub
{
    private readonly ApplicationDbContext _context;

    public CustomerHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task JoinOrderGroup(int orderId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
    }

    public async Task LeaveOrderGroup(int orderId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"order-{orderId}");
    }

    public async Task JoinEventGroup(int eventId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"customer-event-{eventId}");
    }

    public async Task LeaveEventGroup(int eventId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"customer-event-{eventId}");
    }

    public async Task SendOrderStatusUpdate(int orderId, string status)
    {
        // Send to specific order group (customers tracking this order)
        await Clients.Group($"order-{orderId}").SendAsync("OrderStatusUpdated", orderId, status);
    }

    public async Task SendOrderReady(int orderId, string customerName, string seatNumber)
    {
        // Send to specific order group
        await Clients.Group($"order-{orderId}").SendAsync("OrderReady", orderId, customerName, seatNumber);
    }

    public async Task SendCustomerNotification(string message, string type)
    {
        // Send notification to specific connection (caller)
        await Clients.Caller.SendAsync("NotificationReceived", message, type);
    }

    public async Task SendBroadcastNotification(string message, string type)
    {
        // Send to all connected customers
        await Clients.All.SendAsync("NotificationReceived", message, type);
    }

    public override async Task OnConnectedAsync()
    {
        // Add to general customers group
        await Groups.AddToGroupAsync(Context.ConnectionId, "customers");
        
        await Clients.Caller.SendAsync("Connected", new { 
            ConnectionId = Context.ConnectionId,
            ConnectedAt = DateTime.UtcNow
        });
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        await base.OnDisconnectedAsync(exception);
    }
}