using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace StadiumDrinkOrdering.API.Services;

public class TicketSessionCleanupService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TicketSessionCleanupService> _logger;
    private readonly TimeSpan _cleanupInterval = TimeSpan.FromMinutes(10); // Run cleanup every 10 minutes

    public TicketSessionCleanupService(
        IServiceProvider serviceProvider,
        ILogger<TicketSessionCleanupService> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Ticket Session Cleanup Service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CleanupExpiredSessionsAsync();
                await CleanupExpiredCartReservationsAsync();
                
                // Wait for the next cleanup interval
                await Task.Delay(_cleanupInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation("Ticket Session Cleanup Service is stopping");
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during ticket session cleanup");
                
                // Wait a shorter time before retrying if there was an error
                await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);
            }
        }
    }

    private async Task CleanupExpiredSessionsAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var ticketAuthService = scope.ServiceProvider.GetRequiredService<ITicketAuthService>();
            
            await ticketAuthService.CleanupExpiredSessionsAsync();
            _logger.LogDebug("Completed ticket session cleanup");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during ticket session cleanup");
        }
    }

    private async Task CleanupExpiredCartReservationsAsync()
    {
        try
        {
            using var scope = _serviceProvider.CreateScope();
            var shoppingCartService = scope.ServiceProvider.GetRequiredService<IShoppingCartService>();
            
            await shoppingCartService.CleanupExpiredReservationsAsync();
            _logger.LogDebug("Completed shopping cart reservation cleanup");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during shopping cart reservation cleanup");
        }
    }

    public override Task StopAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Ticket Session Cleanup Service is stopping");
        return base.StopAsync(stoppingToken);
    }
}