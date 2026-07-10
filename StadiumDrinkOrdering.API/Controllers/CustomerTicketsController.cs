using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// A logged-in customer's own tickets. Everything here is scoped to the caller's email claim —
/// a customer can only ever list or drill into tickets that were purchased under their address.
/// Backs the Customer app's "My Tickets" screen (list + spending detail + printable PDF card).
/// </summary>
[ApiController]
[Route("customer/tickets")]
[Authorize]
public class CustomerTicketsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly IQRCodeService _qrCodeService;
    private readonly ITicketCardPdfService _ticketCardPdfService;
    private readonly ITicketDetailService _ticketDetailService;

    public CustomerTicketsController(
        ApplicationDbContext context,
        IQRCodeService qrCodeService,
        ITicketCardPdfService ticketCardPdfService,
        ITicketDetailService ticketDetailService)
    {
        _context = context;
        _qrCodeService = qrCodeService;
        _ticketCardPdfService = ticketCardPdfService;
        _ticketDetailService = ticketDetailService;
    }

    private string? CurrentEmail =>
        User.FindFirst(ClaimTypes.Email)?.Value ?? User.FindFirst("email")?.Value;

    /// <summary>Lists the current customer's tickets (matched by their email), newest purchase first.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TicketDto>>> GetMyTickets()
    {
        var email = CurrentEmail;
        if (string.IsNullOrWhiteSpace(email))
        {
            return Ok(Array.Empty<TicketDto>());
        }

        var tickets = await _context.Tickets
            .Where(t => t.CustomerEmail != null && t.CustomerEmail.ToLower() == email.ToLower())
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
                Price = t.Price,
                Kind = t.Kind
            })
            .ToListAsync();

        return Ok(tickets);
    }

    /// <summary>Full spending drill-down for one of the current customer's tickets.</summary>
    [HttpGet("{id:int}/details")]
    public async Task<ActionResult<TicketDetailDto>> GetMyTicketDetails(int id)
    {
        if (!await OwnsTicketAsync(id))
        {
            return NotFound();
        }

        var dto = await _ticketDetailService.BuildDetailAsync(id);
        return dto == null ? NotFound() : Ok(dto);
    }

    /// <summary>Printable PDF ticket card (with QR) for one of the current customer's tickets.</summary>
    [HttpGet("{id:int}/card.pdf")]
    public async Task<IActionResult> GetMyTicketCardPdf(int id)
    {
        var ticket = await _context.Tickets.FirstOrDefaultAsync(t => t.Id == id);
        if (ticket == null || !IsOwner(ticket.CustomerEmail))
        {
            return NotFound();
        }

        var qrDataUri = await _qrCodeService.GetQrImageDataUriAsync(ticket);
        var qrPng = DataUriToBytes(qrDataUri);

        var pdf = _ticketCardPdfService.GenerateTicketCard(ticket, qrPng);
        return File(pdf, "application/pdf", $"ticket-{ticket.TicketNumber}.pdf");
    }

    private async Task<bool> OwnsTicketAsync(int ticketId)
    {
        var email = CurrentEmail;
        if (string.IsNullOrWhiteSpace(email)) return false;
        return await _context.Tickets.AnyAsync(t =>
            t.Id == ticketId &&
            t.CustomerEmail != null &&
            t.CustomerEmail.ToLower() == email.ToLower());
    }

    private bool IsOwner(string? ticketEmail)
    {
        var email = CurrentEmail;
        return !string.IsNullOrWhiteSpace(email)
            && !string.IsNullOrWhiteSpace(ticketEmail)
            && string.Equals(email, ticketEmail, StringComparison.OrdinalIgnoreCase);
    }

    private static byte[] DataUriToBytes(string dataUri)
    {
        var commaIndex = dataUri.IndexOf(',');
        var base64 = commaIndex >= 0 ? dataUri[(commaIndex + 1)..] : dataUri;
        return Convert.FromBase64String(base64);
    }
}
