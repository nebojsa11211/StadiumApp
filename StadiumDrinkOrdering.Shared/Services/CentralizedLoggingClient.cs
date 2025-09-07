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
        Task LogEventCreatedAsync(int eventId, string eventName, DateTime eventDate, string? creatorId = null, string? creatorEmail = null);
        Task LogTicketPurchaseAsync(int orderId, int ticketCount, decimal amount, string currency, string? customerId = null, string? customerEmail = null, Dictionary<string, object>? metadata = null);
        Task LogOrderStatusChangeAsync(int orderId, string orderNumber, string previousStatus, string newStatus, string? userId = null, string? userEmail = null);
        Task LogStaffAddedAsync(int staffId, string staffName, string role, string? addedById = null, string? addedByEmail = null);
        Task LogOrderDeliveredAsync(int orderId, string orderNumber, string? deliveredById = null, string? deliveredByEmail = null, string? locationInfo = null);
    }

    public class CentralizedLoggingClient : ICentralizedLoggingClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiBaseUrl;
        private readonly string _source;
        private readonly Queue<LogUserActionRequest> _logQueue = new();
        private readonly Timer _batchTimer;
        private readonly SemaphoreSlim _queueSemaphore = new(1, 1);
        private const int BatchSize = 10;
        private const int BatchIntervalMs = 5000; // 5 seconds

        public CentralizedLoggingClient(HttpClient httpClient, string apiBaseUrl, string source)
        {
            _httpClient = httpClient;
            _apiBaseUrl = apiBaseUrl.TrimEnd('/');
            _source = source;
            
            // Start batch timer for high-volume logging
            _batchTimer = new Timer(ProcessBatchTimerCallback, null, BatchIntervalMs, BatchIntervalMs);
        }

        public async Task LogUserActionAsync(string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
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

            // For high-priority user actions, send immediately
            if (category == "Authentication" || category == "Security")
            {
                await SendLogEntryAsync(logEntry);
            }
            else
            {
                // Queue for batch processing
                await QueueLogEntryAsync(logEntry);
            }
        }

        public async Task LogErrorAsync(Exception exception, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
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

            // Errors are always sent immediately
            await SendLogEntryAsync(logEntry);
        }

        public async Task LogWarningAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
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

            await SendLogEntryAsync(logEntry);
        }

        public async Task LogInfoAsync(string message, string action, string category, string? userId = null, 
            string? userEmail = null, string? userRole = null, string? details = null, 
            string? requestPath = null, string? httpMethod = null, string source = "Unknown")
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

            await QueueLogEntryAsync(logEntry);
        }

        public async Task LogBatchAsync(List<LogUserActionRequest> logEntries)
        {
            if (!logEntries.Any()) return;

            try
            {
                var json = JsonSerializer.Serialize(logEntries, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync($"{_apiBaseUrl}/api/logs/log-batch", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    // Fallback: try individual entries
                    foreach (var entry in logEntries)
                    {
                        await SendLogEntryAsync(entry);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log to console as fallback
                Console.WriteLine($"Failed to send batch logs: {ex.Message}");
            }
        }

        private async Task SendLogEntryAsync(LogUserActionRequest logEntry)
        {
            try
            {
                var json = JsonSerializer.Serialize(logEntry, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await _httpClient.PostAsync($"{_apiBaseUrl}/api/logs/log-action", content);
            }
            catch (Exception ex)
            {
                // Log to console as fallback
                Console.WriteLine($"Failed to send log entry: {ex.Message}");
            }
        }

        private async Task QueueLogEntryAsync(LogUserActionRequest logEntry)
        {
            await _queueSemaphore.WaitAsync();
            try
            {
                _logQueue.Enqueue(logEntry);
                
                // If queue is full, process immediately
                if (_logQueue.Count >= BatchSize)
                {
                    await ProcessBatchAsync(null);
                }
            }
            finally
            {
                _queueSemaphore.Release();
            }
        }

        private void ProcessBatchTimerCallback(object? state)
        {
            // Fire and forget - don't wait for completion
            _ = Task.Run(async () => await ProcessBatchAsync(state));
        }

        private async Task ProcessBatchAsync(object? state)
        {
            if (_logQueue.Count == 0) return;

            await _queueSemaphore.WaitAsync();
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
                    await LogBatchAsync(batch);
                }
            }
            finally
            {
                _queueSemaphore.Release();
            }
        }

        public async Task LogBusinessEventAsync(BusinessEventDto businessEvent)
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

            // High-priority business events
            if (businessEvent.Category == BusinessEventCategories.PaymentProcessing || 
                businessEvent.Category == BusinessEventCategories.TicketSales ||
                businessEvent.MonetaryAmount.HasValue)
            {
                await SendLogEntryAsync(logEntry);
            }
            else
            {
                await QueueLogEntryAsync(logEntry);
            }
        }

        public async Task LogEventCreatedAsync(int eventId, string eventName, DateTime eventDate, string? creatorId = null, string? creatorEmail = null)
        {
            await LogBusinessEventAsync(new BusinessEventDto
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
            });
        }

        public async Task LogTicketPurchaseAsync(int orderId, int ticketCount, decimal amount, string currency, string? customerId = null, string? customerEmail = null, Dictionary<string, object>? metadata = null)
        {
            await LogBusinessEventAsync(new BusinessEventDto
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
            });
        }

        public async Task LogOrderStatusChangeAsync(int orderId, string orderNumber, string previousStatus, string newStatus, string? userId = null, string? userEmail = null)
        {
            await LogBusinessEventAsync(new BusinessEventDto
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
            });
        }

        public async Task LogStaffAddedAsync(int staffId, string staffName, string role, string? addedById = null, string? addedByEmail = null)
        {
            await LogBusinessEventAsync(new BusinessEventDto
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
            });
        }

        public async Task LogOrderDeliveredAsync(int orderId, string orderNumber, string? deliveredById = null, string? deliveredByEmail = null, string? locationInfo = null)
        {
            await LogBusinessEventAsync(new BusinessEventDto
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
            });
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
            _batchTimer?.Dispose();
            _queueSemaphore?.Dispose();
        }
    }

    public static class LoggingClientExtensions
    {
        public static IServiceCollection AddCentralizedLogging(this IServiceCollection services, 
            string apiBaseUrl, string source)
        {
            services.AddHttpClient<ICentralizedLoggingClient, CentralizedLoggingClient>(client =>
            {
                client.BaseAddress = new Uri(apiBaseUrl);
                client.Timeout = TimeSpan.FromSeconds(10);
            });

            services.AddSingleton<ICentralizedLoggingClient>(provider =>
            {
                var httpClientFactory = provider.GetRequiredService<IHttpClientFactory>();
                var httpClient = httpClientFactory.CreateClient();
                return new CentralizedLoggingClient(httpClient, apiBaseUrl, source);
            });

            return services;
        }
    }
}

