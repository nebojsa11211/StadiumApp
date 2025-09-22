using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Http;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Shared.Services
{
    public interface ICentralizedLoggingClient
    {
        Task LogUserActionAsync(string action, string category, string? userId = null, string? userEmail = null, 
            string? userRole = null, string? details = null, string? requestPath = null, 
            string? httpMethod = null, string source = "Unknown");
        
        Task LogErrorAsync(Exception exception, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string source = "Unknown");
        
        Task LogWarningAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string source = "Unknown");
        
        Task LogInfoAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string source = "Unknown");

        Task LogBatchAsync(List<LogUserActionRequest> logEntries);
        
        // Business Event Methods
        Task LogBusinessEventAsync(BusinessEventDto businessEvent);
        void LogEventCreatedAsync(int eventId, string eventName, DateTime eventDate, string? creatorId = null, string? creatorEmail = null);
        void LogTicketPurchaseAsync(int orderId, int ticketCount, decimal amount, string currency, string? customerId = null, string? customerEmail = null, Dictionary<string, object>? metadata = null);
        void LogOrderStatusChangeAsync(int orderId, string orderNumber, string previousStatus, string newStatus, string? userId = null, string? userEmail = null);
        void LogStaffAddedAsync(int staffId, string staffName, string role, string? addedById = null, string? addedByEmail = null);
        void LogOrderDeliveredAsync(int orderId, string orderNumber, string? deliveredById = null, string? deliveredByEmail = null, string? locationInfo = null);
    }

    public class CentralizedLoggingClient : ICentralizedLoggingClient, IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _source;
        private readonly Queue<LogUserActionRequest> _logQueue = new();
        private readonly Timer _batchTimer;
        private readonly SemaphoreSlim _queueSemaphore = new(1, 1);
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private const int BatchSize = 10;
        private const int BatchIntervalMs = 5000; // 5 seconds
        private volatile bool _disposed = false;

        public CentralizedLoggingClient(HttpClient httpClient, string apiBaseUrl, string source)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _apiBaseUrl = apiBaseUrl?.TrimEnd('/') ?? throw new ArgumentNullException(nameof(apiBaseUrl));
            _source = source ?? throw new ArgumentNullException(nameof(source));

            // Start batch timer for high-volume logging
            _batchTimer = new Timer(ProcessBatchTimerCallback, null, BatchIntervalMs, BatchIntervalMs);
        }

        /// <summary>
        /// Fire-and-forget logging method that prevents deadlocks
        /// </summary>
        private void LogSafely(Func<Task> logAction)
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            _ = Task.Run(async () =>
            {
                try
                {
                    await logAction().ConfigureAwait(false);
                }
                catch (Exception ex)
                {
                    // Fail silently to prevent application disruption
                    Console.WriteLine($"Logging error: {ex.Message}");
                }
            }, _cancellationTokenSource.Token);
        }

        public Task LogUserActionAsync(string action, string category, string? userId = null,
            string? userEmail = null, string? userRole = null, string? details = null,
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return Task.CompletedTask;

            try
            {
                var logEntry = new LogUserActionRequest
                {
                    Action = action,
                    Category = category,
                    UserId = userId,
                    UserEmail = userEmail,
                    UserRole = userRole,
                    Details = details,
                    RequestPath = requestPath,
                    HttpMethod = httpMethod,
                    Source = source == "Unknown" ? _source : source
                };

                // For high-priority user actions, send immediately but don't await to prevent deadlocks
                if (category == "Authentication" || category == "Security")
                {
                    LogSafely(() => SendLogEntryAsync(logEntry));
                }
                else
                {
                    // Queue for batch processing (non-blocking)
                    LogSafely(() => QueueLogEntryAsync(logEntry));
                }
            }
            catch (Exception ex)
            {
                // Fail silently to prevent application disruption
                Console.WriteLine($"Failed to log user action: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public Task LogErrorAsync(Exception exception, string action, string category, string? userId = null,
            string? userEmail = null, string? userRole = null, string? details = null,
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return Task.CompletedTask;

            try
            {
                var logEntry = new LogUserActionRequest
                {
                    Action = action,
                    Category = category,
                    UserId = userId,
                    UserEmail = userEmail,
                    UserRole = userRole,
                    Details = $"Error: {exception.Message} | Details: {details}",
                    RequestPath = requestPath,
                    HttpMethod = httpMethod,
                    Source = source == "Unknown" ? _source : source
                };

                // Errors are always sent immediately but fire-and-forget to prevent deadlocks
                LogSafely(() => SendLogEntryAsync(logEntry));
            }
            catch (Exception ex)
            {
                // Fail silently to prevent recursive error logging
                Console.WriteLine($"Failed to log error: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public Task LogWarningAsync(string message, string action, string category, string? userId = null,
            string? userEmail = null, string? userRole = null, string? details = null,
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return Task.CompletedTask;

            try
            {
                var logEntry = new LogUserActionRequest
                {
                    Action = action,
                    Category = category,
                    UserId = userId,
                    UserEmail = userEmail,
                    UserRole = userRole,
                    Details = $"Warning: {message} | Details: {details}",
                    RequestPath = requestPath,
                    HttpMethod = httpMethod,
                    Source = source == "Unknown" ? _source : source
                };

                // Send warnings immediately but fire-and-forget to prevent deadlocks
                LogSafely(() => SendLogEntryAsync(logEntry));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log warning: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public Task LogInfoAsync(string message, string action, string category, string? userId = null,
            string? userEmail = null, string? userRole = null, string? details = null,
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return Task.CompletedTask;

            try
            {
                var logEntry = new LogUserActionRequest
                {
                    Action = action,
                    Category = category,
                    UserId = userId,
                    UserEmail = userEmail,
                    UserRole = userRole,
                    Details = $"Info: {message} | Details: {details}",
                    RequestPath = requestPath,
                    HttpMethod = httpMethod,
                    Source = source == "Unknown" ? _source : source
                };

                // Queue info logs for batch processing (non-blocking)
                LogSafely(() => QueueLogEntryAsync(logEntry));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log info: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public async Task LogBatchAsync(List<LogUserActionRequest> logEntries)
        {
            if (!logEntries.Any() || _disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            try
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(15));
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    _cancellationTokenSource.Token, timeoutCts.Token);

                var json = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/logs/log-batch", content, combinedCts.Token)
                    .ConfigureAwait(false);

                if (!response.IsSuccessStatusCode)
                {
                    // Fallback: try individual entries (with fire-and-forget to prevent blocking)
                    _ = Task.Run(async () =>
                    {
                        foreach (var entry in logEntries)
                        {
                            await SendLogEntryAsync(entry).ConfigureAwait(false);
                        }
                    }, combinedCts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                // Timeout or cancellation - fail silently to prevent blocking
            }
            catch (Exception ex)
            {
                // Log to console as fallback
                Console.WriteLine($"Failed to send batch logs: {ex.Message}");
            }
        }

        private async Task SendLogEntryAsync(LogUserActionRequest logEntry)
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            try
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    _cancellationTokenSource.Token, timeoutCts.Token);

                var json = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                using var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync($"{_apiBaseUrl}/logs/log-action", content, combinedCts.Token)
                    .ConfigureAwait(false);
            }
            catch (OperationCanceledException)
            {
                // Timeout or cancellation - fail silently to prevent blocking
            }
            catch (Exception ex)
            {
                // Log to console as fallback
                Console.WriteLine($"Failed to send log entry: {ex.Message}");
            }
        }

        private async Task QueueLogEntryAsync(LogUserActionRequest logEntry)
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            try
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(5));
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    _cancellationTokenSource.Token, timeoutCts.Token);

                await _queueSemaphore.WaitAsync(combinedCts.Token).ConfigureAwait(false);
                try
                {
                    _logQueue.Enqueue(logEntry);

                    // If queue is full, process immediately in background
                    if (_logQueue.Count >= BatchSize)
                    {
                        _ = ProcessBatchSafeAsync();
                    }
                }
                finally
                {
                    _queueSemaphore.Release();
                }
            }
            catch (OperationCanceledException)
            {
                // Timeout - fail silently to prevent blocking
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to queue log entry: {ex.Message}");
            }
        }

        private void ProcessBatchTimerCallback(object? state)
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return;

            // Use ConfigureAwait(false) to avoid deadlocks and fire-and-forget pattern
            _ = ProcessBatchSafeAsync();
        }

        private async Task ProcessBatchSafeAsync()
        {
            try
            {
                await ProcessBatchAsync(null).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                // Silently handle errors in background processing to prevent crashes
                Console.WriteLine($"Error in batch processing: {ex.Message}");
            }
        }

        private async Task ProcessBatchAsync(object? state)
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested || _logQueue.Count == 0)
                return;

            try
            {
                using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
                using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                    _cancellationTokenSource.Token, timeoutCts.Token);

                await _queueSemaphore.WaitAsync(combinedCts.Token).ConfigureAwait(false);
                try
                {
                    var batch = new List<LogUserActionRequest>();

                    // Take up to BatchSize items from queue
                    var itemsToTake = Math.Min(_logQueue.Count, BatchSize);
                    for (int i = 0; i < itemsToTake; i++)
                    {
                        if (_logQueue.TryDequeue(out var entry))
                        {
                            batch.Add(entry);
                        }
                    }

                    if (batch.Any())
                    {
                        await LogBatchAsync(batch).ConfigureAwait(false);
                    }
                }
                finally
                {
                    _queueSemaphore.Release();
                }
            }
            catch (OperationCanceledException)
            {
                // Timeout or cancellation - fail silently
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing batch: {ex.Message}");
            }
        }

        public Task LogBusinessEventAsync(BusinessEventDto businessEvent)
        {
            if (_disposed || _cancellationTokenSource.Token.IsCancellationRequested)
                return Task.CompletedTask;

            try
            {
                var logEntry = new LogUserActionRequest
                {
                    Action = businessEvent.Action,
                    Category = businessEvent.Category,
                    UserId = businessEvent.UserId,
                    UserEmail = businessEvent.UserEmail,
                    UserRole = businessEvent.UserRole,
                    Details = businessEvent.Details,
                    Source = string.IsNullOrEmpty(businessEvent.Source) ? _source : businessEvent.Source,
                    BusinessEntityType = businessEvent.BusinessEntityType,
                    BusinessEntityId = businessEvent.BusinessEntityId,
                    BusinessEntityName = businessEvent.BusinessEntityName,
                    RelatedEntityType = businessEvent.RelatedEntityType,
                    RelatedEntityId = businessEvent.RelatedEntityId,
                    MonetaryAmount = businessEvent.MonetaryAmount,
                    Currency = businessEvent.Currency,
                    Quantity = businessEvent.Quantity,
                    LocationInfo = businessEvent.LocationInfo,
                    StatusBefore = businessEvent.StatusBefore,
                    StatusAfter = businessEvent.StatusAfter,
                    MetadataJson = businessEvent.Metadata != null ? JsonSerializer.Serialize(businessEvent.Metadata) : null
                };

                // High-priority business events - fire and forget to prevent deadlocks
                if (businessEvent.Category == BusinessEventCategories.PaymentProcessing ||
                    businessEvent.Category == BusinessEventCategories.TicketSales ||
                    businessEvent.MonetaryAmount.HasValue)
                {
                    LogSafely(() => SendLogEntryAsync(logEntry));
                }
                else
                {
                    LogSafely(() => QueueLogEntryAsync(logEntry));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to log business event: {ex.Message}");
            }

            return Task.CompletedTask;
        }

        public void LogEventCreatedAsync(int eventId, string eventName, DateTime eventDate, string? creatorId = null, string? creatorEmail = null)
        {
            LogSafely(() => LogBusinessEventAsync(new BusinessEventDto
            {
                Action = BusinessEventActions.EventCreated,
                Category = BusinessEventCategories.EventManagement,
                BusinessEntityType = "Event",
                BusinessEntityId = eventId.ToString(),
                BusinessEntityName = eventName,
                UserId = creatorId,
                UserEmail = creatorEmail,
                Details = $"Event '{eventName}' created for {eventDate:yyyy-MM-dd}",
                Metadata = new Dictionary<string, object> { ["EventDate"] = eventDate.ToString("O") }
            }));
        }

        public void LogTicketPurchaseAsync(int orderId, int ticketCount, decimal amount, string currency, string? customerId = null, string? customerEmail = null, Dictionary<string, object>? metadata = null)
        {
            LogSafely(() => LogBusinessEventAsync(new BusinessEventDto
            {
                Action = BusinessEventActions.TicketPurchased,
                Category = BusinessEventCategories.TicketSales,
                BusinessEntityType = "Order",
                BusinessEntityId = orderId.ToString(),
                RelatedEntityType = "Customer",
                RelatedEntityId = customerId,
                MonetaryAmount = amount,
                Currency = currency,
                Quantity = ticketCount,
                UserId = customerId,
                UserEmail = customerEmail,
                Details = $"Purchased {ticketCount} ticket(s) for {amount:C}",
                Metadata = metadata
            }));
        }

        public void LogOrderStatusChangeAsync(int orderId, string orderNumber, string previousStatus, string newStatus, string? userId = null, string? userEmail = null)
        {
            LogSafely(() => LogBusinessEventAsync(new BusinessEventDto
            {
                Action = GetOrderActionFromStatus(newStatus),
                Category = BusinessEventCategories.OrderProcessing,
                BusinessEntityType = "Order",
                BusinessEntityId = orderId.ToString(),
                BusinessEntityName = orderNumber,
                StatusBefore = previousStatus,
                StatusAfter = newStatus,
                UserId = userId,
                UserEmail = userEmail,
                Details = $"Order {orderNumber} status changed from {previousStatus} to {newStatus}"
            }));
        }

        public void LogStaffAddedAsync(int staffId, string staffName, string role, string? addedById = null, string? addedByEmail = null)
        {
            LogSafely(() => LogBusinessEventAsync(new BusinessEventDto
            {
                Action = BusinessEventActions.StaffMemberAdded,
                Category = BusinessEventCategories.UserManagement,
                BusinessEntityType = "User",
                BusinessEntityId = staffId.ToString(),
                BusinessEntityName = staffName,
                UserId = addedById,
                UserEmail = addedByEmail,
                Details = $"Staff member '{staffName}' added with role '{role}'",
                Metadata = new Dictionary<string, object> { ["Role"] = role }
            }));
        }

        public void LogOrderDeliveredAsync(int orderId, string orderNumber, string? deliveredById = null, string? deliveredByEmail = null, string? locationInfo = null)
        {
            LogSafely(() => LogBusinessEventAsync(new BusinessEventDto
            {
                Action = BusinessEventActions.OrderDelivered,
                Category = BusinessEventCategories.OrderProcessing,
                BusinessEntityType = "Order",
                BusinessEntityId = orderId.ToString(),
                BusinessEntityName = orderNumber,
                LocationInfo = locationInfo,
                StatusAfter = "Delivered",
                UserId = deliveredById,
                UserEmail = deliveredByEmail,
                Details = $"Order {orderNumber} delivered" + (locationInfo != null ? $" at {locationInfo}" : "")
            }));
        }

        private string GetOrderActionFromStatus(string status)
        {
            return status?.ToLower() switch
            {
                "created" or "pending" => BusinessEventActions.OrderCreated,
                "in preparation" => BusinessEventActions.OrderInPreparation,
                "ready" => BusinessEventActions.OrderReady,
                "delivered" => BusinessEventActions.OrderDelivered,
                "cancelled" => BusinessEventActions.OrderCancelled,
                "paid" => BusinessEventActions.OrderPaid,
                _ => BusinessEventActions.OrderUpdated
            };
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            try
            {
                // Cancel all operations
                _cancellationTokenSource.Cancel();

                // Process remaining queue items synchronously (best effort)
                if (_logQueue.Count > 0)
                {
                    _ = Task.Run(async () =>
                    {
                        try
                        {
                            await ProcessBatchAsync(null).ConfigureAwait(false);
                        }
                        catch
                        {
                            // Ignore errors during disposal
                        }
                    });
                }
            }
            catch
            {
                // Ignore errors during disposal
            }
            finally
            {
                _batchTimer?.Dispose();
                _queueSemaphore?.Dispose();
                _cancellationTokenSource.Dispose();
            }
        }
    }

    public static class LoggingClientExtensions
    {
        public static IServiceCollection AddCentralizedLogging(this IServiceCollection services,
            string apiBaseUrl, string source)
        {
            // Register as Singleton to avoid deadlock issues with scoped services
            services.AddSingleton<ICentralizedLoggingClient>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient("CentralizedLogging");

                // Configure HttpClient for logging
                httpClient.BaseAddress = new Uri(apiBaseUrl);
                httpClient.Timeout = TimeSpan.FromSeconds(15);
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("User-Agent", $"CentralizedLogging/{source}");

                return new CentralizedLoggingClient(httpClient, apiBaseUrl, source);
            });

            // Register named HttpClient for logging to avoid conflicts
            services.AddHttpClient("CentralizedLogging", client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(15);
                client.DefaultRequestHeaders.Add("User-Agent", $"CentralizedLogging/{source}");
            }).ConfigurePrimaryHttpMessageHandler(() =>
            {
                var handler = new HttpClientHandler();
                // Add certificate validation bypass if needed for development
                if (apiBaseUrl.StartsWith("https://api:", StringComparison.OrdinalIgnoreCase))
                {
                    handler.ServerCertificateCustomValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
                }
                return handler;
            });

            return services;
        }
    }
}

