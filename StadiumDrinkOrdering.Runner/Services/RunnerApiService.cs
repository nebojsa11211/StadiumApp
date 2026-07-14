using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Runner.Models;

namespace StadiumDrinkOrdering.Runner.Services;

public enum ClaimResult { Claimed, Taken, Failed }

public enum ScanOutcome { Found, NotFound, Error }

// Result of scanning/looking up a ticket: the outcome plus the detail when found. Lets the UI
// distinguish a genuinely unknown ticket (NotFound) from a transport/auth failure (Error).
public record ScanResult(ScanOutcome Outcome, TicketDetailDto? Ticket);

// Result of a staff re-issue: whether the call succeeded and how many active sessions were released.
public record ReleaseAccessResult(bool Ok, int Released);

/// <summary>
/// All API calls the Runner makes. Uses System.Net.Http.Json (web JSON defaults = camelCase,
/// case-insensitive), matching the API. A 401 (expired/invalid token) clears the session and
/// bounces to login.
/// </summary>
public class RunnerApiService
{
    private readonly HttpClient _http;
    private readonly RunnerAuthService _auth;
    private readonly NavigationManager _nav;

    public RunnerApiService(HttpClient http, RunnerAuthService auth, NavigationManager nav)
    {
        _http = http;
        _auth = auth;
        _nav = nav;
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto dto)
    {
        try
        {
            var resp = await _http.PostAsJsonAsync("auth/login", dto);
            if (resp.IsSuccessStatusCode)
                return await resp.Content.ReadFromJsonAsync<LoginResponseDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Login failed: {ex.Message}");
        }
        return null;
    }

    /// <summary>
    /// Reports whether an event is currently live: true (one is), false (the server is certain none
    /// is), or null when the check couldn't complete (offline / transport error). The event gate
    /// blocks only on a definitive false, so an offline runner is never locked out mid-shift. This
    /// endpoint is anonymous and read-only, so it deliberately skips the 401 bounce.
    /// </summary>
    public async Task<bool?> HasLiveEventAsync()
    {
        try
        {
            var resp = await _http.GetAsync("events/live");
            if (resp.StatusCode == HttpStatusCode.NoContent) return false;
            if (resp.IsSuccessStatusCode) return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Live-event check failed: {ex.Message}");
        }
        return null;
    }

    public Task<List<OrderDto>?> GetPoolAsync() => GetListAsync("orders/available-for-delivery");

    public Task<List<OrderDto>?> GetMyDeliveriesAsync() => GetListAsync("orders/mine");

    private async Task<List<OrderDto>?> GetListAsync(string url)
    {
        try
        {
            var resp = await _http.GetAsync(url);
            if (await Handle401(resp)) return null;
            if (resp.IsSuccessStatusCode)
                return await resp.Content.ReadFromJsonAsync<List<OrderDto>>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"GET {url} failed: {ex.Message}");
        }
        return null;
    }

    public async Task<ClaimResult> ClaimAsync(int orderId)
    {
        try
        {
            var resp = await _http.PostAsync($"orders/{orderId}/claim", null);
            if (await Handle401(resp)) return ClaimResult.Failed;
            if (resp.IsSuccessStatusCode) return ClaimResult.Claimed;
            if (resp.StatusCode == HttpStatusCode.Conflict) return ClaimResult.Taken;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Claim {orderId} failed: {ex.Message}");
        }
        return ClaimResult.Failed;
    }

    /// <summary>
    /// Claims several pool orders at once for this runner. Returns the server's per-order breakdown
    /// (claimed / taken / not-found), or null on a transport/auth failure so the caller can surface a
    /// generic error. A partially-stale selection still claims whatever is available.
    /// </summary>
    public async Task<BatchClaimResultDto?> ClaimBatchAsync(IEnumerable<int> orderIds)
    {
        try
        {
            var dto = new BatchClaimRequestDto { OrderIds = orderIds.ToList() };
            var resp = await _http.PostAsJsonAsync("orders/claim-batch", dto);
            if (await Handle401(resp)) return null;
            if (resp.IsSuccessStatusCode)
                return await resp.Content.ReadFromJsonAsync<BatchClaimResultDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Batch claim failed: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> MarkDeliveredAsync(int orderId, Guid clientActionId)
    {
        try
        {
            var dto = new UpdateOrderStatusDto
            {
                Status = OrderStatus.Delivered,
                Notes = "Delivered by runner",
                ClientActionId = clientActionId
            };
            var resp = await _http.PutAsJsonAsync($"orders/{orderId}/status", dto);
            if (await Handle401(resp)) return false;
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"MarkDelivered {orderId} failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Reports that this runner couldn't deliver an order (fan absent, refused, wrong seat…). The API
    /// treats an already-failed order as an idempotent success, so the offline outbox can safely retry.
    /// A 409 means it's no longer this runner's to fail (e.g. reassigned); treated as "done" so the
    /// queued action is dropped rather than retried forever.
    /// </summary>
    public async Task<bool> ReportDeliveryFailedAsync(int orderId, DeliveryFailureReason reason, Guid clientActionId, string? notes = null)
    {
        try
        {
            var dto = new ReportDeliveryFailedDto { Reason = reason, Notes = notes, ClientActionId = clientActionId };
            var resp = await _http.PostAsJsonAsync($"orders/{orderId}/delivery-failed", dto);
            if (await Handle401(resp)) return false;
            if (resp.StatusCode == HttpStatusCode.Conflict) return true; // no longer ours — stop retrying
            return resp.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ReportDeliveryFailed {orderId} failed: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Resolves a scanned/typed ticket code to its full drill-down. The code may be a scanned QR
    /// payload (the customer deep link .../t/{token}), a bare QR token, or a printed ticket number —
    /// the server normalizes all three. Returns NotFound for an unknown ticket and Error on a
    /// transport/auth failure.
    /// </summary>
    public async Task<ScanResult> ScanTicketAsync(string code)
    {
        try
        {
            var resp = await _http.GetAsync($"tickets/scan-details?code={Uri.EscapeDataString(code)}");
            if (await Handle401(resp)) return new ScanResult(ScanOutcome.Error, null);
            if (resp.StatusCode == HttpStatusCode.NotFound) return new ScanResult(ScanOutcome.NotFound, null);
            if (resp.IsSuccessStatusCode)
            {
                var dto = await resp.Content.ReadFromJsonAsync<TicketDetailDto>();
                return dto == null
                    ? new ScanResult(ScanOutcome.Error, null)
                    : new ScanResult(ScanOutcome.Found, dto);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ScanTicket failed: {ex.Message}");
        }
        return new ScanResult(ScanOutcome.Error, null);
    }

    /// <summary>
    /// Staff re-issue: releases a ticket's active ordering sessions so a genuine holder who lost access
    /// can re-scan and claim it on a new device. Returns how many sessions were released.
    /// </summary>
    public async Task<ReleaseAccessResult> ReleaseTicketAccessAsync(int ticketId)
    {
        try
        {
            var resp = await _http.PostAsync($"tickets/{ticketId}/release-access", null);
            if (await Handle401(resp)) return new ReleaseAccessResult(false, 0);
            if (resp.IsSuccessStatusCode)
            {
                var body = await resp.Content.ReadFromJsonAsync<ReleaseAccessResponse>();
                return new ReleaseAccessResult(true, body?.Released ?? 0);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ReleaseTicketAccess failed: {ex.Message}");
        }
        return new ReleaseAccessResult(false, 0);
    }

    private sealed class ReleaseAccessResponse
    {
        public int Released { get; set; }
    }

    private async Task<bool> Handle401(HttpResponseMessage resp)
    {
        if (resp.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _auth.ClearAsync();
            _nav.NavigateTo("login");
            return true;
        }
        return false;
    }
}
