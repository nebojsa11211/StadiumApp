using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Services
{
    public interface IAdminApiService
    {
        Task<IEnumerable<OrderDto>?> GetOrdersAsync();
        Task<OrderDto?> GetOrderAsync(int id);
        Task<OrderDto?> UpdateOrderAsync(int id, OrderDto order);
        Task<bool> DeleteOrderAsync(int id);

        Task<IEnumerable<UserDto>?> GetUsersAsync();
        Task<UserDto?> GetUserAsync(int id);
        Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto);
        Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto);
        Task<bool> DeleteUserAsync(int id);

        Task<IEnumerable<DrinkDto>?> GetDrinksAsync();
        Task<DrinkDto?> GetDrinkAsync(int id);
        Task<DrinkDto?> CreateDrinkAsync(CreateDrinkDto createDrinkDto);
        Task<DrinkDto?> UpdateDrinkAsync(int id, UpdateDrinkDto updateDrinkDto);
        Task<bool> DeleteDrinkAsync(int id);

        Task<IEnumerable<TicketDto>?> GetTicketsAsync();
        Task<bool> ValidateTicketAsync(string ticketCode);

        Task<string?> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync();
        Task<UserDto?> GetCurrentUserAsync();

        Task<IEnumerable<LogEntryDto>?> GetLogsAsync(LogFilterDto? filterDto = null);
        Task<LogSummaryDto?> GetLogSummaryAsync();
        Task<bool> ClearAllLogsAsync();
        Task LogUserActionAsync(string action, string category, int? userId = null, string? userEmail = null, string? details = null);

        // Customer Analytics
        Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
        Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync();
        Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);

        // Generic HTTP methods
        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object? data = null);

        // Non-generic HTTP methods
        Task<HttpResponseMessage> GetAsync(string endpoint);
        Task<HttpResponseMessage> PostAsync(string endpoint, object? data = null);

        // Stadium Layout
        Task<StadiumLayoutDto?> GetStadiumLayoutAsync();

        // Token property
        string? Token { get; set; }

        // Additional missing methods from compilation errors
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);
        Task<StadiumSummaryDto?> GetStadiumSummaryAsync();
        Task<IEnumerable<TicketDto>?> GetTicketsAsync(int? eventId);
        Task<bool> ImportStadiumStructureAsync(Stream fileStream);
        Task<bool> ImportStadiumStructureAsync(string jsonContent);
        Task<string?> ExportStadiumStructureAsync();
        Task<bool> SimulateTicketSalesAsync(int eventId, int seatCount);
        Task<IEnumerable<EventDto>?> GetEventsAsync();
        Task<object?> GetSeatStatusForEventAsync(int eventId);
        Task<bool> ClearStadiumStructureAsync();
        Task<(bool success, string errorMessage)> DeleteAsync(string endpoint);
    }
}