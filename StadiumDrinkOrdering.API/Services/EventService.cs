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

    /// <summary>
    /// Post-event statistics (ticketing/occupancy + drink ordering + combined revenue) for a single
    /// event, computed live from tickets and orders. Returns null when the event does not exist.
    /// Excludes cancelled tickets and cancelled orders so the figures reflect realised sales.
    /// </summary>
    Task<EventStatisticsDto?> GetEventStatisticsAsync(int id);
    Task<IEnumerable<Event>> GetActiveEventsAsync();
    /// <summary>
    /// Transitions any non-terminal event whose time window has already elapsed to
    /// <see cref="EventStatus.Completed"/> — both stale-live events (Active/InProgress) and ones that
    /// never went live (Planned/OnSale/SoldOut) but whose date has passed. Returns the number of
    /// events closed. Called opportunistically on read so past-dated events don't linger.
    /// </summary>
    Task<int> AutoCompleteEndedEventsAsync();

    /// <summary>
    /// Time-driven counterpart to <see cref="AutoCompleteEndedEventsAsync"/> at the start of the window:
    /// brings a sellable/published event (OnSale/SoldOut) live — transitioning it to
    /// <see cref="EventStatus.Active"/> — once its scheduled window has opened (started, not yet ended),
    /// so drink ordering unlocks at kickoff without a manual "make live" click. Respects the single-live
    /// invariant (at most one Active/InProgress event), so it promotes at most one event per pass and
    /// does nothing while another event is already live. Returns the number of events activated.
    /// </summary>
    Task<int> AutoActivateStartedEventsAsync();

    /// <summary>
    /// One-time backfill: reconciles orders for events that are already terminal (Completed/Cancelled)
    /// but still have in-flight (non-terminal) drink orders — historical orphans left behind before the
    /// on-completion order sweep existed. Re-runs the same idempotent, refund-correct
    /// <see cref="IOrderService.CancelOpenOrdersForEventAsync"/> per affected event, so it restores stock
    /// and refunds wallet-funded orders exactly like the live completion path. Safe to run repeatedly
    /// (does nothing once every terminal event is clean). Returns the total number of orders reconciled.
    /// Pass <paramref name="eventId"/> to reconcile only that one event (a no-op unless it is terminal);
    /// pass null to sweep every terminal event.
    /// </summary>
    Task<int> ReconcileTerminalEventOrdersAsync(int? eventId = null);

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

    /// <summary>
    /// Lists every staff member (Bartender/Waiter) together with whether they are assigned to work the
    /// event. Powers the "Staff" picker in the Admin event modal. For a new (unsaved) event pass null
    /// (nobody is assigned yet).
    /// </summary>
    Task<List<EventStaffMemberDto>> GetEventStaffAsync(int? eventId);

    /// <summary>
    /// Replaces an event's staff assignments with <paramref name="staff"/>: members not in the set are
    /// unassigned; each supplied member is assigned with their function (Runner/Barman) and covered
    /// sectors. Ids that are not Bartenders/Waiters, and unknown sector ids, are ignored. A null
    /// <paramref name="staff"/> is a no-op (leaves existing assignments untouched).
    /// </summary>
    Task SaveEventStaffAsync(int eventId, IEnumerable<EventStaffInputDto>? staff);
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
        existingEvent.EventDate = eventItem.EventDate;
        existingEvent.EventEndDate = eventItem.EventEndDate;
        existingEvent.TicketSalesStartDate = eventItem.TicketSalesStartDate;
        existingEvent.TicketSalesEndDate = eventItem.TicketSalesEndDate;
        existingEvent.DrinkSalesStartDate = eventItem.DrinkSalesStartDate;
        existingEvent.DrinkSalesEndDate = eventItem.DrinkSalesEndDate;
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

    // Staff eligible to be assigned to an event: the two on-the-floor roles.
    private static readonly UserRole[] AssignableStaffRoles = { UserRole.Bartender, UserRole.Waiter };

    public async Task<List<EventStaffMemberDto>> GetEventStaffAsync(int? eventId)
    {
        var staff = await _context.Users
            .AsNoTracking()
            .Where(u => AssignableStaffRoles.Contains(u.Role))
            .OrderBy(u => u.Username)
            .Select(u => new { u.Id, u.Username, u.FirstName, u.LastName, u.Email, u.Role, u.IsActive })
            .ToListAsync();

        var assignments = eventId == null
            ? new Dictionary<int, EventStaffAssignment>()
            : await _context.EventStaffAssignments
                .AsNoTracking()
                .Where(a => a.EventId == eventId.Value)
                .ToDictionaryAsync(a => a.StaffId);

        return staff.Select(u =>
        {
            assignments.TryGetValue(u.Id, out var a);
            return new EventStaffMemberDto
            {
                StaffId = u.Id,
                Username = u.Username,
                FullName = $"{u.FirstName} {u.LastName}".Trim(),
                Email = u.Email,
                Role = u.Role,
                RoleName = u.Role.ToString(),
                IsActive = u.IsActive,
                IsAssigned = a != null,
                // Editing: use the stored function; otherwise default from the system role.
                EventRole = a != null
                    ? EventStaffRoles.Normalize(a.Role, EventStaffRoles.DefaultFor(u.Role))
                    : EventStaffRoles.DefaultFor(u.Role),
                SectorOverlayIds = ParseSectorIds(a?.AssignedSections)
            };
        }).ToList();
    }

    /// <summary>Deserializes a stored <c>AssignedSections</c> JSON array (e.g. "[12,13]") to ids; empty on error/null.</summary>
    private static List<int> ParseSectorIds(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return new List<int>();
        try
        {
            return System.Text.Json.JsonSerializer.Deserialize<List<int>>(json) ?? new List<int>();
        }
        catch
        {
            return new List<int>();
        }
    }

    public async Task SaveEventStaffAsync(int eventId, IEnumerable<EventStaffInputDto>? staff)
    {
        if (staff == null)
            return;

        // Keep the last entry per staff member (guards against duplicate rows in the payload).
        var desired = staff
            .GroupBy(s => s.StaffId)
            .ToDictionary(g => g.Key, g => g.Last());

        // Only assignable staff (Bartender/Waiter) may be assigned, so a stale payload can't attach a
        // customer/admin id. Keep each valid id's system role to default the event function.
        var validRoleById = await _context.Users
            .Where(u => AssignableStaffRoles.Contains(u.Role))
            .Select(u => new { u.Id, u.Role })
            .ToDictionaryAsync(x => x.Id, x => x.Role);

        // Sector ids are validated against the live overlay sectors so a stale payload can't persist
        // references to sectors that don't exist.
        var validSectorIds = (await _context.StadiumSectorOverlays
            .Where(o => !o.IsDeleted)
            .Select(o => o.Id)
            .ToListAsync()).ToHashSet();

        var existing = await _context.EventStaffAssignments
            .Where(a => a.EventId == eventId)
            .ToListAsync();
        var existingByStaff = existing.ToDictionary(a => a.StaffId);

        // Remove assignments no longer wanted.
        foreach (var row in existing)
        {
            if (!desired.ContainsKey(row.StaffId))
                _context.EventStaffAssignments.Remove(row);
        }

        foreach (var (staffId, input) in desired)
        {
            if (!validRoleById.TryGetValue(staffId, out var systemRole))
                continue; // ignore non-staff ids

            var role = EventStaffRoles.Normalize(input.EventRole, EventStaffRoles.DefaultFor(systemRole));
            // A barman works the bar, not sectors — never persist sector coverage for one.
            var sectorsJson = role == EventStaffRoles.Bartender
                ? null
                : SerializeSectorIds(input.SectorOverlayIds, validSectorIds);

            if (existingByStaff.TryGetValue(staffId, out var row))
            {
                // Update the existing assignment in place.
                row.Role = role;
                row.AssignedSections = sectorsJson;
                row.IsActive = true;
            }
            else
            {
                _context.EventStaffAssignments.Add(new EventStaffAssignment
                {
                    EventId = eventId,
                    StaffId = staffId,
                    Role = role,
                    AssignedSections = sectorsJson,
                    IsActive = true
                });
            }
        }

        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Serializes the requested sector ids to a JSON array, keeping only ids that are real overlay
    /// sectors (distinct, ordered). Returns null when nothing valid remains, so the column stays empty.
    /// </summary>
    private static string? SerializeSectorIds(IEnumerable<int>? ids, HashSet<int> validSectorIds)
    {
        if (ids == null)
            return null;
        var kept = ids.Where(validSectorIds.Contains).Distinct().OrderBy(x => x).ToList();
        return kept.Count == 0 ? null : System.Text.Json.JsonSerializer.Serialize(kept);
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

    public async Task<EventStatisticsDto?> GetEventStatisticsAsync(int id)
    {
        var evt = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.Id == id);
        if (evt == null)
            return null;

        // Ticketing / occupancy — non-cancelled tickets only (they still occupy a seat).
        var tickets = await _context.Tickets
            .AsNoTracking()
            .Where(t => t.EventId == id && t.Status != TicketStatuses.Cancelled)
            .Select(t => new { t.Kind, t.Price })
            .ToListAsync();

        var totalTicketsSold = tickets.Count;
        var seasonTicketsSold = tickets.Count(t => t.Kind == TicketKind.Season);
        var ticketRevenue = tickets.Sum(t => t.Price);

        // Physical capacity = sum of drawn overlay sectors' seats (mirrors ITicketIngestionService).
        var overlays = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .ToListAsync();
        var totalCapacity = overlays.Sum(o => o.TotalSeats);
        var occupancyPercent = totalCapacity > 0
            ? Math.Round((decimal)totalTicketsSold / totalCapacity * 100m, 1)
            : 0m;

        // Drink ordering — exclude cancelled orders so revenue reflects realised sales.
        var orders = await _context.Orders
            .AsNoTracking()
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
            .Where(o => o.EventId == id && o.Status != OrderStatus.Cancelled)
            .ToListAsync();

        var totalOrders = orders.Count;
        var orderItems = orders.SelectMany(o => o.OrderItems).ToList();
        var totalDrinksSold = orderItems.Sum(oi => oi.Quantity);
        var drinksRevenue = orders.Sum(o => o.TotalAmount);
        var averageOrderValue = totalOrders > 0 ? Math.Round(drinksRevenue / totalOrders, 2) : 0m;

        var mostPopularDrink = orderItems
            .GroupBy(oi => oi.Drink != null ? oi.Drink.Name : "Unknown")
            .OrderByDescending(g => g.Sum(x => x.Quantity))
            .FirstOrDefault()?.Key;

        return new EventStatisticsDto
        {
            EventId = evt.Id,
            EventName = evt.EventName,
            EventDate = evt.EventDate,
            Status = evt.Status.ToString(),
            TotalTicketsSold = totalTicketsSold,
            SeasonTicketsSold = seasonTicketsSold,
            TotalCapacity = totalCapacity,
            OccupancyPercent = occupancyPercent,
            TicketRevenue = ticketRevenue,
            TotalOrders = totalOrders,
            TotalDrinksSold = totalDrinksSold,
            DrinksRevenue = drinksRevenue,
            AverageOrderValue = averageOrderValue,
            MostPopularDrink = mostPopularDrink,
            TotalRevenue = ticketRevenue + drinksRevenue,
            CalculatedAt = DateTime.UtcNow
        };
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

    public async Task<int> AutoActivateStartedEventsAsync()
    {
        var now = DateTime.UtcNow;

        // Single-live invariant: only one event may be Active/InProgress at a time. If one is already
        // live, we cannot promote another — nothing to do this pass.
        var alreadyLive = await _context.Events
            .AnyAsync(e => e.Status == EventStatus.Active || e.Status == EventStatus.InProgress);
        if (alreadyLive)
            return 0;

        // Sellable/published events (OnSale/SoldOut) whose scheduled window is currently open — started
        // but not yet ended — should go live. Evaluate the window in memory to avoid Npgsql timestamptz
        // Kind pitfalls; the window mirrors Event.IsLiveAt (closed at EventEndDate when set, otherwise at
        // end of the start day). Past-window events are left to the auto-completer, not activated here.
        var candidates = await _context.Events
            .Where(e => e.Status == EventStatus.OnSale || e.Status == EventStatus.SoldOut)
            .Select(e => new { e.Id, e.EventDate, e.EventEndDate })
            .ToListAsync();

        var startedIds = candidates
            .Where(e => now >= e.EventDate
                && (e.EventEndDate.HasValue ? now <= e.EventEndDate.Value : now.Date <= e.EventDate.Date))
            .OrderBy(e => e.EventDate) // earliest-started open event wins the single live slot
            .Select(e => e.Id)
            .ToList();

        // Promote at most one: the first success takes the single live slot; the rest can't be promoted.
        foreach (var id in startedIds)
        {
            var result = await TransitionEventStatusAsync(id, EventStatus.Active);
            if (result.Success)
            {
                _logger.LogInformation("Auto-activated started event {EventId} (window open, went live)", id);
                return 1;
            }
        }

        return 0;
    }

    public async Task<int> ReconcileTerminalEventOrdersAsync(int? eventId = null)
    {
        var openStatuses = new[]
        {
            OrderStatus.Pending, OrderStatus.Accepted, OrderStatus.InPreparation,
            OrderStatus.Ready, OrderStatus.OutForDelivery
        };

        // Find already-terminal event(s) that still carry at least one in-flight order. These are the
        // pre-sweep orphans (e.g. orders stuck in InPreparation on a long-completed event). When a
        // specific eventId is requested, restrict to it so only that event's orders are touched — and
        // still only if it is itself terminal, so a live event's orders are never cancelled here.
        var eventIds = await _context.Orders
            .Where(o => o.EventId != null
                && (eventId == null || o.EventId == eventId)
                && openStatuses.Contains(o.Status)
                && o.Event != null
                && (o.Event.Status == EventStatus.Completed || o.Event.Status == EventStatus.Cancelled))
            .Select(o => o.EventId!.Value)
            .Distinct()
            .ToListAsync();

        var total = 0;
        foreach (var id in eventIds)
        {
            // Reuse the live sweep so stock restoration and idempotent wallet refunds happen identically.
            total += await _orderService.CancelOpenOrdersForEventAsync(
                id, $"Auto-cancelled: reconciling orphaned orders for ended event (event #{id})");
        }

        if (eventIds.Count > 0)
            _logger.LogInformation(
                "Reconciled {OrderCount} orphaned in-flight order(s) across {EventCount} terminal event(s): {EventIds}",
                total, eventIds.Count, string.Join(", ", eventIds));

        return total;
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