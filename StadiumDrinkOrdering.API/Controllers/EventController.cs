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

            return Ok(MapEventToDto(eventItem));
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

            var eventItem = new Event
            {
                EventName = request.Name,
                // The admin UI's "Location" maps to Event.EventType (see MapEventToDto).
                EventType = string.IsNullOrWhiteSpace(request.Location) ? "General" : request.Location,
                EventDate = request.Date!.Value,
                EventEndDate = request.EndDate,
                TotalSeats = request.Capacity,
                Description = request.Description,
                BaseTicketPrice = request.BasePrice,
                IsActive = request.IsActive,
                SeasonId = request.SeasonId
            };

            var createdEvent = await _eventService.CreateEventAsync(eventItem);

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

            var newStart = request.Date ?? existing.EventDate;
            var newEnd = request.EndDate ?? existing.EventEndDate;
            if (!IsValidWindow(newStart, newEnd, out var windowError))
            {
                return BadRequest(new { message = windowError });
            }

            var newSeasonId = request.SeasonId ?? existing.SeasonId;
            var seasonLinkIsNew = newSeasonId != null && newSeasonId != existing.SeasonId;

            var eventItem = new Event
            {
                EventName = string.IsNullOrWhiteSpace(request.Name) ? existing.EventName : request.Name,
                EventType = string.IsNullOrWhiteSpace(request.Location) ? existing.EventType : request.Location,
                EventDate = newStart,
                EventEndDate = newEnd,
                VenueId = existing.VenueId,
                TotalSeats = request.Capacity ?? existing.TotalSeats,
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
            return Ok(MapEventToDto(updated!));
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
        var soldCounts = await _eventService.GetSoldSeatCountsAsync(eventList.Select(e => e.Id));
        var seasonNames = await _seasonService.GetSeasonNamesAsync(
            eventList.Where(e => e.SeasonId != null).Select(e => e.SeasonId!.Value));

        return eventList
            .Select(e => MapEventToDto(
                e,
                soldCounts.GetValueOrDefault(e.Id, 0),
                e.SeasonId != null ? seasonNames.GetValueOrDefault(e.SeasonId.Value) : null))
            .ToList();
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
        return MapEventToDto(evt, seasonName: seasonName);
    }

    /// <summary>
    /// Maps an <see cref="Event"/> to <see cref="EventDto"/>.
    /// </summary>
    /// <param name="soldSeatsOverride">
    /// Sold-seat count to use when the event's Tickets navigation is not loaded (list
    /// endpoints use raw SQL that omits it). When null, sold seats are counted from the
    /// loaded Tickets navigation. Both paths funnel through <see cref="TicketStatuses.CountsAsSold"/>.
    /// </param>
    private EventDto MapEventToDto(Event evt, int? soldSeatsOverride = null, string? seasonName = null)
    {
        if (evt == null)
        {
            return null;
        }

        // Calculate available seats based on sold tickets. List endpoints pass an explicit
        // count (their Tickets navigation isn't loaded); single-event endpoints load Tickets.
        int soldSeats = soldSeatsOverride
            ?? evt.Tickets?.Count(t => TicketStatuses.CountsAsSold(t.Status))
            ?? 0;

        return new EventDto
        {
            Id = evt.Id,
            Name = !string.IsNullOrWhiteSpace(evt.EventName) ? evt.EventName : $"Event {evt.Id}",
            Date = evt.EventDate,
            EndDate = evt.EventEndDate,
            Description = evt.Description,
            Location = evt.EventType ?? "Main Stadium", // Using EventType as location for now
            Capacity = evt.TotalSeats,
            AvailableSeats = Math.Max(0, evt.TotalSeats - soldSeats),
            BasePrice = evt.BaseTicketPrice ?? 0m,
            IsActive = evt.IsActive,
            CreatedAt = evt.CreatedAt,
            Status = evt.Status,
            StatusName = evt.Status.ToString(),
            Phase = EventLifecycle.PhaseOf(evt.Status),
            CanSellTickets = EventLifecycle.CanSellTickets(evt.Status),
            CanOrderDrinks = EventLifecycle.CanOrderDrinks(evt.Status),
            IsCurrentlyLive = evt.IsLiveAt(DateTime.UtcNow),
            SeasonId = evt.SeasonId,
            SeasonName = seasonName ?? evt.Season?.Name
        };
    }
}