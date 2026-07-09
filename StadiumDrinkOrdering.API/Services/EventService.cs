using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IEventService
{
    Task<IEnumerable<Event>> GetEventsAsync(bool activeOnly = false);
    Task<Dictionary<int, int>> GetSoldSeatCountsAsync(IEnumerable<int> eventIds);
    Task<Dictionary<int, int>> GetSeasonSoldSeatCountsAsync(IEnumerable<int> eventIds);
    Task<Event?> GetEventByIdAsync(int id);
    Task<Event> CreateEventAsync(Event eventItem);
    Task<Event?> UpdateEventAsync(int id, Event eventItem);
    /// <summary>
    /// True if another event already uses this name (case-insensitive, trimmed). Pass the id of the
    /// event being edited as <paramref name="excludeEventId"/> so it doesn't clash with itself.
    /// </summary>
    Task<bool> IsEventNameTakenAsync(string name, int? excludeEventId = null);
    Task<bool> DeleteEventAsync(int id);
    Task<bool> ActivateEventAsync(int id);
    Task<bool> DeactivateEventAsync(int id);
    /// <summary>
    /// Moves an event to <paramref name="newStatus"/>. Manual transitions (default) are validated
    /// against <see cref="EventLifecycle.AllowedTransitions"/>. Pass <paramref name="systemAutoClose"/>
    /// = true only from the time-driven auto-completer, which may close any non-terminal event to
    /// <see cref="EventStatus.Completed"/> once its window has elapsed (see <see cref="EventLifecycle.CanAutoComplete"/>).
    /// </summary>
    Task<EventStatusTransitionResult> TransitionEventStatusAsync(int id, EventStatus newStatus, bool systemAutoClose = false);
    Task<EventAnalyticsResponse?> GetEventAnalyticsAsync(int id);
    Task<IEnumerable<Event>> GetActiveEventsAsync();
    /// <summary>
    /// Transitions any non-terminal event whose time window has already elapsed to
    /// <see cref="EventStatus.Completed"/> — both stale-live events (Active/InProgress) and ones that
    /// never went live (Planned/OnSale/SoldOut) but whose date has passed. Returns the number of
    /// events closed. Called opportunistically on read so past-dated events don't linger.
    /// </summary>
    Task<int> AutoCompleteEndedEventsAsync();
    Task<IEnumerable<Event>> GetUpcomingEventsAsync();
    Task<IEnumerable<Event>> GetPastEventsAsync();

    /// <summary>
    /// Lists every overlay sector together with its default price and, when <paramref name="eventId"/>
    /// is supplied, the current per-event price override and disabled flag for that sector. Powers the
    /// "Sector prices" editor in the Admin event modal. For a new (unsaved) event pass null.
    /// </summary>
    Task<List<EventSectorPriceDto>> GetSectorPricingAsync(int? eventId);

    /// <summary>
    /// Replaces an event's per-sector configuration with <paramref name="prices"/>: a row is kept for
    /// each entry that carries a non-null price and/or a disable; everything else is cleared. A null
    /// <paramref name="prices"/> is a no-op (leaves existing configuration untouched).
    /// </summary>
    Task SaveSectorPricesAsync(int eventId, IEnumerable<EventSectorPriceInputDto>? prices);

    /// <summary>
    /// Returns a human-readable error when <paramref name="prices"/> would disable a sector that already
    /// has sold tickets for the event (which would orphan paid customers), or null when every requested
    /// disable is safe. A null <paramref name="prices"/> returns null. Call before persisting.
    /// </summary>
    Task<string?> ValidateSectorDisablesAsync(int eventId, IEnumerable<EventSectorPriceInputDto>? prices);
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
    private readonly IOverlaySeatService _overlaySeats;
    private readonly IOrderService _orderService;
    private readonly ILogger<EventService> _logger;

    public EventService(ApplicationDbContext context, IOverlaySeatService overlaySeats, IOrderService orderService, ILogger<EventService> logger)
    {
        _context = context;
        _overlaySeats = overlaySeats;
        _orderService = orderService;
        _logger = logger;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync(bool activeOnly = false)
    {
        try
        {
            // Close out any past-dated events before listing so the admin grid never shows a
            // finished (or never-started) event still sitting in a future status like Planned.
            await AutoCompleteEndedEventsAsync();

            // Raw SQL for performance (skips EF LINQ translation and does not load navigations).
            // Select all columns (e.*) so adding a mapped Event column never breaks materialization —
            // FromSqlRaw requires every mapped column to be present in the result set.
            var sql = @"
                SELECT e.*
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

    /// <summary>
    /// Returns sold-seat counts keyed by event id for the given events. Because
    /// <see cref="GetEventsAsync"/> uses raw SQL and does not load the Tickets navigation,
    /// list endpoints must fetch sold counts separately. "Sold" funnels through
    /// <see cref="TicketStatuses.CountsAsSold"/> (any non-cancelled ticket occupies a seat).
    /// Events with no sold tickets are simply absent from the dictionary.
    /// </summary>
    public async Task<Dictionary<int, int>> GetSoldSeatCountsAsync(IEnumerable<int> eventIds)
    {
        var ids = eventIds.Distinct().ToList();
        if (ids.Count == 0)
            return new Dictionary<int, int>();

        return await _context.Tickets
            .AsNoTracking()
            .Where(t => ids.Contains(t.EventId)
                        && t.Status != TicketStatuses.Cancelled)
            .GroupBy(t => t.EventId)
            .Select(g => new { EventId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.EventId, x => x.Count);
    }

    /// <summary>
    /// Like <see cref="GetSoldSeatCountsAsync"/>, but restricted to season-pass–derived tickets
    /// (<see cref="TicketKind.Season"/>). The remainder of an event's sold seats are ordinary
    /// single-event tickets. Events with no season tickets are absent from the dictionary.
    /// </summary>
    public async Task<Dictionary<int, int>> GetSeasonSoldSeatCountsAsync(IEnumerable<int> eventIds)
    {
        var ids = eventIds.Distinct().ToList();
        if (ids.Count == 0)
            return new Dictionary<int, int>();

        return await _context.Tickets
            .AsNoTracking()
            .Where(t => ids.Contains(t.EventId)
                        && t.Kind == TicketKind.Season
                        && t.Status != TicketStatuses.Cancelled)
            .GroupBy(t => t.EventId)
            .Select(g => new { EventId = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.EventId, x => x.Count);
    }

    public async Task<Event?> GetEventByIdAsync(int id)
    {
        return await _context.Events
            .Include(e => e.Tickets)
            .Include(e => e.Orders)
            .Include(e => e.StaffAssignments)
                .ThenInclude(sa => sa.Staff)
            .Include(e => e.Season)
            // Temporarily removed .Include(e => e.Analytics) due to database schema mismatch
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<bool> IsEventNameTakenAsync(string name, int? excludeEventId = null)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var normalized = name.Trim().ToLower();
        return await _context.Events
            .AnyAsync(e => e.EventName.ToLower() == normalized
                && (excludeEventId == null || e.Id != excludeEventId));
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
        existingEvent.HomeTeam = eventItem.HomeTeam;
        existingEvent.AwayTeam = eventItem.AwayTeam;
        existingEvent.Location = eventItem.Location;
        existingEvent.EventDate = eventItem.EventDate;
        existingEvent.EventEndDate = eventItem.EventEndDate;
        existingEvent.VenueId = eventItem.VenueId;
        existingEvent.TotalSeats = eventItem.TotalSeats;
        existingEvent.Description = eventItem.Description;
        existingEvent.ImageUrl = eventItem.ImageUrl;
        existingEvent.BaseTicketPrice = eventItem.BaseTicketPrice;
        existingEvent.SeasonId = eventItem.SeasonId;
        existingEvent.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        _logger.LogInformation("Updated event: {EventName} (ID: {EventId})", existingEvent.EventName, id);
        return existingEvent;
    }

    public async Task<List<EventSectorPriceDto>> GetSectorPricingAsync(int? eventId)
    {
        var overlays = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .OrderBy(o => o.SectorCode)
            .ToListAsync();

        var overrides = eventId == null
            ? new Dictionary<int, EventSectorPrice>()
            : await _context.EventSectorPrices
                .AsNoTracking()
                .Where(p => p.EventId == eventId.Value)
                .ToDictionaryAsync(p => p.SectorOverlayId);

        return overlays.Select(o =>
        {
            overrides.TryGetValue(o.Id, out var cfg);
            return new EventSectorPriceDto
            {
                SectorOverlayId = o.Id,
                SectorCode = o.SectorCode,
                SectorName = o.Name,
                Type = o.Type,
                SectorDefaultPrice = o.Price,
                EventPrice = cfg?.Price,
                IsDisabled = cfg?.IsDisabled ?? false
            };
        }).ToList();
    }

    public async Task SaveSectorPricesAsync(int eventId, IEnumerable<EventSectorPriceInputDto>? prices)
    {
        if (prices == null)
            return;

        // Keep the last entry per sector (guards against duplicate rows in the payload), and only those
        // that actually carry configuration — a price override, a disable, or both. An entry with neither
        // clears the sector's row.
        var desired = prices
            .GroupBy(p => p.SectorOverlayId)
            .Select(g => g.Last())
            .Where(p => p.Price.HasValue || p.IsDisabled)
            .ToDictionary(p => p.SectorOverlayId);

        // Only apply configuration to sectors that actually exist, so a stale payload can't create orphans.
        var validSectorIds = await _context.StadiumSectorOverlays
            .Where(o => !o.IsDeleted)
            .Select(o => o.Id)
            .ToListAsync();
        var validSet = validSectorIds.ToHashSet();

        var existing = await _context.EventSectorPrices
            .Where(p => p.EventId == eventId)
            .ToListAsync();
        var existingBySector = existing.ToDictionary(p => p.SectorOverlayId);

        // Remove rows no longer wanted (cleared, or for a sector that vanished).
        foreach (var row in existing)
        {
            if (!desired.TryGetValue(row.SectorOverlayId, out var input) || !validSet.Contains(row.SectorOverlayId))
            {
                _context.EventSectorPrices.Remove(row);
            }
            else
            {
                row.Price = input.Price; // Update in place.
                row.IsDisabled = input.IsDisabled;
            }
        }

        // Add rows for sectors that didn't have one yet.
        foreach (var (sectorId, input) in desired)
        {
            if (!validSet.Contains(sectorId) || existingBySector.ContainsKey(sectorId))
                continue;
            _context.EventSectorPrices.Add(new EventSectorPrice
            {
                EventId = eventId,
                SectorOverlayId = sectorId,
                Price = input.Price,
                IsDisabled = input.IsDisabled
            });
        }

        await _context.SaveChangesAsync();
    }

    public async Task<string?> ValidateSectorDisablesAsync(int eventId, IEnumerable<EventSectorPriceInputDto>? prices)
    {
        if (prices == null)
            return null;

        var toDisable = prices
            .Where(p => p.IsDisabled)
            .Select(p => p.SectorOverlayId)
            .ToHashSet();
        if (toDisable.Count == 0)
            return null;

        // "Sold" = any non-cancelled ticket (incl. season passes) occupying a seat in the sector for this
        // event. Reuse the customer flow's per-sector summary so the count matches what buyers actually see.
        var summaries = await _overlaySeats.GetSectionSummariesAsync(eventId);
        var blocked = toDisable
            .Where(id => summaries.TryGetValue(id, out var s) && s.SoldSeats > 0)
            .Select(id => summaries[id].Code)
            .OrderBy(code => code)
            .ToList();

        if (blocked.Count == 0)
            return null;

        return blocked.Count == 1
            ? $"Cannot disable sector {blocked[0]}: it already has tickets sold for this event."
            : $"Cannot disable sectors {string.Join(", ", blocked)}: they already have tickets sold for this event.";
    }

    public async Task<bool> DeleteEventAsync(int id)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
            return false;

        // Force delete: remove the event together with all its tickets and their dependent
        // sessions. Tickets/TicketSessions/OrderSessions hold Restrict FKs into Event/Ticket,
        // and Orders reference them through optional (client-set-null) shadow FKs, so we clear
        // everything explicitly inside a transaction before removing the event itself. The
        // remaining Event references cascade (EventStaffAssignment, EventAnalytics, CartItem,
        // SeatReservation) or are set null (Order, Notification) per the model's configured
        // delete behaviors.
        //
        // The context uses a retrying execution strategy (EnableRetryOnFailure), which forbids
        // a manually-started transaction unless the whole unit runs inside the strategy.
        var strategy = _context.Database.CreateExecutionStrategy();
        var ticketCount = await strategy.ExecuteAsync(async () =>
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();

            var ticketIds = await _context.Tickets
                .Where(t => t.EventId == id)
                .Select(t => t.Id)
                .ToListAsync();

            // TicketSessions restrict on both Event and Ticket. Including Orders lets EF null
            // the optional Order -> TicketSession shadow FK on the tracked orders.
            var ticketSessions = await _context.TicketSessions
                .Include(ts => ts.Orders)
                .Where(ts => ts.EventId == id || ticketIds.Contains(ts.TicketId))
                .ToListAsync();
            _context.TicketSessions.RemoveRange(ticketSessions);

            if (ticketIds.Count > 0)
            {
                // OrderSessions restrict on Ticket (required FK) — must go before the tickets.
                var orderSessions = await _context.OrderSessions
                    .Where(os => ticketIds.Contains(os.TicketId))
                    .ToListAsync();
                _context.OrderSessions.RemoveRange(orderSessions);

                // Including Orders lets EF null the optional Order -> Ticket shadow FK.
                var tickets = await _context.Tickets
                    .Include(t => t.Orders)
                    .Where(t => t.EventId == id)
                    .ToListAsync();
                _context.Tickets.RemoveRange(tickets);
            }

            await _context.SaveChangesAsync();

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            await transaction.CommitAsync();
            return ticketIds.Count;
        });

        _logger.LogInformation(
            "Force-deleted event {EventName} (ID: {EventId}) and {TicketCount} ticket(s)",
            eventItem.EventName, id, ticketCount);
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

    public async Task<EventStatusTransitionResult> TransitionEventStatusAsync(int id, EventStatus newStatus, bool systemAutoClose = false)
    {
        var eventItem = await _context.Events.FindAsync(id);
        if (eventItem == null)
            return EventStatusTransitionResult.Missing();

        var current = eventItem.Status;
        // Manual transitions follow the state machine; the time-driven auto-completer may additionally
        // close any non-terminal event straight to Completed once its window has passed.
        var permitted = EventLifecycle.CanTransition(current, newStatus)
            || (systemAutoClose && newStatus == EventStatus.Completed && EventLifecycle.CanAutoComplete(current));
        if (!permitted)
        {
            var allowed = string.Join(", ", EventLifecycle.AllowedNextStatuses(current));
            var message = string.IsNullOrEmpty(allowed)
                ? $"Event is in terminal state '{current}' and its status can no longer change."
                : $"Cannot move event from '{current}' to '{newStatus}'. Allowed next states: {allowed}.";
            _logger.LogWarning("Rejected status transition {From} -> {To} for event {EventId}", current, newStatus, id);
            return EventStatusTransitionResult.Invalid(message, current);
        }

        // Single-live-event invariant: only one event may occupy the Active phase (Active/InProgress)
        // at a time. Block promoting this event into that phase while a different event is already live.
        var enteringActivePhase = EventLifecycle.PhaseOf(newStatus) == EventPhase.Active &&
            EventLifecycle.PhaseOf(current) != EventPhase.Active;
        if (enteringActivePhase)
        {
            var live = await _context.Events
                .FirstOrDefaultAsync(e => e.Id != id &&
                    (e.Status == EventStatus.Active || e.Status == EventStatus.InProgress));
            if (live != null)
            {
                var message = $"Cannot make event '{eventItem.EventName}' live: event '{live.EventName}' " +
                    $"(ID: {live.Id}) is already in progress. Complete or cancel it first.";
                _logger.LogWarning("Rejected status transition {From} -> {To} for event {EventId}: " +
                    "event {LiveEventId} is already live", current, newStatus, id, live.Id);
                return EventStatusTransitionResult.Invalid(message, current);
            }
        }

        var now = DateTime.UtcNow;
        eventItem.Status = newStatus;
        eventItem.UpdatedAt = now;

        // Going live means the event is happening *now*. Slide a still-future window forward to
        // "now" (preserving its original duration) so drink ordering — which unlocks in the Active
        // phase and stamps orders at the current time — can never produce orders dated before the
        // event's own start. Mirrors the external "event went live" webhook path.
        if (enteringActivePhase && eventItem.EventDate > now)
        {
            var window = (eventItem.EventEndDate ?? eventItem.EventDate.AddHours(2)) - eventItem.EventDate;
            if (window <= TimeSpan.Zero) window = TimeSpan.FromHours(2);
            eventItem.EventDate = now;
            eventItem.EventEndDate = now.Add(window);
            _logger.LogInformation("Event {EventId} went live: start slid to {Start:o} (window {Window})",
                id, eventItem.EventDate, window);
        }

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

        // Phase 3 closure (cont.): close out any still-in-flight drink orders for a now-terminal event so
        // none lingers (e.g. as OutForDelivery) after the event is over. Runs after the transition is
        // committed above because the wallet refunds inside the sweep clear EF's change tracker.
        if (EventLifecycle.IsTerminal(newStatus))
        {
            var reason = $"Auto-cancelled: event {(newStatus == EventStatus.Cancelled ? "cancelled" : "completed")} (event #{id})";
            var cancelledOrders = await _orderService.CancelOpenOrdersForEventAsync(id, reason);
            if (cancelledOrders > 0)
                _logger.LogInformation("Cancelled {Count} in-flight order(s) while closing event {EventId}",
                    cancelledOrders, id);
        }

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

    public async Task<int> AutoCompleteEndedEventsAsync()
    {
        var now = DateTime.UtcNow;

        // Pull every non-terminal event (including ones that never went live — Planned/OnSale/SoldOut)
        // and evaluate the end-of-window in memory to avoid Npgsql DateTime.Kind pitfalls on
        // timestamptz comparisons. The window mirrors Event.IsLiveAt exactly: closed at EventEndDate
        // when set, otherwise at end of the start day.
        var openEvents = await _context.Events
            .Where(e => e.Status != EventStatus.Completed && e.Status != EventStatus.Cancelled)
            .Select(e => new { e.Id, e.EventDate, e.EventEndDate })
            .ToListAsync();

        var endedIds = openEvents
            .Where(e => e.EventEndDate.HasValue
                ? now > e.EventEndDate.Value
                : now.Date > e.EventDate.Date)
            .Select(e => e.Id)
            .ToList();

        foreach (var id in endedIds)
            await TransitionEventStatusAsync(id, EventStatus.Completed, systemAutoClose: true);

        if (endedIds.Count > 0)
            _logger.LogInformation("Auto-completed {Count} ended event(s): {EventIds}",
                endedIds.Count, string.Join(", ", endedIds));

        return endedIds.Count;
    }

    public async Task<IEnumerable<Event>> GetActiveEventsAsync()
    {
        // Close out any events whose window has elapsed before reporting the live set, so a
        // stale-Active event never surfaces as "current" on dashboards.
        await AutoCompleteEndedEventsAsync();

        // Use raw SQL for better performance
        var sql = @"
            SELECT e.*
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
            SELECT e.*
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
            SELECT e.*
            FROM ""Events"" e
            WHERE e.""EventDate"" < {0}
            ORDER BY e.""EventDate"" DESC";

        return await _context.Events
            .FromSqlRaw(sql, now)
            .AsNoTracking()
            .ToListAsync();
    }
}