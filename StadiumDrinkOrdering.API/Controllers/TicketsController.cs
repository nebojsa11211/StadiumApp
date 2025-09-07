using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TicketsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TicketsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    // [Authorize(Roles = "Admin")] // Temporarily disabled for testing
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetAllTickets([FromQuery] int? eventId = null, [FromQuery] bool? isActive = null)
    {
        var query = _context.Tickets.AsQueryable();

        if (eventId.HasValue)
        {
            query = query.Where(t => t.EventId == eventId.Value);
        }

        if (isActive.HasValue)
        {
            query = query.Where(t => t.IsActive == isActive.Value);
        }

        var tickets = await query
            .OrderByDescending(t => t.PurchaseDate)
            .Select(t => new TicketDto
            {
                Id = t.Id,
                TicketNumber = t.TicketNumber,
                SeatNumber = t.SeatNumber ?? string.Empty,
                Section = t.Section,
                Row = t.Row,
                EventName = t.EventName,
                EventDate = t.EventDate,
                IsActive = t.IsActive,
                EventId = t.EventId,
                OrderId = t.Orders.Any() ? t.Orders.First().Id : null,
                PurchaseDate = t.PurchaseDate,
                CustomerEmail = t.CustomerEmail,
                CustomerName = t.CustomerName,
                Price = t.Price
            })
            .ToListAsync();

        return Ok(tickets);
    }

    [HttpPost("validate")]
    public async Task<ActionResult<TicketValidationResultDto>> ValidateTicket([FromBody] ValidateTicketDto validateDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.TicketNumber == validateDto.TicketNumber && t.IsActive);

        if (ticket == null)
        {
            return Ok(new TicketValidationResultDto
            {
                IsValid = false,
                ErrorMessage = "Invalid ticket number or ticket is not active"
            });
        }

        var ticketDto = new TicketDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            SeatNumber = ticket.SeatNumber ?? string.Empty,
            Section = ticket.Section,
            Row = ticket.Row,
            EventName = ticket.EventName,
            EventDate = ticket.EventDate,
            IsActive = ticket.IsActive
        };

        return Ok(new TicketValidationResultDto
        {
            IsValid = true,
            Ticket = ticketDto
        });
    }

    [HttpGet("{ticketNumber}")]
    public async Task<ActionResult<TicketDto>> GetTicketByNumber(string ticketNumber)
    {
        var ticket = await _context.Tickets
            .FirstOrDefaultAsync(t => t.TicketNumber == ticketNumber);

        if (ticket == null)
        {
            return NotFound();
        }

        var ticketDto = new TicketDto
        {
            Id = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            SeatNumber = ticket.SeatNumber ?? string.Empty,
            Section = ticket.Section,
            Row = ticket.Row,
            EventName = ticket.EventName,
            EventDate = ticket.EventDate,
            IsActive = ticket.IsActive
        };

        return Ok(ticketDto);
    }
}


