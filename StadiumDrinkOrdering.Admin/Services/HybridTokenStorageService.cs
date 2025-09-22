using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.Authentication.Constants;
using StadiumDrinkOrdering.Shared.Authentication.Utilities;
using System.Text.Json;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Hybrid token storage service that supports both client-side localStorage
/// and server-side session state for Blazor Server applications.
/// This enables authentication to work for both interactive client operations
/// and server-side operations like file uploads.
/// </summary>
public class HybridTokenStorageService : ITokenStorageService, IDisposable
{
    private readonly IJSRuntime _jsRuntime;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly ILogger<HybridTokenStorageService> _logger;
    private readonly object _lock = new();
    private string? _token;
    private string? _refreshToken;
    private TokenInfo? _tokenInfo;
    private Timer? _expirationTimer;
    private Timer? _refreshExpirationTimer;

    // Session keys for server-side storage
    private const string SESSION_TOKEN_KEY = "auth_token";
    private const string SESSION_TOKEN_EXPIRATION_KEY = "auth_token_expiration";
    private const string SESSION_REFRESH_TOKEN_KEY = "auth_refresh_token";
    private const string SESSION_REFRESH_TOKEN_EXPIRATION_KEY = "auth_refresh_token_expiration";
    private const string SESSION_USER_EMAIL_KEY = "auth_user_email";

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
                _ = Task.Run(() => SyncTokenToSession(value));
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
                _ = Task.Run(() => SyncRefreshTokenToSession(value));
            }
        }
    }

    public event Action? OnTokenExpired;
    public event Action? OnRefreshTokenExpired;

    public HybridTokenStorageService(
        IJSRuntime jsRuntime,
        IHttpContextAccessor httpContextAccessor,
        ILogger<HybridTokenStorageService> logger)
    {
        _jsRuntime = jsRuntime;
        _httpContextAccessor = httpContextAccessor;
        _logger = logger;
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

        // Store in both localStorage and session
        await StoreInLocalStorage(token, expiresAt, userEmail);
        StoreInSession(token, expiresAt, userEmail);
    }

    public async Task<string?> GetTokenAsync()
    {
        // First try memory cache
        if (!string.IsNullOrEmpty(_token))
        {
            _logger.LogTrace("Token retrieved from memory cache");
            return _token;
        }

        // Try session state first (server-side operations)
        var sessionToken = GetFromSession(SESSION_TOKEN_KEY);
        if (!string.IsNullOrEmpty(sessionToken))
        {
            _logger.LogTrace("Token retrieved from session state");
            lock (_lock)
            {
                _token = sessionToken;
                UpdateTokenInfo();
            }
            return sessionToken;
        }

        // Fall back to localStorage (client-side operations)
        try
        {
            var storedToken = await GetFromLocalStorage();
            if (!string.IsNullOrEmpty(storedToken))
            {
                _logger.LogTrace("Token retrieved from localStorage");
                lock (_lock)
                {
                    _token = storedToken;
                    UpdateTokenInfo();
                }

                // Sync to session for future server-side access
                await SyncTokenToSession(storedToken);
            }

            return storedToken;
        }
        catch (Exception ex)
        {
            _logger.LogDebug(ex, "Failed to retrieve token from localStorage, likely server-side context");
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

            var expirationStr = GetFromSession(SESSION_TOKEN_EXPIRATION_KEY);
            var userEmail = GetFromSession(SESSION_USER_EMAIL_KEY);

            // Try localStorage if session is empty
            if (string.IsNullOrEmpty(expirationStr))
            {
                try
                {
                    expirationStr = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem",
                        AuthenticationConstants.StorageKeys.Admin.TokenExpiration);
                    userEmail = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem",
                        AuthenticationConstants.StorageKeys.Admin.Email);
                }
                catch (Exception ex)
                {
                    _logger.LogTrace(ex, "Cannot access localStorage, using JWT token data only");
                }
            }

            DateTime? expiresAt = null;
            if (DateTime.TryParse(expirationStr, out var parsedDate))
            {
                expiresAt = parsedDate;
            }
            else
            {
                // Extract from token
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get token info");
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

        // Clear from both localStorage and session
        await Task.WhenAll(
            ClearFromLocalStorage(),
            Task.Run(ClearFromSession)
        );
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
        var sessionKey = $"user_data_{key}";

        await Task.WhenAll(
            StoreUserDataInLocalStorage(userDataKey, value),
            Task.Run(() => StoreInSession(sessionKey, value))
        );
    }

    public async Task<string?> GetUserDataAsync(string key)
    {
        if (string.IsNullOrEmpty(key))
            return null;

        var sessionKey = $"user_data_{key}";
        var sessionValue = GetFromSession(sessionKey);
        if (!string.IsNullOrEmpty(sessionValue))
            return sessionValue;

        var userDataKey = $"{AuthenticationConstants.StorageKeys.Admin.UserData}_{key}";
        return await GetUserDataFromLocalStorage(userDataKey);
    }

    public async Task ClearUserDataAsync()
    {
        await Task.WhenAll(
            ClearUserDataFromLocalStorage(),
            Task.Run(ClearUserDataFromSession)
        );
    }

    public async Task StoreRefreshTokenAsync(string refreshToken, DateTime? expiresAt = null)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new ArgumentException("Refresh token cannot be null or empty", nameof(refreshToken));

        lock (_lock)
        {
            _refreshToken = refreshToken;
        }

        await Task.WhenAll(
            StoreRefreshTokenInLocalStorage(refreshToken, expiresAt),
            Task.Run(() => StoreRefreshTokenInSession(refreshToken, expiresAt))
        );
    }

    public async Task<string?> GetRefreshTokenAsync()
    {
        if (!string.IsNullOrEmpty(_refreshToken))
            return _refreshToken;

        // Try session first
        var sessionRefreshToken = GetFromSession(SESSION_REFRESH_TOKEN_KEY);
        if (!string.IsNullOrEmpty(sessionRefreshToken))
        {
            lock (_lock)
            {
                _refreshToken = sessionRefreshToken;
            }
            return sessionRefreshToken;
        }

        // Fall back to localStorage
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
                // Sync to session
                await SyncRefreshTokenToSession(storedRefreshToken);
            }

            return storedRefreshToken;
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot access localStorage for refresh token");
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

        await Task.WhenAll(
            ClearRefreshTokenFromLocalStorage(),
            Task.Run(ClearRefreshTokenFromSession)
        );
    }

    public async Task<bool> IsRefreshTokenValidAsync()
    {
        var refreshToken = await GetRefreshTokenAsync();
        if (string.IsNullOrEmpty(refreshToken))
            return false;

        try
        {
            var expirationStr = GetFromSession(SESSION_REFRESH_TOKEN_EXPIRATION_KEY);
            if (string.IsNullOrEmpty(expirationStr))
            {
                expirationStr = await _jsRuntime.InvokeAsync<string?>(
                    "localStorage.getItem", AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration);
            }

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
            var expirationStr = GetFromSession(SESSION_REFRESH_TOKEN_EXPIRATION_KEY);
            if (string.IsNullOrEmpty(expirationStr))
            {
                expirationStr = _jsRuntime.InvokeAsync<string?>(
                    "localStorage.getItem", AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration)
                    .AsTask().Result;
            }

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
        await Task.WhenAll(
            StoreTokenAsync(accessToken, accessTokenExpiresAt, userEmail),
            StoreRefreshTokenAsync(refreshToken, refreshTokenExpiresAt)
        );
    }

    #region Private Helper Methods

    private async Task StoreInLocalStorage(string token, DateTime? expiresAt, string? userEmail)
    {
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
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot access localStorage, likely server-side context");
        }
    }

    private void StoreInSession(string token, DateTime? expiresAt, string? userEmail)
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetString(SESSION_TOKEN_KEY, token);
                if (expiresAt.HasValue)
                {
                    session.SetString(SESSION_TOKEN_EXPIRATION_KEY, expiresAt.Value.ToString("O"));
                }
                if (!string.IsNullOrEmpty(userEmail))
                {
                    session.SetString(SESSION_USER_EMAIL_KEY, userEmail);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot access session state");
        }
    }

    private async Task<string?> GetFromLocalStorage()
    {
        return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem",
            AuthenticationConstants.StorageKeys.Admin.Token);
    }

    private string? GetFromSession(string key)
    {
        try
        {
            return _httpContextAccessor.HttpContext?.Session?.GetString(key);
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot access session state for key: {Key}", key);
            return null;
        }
    }

    private void StoreInSession(string key, string? value)
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null && !string.IsNullOrEmpty(value))
            {
                session.SetString(key, value);
            }
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot store in session for key: {Key}", key);
        }
    }

    private async Task SyncTokenToSession(string? token)
    {
        if (!string.IsNullOrEmpty(token))
        {
            StoreInSession(SESSION_TOKEN_KEY, token);
        }
    }

    private async Task SyncRefreshTokenToSession(string? refreshToken)
    {
        if (!string.IsNullOrEmpty(refreshToken))
        {
            StoreInSession(SESSION_REFRESH_TOKEN_KEY, refreshToken);
        }
    }

    private async Task ClearFromLocalStorage()
    {
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
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot clear localStorage");
        }
    }

    private void ClearFromSession()
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(SESSION_TOKEN_KEY);
                session.Remove(SESSION_TOKEN_EXPIRATION_KEY);
                session.Remove(SESSION_REFRESH_TOKEN_KEY);
                session.Remove(SESSION_REFRESH_TOKEN_EXPIRATION_KEY);
                session.Remove(SESSION_USER_EMAIL_KEY);
            }
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot clear session state");
        }
    }

    private async Task StoreUserDataInLocalStorage(string key, string value)
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", key, value);
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot store user data in localStorage");
        }
    }

    private async Task<string?> GetUserDataFromLocalStorage(string key)
    {
        try
        {
            return await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", key);
        }
        catch (Exception)
        {
            return null;
        }
    }

    private async Task ClearUserDataFromLocalStorage()
    {
        try
        {
            var userDataPrefix = AuthenticationConstants.StorageKeys.Admin.UserData;
            await _jsRuntime.InvokeVoidAsync("eval", $@"
                Object.keys(localStorage).forEach(key => {{
                    if (key.startsWith('{userDataPrefix}_')) {{
                        localStorage.removeItem(key);
                    }}
                }});
            ");
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot clear user data from localStorage");
        }
    }

    private void ClearUserDataFromSession()
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                var keysToRemove = new List<string>();
                foreach (var key in session.Keys)
                {
                    if (key.StartsWith("user_data_"))
                    {
                        keysToRemove.Add(key);
                    }
                }
                foreach (var key in keysToRemove)
                {
                    session.Remove(key);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot clear user data from session");
        }
    }

    private async Task StoreRefreshTokenInLocalStorage(string refreshToken, DateTime? expiresAt)
    {
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
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot store refresh token in localStorage");
        }
    }

    private void StoreRefreshTokenInSession(string refreshToken, DateTime? expiresAt)
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.SetString(SESSION_REFRESH_TOKEN_KEY, refreshToken);
                if (expiresAt.HasValue)
                {
                    session.SetString(SESSION_REFRESH_TOKEN_EXPIRATION_KEY, expiresAt.Value.ToString("O"));
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot store refresh token in session");
        }
    }

    private async Task ClearRefreshTokenFromLocalStorage()
    {
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.RefreshToken);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem",
                AuthenticationConstants.StorageKeys.Admin.RefreshTokenExpiration);
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot clear refresh token from localStorage");
        }
    }

    private void ClearRefreshTokenFromSession()
    {
        try
        {
            var session = _httpContextAccessor.HttpContext?.Session;
            if (session != null)
            {
                session.Remove(SESSION_REFRESH_TOKEN_KEY);
                session.Remove(SESSION_REFRESH_TOKEN_EXPIRATION_KEY);
            }
        }
        catch (Exception ex)
        {
            _logger.LogTrace(ex, "Cannot clear refresh token from session");
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

    #endregion

    public void Dispose()
    {
        _expirationTimer?.Dispose();
        _refreshExpirationTimer?.Dispose();
        GC.SuppressFinalize(this);
    }
}