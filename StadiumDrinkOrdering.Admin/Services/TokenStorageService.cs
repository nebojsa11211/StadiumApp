using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.Authentication.Constants;
using StadiumDrinkOrdering.Shared.Authentication.Utilities;
using System.Text.Json;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Admin-specific implementation of the unified token storage service
/// </summary>
public class TokenStorageService : ITokenStorageService
{
    private readonly IJSRuntime _jsRuntime;
    private readonly object _lock = new();
    private string? _token;
    private string? _refreshToken;
    private TokenInfo? _tokenInfo;
    private Timer? _expirationTimer;
    private Timer? _refreshExpirationTimer;

    public string ApplicationContext => AuthenticationConstants.ApplicationContexts.Admin;

    public string? Token
    {
        get
        {
            lock (_lock)
            {
                return _token;
            }
        }
        set
        {
            lock (_lock)
            {
                _token = value;
                UpdateTokenInfo();
            }
        }
    }

    public TokenInfo? TokenInfo
    {
        get
        {
            lock (_lock)
            {
                return _tokenInfo;
            }
        }
    }

    public string? RefreshToken
    {
        get
        {
            lock (_lock)
            {
                return _refreshToken;
            }
        }
        set
        {
            lock (_lock)
            {
                _refreshToken = value;
            }
        }
    }

    public event Action? OnTokenExpired;
    public event Action? OnRefreshTokenExpired;

    public TokenStorageService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task StoreTokenAsync(string token, DateTime? expiresAt = null, string? userEmail = null)
    {
        if (string.IsNullOrEmpty(token))
            throw new ArgumentException("Token cannot be null or empty", nameof(token));

        lock (_lock)
        {
            _token = token;

            // Extract expiration from token if not provided
            if (!expiresAt.HasValue)
            {
                expiresAt = JwtTokenValidator.GetTokenExpiration(token);
            }

            _tokenInfo = TokenInfo.Create(
                token,
                expiresAt ?? DateTime.UtcNow.AddDays(1),
                userEmail,
                null
            );

            SetupExpirationTimer();
        }

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
                AuthenticationConstants.StorageKeys.Admin.Token, token);

            if (expiresAt.HasValue)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
                    AuthenticationConstants.StorageKeys.Admin.TokenExpiration,
                    expiresAt.Value.ToString("O"));
            }

            if (!string.IsNullOrEmpty(userEmail))
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
                    AuthenticationConstants.StorageKeys.Admin.Email, userEmail);
            }
        }
        catch (Exception)
        {
            // Handle localStorage errors silently during prerendering
        }
    }

    public async Task<string?> GetTokenAsync()
    {
        if (!string.IsNullOrEmpty(_token))
            return _token;

        try
        {
            // Add a small delay to ensure JSRuntime is ready
            await Task.Delay(50);

            var storedToken = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem",
                AuthenticationConstants.StorageKeys.Admin.Token);

            if (!string.IsNullOrEmpty(storedToken))
            {
                lock (_lock)
                {
                    _token = storedToken;
                    UpdateTokenInfo();
                }
            }

            return storedToken;
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("prerendering") || ex.Message.Contains("JSRuntime"))
        {
            Console.WriteLine($"TokenStorageService.GetTokenAsync: JSRuntime not available during prerendering");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"TokenStorageService.GetTokenAsync error: {ex.Message}");
            return null;
        }
    }

    public async Task<TokenInfo?> GetTokenInfoAsync()
    {
        if (_tokenInfo != null)
            return _tokenInfo;

        try
        {
            var token = await GetTokenAsync();
            if (string.IsNullOrEmpty(token))
                return null;

            var expirationStr = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem",
                AuthenticationConstants.StorageKeys.Admin.TokenExpiration);
            var userEmail = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem",
                AuthenticationConstants.StorageKeys.Admin.Email);

            DateTime? expiresAt = null;
            if (DateTime.TryParse(expirationStr, out var parsedDate))
            {
                expiresAt = parsedDate;
            }
            else
            {
                // Try to extract from token
                expiresAt = JwtTokenValidator.GetTokenExpiration(token);
            }

            lock (_lock)
            {
                _tokenInfo = TokenInfo.Create(
                    token,
                    expiresAt ?? DateTime.UtcNow.AddDays(1),
                    userEmail,
                    null
                );
            }

            return _tokenInfo;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task ClearTokenAsync()
    {
        lock (_lock)
        {
            _token = null;
            _refreshToken = null;
            _tokenInfo = null;
            _expirationTimer?.Dispose();
            _expirationTimer = null;
            _refreshExpirationTimer?.Dispose();
            _refreshExpirationTimer = null;
        }

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.Token);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.TokenExpiration);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.Email);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.RefreshToken);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.UserData);
        }
        catch (Exception)
        {
            // Handle localStorage errors silently
        }
    }

    public async Task<bool> IsTokenValidAsync()
    {
        var tokenInfo = await GetTokenInfoAsync();

        if (tokenInfo?.Token == null)
            return false;

        if (tokenInfo.ExpiresAt <= DateTime.UtcNow)
            return false;

        return JwtTokenValidator.IsValidTokenFormat(tokenInfo.Token);
    }

    public TimeSpan? GetTokenTimeToExpiry()
    {
        var tokenInfo = _tokenInfo ?? GetTokenInfoAsync().Result;

        if (tokenInfo == null)
            return null;

        var timeToExpiry = tokenInfo.ExpiresAt - DateTime.UtcNow;
        return timeToExpiry > TimeSpan.Zero ? timeToExpiry : TimeSpan.Zero;
    }

    public async Task StoreUserDataAsync(string key, string value)
    {
        if (string.IsNullOrEmpty(key))
            throw new ArgumentException("Key cannot be null or empty", nameof(key));

        var userDataKey = $"{AuthenticationConstants.StorageKeys.Admin.UserData}_{key}";

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", userDataKey, value);
        }
        catch (Exception)
        {
            // Handle localStorage errors silently
        }
    }

    public async Task<string?> GetUserDataAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            return null;

        var userDataKey = $"{AuthenticationConstants.StorageKeys.Admin.UserData}_{key}";

        try
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", userDataKey);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task ClearUserDataAsync()
    {
        try
        {
            // Clear all user data keys with the prefix
            var userDataPrefix = AuthenticationConstants.StorageKeys.Admin.UserData;
            await _jsRuntime.InvokeVoidAsync("eval", $@"
                Object.keys(localStorage).forEach(key => {{
                    if (key.startsWith('{userDataPrefix}_')) {{
                        localStorage.removeItem(key);
                    }}
                }});
            ");
        }
        catch (Exception)
        {
            // Handle localStorage errors silently
        }
    }

    private void UpdateTokenInfo()
    {
        if (string.IsNullOrEmpty(_token))
        {
            _tokenInfo = null;
            return;
        }

        var expiresAt = JwtTokenValidator.GetTokenExpiration(_token);
        var userEmail = JwtTokenValidator.GetClaimValue(_token, AuthenticationConstants.ClaimTypes.Email);

        _tokenInfo = TokenInfo.Create(
            _token,
            expiresAt ?? DateTime.UtcNow.AddDays(1),
            userEmail,
            null
        );

        SetupExpirationTimer();
    }

    private void SetupExpirationTimer()
    {
        _expirationTimer?.Dispose();

        var timeToExpiry = GetTokenTimeToExpiry();
        if (timeToExpiry.HasValue && timeToExpiry.Value > TimeSpan.Zero)
        {
            _expirationTimer = new Timer(
                callback: _ => OnTokenExpired?.Invoke(),
                state: null,
                dueTime: timeToExpiry.Value,
                period: Timeout.InfiniteTimeSpan);
        }
    }

    public async Task StoreRefreshTokenAsync(string refreshToken, DateTime? expiresAt = null)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));

        lock (_lock)
        {
            _refreshToken = refreshToken;
        }

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
                AuthenticationConstants.StorageKeys.Admin.RefreshToken, refreshToken);

            if (expiresAt.HasValue)
            {
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem",
                    AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration,
                    expiresAt.Value.ToString("O"));
            }
        }
        catch (Exception)
        {
            // Handle localStorage errors silently during prerendering
        }
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        if (!string.IsNullOrEmpty(_refreshToken))
            return _refreshToken;

        try
        {
            var storedRefreshToken = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem", AuthenticationConstants.StorageKeys.Admin.RefreshToken);

            if (!string.IsNullOrEmpty(storedRefreshToken))
            {
                lock (_lock)
                {
                    _refreshToken = storedRefreshToken;
                }
            }

            return storedRefreshToken;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public async Task ClearRefreshTokenAsync()
    {
        lock (_lock)
        {
            _refreshToken = null;
            _refreshExpirationTimer?.Dispose();
            _refreshExpirationTimer = null;
        }

        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.RefreshToken);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration);
        }
        catch (Exception)
        {
            // Handle localStorage errors silently
        }
    }

    public async Task<bool> IsRefreshTokenValidAsync()
    {
        var refreshToken = await GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refreshToken))
            return false;

        try
        {
            var expirationStr = await _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem", AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration);

            if (DateTime.TryParse(expirationStr, out var expiresAt))
            {
                return expiresAt > DateTime.UtcNow;
            }

            return true; // Assume valid if no expiration set
        }
        catch (Exception)
        {
            return false;
        }
    }

    public TimeSpan? GetRefreshTokenTimeToExpiry()
    {
        try
        {
            var expirationStr = _jsRuntime.InvokeAsync<string?>(
                "localStorage.getItem", AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration)
                .AsTask().Result;

            if (DateTime.TryParse(expirationStr, out var expiresAt))
            {
                var timeToExpiry = expiresAt - DateTime.UtcNow;
                return timeToExpiry > TimeSpan.Zero ? timeToExpiry : TimeSpan.Zero;
            }
        }
        catch (Exception)
        {
            // Return null on error
        }

        return null;
    }

    public async Task StoreTokenPairAsync(
        string accessToken,
        string refreshToken,
        DateTime? accessTokenExpiresAt = null,
        DateTime? refreshTokenExpiresAt = null,
        string? userEmail = null)
    {
        await StoreTokenAsync(accessToken, accessTokenExpiresAt, userEmail);
        await StoreRefreshTokenAsync(refreshToken, refreshTokenExpiresAt);
    }

    public void Dispose()
    {
        _expirationTimer?.Dispose();
        _refreshExpirationTimer?.Dispose();
    }
}