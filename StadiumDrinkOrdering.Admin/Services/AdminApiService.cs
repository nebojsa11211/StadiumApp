using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace StadiumDrinkOrdering.Admin.Services;

// DTOs for Ticket Sales
public class SeatStatusDto
{
    public int StadiumSeatId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string Status { get; set; } = "Available";
    public string UniqueCode { get; set; } = string.Empty;
}

public class SeatStatusResponseDto
{
    public int EventId { get; set; }
    public Dictionary<string, SeatStatusDto> SoldSeats { get; set; } = new();
}

public interface IAdminApiService
{
    Task<List<DrinkDto>?> GetDrinksAsync();
    Task<DrinkDto?> CreateDrinkAsync(CreateDrinkDto createDrinkDto);
    Task<bool> UpdateDrinkAsync(int id, UpdateDrinkDto updateDrinkDto);
    Task<bool> DeleteDrinkAsync(int id);
    Task<List<OrderDto>?> GetOrdersAsync(OrderStatus? status = null);
    Task<OrderDto?> GetOrderAsync(int id);
    Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateDto);
    Task<StadiumLayoutDto?> GetStadiumLayoutAsync();
    Task<List<StadiumSeatDto>?> GetSectionSeatsAsync(string sectionName);
    Task<StadiumSeatDto?> GetSeatOrderAsync(int seatId);
    Task<List<Event>?> GetEventsAsync();
    Task<Event?> CreateEventAsync(Event eventObj);
    Task<Event?> UpdateEventAsync(int id, Event eventObj);
    Task<bool> ActivateEventAsync(int id);
    Task<bool> DeactivateEventAsync(int id);
    Task<bool> GenerateDemoDataAsync(int eventId);
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    
    // Stadium Structure methods
    Task<StadiumSummaryDto?> GetStadiumSummaryAsync();
    Task<List<Tribune>?> GetStadiumStructureAsync();
    Task<bool> ImportStadiumStructureAsync(Stream fileStream, string fileName);
    Task<bool> ClearStadiumStructureAsync();
    Task<Stream?> ExportStadiumStructureAsync();
    
    // Logging methods
    Task<PagedLogsDto?> GetLogsAsync(LogFilterDto filter);
    Task<LogSummaryDto?> GetLogSummaryAsync();
    Task<bool> ClearOldLogsAsync(int daysToKeep = 30);
    Task<bool> ClearAllLogsAsync();
    Task<bool> LogUserActionAsync(string action, string category, string? details = null);
    
    // User management methods
    Task<UserListDto?> GetUsersAsync(UserFilterDto filter);
    Task<UserDto?> GetUserAsync(int id);
    Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
    Task<bool> ChangeUserPasswordAsync(int id, ChangePasswordDto changePasswordDto);
    Task<bool> DeleteUserAsync(int id);
    
    // Ticket Sales methods
    Task<SeatStatusResponseDto?> GetSeatStatusForEventAsync(int eventId);
    Task<bool> SimulateTicketSalesAsync(int eventId, int numberOfTickets = 10, decimal basePrice = 50.00m);
    
    // Ticket methods
    Task<List<TicketDto>?> GetTicketsAsync(int? eventId = null, bool? isActive = null);
    Task<TicketDto?> GetTicketByNumberAsync(string ticketNumber);
    Task<TicketValidationResultDto?> ValidateTicketAsync(ValidateTicketDto validateDto);
    
    // Customer Analytics methods
    Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
    Task<CustomerSpendingDetailDto?> GetCustomerSpendingDetailsAsync(string customerEmail);
    Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync();
    Task<List<CustomerAnalyticsDto>?> GetTopSpendingCustomersAsync(int limit = 10);
    Task<Dictionary<string, decimal>?> GetCustomerSpendingTrendsAsync(int days = 30);
    Task<HttpResponseMessage> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
    
    // Generic HTTP methods
    Task<HttpResponseMessage> GetAsync(string endpoint);
    Task<T?> GetAsync<T>(string endpoint);
    Task<T?> PostAsync<T>(string endpoint, object data);
    Task<HttpResponseMessage> PostAsync(string endpoint, object data);
    Task<(bool success, string? errorMessage)> DeleteAsync(string endpoint);
    
    string? Token { get; set; }
}

public class AdminApiService : IAdminApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly ITokenStorageService _tokenStorage;

    public string? Token 
    { 
        get => _tokenStorage.Token; 
        set => _tokenStorage.Token = value; 
    }

    public AdminApiService(HttpClient httpClient, ITokenStorageService tokenStorage)
    {
        _httpClient = httpClient;
        _tokenStorage = tokenStorage;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
            WriteIndented = false,
            DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
        };
    }

    public async Task<LoginResponseDto?> LoginAsync(LoginDto loginDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(loginDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/auth/login", content);
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

    public async Task<List<DrinkDto>?> GetDrinksAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/drinks");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<DrinkDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting drinks: {ex.Message}");
        }
        return null;
    }

    public async Task<DrinkDto?> CreateDrinkAsync(CreateDrinkDto createDrinkDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(createDrinkDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/drinks", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<DrinkDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating drink: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> UpdateDrinkAsync(int id, UpdateDrinkDto updateDrinkDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(updateDrinkDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/drinks/{id}", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating drink: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteDrinkAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"api/drinks/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting drink: {ex.Message}");
            return false;
        }
    }

    public async Task<List<OrderDto>?> GetOrdersAsync(OrderStatus? status = null)
    {
        const int maxRetries = 3;
        
        for (int attempt = 1; attempt <= maxRetries; attempt++)
        {
            try
            {
                SetAuthHeader();
                var url = "api/orders";
                if (status.HasValue)
                {
                    url += $"?status={status}";
                }
                
                Console.WriteLine($"=== GetOrdersAsync DEBUG (Attempt {attempt}/{maxRetries}) ===");
                Console.WriteLine($"URL: {url}");
                Console.WriteLine($"Full URL: {_httpClient.BaseAddress}{url}");
                Console.WriteLine($"HasToken: {!string.IsNullOrEmpty(Token)}");
                Console.WriteLine($"Token Value: {(string.IsNullOrEmpty(Token) ? "NULL/EMPTY" : $"Bearer {Token[..Math.Min(20, Token.Length)]}...")}");
                Console.WriteLine($"HttpClient Headers: {string.Join(", ", _httpClient.DefaultRequestHeaders.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                
                // Use progressively longer timeouts for retries
                var timeoutSeconds = 10 + (attempt * 10);
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutSeconds));
                
                var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseContentRead, cts.Token);
                
                Console.WriteLine($"Response Status: {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"Response Headers: {string.Join(", ", response.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                Console.WriteLine($"Content Headers: {string.Join(", ", response.Content.Headers.Select(h => $"{h.Key}: {string.Join(", ", h.Value)}"))}");
                
                var responseContent = await response.Content.ReadAsStringAsync(cts.Token);
                Console.WriteLine($"Response Content Length: {responseContent?.Length ?? 0}");
                Console.WriteLine($"Response Content (first 200 chars): {(responseContent?.Length > 200 ? responseContent[..200] + "..." : responseContent ?? "NULL")}");
                
                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine($"✅ SUCCESS PATH: Attempting to deserialize JSON");
                    Console.WriteLine($"JSON to deserialize: {(responseContent?.Length > 500 ? responseContent[..500] + "..." : responseContent ?? "NULL")}");
                    
                    try
                    {
                        var result = JsonSerializer.Deserialize<List<OrderDto>>(responseContent, _jsonOptions);
                        Console.WriteLine($"✅ SUCCESS: Deserialized Orders Count: {result?.Count ?? 0}");
                        return result;
                    }
                    catch (JsonException jsonEx)
                    {
                        Console.WriteLine($"🚨 JSON DESERIALIZATION ERROR: {jsonEx.Message}");
                        Console.WriteLine($"🚨 Raw response causing error: {responseContent}");
                        throw; // Re-throw to trigger retry
                    }
                }
                else
                {
                    Console.WriteLine($"❌ ERROR PATH: HTTP Error {response.StatusCode}: {responseContent}");
                    
                    // Don't retry on authorization errors
                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized || 
                        response.StatusCode == System.Net.HttpStatusCode.Forbidden)
                    {
                        Console.WriteLine($"⚠️ Authorization error - throwing exception immediately");
                        throw new UnauthorizedAccessException("Authentication required. Please log in to access this resource.");
                    }
                    
                    Console.WriteLine($"⚠️ Non-auth error - will retry if attempts remain");
                }
            }
            catch (HttpRequestException ex) when (ex.Message.Contains("ResponseEnded"))
            {
                Console.WriteLine($"⚠️ ResponseEnded error (attempt {attempt}/{maxRetries}): {ex.Message}");
                if (attempt == maxRetries)
                {
                    Console.WriteLine($"❌ FINAL FAILURE: All retry attempts exhausted");
                    Console.WriteLine($"This suggests network connectivity issues or server overload");
                    Console.WriteLine($"Solution: Restart the API service or check for port conflicts");
                }
                else
                {
                    Console.WriteLine($"🔄 Waiting 2 seconds before retry {attempt + 1}...");
                    await Task.Delay(2000);
                    continue;
                }
            }
            catch (TaskCanceledException ex) when (ex.InnerException is TimeoutException)
            {
                Console.WriteLine($"⏱️ Timeout on attempt {attempt}/{maxRetries}: {ex.Message}");
                if (attempt < maxRetries)
                {
                    await Task.Delay(1000);
                    continue;
                }
            }
            catch (TaskCanceledException ex)
            {
                Console.WriteLine($"🚫 Request cancelled on attempt {attempt}/{maxRetries}: {ex.Message}");
                if (attempt < maxRetries)
                {
                    await Task.Delay(1000);
                    continue;
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Re-throw authentication errors immediately - don't retry
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"💥 Unexpected error on attempt {attempt}/{maxRetries}: {ex.Message}");
                if (attempt < maxRetries)
                {
                    await Task.Delay(1000);
                    continue;
                }
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
            }
            
            // If we get here and it's not the last attempt, continue to retry
            if (attempt < maxRetries)
            {
                Console.WriteLine($"🔄 Retrying in 1 second...");
                await Task.Delay(1000);
            }
        }
        
        Console.WriteLine($"❌ GetOrdersAsync failed after {maxRetries} attempts");
        return new List<OrderDto>(); // Return empty list instead of null to prevent UI crashes
    }

    public async Task<OrderDto?> GetOrderAsync(int id)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync($"api/orders/{id}");
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

    public async Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateDto)
    {
        try
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(updateDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/orders/{id}/status", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order status: {ex.Message}");
            return false;
        }
    }

    public async Task<StadiumLayoutDto?> GetStadiumLayoutAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/stadium/layout");
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

    public async Task<List<StadiumSeatDto>?> GetSectionSeatsAsync(string sectionName)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/stadium/section/{sectionName}/seats");
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

    public async Task<StadiumSeatDto?> GetSeatOrderAsync(int seatId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/stadium/seat/{seatId}/order");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StadiumSeatDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting seat order: {ex.Message}");
        }
        return null;
    }

    public async Task<List<Event>?> GetEventsAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/events");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Event>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting events: {ex.Message}");
        }
        return null;
    }

    public async Task<Event?> CreateEventAsync(Event eventObj)
    {
        try
        {
            var json = JsonSerializer.Serialize(eventObj, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/event", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Event>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating event: {ex.Message}");
        }
        return null;
    }

    public async Task<Event?> UpdateEventAsync(int id, Event eventObj)
    {
        try
        {
            var json = JsonSerializer.Serialize(eventObj, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PutAsync($"api/event/{id}", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Event>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating event: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> ActivateEventAsync(int id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/event/{id}/activate", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error activating event: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeactivateEventAsync(int id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/event/{id}/deactivate", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deactivating event: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> GenerateDemoDataAsync(int eventId)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.PostAsync($"api/demo-data/generate/{eventId}", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error generating demo data: {ex.Message}");
            return false;
        }
    }

    public async Task<StadiumSummaryDto?> GetStadiumSummaryAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/stadiumstructure/summary");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<StadiumSummaryDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting stadium summary: {ex.Message}");
        }
        return null;
    }

    public async Task<List<Tribune>?> GetStadiumStructureAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/stadiumstructure/full-structure");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<Tribune>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting stadium structure: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> ImportStadiumStructureAsync(Stream fileStream, string fileName)
    {
        try
        {
            SetAuthHeader();
            
            // Reset stream position to beginning
            if (fileStream.CanSeek)
            {
                fileStream.Position = 0;
            }
            
            using var content = new MultipartFormDataContent();
            var streamContent = new StreamContent(fileStream);
            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            content.Add(streamContent, "file", fileName);

            var response = await _httpClient.PostAsync("api/stadiumstructure/import/json", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error importing stadium structure: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ClearStadiumStructureAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync("api/stadiumstructure/clear");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing stadium structure: {ex.Message}");
            return false;
        }
    }

    public async Task<Stream?> ExportStadiumStructureAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/stadiumstructure/export/json");
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStreamAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting stadium structure: {ex.Message}");
        }
        return null;
    }

    public async Task<PagedLogsDto?> GetLogsAsync(LogFilterDto filter)
    {
        try
        {
            SetAuthHeader();
            var json = JsonSerializer.Serialize(filter, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/logs/search", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PagedLogsDto>(responseJson, _jsonOptions);
            }
            else
            {
                var errorMessage = $"API Error: {response.StatusCode} - {response.ReasonPhrase}";
                var errorContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(errorContent))
                {
                    errorMessage += $" Details: {errorContent}";
                }
                Console.WriteLine($"Error getting logs: {errorMessage}");
                throw new HttpRequestException(errorMessage);
            }
        }
        catch (Exception ex) when (!(ex is HttpRequestException))
        {
            Console.WriteLine($"Error getting logs: {ex.Message}");
            throw new Exception($"Failed to get logs: {ex.Message}", ex);
        }
    }

    public async Task<LogSummaryDto?> GetLogSummaryAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/logs/summary");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LogSummaryDto>(json, _jsonOptions);
            }
            else
            {
                var errorMessage = $"API Error: {response.StatusCode} - {response.ReasonPhrase}";
                var errorContent = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(errorContent))
                {
                    errorMessage += $" Details: {errorContent}";
                }
                Console.WriteLine($"Error getting log summary: {errorMessage}");
                throw new HttpRequestException(errorMessage);
            }
        }
        catch (Exception ex) when (!(ex is HttpRequestException))
        {
            Console.WriteLine($"Error getting log summary: {ex.Message}");
            throw new Exception($"Failed to get log summary: {ex.Message}", ex);
        }
    }

    public async Task<bool> ClearOldLogsAsync(int daysToKeep = 30)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/logs/clear-old?daysToKeep={daysToKeep}");
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Successfully cleared logs older than {daysToKeep} days");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to clear logs. Status: {response.StatusCode}, Response: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing old logs: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ClearAllLogsAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync("api/logs/clear-all");
            
            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Successfully cleared all logs");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Failed to clear all logs. Status: {response.StatusCode}, Response: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing all logs: {ex.Message}");
            return false;
        }
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
                Source = "Admin",
                RequestPath = "/admin",
                HttpMethod = "POST"
            };

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/logs/log-action", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging user action: {ex.Message}");
            return false;
        }
    }

    public async Task<UserListDto?> GetUsersAsync(UserFilterDto filter)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(filter, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/users/search", content);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserListDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching users: {ex.Message}");
        }
        return null;
    }

    public async Task<UserDto?> GetUserAsync(int id)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync($"api/users/{id}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching user {id}: {ex.Message}");
        }
        return null;
    }

    public async Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(createUserDto, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/users", content);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating user: {ex.Message}");
        }
        return null;
    }

    public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(updateUserDto, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/users/{id}", content);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating user {id}: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> ChangeUserPasswordAsync(int id, ChangePasswordDto changePasswordDto)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(changePasswordDto, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"api/users/{id}/password", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error changing user password {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/users/{id}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting user {id}: {ex.Message}");
            return false;
        }
    }

    public async Task<SeatStatusResponseDto?> GetSeatStatusForEventAsync(int eventId)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync($"api/ticketsales/event/{eventId}/seat-status");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SeatStatusResponseDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting seat status for event {eventId}: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> SimulateTicketSalesAsync(int eventId, int numberOfTickets = 10, decimal basePrice = 50.00m)
    {
        try
        {
            SetAuthHeader();
            var request = new
            {
                NumberOfTickets = numberOfTickets,
                BasePrice = basePrice
            };
            
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync($"api/ticketsales/event/{eventId}/simulate-sales", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error simulating ticket sales for event {eventId}: {ex.Message}");
            return false;
        }
    }

    public async Task<List<TicketDto>?> GetTicketsAsync(int? eventId = null, bool? isActive = null)
    {
        try
        {
            SetAuthHeader();
            var queryParams = new List<string>();
            if (eventId.HasValue) queryParams.Add($"eventId={eventId}");
            if (isActive.HasValue) queryParams.Add($"isActive={isActive}");
            
            var query = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"api/tickets{query}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<TicketDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching tickets: {ex.Message}");
        }
        return null;
    }

    public async Task<TicketDto?> GetTicketByNumberAsync(string ticketNumber)
    {
        try
        {
            SetAuthHeader();
            var encodedTicketNumber = Uri.EscapeDataString(ticketNumber);
            var response = await _httpClient.GetAsync($"api/tickets/{encodedTicketNumber}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TicketDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting ticket by number: {ex.Message}");
        }
        return null;
    }

    public async Task<TicketValidationResultDto?> ValidateTicketAsync(ValidateTicketDto validateDto)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(validateDto, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/tickets/validate", content);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TicketValidationResultDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating ticket: {ex.Message}");
        }
        return null;
    }

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync($"api/{endpoint.TrimStart('/')}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching from {endpoint}: {ex.Message}");
        }
        return default(T);
    }

    public async Task<T?> PostAsync<T>(string endpoint, object data)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(data, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{endpoint.TrimStart('/')}", content);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error posting to {endpoint}: {ex.Message}");
        }
        return default(T);
    }

    public async Task<HttpResponseMessage> PostAsync(string endpoint, object data)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(data, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"api/{endpoint.TrimStart('/')}", content);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error posting to {endpoint}: {ex.Message}");
            throw;
        }
    }

    public async Task<(bool success, string? errorMessage)> DeleteAsync(string endpoint)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.DeleteAsync($"api/{endpoint.TrimStart('/')}");
            
            if (response.IsSuccessStatusCode)
            {
                return (true, null);
            }
            
            // Try to get error message from response
            string? errorMessage = null;
            try
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrWhiteSpace(content))
                {
                    // Try to parse as JSON to get the message
                    using var doc = JsonDocument.Parse(content);
                    if (doc.RootElement.TryGetProperty("message", out var messageProp))
                    {
                        errorMessage = messageProp.GetString();
                    }
                    else if (doc.RootElement.TryGetProperty("Message", out var messageCapProp))
                    {
                        errorMessage = messageCapProp.GetString();
                    }
                    else
                    {
                        errorMessage = content;
                    }
                }
            }
            catch
            {
                errorMessage = await response.Content.ReadAsStringAsync();
            }
            
            return (false, errorMessage ?? $"Failed with status {response.StatusCode}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error deleting from {endpoint}: {ex.Message}");
            return (false, ex.Message);
        }
    }

    public async Task<HttpResponseMessage> GetAsync(string endpoint)
    {
        SetAuthHeader();
        var temp = $"api/{endpoint.TrimStart('/')}";
        var fullUrl = $"{_httpClient.BaseAddress}{temp}";
        
        Console.WriteLine($"=== API CALL DEBUG ===");
        Console.WriteLine($"Endpoint: {endpoint}");
        Console.WriteLine($"Full URL: {fullUrl}");
        Console.WriteLine($"Base Address: {_httpClient.BaseAddress}");
        
        try 
        {
            var response = await _httpClient.GetAsync(temp);
            Console.WriteLine($"Response Status: {response.StatusCode}");
            Console.WriteLine($"======================");
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception: {ex.Message}");
            Console.WriteLine($"======================");
            throw;
        }
    }

    public async Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(filter, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/customeranalytics/search", content);
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PagedCustomerAnalyticsDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting customer analytics: {ex.Message}");
        }
        return null;
    }

    public async Task<CustomerSpendingDetailDto?> GetCustomerSpendingDetailsAsync(string customerEmail)
    {
        try
        {
            SetAuthHeader();
            var encodedEmail = Uri.EscapeDataString(customerEmail);
            var response = await _httpClient.GetAsync($"api/customeranalytics/customer/{encodedEmail}/details");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CustomerSpendingDetailDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting customer spending details: {ex.Message}");
        }
        return null;
    }

    public async Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync()
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync("api/customeranalytics/summary");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CustomerAnalyticsSummaryDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting customer analytics summary: {ex.Message}");
        }
        return null;
    }

    public async Task<List<CustomerAnalyticsDto>?> GetTopSpendingCustomersAsync(int limit = 10)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync($"api/customeranalytics/top-customers?limit={limit}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CustomerAnalyticsDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting top spending customers: {ex.Message}");
        }
        return null;
    }

    public async Task<Dictionary<string, decimal>?> GetCustomerSpendingTrendsAsync(int days = 30)
    {
        try
        {
            SetAuthHeader();
            var response = await _httpClient.GetAsync($"api/customeranalytics/spending-trends?days={days}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Dictionary<string, decimal>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting customer spending trends: {ex.Message}");
        }
        return null;
    }

    public async Task<HttpResponseMessage> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
    {
        try
        {
            SetAuthHeader();
            var content = new StringContent(JsonSerializer.Serialize(filter, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("api/customeranalytics/export", content);
            return response;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error exporting customer analytics: {ex.Message}");
            throw;
        }
    }

    private void SetAuthHeader()
    {
        if (!string.IsNullOrEmpty(Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
            Console.WriteLine($"Token set: {Token[..10]}... (length: {Token.Length})");
        }
        else
        {
            Console.WriteLine("⚠️ NO TOKEN FOUND - This will cause 401 Unauthorized");
        }
    }
}
