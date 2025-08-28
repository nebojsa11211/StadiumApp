using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;
using System.Text;

namespace StadiumDrinkOrdering.Admin.Services;

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
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    string? Token { get; set; }
}

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

    public async Task<List<DrinkDto>?> GetDrinksAsync()
    {
        try
        {
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
        try
        {
            var url = "api/orders";
            if (status.HasValue)
            {
                url += $"?status={status}";
            }
            
            var response = await _httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting orders: {ex.Message}");
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
}
