using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.API.Services;

public interface ICustomerAnalyticsService
{
    Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
    Task<CustomerSpendingDetailDto?> GetCustomerSpendingDetailsAsync(string customerEmail);
    Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync();
    Task<List<CustomerAnalyticsDto>?> GetTopSpendingCustomersAsync(int limit);
    Task<Dictionary<string, decimal>?> GetCustomerSpendingTrendsAsync(int days);
    Task<Dictionary<string, object>?> GetCustomerAcquisitionMetricsAsync(int months);
    Task<Dictionary<string, object>?> GetCustomerRetentionAnalysisAsync();
    Task<string?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
}