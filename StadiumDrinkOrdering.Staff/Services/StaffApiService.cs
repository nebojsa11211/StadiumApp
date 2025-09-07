using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;
using System.Text;

namespace StadiumDrinkOrdering.Staff.Services;

public interface IStaffApiService
{
    Task<List<OrderDto>?> GetActiveOrdersAsync();
    Task<OrderDto?> GetOrderAsync(int id);
    Task<bool> AssignOrderAsync(int orderId, int staffId);
    Task<bool> UpdateOrderStatusAsync(int id, UpdateOrderStatusDto updateDto);
    Task<List<OrderDto>?> GetAssignedOrdersAsync(int staffId);
    Task<List<OrderDto>?> GetOrderQueueAsync();
    Task<bool> AcceptOrderAsync(int orderId);
    Task<bool> StartPreparationAsync(int orderId);
    Task<bool> MarkReadyAsync(int orderId);
    Task<bool> StartDeliveryAsync(int orderId);
    Task<bool> ConfirmDeliveryAsync(int orderId);
    Task<StadiumLayoutDto?> GetStadiumLayoutAsync();
    Task<List<StadiumSeatDto>?> GetSectionSeatsAsync(string sectionName);
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    
    // Logging methods (for staff to view and log activities)
    Task<PagedLogsDto?> GetLogsAsync(LogFilterDto filter);
    Task<LogSummaryDto?> GetLogSummaryAsync();
    Task<bool> LogUserActionAsync(string action, string category, string? details = null);
    
    string? Token { get; set; }
}

public class StaffApiService : IStaffApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public string? Token { get; set; }

    public StaffApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
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

    public async Task<List<OrderDto>?> GetActiveOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/orders?status=pending,accepted,in-preparation");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting active orders: {ex.Message}");
        }
        return null;
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
            
            var response = await _httpClient.PutAsync($"api/orders/{orderId}/assign", content);
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
            
            var response = await _httpClient.PutAsync($"api/orders/{id}/status", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating order status: {ex.Message}");
            return false;
        }
    }

    public async Task<List<OrderDto>?> GetAssignedOrdersAsync(int staffId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"api/orders/staff/{staffId}");
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

    public async Task<List<OrderDto>?> GetOrderQueueAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/orders/queue");
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
            var response = await _httpClient.PostAsync($"api/orders/{orderId}/accept", null);
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
            var response = await _httpClient.PostAsync($"api/orders/{orderId}/start-preparation", null);
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
            var response = await _httpClient.PostAsync($"api/orders/{orderId}/ready", null);
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
            var response = await _httpClient.PostAsync($"api/orders/{orderId}/start-delivery", null);
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
            var response = await _httpClient.PostAsync($"api/orders/{orderId}/confirm-delivery", null);
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
            
            var response = await _httpClient.PostAsync("api/logs/search", content);
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
            var response = await _httpClient.GetAsync("api/logs/summary");
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
            
            var response = await _httpClient.PostAsync("api/logs/log-action", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging user action: {ex.Message}");
            return false;
        }
    }
}