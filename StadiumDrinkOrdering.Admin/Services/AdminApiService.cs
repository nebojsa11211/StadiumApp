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
    /// Composite service that delegates to specialized services and maintains backward compatibility
    /// </summary>
    public class AdminApiService : IAdminApiService
    {
        // Specialized service properties
        public IOrderService Orders { get; }
        public IUserService Users { get; }
        public IDrinkService Drinks { get; }
        public ITicketService Tickets { get; }
        public IAuthService Auth { get; }
        public ILogService Logs { get; }
        public IAnalyticsService Analytics { get; }
        public IStadiumService Stadium { get; }
        public IEventService Events { get; }
        public IHttpService Http { get; }

        // Token property delegated to Auth service
        public string? Token
        {
            get => Auth.Token;
            set => Auth.Token = value;
        }

        public AdminApiService(
            IOrderService orderService,
            IUserService userService,
            IDrinkService drinkService,
            ITicketService ticketService,
            IAuthService authService,
            ILogService logService,
            IAnalyticsService analyticsService,
            IStadiumService stadiumService,
            IEventService eventService,
            IHttpService httpService)
        {
            Orders = orderService;
            Users = userService;
            Drinks = drinkService;
            Tickets = ticketService;
            Auth = authService;
            Logs = logService;
            Analytics = analyticsService;
            Stadium = stadiumService;
            Events = eventService;
            Http = httpService;
        }

        // Legacy methods - Order operations (delegated to OrderService)
        public Task<IEnumerable<OrderDto>?> GetOrdersAsync() => Orders.GetOrdersAsync();
        public Task<OrderDto?> GetOrderAsync(int id) => Orders.GetOrderAsync(id);
        public Task<OrderDto?> UpdateOrderAsync(int id, OrderDto order) => Orders.UpdateOrderAsync(id, order);
        public Task<bool> DeleteOrderAsync(int id) => Orders.DeleteOrderAsync(id);
        public Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus status) => Orders.UpdateOrderStatusAsync(orderId, status);

        // Legacy methods - User operations (delegated to UserService)
        public Task<IEnumerable<UserDto>?> GetUsersAsync() => Users.GetUsersAsync();
        public Task<UserDto?> GetUserAsync(int id) => Users.GetUserAsync(id);
        public Task<UserDto?> CreateUserAsync(CreateUserDto createUserDto) => Users.CreateUserAsync(createUserDto);
        public Task<UserDto?> UpdateUserAsync(int id, UpdateUserDto updateUserDto) => Users.UpdateUserAsync(id, updateUserDto);
        public Task<bool> DeleteUserAsync(int id) => Users.DeleteUserAsync(id);

        // Legacy methods - Drink operations (delegated to DrinkService)
        public Task<IEnumerable<DrinkDto>?> GetDrinksAsync() => Drinks.GetDrinksAsync();
        public Task<DrinkDto?> GetDrinkAsync(int id) => Drinks.GetDrinkAsync(id);
        public Task<DrinkDto?> CreateDrinkAsync(CreateDrinkDto createDrinkDto) => Drinks.CreateDrinkAsync(createDrinkDto);
        public Task<DrinkDto?> UpdateDrinkAsync(int id, UpdateDrinkDto updateDrinkDto) => Drinks.UpdateDrinkAsync(id, updateDrinkDto);
        public Task<bool> DeleteDrinkAsync(int id) => Drinks.DeleteDrinkAsync(id);

        // Legacy methods - Ticket operations (delegated to TicketService)
        public Task<IEnumerable<TicketDto>?> GetTicketsAsync() => Tickets.GetTicketsAsync();
        public Task<IEnumerable<TicketDto>?> GetTicketsAsync(int? eventId) => Tickets.GetTicketsAsync(eventId);
        public Task<bool> ValidateTicketAsync(string ticketCode) => Tickets.ValidateTicketAsync(ticketCode);

        // Legacy methods - Auth operations (delegated to AuthService)
        public Task<string?> LoginAsync(LoginDto loginDto) => Auth.LoginAsync(loginDto);
        public Task<bool> LogoutAsync() => Auth.LogoutAsync();
        public Task<UserDto?> GetCurrentUserAsync() => Auth.GetCurrentUserAsync();

        // Legacy methods - Log operations (delegated to LogService)
        public Task<IEnumerable<LogEntryDto>?> GetLogsAsync(LogFilterDto? filterDto = null) => Logs.GetLogsAsync(filterDto);
        public Task<LogSummaryDto?> GetLogSummaryAsync() => Logs.GetLogSummaryAsync();
        public Task<bool> ClearAllLogsAsync() => Logs.ClearAllLogsAsync();
        public Task LogUserActionAsync(string action, string category, int? userId = null, string? userEmail = null, string? details = null) =>
            Logs.LogUserActionAsync(action, category, userId, userEmail, details);

        // Legacy methods - Analytics operations (delegated to AnalyticsService)
        public Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter) => Analytics.GetCustomerAnalyticsAsync(filter);
        public Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync() => Analytics.GetCustomerAnalyticsSummaryAsync();
        public Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter) => Analytics.ExportCustomerAnalyticsAsync(filter);

        // Legacy methods - Stadium operations (delegated to StadiumService)
        public Task<StadiumLayoutDto?> GetStadiumLayoutAsync() => Stadium.GetStadiumLayoutAsync();
        public Task<StadiumSummaryDto?> GetStadiumSummaryAsync() => Stadium.GetStadiumSummaryAsync();
        public Task<bool> ImportStadiumStructureAsync(Stream fileStream) => Stadium.ImportStadiumStructureAsync(fileStream);
        public Task<bool> ImportStadiumStructureAsync(string jsonContent) => Stadium.ImportStadiumStructureAsync(jsonContent);
        public Task<string?> ExportStadiumStructureAsync() => Stadium.ExportStadiumStructureAsync();
        public Task<bool> ClearStadiumStructureAsync() => Stadium.ClearStadiumStructureAsync();

        // Legacy methods - Event operations (delegated to EventService)
        public Task<IEnumerable<EventDto>?> GetEventsAsync() => Events.GetEventsAsync();
        public Task<object?> GetSeatStatusForEventAsync(int eventId) => Events.GetSeatStatusForEventAsync(eventId);
        public Task<bool> SimulateTicketSalesAsync(int eventId, int ticketCount) => Events.SimulateTicketSalesAsync(eventId, ticketCount);

        // Legacy methods - HTTP operations (delegated to HttpService)
        public Task<T?> GetAsync<T>(string endpoint) => Http.GetAsync<T>(endpoint);
        public Task<T?> PostAsync<T>(string endpoint, object? data = null) => Http.PostAsync<T>(endpoint, data);
        public Task<HttpResponseMessage> GetAsync(string endpoint) => Http.GetAsync(endpoint);
        public Task<HttpResponseMessage> PostAsync(string endpoint, object? data = null) => Http.PostAsync(endpoint, data);
        public Task<(bool success, string errorMessage)> DeleteAsync(string endpoint) => Http.DeleteAsync(endpoint);
    }
}