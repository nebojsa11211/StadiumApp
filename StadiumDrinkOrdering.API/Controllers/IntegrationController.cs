using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.Shared.DTOs.Integration;

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
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<IntegrationController> _logger;

    private static readonly System.Text.Json.JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public IntegrationController(
        ITicketIngestionService ingestion,
        ApplicationDbContext context,
        IConfiguration configuration,
        ILogger<IntegrationController> logger)
    {
        _ingestion = ingestion;
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
