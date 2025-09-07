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
    Task<EventAnalyticsResponse?> GetEventAnalyticsAsync(int id);
    Task<IEnumerable<Event>> GetActiveEventsAsync();
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<IEnumerable<Event>> GetPastEventsAsync();
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
        var query = _context.Events.AsQueryable();
        
        if (activeOnly)
        {
            query = query.Where(e => e.IsActive);
        }

        return await query
            .Include(e => e.Analytics)
            .Include(e => e.Tickets)
            .OrderBy(e => e.EventDate)
            .ToListAsync();
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
        eventItem.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
        
        _logger.LogInformation("Deactivated event: {EventName} (ID: {EventId})", eventItem.EventName, id);
        return true;
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
        return await _context.Events
            .Where(e => e.IsActive)
            .Include(e => e.Analytics)
            .OrderBy(e => e.EventDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetUpcomingEventsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Events
            .Where(e => e.IsActive && e.EventDate > now)
            .Include(e => e.Analytics)
            .OrderBy(e => e.EventDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Event>> GetPastEventsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.Events
            .Where(e => e.EventDate < now)
            .Include(e => e.Analytics)
            .OrderByDescending(e => e.EventDate)
            .ToListAsync();
    }
}