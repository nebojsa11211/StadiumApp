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
    private readonly ILogger<EventController> _logger;

    public EventController(IEventService eventService, ILogger<EventController> logger)
    {
        _eventService = eventService;
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
            var eventDtos = events.Select(e => MapEventToDto(e));
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
            var eventDtos = events.Select(e => MapEventToDto(e));
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
            var eventDtos = events.Select(e => MapEventToDto(e));
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
            var eventDtos = events.Select(e => MapEventToDto(e));
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
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventItem = new Event
            {
                EventName = request.EventName,
                EventType = request.EventType,
                EventDate = request.EventDate,
                VenueId = request.VenueId,
                TotalSeats = request.TotalSeats,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                BaseTicketPrice = request.BaseTicketPrice
            };

            var createdEvent = await _eventService.CreateEventAsync(eventItem);
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, MapEventToDto(createdEvent));
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
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int id, [FromBody] UpdateEventRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var eventItem = new Event
            {
                EventName = request.EventName,
                EventType = request.EventType,
                EventDate = request.EventDate,
                VenueId = request.VenueId,
                TotalSeats = request.TotalSeats,
                Description = request.Description,
                ImageUrl = request.ImageUrl,
                BaseTicketPrice = request.BaseTicketPrice
            };

            var updatedEvent = await _eventService.UpdateEventAsync(id, eventItem);
            if (updatedEvent == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(MapEventToDto(updatedEvent));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
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
                return NotFound($"Event with ID {id} not found or cannot be deleted (has tickets sold)");
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
    /// Maps Event entity to EventDto
    /// </summary>
    private EventDto MapEventToDto(Event evt)
    {
        if (evt == null)
        {
            return null;
        }

        // Calculate available seats based on sold tickets
        int soldSeats = 0;
        if (evt.Tickets != null)
        {
            soldSeats = evt.Tickets.Count(t => t.Status == "Active" || t.Status == "Used");
        }

        return new EventDto
        {
            Id = evt.Id,
            Name = !string.IsNullOrWhiteSpace(evt.EventName) ? evt.EventName : $"Event {evt.Id}",
            Date = evt.EventDate,
            Description = evt.Description,
            Location = evt.EventType ?? "Main Stadium", // Using EventType as location for now
            Capacity = evt.TotalSeats,
            AvailableSeats = Math.Max(0, evt.TotalSeats - soldSeats),
            BasePrice = evt.BaseTicketPrice ?? 0m,
            IsActive = evt.IsActive,
            CreatedAt = evt.CreatedAt
        };
    }
}

// DTOs for Event Controller
public class CreateEventRequest
{
    [Required]
    [StringLength(200)]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string EventType { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; }

    public int? VenueId { get; set; }

    [Required]
    [Range(1, 100000)]
    public int TotalSeats { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Range(0.01, 9999.99)]
    public decimal? BaseTicketPrice { get; set; }
}

public class UpdateEventRequest
{
    [Required]
    [StringLength(200)]
    public string EventName { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string EventType { get; set; } = string.Empty;

    [Required]
    public DateTime EventDate { get; set; }

    public int? VenueId { get; set; }

    [Required]
    [Range(1, 100000)]
    public int TotalSeats { get; set; }

    [StringLength(1000)]
    public string? Description { get; set; }

    [StringLength(500)]
    public string? ImageUrl { get; set; }

    [Range(0.01, 9999.99)]
    public decimal? BaseTicketPrice { get; set; }
}