using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;

namespace StadiumDrinkOrdering.API.Services
{
    public class LogRetentionBackgroundService : BackgroundService
    {
        private readonly ILogger<LogRetentionBackgroundService> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromHours(24); // Run daily
        private readonly int _defaultRetentionDays = 30;

        public LogRetentionBackgroundService(ILogger<LogRetentionBackgroundService> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Log Retention Background Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PerformLogRetentionAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred while performing log retention");
                }

                // Wait for the next interval
                await Task.Delay(_interval, stoppingToken);
            }
        }

        private async Task PerformLogRetentionAsync()
        {
            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

            try
            {
                var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
                var retentionDays = configuration.GetValue("LogRetention:Days", _defaultRetentionDays);
                var cutoffDate = DateTime.UtcNow.AddDays(-retentionDays);

                _logger.LogInformation($"Starting log retention cleanup. Removing logs older than {cutoffDate}");

                // Get count of logs to be deleted
                var logsToDeleteCount = await context.LogEntries
                    .Where(l => l.Timestamp < cutoffDate)
                    .CountAsync();

                if (logsToDeleteCount > 0)
                {
                    // Archive critical logs before deletion (optional)
                    await ArchiveCriticalLogsAsync(context, cutoffDate);

                    // Delete old logs
                    var deletedCount = await context.Database.ExecuteSqlRawAsync(
                        "DELETE FROM LogEntries WHERE Timestamp < {0}", cutoffDate);

                    _logger.LogInformation($"Log retention cleanup completed. Deleted {deletedCount} old log entries");

                    // Log this maintenance action
                    await loggingService.LogInfoAsync(
                        message: $"Automated log retention cleanup completed",
                        action: "LogRetentionCleanup",
                        category: "System",
                        details: $"Deleted {deletedCount} logs older than {retentionDays} days",
                        source: "LogRetentionService"
                    );
                }
                else
                {
                    _logger.LogInformation("No old logs to clean up");
                }

                // Vacuum SQLite database to reclaim space (if using SQLite)
                await OptimizeDatabaseAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to perform log retention cleanup");
                
                // Try to log this error (if logging service is available)
                try
                {
                    await loggingService.LogErrorAsync(
                        exception: ex,
                        action: "LogRetentionCleanup",
                        category: "System",
                        details: "Failed to perform automated log retention cleanup",
                        source: "LogRetentionService"
                    );
                }
                catch (Exception loggingEx)
                {
                    _logger.LogError(loggingEx, "Failed to log retention cleanup error");
                }
            }
        }

        private async Task ArchiveCriticalLogsAsync(ApplicationDbContext context, DateTime cutoffDate)
        {
            try
            {
                // Archive critical errors and security events before deletion
                var criticalLogs = await context.LogEntries
                    .Where(l => l.Timestamp < cutoffDate && 
                               (l.Level == "Error" || l.Level == "Critical" || l.Category == "Security"))
                    .Select(l => new
                    {
                        l.Timestamp,
                        l.Level,
                        l.Category,
                        l.Action,
                        l.Message,
                        l.Details,
                        l.UserEmail,
                        l.Source
                    })
                    .ToListAsync();

                if (criticalLogs.Any())
                {
                    // Simple file-based archiving (can be enhanced to use cloud storage)
                    var archiveFile = $"critical_logs_archive_{DateTime.UtcNow:yyyyMMdd}.json";
                    var archivePath = Path.Combine("logs", "archives");
                    Directory.CreateDirectory(archivePath);

                    var archiveFilePath = Path.Combine(archivePath, archiveFile);
                    var json = System.Text.Json.JsonSerializer.Serialize(criticalLogs, new System.Text.Json.JsonSerializerOptions
                    {
                        WriteIndented = true
                    });

                    await File.WriteAllTextAsync(archiveFilePath, json);
                    _logger.LogInformation($"Archived {criticalLogs.Count} critical logs to {archiveFilePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to archive critical logs, proceeding with deletion");
            }
        }

        private async Task OptimizeDatabaseAsync(ApplicationDbContext context)
        {
            try
            {
                // For SQLite, run VACUUM to reclaim space
                if (context.Database.IsSqlite())
                {
                    await context.Database.ExecuteSqlRawAsync("VACUUM;");
                    _logger.LogInformation("Database optimization (VACUUM) completed");
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to optimize database");
            }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Log Retention Background Service is stopping");
            await base.StopAsync(stoppingToken);
        }
    }
}