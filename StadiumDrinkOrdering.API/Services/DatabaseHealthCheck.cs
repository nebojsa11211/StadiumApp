using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Custom health check for PostgreSQL/Supabase database connectivity
/// Provides detailed health status information for monitoring
/// </summary>
public class DatabaseHealthCheck : IHealthCheck
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DatabaseHealthCheck> _logger;

    public DatabaseHealthCheck(ApplicationDbContext context, ILogger<DatabaseHealthCheck> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Performing database health check");

            // Test basic connectivity with timeout
            using var timeoutCts = new CancellationTokenSource(TimeSpan.FromSeconds(10));
            using var combinedCts = CancellationTokenSource.CreateLinkedTokenSource(
                cancellationToken, timeoutCts.Token);

            var canConnect = await _context.Database.CanConnectAsync(combinedCts.Token);

            if (!canConnect)
            {
                const string message = "Database connection test failed";
                _logger.LogWarning(message);
                return HealthCheckResult.Unhealthy(message);
            }

            // Test query execution
            var connectionInfo = new Dictionary<string, object>
            {
                ["DatabaseName"] = _context.Database.GetConnectionString() ?? "Unknown",
                ["Provider"] = _context.Database.ProviderName ?? "PostgreSQL"
            };

            // Test table accessibility
            try
            {
                var userCount = await _context.Users.CountAsync(combinedCts.Token);
                connectionInfo["UserCount"] = userCount;
                connectionInfo["TablesAccessible"] = true;
            }
            catch (Exception ex)
            {
                connectionInfo["TablesAccessible"] = false;
                connectionInfo["TableError"] = ex.Message;
            }

            _logger.LogDebug("Database health check passed");
            return HealthCheckResult.Healthy("Database is accessible and responsive", connectionInfo);
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            const string message = "Database health check was cancelled";
            _logger.LogWarning(message);
            return HealthCheckResult.Degraded(message);
        }
        catch (OperationCanceledException)
        {
            const string message = "Database health check timed out";
            _logger.LogWarning(message);
            return HealthCheckResult.Degraded(message);
        }
        catch (Exception ex)
        {
            var message = $"Database health check failed: {ex.Message}";
            _logger.LogError(ex, "Database health check encountered an error");
            return HealthCheckResult.Unhealthy(message, ex);
        }
    }

}