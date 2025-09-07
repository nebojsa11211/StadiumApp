using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text;
using System.Globalization;

namespace StadiumDrinkOrdering.API.Services;

public class CustomerAnalyticsService : ICustomerAnalyticsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CustomerAnalyticsService> _logger;

    public CustomerAnalyticsService(
        ApplicationDbContext context,
        ILogger<CustomerAnalyticsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
    {
        try
        {
            // Get all unique customers from tickets (primary data source)
            var ticketQuery = _context.Tickets.AsQueryable();
            
            // Apply date filters
            if (filter.StartDate.HasValue)
            {
                ticketQuery = ticketQuery.Where(t => t.PurchaseDate >= filter.StartDate.Value);
            }
            
            if (filter.EndDate.HasValue)
            {
                ticketQuery = ticketQuery.Where(t => t.PurchaseDate <= filter.EndDate.Value);
            }

            // Apply event type filter
            if (!string.IsNullOrEmpty(filter.EventType))
            {
                ticketQuery = ticketQuery.Include(t => t.Event)
                    .Where(t => t.Event != null && t.Event.EventType.Contains(filter.EventType));
            }

            // Apply section filter
            if (!string.IsNullOrEmpty(filter.Section))
            {
                ticketQuery = ticketQuery.Where(t => !string.IsNullOrEmpty(t.Section) && t.Section.Contains(filter.Section));
            }

            // Get tickets data first to handle SQLite decimal limitations
            var ticketsData = await ticketQuery
                .Where(t => !string.IsNullOrEmpty(t.CustomerEmail))
                .ToListAsync();

            // Group by customer email to get aggregated data (client-side)
            var customerQuery = ticketsData
                .GroupBy(t => t.CustomerEmail)
                .Select(g => new
                {
                    CustomerEmail = g.Key,
                    CustomerName = g.FirstOrDefault()?.CustomerName ?? "",
                    CustomerPhone = g.FirstOrDefault()?.CustomerPhone,
                    TotalTicketSpent = g.Sum(t => t.Price),
                    TotalTicketsPurchased = g.Count(),
                    EventsAttended = g.Select(t => t.EventId).Distinct().Count(),
                    FirstPurchaseDate = g.Min(t => t.PurchaseDate),
                    LastPurchaseDate = g.Max(t => t.PurchaseDate),
                    FavoriteSections = g.Where(t => !string.IsNullOrEmpty(t.Section))
                                      .GroupBy(t => t.Section)
                                      .OrderByDescending(sg => sg.Count())
                                      .Take(3)
                                      .Select(sg => sg.Key)
                                      .ToList(),
                    TotalTransactions = g.Count()
                })
                .AsQueryable();

            // Apply search filter
            if (!string.IsNullOrEmpty(filter.SearchTerm))
            {
                var searchTerm = filter.SearchTerm.ToLower();
                customerQuery = customerQuery.Where(c => 
                    (c.CustomerEmail != null && c.CustomerEmail.ToLower().Contains(searchTerm)) ||
                    (c.CustomerName != null && c.CustomerName.ToLower().Contains(searchTerm))
                );
            }

            // Apply spending filters
            if (filter.MinSpent.HasValue)
            {
                customerQuery = customerQuery.Where(c => c.TotalTicketSpent >= filter.MinSpent.Value);
            }
            
            if (filter.MaxSpent.HasValue)
            {
                customerQuery = customerQuery.Where(c => c.TotalTicketSpent <= filter.MaxSpent.Value);
            }

            // Apply tickets count filters
            if (filter.MinTickets.HasValue)
            {
                customerQuery = customerQuery.Where(c => c.TotalTicketsPurchased >= filter.MinTickets.Value);
            }
            
            if (filter.MaxTickets.HasValue)
            {
                customerQuery = customerQuery.Where(c => c.TotalTicketsPurchased <= filter.MaxTickets.Value);
            }

            // Get total count before pagination
            var totalCount = customerQuery.Count();

            // Apply sorting
            customerQuery = filter.SortBy switch
            {
                CustomerSortBy.CustomerName => filter.SortDescending 
                    ? customerQuery.OrderByDescending(c => c.CustomerName)
                    : customerQuery.OrderBy(c => c.CustomerName),
                CustomerSortBy.CustomerEmail => filter.SortDescending
                    ? customerQuery.OrderByDescending(c => c.CustomerEmail)
                    : customerQuery.OrderBy(c => c.CustomerEmail),
                CustomerSortBy.TotalSpent => filter.SortDescending
                    ? customerQuery.OrderByDescending(c => c.TotalTicketSpent)
                    : customerQuery.OrderBy(c => c.TotalTicketSpent),
                CustomerSortBy.TicketsPurchased => filter.SortDescending
                    ? customerQuery.OrderByDescending(c => c.TotalTicketsPurchased)
                    : customerQuery.OrderBy(c => c.TotalTicketsPurchased),
                CustomerSortBy.EventsAttended => filter.SortDescending
                    ? customerQuery.OrderByDescending(c => c.EventsAttended)
                    : customerQuery.OrderBy(c => c.EventsAttended),
                CustomerSortBy.FirstPurchaseDate => filter.SortDescending
                    ? customerQuery.OrderByDescending(c => c.FirstPurchaseDate)
                    : customerQuery.OrderBy(c => c.FirstPurchaseDate),
                CustomerSortBy.LastPurchaseDate => filter.SortDescending
                    ? customerQuery.OrderByDescending(c => c.LastPurchaseDate)
                    : customerQuery.OrderBy(c => c.LastPurchaseDate),
                CustomerSortBy.AverageSpendPerTransaction => filter.SortDescending
                    ? customerQuery.OrderByDescending(c => c.TotalTicketSpent / c.TotalTransactions)
                    : customerQuery.OrderBy(c => c.TotalTicketSpent / c.TotalTransactions),
                _ => customerQuery.OrderByDescending(c => c.TotalTicketSpent)
            };

            // Apply pagination
            var customers = customerQuery
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToList();

            // Get drink order data for these customers (handle SQLite decimal limitation)
            var customerEmails = customers.Select(c => c.CustomerEmail).ToList();
            var drinkOrdersData = await _context.Orders
                .Include(o => o.Customer)
                .Where(o => customerEmails.Contains(o.Customer.Email))
                .ToListAsync();

            var drinkOrders = drinkOrdersData
                .GroupBy(o => o.Customer.Email)
                .Select(g => new
                {
                    CustomerEmail = g.Key,
                    TotalDrinkSpent = g.Sum(o => o.TotalAmount),
                    TotalDrinkOrders = g.Count()
                })
                .ToList();

            // Combine ticket and drink data
            var result = customers.Select(c =>
            {
                var drinkData = drinkOrders.FirstOrDefault(d => d.CustomerEmail == c.CustomerEmail);
                var drinkSpent = drinkData?.TotalDrinkSpent ?? 0;
                var drinkOrderCount = drinkData?.TotalDrinkOrders ?? 0;
                var totalSpent = c.TotalTicketSpent + drinkSpent;
                var totalTransactions = c.TotalTransactions + drinkOrderCount;

                return new CustomerAnalyticsDto
                {
                    CustomerEmail = c.CustomerEmail,
                    CustomerName = c.CustomerName ?? "",
                    CustomerPhone = c.CustomerPhone,
                    TotalSpent = totalSpent,
                    TotalTicketsPurchased = c.TotalTicketsPurchased,
                    TotalEventsAttended = c.EventsAttended,
                    TotalDrinkOrders = drinkOrderCount,
                    AverageSpendPerTransaction = totalTransactions > 0 ? totalSpent / totalTransactions : 0,
                    FirstPurchaseDate = c.FirstPurchaseDate,
                    LastPurchaseDate = c.LastPurchaseDate,
                    PreferredPaymentMethod = "Credit Card", // Default for now
                    FavoriteEventTypes = new List<string>(), // Can be enhanced later
                    FavoriteSections = c.FavoriteSections ?? new List<string>(),
                    TotalTransactions = totalTransactions,
                    TicketSpending = c.TotalTicketSpent,
                    DrinkSpending = drinkSpent
                };
            }).ToList();

            // Calculate summary
            var summary = await GetCustomerAnalyticsSummaryAsync();

            return new PagedCustomerAnalyticsDto
            {
                Customers = result,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize,
                Summary = summary ?? new CustomerAnalyticsSummaryDto()
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer analytics");
            throw;
        }
    }

    public async Task<CustomerSpendingDetailDto?> GetCustomerSpendingDetailsAsync(string customerEmail)
    {
        try
        {
            // Get ticket purchases
            var ticketPurchases = await _context.Tickets
                .Include(t => t.Event)
                .Where(t => t.CustomerEmail == customerEmail)
                .OrderByDescending(t => t.PurchaseDate)
                .Select(t => new CustomerPurchaseDto
                {
                    PurchaseDate = t.PurchaseDate,
                    EventName = t.EventName ?? "",
                    EventDate = t.EventDate ?? DateTime.MinValue,
                    SectionName = t.Section ?? "",
                    SeatCode = $"{t.Section}-R{t.Row}-S{t.SeatNumber}",
                    Price = t.Price,
                    PaymentMethod = "Credit Card", // Default
                    TicketNumber = t.TicketNumber,
                    Status = t.Status
                })
                .ToListAsync();

            // Get drink orders
            var drinkOrders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Drink)
                .Where(o => o.Customer.Email == customerEmail)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => new CustomerOrderDto
                {
                    OrderDate = o.CreatedAt,
                    SeatNumber = o.SeatNumber,
                    TotalAmount = o.TotalAmount,
                    Status = o.Status.ToString(),
                    DeliveredAt = o.DeliveredAt,
                    Items = o.OrderItems.Select(oi => new CustomerOrderItemDto
                    {
                        DrinkName = oi.Drink.Name,
                        Quantity = oi.Quantity,
                        UnitPrice = oi.UnitPrice,
                        TotalPrice = oi.TotalPrice
                    }).ToList()
                })
                .ToListAsync();

            if (!ticketPurchases.Any() && !drinkOrders.Any())
            {
                return null;
            }

            // Calculate summary
            var ticketSpending = ticketPurchases.Sum(t => t.Price);
            var drinkSpending = drinkOrders.Sum(o => o.TotalAmount);
            var totalSpent = ticketSpending + drinkSpending;

            var customerName = ticketPurchases.FirstOrDefault()?.EventName ?? "";
            if (string.IsNullOrEmpty(customerName) && ticketPurchases.Any())
            {
                var firstTicket = await _context.Tickets
                    .Where(t => t.CustomerEmail == customerEmail)
                    .FirstOrDefaultAsync();
                customerName = firstTicket?.CustomerName ?? "";
            }

            var summary = new CustomerSpendingSummaryDto
            {
                TotalSpent = totalSpent,
                TicketSpending = ticketSpending,
                DrinkSpending = drinkSpending,
                TotalTickets = ticketPurchases.Count,
                TotalDrinkOrders = drinkOrders.Count,
                TotalEvents = ticketPurchases.Select(t => t.EventName).Distinct().Count(),
                AverageTicketPrice = ticketPurchases.Any() ? ticketSpending / ticketPurchases.Count : 0,
                AverageDrinkOrderValue = drinkOrders.Any() ? drinkSpending / drinkOrders.Count : 0,
                FirstPurchase = ticketPurchases.Concat(drinkOrders.Select(o => new CustomerPurchaseDto { PurchaseDate = o.OrderDate })).Min(p => p.PurchaseDate),
                LastPurchase = ticketPurchases.Concat(drinkOrders.Select(o => new CustomerPurchaseDto { PurchaseDate = o.OrderDate })).Max(p => p.PurchaseDate),
                SpendingByMonth = new Dictionary<string, decimal>(),
                EventAttendanceByType = new Dictionary<string, int>(),
                PreferredSections = ticketPurchases
                    .Where(t => !string.IsNullOrEmpty(t.SectionName))
                    .GroupBy(t => t.SectionName)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return new CustomerSpendingDetailDto
            {
                CustomerEmail = customerEmail,
                CustomerName = customerName,
                TicketPurchases = ticketPurchases,
                DrinkOrders = drinkOrders,
                Summary = summary
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer spending details for {CustomerEmail}", customerEmail);
            throw;
        }
    }

    public async Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync()
    {
        try
        {
            var now = DateTime.UtcNow;
            var thirtyDaysAgo = now.AddDays(-30);

            // Get basic customer metrics
            var totalCustomers = await _context.Tickets
                .Where(t => !string.IsNullOrEmpty(t.CustomerEmail))
                .Select(t => t.CustomerEmail)
                .Distinct()
                .CountAsync();

            var ticketsTotal = await _context.Tickets.ToListAsync();
            var ordersTotal = await _context.Orders.ToListAsync();
            var totalRevenue = ticketsTotal.Sum(t => t.Price) + ordersTotal.Sum(o => o.TotalAmount);

            var totalTicketsSold = await _context.Tickets.CountAsync();
            var totalDrinkOrders = await _context.Orders.CountAsync();

            var averageTicketPrice = totalTicketsSold > 0 
                ? ticketsTotal.Average(t => t.Price) 
                : 0;

            var averageDrinkOrderValue = totalDrinkOrders > 0 
                ? ordersTotal.Average(o => o.TotalAmount) 
                : 0;

            // Active customers last 30 days
            var activeCustomersLast30Days = await _context.Tickets
                .Where(t => t.PurchaseDate >= thirtyDaysAgo && !string.IsNullOrEmpty(t.CustomerEmail))
                .Select(t => t.CustomerEmail)
                .Distinct()
                .CountAsync();

            // New customers last 30 days
            var newCustomersLast30Days = await _context.Tickets
                .Where(t => !string.IsNullOrEmpty(t.CustomerEmail))
                .GroupBy(t => t.CustomerEmail)
                .Where(g => g.Min(t => t.PurchaseDate) >= thirtyDaysAgo)
                .CountAsync();

            // Top spending customer (handle SQLite decimal limitation)
            var topSpenderTickets = await _context.Tickets
                .Where(t => !string.IsNullOrEmpty(t.CustomerEmail))
                .ToListAsync();
                
            var topSpender = topSpenderTickets
                .GroupBy(t => t.CustomerEmail)
                .Select(g => new { Email = g.Key, Amount = g.Sum(t => t.Price) })
                .OrderByDescending(x => x.Amount)
                .FirstOrDefault();

            var topSpenderName = "";
            var topSpenderAmount = 0m;
            
            if (topSpender != null)
            {
                var topCustomer = await _context.Tickets
                    .Where(t => t.CustomerEmail == topSpender.Email)
                    .FirstOrDefaultAsync();
                topSpenderName = topCustomer?.CustomerName ?? topSpender.Email;
                topSpenderAmount = topSpender.Amount;
            }

            return new CustomerAnalyticsSummaryDto
            {
                TotalCustomers = totalCustomers,
                TotalRevenue = totalRevenue,
                AverageCustomerValue = totalCustomers > 0 ? totalRevenue / totalCustomers : 0,
                AverageTicketPrice = averageTicketPrice,
                AverageDrinkOrderValue = averageDrinkOrderValue,
                TotalTicketsSold = totalTicketsSold,
                TotalDrinkOrders = totalDrinkOrders,
                ActiveCustomersLast30Days = activeCustomersLast30Days,
                NewCustomersLast30Days = newCustomersLast30Days,
                RevenueGrowthPercentage = 0, // Can be calculated with historical data
                TopSpendingCustomer = topSpenderName,
                TopSpendingAmount = topSpenderAmount
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer analytics summary");
            throw;
        }
    }

    public async Task<List<CustomerAnalyticsDto>?> GetTopSpendingCustomersAsync(int limit)
    {
        try
        {
            var filter = new CustomerAnalyticsFilterDto
            {
                SortBy = CustomerSortBy.TotalSpent,
                SortDescending = true,
                Page = 1,
                PageSize = limit
            };

            var result = await GetCustomerAnalyticsAsync(filter);
            return result?.Customers;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting top spending customers");
            throw;
        }
    }

    public async Task<Dictionary<string, decimal>?> GetCustomerSpendingTrendsAsync(int days)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days);
            
            var ticketData = await _context.Tickets
                .Where(t => t.PurchaseDate >= startDate)
                .ToListAsync();

            var ticketSpending = ticketData
                .GroupBy(t => t.PurchaseDate.Date)
                .Select(g => new { Date = g.Key, Amount = g.Sum(t => t.Price) })
                .ToList();

            var drinkData = await _context.Orders
                .Where(o => o.CreatedAt >= startDate)
                .ToListAsync();

            var drinkSpending = drinkData
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new { Date = g.Key, Amount = g.Sum(o => o.TotalAmount) })
                .ToList();

            var trends = new Dictionary<string, decimal>();
            
            for (var date = startDate.Date; date <= DateTime.UtcNow.Date; date = date.AddDays(1))
            {
                var ticketAmount = ticketSpending.FirstOrDefault(t => t.Date == date)?.Amount ?? 0;
                var drinkAmount = drinkSpending.FirstOrDefault(d => d.Date == date)?.Amount ?? 0;
                trends[date.ToString("yyyy-MM-dd")] = ticketAmount + drinkAmount;
            }

            return trends;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer spending trends");
            throw;
        }
    }

    public async Task<Dictionary<string, object>?> GetCustomerAcquisitionMetricsAsync(int months)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddMonths(-months);
            
            var acquisitionTickets = await _context.Tickets
                .Where(t => !string.IsNullOrEmpty(t.CustomerEmail) && t.PurchaseDate >= startDate)
                .ToListAsync();

            var acquisitionData = acquisitionTickets
                .GroupBy(t => t.CustomerEmail)
                .Select(g => new { 
                    CustomerEmail = g.Key, 
                    FirstPurchase = g.Min(t => t.PurchaseDate),
                    TotalSpent = g.Sum(t => t.Price)
                })
                .ToList();

            var monthlyAcquisition = acquisitionData
                .GroupBy(c => new { c.FirstPurchase.Year, c.FirstPurchase.Month })
                .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
                .ToDictionary(
                    g => $"{g.Key.Year}-{g.Key.Month:D2}",
                    g => g.Count()
                );

            var averageFirstOrderValue = acquisitionData.Any() 
                ? acquisitionData.Average(c => c.TotalSpent) 
                : 0;

            return new Dictionary<string, object>
            {
                { "MonthlyNewCustomers", monthlyAcquisition },
                { "AverageFirstOrderValue", averageFirstOrderValue },
                { "TotalNewCustomers", acquisitionData.Count }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer acquisition metrics");
            throw;
        }
    }

    public async Task<Dictionary<string, object>?> GetCustomerRetentionAnalysisAsync()
    {
        try
        {
            var allTicketsData = await _context.Tickets
                .Where(t => !string.IsNullOrEmpty(t.CustomerEmail))
                .ToListAsync();

            var allCustomers = allTicketsData
                .GroupBy(t => t.CustomerEmail)
                .Select(g => new
                {
                    CustomerEmail = g.Key,
                    FirstPurchase = g.Min(t => t.PurchaseDate),
                    LastPurchase = g.Max(t => t.PurchaseDate),
                    TotalPurchases = g.Count(),
                    TotalSpent = g.Sum(t => t.Price)
                })
                .ToList();

            var now = DateTime.UtcNow;
            var thirtyDaysAgo = now.AddDays(-30);
            var sixtyDaysAgo = now.AddDays(-60);
            var ninetyDaysAgo = now.AddDays(-90);

            var repeatCustomers = allCustomers.Where(c => c.TotalPurchases > 1).Count();
            var activeCustomers30Days = allCustomers.Where(c => c.LastPurchase >= thirtyDaysAgo).Count();
            var activeCustomers60Days = allCustomers.Where(c => c.LastPurchase >= sixtyDaysAgo).Count();
            var activeCustomers90Days = allCustomers.Where(c => c.LastPurchase >= ninetyDaysAgo).Count();

            var retentionRate30Days = allCustomers.Any() ? (double)activeCustomers30Days / allCustomers.Count : 0;
            var retentionRate60Days = allCustomers.Any() ? (double)activeCustomers60Days / allCustomers.Count : 0;
            var retentionRate90Days = allCustomers.Any() ? (double)activeCustomers90Days / allCustomers.Count : 0;

            return new Dictionary<string, object>
            {
                { "TotalCustomers", allCustomers.Count },
                { "RepeatCustomers", repeatCustomers },
                { "RepeatCustomerRate", allCustomers.Any() ? (double)repeatCustomers / allCustomers.Count : 0 },
                { "ActiveCustomers30Days", activeCustomers30Days },
                { "ActiveCustomers60Days", activeCustomers60Days },
                { "ActiveCustomers90Days", activeCustomers90Days },
                { "RetentionRate30Days", retentionRate30Days },
                { "RetentionRate60Days", retentionRate60Days },
                { "RetentionRate90Days", retentionRate90Days },
                { "AverageCustomerLifetime", allCustomers.Any() ? allCustomers.Average(c => (c.LastPurchase - c.FirstPurchase).Days) : 0 }
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting customer retention analysis");
            throw;
        }
    }

    public async Task<string?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
    {
        try
        {
            var data = await GetCustomerAnalyticsAsync(filter);
            
            if (data?.Customers == null || !data.Customers.Any())
            {
                return null;
            }

            var csv = new StringBuilder();
            
            // Header
            csv.AppendLine("Customer Name,Customer Email,Phone,Total Spent,Tickets Purchased,Events Attended,Drink Orders,Avg Spend Per Transaction,First Purchase,Last Purchase,Preferred Payment Method,Favorite Events,Favorite Sections,Ticket Spending,Drink Spending,Total Transactions");

            // Data rows
            foreach (var customer in data.Customers)
            {
                csv.AppendLine($"\"{customer.CustomerName}\",\"{customer.CustomerEmail}\",\"{customer.CustomerPhone}\",{customer.TotalSpent},{customer.TotalTicketsPurchased},{customer.TotalEventsAttended},{customer.TotalDrinkOrders},{customer.AverageSpendPerTransaction:F2},\"{customer.FirstPurchaseDate:yyyy-MM-dd}\",\"{customer.LastPurchaseDate:yyyy-MM-dd}\",\"{customer.PreferredPaymentMethod}\",\"{string.Join("; ", customer.FavoriteEventTypes)}\",\"{string.Join("; ", customer.FavoriteSections)}\",{customer.TicketSpending},{customer.DrinkSpending},{customer.TotalTransactions}");
            }

            return csv.ToString();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting customer analytics");
            throw;
        }
    }
}