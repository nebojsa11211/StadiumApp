using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace StadiumDrinkOrdering.Bar.Services;

public interface IStaffApiService
{
    Task<List<OrderDto>?> GetActiveOrdersAsync();
    Task<EventDto?> GetCurrentEventAsync();

    /// <summary>
    /// Whether an event is currently live: true (one is), false (the server is certain none is), or
    /// null when the check couldn't complete (API/transport error). The event gate blocks only on a
    /// definitive false so a transient glitch never locks staff out.
    /// </summary>
    Task<bool?> HasLiveEventAsync();
    Task<OrderDto?> GetOrderAsync(int id);
    Task<bool> AssignOrderAsync(int orderId, int staffId);
    Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateDto);

    /// <summary>Bar triage: requeue a returned (DeliveryFailed) order for another delivery attempt.</summary>
    Task<bool> RetryFailedDeliveryAsync(int orderId);

    /// <summary>Bar triage: cancel a returned (DeliveryFailed) order and refund any wallet payment.</summary>
    Task<bool> CancelFailedDeliveryAsync(int orderId);

    Task<List<OrderDto>?> GetAssignedOrdersAsync(int staffId);
    Task<List<OrderDto>?> GetOrderQueueAsync();
    Task<bool> AcceptOrderAsync(int orderId);
    Task<bool> StartPreparationAsync(int orderId);
    Task<bool> MarkReadyAsync(int orderId);
    Task<bool> StartDeliveryAsync(int orderId);
    Task<bool> ConfirmDeliveryAsync(int orderId);
    Task<StadiumLayoutDto?> GetStadiumLayoutAsync();
    Task<List<SectionRefDto>?> GetSectionsAsync();
    Task<List<StadiumSeatDto>?> GetSectionSeatsAsync(string sectionName);
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    
    // Logging methods (for staff to view and log activities)
    Task<PagedLogsDto?> GetLogsAsync(LogFilterDto filter);
    Task<LogSummaryDto?> GetLogSummaryAsync();
    Task<bool> LogUserActionAsync(string action, string category, string? details = null);

    // Bar cash wallet top-up (bartender loads cash onto a fan's wallet at the counter)
    Task<BarTopupResolveResultDto?> ResolveTopupAsync(string query);
    Task<BarTopupResultDto?> SubmitTopupAsync(BarTopupRequestDto request);

    // Bar-counter cash history (top-ups + ticket loads + cash-outs), for the history/reconciliation page.
    Task<BarTopupHistoryListDto?> GetBarTopupHistoryAsync(string? search, bool onlyMine, int page, int pageSize);

    // Create (or reuse) a claimable account for an OIB-identified ticket with no account, so cash lands on
    // the fan's own wallet. Returns a resolve result for that account (HasAccount=true) on success.
    Task<BarTopupResolveResultDto?> ProvisionTopupAccountAsync(BarTopupProvisionRequestDto request);

    // Anonymous ticket wallet (load / cash out a bearer balance on the ticket itself, no account)
    Task<TicketWalletResultDto?> TopUpTicketWalletAsync(TicketWalletTopupRequestDto request);
    Task<TicketWalletResultDto?> CashOutTicketWalletAsync(TicketWalletCashoutRequestDto request);
    Task<TicketWalletClaimResultDto?> ClaimTicketWalletAsync(TicketWalletClaimRequestDto request);

    // DEV-ONLY: real tickets of the currently-live event, so the top-up page can offer a
    // pick-a-ticket combobox instead of forcing a code to be typed/scanned. The endpoint 404s
    // outside Development, so this returns an empty list there.
    Task<List<TestTicketDto>?> GetLiveEventTestTicketsAsync();

    string? Token { get; set; }
}

public class StaffApiService : IStaffApiService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;
    private readonly JsonSerializerOptions _jsonOptions;

    public string? Token { get; set; }

    public StaffApiService(HttpClient httpClient, ITokenStorageService tokenStorage)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
    }

    // IStaffApiService is a transient typed HttpClient, so the instance that DashboardService (and
    // other consumers) receive is NOT the one Login set a token on — its own Token property/header is
    // empty, making authenticated calls fail with 401. The JWT is reliably held by the circuit-shared
    // ITokenStorageService, so we source it from there on every request. GetTokenAsync returns the
    // in-memory token when present (no JS interop) and falls back to localStorage on a fresh circuit.
    private async Task EnsureAuthHeaderAsync()
    {
        var token = await _tokenStorage.GetTokenAsync() ?? Token;
        _httpClient.DefaultRequestHeaders.Authorization =
            string.IsNullOrEmpty(token) ? null : new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(loginDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("auth/login", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var loginResponse = JsonSerializer.Deserialize<LoginResponseDto>(responseJson, _jsonOptions);
                
                if (loginResponse != null)
                {
                    Token = loginResponse.Token;
                    _httpClient.DefaultRequestHeaders.Authorization = 
                        new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
                }
                
                return loginResponse;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging in: {ex.Message}");
        }
        return null;
    }

    public async Task<List<OrderDto>?> GetActiveOrdersAsync()
    {
        try
        {
            // A bar station only works the event that is live right now. If no event is currently
            // live, there is nothing to fulfil — return an empty board rather than surfacing
            // orphaned orders from past/ended events.
            var currentEvent = await GetCurrentEventAsync();
            if (currentEvent == null)
                return new List<OrderDto>();

            await EnsureAuthHeaderAsync();
            // Include ready + out-for-delivery so the KDS can show prepared orders and keep
            // runner-collected ones on the board (greyed) until they're delivered. Delivered orders
            // are fetched too so the dashboard can tally the "Preuzeto"/"Dostavljeno" metrics; the
            // KDS board simply ignores delivered orders (no column renders them).
            var response = await _httpClient.GetAsync("orders?status=pending,accepted,in-preparation,ready,out-for-delivery,delivery-failed,delivered");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var orders = JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions) ?? new();
                // Scope to the live event so tiles/queue stay consistent with the dashboard banner.
                return orders.Where(o => o.EventId == currentEvent.Id).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting active orders: {ex.Message}");
        }
        return null;
    }

    public async Task<EventDto?> GetCurrentEventAsync()
    {
        try
        {
            await EnsureAuthHeaderAsync();
            // The API's /events/live endpoint is the single source of truth for "currently live"
            // (Active/InProgress within its time window); 204 No Content means nothing is live.
            var response = await _httpClient.GetAsync("events/live");
            if (response.StatusCode == HttpStatusCode.NoContent)
                return null;
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<EventDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting current event: {ex.Message}");
        }
        return null;
    }

    public async Task<bool?> HasLiveEventAsync()
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync("events/live");
            if (response.StatusCode == HttpStatusCode.NoContent)
                return false; // Server is definitive: no event is live right now.
            if (response.IsSuccessStatusCode)
                return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking for live event: {ex.Message}");
        }
        return null; // Unknown (API/transport error) — the gate fails open on this.
    }

    public async Task<OrderDto?> GetOrderAsync(int id)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"orders/{id}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting order: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> AssignOrderAsync(int orderId, int staffId)
    {
        try
        {
            var assignmentDto = new { StaffId = staffId };
            var json = JsonSerializer.Serialize(assignmentDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PutAsync($"orders/{orderId}/assign", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error assigning order: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(updateDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PutAsync($"orders/{id}/status", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order status: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RetryFailedDeliveryAsync(int orderId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"orders/{orderId}/retry-delivery", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrying delivery for order {orderId}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> CancelFailedDeliveryAsync(int orderId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"orders/{orderId}/cancel-failed", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cancelling failed delivery for order {orderId}: {ex.Message}");
            return false;
        }
    }

    public async Task<List<OrderDto>?> GetAssignedOrdersAsync(int staffId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"orders/staff/{staffId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting assigned orders: {ex.Message}");
        }
        return null;
    }

    public async Task<StadiumLayoutDto?> GetStadiumLayoutAsync()
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync("stadium/layout");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StadiumLayoutDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting stadium layout: {ex.Message}");
        }
        return null;
    }

    public async Task<List<SectionRefDto>?> GetSectionsAsync()
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync("stadium/sections");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<SectionRefDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting sections: {ex.Message}");
        }
        return null;
    }

    public async Task<List<StadiumSeatDto>?> GetSectionSeatsAsync(string sectionName)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync($"stadium/section/{sectionName}/seats");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<StadiumSeatDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting section seats: {ex.Message}");
        }
        return null;
    }

    public async Task<List<OrderDto>?> GetOrderQueueAsync()
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync("orders/queue");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting order queue: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> AcceptOrderAsync(int orderId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"orders/{orderId}/accept", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accepting order: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> StartPreparationAsync(int orderId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"orders/{orderId}/start-preparation", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting preparation: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> MarkReadyAsync(int orderId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"orders/{orderId}/ready", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error marking order ready: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> StartDeliveryAsync(int orderId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"orders/{orderId}/start-delivery", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error starting delivery: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ConfirmDeliveryAsync(int orderId)
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync($"orders/{orderId}/confirm-delivery", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error confirming delivery: {ex.Message}");
            return false;
        }
    }

    public async Task<PagedLogsDto?> GetLogsAsync(LogFilterDto filter)
    {
        try
        {
            var json = JsonSerializer.Serialize(filter, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("logs/search", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PagedLogsDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting logs: {ex.Message}");
        }
        return null;
    }

    public async Task<LogSummaryDto?> GetLogSummaryAsync()
    {
        try
        {
            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync("logs/summary");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LogSummaryDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting log summary: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> LogUserActionAsync(string action, string category, string? details = null)
    {
        try
        {
            var request = new
            {
                Action = action,
                Category = category,
                Details = details,
                Source = "Staff",
                RequestPath = "/staff",
                HttpMethod = "POST"
            };

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("logs/log-action", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging user action: {ex.Message}");
            return false;
        }
    }

    // Resolve a scanned/typed value (ticket number, QR token, email, or OIB) to the fan account that
    // will receive the cash. BarTopupController is routed under "api/bar/topup" (unlike the other
    // controllers here which have no api prefix), so the endpoint path carries the full "api/bar/..."
    // prefix on top of the configured base address.
    public async Task<BarTopupResolveResultDto?> ResolveTopupAsync(string query)
    {
        try
        {
            var request = new BarTopupResolveRequestDto { Query = query };
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("api/bar/topup/resolve", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BarTopupResolveResultDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error resolving top-up query: {ex.Message}");
        }
        return null;
    }

    // Credit the confirmed cash amount onto the resolved fan's wallet. The caller supplies a fresh
    // IdempotencyKey per confirmed attempt so a retried submit is a safe no-op server-side.
    public async Task<BarTopupResultDto?> SubmitTopupAsync(BarTopupRequestDto request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("api/bar/topup", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BarTopupResultDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error submitting top-up: {ex.Message}");
        }
        return null;
    }

    // Recent bar-counter cash movements for the history page. Same "api/bar/topup" prefix as resolve/submit.
    public async Task<BarTopupHistoryListDto?> GetBarTopupHistoryAsync(string? search, bool onlyMine, int page, int pageSize)
    {
        try
        {
            var url = $"api/bar/topup/history?onlyMine={(onlyMine ? "true" : "false")}&page={page}&pageSize={pageSize}";
            if (!string.IsNullOrWhiteSpace(search))
                url += $"&search={Uri.EscapeDataString(search.Trim())}";

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BarTopupHistoryListDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving bar top-up history: {ex.Message}");
        }
        return null;
    }

    // Create (or reuse) a claimable account for an OIB-identified ticket with no account. On success the
    // returned resolve result has HasAccount=true so the caller can proceed straight to the amount step.
    public async Task<BarTopupResolveResultDto?> ProvisionTopupAccountAsync(BarTopupProvisionRequestDto request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("api/bar/topup/provision", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<BarTopupResolveResultDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error provisioning top-up account: {ex.Message}");
        }
        return null;
    }

    // Anonymous ticket wallet: load cash onto the ticket itself (no account). IdempotencyKey per
    // confirmed attempt makes a retried submit a safe server-side no-op.
    public async Task<TicketWalletResultDto?> TopUpTicketWalletAsync(TicketWalletTopupRequestDto request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("api/bar/ticket-wallet/topup", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TicketWalletResultDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading ticket wallet: {ex.Message}");
        }
        return null;
    }

    // Anonymous ticket wallet: hand back the remaining balance and close the wallet. A fresh
    // IdempotencyKey per attempt lets a network retry complete the same payout while a re-scan of an
    // already-cashed-out ticket comes back as AlreadyCashedOut (nothing to pay).
    public async Task<TicketWalletResultDto?> CashOutTicketWalletAsync(TicketWalletCashoutRequestDto request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("api/bar/ticket-wallet/cashout", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TicketWalletResultDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cashing out ticket wallet: {ex.Message}");
        }
        return null;
    }

    // Anonymous ticket wallet: move the balance onto a registered account by email (and email a
    // set-password link). Idempotent per ticket server-side.
    public async Task<TicketWalletClaimResultDto?> ClaimTicketWalletAsync(TicketWalletClaimRequestDto request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            await EnsureAuthHeaderAsync();
            var response = await _httpClient.PostAsync("api/bar/ticket-wallet/claim", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TicketWalletClaimResultDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error claiming ticket wallet: {ex.Message}");
        }
        return null;
    }

    // DEV-ONLY helper feeding the top-up page's test-ticket picker. Unauthenticated on the API side
    // and 404s outside Development, so we return an empty list on any non-success/failure rather than
    // surfacing an error to the bartender.
    public async Task<List<TestTicketDto>?> GetLiveEventTestTicketsAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("events/live/test-tickets");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TestTicketDto>>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving live-event test tickets: {ex.Message}");
        }
        return null;
    }
}