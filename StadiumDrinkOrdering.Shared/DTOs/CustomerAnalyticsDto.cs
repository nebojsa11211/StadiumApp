using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Shared.DTOs;

public class CustomerAnalyticsDto
{
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public decimal TotalSpent { get; set; }
    public int TotalTicketsPurchased { get; set; }
    public int TotalEventsAttended { get; set; }
    public int TotalDrinkOrders { get; set; }
    public decimal AverageSpendPerTransaction { get; set; }
    public DateTime FirstPurchaseDate { get; set; }
    public DateTime LastPurchaseDate { get; set; }
    public string PreferredPaymentMethod { get; set; } = string.Empty;
    public List<string> FavoriteEventTypes { get; set; } = new();
    public List<string> FavoriteSections { get; set; } = new();
    public int TotalTransactions { get; set; }
    public decimal TicketSpending { get; set; }
    public decimal DrinkSpending { get; set; }
}

public class CustomerSpendingDetailDto
{
    public string CustomerEmail { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public List<CustomerPurchaseDto> TicketPurchases { get; set; } = new();
    public List<CustomerOrderDto> DrinkOrders { get; set; } = new();
    public CustomerSpendingSummaryDto Summary { get; set; } = new();
}

public class CustomerPurchaseDto
{
    public DateTime PurchaseDate { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string SectionName { get; set; } = string.Empty;
    public string SeatCode { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string PaymentMethod { get; set; } = string.Empty;
    public string TicketNumber { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class CustomerOrderDto
{
    public DateTime OrderDate { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public List<CustomerOrderItemDto> Items { get; set; } = new();
    public DateTime? DeliveredAt { get; set; }
}

public class CustomerOrderItemDto
{
    public string DrinkName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class CustomerSpendingSummaryDto
{
    public decimal TotalSpent { get; set; }
    public decimal TicketSpending { get; set; }
    public decimal DrinkSpending { get; set; }
    public int TotalTickets { get; set; }
    public int TotalDrinkOrders { get; set; }
    public int TotalEvents { get; set; }
    public decimal AverageTicketPrice { get; set; }
    public decimal AverageDrinkOrderValue { get; set; }
    public DateTime FirstPurchase { get; set; }
    public DateTime LastPurchase { get; set; }
    public TimeSpan CustomerLifetime => LastPurchase - FirstPurchase;
    public Dictionary<string, decimal> SpendingByMonth { get; set; } = new();
    public Dictionary<string, int> EventAttendanceByType { get; set; } = new();
    public Dictionary<string, int> PreferredSections { get; set; } = new();
}

public class CustomerAnalyticsFilterDto
{
    public string? SearchTerm { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public decimal? MinSpent { get; set; }
    public decimal? MaxSpent { get; set; }
    public int? MinTickets { get; set; }
    public int? MaxTickets { get; set; }
    public string? EventType { get; set; }
    public string? Section { get; set; }
    public CustomerSortBy SortBy { get; set; } = CustomerSortBy.TotalSpent;
    public bool SortDescending { get; set; } = true;
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class PagedCustomerAnalyticsDto
{
    public List<CustomerAnalyticsDto> Customers { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasNextPage => Page < TotalPages;
    public bool HasPreviousPage => Page > 1;
    public CustomerAnalyticsSummaryDto Summary { get; set; } = new();
}

public class CustomerAnalyticsSummaryDto
{
    public int TotalCustomers { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal AverageCustomerValue { get; set; }
    public decimal AverageTicketPrice { get; set; }
    public decimal AverageDrinkOrderValue { get; set; }
    public int TotalTicketsSold { get; set; }
    public int TotalDrinkOrders { get; set; }
    public int ActiveCustomersLast30Days { get; set; }
    public int NewCustomersLast30Days { get; set; }
    public decimal RevenueGrowthPercentage { get; set; }
    public string TopSpendingCustomer { get; set; } = string.Empty;
    public decimal TopSpendingAmount { get; set; }
}

public enum CustomerSortBy
{
    CustomerName,
    CustomerEmail,
    TotalSpent,
    TicketsPurchased,
    EventsAttended,
    FirstPurchaseDate,
    LastPurchaseDate,
    AverageSpendPerTransaction
}

public class CustomerAnalyticsExportDto
{
    public string CustomerName { get; set; } = string.Empty;
    public string CustomerEmail { get; set; } = string.Empty;
    public string? CustomerPhone { get; set; }
    public decimal TotalSpent { get; set; }
    public int TicketsPurchased { get; set; }
    public int EventsAttended { get; set; }
    public int DrinkOrders { get; set; }
    public decimal AvgSpendPerTransaction { get; set; }
    public string FirstPurchase { get; set; } = string.Empty;
    public string LastPurchase { get; set; } = string.Empty;
    public string PreferredPaymentMethod { get; set; } = string.Empty;
    public string FavoriteEvents { get; set; } = string.Empty;
    public string FavoriteSections { get; set; } = string.Empty;
    public decimal TicketSpending { get; set; }
    public decimal DrinkSpending { get; set; }
    public int TotalTransactions { get; set; }
}