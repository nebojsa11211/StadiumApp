using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("customer/ticketing")]
public class CustomerTicketingController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IEventService _eventService;
    private readonly ISeatMappingService _seatMappingService;
    private readonly IOverlaySeatService _overlaySeats;
    private readonly ILogger<CustomerTicketingController> _logger;

    public CustomerTicketingController(
        ApplicationDbContext context,
        IEventService eventService,
        ISeatMappingService seatMappingService,
        IOverlaySeatService overlaySeats,
        ILogger<CustomerTicketingController> logger)
    {
        _context = context;
        _eventService = eventService;
        _seatMappingService = seatMappingService;
        _overlaySeats = overlaySeats;
        _logger = logger;
    }

    /// <summary>
    /// Get all active events available for customer booking
    /// </summary>
    [HttpGet("events")]
    [ResponseCache(Duration = 300)] // Cache for 5 minutes
    public async Task<ActionResult<List<CustomerEventDto>>> GetAvailableEvents([FromQuery] CustomerEventFilterDto filter)
    {
        try
        {
            var query = _context.Events
                .Where(e => e.IsActive)
                .AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(filter.EventType))
            {
                query = query.Where(e => e.EventType.Contains(filter.EventType));
            }

            if (filter.DateFrom.HasValue)
            {
                query = query.Where(e => e.EventDate >= filter.DateFrom.Value);
            }

            if (filter.DateTo.HasValue)
            {
                query = query.Where(e => e.EventDate <= filter.DateTo.Value);
            }

            if (filter.MinPrice.HasValue)
            {
                query = query.Where(e => e.BaseTicketPrice >= filter.MinPrice.Value);
            }

            if (filter.MaxPrice.HasValue)
            {
                query = query.Where(e => e.BaseTicketPrice <= filter.MaxPrice.Value);
            }

            var events = await query
                .OrderBy(e => e.EventDate)
                .Take(50) // Limit results
                .ToListAsync();

            var eventDtos = new List<CustomerEventDto>();
            
            foreach (var evt in events)
            {
                // Get ticket count and availability. "Sold" = any non-cancelled ticket (incl. season
                // passes' derived tickets), funnelled through TicketStatuses.CountsAsSold.
                var totalTickets = await _context.Tickets.CountAsync(t => t.EventId == evt.Id && t.Status != TicketStatuses.Cancelled);
                var availableSeats = evt.TotalSeats - totalTickets;
                
                eventDtos.Add(new CustomerEventDto
                {
                    Id = evt.Id,
                    EventName = evt.EventName,
                    EventType = evt.EventType,
                    EventDate = evt.EventDate,
                    Description = evt.Description,
                    BaseTicketPrice = evt.BaseTicketPrice ?? 50.00m,
                    TotalSeats = evt.TotalSeats,
                    AvailableSeats = Math.Max(0, availableSeats),
                    SoldTickets = totalTickets,
                    VenueInfo = "Stadium Arena" // Could be made dynamic
                });
            }

            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving available events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get detailed information about a specific event
    /// </summary>
    [HttpGet("events/{eventId}")]
    [ResponseCache(Duration = 120)] // Cache for 2 minutes
    public async Task<ActionResult<CustomerEventDetailsDto>> GetEventDetails(int eventId)
    {
        try
        {
            var evt = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.IsActive);

            if (evt == null)
            {
                return NotFound("Event not found or not available");
            }

            // Get availability by section
            var sectionAvailability = await GetSectionAvailabilityForEvent(eventId);

            var eventDetails = new CustomerEventDetailsDto
            {
                Id = evt.Id,
                EventName = evt.EventName,
                EventType = evt.EventType,
                EventDate = evt.EventDate,
                Description = evt.Description,
                BaseTicketPrice = evt.BaseTicketPrice ?? 50.00m,
                TotalSeats = evt.TotalSeats,
                VenueInfo = "Stadium Arena",
                SectionAvailability = sectionAvailability,
                PricingTiers = await GetPricingTiers(eventId)
            };

            return Ok(eventDetails);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event details for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get seat availability for a specific section in an event. <paramref name="sectionId"/> is a
    /// real-stadium overlay sector id (StadiumSectorOverlay.Id). Seats occupied by any non-cancelled
    /// ticket — including a season pass's derived tickets — are excluded, so season-held seats can't
    /// be picked.
    /// </summary>
    [HttpGet("events/{eventId}/sections/{sectionId}/availability")]
    public async Task<ActionResult<SectionAvailabilityDto>> GetSectionAvailability(int eventId, int sectionId)
    {
        try
        {
            var evt = await _context.Events
                .FirstOrDefaultAsync(e => e.Id == eventId && e.IsActive);

            if (evt == null)
            {
                return NotFound("Event not found");
            }

            var overlay = await _overlaySeats.GetOverlayAsync(sectionId);
            if (overlay == null)
            {
                return NotFound("Section not found");
            }

            var basePrice = evt.BaseTicketPrice ?? 50.00m;
            var seatPrice = CalculateOverlaySeatPrice(basePrice, overlay.Type);

            var available = await _overlaySeats.GetAvailableSeatsAsync(eventId, sectionId);
            var availableSeats = available
                .Select(s => new CustomerSeatDto
                {
                    SectorId = sectionId,
                    RowNumber = s.RowNumber,
                    SeatNumber = s.SeatNumber,
                    Price = seatPrice,
                    SeatCode = s.SeatCode
                })
                .ToList();

            return Ok(new SectionAvailabilityDto
            {
                EventId = eventId,
                SectionId = sectionId,
                SectionName = overlay.Name,
                TribuneName = overlay.SectorCode,
                RingName = overlay.Type,
                TotalSeats = overlay.TotalSeats,
                AvailableSeats = availableSeats
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving section availability for overlay {SectionId}, event {EventId}", sectionId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    private async Task<Dictionary<string, SectionAvailabilityInfo>> GetSectionAvailabilityForEvent(int eventId)
    {
        // Real stadium = the drawing-tool overlay sectors. Sold counts include season-pass seats.
        var summaries = await _overlaySeats.GetSectionSummariesAsync(eventId);

        var result = new Dictionary<string, SectionAvailabilityInfo>();
        foreach (var summary in summaries.Values)
        {
            result[summary.Code] = new SectionAvailabilityInfo
            {
                SectionId = summary.OverlayId,
                SectionName = summary.Name,
                TotalSeats = summary.TotalSeats,
                AvailableSeats = summary.AvailableSeats,
                BasePrice = CalculateOverlaySeatPrice(50.00m, summary.Type)
            };
        }

        return result;
    }

    private static decimal CalculateOverlaySeatPrice(decimal basePrice, string? sectorType) => (sectorType?.ToLowerInvariant()) switch
    {
        "vip" => basePrice * 2.0m,
        "premium" => basePrice * 1.5m,
        "family" => basePrice * 1.2m,
        _ => basePrice
    };

    private Task<List<PricingTierDto>> GetPricingTiers(int eventId)
    {
        // This would typically be configurable per event
        return Task.FromResult(new List<PricingTierDto>
        {
            new PricingTierDto { Name = "VIP", BasePrice = 150.00m, Description = "Premium seating with exclusive access" },
            new PricingTierDto { Name = "Premium", BasePrice = 100.00m, Description = "Great views and comfort" },
            new PricingTierDto { Name = "Standard", BasePrice = 75.00m, Description = "Good value seating" },
            new PricingTierDto { Name = "Economy", BasePrice = 50.00m, Description = "Budget-friendly options" }
        });
    }

    private decimal CalculateSeatPrice(decimal basePrice, string tribuneCode)
    {
        // Price multipliers based on tribune location
        return tribuneCode switch
        {
            "E" => basePrice * 2.0m, // VIP East
            "W" => basePrice * 2.0m, // VIP West  
            "N" => basePrice * 1.5m, // North premium
            "S" => basePrice * 1.2m, // South standard
            _ => basePrice
        };
    }
}

// Customer-specific DTOs
public class CustomerEventDto
{
    public int Id { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal BaseTicketPrice { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public int SoldTickets { get; set; }
    public string VenueInfo { get; set; } = string.Empty;
}

public class CustomerEventDetailsDto : CustomerEventDto
{
    public Dictionary<string, SectionAvailabilityInfo> SectionAvailability { get; set; } = new();
    public List<PricingTierDto> PricingTiers { get; set; } = new();
}

public class CustomerEventFilterDto
{
    public string? EventType { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class SectionAvailabilityInfo
{
    public int SectionId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal BasePrice { get; set; }
}

public class SectionAvailabilityDto
{
    public int EventId { get; set; }
    public int SectionId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public string TribuneName { get; set; } = string.Empty;
    public string RingName { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public List<CustomerSeatDto> AvailableSeats { get; set; } = new();
}

public class CustomerSeatDto
{
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public string SeatCode { get; set; } = string.Empty;
}

public class PricingTierDto
{
    public string Name { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Description { get; set; } = string.Empty;
}