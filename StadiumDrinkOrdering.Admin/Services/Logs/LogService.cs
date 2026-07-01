using System.Net.Http.Json;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Services;

namespace StadiumDrinkOrdering.Admin.Services.Logs
{
    public class LogService : ILogService
    {
        private readonly HttpClient _httpClient;
        private readonly ICentralizedLoggingClient _loggingClient;

        public LogService(HttpClient httpClient, ICentralizedLoggingClient loggingClient)
        {
            _httpClient = httpClient;
            _loggingClient = loggingClient;
        }

        public async Task<IEnumerable<LogEntryDto>?> GetLogsAsync(LogFilterDto? filterDto = null)
        {
            try
            {
                LogSearchRequestDto request = new LogSearchRequestDto
                {
                    StartDate = filterDto?.FromDate,
                    EndDate = filterDto?.ToDate,
                    Level = filterDto?.Level,
                    Category = filterDto?.Category,
                    UserId = filterDto?.UserId,
                    UserEmail = filterDto?.UserEmail,
                    Source = filterDto?.Source,
                    SearchText = filterDto?.SearchText,
                    Page = filterDto?.Page ?? 1,
                    PageSize = filterDto?.PageSize ?? 50
                };

                var response = await _httpClient.PostAsJsonAsync("logs/search", request);
                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<LogSearchResultDto>();
                    return result?.Logs;
                }
                return null;
            }
            catch (Exception ex)
            {
                await _loggingClient.LogErrorAsync(ex, "GetLogs", "AdminService");
                return null;
            }
        }

        public async Task<LogSummaryDto?> GetLogSummaryAsync()
        {
            return await GetLogSummaryAsync(null, null);
        }

        public async Task<bool> ClearAllLogsAsync()
        {
            try
            {
                var response = await _httpClient.DeleteAsync("logs/clear-old?daysToKeep=0");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await _loggingClient.LogErrorAsync(ex, "ClearAllLogs", "AdminService");
                return false;
            }
        }

        public async Task LogUserActionAsync(string action, string category, int? userId = null, string? userEmail = null, string? details = null)
        {
            await _loggingClient.LogUserActionAsync(action, category, userId?.ToString(), userEmail, details);
        }

        public async Task<LogSearchResultDto?> SearchLogsAsync(LogSearchRequestDto request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("logs/search", request);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LogSearchResultDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                await _loggingClient.LogErrorAsync(ex, "SearchLogs", "AdminService");
                return null;
            }
        }

        public async Task<LogSummaryDto?> GetLogSummaryAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var query = "logs/summary";
                if (startDate.HasValue || endDate.HasValue)
                {
                    var queryParams = new List<string>();
                    if (startDate.HasValue)
                        queryParams.Add($"startDate={startDate.Value:yyyy-MM-dd}");
                    if (endDate.HasValue)
                        queryParams.Add($"endDate={endDate.Value:yyyy-MM-dd}");
                    query += "?" + string.Join("&", queryParams);
                }

                var response = await _httpClient.GetAsync(query);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<LogSummaryDto>();
                }
                return null;
            }
            catch (Exception ex)
            {
                await _loggingClient.LogErrorAsync(ex, "GetLogSummary", "AdminService");
                return null;
            }
        }

        public async Task<bool> ClearOldLogsAsync(int daysToKeep)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"logs/clear-old?daysToKeep={daysToKeep}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                await _loggingClient.LogErrorAsync(ex, "ClearOldLogs", "AdminService");
                return false;
            }
        }
    }
}