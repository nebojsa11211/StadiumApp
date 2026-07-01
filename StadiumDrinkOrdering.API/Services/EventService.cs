using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetEventsAsync(bool activeOnly = false);
    Task<Event?> GetEventByIdAsync(int id);
    Task<Event> CreateEventAsync(Event eventItem);
    Task<Event?> UpdateEventAsync(int id, Event eventItem);
    Task<bool> DeleteEventAsync(int id);
    Task<bool> ActivateEventAsync(int id);
    Task<bool> DeactivateEventAsync(int id);
    Task<EventStatusTransitionResult> TransitionEventStatusAsync(int id, EventStatus newStatus);
    Task<EventAnalyticsResponse?> GetEventAnalyticsAsync(int id);
    Task<IEnumerable<Event>> GetActiveEventsAsync();
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<IEnumerable<Event>> GetPastEventsAsync();
}

/// <summary>
/// Outcome of an attempted event status transition.
/// </summary>
public class EventStatusTransitionResult
{
    public bool Success { get; init; }
    public bool NotFound { get; init; }
    public string? ErrorMessage { get; init; }
    public EventStatus? PreviousStatus { get; init; }
    public EventStatus? NewStatus { get; init; }

    public static EventStatusTransitionResult Ok(EventStatus from, EventStatus to) =>
        new() { Success = true, PreviousStatus = from, NewStatus = to };

    public static EventStatusTransitionResult Missing() =>
        new() { Success = false, NotFound = true, ErrorMessage = "Event not found." };

    public static EventStatusTransitionResult Invalid(string message, EventStatus from) =>
        new() { Success = false, ErrorMessage = message, PreviousStatus = from };
}

public class EventService : IEventService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EventService> _logger;

    public EventService(ApplicationDbContext context, ILogger<EventService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(bool activeOnly = false)
    {
        try
        {
            // Use raw SQL for much better performance
            var sql = @"
                SELECT
                    e.""Id"", e.""EventName"", e.""EventType"", e.""EventDate"",
                    e.""VenueId"", e.""TotalSeats"", e.""IsActive"", e.""Status"", e.""CreatedAt"",
                    e.""UpdatedAt"", e.""Description"", e.""ImageUrl"", e.""BaseTicketPrice""
                FROM ""Events"" e
                {0}
                ORDER BY e.""EventDate""";

            var whereClause = activeOnly ? "WHERE e.\"IsActive\" = true" : "";
            var finalSql = string.Format(sql, whereClause);

            var events = await _context.Events
                .FromSqlRaw(finalSql)
                .AsNoTracking() // Better performance for read-only operations
                .ToListAsync();

            return events;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting events (activeOnly: {ActiveOnly})", activeOnly);
            throw;
        }
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Tickets)
            .Include(e => e.Orders)
            .Include(e => e.StaffAssignments)
                .ThenInclude(sa => sa.Staff)
            // Temporarily removed .Include(e => e.Analytics) due to database schema mismatch
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<Event> CreateEventAsync(Event eventItem)
    {
        eventItem.CreatedAt = DateTime.UtcNow;
        eventItem.UpdatedAt = DateTime.UtcNow;

        // Initial lifecycle state derives from whether the event is published on creation:
        // a published event is immediately on sale; an unpublished one stays in planning.
        if (eventItem.Status == EventStatus.Planned)
            eventItem.Status = eventItem.IsActive ? EventStatus.OnSale : EventStatus.Planned;

        _context.Events.Add(eventItem);
        await _context.SaveChangesAsync();

        // TODO: Re-enable analytics creation once database schema is fixed
        /*
        // Create initial analytics record
        var analytics = new EventAnalytics
        {
            EventId = eventItem.Id,
            CalculatedAt = DateTime.UtcNow
        };
        
        _context.EventAnalytics.Add(analytics);
        await _context.SaveChangesAsync();
        */

        _logger.LogInformation("Created new event: {EventName} (ID: {EventId})", eventItem.EventName, eventItem.Id);
        return eventItem;
    }

    public async Task<Event?> UpdateEventAsync(int id, Event eventItem)
    {
        var existingEvent = await _context.Events.FindAsync(id);
        if (existingEvent == null)
            return null;

        existingEvent.EventName = eventItem.EventName;
        existingEvent.EventType = eventItem.EventType;
        existingEvent.EventDate = eventItem.EventDate;
        existingEvent.VenueId = eventItem.VenueId;
        existingEvent.TotalSeats = eventItem.TotalSeats;
        existingEvent.Description = eventItem.Description;
        existingEvent.ImageUrl = eventItem.ImageUrl;
        existingEvent.BaseTicketPrice = eventItem.BaseTicketPrice;
        existingEvent.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated event: {EventName} (ID: {EventId})", existingEvent.EventName, id);
        return existingEvent;
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
            return false;

        // Check if event has tickets sold
        var hasTickets = await _context.Tickets.AnyAsync(t => t.EventId == id);
        if (hasTickets)
        {
            _logger.LogWarning("Cannot delete event {EventId} - tickets have been sold", id);
            return false;
        }

        _context.Events.Remove(eventItem);
        await _context.SaveChangesAsync();
        _logger.LogInformation("Deleted event: {EventName} (ID: {EventId})", eventItem.EventName, id);
        return true;
    }

    public async Task<bool> ActivateEventAsync(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
            return false;

        eventItem.IsActive = true;
        // Publishing an event in planning puts it on sale (Phase 1).
        if (eventItem.Status == EventStatus.Planned)
            eventItem.Status = EventStatus.OnSale;
        eventItem.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Activated event: {EventName} (ID: {EventId})", eventItem.EventName, id);
        return true;
    }

    public async Task<bool> DeactivateEventAsync(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
            return false;

        eventItem.IsActive = false;
        // Unpublishing a not-yet-started, on-sale event returns it to planning (pulls it from sale).
        if (eventItem.Status == EventStatus.OnSale)
            eventItem.Status = EventStatus.Planned;
        eventItem.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deactivated event: {EventName} (ID: {EventId})", eventItem.EventName, id);
        return true;
    }

    public async Task<EventStatusTransitionResult> TransitionEventStatusAsync(int id, EventStatus newStatus)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
            return EventStatusTransitionResult.Missing();

        var current = eventItem.Status;
        if (!EventLifecycle.CanTransition(current, newStatus))
        {
            var allowed = string.Join(", ", EventLifecycle.AllowedNextStatuses(current));
            var message = string.IsNullOrEmpty(allowed)
                ? $"Event is in terminal state '{current}' and its status can no longer change."
                : $"Cannot move event from '{current}' to '{newStatus}'. Allowed next states: {allowed}.";
            _logger.LogWarning("Rejected status transition {From} -> {To} for event {EventId}", current, newStatus, id);
            return EventStatusTransitionResult.Invalid(message, current);
        }

        eventItem.Status = newStatus;
        eventItem.UpdatedAt = DateTime.UtcNow;

        // Keep the published/visible flag consistent with the lifecycle: live & sellable states
        // are visible; terminal states drop out of "active"/"upcoming" listings.
        if (EventLifecycle.IsTerminal(newStatus))
            eventItem.IsActive = false;
        else if (newStatus is EventStatus.OnSale or EventStatus.Active or EventStatus.InProgress)
            eventItem.IsActive = true;

        // Phase 3 closure: invalidate all active ticket sessions so no new drink orders are possible.
        if (EventLifecycle.IsTerminal(newStatus))
        {
            var activeSessions = await _context.TicketSessions
                .Where(s => s.EventId == id && s.IsActive)
                .ToListAsync();
            foreach (var session in activeSessions)
                session.IsActive = false;

            if (activeSessions.Count > 0)
                _logger.LogInformation("Invalidated {Count} active ticket session(s) while closing event {EventId}",
                    activeSessions.Count, id);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Event {EventId} status transition: {From} -> {To}", id, current, newStatus);
        return EventStatusTransitionResult.Ok(current, newStatus);
    }

    public Task<EventAnalyticsResponse?> GetEventAnalyticsAsync(int id)
    {
        // Temporarily disabled due to database schema mismatch 
        // TODO: Fix EventAnalytics table schema and re-enable this method
        return Task.FromResult<EventAnalyticsResponse?>(null);
        
        /*
        var analytics = await _context.EventAnalytics
            .Include(ea => ea.Event)
            .FirstOrDefaultAsync(ea => ea.EventId == id);

        if (analytics == null)
            return null;

        // Recalculate analytics
        var ticketsSold = await _context.Tickets.CountAsync(t => t.EventId == id && t.Status == "Active");
        var totalRevenue = await _context.Tickets
            .Where(t => t.EventId == id && t.Status == "Active")
            .SumAsync(t => t.Price);
        
        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .Where(o => o.EventId == id)
            .ToListAsync();

        var totalOrders = orders.Count;
        var totalDrinksSold = orders.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity);
        var averageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0;

        // Find peak order time (hour with most orders)
        var ordersByHour = orders
            .GroupBy(o => o.CreatedAt.Hour)
            .OrderByDescending(g => g.Count())
            .FirstOrDefault();

        DateTime? peakOrderTime = null;
        if (ordersByHour != null)
        {
            var eventDate = analytics.Event.EventDate.Date;
            peakOrderTime = eventDate.AddHours(ordersByHour.Key);
        }

        // Find most popular drink
        var mostPopularDrink = orders
            .SelectMany(o => o.OrderItems)
            .GroupBy(oi => oi.Drink?.Name ?? "Unknown")
            .OrderByDescending(g => g.Sum(x => x.Quantity))
            .FirstOrDefault()?.Key;

        // Update analytics
        analytics.TotalTicketsSold = ticketsSold;
        analytics.TotalRevenue = totalRevenue;
        analytics.TotalOrders = totalOrders;
        analytics.TotalDrinksSold = totalDrinksSold;
        analytics.AverageOrderValue = averageOrderValue;
        analytics.PeakOrderTime = peakOrderTime;
        analytics.MostPopularDrink = mostPopularDrink;
        analytics.CalculatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return analytics;
        */
    }

    public async Task<IEnumerable<Event>> GetActiveEventsAsync()
    {
        // Use raw SQL for better performance
        var sql = @"
            SELECT
                e.""Id"", e.""EventName"", e.""EventType"", e.""EventDate"",
                e.""VenueId"", e.""TotalSeats"", e.""IsActive"", e.""Status"", e.""CreatedAt"",
                e.""UpdatedAt"", e.""Description"", e.""ImageUrl"", e.""BaseTicketPrice""
            FROM ""Events"" e
            WHERE e.""IsActive"" = true
            ORDER BY e.""EventDate""";

        return await _context.Events
            .FromSqlRaw(sql)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
    {
        var now = DateTime.UtcNow;
        // Use raw SQL for better performance
        var sql = @"
            SELECT
                e.""Id"", e.""EventName"", e.""EventType"", e.""EventDate"",
                e.""VenueId"", e.""TotalSeats"", e.""IsActive"", e.""Status"", e.""CreatedAt"",
                e.""UpdatedAt"", e.""Description"", e.""ImageUrl"", e.""BaseTicketPrice""
            FROM ""Events"" e
            WHERE e.""IsActive"" = true AND e.""EventDate"" > {0}
            ORDER BY e.""EventDate""";

        return await _context.Events
            .FromSqlRaw(sql, now)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetPastEventsAsync()
    {
        var now = DateTime.UtcNow;
        // Use raw SQL for better performance
        var sql = @"
            SELECT
                e.""Id"", e.""EventName"", e.""EventType"", e.""EventDate"",
                e.""VenueId"", e.""TotalSeats"", e.""IsActive"", e.""Status"", e.""CreatedAt"",
                e.""UpdatedAt"", e.""Description"", e.""ImageUrl"", e.""BaseTicketPrice""
            FROM ""Events"" e
            WHERE e.""EventDate"" < {0}
            ORDER BY e.""EventDate"" DESC";

        return await _context.Events
            .FromSqlRaw(sql, now)
            .AsNoTracking()
            .ToListAsync();
    }
}