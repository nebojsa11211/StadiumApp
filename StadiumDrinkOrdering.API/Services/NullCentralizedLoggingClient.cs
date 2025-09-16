using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Null implementation of ICentralizedLoggingClient for use during migrations and testing
/// </summary>
public class NullCentralizedLoggingClient : ICentralizedLoggingClient
{
    public Task LogUserActionAsync(string action, string category, string? userId = null, string? userEmail = null,
        string? userRole = null, string? details = null, string? requestPath = null,
        string? httpMethod = null, string source = "Unknown")
    {
        return Task.CompletedTask;
    }

    public Task LogErrorAsync(Exception exception, string action, string category, string? userId = null,
        string? userEmail = null, string? userRole = null, string? details = null,
        string? requestPath = null, string? httpMethod = null, string source = "Unknown")
    {
        return Task.CompletedTask;
    }

    public Task LogWarningAsync(string message, string action, string category, string? userId = null,
        string? userEmail = null, string? userRole = null, string? details = null,
        string? requestPath = null, string? httpMethod = null, string source = "Unknown")
    {
        return Task.CompletedTask;
    }

    public Task LogInfoAsync(string message, string action, string category, string? userId = null,
        string? userEmail = null, string? userRole = null, string? details = null,
        string? requestPath = null, string? httpMethod = null, string source = "Unknown")
    {
        return Task.CompletedTask;
    }

    public Task LogBatchAsync(List<LogUserActionRequest> logEntries)
    {
        return Task.CompletedTask;
    }

    public Task LogBusinessEventAsync(BusinessEventDto businessEvent)
    {
        return Task.CompletedTask;
    }

    public void LogEventCreatedAsync(int eventId, string eventName, DateTime eventDate, string? creatorId = null, string? creatorEmail = null)
    {
        // No-op
    }

    public void LogTicketPurchaseAsync(int orderId, int ticketCount, decimal amount, string currency, string? customerId = null, string? customerEmail = null, Dictionary<string, object>? metadata = null)
    {
        // No-op
    }

    public void LogOrderStatusChangeAsync(int orderId, string orderNumber, string previousStatus, string newStatus, string? userId = null, string? userEmail = null)
    {
        // No-op
    }

    public void LogStaffAddedAsync(int staffId, string staffName, string role, string? addedById = null, string? addedByEmail = null)
    {
        // No-op
    }

    public void LogOrderDeliveredAsync(int orderId, string orderNumber, string? deliveredById = null, string? deliveredByEmail = null, string? locationInfo = null)
    {
        // No-op
    }
}