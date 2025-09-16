namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Background service for cleaning up expired rate limiting data
/// </summary>
public class RateLimitCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RateLimitCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromHours(1); // Run cleanup every hour

    public RateLimitCleanupService(
        IServiceProvider serviceProvider,
        ILogger<RateLimitCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Rate Limit Cleanup Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await PerformCleanupAsync();

                // Wait for the next cleanup interval
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Service is shutting down
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during rate limit cleanup");

                // Wait a shorter time before retrying on error
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        _logger.LogInformation("Rate Limit Cleanup Service stopped");
    }

    private async Task PerformCleanupAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var bruteForceService = scope.ServiceProvider.GetRequiredService<IBruteForceProtectionService>();

            _logger.LogDebug("Starting rate limit cleanup...");

            await bruteForceService.CleanupExpiredRestrictionsAsync();

            _logger.LogDebug("Rate limit cleanup completed successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during rate limit cleanup execution");
            throw;
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Rate Limit Cleanup Service is stopping");
        await base.StopAsync(cancellationToken);
    }
}