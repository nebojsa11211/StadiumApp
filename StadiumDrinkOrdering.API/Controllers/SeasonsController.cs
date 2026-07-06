using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Admin management of seasons (e.g. "2026/2027"). Events link to a season and season tickets
/// are sold against one. External (simulator) seasons are ingested via the integration webhook.
/// </summary>
[ApiController]
[Route("seasons")]
public class SeasonsController : ControllerBase
{
    private readonly ISeasonService _seasons;
    private readonly ILogger<SeasonsController> _logger;

    public SeasonsController(ISeasonService seasons, ILogger<SeasonsController> logger)
    {
        _seasons = seasons;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<List<SeasonDto>>> GetSeasons(CancellationToken ct)
        => Ok(await _seasons.GetSeasonsAsync(ct));

    /// <summary>
    /// Public: the current season plus its next upcoming/live fixture, for the mobile landing.
    /// Anonymous by design (shown pre-login on <c>/welcome</c>).
    /// </summary>
    [HttpGet("current")]
    public async Task<ActionResult<CurrentSeasonDto>> GetCurrentSeason(CancellationToken ct)
    {
        var current = await _seasons.GetCurrentSeasonAsync(ct);
        return current == null ? NotFound() : Ok(current);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SeasonDto>> GetSeason(int id, CancellationToken ct)
    {
        var season = await _seasons.GetSeasonAsync(id, ct);
        return season == null ? NotFound() : Ok(season);
    }

    /// <summary>The active season passes for a season, each with its fixed seat (admin drill-down).</summary>
    [HttpGet("{id:int}/tickets")]
    public async Task<ActionResult<List<SeasonTicketDto>>> GetSeasonTickets(int id, CancellationToken ct)
    {
        var season = await _seasons.GetSeasonAsync(id, ct);
        if (season == null)
            return NotFound();
        return Ok(await _seasons.GetSeasonTicketsAsync(id, ct));
    }

    /// <summary>The scannable QR for a single season pass (deep link to the mobile ordering app).</summary>
    [HttpGet("tickets/{seasonTicketId:int}/qr")]
    public async Task<ActionResult<SeasonPassQrDto>> GetPassQr(int seasonTicketId, CancellationToken ct)
    {
        var qr = await _seasons.GetPassQrAsync(seasonTicketId, ct);
        return qr == null ? NotFound() : Ok(qr);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SeasonDto>> CreateSeason([FromBody] CreateSeasonDto request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (request.EndDate <= request.StartDate)
            return BadRequest(new { message = "End date must be after the start date." });

        var created = await _seasons.CreateSeasonAsync(request, ct);
        return CreatedAtAction(nameof(GetSeason), new { id = created.Id }, created);
    }

    // PUT + POST alias so the Admin HTTP helper (POST/GET/DELETE only) can call it.
    [HttpPut("{id:int}")]
    [HttpPost("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SeasonDto>> UpdateSeason(int id, [FromBody] UpdateSeasonDto request, CancellationToken ct)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        if (request.StartDate.HasValue && request.EndDate.HasValue && request.EndDate <= request.StartDate)
            return BadRequest(new { message = "End date must be after the start date." });

        var updated = await _seasons.UpdateSeasonAsync(id, request, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPost("{id:int}/set-current")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SeasonDto>> SetCurrent(int id, CancellationToken ct)
    {
        var updated = await _seasons.SetCurrentAsync(id, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteSeason(int id, CancellationToken ct)
    {
        var (found, deleted) = await _seasons.DeleteSeasonAsync(id, ct);
        if (!found)
            return NotFound();
        if (!deleted)
            return Conflict(new { message = "This season has season tickets. Refund them before deleting the season." });
        return NoContent();
    }
}
