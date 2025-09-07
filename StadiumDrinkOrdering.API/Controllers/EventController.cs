using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.Models;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("api/events")]
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
    public async Task<ActionResult<IEnumerable<Event>>> GetEvents([FromQuery] bool activeOnly = false)
    {
        try
        {
            var events = await _eventService.GetEventsAsync(activeOnly);
            return Ok(events);
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
    public async Task<ActionResult<IEnumerable<Event>>> GetActiveEvents()
    {
        try
        {
            var events = await _eventService.GetActiveEventsAsync();
            return Ok(events);
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
    public async Task<ActionResult<IEnumerable<Event>>> GetUpcomingEvents()
    {
        try
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            return Ok(events);
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
    public async Task<ActionResult<IEnumerable<Event>>> GetPastEvents()
    {
        try
        {
            var events = await _eventService.GetPastEventsAsync();
            return Ok(events);
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
    public async Task<ActionResult<Event>> GetEvent(int id)
    {
        try
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(eventItem);
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Event>> CreateEvent([FromBody] CreateEventRequest request)
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
            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, createdEvent);
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
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Event>> UpdateEvent(int id, [FromBody] UpdateEventRequest request)
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

            return Ok(updatedEvent);
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