using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Hubs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface INotificationService
{
    Task SendOrderNotificationAsync(Order order, string notificationType);
    Task SendPaymentNotificationAsync(Payment payment, string notificationType);
    Task SendStaffNotificationAsync(int staffId, string type, string title, string message, object? data = null);
    Task SendEventNotificationAsync(int eventId, string type, string title, string message, object? data = null);
    Task SendBroadcastNotificationAsync(string type, string title, string message, object? data = null);
    Task CreatePersistentNotificationAsync(int? userId, string type, string title, string message, object? data = null, int? eventId = null);
    Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false);
    Task MarkNotificationAsReadAsync(int notificationId, int userId);
}

public class NotificationService : INotificationService
{
    private readonly IHubContext<BartenderHub> _hubContext;
    private readonly ApplicationDbContext _context;
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(IHubContext<BartenderHub> hubContext, ApplicationDbContext context, ILogger<NotificationService> logger)
    {
        _hubContext = hubContext;
        _context = context;
        _logger = logger;
    }

    public async Task SendOrderNotificationAsync(Order order, string notificationType)
    {
        try
        {
            var notification = new
            {
                Type = notificationType,
                OrderId = order.Id,
                SeatNumber = order.SeatNumber,
                EventId = order.EventId,
                CustomerNotes = order.CustomerNotes,
                TotalAmount = order.TotalAmount,
                Status = order.Status.ToString(),
                Timestamp = DateTime.UtcNow
            };

            switch (notificationType.ToLower())
            {
                case "neworder":
                    await _hubContext.Clients.Group("staff-waiter").SendAsync("NewOrderNotification", notification);
                    await _hubContext.Clients.Group("staff-admin").SendAsync("NewOrderNotification", notification);
                    
                    // Create persistent notification
                    await CreatePersistentNotificationAsync(
                        null, 
                        "NewOrder",
                        "New Order Received", 
                        $"New order from seat {order.SeatNumber} - €{order.TotalAmount:F2}",
                        notification,
                        order.EventId
                    );
                    break;

                case "orderready":
                    await _hubContext.Clients.Group("staff-waiter").SendAsync("OrderReadyNotification", notification);
                    
                    // Notify customer if they have a session
                    if (order.SessionId.HasValue)
                    {
                        await _hubContext.Clients.Group($"session-{order.SessionId}").SendAsync("OrderReady", notification);
                    }
                    break;

                case "orderdelivered":
                    await _hubContext.Clients.Group("staff-all").SendAsync("OrderDeliveredNotification", notification);
                    break;
            }

            _logger.LogInformation("Order notification sent: {Type} for Order {OrderId}", notificationType, order.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending order notification: {Type} for Order {OrderId}", notificationType, order.Id);
        }
    }

    public async Task SendPaymentNotificationAsync(Payment payment, string notificationType)
    {
        try
        {
            var notification = new
            {
                Type = notificationType,
                PaymentId = payment.Id,
                OrderId = payment.OrderId,
                Amount = payment.Amount,
                Status = payment.Status,
                PaymentMethod = payment.PaymentMethod,
                TransactionId = payment.TransactionId,
                Timestamp = DateTime.UtcNow
            };

            switch (notificationType.ToLower())
            {
                case "paymentreceived":
                    await _hubContext.Clients.Group("staff-admin").SendAsync("PaymentReceivedNotification", notification);
                    
                    await CreatePersistentNotificationAsync(
                        null,
                        "PaymentReceived",
                        "Payment Received",
                        $"Payment of €{payment.Amount:F2} received for Order #{payment.OrderId}",
                        notification
                    );
                    break;

                case "paymentfailed":
                    await _hubContext.Clients.Group("staff-admin").SendAsync("PaymentFailedNotification", notification);
                    
                    await CreatePersistentNotificationAsync(
                        null,
                        "PaymentFailed",
                        "Payment Failed",
                        $"Payment failed for Order #{payment.OrderId} - {payment.FailureReason}",
                        notification
                    );
                    break;

                case "refundprocessed":
                    await _hubContext.Clients.Group("staff-admin").SendAsync("RefundProcessedNotification", notification);
                    break;
            }

            _logger.LogInformation("Payment notification sent: {Type} for Payment {PaymentId}", notificationType, payment.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending payment notification: {Type} for Payment {PaymentId}", notificationType, payment.Id);
        }
    }

    public async Task SendStaffNotificationAsync(int staffId, string type, string title, string message, object? data = null)
    {
        try
        {
            var notification = new
            {
                Type = type,
                Title = title,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.User(staffId.ToString()).SendAsync("NotificationReceived", notification);
            
            // Also create persistent notification
            await CreatePersistentNotificationAsync(staffId, type, title, message, data);
            
            _logger.LogInformation("Staff notification sent: {Type} to User {StaffId}", type, staffId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending staff notification: {Type} to User {StaffId}", type, staffId);
        }
    }

    public async Task SendEventNotificationAsync(int eventId, string type, string title, string message, object? data = null)
    {
        try
        {
            var notification = new
            {
                Type = type,
                Title = title,
                Message = message,
                Data = data,
                EventId = eventId,
                Timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.Group($"event-{eventId}").SendAsync("EventNotification", notification);
            
            // Create persistent notification for event staff
            var eventStaff = await _context.EventStaffAssignments
                .Where(esa => esa.EventId == eventId && esa.IsActive)
                .Select(esa => esa.StaffId)
                .ToListAsync();
                
            foreach (var staffId in eventStaff)
            {
                await CreatePersistentNotificationAsync(staffId, type, title, message, data, eventId);
            }
            
            _logger.LogInformation("Event notification sent: {Type} for Event {EventId}", type, eventId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending event notification: {Type} for Event {EventId}", type, eventId);
        }
    }

    public async Task SendBroadcastNotificationAsync(string type, string title, string message, object? data = null)
    {
        try
        {
            var notification = new
            {
                Type = type,
                Title = title,
                Message = message,
                Data = data,
                Timestamp = DateTime.UtcNow
            };

            await _hubContext.Clients.Group("staff-all").SendAsync("BroadcastNotification", notification);
            
            // Create persistent notification for all staff
            await CreatePersistentNotificationAsync(null, type, title, message, data);
            
            _logger.LogInformation("Broadcast notification sent: {Type}", type);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending broadcast notification: {Type}", type);
        }
    }

    public async Task CreatePersistentNotificationAsync(int? userId, string type, string title, string message, object? data = null, int? eventId = null)
    {
        try
        {
            var notification = new Notification
            {
                UserId = userId,
                Type = type,
                Title = title,
                Message = message,
                Data = data != null ? System.Text.Json.JsonSerializer.Serialize(data) : null,
                EventId = eventId,
                CreatedAt = DateTime.UtcNow,
                IsRead = false,
                Priority = "Normal"
            };

            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("Persistent notification created: {Type} for User {UserId}", type, userId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating persistent notification: {Type} for User {UserId}", type, userId);
        }
    }

    public async Task<List<Notification>> GetUserNotificationsAsync(int userId, bool unreadOnly = false)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId || n.UserId == null) // Include broadcast notifications
            .OrderByDescending(n => n.CreatedAt);

        if (unreadOnly)
        {
            query = (IOrderedQueryable<Notification>)query.Where(n => !n.IsRead);
        }

        return await query.Take(50).ToListAsync(); // Limit to recent 50 notifications
    }

    public async Task MarkNotificationAsReadAsync(int notificationId, int userId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && (n.UserId == userId || n.UserId == null));

        if (notification != null)
        {
            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}