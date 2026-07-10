using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ISeasonService _seasonService;
    private readonly ITicketIngestionService _ingestion;
    private readonly ILogger<EventController> _logger;

    public EventController(
        IEventService eventService,
        ISeasonService seasonService,
        ITicketIngestionService ingestion,
        ILogger<EventController> logger)
    {
        _eventService = eventService;
        _seasonService = seasonService;
        _ingestion = ingestion;
        _logger = logger;
    }

    /// <summary>
    /// Get all events with optional filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] bool activeOnly = false)
    {
        try
        {
            var events = await _eventService.GetEventsAsync(activeOnly);
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get currently active events
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetActiveEvents()
    {
        try
        {
            var events = await _eventService.GetActiveEventsAsync();
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get upcoming events
    /// </summary>
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetUpcomingEvents()
    {
        try
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving upcoming events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get past events
    /// </summary>
    [HttpGet("past")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetPastEvents()
    {
        try
        {
            var events = await _eventService.GetPastEventsAsync();
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving past events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Real stadium capacity — the sum of the drawing-tool overlay sectors' seats. This is the
    /// authoritative per-event capacity (every event is held in the same physical stadium), so the
    /// Admin event form surfaces it read-only rather than letting an arbitrary number be typed.
    /// Returns 0 when no stadium has been drawn yet.
    /// </summary>
    [HttpGet("stadium-capacity")]
    public async Task<ActionResult<int>> GetStadiumCapacity()
    {
        try
        {
            return Ok(await _ingestion.GetStadiumCapacityAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving stadium capacity");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Lists overlay sectors with their default price and the per-event override for each, used by
    /// the Admin event modal's "Sector prices" editor. Pass <paramref name="eventId"/> to prefill the
    /// existing event's overrides; omit it for a new (unsaved) event.
    /// </summary>
    [HttpGet("sector-prices")]
    public async Task<ActionResult<List<EventSectorPriceDto>>> GetSectorPrices([FromQuery] int? eventId = null)
    {
        try
        {
            return Ok(await _eventService.GetSectorPricingAsync(eventId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sector prices for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get event by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        try
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();
            return Ok(MapEventToDto(eventItem, stadiumCapacity: stadiumCapacity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get event analytics
    /// </summary>
    [HttpGet("{id}/analytics")]
    public async Task<ActionResult<EventAnalyticsResponse>> GetEventAnalytics(int id)
    {
        try
        {
            var analytics = await _eventService.GetEventAnalyticsAsync(id);
            if (analytics == null)
            {
                return NotFound($"Analytics for event {id} not found");
            }

            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving analytics for event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get post-event statistics (ticketing/occupancy + drink ordering + combined revenue) for a
    /// single event. Powers the admin event-statistics page opened from a completed event's card.
    /// </summary>
    [HttpGet("{id}/statistics")]
    public async Task<ActionResult<EventStatisticsDto>> GetEventStatistics(int id)
    {
        try
        {
            var stats = await _eventService.GetEventStatisticsAsync(id);
            if (stats == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving statistics for event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create new event
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidWindow(request.Date, request.EndDate, out var windowError))
            {
                return BadRequest(new { message = windowError });
            }

            if (!IsValidSalesWindow(request.TicketSalesStartDate, request.TicketSalesEndDate, out var salesWindowError))
            {
                return BadRequest(new { message = salesWindowError });
            }

            var name = request.Name.Trim();
            if (await _eventService.IsEventNameTakenAsync(name))
            {
                return Conflict(new { message = $"An event named \"{name}\" already exists. Event names must be unique." });
            }

            // Capacity is not client-supplied trivia: it is the real stadium seat count (sum of the
            // drawing-tool overlays). Compute it server-side so the stored TotalSeats always matches
            // the actual seating layout; only fall back to the request value before a stadium exists.
            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();

            var eventType = string.IsNullOrWhiteSpace(request.EventType) ? "Match" : request.EventType.Trim();
            if (!ResolveTeams(eventType, request.HomeTeam, request.AwayTeam, out var homeTeam, out var awayTeam, out var teamError))
            {
                return BadRequest(new { message = teamError });
            }

            var eventItem = new Event
            {
                EventName = name,
                EventType = eventType,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                EventDate = request.Date!.Value,
                EventEndDate = request.EndDate,
                TicketSalesStartDate = request.TicketSalesStartDate,
                TicketSalesEndDate = request.TicketSalesEndDate,
                TotalSeats = stadiumCapacity > 0 ? stadiumCapacity : request.Capacity,
                Description = request.Description,
                BaseTicketPrice = request.BasePrice,
                IsActive = request.IsActive,
                SeasonId = request.SeasonId
            };

            var createdEvent = await _eventService.CreateEventAsync(eventItem);

            // Persist any per-sector price overrides supplied with the new event.
            await _eventService.SaveSectorPricesAsync(createdEvent.Id, request.SectorPrices);

            // If linked to a season, extend existing season passes to cover this new event.
            if (createdEvent.SeasonId != null)
                await _ingestion.BackfillSeasonTicketsForEventAsync(createdEvent.Id);

            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, await MapEventWithSeasonAsync(createdEvent));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update existing event
    /// </summary>
    [HttpPut("{id}")]
    [HttpPost("{id}")] // Alias: the Admin HTTP helper only issues POST/GET/DELETE.
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int id, [FromBody] UpdateEventDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Load first so unspecified (null) fields retain their existing values.
            var existing = await _eventService.GetEventByIdAsync(id);
            if (existing == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            // Past/terminal events are frozen: their record can no longer be edited.
            if (!EventLifecycle.CanEdit(existing.Status))
            {
                return BadRequest(new { message = EventLifecycle.EditBlockedReason(existing.Status) });
            }

            // A sector with tickets already sold for this event can't be disabled (would orphan buyers).
            // Validate before persisting anything so a rejected disable leaves the event untouched.
            var disableBlock = await _eventService.ValidateSectorDisablesAsync(id, request.SectorPrices);
            if (disableBlock != null)
            {
                return BadRequest(new { message = disableBlock });
            }

            var newStart = request.Date ?? existing.EventDate;
            var newEnd = request.EndDate ?? existing.EventEndDate;
            if (!IsValidWindow(newStart, newEnd, out var windowError))
            {
                return BadRequest(new { message = windowError });
            }

            // Null in the request means "unchanged" (matches the event-window merge above), so validate
            // the effective (merged) sales window.
            var newSalesStart = request.TicketSalesStartDate ?? existing.TicketSalesStartDate;
            var newSalesEnd = request.TicketSalesEndDate ?? existing.TicketSalesEndDate;
            if (!IsValidSalesWindow(newSalesStart, newSalesEnd, out var salesWindowError))
            {
                return BadRequest(new { message = salesWindowError });
            }

            var newSeasonId = request.SeasonId ?? existing.SeasonId;
            var seasonLinkIsNew = newSeasonId != null && newSeasonId != existing.SeasonId;

            var newName = string.IsNullOrWhiteSpace(request.Name) ? existing.EventName : request.Name.Trim();
            if (await _eventService.IsEventNameTakenAsync(newName, id))
            {
                return Conflict(new { message = $"An event named \"{newName}\" already exists. Event names must be unique." });
            }

            // Re-derive capacity from the real stadium (sum of overlays) rather than trusting the
            // request; the Admin form shows it read-only. Fall back to the existing value only when
            // no stadium has been drawn yet.
            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();

            // Null in the request means "unchanged", so validate the effective (merged) type/teams.
            var newType = string.IsNullOrWhiteSpace(request.EventType) ? existing.EventType : request.EventType.Trim();
            if (!ResolveTeams(newType, request.HomeTeam ?? existing.HomeTeam, request.AwayTeam ?? existing.AwayTeam,
                    out var homeTeam, out var awayTeam, out var teamError))
            {
                return BadRequest(new { message = teamError });
            }

            var eventItem = new Event
            {
                EventName = newName,
                EventType = newType,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                EventDate = newStart,
                EventEndDate = newEnd,
                TicketSalesStartDate = newSalesStart,
                TicketSalesEndDate = newSalesEnd,
                VenueId = existing.VenueId,
                TotalSeats = stadiumCapacity > 0 ? stadiumCapacity : (request.Capacity ?? existing.TotalSeats),
                Description = request.Description ?? existing.Description,
                ImageUrl = existing.ImageUrl,
                BaseTicketPrice = request.BasePrice ?? existing.BaseTicketPrice,
                SeasonId = newSeasonId
            };

            var updatedEvent = await _eventService.UpdateEventAsync(id, eventItem);
            if (updatedEvent == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            // Apply per-sector price overrides (null = leave existing overrides unchanged).
            await _eventService.SaveSectorPricesAsync(id, request.SectorPrices);

            // Newly linking an event to a season backfills its existing season passes.
            if (seasonLinkIsNew)
                await _ingestion.BackfillSeasonTicketsForEventAsync(updatedEvent.Id);

            return Ok(await MapEventWithSeasonAsync(updatedEvent));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Validates the optional event window: an end must come strictly after the start.
    /// A missing start or end is allowed (open-ended window).
    /// </summary>
    private static bool IsValidWindow(DateTime? start, DateTime? end, out string error)
    {
        if (start.HasValue && end.HasValue && end.Value <= start.Value)
        {
            error = "The event end time must be after the start time.";
            return false;
        }
        error = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the optional ticket-sales window: when both bounds are supplied the end must come
    /// strictly after the start. A missing start or end is allowed (that side of the window is open).
    /// </summary>
    private static bool IsValidSalesWindow(DateTime? start, DateTime? end, out string error)
    {
        if (start.HasValue && end.HasValue && end.Value <= start.Value)
        {
            error = "The ticket sales end time must be after the sales start time.";
            return false;
        }
        error = string.Empty;
        return true;
    }

    /// <summary>
    /// Delete event
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        try
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Activate event
    /// </summary>
    [HttpPost("{id}/activate")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ActivateEvent(int id)
    {
        try
        {
            var result = await _eventService.ActivateEventAsync(id);
            if (!result)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(new { message = "Event activated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deactivate event
    /// </summary>
    [HttpPost("{id}/deactivate")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeactivateEvent(int id)
    {
        try
        {
            var result = await _eventService.DeactivateEventAsync(id);
            if (!result)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(new { message = "Event deactivated successfully" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Transition an event to a new lifecycle status (e.g. OnSale → Active → InProgress → Completed).
    /// Validated against the allowed state machine; closing an event invalidates its ticket sessions.
    /// </summary>
    [HttpPost("{id}/status")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDto>> TransitionStatus(int id, [FromBody] TransitionEventStatusRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.TransitionEventStatusAsync(id, request.NewStatus);
            if (result.NotFound)
            {
                return NotFound($"Event with ID {id} not found");
            }

            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            var updated = await _eventService.GetEventByIdAsync(id);
            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();
            return Ok(MapEventToDto(updated!, stadiumCapacity: stadiumCapacity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error transitioning status for event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Maps Event entity to EventDto
    /// </summary>
    /// <summary>
    /// Maps a list of events to DTOs, batch-fetching sold-seat counts in a single query
    /// because the list data source (raw SQL) does not load each event's Tickets navigation.
    /// </summary>
    private async Task<List<EventDto>> MapEventsWithSoldCountsAsync(IEnumerable<Event> events)
    {
        var eventList = events.ToList();
        var ids = eventList.Select(e => e.Id).ToList();
        var soldCounts = await _eventService.GetSoldSeatCountsAsync(ids);
        var seasonSoldCounts = await _eventService.GetSeasonSoldSeatCountsAsync(ids);
        var seasonNames = await _seasonService.GetSeasonNamesAsync(
            eventList.Where(e => e.SeasonId != null).Select(e => e.SeasonId!.Value));
        var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();

        return eventList
            .Select(e => MapEventToDto(
                e,
                soldCounts.GetValueOrDefault(e.Id, 0),
                e.SeasonId != null ? seasonNames.GetValueOrDefault(e.SeasonId.Value) : null,
                seasonSoldCounts.GetValueOrDefault(e.Id, 0),
                stadiumCapacity))
            .ToList();
    }

    /// <summary>
    /// Normalizes and validates the home/away teams against the event type. A "Match" must carry both
    /// a home side (a resident club) and an away side; any other type carries no teams (both cleared).
    /// Returns false with <paramref name="error"/> set when a Match is missing a team.
    /// </summary>
    private static bool ResolveTeams(string eventType, string? home, string? away,
        out string? homeTeam, out string? awayTeam, out string? error)
    {
        error = null;
        if (!string.Equals(eventType, "Match", StringComparison.OrdinalIgnoreCase))
        {
            // Non-match events are single-title: never keep stale team labels.
            homeTeam = null;
            awayTeam = null;
            return true;
        }

        homeTeam = string.IsNullOrWhiteSpace(home) ? null : home.Trim();
        awayTeam = string.IsNullOrWhiteSpace(away) ? null : away.Trim();
        if (homeTeam == null || awayTeam == null)
        {
            error = "A Match event requires both a home team and an away team.";
            return false;
        }
        return true;
    }

    /// <summary>Maps a single event, resolving its season name (used by create/update responses).</summary>
    private async Task<EventDto> MapEventWithSeasonAsync(Event evt)
    {
        string? seasonName = null;
        if (evt.SeasonId != null)
        {
            var names = await _seasonService.GetSeasonNamesAsync(new[] { evt.SeasonId.Value });
            seasonName = names.GetValueOrDefault(evt.SeasonId.Value);
        }
        var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();
        return MapEventToDto(evt, seasonName: seasonName, stadiumCapacity: stadiumCapacity);
    }

    /// <summary>
    /// Maps an <see cref="Event"/> to <see cref="EventDto"/>.
    /// </summary>
    /// <param name="soldSeatsOverride">
    /// Sold-seat count to use when the event's Tickets navigation is not loaded (list
    /// endpoints use raw SQL that omits it). When null, sold seats are counted from the
    /// loaded Tickets navigation. Both paths funnel through <see cref="TicketStatuses.CountsAsSold"/>.
    /// </param>
    private EventDto MapEventToDto(Event evt, int? soldSeatsOverride = null, string? seasonName = null, int? seasonSoldOverride = null, int? stadiumCapacity = null)
    {
        if (evt == null)
        {
            return null;
        }

        // Capacity reflects the REAL stadium (sum of the drawing-tool overlay sectors) whenever a
        // stadium has been drawn; otherwise fall back to the event's stored TotalSeats. This keeps
        // the dashboard "od X mjesta" figure in sync with the actual seating layout.
        int capacity = stadiumCapacity is > 0 ? stadiumCapacity.Value : evt.TotalSeats;

        // Calculate available seats based on sold tickets. List endpoints pass an explicit
        // count (their Tickets navigation isn't loaded); single-event endpoints load Tickets.
        int soldSeats = soldSeatsOverride
            ?? evt.Tickets?.Count(t => TicketStatuses.CountsAsSold(t.Status))
            ?? 0;

        // Of those sold seats, how many are season-pass–derived (the rest are single-event).
        // Same override/loaded-navigation split as above; clamp so it never exceeds total sold.
        int seasonSold = seasonSoldOverride
            ?? evt.Tickets?.Count(t => t.Kind == TicketKind.Season && TicketStatuses.CountsAsSold(t.Status))
            ?? 0;

        return new EventDto
        {
            Id = evt.Id,
            Name = !string.IsNullOrWhiteSpace(evt.EventName) ? evt.EventName : $"Event {evt.Id}",
            EventType = string.IsNullOrWhiteSpace(evt.EventType) ? "Match" : evt.EventType,
            HomeTeam = evt.HomeTeam,
            AwayTeam = evt.AwayTeam,
            Date = evt.EventDate,
            EndDate = evt.EventEndDate,
            TicketSalesStartDate = evt.TicketSalesStartDate,
            TicketSalesEndDate = evt.TicketSalesEndDate,
            Description = evt.Description,
            Capacity = capacity,
            AvailableSeats = Math.Max(0, capacity - soldSeats),
            SeasonTicketsSold = Math.Min(seasonSold, soldSeats),
            BasePrice = evt.BaseTicketPrice ?? 0m,
            IsActive = evt.IsActive,
            CreatedAt = evt.CreatedAt,
            Status = evt.Status,
            StatusName = evt.Status.ToString(),
            Phase = EventLifecycle.PhaseOf(evt.Status),
            CanSellTickets = evt.AreTicketSalesOpenAt(DateTime.UtcNow),
            CanOrderDrinks = EventLifecycle.CanOrderDrinks(evt.Status),
            IsCurrentlyLive = evt.IsLiveAt(DateTime.UtcNow),
            SeasonId = evt.SeasonId,
            SeasonName = seasonName ?? evt.Season?.Name
        };
    }
}