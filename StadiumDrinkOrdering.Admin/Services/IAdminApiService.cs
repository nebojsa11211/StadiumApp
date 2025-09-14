using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services.Orders;
using StadiumDrinkOrdering.Admin.Services.Users;
using StadiumDrinkOrdering.Admin.Services.Drinks;
using StadiumDrinkOrdering.Admin.Services.Tickets;
using StadiumDrinkOrdering.Admin.Services.Auth;
using StadiumDrinkOrdering.Admin.Services.Logs;
using StadiumDrinkOrdering.Admin.Services.Analytics;
using StadiumDrinkOrdering.Admin.Services.Stadium;
using StadiumDrinkOrdering.Admin.Services.Events;
using StadiumDrinkOrdering.Admin.Services.Http;

namespace StadiumDrinkOrdering.Admin.Services
{
    /// <summary>
    /// Composite interface that provides access to all admin services
    /// </summary>
    public interface IAdminApiService
    {
        // Service properties for direct access to specialized services
        IOrderService Orders { get; }
        IUserService Users { get; }
        IDrinkService Drinks { get; }
        ITicketService Tickets { get; }
        IAuthService Auth { get; }
        ILogService Logs { get; }
        IAnalyticsService Analytics { get; }
        IStadiumService Stadium { get; }
        IEventService Events { get; }
        IHttpService Http { get; }

        // Legacy methods for backward compatibility (delegated to specialized services)
        Task<IEnumerable<OrderDto>?> GetOrdersAsync();
        Task<OrderDto?> GetOrderAsync(int id);
        Task<OrderDto?> UpdateOrderAsync(int id, OrderDto order);
        Task<bool> DeleteOrderAsync(int id);
        Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status);

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
        Task<IEnumerable<TicketDto>?> GetTicketsAsync(int? eventId);
        Task<bool> ValidateTicketAsync(string ticketCode);

        string? Token { get; set; }
        Task<string?> LoginAsync(LoginDto loginDto);
        Task<bool> LogoutAsync();
        Task<UserDto?> GetCurrentUserAsync();

        Task<IEnumerable<LogEntryDto>?> GetLogsAsync(LogFilterDto? filterDto = null);
        Task<LogSummaryDto?> GetLogSummaryAsync();
        Task<bool> ClearAllLogsAsync();
        Task LogUserActionAsync(string action, string category, int? userId = null, string? userEmail = null, string? details = null);

        Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
        Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync();
        Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);

        Task<StadiumLayoutDto?> GetStadiumLayoutAsync();
        Task<StadiumSummaryDto?> GetStadiumSummaryAsync();
        Task<bool> ImportStadiumStructureAsync(Stream fileStream);
        Task<bool> ImportStadiumStructureAsync(string jsonContent);
        Task<string?> ExportStadiumStructureAsync();
        Task<bool> ClearStadiumStructureAsync();

        Task<IEnumerable<EventDto>?> GetEventsAsync();
        Task<object?> GetSeatStatusForEventAsync(int eventId);
        Task<bool> SimulateTicketSalesAsync(int eventId, int ticketCount);

        Task<T?> GetAsync<T>(string endpoint);
        Task<T?> PostAsync<T>(string endpoint, object? data = null);
        Task<HttpResponseMessage> GetAsync(string endpoint);
        Task<HttpResponseMessage> PostAsync(string endpoint, object? data = null);
        Task<(bool success, string errorMessage)> DeleteAsync(string endpoint);
    }
}