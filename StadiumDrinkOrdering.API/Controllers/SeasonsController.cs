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

        // Turning on "current" is subject to the same rule as the set-current endpoint: a season
        // whose (effective) window has already ended can't become the current season.
        if (request.IsCurrent == true)
        {
            var existing = await _seasons.GetSeasonAsync(id, ct);
            if (existing == null)
                return NotFound();
            var effectiveEnd = request.EndDate ?? existing.EndDate;
            if (IsFinished(effectiveEnd))
                return BadRequest(new { message = FinishedSeasonMessage });
        }

        var updated = await _seasons.UpdateSeasonAsync(id, request, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    [HttpPost("{id:int}/set-current")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SeasonDto>> SetCurrent(int id, CancellationToken ct)
    {
        var season = await _seasons.GetSeasonAsync(id, ct);
        if (season == null)
            return NotFound();
        if (IsFinished(season.EndDate))
            return BadRequest(new { message = FinishedSeasonMessage });

        var updated = await _seasons.SetCurrentAsync(id, ct);
        return updated == null ? NotFound() : Ok(updated);
    }

    /// <summary>
    /// A season whose window has already ended cannot be the "current" (default) season — that flag
    /// marks the live/upcoming season the UIs default to. Compared at whole-day granularity in UTC so
    /// the season stays eligible through the entirety of its final day.
    /// </summary>
    private static bool IsFinished(DateTime endDateUtc) => endDateUtc.Date < DateTime.UtcNow.Date;

    private const string FinishedSeasonMessage =
        "This season has already finished, so it can't be set as the current season.";

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

    /// <summary>
    /// What a full purge of this season would destroy. Read-only; call before showing the
    /// confirmation dialog so the admin sees the exact damage.
    /// </summary>
    [HttpGet("{id:int}/purge-preview")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SeasonPurgePreviewDto>> GetPurgePreview(int id, CancellationToken ct)
    {
        var preview = await _seasons.GetPurgePreviewAsync(id, ct);
        return preview == null ? NotFound() : Ok(preview);
    }

    /// <summary>
    /// Permanently deletes the season and EVERYTHING attached to it: its events, match tickets,
    /// season passes, orders, order items and payments — plus any anonymous ticket wallets, whose
    /// remaining balance is destroyed rather than refunded. Irreversible.
    /// The caller must echo the season's exact name in <c>confirmName</c>, so a mis-aimed request
    /// (wrong id, replayed call) cannot wipe the wrong season.
    /// </summary>
    [HttpPost("{id:int}/purge")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SeasonPurgeResultDto>> PurgeSeason(
        int id, [FromBody] PurgeSeasonRequest request, CancellationToken ct)
    {
        var season = await _seasons.GetSeasonAsync(id, ct);
        if (season == null)
            return NotFound();

        if (!string.Equals(request?.ConfirmName?.Trim(), season.Name, StringComparison.Ordinal))
            return BadRequest(new { message = "The confirmation name does not match this season's name. Nothing was deleted." });

        try
        {
            var result = await _seasons.PurgeSeasonAsync(id, ct);
            return result == null ? NotFound() : Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Purge of season {SeasonId} failed and was rolled back", id);
            return StatusCode(500, new { message = "Failed to purge the season. No changes were made.", error = ex.Message });
        }
    }

    /// <summary>Body of a purge request — the typed confirmation of the season's name.</summary>
    public class PurgeSeasonRequest
    {
        public string? ConfirmName { get; set; }
    }
}
