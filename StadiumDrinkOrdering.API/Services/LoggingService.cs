using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using System.Text.Json;
using System.Security.Claims;

namespace StadiumDrinkOrdering.API.Services
{
    public interface ILoggingService
    {
        Task LogUserActionAsync(string action, string category, string? userId = null, string? userEmail = null, 
            string? userRole = null, string? details = null, string? requestPath = null, 
            string? httpMethod = null, string? ipAddress = null, string? userAgent = null, string source = "API");
        
        Task LogErrorAsync(Exception exception, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string? ipAddress = null, 
            string? userAgent = null, string source = "API");
        
        Task LogInfoAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string? ipAddress = null, 
            string? userAgent = null, string source = "API");
        
        Task LogWarningAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string? ipAddress = null, 
            string? userAgent = null, string source = "API");
        
        Task LogBusinessEventAsync(LogUserActionRequest request);
        Task<PagedLogsDto> GetLogsAsync(LogFilterDto filter);
        Task<LogSummaryDto> GetLogSummaryAsync();
        Task<bool> ClearOldLogsAsync(int daysToKeep = 30);
        Task<bool> ClearAllLogsAsync();
    }

    public class LoggingService : ILoggingService
    {
        private readonly ApplicationDbContext _context;

        public LoggingService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task LogUserActionAsync(string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string? ipAddress = null, 
            string? userAgent = null, string source = "API")
        {
            await CreateLogEntryAsync(CustomLogLevel.Info.ToString(), category, action, userId, userEmail, userRole, 
                null, details, requestPath, httpMethod, ipAddress, userAgent, source);
        }

        public async Task LogErrorAsync(Exception exception, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string? ipAddress = null, 
            string? userAgent = null, string source = "API")
        {
            await CreateLogEntryAsync(CustomLogLevel.Error.ToString(), category, action, userId, userEmail, userRole, 
                exception.Message, details, requestPath, httpMethod, ipAddress, userAgent, source, 
                exception.GetType().Name, exception.StackTrace);
        }

        public async Task LogInfoAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string? ipAddress = null, 
            string? userAgent = null, string source = "API")
        {
            await CreateLogEntryAsync(CustomLogLevel.Info.ToString(), category, action, userId, userEmail, userRole, 
                message, details, requestPath, httpMethod, ipAddress, userAgent, source);
        }

        public async Task LogWarningAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string? ipAddress = null, 
            string? userAgent = null, string source = "API")
        {
            await CreateLogEntryAsync(CustomLogLevel.Warning.ToString(), category, action, userId, userEmail, userRole, 
                message, details, requestPath, httpMethod, ipAddress, userAgent, source);
        }

        public async Task LogBusinessEventAsync(LogUserActionRequest request)
        {
            await CreateLogEntryAsync(
                level: CustomLogLevel.Info.ToString(),
                category: request.Category,
                action: request.Action,
                userId: request.UserId,
                userEmail: request.UserEmail,
                userRole: request.UserRole,
                message: null,
                details: request.Details,
                requestPath: request.RequestPath,
                httpMethod: request.HttpMethod,
                ipAddress: null,
                userAgent: null,
                source: request.Source ?? "API",
                businessEntityType: request.BusinessEntityType,
                businessEntityId: request.BusinessEntityId,
                businessEntityName: request.BusinessEntityName,
                relatedEntityType: request.RelatedEntityType,
                relatedEntityId: request.RelatedEntityId,
                monetaryAmount: request.MonetaryAmount,
                currency: request.Currency,
                quantity: request.Quantity,
                locationInfo: request.LocationInfo,
                statusBefore: request.StatusBefore,
                statusAfter: request.StatusAfter,
                metadataJson: request.MetadataJson
            );
        }

        private async Task CreateLogEntryAsync(string level, string category, string action, string? userId, 
            string? userEmail, string? userRole, string? message, string? details, string? requestPath, 
            string? httpMethod, string? ipAddress, string? userAgent, string source, 
            string? exceptionType = null, string? stackTrace = null,
            string? businessEntityType = null, string? businessEntityId = null, string? businessEntityName = null,
            string? relatedEntityType = null, string? relatedEntityId = null, decimal? monetaryAmount = null,
            string? currency = null, int? quantity = null, string? locationInfo = null,
            string? statusBefore = null, string? statusAfter = null, string? metadataJson = null)
        {
            try
            {
                var logEntry = new LogEntry
                {
                    Timestamp = DateTime.UtcNow,
                    Level = level,
                    Category = category,
                    Action = action,
                    UserId = userId,
                    UserEmail = userEmail,
                    UserRole = userRole,
                    IPAddress = ipAddress,
                    UserAgent = userAgent,
                    RequestPath = requestPath,
                    HttpMethod = httpMethod,
                    Message = message,
                    Details = details,
                    ExceptionType = exceptionType,
                    StackTrace = stackTrace,
                    Source = source,
                    // Business Event Fields
                    BusinessEntityType = businessEntityType,
                    BusinessEntityId = businessEntityId,
                    BusinessEntityName = businessEntityName,
                    RelatedEntityType = relatedEntityType,
                    RelatedEntityId = relatedEntityId,
                    MonetaryAmount = monetaryAmount,
                    Currency = currency,
                    Quantity = quantity,
                    LocationInfo = locationInfo,
                    StatusBefore = statusBefore,
                    StatusAfter = statusAfter,
                    MetadataJson = metadataJson
                };

                _context.LogEntries.Add(logEntry);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log to console if database logging fails to avoid infinite loops
                Console.WriteLine($"Failed to log to database: {ex.Message}");
            }
        }

        public async Task<PagedLogsDto> GetLogsAsync(LogFilterDto filter)
        {
            var query = _context.LogEntries.AsQueryable();

            // Apply filters
            if (filter.FromDate.HasValue)
                query = query.Where(l => l.Timestamp >= filter.FromDate.Value);

            if (filter.ToDate.HasValue)
                query = query.Where(l => l.Timestamp <= filter.ToDate.Value);

            if (!string.IsNullOrEmpty(filter.Level))
                query = query.Where(l => l.Level == filter.Level);

            if (!string.IsNullOrEmpty(filter.Category))
                query = query.Where(l => l.Category == filter.Category);

            if (!string.IsNullOrEmpty(filter.UserId))
                query = query.Where(l => l.UserId == filter.UserId);

            if (!string.IsNullOrEmpty(filter.UserEmail))
                query = query.Where(l => l.UserEmail!.Contains(filter.UserEmail));

            if (!string.IsNullOrEmpty(filter.Source))
                query = query.Where(l => l.Source == filter.Source);

            if (!string.IsNullOrEmpty(filter.SearchText))
            {
                query = query.Where(l => 
                    l.Message!.Contains(filter.SearchText) || 
                    l.Action.Contains(filter.SearchText) ||
                    l.Details!.Contains(filter.SearchText));
            }

            var totalCount = await query.CountAsync();

            var logs = await query
                .OrderByDescending(l => l.Timestamp)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .Select(l => new LogEntryDto
                {
                    Id = l.Id,
                    Timestamp = l.Timestamp,
                    Level = l.Level,
                    Category = l.Category,
                    Action = l.Action,
                    UserId = l.UserId,
                    UserEmail = l.UserEmail,
                    UserRole = l.UserRole,
                    IPAddress = l.IPAddress,
                    UserAgent = l.UserAgent,
                    RequestPath = l.RequestPath,
                    HttpMethod = l.HttpMethod,
                    Message = l.Message,
                    Details = l.Details,
                    ExceptionType = l.ExceptionType,
                    StackTrace = l.StackTrace,
                    Source = l.Source,
                    // Business Event Fields
                    BusinessEntityType = l.BusinessEntityType,
                    BusinessEntityId = l.BusinessEntityId,
                    BusinessEntityName = l.BusinessEntityName,
                    RelatedEntityType = l.RelatedEntityType,
                    RelatedEntityId = l.RelatedEntityId,
                    MonetaryAmount = l.MonetaryAmount,
                    Currency = l.Currency,
                    Quantity = l.Quantity,
                    LocationInfo = l.LocationInfo,
                    StatusBefore = l.StatusBefore,
                    StatusAfter = l.StatusAfter,
                    MetadataJson = l.MetadataJson
                })
                .ToListAsync();

            return new PagedLogsDto
            {
                Logs = logs,
                TotalCount = totalCount,
                Page = filter.Page,
                PageSize = filter.PageSize
            };
        }

        public async Task<LogSummaryDto> GetLogSummaryAsync()
        {
            var summary = new LogSummaryDto();

            var allLogs = await _context.LogEntries.ToListAsync();
            
            summary.TotalLogs = allLogs.Count;
            summary.ErrorCount = allLogs.Count(l => l.Level == CustomLogLevel.Error.ToString());
            summary.WarningCount = allLogs.Count(l => l.Level == CustomLogLevel.Warning.ToString());
            summary.InfoCount = allLogs.Count(l => l.Level == CustomLogLevel.Info.ToString());
            summary.CriticalCount = allLogs.Count(l => l.Level == CustomLogLevel.Critical.ToString());
            summary.LastLogTime = allLogs.OrderByDescending(l => l.Timestamp).FirstOrDefault()?.Timestamp;

            summary.LogsBySource = allLogs
                .GroupBy(l => l.Source)
                .ToDictionary(g => g.Key, g => g.Count());

            summary.LogsByCategory = allLogs
                .GroupBy(l => l.Category)
                .ToDictionary(g => g.Key, g => g.Count());

            return summary;
        }

        public async Task<bool> ClearOldLogsAsync(int daysToKeep = 30)
        {
            try
            {
                var cutoffDate = DateTime.UtcNow.AddDays(-daysToKeep);
                var oldLogs = await _context.LogEntries
                    .Where(l => l.Timestamp < cutoffDate)
                    .ToListAsync();

                if (oldLogs.Any())
                {
                    _context.LogEntries.RemoveRange(oldLogs);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to clear old logs: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> ClearAllLogsAsync()
        {
            try
            {
                var allLogs = await _context.LogEntries.ToListAsync();

                if (allLogs.Any())
                {
                    _context.LogEntries.RemoveRange(allLogs);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Successfully cleared all {allLogs.Count} log entries");
                }
                else
                {
                    Console.WriteLine("No logs found to clear");
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to clear all logs: {ex.Message}");
                return false;
            }
        }
    }

    public static class LoggingExtensions
    {
        public static string? GetUserIdFromClaims(this ClaimsPrincipal? user)
        {
            return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        public static string? GetUserEmailFromClaims(this ClaimsPrincipal? user)
        {
            return user?.FindFirst(ClaimTypes.Email)?.Value;
        }

        public static string? GetUserRoleFromClaims(this ClaimsPrincipal? user)
        {
            return user?.FindFirst(ClaimTypes.Role)?.Value;
        }
    }
}