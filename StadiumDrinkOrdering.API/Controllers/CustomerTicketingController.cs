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
    private readonly ILogger<CustomerTicketingController> _logger;

    public CustomerTicketingController(
        ApplicationDbContext context, 
        IEventService eventService,
        ISeatMappingService seatMappingService,
        ILogger<CustomerTicketingController> logger)
    {
        _context = context;
        _eventService = eventService;
        _seatMappingService = seatMappingService;
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
                // Get ticket count and availability
                var totalTickets = await _context.Tickets.CountAsync(t => t.EventId == evt.Id);
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
    /// Get seat availability for a specific section in an event
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

            // Get section details
            var section = await _context.Sectors
                .Include(s => s.Ring)
                .ThenInclude(r => r.Tribune)
                .FirstOrDefaultAsync(s => s.Id == sectionId);

            if (section == null)
            {
                return NotFound("Section not found");
            }

            // OPTIMIZED: Use batch operation for sold tickets 
            var soldTicketsDict = await _seatMappingService.GetSoldTicketsForMultipleSectorsAsync(new List<int> { sectionId }, eventId);
            var soldTickets = soldTicketsDict.ContainsKey(sectionId) ? soldTicketsDict[sectionId] : new List<Ticket>();

            // Get sold seat positions more efficiently
            var soldSeatPositions = new HashSet<(int row, int seat)>();
            foreach (var ticket in soldTickets)
            {
                // Parse seat code to get position
                var parts = ticket.Seat.SeatCode.Split('-');
                if (parts.Length >= 4)
                {
                    if (int.TryParse(parts[2].Substring(1), out int row) && int.TryParse(parts[3].Substring(1), out int seatNum))
                    {
                        soldSeatPositions.Add((row, seatNum));
                    }
                }
            }

            var availableSeats = new List<CustomerSeatDto>();
            
            // Generate available seats efficiently
            for (int row = section.StartRow; row < section.StartRow + section.TotalRows; row++)
            {
                for (int seat = section.StartSeat; seat < section.StartSeat + section.SeatsPerRow; seat++)
                {
                    if (!soldSeatPositions.Contains((row, seat)))
                    {
                        availableSeats.Add(new CustomerSeatDto
                        {
                            SectorId = sectionId,
                            RowNumber = row,
                            SeatNumber = seat,
                            Price = CalculateSeatPrice(evt.BaseTicketPrice ?? 50.00m, section.Ring.Tribune.Code),
                            SeatCode = $"{section.Ring.Tribune.Code}{section.Ring.Number}-{section.Code}-R{row}-S{seat}"
                        });
                    }
                }
            }

            return Ok(new SectionAvailabilityDto
            {
                EventId = eventId,
                SectionId = sectionId,
                SectionName = section.Name,
                TribuneName = section.Ring.Tribune.Name,
                RingName = section.Ring.Name,
                TotalSeats = section.TotalRows * section.SeatsPerRow,
                AvailableSeats = availableSeats
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving section availability for section {SectionId}, event {EventId}", sectionId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    private async Task<Dictionary<string, SectionAvailabilityInfo>> GetSectionAvailabilityForEvent(int eventId)
    {
        // OPTIMIZED: Use the new batch summary method instead of individual queries
        var availabilitySummary = await _seatMappingService.GetSectionAvailabilitySummaryAsync(eventId);
        
        var result = new Dictionary<string, SectionAvailabilityInfo>();
        
        foreach (var summary in availabilitySummary.Values)
        {
            var sectionKey = $"{summary.TribuneName.Substring(0,1)}-{summary.SectorName}";
            result[sectionKey] = new SectionAvailabilityInfo
            {
                SectionId = summary.SectorId,
                SectionName = summary.SectorName,
                TotalSeats = summary.TotalSeats,
                AvailableSeats = summary.AvailableSeats,
                BasePrice = CalculateSeatPrice(50.00m, summary.TribuneName.Substring(0,1)) // Extract tribune code from name
            };
        }

        return result;
    }

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