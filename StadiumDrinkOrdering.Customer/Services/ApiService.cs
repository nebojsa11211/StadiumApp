using StadiumDrinkOrdering.Shared.DTOs;
using System.Text.Json;
using System.Text;

namespace StadiumDrinkOrdering.Customer.Services;

public interface IApiService
{
    Task<List<DrinkDto>?> GetDrinksAsync();
    Task<DrinkDto?> GetDrinkAsync(int id);
    Task<LoginResponseDto?> LoginAsync(LoginDto loginDto);
    Task<UserDto?> RegisterAsync(RegisterDto registerDto);
    Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto);
    Task<List<OrderDto>?> GetMyOrdersAsync();
    Task<OrderDto?> GetOrderAsync(int id);
    Task<bool> CancelOrderAsync(int id);
    Task<StadiumLayoutDto?> GetStadiumLayoutAsync();
    string? Token { get; set; }
}

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public string? Token { get; set; }

    public ApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting drink: {ex.Message}");
        }
        return null;
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

    public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(registerDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/auth/register", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error registering: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderDto?> CreateOrderAsync(CreateOrderDto createOrderDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(createOrderDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("api/orders", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating order: {ex.Message}");
        }
        return null;
    }

    public async Task<List<OrderDto>?> GetMyOrdersAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/orders/my-orders");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<OrderDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting my orders: {ex.Message}");
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

    public async Task<bool> CancelOrderAsync(int id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"api/orders/{id}/cancel", null);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error cancelling order: {ex.Message}");
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
}
