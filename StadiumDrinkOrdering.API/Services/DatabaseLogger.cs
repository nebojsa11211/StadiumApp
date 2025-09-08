using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Text;
using System.Text.Json;

namespace StadiumDrinkOrdering.API.Services
{
    public class DatabaseLogger : ILogger
    {
        private readonly string _categoryName;
        private readonly IServiceProvider _serviceProvider;
        private readonly DatabaseLoggerConfiguration _configuration;
        private static readonly ConcurrentQueue<LogEntry> _logQueue = new();
        private static readonly SemaphoreSlim _queueSemaphore = new(1, 1);
        private static Timer? _batchTimer;
        private static readonly object _timerLock = new();
        private const int BatchSize = 10;
        private const int BatchIntervalMs = 5000;

        public DatabaseLogger(string categoryName, IServiceProvider serviceProvider, DatabaseLoggerConfiguration configuration)
        {
            _categoryName = categoryName;
            _serviceProvider = serviceProvider;
            _configuration = configuration;

            // Initialize the batch timer if not already initialized
            lock (_timerLock)
            {
                if (_batchTimer == null && _configuration.BatchingEnabled)
                {
                    _batchTimer = new Timer(ProcessBatchTimerCallback, serviceProvider, BatchIntervalMs, BatchIntervalMs);
                }
            }
        }

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull
        {
            return null;
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            if (!_configuration.IsEnabled)
                return false;

            // Check if category is excluded
            if (_configuration.ExcludeCategories.Any(excludedCategory => 
                _categoryName.StartsWith(excludedCategory, StringComparison.OrdinalIgnoreCase)))
            {
                return false;
            }

            return logLevel >= _configuration.MinimumLevel;
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
        {
            if (!IsEnabled(logLevel))
                return;

            var message = formatter(state, exception);
            
            // Always write to console for immediate feedback
            WriteToConsole(logLevel, message, exception);

            // Determine category and action
            var category = DetermineCategory(_categoryName, logLevel, exception);
            var action = DetermineAction(eventId, message, _categoryName);

            var logEntry = new LogEntry
            {
                CategoryName = _categoryName,
                LogLevel = logLevel,
                Message = message,
                Exception = exception,
                Timestamp = DateTime.UtcNow,
                Category = category,
                Action = action,
                EventId = eventId.Id,
                EventName = eventId.Name
            };

            // Critical logs and errors should be sent immediately
            var isCritical = _configuration.CriticalCategories.Any(criticalCategory =>
                category.Contains(criticalCategory, StringComparison.OrdinalIgnoreCase)) ||
                logLevel >= LogLevel.Error;

            if (isCritical || !_configuration.BatchingEnabled)
            {
                Task.Run(async () => await SendLogEntryAsync(logEntry));
            }
            else
            {
                // Queue for batch processing
                _logQueue.Enqueue(logEntry);
                
                // Process immediately if queue is full
                if (_logQueue.Count >= BatchSize)
                {
                    Task.Run(async () => await ProcessBatchAsync());
                }
            }
        }

        private void WriteToConsole(LogLevel logLevel, string message, Exception? exception)
        {
            var color = GetLogLevelConsoleColor(logLevel);
            var logLevelString = GetLogLevelString(logLevel);
            
            Console.ForegroundColor = color;
            Console.Write($"[{DateTime.Now:HH:mm:ss} {logLevelString}] ");
            Console.ResetColor();
            
            Console.WriteLine($"{_categoryName}: {message}");
            
            if (exception != null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Exception: {exception}");
                Console.ResetColor();
            }
        }

        private static ConsoleColor GetLogLevelConsoleColor(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Critical => ConsoleColor.Magenta,
                LogLevel.Error => ConsoleColor.Red,
                LogLevel.Warning => ConsoleColor.Yellow,
                LogLevel.Information => ConsoleColor.Green,
                LogLevel.Debug => ConsoleColor.Gray,
                LogLevel.Trace => ConsoleColor.DarkGray,
                _ => ConsoleColor.White
            };
        }

        private static string GetLogLevelString(LogLevel logLevel)
        {
            return logLevel switch
            {
                LogLevel.Critical => "CRIT",
                LogLevel.Error => "ERRO",
                LogLevel.Warning => "WARN",
                LogLevel.Information => "INFO",
                LogLevel.Debug => "DBUG",
                LogLevel.Trace => "TRCE",
                _ => "NONE"
            };
        }

        private string DetermineCategory(string categoryName, LogLevel logLevel, Exception? exception)
        {
            if (exception != null)
                return "SystemError";

            if (categoryName.Contains("Auth", StringComparison.OrdinalIgnoreCase))
                return "Authentication";

            if (categoryName.Contains("Security", StringComparison.OrdinalIgnoreCase))
                return "Security";

            if (categoryName.Contains("Payment", StringComparison.OrdinalIgnoreCase))
                return "Payment";

            if (categoryName.Contains("Order", StringComparison.OrdinalIgnoreCase))
                return "Orders";

            if (categoryName.Contains("Database", StringComparison.OrdinalIgnoreCase))
                return "Database";

            if (categoryName.Contains("SignalR", StringComparison.OrdinalIgnoreCase) || 
                categoryName.Contains("Hub", StringComparison.OrdinalIgnoreCase))
                return "RealTime";

            return logLevel switch
            {
                LogLevel.Error or LogLevel.Critical => "SystemError",
                LogLevel.Warning => "SystemWarning",
                _ => "System"
            };
        }

        private string DetermineAction(EventId eventId, string message, string categoryName)
        {
            if (!string.IsNullOrEmpty(eventId.Name))
                return eventId.Name;

            // Extract action from message patterns
            if (message.Contains("started", StringComparison.OrdinalIgnoreCase))
                return "Started";
            if (message.Contains("completed", StringComparison.OrdinalIgnoreCase))
                return "Completed";
            if (message.Contains("failed", StringComparison.OrdinalIgnoreCase))
                return "Failed";
            if (message.Contains("executing", StringComparison.OrdinalIgnoreCase))
                return "Executing";

            // Extract action from category name
            var parts = categoryName.Split('.');
            if (parts.Length > 0)
            {
                var lastPart = parts[^1];
                if (lastPart.EndsWith("Controller"))
                    return lastPart.Replace("Controller", "");
                if (lastPart.EndsWith("Service"))
                    return lastPart.Replace("Service", "");
                return lastPart;
            }

            return "SystemLog";
        }

        private async Task SendLogEntryAsync(LogEntry logEntry)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var loggingService = scope.ServiceProvider.GetService<ILoggingService>();
                
                if (loggingService == null)
                    return;

                if (logEntry.Exception != null)
                {
                    await loggingService.LogErrorAsync(
                        logEntry.Exception,
                        logEntry.Action,
                        logEntry.Category,
                        details: logEntry.Message,
                        source: "API-Logger"
                    );
                }
                else
                {
                    switch (logEntry.LogLevel)
                    {
                        case LogLevel.Warning:
                            await loggingService.LogWarningAsync(
                                logEntry.Message,
                                logEntry.Action,
                                logEntry.Category,
                                source: "API-Logger"
                            );
                            break;
                        case LogLevel.Error:
                        case LogLevel.Critical:
                            await loggingService.LogErrorAsync(
                                new Exception(logEntry.Message),
                                logEntry.Action,
                                logEntry.Category,
                                source: "API-Logger"
                            );
                            break;
                        default:
                            await loggingService.LogInfoAsync(
                                logEntry.Message,
                                logEntry.Action,
                                logEntry.Category,
                                source: "API-Logger"
                            );
                            break;
                    }
                }
            }
            catch
            {
                // Silently fail to avoid recursive logging issues
            }
        }

        private static async Task ProcessBatchAsync()
        {
            await _queueSemaphore.WaitAsync();
            try
            {
                if (_logQueue.IsEmpty)
                    return;

                var batch = new List<LogEntry>();
                while (batch.Count < BatchSize && _logQueue.TryDequeue(out var logEntry))
                {
                    batch.Add(logEntry);
                }

                if (batch.Count > 0)
                {
                    // Send batch to database
                    // Note: ServiceProvider is passed through timer state
                    // This is handled in ProcessBatchTimerCallback
                }
            }
            finally
            {
                _queueSemaphore.Release();
            }
        }

        private static async void ProcessBatchTimerCallback(object? state)
        {
            if (state is not IServiceProvider serviceProvider)
                return;

            await _queueSemaphore.WaitAsync();
            try
            {
                if (_logQueue.IsEmpty)
                    return;

                var batch = new List<LogEntry>();
                while (batch.Count < BatchSize && _logQueue.TryDequeue(out var logEntry))
                {
                    batch.Add(logEntry);
                }

                if (batch.Count > 0)
                {
                    using var scope = serviceProvider.CreateScope();
                    var loggingService = scope.ServiceProvider.GetService<ILoggingService>();
                    
                    if (loggingService != null)
                    {
                        foreach (var logEntry in batch)
                        {
                            try
                            {
                                await SendLogEntryToServiceAsync(loggingService, logEntry);
                            }
                            catch
                            {
                                // Silently fail individual log entries
                            }
                        }
                    }
                }
            }
            catch
            {
                // Silently fail to avoid issues
            }
            finally
            {
                _queueSemaphore.Release();
            }
        }

        private static async Task SendLogEntryToServiceAsync(ILoggingService loggingService, LogEntry logEntry)
        {
            if (logEntry.Exception != null)
            {
                await loggingService.LogErrorAsync(
                    logEntry.Exception,
                    logEntry.Action,
                    logEntry.Category,
                    details: logEntry.Message,
                    source: "API-Logger"
                );
            }
            else
            {
                switch (logEntry.LogLevel)
                {
                    case LogLevel.Warning:
                        await loggingService.LogWarningAsync(
                            logEntry.Message,
                            logEntry.Action,
                            logEntry.Category,
                            source: "API-Logger"
                        );
                        break;
                    case LogLevel.Error:
                    case LogLevel.Critical:
                        await loggingService.LogErrorAsync(
                            new Exception(logEntry.Message),
                            logEntry.Action,
                            logEntry.Category,
                            source: "API-Logger"
                        );
                        break;
                    default:
                        await loggingService.LogInfoAsync(
                            logEntry.Message,
                            logEntry.Action,
                            logEntry.Category,
                            source: "API-Logger"
                        );
                        break;
                }
            }
        }

        private class LogEntry
        {
            public string CategoryName { get; set; } = string.Empty;
            public LogLevel LogLevel { get; set; }
            public string Message { get; set; } = string.Empty;
            public Exception? Exception { get; set; }
            public DateTime Timestamp { get; set; }
            public string Category { get; set; } = string.Empty;
            public string Action { get; set; } = string.Empty;
            public int EventId { get; set; }
            public string? EventName { get; set; }
        }
    }
}