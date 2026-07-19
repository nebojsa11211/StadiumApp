using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Analytics
{
    public interface IAnalyticsService
    {
        Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
        Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync();
        Task<CustomerSpendingDetailDto?> GetCustomerSpendingDetailsAsync(string customerEmail);
        Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter);
    }
}