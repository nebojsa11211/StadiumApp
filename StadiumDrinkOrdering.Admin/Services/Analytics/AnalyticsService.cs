using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.Analytics
{
    public class AnalyticsService : BaseApiService, IAnalyticsService
    {
        public AnalyticsService(HttpClient httpClient, ICentralizedLoggingClient loggingClient)
            : base(httpClient, loggingClient)
        {
        }

        public async Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
        {
            try
            {
                var content = CreateJsonContent(filter);
                var response = await HttpClient.PostAsync("analytics/customers", content);
                if (response.IsSuccessStatusCode)
                {
                    var responseJson = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<PagedCustomerAnalyticsDto>(responseJson);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetCustomerAnalytics", "Failed to retrieve customer analytics");
            }
            return null;
        }

        public async Task<CustomerAnalyticsSummaryDto?> GetCustomerAnalyticsSummaryAsync()
        {
            try
            {
                var response = await HttpClient.GetAsync("analytics/customers/summary");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<CustomerAnalyticsSummaryDto>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetCustomerAnalyticsSummary",
                    "Failed to retrieve customer analytics summary");
            }
            return null;
        }

        public async Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
        {
            try
            {
                var content = CreateJsonContent(filter);
                return await HttpClient.PostAsync("analytics/customers/export", content);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ExportCustomerAnalytics", "Failed to export customer analytics");
            }
            return null;
        }
    }
}