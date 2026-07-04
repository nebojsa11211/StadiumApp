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

    public async Task<EventSalesSnapshotDto?> GetSnapshotAsync(string externalEventId)
    {
        var resp = await _http.GetAsync($"api/integration/ticketing/events/{Uri.EscapeDataString(externalEventId)}/snapshot");
        return resp.IsSuccessStatusCode
            ? await resp.Content.ReadFromJsonAsync<EventSalesSnapshotDto>(Json)
            : null;
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
