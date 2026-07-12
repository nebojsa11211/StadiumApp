using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin,Staff")]
public class TicketSalesController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ISeatMappingService _seatMappingService;
    private readonly ITicketIngestionService _ticketIngestionService;
    private readonly ILogger<TicketSalesController> _logger;

    public TicketSalesController(ApplicationDbContext context, ISeatMappingService seatMappingService, ITicketIngestionService ticketIngestionService, ILogger<TicketSalesController> logger)
    {
        _context = context;
        _seatMappingService = seatMappingService;
        _ticketIngestionService = ticketIngestionService;
        _logger = logger;
    }

    /// <summary>
    /// Get all sold tickets for a specific event
    /// </summary>
    [HttpGet("event/{eventId}/sold-tickets")]
    public async Task<ActionResult<List<TicketSalesDto>>> GetSoldTicketsForEvent(int eventId)
    {
        try
        {
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .Include(t => t.Seat)
                .ThenInclude(s => s.Section)
                .Where(t => t.EventId == eventId && t.Status == "Active")
                .OrderBy(t => t.PurchaseDate)
                .ToListAsync();

            var ticketDtos = tickets.Select(t => new TicketSalesDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                CustomerName = t.CustomerName ?? "N/A",
                CustomerEmail = t.CustomerEmail ?? "N/A",
                CustomerPhone = t.CustomerPhone ?? "N/A",
                SeatCode = t.Seat?.SeatCode ?? "N/A",
                SectionName = t.Seat?.Section?.SectionName ?? t.Section ?? "N/A",
                RowNumber = t.Seat?.RowNumber ?? int.Parse(t.Row ?? "0"),
                SeatNumber = t.Seat?.SeatNumber ?? int.Parse(t.SeatNumber ?? "0"),
                Price = t.Price,
                PurchaseDate = t.PurchaseDate,
                IsUsed = t.IsUsed,
                UsedAt = t.UsedAt,
                QRCodeToken = t.QRCodeToken
            }).ToList();

            return Ok(ticketDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sold tickets for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get sold tickets for a specific sector
    /// </summary>
    [HttpGet("sector/{sectorId}/event/{eventId}/sold-tickets")]
    public async Task<ActionResult<List<TicketSalesDto>>> GetSoldTicketsForSector(int sectorId, int eventId)
    {
        try
        {
            var tickets = await _seatMappingService.GetSoldTicketsForSectorAsync(sectorId, eventId);

            var ticketDtos = tickets.Select(t => new TicketSalesDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                CustomerName = t.CustomerName ?? "N/A",
                CustomerEmail = t.CustomerEmail ?? "N/A",
                CustomerPhone = t.CustomerPhone ?? "N/A",
                SeatCode = t.Seat?.SeatCode ?? "N/A",
                SectionName = t.Seat?.Section?.SectionName ?? t.Section ?? "N/A",
                RowNumber = t.Seat?.RowNumber ?? int.Parse(t.Row ?? "0"),
                SeatNumber = t.Seat?.SeatNumber ?? int.Parse(t.SeatNumber ?? "0"),
                Price = t.Price,
                PurchaseDate = t.PurchaseDate,
                IsUsed = t.IsUsed,
                UsedAt = t.UsedAt,
                QRCodeToken = t.QRCodeToken
            }).ToList();

            return Ok(ticketDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sold tickets for sector {SectorId} and event {EventId}", sectorId, eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get seat status for stadium overview
    /// </summary>
    [HttpGet("event/{eventId}/seat-status")]
    public async Task<ActionResult<SeatStatusResponseDto>> GetSeatStatusForEvent(int eventId)
    {
        try
        {
            var soldStadiumSeats = await _seatMappingService.GetSoldStadiumSeatsForEventAsync(eventId);
            
            var seatStatusMap = soldStadiumSeats.ToDictionary(
                seat => $"{seat.SectorId}-{seat.RowNumber}-{seat.SeatNumber}",
                seat => new SeatStatusDto
                {
                    StadiumSeatId = seat.Id,
                    SectorId = seat.SectorId ?? 0,
                    RowNumber = seat.RowNumber,
                    SeatNumber = seat.SeatNumber,
                    Status = "Sold",
                    UniqueCode = seat.UniqueCode
                });

            return Ok(new SeatStatusResponseDto
            {
                EventId = eventId,
                SoldSeats = seatStatusMap
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving seat status for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Simulate ticket sales for testing
    /// </summary>
    [HttpPost("event/{eventId}/simulate-sales")]
    public async Task<ActionResult> SimulateTicketSales(int eventId, [FromBody] TicketSalesSimulationRequest request)
    {
        try
        {
            // Delegate to the ingestion service so simulated seats are allocated from the drawing-tool
            // overlays (the real stadium). This keeps each ticket's Section a genuine overlay SectorCode,
            // which the admin ticket-detail blueprint locator can resolve instead of "location not available".
            var result = await _ticketIngestionService.SimulateTicketSalesAsync(eventId, request.NumberOfTickets, request.BasePrice);

            if (!result.Accepted)
            {
                return result.Message.Contains("not found", StringComparison.OrdinalIgnoreCase)
                    ? NotFound(result.Message)
                    : BadRequest(new { message = result.Message });
            }

            return Ok(new
            {
                message = result.Message,
                ticketsCreated = result.TicketsCreated
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error simulating ticket sales for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }
}

// DTOs for Ticket Sales
public class TicketSalesDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerPhone { get; set; } = string.Empty;
    public string SeatCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
    public bool IsUsed { get; set; }
    public DateTime? UsedAt { get; set; }
    public string QRCodeToken { get; set; } = string.Empty;
}

public class SeatStatusDto
{
    public int StadiumSeatId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string Status { get; set; } = "Available"; // Available, Sold, Reserved
    public string UniqueCode { get; set; } = string.Empty;
}

public class SeatStatusResponseDto
{
    public int EventId { get; set; }
    public Dictionary<string, SeatStatusDto> SoldSeats { get; set; } = new();
}

public class TicketSalesSimulationRequest
{
    public int NumberOfTickets { get; set; } = 10;
    public decimal BasePrice { get; set; } = 50.00m;
}