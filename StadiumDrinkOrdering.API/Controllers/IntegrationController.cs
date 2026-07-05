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
                Capacity = o.TotalSeats
            })
            .ToList();

        return Ok(sections);
    }

    /// <summary>
    /// Lists events that originated from an external ticketing system (they carry an
    /// <c>ExternalEventId</c>), newest first, with their current sold/capacity totals. Lets
    /// the external system/simulator show already-created events and resume selling into one.
    /// </summary>
    [HttpGet("events")]
    public async Task<ActionResult<List<ExternalEventSummaryDto>>> GetEvents(CancellationToken ct)
    {
        // Project raw fields (incl. Status) first, then compute the lifecycle flags in memory —
        // EventLifecycle rules don't translate to SQL.
        var rows = await _context.Events
            .AsNoTracking()
            .Where(e => e.ExternalEventId != null)
            .OrderByDescending(e => e.CreatedAt)
            .Select(e => new
            {
                e.Id,
                e.ExternalEventId,
                e.EventName,
                e.EventType,
                e.EventDate,
                e.EventEndDate,
                e.SourceSystem,
                e.BaseTicketPrice,
                e.TotalSeats,
                e.Status,
                TotalSold = _context.Tickets.Count(t => t.EventId == e.Id && t.Status != TicketStatuses.Cancelled)
            })
            .ToListAsync(ct);

        var events = rows.Select(e => new ExternalEventSummaryDto
        {
            EventId = e.Id,
            ExternalEventId = e.ExternalEventId!,
            EventName = e.EventName,
            EventType = e.EventType,
            EventDate = e.EventDate,
            EventEndDate = e.EventEndDate,
            SourceSystem = e.SourceSystem,
            BaseTicketPrice = e.BaseTicketPrice,
            TotalSeats = e.TotalSeats,
            TotalSold = e.TotalSold,
            StatusName = e.Status.ToString(),
            CanSellTickets = EventLifecycle.CanSellTickets(e.Status),
            CanOrderDrinks = EventLifecycle.CanOrderDrinks(e.Status)
        }).ToList();

        return Ok(events);
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
