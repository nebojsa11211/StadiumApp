using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.Models;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace StadiumDrinkOrdering.Shared.Authentication.Middleware;

/// <summary>
/// HTTP message handler that automatically injects JWT tokens into outgoing requests
/// and handles token refresh when tokens are expired or near expiry
/// </summary>
public class AuthenticationHandler : DelegatingHandler
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly ITokenRefreshService _tokenRefreshService;
    private readonly ILogger<AuthenticationHandler> _logger;
    private readonly SemaphoreSlim _refreshSemaphore = new(1, 1);
    private readonly HashSet<string> _skipAuthPaths;
    private readonly TimeSpan _refreshWindow = TimeSpan.FromMinutes(2); // Refresh tokens that expire within 2 minutes

    public AuthenticationHandler(
        ITokenStorageService tokenStorage,
        ITokenRefreshService tokenRefreshService,
        ILogger<AuthenticationHandler> logger)
    {
        _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        _tokenRefreshService = tokenRefreshService ?? throw new ArgumentNullException(nameof(tokenRefreshService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        // Paths that should not include authentication headers
        _skipAuthPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "/api/auth/login",
            "/api/auth/register",
            "/api/auth/refresh-token",
            "/api/health",
            "/api/ping"
        };
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        // Skip authentication for certain endpoints
        if (ShouldSkipAuthentication(request))
        {
            return await base.SendAsync(request, cancellationToken);
        }

        // Try to add authentication header
        var tokenAdded = await TryAddAuthenticationHeaderAsync(request);

        // Send the request
        var response = await base.SendAsync(request, cancellationToken);

        // Handle 401 Unauthorized - attempt token refresh and retry
        if (response.StatusCode == HttpStatusCode.Unauthorized && tokenAdded)
        {
            _logger.LogDebug("Received 401 Unauthorized, attempting token refresh for request to {RequestUri}", request.RequestUri);

            var retryResponse = await HandleUnauthorizedResponseAsync(request, cancellationToken);
            if (retryResponse != null)
            {
                response.Dispose(); // Dispose the original failed response
                return retryResponse;
            }
        }

        return response;
    }

    /// <summary>
    /// Attempts to add authentication header to the request
    /// </summary>
    private async Task<bool> TryAddAuthenticationHeaderAsync(HttpRequestMessage request)
    {
        try
        {
            var tokenInfo = await _tokenStorage.GetTokenInfoAsync();

            if (tokenInfo?.Token == null || !tokenInfo.IsValid)
            {
                _logger.LogDebug("No valid token available for request to {RequestUri}", request.RequestUri);
                return false;
            }

            // Check if token needs refresh (expires within refresh window)
            if (tokenInfo.IsExpiringSoon || tokenInfo.TimeToExpiry <= _refreshWindow)
            {
                _logger.LogDebug("Token expires soon ({TimeToExpiry}), attempting proactive refresh", tokenInfo.TimeToExpiry);

                var refreshedToken = await RefreshTokenWithLockAsync();
                if (refreshedToken != null)
                {
                    tokenInfo = refreshedToken;
                }
            }

            // Add Authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenInfo.Token);
            _logger.LogTrace("Added Bearer token to request to {RequestUri}", request.RequestUri);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to add authentication header to request to {RequestUri}", request.RequestUri);
            return false;
        }
    }

    /// <summary>
    /// Handles 401 Unauthorized responses by attempting token refresh and retrying the request
    /// </summary>
    private async Task<HttpResponseMessage?> HandleUnauthorizedResponseAsync(
        HttpRequestMessage originalRequest,
        CancellationToken cancellationToken)
    {
        try
        {
            // Attempt to refresh the token
            var refreshedToken = await RefreshTokenWithLockAsync();
            if (refreshedToken?.Token == null)
            {
                _logger.LogDebug("Token refresh failed for request to {RequestUri}", originalRequest.RequestUri);
                return null;
            }

            _logger.LogDebug("Token refreshed successfully, retrying request to {RequestUri}", originalRequest.RequestUri);

            // Clone the original request for retry
            var retryRequest = await CloneRequestAsync(originalRequest);

            // Add the new token
            retryRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", refreshedToken.Token);

            // Retry the request
            var retryResponse = await base.SendAsync(retryRequest, cancellationToken);

            _logger.LogDebug("Retry request to {RequestUri} completed with status {StatusCode}",
                retryRequest.RequestUri, retryResponse.StatusCode);

            return retryResponse;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to handle unauthorized response for request to {RequestUri}", originalRequest.RequestUri);
            return null;
        }
    }

    /// <summary>
    /// Refreshes the token with thread safety using a semaphore
    /// </summary>
    private async Task<TokenInfo?> RefreshTokenWithLockAsync()
    {
        if (!await _refreshSemaphore.WaitAsync(TimeSpan.FromSeconds(10)))
        {
            _logger.LogWarning("Timeout waiting for token refresh semaphore");
            return null;
        }

        try
        {
            // Double-check if token was already refreshed by another thread
            var currentToken = await _tokenStorage.GetTokenInfoAsync();
            if (currentToken?.IsValid == true && currentToken.TimeToExpiry > _refreshWindow)
            {
                _logger.LogDebug("Token was already refreshed by another thread");
                return currentToken;
            }

            // Perform the token refresh
            _logger.LogDebug("Performing token refresh");
            var refreshResult = await _tokenRefreshService.RefreshTokenAsync();

            if (refreshResult.Success && refreshResult.TokenInfo != null)
            {
                _logger.LogDebug("Token refresh successful, new token expires at {ExpiresAt}",
                    refreshResult.TokenInfo.ExpiresAt);
                return refreshResult.TokenInfo;
            }
            else
            {
                _logger.LogWarning("Token refresh failed: {ErrorMessage}", refreshResult.ErrorMessage);
                return null;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception during token refresh");
            return null;
        }
        finally
        {
            _refreshSemaphore.Release();
        }
    }

    /// <summary>
    /// Checks if authentication should be skipped for this request
    /// </summary>
    private bool ShouldSkipAuthentication(HttpRequestMessage request)
    {
        if (request.RequestUri?.AbsolutePath == null)
            return true;

        var path = request.RequestUri.AbsolutePath;
        return _skipAuthPaths.Any(skipPath => path.StartsWith(skipPath, StringComparison.OrdinalIgnoreCase));
    }

    /// <summary>
    /// Clones an HTTP request for retry scenarios
    /// </summary>
    private static async Task<HttpRequestMessage> CloneRequestAsync(HttpRequestMessage original)
    {
        var clone = new HttpRequestMessage(original.Method, original.RequestUri)
        {
            Version = original.Version
        };

        // Copy headers
        foreach (var header in original.Headers)
        {
            clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }

        // Copy content if present
        if (original.Content != null)
        {
            var contentBytes = await original.Content.ReadAsByteArrayAsync();
            clone.Content = new ByteArrayContent(contentBytes);

            // Copy content headers
            foreach (var header in original.Content.Headers)
            {
                clone.Content.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }
        }

        // Copy options (replaces deprecated Properties)
        foreach (var option in original.Options)
        {
            clone.Options.TryAdd(option.Key, option.Value);
        }

        return clone;
    }

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            _refreshSemaphore?.Dispose();
        }
        base.Dispose(disposing);
    }
}

/// <summary>
/// Result of a token refresh operation
/// </summary>
public class TokenRefreshResult
{
    public bool Success { get; set; }
    public TokenInfo? TokenInfo { get; set; }
    public string? ErrorMessage { get; set; }
    public bool RequiresReauthentication { get; set; }

    public static TokenRefreshResult Successful(TokenInfo tokenInfo)
    {
        return new TokenRefreshResult
        {
            Success = true,
            TokenInfo = tokenInfo
        };
    }

    public static TokenRefreshResult Failed(string errorMessage, bool requiresReauth = false)
    {
        return new TokenRefreshResult
        {
            Success = false,
            ErrorMessage = errorMessage,
            RequiresReauthentication = requiresReauth
        };
    }
}

/// <summary>
/// Service interface for handling token refresh operations
/// </summary>
public interface ITokenRefreshService
{
    /// <summary>
    /// Attempts to refresh the current access token using the stored refresh token
    /// </summary>
    Task<TokenRefreshResult> RefreshTokenAsync();

    /// <summary>
    /// Event fired when a token refresh fails and user needs to re-authenticate
    /// </summary>
    event Action? OnAuthenticationRequired;
}