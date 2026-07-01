using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services.Logs
{
    public interface ILogService
    {
        Task<IEnumerable<LogEntryDto>?> GetLogsAsync(LogFilterDto? filterDto = null);
        Task<LogSummaryDto?> GetLogSummaryAsync();
        Task<bool> ClearAllLogsAsync();
        Task LogUserActionAsync(string action, string category, int? userId = null, string? userEmail = null, string? details = null);

        // Additional methods for enhanced functionality
        Task<LogSearchResultDto?> SearchLogsAsync(LogSearchRequestDto request);
        Task<LogSummaryDto?> GetLogSummaryAsync(DateTime? startDate, DateTime? endDate);
        Task<bool> ClearOldLogsAsync(int daysToKeep);
    }
}