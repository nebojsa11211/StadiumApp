using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.Authentication.Constants;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Middleware;

namespace StadiumDrinkOrdering.Shared.Authentication.Services;

/// <summary>
/// Background service that monitors token expiration and automatically refreshes tokens
/// </summary>
public class BackgroundTokenRefreshService : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<BackgroundTokenRefreshService> _logger;
    private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(1); // Check every minute
    private readonly TimeSpan _refreshThreshold = AuthenticationConstants.TokenRefresh.RefreshWindow;

    public BackgroundTokenRefreshService(
        IServiceProvider serviceProvider,
        ILogger<BackgroundTokenRefreshService> logger)
    {
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Background token refresh service started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CheckAndRefreshTokensAsync();
                await Task.Delay(_checkInterval, stoppingToken);
            }
            catch (OperationCanceledException)
            {
                // Expected when cancellation is requested
                break;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during background token refresh check");

                // Wait a bit longer after an error to avoid tight error loops
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }

        _logger.LogInformation("Background token refresh service stopped");
    }

    /// <summary>
    /// Checks if tokens need refreshing and performs refresh if necessary
    /// </summary>
    private async Task CheckAndRefreshTokensAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        try
        {
            var tokenStorage = scope.ServiceProvider.GetService<ITokenStorageService>();
            var tokenRefreshService = scope.ServiceProvider.GetService<ITokenRefreshService>();

            if (tokenStorage == null || tokenRefreshService == null)
            {
                _logger.LogDebug("Token storage or refresh service not available, skipping refresh check");
                return;
            }

            var tokenInfo = await tokenStorage.GetTokenInfoAsync();
            if (tokenInfo?.Token == null || !tokenInfo.IsValid)
            {
                _logger.LogDebug("No valid token found, skipping refresh check");
                return;
            }

            // Check if token needs refresh
            if (tokenInfo.TimeToExpiry <= _refreshThreshold)
            {
                _logger.LogDebug("Token expires in {TimeToExpiry}, attempting background refresh",
                    tokenInfo.TimeToExpiry);

                var refreshResult = await tokenRefreshService.RefreshTokenAsync();

                if (refreshResult.Success)
                {
                    _logger.LogInformation("Background token refresh successful, new token expires at {ExpiresAt}",
                        refreshResult.TokenInfo?.ExpiresAt);
                }
                else if (refreshResult.RequiresReauthentication)
                {
                    _logger.LogWarning("Background token refresh failed, user re-authentication required: {ErrorMessage}",
                        refreshResult.ErrorMessage);

                    // Clear invalid tokens
                    await tokenStorage.ClearTokenAsync();
                    await tokenStorage.ClearRefreshTokenAsync();
                }
                else
                {
                    _logger.LogWarning("Background token refresh failed: {ErrorMessage}",
                        refreshResult.ErrorMessage);
                }
            }
            else
            {
                _logger.LogTrace("Token is still valid for {TimeToExpiry}, no refresh needed",
                    tokenInfo.TimeToExpiry);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during token refresh check");
        }
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background token refresh service is stopping");
        await base.StopAsync(cancellationToken);
    }
}

/// <summary>
/// Extension methods for registering the background token refresh service
/// </summary>
public static class BackgroundTokenRefreshServiceExtensions
{
    /// <summary>
    /// Adds the background token refresh service to the service collection
    /// </summary>
    public static IServiceCollection AddBackgroundTokenRefresh(this IServiceCollection services)
    {
        services.AddHostedService<BackgroundTokenRefreshService>();
        return services;
    }

    /// <summary>
    /// Adds the background token refresh service with custom configuration
    /// </summary>
    public static IServiceCollection AddBackgroundTokenRefresh(
        this IServiceCollection services,
        Action<BackgroundTokenRefreshOptions> configureOptions)
    {
        services.Configure(configureOptions);
        services.AddHostedService<BackgroundTokenRefreshService>();
        return services;
    }
}

/// <summary>
/// Configuration options for the background token refresh service
/// </summary>
public class BackgroundTokenRefreshOptions
{
    /// <summary>
    /// How often to check for tokens that need refreshing
    /// </summary>
    public TimeSpan CheckInterval { get; set; } = TimeSpan.FromMinutes(1);

    /// <summary>
    /// Time before expiration to trigger refresh
    /// </summary>
    public TimeSpan RefreshThreshold { get; set; } = AuthenticationConstants.TokenRefresh.RefreshWindow;

    /// <summary>
    /// Whether the service is enabled
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Delay after errors to prevent tight error loops
    /// </summary>
    public TimeSpan ErrorDelay { get; set; } = TimeSpan.FromMinutes(5);
}