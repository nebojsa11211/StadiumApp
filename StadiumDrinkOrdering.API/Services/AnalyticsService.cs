using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IAnalyticsService
{
    Task<EventAnalyticsResponse> GetEventAnalyticsAsync(int eventId);
    Task<DashboardSummary> GetDashboardSummaryAsync();
    Task<List<OrderTrend>> GetOrderTrendsAsync(int eventId, int hours = 24);
    Task<List<DrinkPopularity>> GetDrinkPopularityAsync(int? eventId = null, int days = 7);
    Task<List<SectionPerformance>> GetSectionPerformanceAsync(int eventId);
    Task<RevenueAnalytics> GetRevenueAnalyticsAsync(int? eventId = null, int days = 30);
    Task<StaffPerformanceAnalytics> GetStaffPerformanceAsync(int? eventId = null, int days = 7);
    Task UpdateEventAnalyticsAsync(int eventId);
}

public class AnalyticsService : IAnalyticsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<AnalyticsService> _logger;

    public AnalyticsService(ApplicationDbContext context, ILogger<AnalyticsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<EventAnalyticsResponse> GetEventAnalyticsAsync(int eventId)
    {
        try
        {
            var analytics = await _context.EventAnalytics
                .Include(ea => ea.Event)
                .FirstOrDefaultAsync(ea => ea.EventId == eventId);

            if (analytics == null)
            {
                // Generate analytics if not exists
                await UpdateEventAnalyticsAsync(eventId);
                analytics = await _context.EventAnalytics
                    .Include(ea => ea.Event)
                    .FirstOrDefaultAsync(ea => ea.EventId == eventId);
            }

            if (analytics == null)
                throw new ArgumentException("Event not found");

            return new EventAnalyticsResponse
            {
                EventId = eventId,
                EventName = analytics.Event.EventName,
                EventDate = analytics.Event.EventDate,
                TotalTicketsSold = analytics.TotalTicketsSold,
                TotalRevenue = analytics.TotalRevenue,
                TicketRevenue = analytics.TicketRevenue,
                DrinksRevenue = analytics.DrinksRevenue,
                TotalOrders = analytics.TotalOrders,
                TotalDrinksSold = analytics.TotalDrinksSold,
                AverageOrderValue = analytics.AverageOrderValue,
                PeakOrderTime = analytics.PeakOrderTime,
                MostPopularDrink = analytics.MostPopularDrink,
                CalculatedAt = analytics.CalculatedAt
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting event analytics for event {EventId}", eventId);
            throw;
        }
    }

    public async Task<DashboardSummary> GetDashboardSummaryAsync()
    {
        try
        {
            var todayUtc = DateTime.UtcNow.Date;
            var thisWeek = todayUtc.AddDays(-(int)todayUtc.DayOfWeek);
            var thisMonth = new DateTime(todayUtc.Year, todayUtc.Month, 1);

            var summary = new DashboardSummary
            {
                TodayOrders = await _context.Orders.CountAsync(o => o.CreatedAt.Date == todayUtc),
                TodayRevenue = await _context.Orders.Where(o => o.CreatedAt.Date == todayUtc).SumAsync(o => o.TotalAmount),
                
                WeeklyOrders = await _context.Orders.CountAsync(o => o.CreatedAt >= thisWeek),
                WeeklyRevenue = await _context.Orders.Where(o => o.CreatedAt >= thisWeek).SumAsync(o => o.TotalAmount),
                
                MonthlyOrders = await _context.Orders.CountAsync(o => o.CreatedAt >= thisMonth),
                MonthlyRevenue = await _context.Orders.Where(o => o.CreatedAt >= thisMonth).SumAsync(o => o.TotalAmount),
                
                ActiveSessions = await _context.OrderSessions.CountAsync(s => s.IsActive && s.ExpiresAt > DateTime.UtcNow),
                
                PendingOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Pending),
                InPreparationOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.InPreparation),
                ReadyOrders = await _context.Orders.CountAsync(o => o.Status == OrderStatus.Ready),
                
                ActiveEvents = await _context.Events.CountAsync(e => e.IsActive && e.EventDate > DateTime.UtcNow),
                StaffOnDuty = await _context.EventStaffAssignments.CountAsync(esa => 
                    esa.IsActive && 
                    esa.ShiftStart <= DateTime.UtcNow && 
                    esa.ShiftEnd >= DateTime.UtcNow),
                
                AverageOrderValue = await _context.Orders.AverageAsync(o => (decimal?)o.TotalAmount) ?? 0,
                
                TopDrinkToday = await GetTopDrinkAsync(todayUtc, todayUtc.AddDays(1))
            };

            return summary;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting dashboard summary");
            throw;
        }
    }

    public async Task<List<OrderTrend>> GetOrderTrendsAsync(int eventId, int hours = 24)
    {
        try
        {
            var startTime = DateTime.UtcNow.AddHours(-hours);
            
            var trends = await _context.Orders
                .Where(o => o.EventId == eventId && o.CreatedAt >= startTime)
                .GroupBy(o => new { 
                    Hour = o.CreatedAt.Hour,
                    Date = o.CreatedAt.Date
                })
                .Select(g => new OrderTrend
                {
                    TimeSlot = g.Key.Date.AddHours(g.Key.Hour),
                    OrderCount = g.Count(),
                    Revenue = g.Sum(o => o.TotalAmount),
                    AverageValue = g.Average(o => o.TotalAmount)
                })
                .OrderBy(t => t.TimeSlot)
                .ToListAsync();

            return trends;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting order trends for event {EventId}", eventId);
            throw;
        }
    }

    public async Task<List<DrinkPopularity>> GetDrinkPopularityAsync(int? eventId = null, int days = 7)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days);
            
            var query = _context.OrderItems
                .Include(oi => oi.Drink)
                .Include(oi => oi.Order)
                .Where(oi => oi.Order.CreatedAt >= startDate);

            if (eventId.HasValue)
            {
                query = query.Where(oi => oi.Order.EventId == eventId);
            }

            var popularity = await query
                .GroupBy(oi => new { oi.DrinkId, oi.Drink.Name })
                .Select(g => new DrinkPopularity
                {
                    DrinkId = g.Key.DrinkId,
                    DrinkName = g.Key.Name,
                    TotalOrdered = g.Sum(oi => oi.Quantity),
                    Revenue = g.Sum(oi => oi.TotalPrice),
                    OrderCount = g.Count()
                })
                .OrderByDescending(dp => dp.TotalOrdered)
                .Take(10)
                .ToListAsync();

            return popularity;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting drink popularity");
            throw;
        }
    }

    public async Task<List<SectionPerformance>> GetSectionPerformanceAsync(int eventId)
    {
        try
        {
            var performance = await _context.Orders
                .Where(o => o.EventId == eventId)
                .GroupBy(o => o.SeatNumber.Substring(0, 1)) // Extract section letter
                .Select(g => new SectionPerformance
                {
                    SectionName = g.Key,
                    OrderCount = g.Count(),
                    Revenue = g.Sum(o => o.TotalAmount),
                    AverageOrderValue = g.Average(o => o.TotalAmount),
                    CustomerCount = g.Select(o => o.CustomerId).Distinct().Count()
                })
                .OrderByDescending(sp => sp.Revenue)
                .ToListAsync();

            return performance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting section performance for event {EventId}", eventId);
            throw;
        }
    }

    public async Task<RevenueAnalytics> GetRevenueAnalyticsAsync(int? eventId = null, int days = 30)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days);
            
            var query = _context.Orders.Where(o => o.CreatedAt >= startDate);
            if (eventId.HasValue)
            {
                query = query.Where(o => o.EventId == eventId);
            }

            var orders = await query.ToListAsync();
            
            var dailyRevenue = orders
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new DailyRevenue
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.TotalAmount),
                    OrderCount = g.Count()
                })
                .OrderBy(dr => dr.Date)
                .ToList();

            var analytics = new RevenueAnalytics
            {
                TotalRevenue = orders.Sum(o => o.TotalAmount),
                AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0,
                TotalOrders = orders.Count,
                DailyRevenue = dailyRevenue,
                PeakDay = dailyRevenue.OrderByDescending(dr => dr.Revenue).FirstOrDefault()?.Date ?? DateTime.UtcNow.Date,
                LowestDay = dailyRevenue.OrderBy(dr => dr.Revenue).FirstOrDefault()?.Date ?? DateTime.UtcNow.Date
            };

            return analytics;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting revenue analytics");
            throw;
        }
    }

    public async Task<StaffPerformanceAnalytics> GetStaffPerformanceAsync(int? eventId = null, int days = 7)
    {
        try
        {
            var startDate = DateTime.UtcNow.AddDays(-days);
            
            var query = _context.Orders
                .Include(o => o.AcceptedByUser)
                .Include(o => o.DeliveredByUser)
                .Where(o => o.CreatedAt >= startDate);

            if (eventId.HasValue)
            {
                query = query.Where(o => o.EventId == eventId);
            }

            var orders = await query.ToListAsync();

            var staffPerformance = new StaffPerformanceAnalytics
            {
                TopAcceptingStaff = orders
                    .Where(o => o.AcceptedByUser != null)
                    .GroupBy(o => new { o.AcceptedByUserId, o.AcceptedByUser!.Username })
                    .Select(g => new StaffMetric
                    {
                        StaffId = g.Key.AcceptedByUserId ?? 0,
                        StaffName = g.Key.Username,
                        Count = g.Count(),
                        AverageTime = TimeSpan.FromMinutes(15) // Placeholder
                    })
                    .OrderByDescending(sm => sm.Count)
                    .Take(5)
                    .ToList(),

                TopDeliveringStaff = orders
                    .Where(o => o.DeliveredByUser != null)
                    .GroupBy(o => new { o.DeliveredByUserId, o.DeliveredByUser!.Username })
                    .Select(g => new StaffMetric
                    {
                        StaffId = g.Key.DeliveredByUserId ?? 0,
                        StaffName = g.Key.Username,
                        Count = g.Count(),
                        AverageTime = TimeSpan.FromMinutes(25) // Placeholder
                    })
                    .OrderByDescending(sm => sm.Count)
                    .Take(5)
                    .ToList(),

                AverageAcceptanceTime = TimeSpan.FromMinutes(3),
                AverageDeliveryTime = TimeSpan.FromMinutes(12)
            };

            return staffPerformance;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting staff performance analytics");
            throw;
        }
    }

    public async Task UpdateEventAnalyticsAsync(int eventId)
    {
        try
        {
            var evt = await _context.Events.FindAsync(eventId);
            if (evt == null) return;

            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Drink)
                .Where(o => o.EventId == eventId)
                .ToListAsync();

            var tickets = await _context.Tickets
                .Where(t => t.EventId == eventId && !string.IsNullOrEmpty(t.CustomerName))
                .ToListAsync();

            var existingAnalytics = await _context.EventAnalytics.FirstOrDefaultAsync(ea => ea.EventId == eventId);

            var analytics = existingAnalytics ?? new EventAnalytics { EventId = eventId };

            analytics.TotalTicketsSold = tickets.Count;
            
            // Calculate ticket revenue (use individual ticket prices, fallback to event base price * count)
            var ticketRevenue = tickets.Sum(t => t.Price > 0 ? t.Price : (evt.BaseTicketPrice ?? 0m));
            
            // Calculate drinks revenue from orders
            var drinksRevenue = orders.Sum(o => o.TotalAmount);
            
            analytics.TicketRevenue = ticketRevenue;
            analytics.DrinksRevenue = drinksRevenue;
            analytics.TotalRevenue = ticketRevenue + drinksRevenue;
            analytics.TotalOrders = orders.Count;
            analytics.TotalDrinksSold = orders.SelectMany(o => o.OrderItems).Sum(oi => oi.Quantity);
            analytics.AverageOrderValue = orders.Any() ? orders.Average(o => o.TotalAmount) : 0;
            analytics.PeakOrderTime = orders.Any() ? 
                orders.GroupBy(o => o.CreatedAt.Hour)
                     .OrderByDescending(g => g.Count())
                     .Select(g => DateTime.UtcNow.Date.AddHours(g.Key))
                     .FirstOrDefault() : null;
            analytics.MostPopularDrink = orders.SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.Drink.Name)
                .OrderByDescending(g => g.Sum(oi => oi.Quantity))
                .FirstOrDefault()?.Key ?? "N/A";
            analytics.CalculatedAt = DateTime.UtcNow;

            if (existingAnalytics == null)
            {
                _context.EventAnalytics.Add(analytics);
            }

            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event analytics for event {EventId}", eventId);
        }
    }

    private async Task<string> GetTopDrinkAsync(DateTime startDate, DateTime endDate)
    {
        var topDrink = await _context.OrderItems
            .Include(oi => oi.Drink)
            .Include(oi => oi.Order)
            .Where(oi => oi.Order.CreatedAt >= startDate && oi.Order.CreatedAt < endDate)
            .GroupBy(oi => oi.Drink.Name)
            .OrderByDescending(g => g.Sum(oi => oi.Quantity))
            .Select(g => g.Key)
            .FirstOrDefaultAsync();

        return topDrink ?? "N/A";
    }
}

// Response DTOs
public class EventAnalyticsResponse
{
    public int EventId { get; set; }
    public string EventName { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public int TotalTicketsSold { get; set; }
    public decimal TotalRevenue { get; set; }
    public decimal TicketRevenue { get; set; }
    public decimal DrinksRevenue { get; set; }
    public int TotalOrders { get; set; }
    public int TotalDrinksSold { get; set; }
    public decimal AverageOrderValue { get; set; }
    public DateTime? PeakOrderTime { get; set; }
    public string? MostPopularDrink { get; set; }
    public DateTime CalculatedAt { get; set; }
}

public class DashboardSummary
{
    public int TodayOrders { get; set; }
    public decimal TodayRevenue { get; set; }
    public int WeeklyOrders { get; set; }
    public decimal WeeklyRevenue { get; set; }
    public int MonthlyOrders { get; set; }
    public decimal MonthlyRevenue { get; set; }
    public int ActiveSessions { get; set; }
    public int PendingOrders { get; set; }
    public int InPreparationOrders { get; set; }
    public int ReadyOrders { get; set; }
    public int ActiveEvents { get; set; }
    public int StaffOnDuty { get; set; }
    public decimal AverageOrderValue { get; set; }
    public string TopDrinkToday { get; set; } = string.Empty;
}

public class OrderTrend
{
    public DateTime TimeSlot { get; set; }
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
    public decimal AverageValue { get; set; }
}

public class DrinkPopularity
{
    public int DrinkId { get; set; }
    public string DrinkName { get; set; } = string.Empty;
    public int TotalOrdered { get; set; }
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
}

public class SectionPerformance
{
    public string SectionName { get; set; } = string.Empty;
    public int OrderCount { get; set; }
    public decimal Revenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int CustomerCount { get; set; }
}

public class RevenueAnalytics
{
    public decimal TotalRevenue { get; set; }
    public decimal AverageOrderValue { get; set; }
    public int TotalOrders { get; set; }
    public List<DailyRevenue> DailyRevenue { get; set; } = new();
    public DateTime PeakDay { get; set; }
    public DateTime LowestDay { get; set; }
}

public class DailyRevenue
{
    public DateTime Date { get; set; }
    public decimal Revenue { get; set; }
    public int OrderCount { get; set; }
}

public class StaffPerformanceAnalytics
{
    public List<StaffMetric> TopAcceptingStaff { get; set; } = new();
    public List<StaffMetric> TopDeliveringStaff { get; set; } = new();
    public TimeSpan AverageAcceptanceTime { get; set; }
    public TimeSpan AverageDeliveryTime { get; set; }
}

public class StaffMetric
{
    public int StaffId { get; set; }
    public string StaffName { get; set; } = string.Empty;
    public int Count { get; set; }
    public TimeSpan AverageTime { get; set; }
}