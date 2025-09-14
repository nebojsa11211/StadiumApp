using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.Admin.Models
{
    public class ActivityItem
    {
        public string Type { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public DateTime Timestamp { get; set; }
    }

    public class RevenueData
    {
        public decimal TodayRevenue { get; set; }
        public decimal ChangePercentage { get; set; }
    }

    public class OrderStatistics
    {
        public int Active { get; set; }
        public int Pending { get; set; }
        public int InPreparation { get; set; }
    }

    public class TicketStatistics
    {
        public int SoldToday { get; set; }
        public string? CurrentEventName { get; set; }
    }

    public class UserStatistics
    {
        public int TotalOnline { get; set; }
        public int StaffOnline { get; set; }
        public int CustomersOnline { get; set; }
    }

    public class OnlineUser
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public string Role { get; set; } = "";
        public DateTime LastActivity { get; set; }
    }

    public class ActiveOrder
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = "";
        public int ItemCount { get; set; }
        public decimal Total { get; set; }
        public string Status { get; set; } = "";
        public DateTime CreatedAt { get; set; }
    }

    public class RevenueDetails
    {
        public decimal DrinkRevenue { get; set; }
        public decimal TicketRevenue { get; set; }
        public decimal ServiceFees { get; set; }
        public decimal YesterdayTotal { get; set; }
        public decimal LastWeekTotal { get; set; }
        public decimal LastMonthTotal { get; set; }
        public List<TopItem> TopItems { get; set; } = new();
    }

    public class TopItem
    {
        public string Name { get; set; } = "";
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }

    public class TicketDetails
    {
        public string EventName { get; set; } = "";
        public DateTime EventDate { get; set; }
        public int TotalCapacity { get; set; }
        public int SoldToday { get; set; }
        public int TotalSold { get; set; }
        public int Available { get; set; }
        public List<SectionSale> SectionSales { get; set; } = new();
    }

    public class SectionSale
    {
        public string Name { get; set; } = "";
        public int Capacity { get; set; }
        public int Sold { get; set; }
    }
}