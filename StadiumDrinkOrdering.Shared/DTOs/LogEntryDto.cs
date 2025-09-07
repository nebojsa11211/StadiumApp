namespace StadiumDrinkOrdering.Shared.DTOs
{
    public class LogEntryDto
    {
        public int Id { get; set; }
        public DateTime Timestamp { get; set; }
        public string Level { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Action { get; set; } = string.Empty;
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserRole { get; set; }
        public string? IPAddress { get; set; }
        public string? UserAgent { get; set; }
        public string? RequestPath { get; set; }
        public string? HttpMethod { get; set; }
        public string? Message { get; set; }
        public string? Details { get; set; }
        public string? ExceptionType { get; set; }
        public string? StackTrace { get; set; }
        public string Source { get; set; } = string.Empty;
        
        // Business Event Fields
        public string? BusinessEntityType { get; set; }
        public string? BusinessEntityId { get; set; }
        public string? BusinessEntityName { get; set; }
        public string? RelatedEntityType { get; set; }
        public string? RelatedEntityId { get; set; }
        public decimal? MonetaryAmount { get; set; }
        public string? Currency { get; set; }
        public int? Quantity { get; set; }
        public string? LocationInfo { get; set; }
        public string? StatusBefore { get; set; }
        public string? StatusAfter { get; set; }
        public string? MetadataJson { get; set; } // Stored as JSON string
    }

    public class LogFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? Level { get; set; }
        public string? Category { get; set; }
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? Source { get; set; }
        public string? SearchText { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
    }

    public class LogSummaryDto
    {
        public int TotalLogs { get; set; }
        public int ErrorCount { get; set; }
        public int WarningCount { get; set; }
        public int InfoCount { get; set; }
        public int CriticalCount { get; set; }
        public DateTime? LastLogTime { get; set; }
        public Dictionary<string, int> LogsBySource { get; set; } = new();
        public Dictionary<string, int> LogsByCategory { get; set; } = new();
    }

    public class PagedLogsDto
    {
        public List<LogEntryDto> Logs { get; set; } = new();
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    }

    public class BusinessEventFilterDto : LogFilterDto
    {
        public string? BusinessEntityType { get; set; }
        public string? BusinessEntityId { get; set; }
        public List<string>? Actions { get; set; }
        public decimal? MinAmount { get; set; }
        public decimal? MaxAmount { get; set; }
        public string? StatusFilter { get; set; }
        public DateTime? BusinessDateFrom { get; set; }
        public DateTime? BusinessDateTo { get; set; }
    }

    public class BusinessEventDto
    {
        public string Action { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? BusinessEntityType { get; set; }
        public string? BusinessEntityId { get; set; }
        public string? BusinessEntityName { get; set; }
        public string? RelatedEntityType { get; set; }
        public string? RelatedEntityId { get; set; }
        public decimal? MonetaryAmount { get; set; }
        public string? Currency { get; set; }
        public int? Quantity { get; set; }
        public string? LocationInfo { get; set; }
        public string? StatusBefore { get; set; }
        public string? StatusAfter { get; set; }
        public Dictionary<string, object>? Metadata { get; set; }
        public string? UserId { get; set; }
        public string? UserEmail { get; set; }
        public string? UserRole { get; set; }
        public string? Details { get; set; }
        public string Source { get; set; } = string.Empty;
    }

    public class BusinessAnalyticsDto
    {
        public int TotalEvents { get; set; }
        public int TotalOrders { get; set; }
        public int TotalTicketsSold { get; set; }
        public decimal TotalRevenue { get; set; }
        public Dictionary<string, int> EventsByType { get; set; } = new();
        public Dictionary<string, int> OrdersByStatus { get; set; } = new();
        public Dictionary<DateTime, int> DailyActivity { get; set; } = new();
        public List<TopUserActivityDto> TopUsers { get; set; } = new();
    }

    public class TopUserActivityDto
    {
        public string UserId { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public int ActivityCount { get; set; }
        public string MostCommonAction { get; set; } = string.Empty;
    }

    public static class BusinessEventCategories
    {
        public const string EventManagement = "EventManagement";
        public const string TicketSales = "TicketSales";
        public const string OrderProcessing = "OrderProcessing";
        public const string UserManagement = "UserManagement";
        public const string StadiumOperations = "StadiumOperations";
        public const string PaymentProcessing = "PaymentProcessing";
        public const string Analytics = "Analytics";
    }

    public static class BusinessEventActions
    {
        // Event Management
        public const string EventCreated = "EventCreated";
        public const string EventUpdated = "EventUpdated";
        public const string EventActivated = "EventActivated";
        public const string EventCancelled = "EventCancelled";
        
        // Ticket Sales
        public const string TicketPurchased = "TicketPurchased";
        public const string TicketRefunded = "TicketRefunded";
        public const string TicketValidated = "TicketValidated";
        public const string SeatReserved = "SeatReserved";
        public const string SeatReleased = "SeatReleased";
        
        // Order Processing
        public const string OrderCreated = "OrderCreated";
        public const string OrderUpdated = "OrderUpdated";
        public const string OrderCancelled = "OrderCancelled";
        public const string OrderDelivered = "OrderDelivered";
        public const string OrderPaid = "OrderPaid";
        public const string OrderInPreparation = "OrderInPreparation";
        public const string OrderReady = "OrderReady";
        
        // User Management
        public const string StaffMemberAdded = "StaffMemberAdded";
        public const string UserRegistered = "UserRegistered";
        public const string UserRoleChanged = "UserRoleChanged";
        public const string UserDeactivated = "UserDeactivated";
        public const string UserLoggedIn = "UserLoggedIn";
        public const string UserLoggedOut = "UserLoggedOut";
    }
}