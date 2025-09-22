using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;
using System.Text;
using System.ComponentModel.DataAnnotations;

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
    Task<bool> ValidateQRCodeAsync(string qrToken);
    Task<Ticket?> GetTicketByQRTokenAsync(string qrToken);
    Task<Ticket?> GetTicketByNumberAsync(string ticketNumber);
    Task<OrderSession?> GetOrderSessionAsync(string sessionToken);
    Task<OrderDto?> CheckoutSessionAsync(string sessionToken);
    Task<PaymentIntentDto?> CreatePaymentIntentAsync(int orderId);
    Task<PaymentConfirmationDto?> ConfirmPaymentAsync(string paymentIntentId);
    Task<OrderSession?> CreateOrderSessionFromQRAsync(string qrToken);
    Task<OrderSession?> AddToCartAsync(string sessionToken, int drinkId, int quantity = 1);
    Task<OrderSession?> RemoveFromCartAsync(string sessionToken, int drinkId);
    Task<OrderSession?> UpdateCartItemAsync(string sessionToken, int drinkId, int quantity);
    
    // Logging methods (limited access for customers)
    Task<bool> LogUserActionAsync(string action, string category, string? details = null);
    
    // Customer Ticketing methods
    Task<List<CustomerEventDto>?> GetAvailableEventsAsync(CustomerEventFilterDto? filter = null);
    Task<CustomerEventDetailsDto?> GetEventDetailsAsync(int eventId);
    Task<SectionAvailabilityDto?> GetSectionAvailabilityAsync(int eventId, int sectionId);
    Task<ShoppingCartDto?> GetCartAsync(string sessionId);
    Task<bool> AddSeatToCartAsync(AddSeatToCartRequest request);
    Task<bool> RemoveSeatFromCartAsync(RemoveSeatFromCartRequest request);
    Task<bool> ClearCartAsync(string sessionId);
    Task<SeatAvailabilityResponse?> CheckSeatAvailabilityAsync(int eventId, int sectorId, int rowNumber, int seatNumber);
    Task<CartSummaryDto?> GetCartSummaryAsync(string sessionId);
    Task<TicketOrderResultDto?> ProcessTicketOrderAsync(CreateTicketOrderRequest request);
    Task<OrderConfirmationDto?> GetOrderConfirmationAsync(string orderId);
    Task<UserDto?> GetUserByIdAsync(int userId);
    
    // Ticket Authentication methods
    Task<ValidateTicketResponse?> ValidateTicketAsync(string qrCodeToken);
    Task<TicketSessionDto?> GetTicketSessionAsync(string sessionToken);
    Task<bool> LogoutTicketAsync(string sessionId);
    
    // Generic HTTP methods
    Task<T?> GetAsync<T>(string endpoint) where T : class;
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data) where TResponse : class;
    
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
            var response = await _httpClient.GetAsync("drinks");
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
            var response = await _httpClient.GetAsync($"drinks/{id}");
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

    public async Task<UserDto?> RegisterAsync(RegisterDto registerDto)
    {
        try
        {
            var json = JsonSerializer.Serialize(registerDto, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("auth/register", content);
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
            
            var response = await _httpClient.PostAsync("orders", content);
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
            var response = await _httpClient.GetAsync("orders/my-orders");
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

    public async Task<bool> CancelOrderAsync(int id)
    {
        try
        {
            var response = await _httpClient.PostAsync($"orders/{id}/cancel", null);
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

    public async Task<bool> ValidateQRCodeAsync(string qrToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"tickets/validate/{qrToken}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating QR code: {ex.Message}");
            return false;
        }
    }

    public async Task<Ticket?> GetTicketByQRTokenAsync(string qrToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"tickets/qr/{qrToken}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Ticket>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting ticket by QR token: {ex.Message}");
        }
        return null;
    }

    public async Task<Ticket?> GetTicketByNumberAsync(string ticketNumber)
    {
        try
        {
            var response = await _httpClient.GetAsync($"tickets/number/{ticketNumber}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Ticket>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting ticket by number: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderSession?> GetOrderSessionAsync(string sessionToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"order-sessions/{sessionToken}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderSession>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting order session: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderDto?> CheckoutSessionAsync(string sessionToken)
    {
        try
        {
            var content = new StringContent("{}", Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"order-sessions/{sessionToken}/checkout", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking out session: {ex.Message}");
        }
        return null;
    }

    public async Task<PaymentIntentDto?> CreatePaymentIntentAsync(int orderId)
    {
        try
        {
            var response = await _httpClient.PostAsync($"payment/create-intent/{orderId}", null);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaymentIntentDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating payment intent: {ex.Message}");
        }
        return null;
    }

    public async Task<PaymentConfirmationDto?> ConfirmPaymentAsync(string paymentIntentId)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(new { PaymentIntentId = paymentIntentId }, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("payment/confirm", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<PaymentConfirmationDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error confirming payment: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderSession?> CreateOrderSessionFromQRAsync(string qrToken)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(new { QRToken = qrToken }, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("order-sessions/create-from-qr", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderSession>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error creating order session from QR: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderSession?> AddToCartAsync(string sessionToken, int drinkId, int quantity = 1)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(new { DrinkId = drinkId, Quantity = quantity }, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"order-sessions/{sessionToken}/add-to-cart", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderSession>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding to cart: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderSession?> RemoveFromCartAsync(string sessionToken, int drinkId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"order-sessions/{sessionToken}/remove-from-cart/{drinkId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderSession>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing from cart: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderSession?> UpdateCartItemAsync(string sessionToken, int drinkId, int quantity)
    {
        try
        {
            var content = new StringContent(JsonSerializer.Serialize(new { DrinkId = drinkId, Quantity = quantity }, _jsonOptions), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"order-sessions/{sessionToken}/update-cart-item", content);
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderSession>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error updating cart item: {ex.Message}");
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
                Source = "Customer",
                RequestPath = "/customer",
                HttpMethod = "POST"
            };

            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("logs/log-action", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging user action: {ex.Message}");
            return false;
        }
    }

    // Customer Ticketing Implementation
    public async Task<List<CustomerEventDto>?> GetAvailableEventsAsync(CustomerEventFilterDto? filter = null)
    {
        try
        {
            var queryParams = new List<string>();
            if (filter != null)
            {
                if (!string.IsNullOrEmpty(filter.EventType))
                    queryParams.Add($"eventType={Uri.EscapeDataString(filter.EventType)}");
                if (filter.DateFrom.HasValue)
                    queryParams.Add($"dateFrom={filter.DateFrom.Value:yyyy-MM-dd}");
                if (filter.DateTo.HasValue)
                    queryParams.Add($"dateTo={filter.DateTo.Value:yyyy-MM-dd}");
                if (filter.MinPrice.HasValue)
                    queryParams.Add($"minPrice={filter.MinPrice.Value}");
                if (filter.MaxPrice.HasValue)
                    queryParams.Add($"maxPrice={filter.MaxPrice.Value}");
            }

            var query = queryParams.Any() ? "?" + string.Join("&", queryParams) : "";
            var response = await _httpClient.GetAsync($"customer/ticketing/events{query}");
            
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<List<CustomerEventDto>>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting available events: {ex.Message}");
        }
        return null;
    }

    public async Task<CustomerEventDetailsDto?> GetEventDetailsAsync(int eventId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"customer/ticketing/events/{eventId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CustomerEventDetailsDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting event details: {ex.Message}");
        }
        return null;
    }

    public async Task<SectionAvailabilityDto?> GetSectionAvailabilityAsync(int eventId, int sectionId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"customer/ticketing/events/{eventId}/sections/{sectionId}/availability");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SectionAvailabilityDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting section availability: {ex.Message}");
        }
        return null;
    }

    public async Task<ShoppingCartDto?> GetCartAsync(string sessionId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"customer/cart?sessionId={Uri.EscapeDataString(sessionId)}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ShoppingCartDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting cart: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> AddSeatToCartAsync(AddSeatToCartRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("customer/cart/add", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error adding seat to cart: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> RemoveSeatFromCartAsync(RemoveSeatFromCartRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var requestMessage = new HttpRequestMessage(HttpMethod.Delete, "customer/cart/remove")
            {
                Content = content
            };
            
            var response = await _httpClient.SendAsync(requestMessage);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error removing seat from cart: {ex.Message}");
            return false;
        }
    }

    public async Task<bool> ClearCartAsync(string sessionId)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"customer/cart/clear?sessionId={Uri.EscapeDataString(sessionId)}");
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error clearing cart: {ex.Message}");
            return false;
        }
    }

    public async Task<SeatAvailabilityResponse?> CheckSeatAvailabilityAsync(int eventId, int sectorId, int rowNumber, int seatNumber)
    {
        try
        {
            var response = await _httpClient.GetAsync($"customer/cart/seat-availability?eventId={eventId}&sectorId={sectorId}&rowNumber={rowNumber}&seatNumber={seatNumber}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<SeatAvailabilityResponse>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error checking seat availability: {ex.Message}");
        }
        return null;
    }

    public async Task<CartSummaryDto?> GetCartSummaryAsync(string sessionId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"customer/cart/summary?sessionId={Uri.EscapeDataString(sessionId)}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<CartSummaryDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting cart summary: {ex.Message}");
        }
        return null;
    }

    public async Task<TicketOrderResultDto?> ProcessTicketOrderAsync(CreateTicketOrderRequest request)
    {
        try
        {
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync("customer/orders/create", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TicketOrderResultDto>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error processing ticket order: {ex.Message}");
        }
        return null;
    }

    public async Task<OrderConfirmationDto?> GetOrderConfirmationAsync(string orderId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"customer/orders/{orderId}/confirmation");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<OrderConfirmationDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting order confirmation: {ex.Message}");
        }
        return null;
    }

    public async Task<UserDto?> GetUserByIdAsync(int userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"auth/users/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<UserDto>(json, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting user by ID: {ex.Message}");
        }
        return null;
    }

    // Ticket Authentication methods
    public async Task<ValidateTicketResponse?> ValidateTicketAsync(string qrCodeToken)
    {
        try
        {
            var request = new ValidateTicketRequest { QRCodeToken = qrCodeToken };
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("TicketAuth/validate", content);
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ValidateTicketResponse>(responseJson, _jsonOptions);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error validating ticket: {ex.Message}");
        }
        return null;
    }

    public async Task<TicketSessionDto?> GetTicketSessionAsync(string sessionToken)
    {
        try
        {
            var response = await _httpClient.GetAsync($"TicketAuth/session/{sessionToken}");
            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                var sessionResponse = JsonSerializer.Deserialize<TicketSessionResponse>(responseJson, _jsonOptions);
                return sessionResponse?.Session;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error getting ticket session: {ex.Message}");
        }
        return null;
    }

    public async Task<bool> LogoutTicketAsync(string sessionId)
    {
        try
        {
            var request = new LogoutTicketRequest { SessionId = sessionId };
            var json = JsonSerializer.Serialize(request, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("TicketAuth/logout", content);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging out ticket session: {ex.Message}");
            return false;
        }
    }

    private void SetAuthorizationHeader()
    {
        if (!string.IsNullOrEmpty(Token))
        {
            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Token);
        }
    }

    // Generic HTTP methods
    public async Task<T?> GetAsync<T>(string endpoint) where T : class
    {
        try
        {
            SetAuthorizationHeader();
            Console.WriteLine($"Making GET request to: {_httpClient.BaseAddress}{endpoint}");
            var response = await _httpClient.GetAsync(endpoint);
            Console.WriteLine($"Response status: {response.StatusCode}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response received, length: {json.Length}");
                return JsonSerializer.Deserialize<T>(json, _jsonOptions);
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"HTTP Error {response.StatusCode}: {errorContent}");
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Exception in GetAsync: {ex.Message}");
            Console.WriteLine($"Stack trace: {ex.StackTrace}");
            return null;
        }
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest data) where TResponse : class
    {
        try
        {
            SetAuthorizationHeader();
            var json = JsonSerializer.Serialize(data, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await _httpClient.PostAsync(endpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseJson = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TResponse>(responseJson, _jsonOptions);
            }

            return null;
        }
        catch (Exception)
        {
            return null;
        }
    }

}

// Customer Ticketing DTOs
public class CustomerEventDto
{
    public int Id { get; set; }
    public string EventName { get; set; } = string.Empty;
    public string EventType { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Description { get; set; } = string.Empty;
    public decimal BaseTicketPrice { get; set; }
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public int SoldTickets { get; set; }
    public string VenueInfo { get; set; } = string.Empty;
}

public class CustomerEventDetailsDto : CustomerEventDto
{
    public Dictionary<string, SectionAvailabilityInfo> SectionAvailability { get; set; } = new();
    public List<PricingTierDto> PricingTiers { get; set; } = new();
}

public class CustomerEventFilterDto
{
    public string? EventType { get; set; }
    public DateTime? DateFrom { get; set; }
    public DateTime? DateTo { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}

public class SectionAvailabilityInfo
{
    public int SectionId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public int AvailableSeats { get; set; }
    public decimal BasePrice { get; set; }
}

public class SectionAvailabilityDto
{
    public int EventId { get; set; }
    public int SectionId { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public string TribuneName { get; set; } = string.Empty;
    public string RingName { get; set; } = string.Empty;
    public int TotalSeats { get; set; }
    public List<CustomerSeatDto> AvailableSeats { get; set; } = new();
}

public class CustomerSeatDto
{
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public string SeatCode { get; set; } = string.Empty;
}

public class PricingTierDto
{
    public string Name { get; set; } = string.Empty;
    public decimal BasePrice { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class AddSeatToCartRequest
{
    public string SessionId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public decimal Price { get; set; }
    public int? UserId { get; set; }
}

public class RemoveSeatFromCartRequest
{
    public string SessionId { get; set; } = string.Empty;
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
}

public class SeatAvailabilityResponse
{
    public int EventId { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public bool IsAvailable { get; set; }
}

public class ShoppingCartDto
{
    public int Id { get; set; }
    public string SessionId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public decimal TotalPrice { get; set; }
    public List<CartItemDto> Items { get; set; } = new();
}

public class CartItemDto
{
    public int Id { get; set; }
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int SectorId { get; set; }
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    public string SectionName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime AddedAt { get; set; }
    public DateTime ReservedUntil { get; set; }
}

public class CartSummaryDto
{
    public string SessionId { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public decimal SubTotal { get; set; }
    public decimal ServiceFee { get; set; }
    public decimal Tax { get; set; }
    public decimal Total { get; set; }
}

// Checkout DTOs

public class TicketSessionResponse
{
    public bool Success { get; set; }
    public TicketSessionDto? Session { get; set; }
    public string? ErrorMessage { get; set; }
}

