using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.API.Authorization.Services;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Season-member customer endpoints. Authenticated (the member has an account linked to their pass by
/// email). Powers the mobile <c>/home</c> landing.
/// </summary>
[ApiController]
[Route("customer/season")]
[Authorize(Policy = AuthorizationPolicies.RequireAuthenticatedUser)]
public class CustomerSeasonController : ControllerBase
{
    private readonly ApplicationDbContext _db;
    private readonly IStadiumAuthorizationService _auth;
    private readonly ILogger<CustomerSeasonController> _logger;

    public CustomerSeasonController(ApplicationDbContext db, IStadiumAuthorizationService auth, ILogger<CustomerSeasonController> logger)
    {
        _db = db;
        _auth = auth;
        _logger = logger;
    }

    /// <summary>Returns the signed-in member's pass, their live match (if any), and upcoming fixtures.</summary>
    [HttpGet("my-tickets")]
    public async Task<ActionResult<SeasonHomeDto>> GetMyTickets()
    {
        var userId = _auth.GetCurrentUserId(User);
        if (userId is null or 0)
            return Unauthorized();

        var user = await _db.Users.FirstOrDefaultAsync(u => u.Id == userId);
        var email = user?.Email;

        // Prefer the FK link; fall back to email match for a pass ingested before the account existed.
        var pass = await _db.SeasonTickets
            .Include(st => st.Season).ThenInclude(s => s.Events)
            .Include(st => st.Seat).ThenInclude(seat => seat.Section)
            .Include(st => st.DerivedTickets)
            .Where(st => st.Status == TicketStatuses.Active &&
                         (st.UserId == userId || (st.UserId == null && email != null && st.HolderEmail == email)))
            .OrderByDescending(st => st.Season.IsCurrent)
            .ThenByDescending(st => st.Id)
            .FirstOrDefaultAsync();

        if (pass is null)
            return Ok(new SeasonHomeDto { HasPass = false, HolderName = user?.Username ?? string.Empty });

        var now = DateTime.UtcNow;
        var events = pass.Season?.Events ?? new List<Event>();

        var live = events.FirstOrDefault(e => EventLifecycle.CanOrderDrinks(e.Status));
        SeasonLiveEventDto? liveDto = null;
        if (live is not null)
        {
            var derived = pass.DerivedTickets.FirstOrDefault(t => t.EventId == live.Id && t.IsActive);
            liveDto = new SeasonLiveEventDto
            {
                EventId = live.Id,
                EventName = live.EventName,
                EventDate = live.EventDate,
                SessionQrToken = derived?.QRCodeToken
            };
        }

        var upcoming = events
            .Where(e => e.EventDate > now
                        && e.Status != EventStatus.Cancelled
                        && e.Status != EventStatus.Completed
                        && !EventLifecycle.CanOrderDrinks(e.Status))
            .OrderBy(e => e.EventDate)
            .Take(5)
            .Select(e => new SeasonFixtureDto
            {
                EventId = e.Id,
                EventName = e.EventName,
                EventDate = e.EventDate,
                Status = e.Status.ToString()
            })
            .ToList();

        var seat = pass.Seat;
        var seatLabel = seat?.Section != null
            ? $"{seat.Section.SectionName} · Red {seat.RowNumber} · Sjed. {seat.SeatNumber}"
            : string.Empty;

        return Ok(new SeasonHomeDto
        {
            HasPass = true,
            HolderName = pass.HolderName ?? user?.Username ?? string.Empty,
            SeasonName = pass.Season?.Name ?? string.Empty,
            SeatLabel = seatLabel,
            PassNumber = pass.SeasonTicketNumber,
            LiveEvent = liveDto,
            Upcoming = upcoming
        });
    }
}
