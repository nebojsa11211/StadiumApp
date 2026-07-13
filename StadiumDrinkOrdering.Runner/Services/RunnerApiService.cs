using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Runner.Models;

namespace StadiumDrinkOrdering.Runner.Services;

public enum ClaimResult { Claimed, Taken, Failed }

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
