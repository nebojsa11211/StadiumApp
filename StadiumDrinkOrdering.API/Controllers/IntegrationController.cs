using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs.Integration;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Controllers;

/// <summary>
/// Inbound integration surface for the external ticketing system (the simulator during
/// development). Webhooks are authenticated with an HMAC-SHA256 signature over the raw
/// body (header <c>X-Signature</c>) rather than JWT, since the caller is a server, not a user.
/// </summary>
[ApiController]
[Route("api/integration/ticketing")]
[AllowAnonymous]
public class IntegrationController : ControllerBase
{
    private const string SignatureHeader = "X-Signature";

    private readonly ITicketIngestionService _ingestion;
    private readonly IEventService _eventService;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IntegrationController> _logger;

    private static readonly System.Text.Json.JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public IntegrationController(
        ITicketIngestionService ingestion,
        IEventService eventService,
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<IntegrationController> logger)
    {
        _ingestion = ingestion;
        _eventService = eventService;
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    /// <summary>Receives ticket-sales webhooks from the external ticketing system.</summary>
    [HttpPost("webhook")]
    public async Task<ActionResult<TicketingWebhookResult>> Webhook(CancellationToken ct)
    {
        // Read the raw body so the HMAC is computed over exactly what was signed.
        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync(ct);
        }

        if (!VerifySignature(body, Request.Headers[SignatureHeader].ToString()))
        {
            _logger.LogWarning("Rejected ticketing webhook: invalid or missing signature");
            return Unauthorized(new TicketingWebhookResult { Accepted = false, Message = "Invalid signature" });
        }

        TicketingWebhookEnvelope? envelope;
        try
        {
            envelope = System.Text.Json.JsonSerializer.Deserialize<TicketingWebhookEnvelope>(body, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Rejected ticketing webhook: malformed JSON");
            return BadRequest(new TicketingWebhookResult { Accepted = false, Message = "Malformed JSON" });
        }

        if (envelope == null)
            return BadRequest(new TicketingWebhookResult { Accepted = false, Message = "Empty payload" });

        var result = await _ingestion.ProcessWebhookAsync(envelope, ct);
        return result.Accepted ? Ok(result) : UnprocessableEntity(result);
    }

    /// <summary>
    /// Testing hook: places a randomised live drink order for the given event (signed like the
    /// webhook). Rejected unless the event is live. Lets the external system/simulator exercise
    /// the game-day drink-ordering flow and populate the Bar/Staff order queue.
    /// </summary>
    [HttpPost("events/{externalEventId}/simulate-order")]
    public async Task<ActionResult<SimulatedDrinkOrderResult>> SimulateOrder(string externalEventId, CancellationToken ct)
    {
        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync(ct);
        }

        if (!VerifySignature(body, Request.Headers[SignatureHeader].ToString()))
        {
            _logger.LogWarning("Rejected simulate-order: invalid or missing signature");
            return Unauthorized(new SimulatedDrinkOrderResult { Accepted = false, Message = "Invalid signature" });
        }

        var result = await _ingestion.SimulateDrinkOrderAsync(externalEventId, ct);
        return result.Accepted ? Ok(result) : UnprocessableEntity(result);
    }

    /// <summary>
    /// Testing hook: builds one complete fixture in a season — creates the event, sells it a crowd,
    /// and for a past date settles it like a played match (attendance mix, optional drink orders).
    /// Signed like the webhook. The simulator's season generator calls this once per fixture.
    /// </summary>
    [HttpPost("simulate-match")]
    public async Task<ActionResult<SimulateMatchResult>> SimulateMatch(
        [FromServices] IMatchSimulationService simulation, CancellationToken ct)
    {
        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync(ct);
        }

        if (!VerifySignature(body, Request.Headers[SignatureHeader].ToString()))
        {
            _logger.LogWarning("Rejected simulate-match: invalid or missing signature");
            return Unauthorized(new SimulateMatchResult { Accepted = false, Message = "Invalid signature" });
        }

        SimulateMatchRequest? request;
        try
        {
            request = System.Text.Json.JsonSerializer.Deserialize<SimulateMatchRequest>(body, JsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Rejected simulate-match: malformed JSON");
            return BadRequest(new SimulateMatchResult { Accepted = false, Message = "Malformed JSON" });
        }

        if (request == null)
            return BadRequest(new SimulateMatchResult { Accepted = false, Message = "Empty payload" });

        var result = await simulation.SimulateMatchAsync(request, ct);
        return result.Accepted ? Ok(result) : UnprocessableEntity(result);
    }

    /// <summary>
    /// The plaintext passwords the development seeder hands out. Stored passwords are BCrypt
    /// hashes and cannot be reversed, so the users endpoint instead *probes* each candidate
    /// against the hash and reports the one that verifies — never a guess.
    /// Mirrors the seeding block in <c>Program.InitializeDatabaseAsync</c>.
    /// </summary>
    private static readonly string[] DevPasswordCandidates = { "Admin123!", "customer123" };

    /// <summary>
    /// Lists login accounts so the simulator can show testers which identity to sign into the
    /// Admin / Bar / Runner / Customer apps with. A password is reported only when one of the
    /// known dev passwords actually verifies against that user's BCrypt hash — otherwise null,
    /// since hashes cannot be reversed. Development-only: returns 404 in any other environment.
    /// </summary>
    [HttpGet("users")]
    public async Task<ActionResult<List<IntegrationUserDto>>> GetUsers(
        [FromServices] IWebHostEnvironment environment, CancellationToken ct)
    {
        if (!environment.IsDevelopment())
            return NotFound();

        var rows = await _context.Users
            .AsNoTracking()
            .OrderBy(u => u.Role)
            .ThenBy(u => u.Username)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.FirstName,
                u.LastName,
                u.Role,
                u.IsActive,
                u.IsShellAccount,
                u.PasswordHash,
                u.CreatedAt,
                u.LastLoginAt
            })
            .ToListAsync(ct);

        var users = rows.Select(u => new IntegrationUserDto
        {
            Id = u.Id,
            Username = u.Username,
            Email = u.Email,
            FullName = string.Join(' ', new[] { u.FirstName, u.LastName }
                .Where(p => !string.IsNullOrWhiteSpace(p))) is { Length: > 0 } name ? name : null,
            Role = u.Role.ToString(),
            IsActive = u.IsActive,
            IsShellAccount = u.IsShellAccount,
            CreatedAt = u.CreatedAt,
            LastLoginAt = u.LastLoginAt,
            // Shell accounts have a deliberately unusable hash — skip the (~100ms each) probe.
            KnownPassword = u.IsShellAccount ? null : ProbePassword(u.PasswordHash)
        }).ToList();

        return Ok(users);
    }

    /// <summary>
    /// Returns whichever dev password verifies against <paramref name="passwordHash"/>, or null
    /// if none does. This confirms a password rather than assuming one, so a renamed or manually
    /// changed account never shows a password that wouldn't actually work.
    /// </summary>
    private static string? ProbePassword(string passwordHash)
    {
        if (string.IsNullOrWhiteSpace(passwordHash))
            return null;

        foreach (var candidate in DevPasswordCandidates)
        {
            try
            {
                if (BCrypt.Net.BCrypt.Verify(candidate, passwordHash))
                    return candidate;
            }
            catch
            {
                // Malformed/non-BCrypt hash — nothing to report for this user.
                return null;
            }
        }

        return null;
    }

    /// <summary>
    /// Lists sellable stadium sectors + capacities (from the drawing-tool overlays — the real
    /// stadium) so the external system/simulator can discover valid sector codes to sell into.
    /// </summary>
    [HttpGet("sections")]
    public async Task<ActionResult<List<StadiumSectionInfoDto>>> GetSections(CancellationToken ct)
    {
        var overlays = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .OrderBy(o => o.SectorCode)
            .ToListAsync(ct);

        var sections = overlays
            .Select(o => new StadiumSectionInfoDto
            {
                Id = o.Id,
                SectionCode = o.SectorCode,
                SectionName = o.Name,
                Capacity = o.TotalSeats,
                Rows = o.Rows,
                SeatsPerRow = o.SeatsPerRow,
                Type = o.Type,
                Price = o.Price,
                SeasonTicketPrice = o.SeasonTicketPrice,
                Color = o.Color,
                ShapeType = o.ShapeType,
                UseVariableSeating = o.UseVariableSeating,
                RowRanges = ParseRowRanges(o),
                TopPercent = o.TopPercent,
                LeftPercent = o.LeftPercent,
                WidthPercent = o.WidthPercent,
                HeightPercent = o.HeightPercent
            })
            .ToList();

        return Ok(sections);
    }

    /// <summary>
    /// Extracts the per-range seat layout for a variable-seating sector from its stored JSON.
    /// Returns an empty list for uniform sectors (or when the JSON is missing/invalid).
    /// </summary>
    private static List<SectorRowRangeDto> ParseRowRanges(StadiumSectorOverlay o)
    {
        if (!o.UseVariableSeating || string.IsNullOrEmpty(o.VariableSeatingData))
            return new List<SectorRowRangeDto>();

        try
        {
            var patterns = System.Text.Json.JsonSerializer.Deserialize<List<RowPattern>>(o.VariableSeatingData);
            return patterns?
                .OrderBy(p => p.FromRow)
                .Select(p => new SectorRowRangeDto
                {
                    FromRow = p.FromRow,
                    ToRow = p.ToRow,
                    SeatsPerRow = p.SeatsPerRow
                })
                .ToList() ?? new List<SectorRowRangeDto>();
        }
        catch
        {
            return new List<SectorRowRangeDto>();
        }
    }

    /// <summary>
    /// Lists events that originated from an external ticketing system (they carry an
    /// <c>ExternalEventId</c>), newest first, with their current sold/capacity totals. Lets
    /// the external system/simulator show already-created events and resume selling into one.
    /// </summary>
    [HttpGet("events")]
    public async Task<ActionResult<List<ExternalEventSummaryDto>>> GetEvents(CancellationToken ct)
    {
        // Lists every event — both externally-synced ones and those created in the Admin panel
        // (which have no ExternalEventId until the simulator adopts them). Project raw fields
        // (incl. Status) first, then compute the lifecycle flags in memory — EventLifecycle
        // rules don't translate to SQL.
        var rows = await _context.Events
            .AsNoTracking()
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => new
            {
                e.Id,
                e.ExternalEventId,
                e.EventName,
                e.EventType,
                e.HomeTeam,
                e.AwayTeam,
                e.EventDate,
                e.EventEndDate,
                e.TicketSalesStartDate,
                e.TicketSalesEndDate,
                e.SourceSystem,
                e.BaseTicketPrice,
                e.TotalSeats,
                e.Status,
                TotalSold = _context.Tickets.Count(t => t.EventId == e.Id && t.Status != TicketStatuses.Cancelled)
            })
            .ToListAsync(ct);

        var nowUtc = DateTime.UtcNow;
        var events = rows.Select(e => new ExternalEventSummaryDto
        {
            EventId = e.Id,
            ExternalEventId = e.ExternalEventId,
            EventName = e.EventName,
            EventType = e.EventType,
            HomeTeam = e.HomeTeam,
            AwayTeam = e.AwayTeam,
            EventDate = e.EventDate,
            EventEndDate = e.EventEndDate,
            SourceSystem = e.SourceSystem,
            BaseTicketPrice = e.BaseTicketPrice,
            TotalSeats = e.TotalSeats,
            TotalSold = e.TotalSold,
            StatusName = e.Status.ToString(),
            CanSellTickets = EventLifecycle.CanSellTickets(e.Status)
                && (e.TicketSalesStartDate == null || nowUtc >= e.TicketSalesStartDate.Value)
                && (e.TicketSalesEndDate == null || nowUtc <= e.TicketSalesEndDate.Value),
            CanOrderDrinks = EventLifecycle.CanOrderDrinks(e.Status)
        }).ToList();

        return Ok(events);
    }

    /// <summary>
    /// Adopts an event that did not originate from the external system (e.g. one created in the
    /// Admin panel) into the integration surface by lazily assigning it an external id, so the
    /// simulator can resume/sell/close/delete it exactly like an externally-created event.
    /// Idempotent: an event that already has an external id is returned unchanged. Signed like
    /// the webhook over the raw — empty — body.
    /// </summary>
    [HttpPost("events/{eventId:int}/adopt")]
    public async Task<IActionResult> AdoptEvent(int eventId, CancellationToken ct)
    {
        // POST carries no body; read (empty) and verify the HMAC over exactly what was signed.
        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync(ct);
        }

        if (!VerifySignature(body, Request.Headers[SignatureHeader].ToString()))
        {
            _logger.LogWarning("Rejected adopt-event: invalid or missing signature");
            return Unauthorized(new { message = "Invalid signature" });
        }

        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, ct);
        if (evt == null)
            return NotFound(new { message = $"Event {eventId} not found" });

        if (string.IsNullOrEmpty(evt.ExternalEventId))
        {
            evt.ExternalEventId = "ADM-" + Guid.NewGuid().ToString("N")[..8].ToUpperInvariant();
            if (string.IsNullOrEmpty(evt.SourceSystem))
                evt.SourceSystem = "TicketingSimulator";
            await _context.SaveChangesAsync(ct);
            _logger.LogInformation("Adopted admin event {EventId} into integration surface as {ExternalId}",
                evt.Id, evt.ExternalEventId);
        }

        return Ok(new { eventId = evt.Id, externalEventId = evt.ExternalEventId });
    }

    /// <summary>
    /// Links an event that isn't part of a season to the current season (if one is set) and
    /// materializes its derived per-seat season-pass tickets, so the sales card reflects season
    /// holders. Idempotent: an event already in a season — or when no current season exists — is a
    /// no-op. Signed like the webhook over the raw — empty — body.
    /// </summary>
    [HttpPost("events/{eventId:int}/link-current-season")]
    public async Task<IActionResult> LinkEventToCurrentSeason(int eventId, CancellationToken ct)
    {
        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync(ct);
        }

        if (!VerifySignature(body, Request.Headers[SignatureHeader].ToString()))
        {
            _logger.LogWarning("Rejected link-current-season: invalid or missing signature");
            return Unauthorized(new { message = "Invalid signature" });
        }

        var exists = await _context.Events.AsNoTracking().AnyAsync(e => e.Id == eventId, ct);
        if (!exists)
            return NotFound(new { message = $"Event {eventId} not found" });

        var (linked, seasonId, derived) = await _ingestion.EnsureEventLinkedToCurrentSeasonAsync(eventId, ct);
        return Ok(new { eventId, linked, seasonId, derivedSeasonTickets = derived });
    }

    /// <summary>
    /// Lists seasons known to our side (newest first) so the external system/simulator can link
    /// events to a season and sell season tickets into one, and resume after a reload.
    /// </summary>
    [HttpGet("seasons")]
    public async Task<ActionResult<List<ExternalSeasonSummaryDto>>> GetSeasons(CancellationToken ct)
    {
        var seasons = await _context.Seasons
            .AsNoTracking()
            .OrderByDescending(s => s.StartDate)
            .Select(s => new ExternalSeasonSummaryDto
            {
                SeasonId = s.Id,
                ExternalSeasonId = s.ExternalSeasonId,
                Name = s.Name,
                StartDate = s.StartDate,
                EndDate = s.EndDate,
                IsCurrent = s.IsCurrent,
                SourceSystem = s.SourceSystem,
                EventCount = _context.Events.Count(e => e.SeasonId == s.Id),
                SeasonTicketCount = _context.SeasonTickets.Count(st => st.SeasonId == s.Id && st.Status != TicketStatuses.Cancelled)
            })
            .ToListAsync(ct);

        return Ok(seasons);
    }

    /// <summary>
    /// Returns the section + external id of every non-cancelled, externally-sold ticket for an
    /// event, so the simulator can rebuild its local per-sector state when resuming that event.
    /// </summary>
    [HttpGet("events/{externalEventId}/tickets")]
    public async Task<ActionResult<List<ExternalTicketRefDto>>> GetEventTickets(string externalEventId, CancellationToken ct)
    {
        var evt = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ExternalEventId == externalEventId, ct);
        if (evt == null)
            return NotFound(new { message = $"No event mapped to external id '{externalEventId}'" });

        var tickets = await _context.Tickets
            .AsNoTracking()
            .Where(t => t.EventId == evt.Id
                        && t.Status != TicketStatuses.Cancelled
                        && t.ExternalTicketId != null
                        && t.Section != null)
            .Select(t => new ExternalTicketRefDto
            {
                SectionCode = t.Section!,
                ExternalTicketId = t.ExternalTicketId!
            })
            .ToListAsync(ct);

        return Ok(tickets);
    }

    /// <summary>
    /// Deletes an externally-originated event by its external id, together with its tickets and
    /// derived season-pass tickets (reusing the Admin force-delete path). Signed like the webhook
    /// over the raw — empty — body, so the external system/simulator can remove a test event it
    /// created. Season passes themselves survive (they belong to the season, not the event).
    /// </summary>
    [HttpDelete("events/{externalEventId}")]
    public async Task<IActionResult> DeleteEvent(string externalEventId, CancellationToken ct)
    {
        // DELETE carries no body; read (empty) and verify the HMAC over exactly what was signed.
        string body;
        using (var reader = new StreamReader(Request.Body, Encoding.UTF8))
        {
            body = await reader.ReadToEndAsync(ct);
        }

        if (!VerifySignature(body, Request.Headers[SignatureHeader].ToString()))
        {
            _logger.LogWarning("Rejected delete-event: invalid or missing signature");
            return Unauthorized(new { message = "Invalid signature" });
        }

        var evt = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ExternalEventId == externalEventId, ct);
        if (evt == null)
            return NotFound(new { message = $"No event mapped to external id '{externalEventId}'" });

        var deleted = await _eventService.DeleteEventAsync(evt.Id);
        if (!deleted)
            return UnprocessableEntity(new { message = "Event could not be deleted" });

        _logger.LogInformation("Deleted external event {ExternalId} (internal {EventId}) via integration surface",
            externalEventId, evt.Id);
        return Ok(new { deleted = true, eventId = evt.Id });
    }

    /// <summary>
    /// Per-sector occupancy for an internal event id, over the real (overlay) sectors.
    /// Used by the Admin Stadium Overview so it reflects the real stadium, not seed sections.
    /// </summary>
    [HttpGet("occupancy/{eventId:int}")]
    public async Task<ActionResult<EventSalesSnapshotDto>> GetOccupancy(int eventId, CancellationToken ct)
    {
        var snapshot = await _ingestion.GetEventOccupancyAsync(eventId, ct);
        return snapshot == null ? NotFound(new { message = $"Event {eventId} not found" }) : Ok(snapshot);
    }

    /// <summary>
    /// Full per-seat map of one sector for an event: every real seat position (row/number) with
    /// its actual occupancy — free, single-match sold, or held by a season pass. Lets the external
    /// system/simulator show the true seat layout the API assigned rather than just counts.
    /// </summary>
    [HttpGet("events/{externalEventId}/sectors/{sectorCode}/seats")]
    public async Task<ActionResult<SectorSeatMapDto>> GetSectorSeats(string externalEventId, string sectorCode, CancellationToken ct)
    {
        var map = await _ingestion.GetSectorSeatMapAsync(externalEventId, sectorCode, ct);
        return map == null
            ? NotFound(new { message = $"No event '{externalEventId}' or sector '{sectorCode}'" })
            : Ok(map);
    }

    /// <summary>
    /// The ticket occupying a specific seat (row/number) of a sector for an event, including its
    /// generated QR code. Lets the external system/simulator show the real ticket for a clicked
    /// seat. The QR image is materialised on first request. 404 when the seat has no active ticket.
    /// </summary>
    [HttpGet("events/{externalEventId}/sectors/{sectorCode}/seats/{row:int}/{seat:int}/ticket")]
    public async Task<ActionResult<SeatTicketDto>> GetSeatTicket(string externalEventId, string sectorCode, int row, int seat, CancellationToken ct)
    {
        var dto = await _ingestion.GetSeatTicketAsync(externalEventId, sectorCode, row, seat, ct);
        return dto == null
            ? NotFound(new { message = $"No active ticket at {sectorCode}-R{row}-S{seat} for event '{externalEventId}'" })
            : Ok(dto);
    }

    /// <summary>
    /// Authoritative per-event sales snapshot used by the external system's reconciliation
    /// to detect and heal drift from missed webhooks.
    /// </summary>
    [HttpGet("events/{externalEventId}/snapshot")]
    public async Task<ActionResult<EventSalesSnapshotDto>> GetSnapshot(string externalEventId, CancellationToken ct)
    {
        var snapshot = await _ingestion.GetEventSnapshotAsync(externalEventId, ct);
        if (snapshot == null)
            return NotFound(new { message = $"No event mapped to external id '{externalEventId}'" });

        return Ok(snapshot);
    }

    /// <summary>
    /// Verifies the HMAC-SHA256 signature of the raw body against the configured shared secret.
    /// The header may be the raw hex digest or prefixed with "sha256=". If no secret is
    /// configured, verification is skipped (development convenience) with a warning.
    /// </summary>
    private bool VerifySignature(string body, string? providedSignature)
    {
        var secret = _configuration["Integration:Ticketing:WebhookSecret"];
        if (string.IsNullOrWhiteSpace(secret))
        {
            _logger.LogWarning("Integration:Ticketing:WebhookSecret is not configured; skipping signature verification");
            return true;
        }

        if (string.IsNullOrWhiteSpace(providedSignature))
            return false;

        var provided = providedSignature.StartsWith("sha256=", StringComparison.OrdinalIgnoreCase)
            ? providedSignature["sha256=".Length..]
            : providedSignature;

        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret));
        var computed = Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(body)));

        // Constant-time comparison (case-insensitive hex).
        return CryptographicOperations.FixedTimeEquals(
            Encoding.ASCII.GetBytes(computed.ToLowerInvariant()),
            Encoding.ASCII.GetBytes(provided.Trim().ToLowerInvariant()));
    }
}
