using System.Text;
using System.Text.Json;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Services
{
    public class AdminApiService : IAdminApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public string? Token { get; set; }

        public AdminApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                PropertyNameCaseInsensitive = true
            };
        }

        // Orders
        public async Task<IEnumerable<OrderDto>?> GetOrdersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/orders");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<OrderDto>>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return Array.Empty<OrderDto>();
        }

        public async Task<OrderDto?> GetOrderAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/orders/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<OrderDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<OrderDto?> UpdateOrderAsync(int id, OrderDto order)
        {
            try
            {
                var json = JsonSerializer.Serialize(order, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/orders/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<OrderDto>(responseJson, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<bool> DeleteOrderAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/orders/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        // Users
        public async Task<IEnumerable<UserDto>?> GetUsersAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/users");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<UserDto>>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return Array.Empty<UserDto>();
        }

        public async Task<UserDto?> GetUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/users/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(createUserDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/users", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(responseJson, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(updateUserDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/users/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(responseJson, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/users/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        // Drinks
        public async Task<IEnumerable<DrinkDto>?> GetDrinksAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/drinks");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<DrinkDto>>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return Array.Empty<DrinkDto>();
        }

        public async Task<DrinkDto?> GetDrinkAsync(int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/drinks/{id}");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<DrinkDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
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
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<DrinkDto?> UpdateDrinkAsync(int id, UpdateDrinkDto updateDrinkDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(updateDrinkDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/drinks/{id}", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<DrinkDto>(responseJson, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<bool> DeleteDrinkAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/drinks/{id}");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        // Tickets
        public async Task<IEnumerable<TicketDto>?> GetTicketsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/tickets");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<TicketDto>>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return Array.Empty<TicketDto>();
        }

        public async Task<bool> ValidateTicketAsync(string ticketCode)
        {
            try
            {
                var response = await _httpClient.PostAsync($"api/tickets/validate/{ticketCode}", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        // Auth
        public async Task<string?> LoginAsync(LoginDto loginDto)
        {
            try
            {
                var json = JsonSerializer.Serialize(loginDto, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/auth/login", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    var result = JsonSerializer.Deserialize<LoginResponseDto>(responseJson, _jsonOptions);
                    return result?.Token;
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                var response = await _httpClient.PostAsync("api/auth/logout", null);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        public async Task<UserDto?> GetCurrentUserAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/auth/current");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        // Logs
        public async Task<IEnumerable<LogEntryDto>?> GetLogsAsync(LogFilterDto? filterDto = null)
        {
            try
            {
                var url = "api/logs";
                if (filterDto != null)
                {
                    var json = JsonSerializer.Serialize(filterDto, _jsonOptions);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await _httpClient.PostAsync($"{url}/search", content);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<IEnumerable<LogEntryDto>>(responseJson, _jsonOptions);
                    }
                }
                else
                {
                    var response = await _httpClient.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseJson = await response.Content.ReadAsStringAsync();
                        return JsonSerializer.Deserialize<IEnumerable<LogEntryDto>>(responseJson, _jsonOptions);
                    }
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return Array.Empty<LogEntryDto>();
        }

        public async Task<LogSummaryDto?> GetLogSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/logs/summary");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<LogSummaryDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        // Additional Log Methods
        public async Task<bool> ClearAllLogsAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync("api/logs/clear-old");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        public async Task LogUserActionAsync(string action, string category, int? userId = null, string? userEmail = null, string? details = null)
        {
            try
            {
                var logEntry = new
                {
                    Action = action,
                    Category = category,
                    UserId = userId,
                    UserEmail = userEmail,
                    Details = details
                };
                var json = JsonSerializer.Serialize(logEntry, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync("api/logs/log-action", content);
            }
            catch
            {
                // Log error in real implementation
            }
        }

        // Customer Analytics Methods
        public async Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
        {
            try
            {
                var json = JsonSerializer.Serialize(filter, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/analytics/customers", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<PagedCustomerAnalyticsDto>(responseJson, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/analytics/customers/summary");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<CustomerAnalyticsSummaryDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
        {
            try
            {
                var json = JsonSerializer.Serialize(filter, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                return await _httpClient.PostAsync("api/analytics/customers/export", content);
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        // Generic HTTP Methods
        public async Task<T?> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return default(T);
        }

        public async Task<T?> PostAsync<T>(string endpoint, object? data = null)
        {
            try
            {
                StringContent? content = null;
                if (data != null)
                {
                    var json = JsonSerializer.Serialize(data, _jsonOptions);
                    content = new StringContent(json, Encoding.UTF8, "application/json");
                }

                var response = await _httpClient.PostAsync(endpoint, content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(responseJson, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return default(T);
        }

        // Stadium Layout
        public async Task<StadiumLayoutDto?> GetStadiumLayoutAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/stadium-structure/layout");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<StadiumLayoutDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        // Additional Methods
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status)
        {
            try
            {
                var updateData = new { Status = status };
                var json = JsonSerializer.Serialize(updateData, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PutAsync($"api/orders/{orderId}/status", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        public async Task<StadiumSummaryDto?> GetStadiumSummaryAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/stadium-structure/summary");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<StadiumSummaryDto>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<IEnumerable<EventDto>?> GetEventsAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/events");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<EventDto>>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return Array.Empty<EventDto>();
        }

        public async Task<object?> GetSeatStatusForEventAsync(int eventId)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/events/{eventId}/seat-status");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<object>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<bool> SimulateTicketSalesAsync(int eventId, int ticketCount)
        {
            try
            {
                var simulationData = new { EventId = eventId, TicketCount = ticketCount };
                var json = JsonSerializer.Serialize(simulationData, _jsonOptions);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"api/events/{eventId}/simulate-sales", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        public async Task<bool> ImportStadiumStructureAsync(string jsonContent)
        {
            try
            {
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync("api/stadium-structure/import", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        public async Task<bool> ClearStadiumStructureAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync("api/stadium-structure/clear");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                // Log error in real implementation
            }
            return false;
        }

        public async Task<string?> ExportStadiumStructureAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/stadium-structure/export");
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<(bool success, string errorMessage)> DeleteAsync(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync(endpoint);
                if (response.IsSuccessStatusCode)
                {
                    return (true, string.Empty);
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    return (false, errorContent);
                }
            }
            catch (Exception ex)
            {
                return (false, ex.Message);
            }
        }

        // Non-generic HTTP methods for DataGrid compatibility
        public async Task<HttpResponseMessage> GetAsync(string endpoint)
        {
            return await _httpClient.GetAsync(endpoint);
        }

        public async Task<HttpResponseMessage> PostAsync(string endpoint, object? data = null)
        {
            var json = JsonSerializer.Serialize(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            return await _httpClient.PostAsync(endpoint, content);
        }

        // Missing method implementations
        public async Task<IEnumerable<TicketDto>?> GetTicketsAsync(int? eventId)
        {
            try
            {
                var url = eventId.HasValue ? $"api/tickets?eventId={eventId.Value}" : "api/tickets";
                var response = await _httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<IEnumerable<TicketDto>>(json, _jsonOptions);
                }
            }
            catch
            {
                // Log error in real implementation
            }
            return null;
        }

        public async Task<bool> ImportStadiumStructureAsync(Stream fileStream)
        {
            try
            {
                var content = new MultipartFormDataContent();
                content.Add(new StreamContent(fileStream), "file", "stadium.json");
                var response = await _httpClient.PostAsync("api/stadium-structure/import", content);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}