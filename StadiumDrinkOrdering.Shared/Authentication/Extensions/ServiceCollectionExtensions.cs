using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Middleware;
using StadiumDrinkOrdering.Shared.Authentication.Services;

namespace StadiumDrinkOrdering.Shared.Authentication.Extensions;

/// <summary>
/// Extension methods for configuring authentication services in the DI container
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the complete authentication system with HTTP client middleware
    /// </summary>
    public static IServiceCollection AddAuthenticatedHttpClient(
        this IServiceCollection services,
        string baseUrl,
        string applicationContext)
    {
        // Register the token refresh service
        services.AddScoped<ITokenRefreshService, TokenRefreshService>();

        // Register a named HTTP client for the token refresh service (without auth handler to avoid circular dependency)
        services.AddHttpClient("TokenRefreshClient", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(120);
            client.DefaultRequestHeaders.Add("User-Agent", $"Stadium-{applicationContext}/1.0");
        });

        // Register the main authenticated HTTP client
        services.AddHttpClient("AuthenticatedClient", client =>
        {
            client.BaseAddress = new Uri(baseUrl);
            client.Timeout = TimeSpan.FromSeconds(120);
            client.DefaultRequestHeaders.Add("User-Agent", $"Stadium-{applicationContext}/1.0");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        })
        .AddHttpMessageHandler<AuthenticationHandler>();

        // Register the authentication handler
        services.AddTransient<AuthenticationHandler>();

        return services;
    }

    /// <summary>
    /// Adds the authentication handler to an existing HTTP client
    /// </summary>
    public static IHttpClientBuilder AddAuthenticationHandler(this IHttpClientBuilder builder)
    {
        return builder.AddHttpMessageHandler<AuthenticationHandler>();
    }

    /// <summary>
    /// Adds background token refresh service
    /// </summary>
    public static IServiceCollection AddBackgroundTokenRefresh(this IServiceCollection services)
    {
        services.AddHostedService<BackgroundTokenRefreshService>();
        return services;
    }


    /// <summary>
    /// Adds the complete authentication infrastructure for a client application
    /// Note: Token storage service must be registered separately in each project
    /// </summary>
    public static IServiceCollection AddClientAuthentication(
        this IServiceCollection services,
        string apiBaseUrl,
        string applicationContext,
        bool enableBackgroundRefresh = true)
    {
        // Add authenticated HTTP client
        services.AddAuthenticatedHttpClient(apiBaseUrl, applicationContext);

        // Add background refresh if enabled
        if (enableBackgroundRefresh)
        {
            services.AddBackgroundTokenRefresh();
        }

        return services;
    }

    /// <summary>
    /// Configures token refresh service with custom HTTP client factory
    /// </summary>
    public static IServiceCollection AddTokenRefreshService(
        this IServiceCollection services,
        Func<IServiceProvider, HttpClient> httpClientFactory)
    {
        services.AddScoped<ITokenRefreshService>(serviceProvider =>  // Changed from Singleton to Scoped - aligns with ITokenStorageService
        {
            var httpClient = httpClientFactory(serviceProvider);
            var tokenStorage = serviceProvider.GetRequiredService<ITokenStorageService>();
            var logger = serviceProvider.GetRequiredService<ILogger<TokenRefreshService>>();

            return new TokenRefreshService(httpClient, tokenStorage, logger);
        });

        return services;
    }

    /// <summary>
    /// Adds authentication middleware components
    /// </summary>
    public static IServiceCollection AddAuthenticationMiddleware(this IServiceCollection services)
    {
        // Register authentication handler as transient since it can be reused
        services.AddTransient<AuthenticationHandler>();

        return services;
    }

    /// <summary>
    /// Legacy method - adds shared authentication components with generic types
    /// </summary>
    public static IServiceCollection AddSharedAuthentication<TAuthState, TTokenStorage, TApiService>(
        this IServiceCollection services,
        string applicationContext,
        string apiBaseUrl)
        where TAuthState : class
        where TTokenStorage : class, ITokenStorageService
        where TApiService : class
    {
        // Register the specific implementations
        services.AddScoped<TAuthState>();
        services.AddScoped<ITokenStorageService, TTokenStorage>();  // Changed from Singleton to Scoped - fixes DI issue with IJSRuntime
        services.AddScoped<TApiService>();

        // Add client authentication
        return services.AddClientAuthentication(apiBaseUrl, applicationContext, enableBackgroundRefresh: true);
    }

    /// <summary>
    /// Simple legacy method - adds shared authentication components
    /// </summary>
    public static IServiceCollection AddSharedAuthentication(this IServiceCollection services, string apiBaseUrl, string applicationContext)
    {
        return services.AddClientAuthentication(apiBaseUrl, applicationContext, enableBackgroundRefresh: true);
    }

    /// <summary>
    /// Adds authentication event handlers
    /// </summary>
    public static IServiceCollection AddAuthenticationEvents<THandler>(this IServiceCollection services)
        where THandler : class, IAuthenticationEventHandler
    {
        services.AddScoped<IAuthenticationEventHandler, THandler>();
        return services;
    }
}

/// <summary>
/// Interface for handling authentication events
/// </summary>
public interface IAuthenticationEventHandler
{
    /// <summary>
    /// Called when authentication is required (e.g., refresh token failed)
    /// </summary>
    Task OnAuthenticationRequiredAsync();

    /// <summary>
    /// Called when token refresh succeeds
    /// </summary>
    Task OnTokenRefreshSuccessAsync();

    /// <summary>
    /// Called when token refresh fails
    /// </summary>
    Task OnTokenRefreshFailedAsync(string error);
}

/// <summary>
/// Default authentication event handler that logs events
/// </summary>
public class DefaultAuthenticationEventHandler : IAuthenticationEventHandler
{
    private readonly ILogger<DefaultAuthenticationEventHandler> _logger;

    public DefaultAuthenticationEventHandler(ILogger<DefaultAuthenticationEventHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public Task OnAuthenticationRequiredAsync()
    {
        _logger.LogWarning("Authentication required - user needs to re-authenticate");
        return Task.CompletedTask;
    }

    public Task OnTokenRefreshSuccessAsync()
    {
        _logger.LogDebug("Token refresh successful");
        return Task.CompletedTask;
    }

    public Task OnTokenRefreshFailedAsync(string error)
    {
        _logger.LogWarning("Token refresh failed: {Error}", error);
        return Task.CompletedTask;
    }
}