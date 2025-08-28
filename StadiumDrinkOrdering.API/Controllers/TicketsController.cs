using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;

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
            SeatNumber = ticket.SeatNumber,
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
            SeatNumber = ticket.SeatNumber,
            Section = ticket.Section,
            Row = ticket.Row,
            EventName = ticket.EventName,
            EventDate = ticket.EventDate,
            IsActive = ticket.IsActive
        };

        return Ok(ticketDto);
    }
}


