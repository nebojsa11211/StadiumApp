using System.Net.Http.Json;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using StadiumDrinkOrdering.Shared.DTOs.Integration;

namespace StadiumDrinkOrdering.TicketingSimulator.Services;

/// <summary>
/// Talks to the Stadium API's integration surface as if it were the external ticketing
/// system: discovers sections, posts HMAC-signed webhooks, and pulls the reconciliation
/// snapshot.
/// </summary>
public class SimulatorApiClient
{
    private readonly HttpClient _http;
    private readonly string _secret;

    private static readonly JsonSerializerOptions Json = new(JsonSerializerDefaults.Web);

    public SimulatorApiClient(HttpClient http, IConfiguration config)
    {
        _http = http;
        _secret = config["Integration:Ticketing:WebhookSecret"] ?? string.Empty;
        SourceSystem = config["Integration:Ticketing:SourceSystem"] ?? "TicketingSimulator";
    }

    public string SourceSystem { get; }
    public string ApiBaseUrl => _http.BaseAddress?.ToString() ?? "(unset)";

    public async Task<List<StadiumSectionInfoDto>> GetSectionsAsync()
        => await _http.GetFromJsonAsync<List<StadiumSectionInfoDto>>("api/integration/ticketing/sections", Json)
           ?? new List<StadiumSectionInfoDto>();

    public async Task<List<ExternalEventSummaryDto>> GetEventsAsync()
        => await _http.GetFromJsonAsync<List<ExternalEventSummaryDto>>("api/integration/ticketing/events", Json)
           ?? new List<ExternalEventSummaryDto>();

    public async Task<List<ExternalSeasonSummaryDto>> GetSeasonsAsync()
        => await _http.GetFromJsonAsync<List<ExternalSeasonSummaryDto>>("api/integration/ticketing/seasons", Json)
           ?? new List<ExternalSeasonSummaryDto>();

    public async Task<List<ExternalTicketRefDto>> GetEventTicketsAsync(string externalEventId)
        => await _http.GetFromJsonAsync<List<ExternalTicketRefDto>>(
               $"api/integration/ticketing/events/{Uri.EscapeDataString(externalEventId)}/tickets", Json)
           ?? new List<ExternalTicketRefDto>();

    /// <summary>
    /// Full per-seat map of a sector for an event: every real seat position with its actual
    /// occupancy (free / single-match sold / season pass). Null if the event or sector is unknown.
    /// </summary>
    public async Task<SectorSeatMapDto?> GetSectorSeatsAsync(string externalEventId, string sectorCode)
    {
        var path = $"api/integration/ticketing/events/{Uri.EscapeDataString(externalEventId)}"
                   + $"/sectors/{Uri.EscapeDataString(sectorCode)}/seats";
        var resp = await _http.GetAsync(path);
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<SectorSeatMapDto>(Json)
            : null;
    }

    /// <summary>
    /// The ticket occupying a specific seat, including its generated QR code. Null when the seat
    /// has no active ticket (free) or the event/sector is unknown.
    /// </summary>
    public async Task<SeatTicketDto?> GetSeatTicketAsync(string externalEventId, string sectorCode, int row, int seat)
    {
        var path = $"api/integration/ticketing/events/{Uri.EscapeDataString(externalEventId)}"
                   + $"/sectors/{Uri.EscapeDataString(sectorCode)}/seats/{row}/{seat}/ticket";
        var resp = await _http.GetAsync(path);
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<SeatTicketDto>(Json)
            : null;
    }

    public async Task<EventSalesSnapshotDto?> GetSnapshotAsync(string externalEventId)
    {
        var resp = await _http.GetAsync($"api/integration/ticketing/events/{Uri.EscapeDataString(externalEventId)}/snapshot");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<EventSalesSnapshotDto>(Json)
            : null;
    }

    /// <summary>
    /// Asks the API to place a randomised live drink order for the event. Signed like the webhook;
    /// the API rejects it unless the event is live.
    /// </summary>
    public async Task<SimulatedDrinkOrderResult> SimulateDrinkOrderAsync(string externalEventId)
    {
        const string body = "{}"; // no payload needed — the server assembles a random order
        using var content = new StringContent(body, Encoding.UTF8, "application/json");
        var path = $"api/integration/ticketing/events/{Uri.EscapeDataString(externalEventId)}/simulate-order";
        using var req = new HttpRequestMessage(HttpMethod.Post, path) { Content = content };
        req.Headers.TryAddWithoutValidation("X-Signature", "sha256=" + Sign(body));

        var resp = await _http.SendAsync(req);
        var result = await resp.Content.ReadFromJsonAsync<SimulatedDrinkOrderResult>(Json);
        return result ?? new SimulatedDrinkOrderResult { Accepted = false, Message = $"HTTP {(int)resp.StatusCode}" };
    }

    /// <summary>
    /// Deletes an event (and its tickets) by external id. Signed like the webhook over the raw —
    /// empty — body. Returns whether the delete succeeded plus a message for the activity log.
    /// </summary>
    public async Task<(bool Ok, string Message)> DeleteEventAsync(string externalEventId)
    {
        const string body = ""; // DELETE has no body — sign the empty string
        var path = $"api/integration/ticketing/events/{Uri.EscapeDataString(externalEventId)}";
        using var req = new HttpRequestMessage(HttpMethod.Delete, path);
        req.Headers.TryAddWithoutValidation("X-Signature", "sha256=" + Sign(body));

        var resp = await _http.SendAsync(req);
        if (resp.IsSuccessStatusCode)
            return (true, "Deleted");

        var text = await resp.Content.ReadAsStringAsync();
        return (false, string.IsNullOrWhiteSpace(text) ? $"HTTP {(int)resp.StatusCode}" : text);
    }

    /// <summary>
    /// Adopts an event lacking an external id (e.g. one created in the Admin panel) into the
    /// integration surface, so it can be resumed/sold/closed/deleted like a simulator-created
    /// event. Signed like the webhook over the raw — empty — body. Returns the assigned external
    /// id, or null if the call failed.
    /// </summary>
    public async Task<string?> AdoptEventAsync(int eventId)
    {
        const string body = ""; // no body — sign the empty string
        var path = $"api/integration/ticketing/events/{eventId}/adopt";
        using var req = new HttpRequestMessage(HttpMethod.Post, path);
        req.Headers.TryAddWithoutValidation("X-Signature", "sha256=" + Sign(body));

        var resp = await _http.SendAsync(req);
        if (!resp.IsSuccessStatusCode) return null;

        var result = await resp.Content.ReadFromJsonAsync<AdoptEventResult>(Json);
        return result?.ExternalEventId;
    }

    private sealed class AdoptEventResult
    {
        public int EventId { get; set; }
        public string? ExternalEventId { get; set; }
    }

    /// <summary>
    /// Links an event that has no season to the current season and materializes its derived
    /// season-pass seats, so the sales card fills with season holders. Signed over the empty body.
    /// Returns whether a link was made and how many season-pass tickets were materialized (both
    /// zero/false when the event already has a season or no current season exists).
    /// </summary>
    public async Task<(bool Linked, int Derived)> LinkEventToCurrentSeasonAsync(int eventId)
    {
        const string body = ""; // no body — sign the empty string
        var path = $"api/integration/ticketing/events/{eventId}/link-current-season";
        using var req = new HttpRequestMessage(HttpMethod.Post, path);
        req.Headers.TryAddWithoutValidation("X-Signature", "sha256=" + Sign(body));

        var resp = await _http.SendAsync(req);
        if (!resp.IsSuccessStatusCode) return (false, 0);

        var result = await resp.Content.ReadFromJsonAsync<LinkSeasonResult>(Json);
        return (result?.Linked ?? false, result?.DerivedSeasonTickets ?? 0);
    }

    private sealed class LinkSeasonResult
    {
        public int EventId { get; set; }
        public bool Linked { get; set; }
        public int? SeasonId { get; set; }
        public int DerivedSeasonTickets { get; set; }
    }

    public async Task<TicketingWebhookResult> SendAsync(TicketingWebhookEnvelope envelope)
    {
        var body = JsonSerializer.Serialize(envelope, Json);
        using var content = new StringContent(body, Encoding.UTF8, "application/json");
        using var req = new HttpRequestMessage(HttpMethod.Post, "api/integration/ticketing/webhook") { Content = content };
        req.Headers.TryAddWithoutValidation("X-Signature", "sha256=" + Sign(body));

        var resp = await _http.SendAsync(req);
        var result = await resp.Content.ReadFromJsonAsync<TicketingWebhookResult>(Json);
        return result ?? new TicketingWebhookResult { Accepted = false, Message = $"HTTP {(int)resp.StatusCode}" };
    }

    private string Sign(string body)
    {
        using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
        return Convert.ToHexString(hmac.ComputeHash(Encoding.UTF8.GetBytes(body))).ToLowerInvariant();
    }
}
