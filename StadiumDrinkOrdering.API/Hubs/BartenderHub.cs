using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.API.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Hubs;

public class BartenderHub : Hub
{
    private readonly ApplicationDbContext _context;

    public BartenderHub(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task JoinEvent(int eventId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"event-{eventId}");
    }

    public async Task LeaveEvent(int eventId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"event-{eventId}");
    }

    public async Task JoinSection(string section)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"section-{section}");
    }

    public async Task LeaveSection(string section)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"section-{section}");
    }

    public async Task JoinStaffRole(string role)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"staff-{role.ToLower()}");
    }

    public async Task LeaveStaffRole(string role)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"staff-{role.ToLower()}");
    }

    public async Task SendOrderUpdate(OrderDto order)
    {
        var section = ExtractSectionFromSeat(order.SeatNumber);
        
        // Send to specific event if available
        if (order.EventId.HasValue)
        {
            await Clients.Group($"event-{order.EventId}").SendAsync("OrderUpdated", order);
        }
        
        // Send to section staff
        await Clients.Group($"section-{section}").SendAsync("OrderUpdated", order);
        
        // Send to all staff
        await Clients.Group("staff-all").SendAsync("OrderUpdated", order);
    }

    public async Task SendNewOrder(OrderDto order)
    {
        var section = ExtractSectionFromSeat(order.SeatNumber);
        
        // Send to specific event if available
        if (order.EventId.HasValue)
        {
            await Clients.Group($"event-{order.EventId}").SendAsync("NewOrder", order);
        }
        
        // Send to section staff
        await Clients.Group($"section-{section}").SendAsync("NewOrder", order);
        
        // Send to waiters for delivery
        await Clients.Group("staff-waiter").SendAsync("NewOrder", order);
        
        // Send to admins
        await Clients.Group("staff-admin").SendAsync("NewOrder", order);
    }

    public async Task SendOrderStatusChanged(int orderId, OrderStatus newStatus, string seatNumber, int? eventId = null)
    {
        var section = ExtractSectionFromSeat(seatNumber);
        
        var statusUpdate = new
        {
            OrderId = orderId,
            Status = newStatus,
            SeatNumber = seatNumber,
            Timestamp = DateTime.UtcNow,
            EventId = eventId
        };
        
        // Send to specific event if available
        if (eventId.HasValue)
        {
            await Clients.Group($"event-{eventId}").SendAsync("OrderStatusChanged", statusUpdate);
        }
        
        // Send to section staff
        await Clients.Group($"section-{section}").SendAsync("OrderStatusChanged", statusUpdate);
        
        // Send to all staff
        await Clients.Group("staff-all").SendAsync("OrderStatusChanged", statusUpdate);
    }

    public async Task SendStaffAssignmentUpdate(int orderId, int staffId, string staffName, string role)
    {
        var assignment = new
        {
            OrderId = orderId,
            StaffId = staffId,
            StaffName = staffName,
            Role = role,
            AssignedAt = DateTime.UtcNow
        };
        
        await Clients.Group("staff-all").SendAsync("StaffAssigned", assignment);
        await Clients.User(staffId.ToString()).SendAsync("OrderAssigned", assignment);
    }

    public async Task SendDeliveryEstimateUpdate(int orderId, DateTime estimatedDeliveryTime)
    {
        var estimate = new
        {
            OrderId = orderId,
            EstimatedDeliveryTime = estimatedDeliveryTime,
            UpdatedAt = DateTime.UtcNow
        };
        
        await Clients.Group("staff-all").SendAsync("DeliveryEstimateUpdated", estimate);
    }

    public async Task SendNotification(int? userId, string type, string title, string message, object? data = null)
    {
        var notification = new
        {
            Type = type,
            Title = title,
            Message = message,
            Data = data,
            Timestamp = DateTime.UtcNow
        };
        
        if (userId.HasValue)
        {
            // Send to specific user
            await Clients.User(userId.ToString()!).SendAsync("NotificationReceived", notification);
        }
        else
        {
            // Broadcast to all staff
            await Clients.Group("staff-all").SendAsync("NotificationReceived", notification);
        }
    }

    public async Task SendSeatHighlight(string seatNumber, bool highlight)
    {
        var section = ExtractSectionFromSeat(seatNumber);
        await Clients.Group($"section-{section}").SendAsync("SeatHighlight", seatNumber, highlight);
        await Clients.All.SendAsync("SeatHighlight", seatNumber, highlight);
    }

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value?.ToLower();
        
        if (Context.User?.Identity?.IsAuthenticated == true)
        {
            // Authenticated staff member
            // Add to general staff group
            await Groups.AddToGroupAsync(Context.ConnectionId, "staff-all");
            
            // Add to role-specific groups
            if (!string.IsNullOrEmpty(userRole))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"staff-{userRole}");
            }
            
            // Get user's event assignments if any
            if (int.TryParse(userId, out var userIdInt))
            {
                var eventAssignments = await _context.EventStaffAssignments
                    .Where(esa => esa.StaffId == userIdInt && esa.IsActive)
                    .Select(esa => esa.EventId)
                    .ToListAsync();
                    
                foreach (var eventId in eventAssignments)
                {
                    await Groups.AddToGroupAsync(Context.ConnectionId, $"event-{eventId}");
                }
            }
            
            // Notify other staff members
            await Clients.GroupExcept("staff-all", Context.ConnectionId).SendAsync("StaffConnected", new {
                ConnectionId = Context.ConnectionId,
                UserId = userId,
                Role = userRole
            });
        }
        else
        {
            // Unauthenticated customer connection
            await Groups.AddToGroupAsync(Context.ConnectionId, "customers");
        }
        
        await Clients.Caller.SendAsync("Connected", new { 
            ConnectionId = Context.ConnectionId,
            UserId = userId,
            Role = userRole ?? "customer",
            ConnectedAt = DateTime.UtcNow
        });
        
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var userRole = Context.User?.FindFirst(ClaimTypes.Role)?.Value?.ToLower();
        
        // Notify other staff members
        await Clients.Group("staff-all").SendAsync("StaffDisconnected", new {
            ConnectionId = Context.ConnectionId,
            UserId = userId,
            Role = userRole,
            DisconnectedAt = DateTime.UtcNow,
            Exception = exception?.Message
        });
        
        await base.OnDisconnectedAsync(exception);
    }

    private static string ExtractSectionFromSeat(string seatNumber)
    {
        if (string.IsNullOrEmpty(seatNumber))
            return "Unknown";

        var parts = seatNumber.Split('-');
        if (parts.Length > 0)
        {
            var sectionPart = parts[0];
            return sectionPart.Length > 0 ? sectionPart[0].ToString() : "Unknown";
        }

        return "Unknown";
    }
}
