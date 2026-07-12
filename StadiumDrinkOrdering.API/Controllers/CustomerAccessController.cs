using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Unified entry resolver for a scanned QR token — the single target of the mobile <c>/t/{token}</c>
/// deep link. Anonymous. Accepts either:
///  • a single-event ticket QR (or an already-materialized season derived ticket) — validated directly; or
///  • a season <see cref="SeasonTicket.PassToken"/> — resolved to the holder's derived ticket for the
///    match that is live now, then validated.
/// In both cases it returns a <see cref="ValidateTicketResponse"/> (session token + seat/event),
/// identical in shape to <c>POST /TicketAuth/validate</c>.
/// </summary>
[ApiController]
[Route("customer/access")]
[AllowAnonymous]
public class CustomerAccessController : ControllerBase
{
    private readonly ITicketAuthService _ticketAuth;
    private readonly ApplicationDbContext _db;
    private readonly ILogger<CustomerAccessController> _logger;

    public CustomerAccessController(ITicketAuthService ticketAuth, ApplicationDbContext db, ILogger<CustomerAccessController> logger)
    {
        _ticketAuth = ticketAuth;
        _db = db;
        _logger = logger;
    }

    [HttpPost("resolve")]
    public async Task<ActionResult<ValidateTicketResponse>> Resolve([FromBody] ValidateTicketRequest request)
    {
        if (!ModelState.IsValid || string.IsNullOrWhiteSpace(request.QRCodeToken))
            return BadRequest(new ValidateTicketResponse { Success = false, ErrorMessage = "Nedostaje kôd ulaznice." });

        var token = request.QRCodeToken.Trim();

        // 1) A direct ticket QR (single-event ticket, or an already-materialized season derived ticket).
        if (await _db.Tickets.AnyAsync(t => t.QRCodeToken == token))
            return Map(await _ticketAuth.AuthenticateWithTicketAsync(token));

        // 2) A human-readable ticket number typed in manually (e.g. "SEA-6-E17" or "TK001") — the printed
        //    QR carries a GUID, but someone who can't scan reads the visible number off the ticket. Resolve
        //    it to that ticket's QR token. Case-insensitive; ignore tickets without a token.
        var tokenByNumber = await _db.Tickets
            .Where(t => t.TicketNumber.ToLower() == token.ToLower() && t.QRCodeToken != "")
            .Select(t => t.QRCodeToken)
            .FirstOrDefaultAsync();
        if (!string.IsNullOrEmpty(tokenByNumber))
            return Map(await _ticketAuth.AuthenticateWithTicketAsync(tokenByNumber));

        // 3) A season pass token: resolve to the holder's derived ticket for the live match.
        var pass = await _db.SeasonTickets
            .Include(st => st.Season).ThenInclude(s => s.Events)
            .Include(st => st.DerivedTickets)
            .FirstOrDefaultAsync(st => st.PassToken == token && st.Status == TicketStatuses.Active);

        if (pass is null)
            return BadRequest(new ValidateTicketResponse { Success = false, ErrorMessage = "Ulaznica nije prepoznata." });

        var live = (pass.Season?.Events ?? new List<Event>())
            .FirstOrDefault(e => e.AreDrinkSalesOpenAt(DateTime.UtcNow));

        if (live is null)
            return BadRequest(new ValidateTicketResponse
            {
                Success = false,
                ErrorMessage = "Trenutno nema utakmice uživo. Naručivanje se otvara kad utakmica počne."
            });

        var derived = pass.DerivedTickets.FirstOrDefault(t => t.EventId == live.Id && t.IsActive);
        if (derived is null || string.IsNullOrEmpty(derived.QRCodeToken))
            return BadRequest(new ValidateTicketResponse
            {
                Success = false,
                ErrorMessage = "Naručivanje trenutno nije dostupno za ovu utakmicu."
            });

        return Map(await _ticketAuth.AuthenticateWithTicketAsync(derived.QRCodeToken));
    }

    private ActionResult<ValidateTicketResponse> Map(TicketAuthResult result)
    {
        if (!result.IsSuccess)
            return BadRequest(new ValidateTicketResponse { Success = false, ErrorMessage = result.ErrorMessage });

        return Ok(new ValidateTicketResponse
        {
            Success = true,
            SessionToken = result.TicketSession?.SessionId,
            TicketSession = result.TicketSession,
            Ticket = result.Ticket,
            SeatInfo = new SeatInfoDto
            {
                SeatNumber = result.TicketSession?.SeatNumber ?? string.Empty,
                Section = result.TicketSession?.Section ?? string.Empty,
                Row = result.TicketSession?.Row ?? string.Empty
            }
        });
    }
}
