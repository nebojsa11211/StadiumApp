using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Middleware;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace StadiumDrinkOrdering.Shared.Authentication.Services;

/// <summary>
/// Service responsible for refreshing JWT tokens using refresh tokens
/// </summary>
public class TokenRefreshService : ITokenRefreshService
{
    private readonly HttpClient _httpClient;
    private readonly ITokenStorageService _tokenStorage;
    private readonly ILogger<TokenRefreshService> _logger;
    private readonly string _refreshEndpoint;
    private readonly JsonSerializerOptions _jsonOptions;

    public event Action? OnAuthenticationRequired;

    public TokenRefreshService(
        HttpClient httpClient,
        ITokenStorageService tokenStorage,
        ILogger<TokenRefreshService> logger)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _tokenStorage = tokenStorage ?? throw new ArgumentNullException(nameof(tokenStorage));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _refreshEndpoint = "/auth/refresh-token";
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = false
        };
    }

    public async Task<TokenRefreshResult> RefreshTokenAsync()
    {
        try
        {
            _logger.LogDebug("Starting token refresh process");

            // Get current tokens
            var currentAccessToken = await _tokenStorage.GetTokenAsync();
            var refreshToken = await _tokenStorage.GetRefreshTokenAsync();

            if (string.IsNullOrEmpty(currentAccessToken) || string.IsNullOrEmpty(refreshToken))
            {
                _logger.LogWarning("Missing access token or refresh token for refresh operation");
                NotifyAuthenticationRequired();
                return TokenRefreshResult.Failed("Missing tokens", requiresReauth: true);
            }

            // Prepare refresh request
            var refreshRequest = new RefreshTokenRequestDto
            {
                AccessToken = currentAccessToken,
                RefreshToken = refreshToken,
                DeviceInfo = await GetDeviceInfoAsync()
            };

            var requestJson = JsonSerializer.Serialize(refreshRequest, _jsonOptions);
            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            _logger.LogDebug("Sending refresh token request to {Endpoint}", _refreshEndpoint);

            // Send refresh request
            var response = await _httpClient.PostAsync(_refreshEndpoint, content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var refreshResponse = JsonSerializer.Deserialize<RefreshTokenResponseDto>(responseContent, _jsonOptions);

                if (refreshResponse?.AccessToken != null)
                {
                    _logger.LogDebug("Token refresh successful, updating stored tokens");

                    // Store new tokens
                    await _tokenStorage.StoreTokenAsync(
                        refreshResponse.AccessToken,
                        refreshResponse.AccessTokenExpiresAt,
                        refreshResponse.User?.Email);

                    if (!string.IsNullOrEmpty(refreshResponse.RefreshToken))
                    {
                        await _tokenStorage.StoreRefreshTokenAsync(
                            refreshResponse.RefreshToken,
                            refreshResponse.RefreshTokenExpiresAt);
                    }

                    // Return success with new token info
                    var tokenInfo = TokenInfo.FromJwtToken(refreshResponse.AccessToken);
                    return TokenRefreshResult.Successful(tokenInfo);
                }
                else
                {
                    _logger.LogWarning("Token refresh response missing access token");
                    return TokenRefreshResult.Failed("Invalid refresh response");
                }
            }
            else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
            {
                _logger.LogWarning("Refresh token was invalid or expired, authentication required");

                // Clear invalid tokens
                await ClearInvalidTokensAsync();
                NotifyAuthenticationRequired();

                return TokenRefreshResult.Failed("Refresh token invalid", requiresReauth: true);
            }
            else
            {
                _logger.LogWarning("Token refresh failed with status {StatusCode}: {ReasonPhrase}",
                    response.StatusCode, response.ReasonPhrase);

                var errorContent = await response.Content.ReadAsStringAsync();
                return TokenRefreshResult.Failed($"HTTP {response.StatusCode}: {errorContent}");
            }
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Network error during token refresh");
            return TokenRefreshResult.Failed($"Network error: {ex.Message}");
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Token refresh request timed out");
            return TokenRefreshResult.Failed("Request timeout");
        }
        catch (JsonException ex)
        {
            _logger.LogError(ex, "Failed to parse token refresh response");
            return TokenRefreshResult.Failed("Invalid response format");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error during token refresh");
            return TokenRefreshResult.Failed($"Unexpected error: {ex.Message}");
        }
    }

    /// <summary>
    /// Clears stored tokens when they are determined to be invalid
    /// </summary>
    private async Task ClearInvalidTokensAsync()
    {
        try
        {
            await _tokenStorage.ClearTokenAsync();
            await _tokenStorage.ClearRefreshTokenAsync();
            _logger.LogDebug("Cleared invalid tokens from storage");
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to clear invalid tokens from storage");
        }
    }

    /// <summary>
    /// Gets device information for tracking refresh token usage
    /// </summary>
    private async Task<string> GetDeviceInfoAsync()
    {
        try
        {
            var userAgent = await _tokenStorage.GetUserDataAsync("user_agent");
            var deviceInfo = await _tokenStorage.GetUserDataAsync("device_info");

            if (!string.IsNullOrEmpty(deviceInfo))
            {
                return deviceInfo;
            }

            // Generate basic device info
            var applicationContext = _tokenStorage.ApplicationContext;
            var platform = Environment.OSVersion.Platform.ToString();
            var version = Environment.OSVersion.Version.ToString();

            return $"{applicationContext} on {platform} {version}";
        }
        catch
        {
            return _tokenStorage.ApplicationContext ?? "Unknown Device";
        }
    }

    /// <summary>
    /// Notifies listeners that authentication is required
    /// </summary>
    private void NotifyAuthenticationRequired()
    {
        try
        {
            OnAuthenticationRequired?.Invoke();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error notifying authentication required event");
        }
    }
}