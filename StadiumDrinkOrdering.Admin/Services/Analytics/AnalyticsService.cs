using StadiumDrinkOrdering.Admin.Services.Base;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;

namespace StadiumDrinkOrdering.Admin.Services.Analytics
{
    public class AnalyticsService : BaseApiService, IAnalyticsService
    {
        public AnalyticsService(HttpClient httpClient, ICentralizedLoggingClient loggingClient, ITokenStorageService tokenStorage)
            : base(httpClient, loggingClient, tokenStorage: tokenStorage)
        {
        }

        public async Task<PagedCustomerAnalyticsDto?> GetCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
        {
            try
            {
                SetAuthorizationHeader();
                var content = CreateJsonContent(filter);
                var response = await HttpClient.PostAsync("CustomerAnalytics/search", content);
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
                SetAuthorizationHeader();
                var response = await HttpClient.GetAsync("CustomerAnalytics/summary");
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

        public async Task<CustomerSpendingDetailDto?> GetCustomerSpendingDetailsAsync(string customerEmail)
        {
            try
            {
                SetAuthorizationHeader();
                var response = await HttpClient.GetAsync(
                    $"CustomerAnalytics/customer/{Uri.EscapeDataString(customerEmail)}/details");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    return DeserializeResponse<CustomerSpendingDetailDto>(json);
                }
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "GetCustomerSpendingDetails",
                    $"Failed to retrieve spending details for {customerEmail}");
            }
            return null;
        }

        public async Task<HttpResponseMessage?> ExportCustomerAnalyticsAsync(CustomerAnalyticsFilterDto filter)
        {
            try
            {
                SetAuthorizationHeader();
                var content = CreateJsonContent(filter);
                return await HttpClient.PostAsync("CustomerAnalytics/export", content);
            }
            catch (Exception ex)
            {
                await LogErrorAsync(ex, "ExportCustomerAnalytics", "Failed to export customer analytics");
            }
            return null;
        }
    }
}