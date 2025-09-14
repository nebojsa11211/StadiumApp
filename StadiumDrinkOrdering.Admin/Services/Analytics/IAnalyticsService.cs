using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Analytics
{
    public interface IAnalyticsService
    {
        Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
        Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync();
        Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
    }
}