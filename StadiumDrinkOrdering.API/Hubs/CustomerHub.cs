using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Hubs;

/// <summary>
/// SignalR hub for customer notifications and order updates.
/// Allows both authenticated customers and anonymous users for public notifications.
/// </summary>
public class CustomerHub : Hub
{
    private readonly ApplicationDbContext _context;

    public CustomerHub(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Joins order group to receive order-specific updates. Requires authentication.
    /// </summary>
    [Authorize(Policy = AuthorizationPolicies.RequireAuthenticatedUser)]
    public async Task JoinOrderGroup(int orderId)
    {
        // Verify the user owns this order or is staff/admin
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value;

        if (userRole == "Admin" || userRole == "Staff")
        {
            // Staff and admins can join any order group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
        }
        else if (int.TryParse(userId, out var userIdInt))
        {
            // Customers can only join groups for their own orders
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.CustomerId == userIdInt);
            if (order != null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"order-{orderId}");
            }
        }
    }

    /// <summary>
    /// Leaves order group. Requires authentication.
    /// </summary>
    [Authorize(Policy = AuthorizationPolicies.RequireAuthenticatedUser)]
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

    /// <summary>
    /// Sends order status updates to customers. Staff/Admin only.
    /// </summary>
    [Authorize(Policy = AuthorizationPolicies.RequireStaffRole)]
    public async Task SendOrderStatusUpdate(int orderId, string status)
    {
        // Send to specific order group (customers tracking this order)
        await Clients.Group($"order-{orderId}").SendAsync("OrderStatusUpdated", orderId, status);
    }

    /// <summary>
    /// Notifies customers that their order is ready. Staff/Admin only.
    /// </summary>
    [Authorize(Policy = AuthorizationPolicies.RequireStaffRole)]
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

    /// <summary>
    /// Broadcasts notifications to all customers. Admin only.
    /// </summary>
    [Authorize(Policy = AuthorizationPolicies.RequireAdminRole)]
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