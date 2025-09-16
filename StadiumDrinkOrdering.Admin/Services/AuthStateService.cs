using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.Authentication.Constants;
using StadiumDrinkOrdering.Shared.Authentication.Utilities;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;
using UserRoleEnum = StadiumDrinkOrdering.Shared.Models.UserRole;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Admin-specific implementation of the unified authentication state service
/// </summary>
public class AuthStateService : IAuthenticationStateService, IAuthStateService
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly IJSRuntime _jsRuntime;
    private bool _initialized = false;
    private AuthenticationState _state = new();

    public AuthenticationState State => _state;
    public bool IsAuthenticated => _state.IsAuthenticated;
    public string? UserEmail => _state.Email;
    public string? UserRole => _state.Role;
    public string? UserId => _state.UserId;

    public event Action<AuthenticationState>? OnAuthenticationStateChanged;

    // Legacy event for backward compatibility
    event Action? IAuthStateService.OnAuthenticationStateChanged
    {
        add => _legacyAuthChanged += value;
        remove => _legacyAuthChanged -= value;
    }
    private event Action? _legacyAuthChanged;

    public AuthStateService(ITokenStorageService tokenStorage, IJSRuntime jsRuntime)
    {
        _tokenStorage = tokenStorage;
        _jsRuntime = jsRuntime;

        // Subscribe to token expiration events
        _tokenStorage.OnTokenExpired += HandleTokenExpired;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            var tokenInfo = await _tokenStorage.GetTokenInfoAsync();

            if (tokenInfo?.Token != null && await _tokenStorage.IsTokenValidAsync())
            {
                await UpdateAuthenticationStateFromToken(tokenInfo.Token);
            }
            else
            {
                await LogoutAsync();
            }
        }
        catch (Exception)
        {
            // Handle initialization errors by logging out
            await LogoutAsync();
        }

        _initialized = true;
        OnAuthenticationStateChanged?.Invoke(_state);
        _legacyAuthChanged?.Invoke();
    }

    public async Task<bool> LoginAsync(AuthenticationResult authResult)
    {
        if (authResult == null || string.IsNullOrEmpty(authResult.Token))
            return false;

        try
        {
            // Store the token
            await _tokenStorage.StoreTokenAsync(
                authResult.Token,
                authResult.ExpiresAt,
                authResult.User?.Email);

            // Store user data if available
            if (authResult.User != null)
            {
                var userDataJson = JsonSerializer.Serialize(authResult.User, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
                await _tokenStorage.StoreUserDataAsync("profile", userDataJson);
            }

            // Update authentication state
            await UpdateAuthenticationStateFromToken(authResult.Token);

            OnAuthenticationStateChanged?.Invoke(_state);
            _legacyAuthChanged?.Invoke();
            return true;
        }
        catch (Exception)
        {
            await LogoutAsync();
            return false;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            await _tokenStorage.ClearTokenAsync();

            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin
            };

            OnAuthenticationStateChanged?.Invoke(_state);
            _legacyAuthChanged?.Invoke();
        }
        catch (Exception)
        {
            // Even if localStorage operations fail, clear the in-memory state
            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin
            };
            OnAuthenticationStateChanged?.Invoke(_state);
            _legacyAuthChanged?.Invoke();
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        // For this implementation, we don't have refresh token functionality
        // Just validate the current token
        if (!await _tokenStorage.IsTokenValidAsync())
        {
            await LogoutAsync();
            return false;
        }

        return true;
    }

    public async Task<bool> ValidateAuthenticationAsync()
    {
        var isValid = await _tokenStorage.IsTokenValidAsync();

        if (!isValid && _state.IsAuthenticated)
        {
            await LogoutAsync();
        }

        return isValid;
    }

    public bool HasRole(string role)
    {
        if (string.IsNullOrEmpty(role) || !IsAuthenticated)
            return false;

        return string.Equals(_state.Role, role, StringComparison.OrdinalIgnoreCase);
    }

    public bool HasAnyRole(params string[] roles)
    {
        if (roles == null || roles.Length == 0 || !IsAuthenticated)
            return false;

        return roles.Any(role => HasRole(role));
    }

    private async Task UpdateAuthenticationStateFromToken(string token)
    {
        try
        {
            var claims = JwtTokenValidator.GetClaims(token);

            _state = new AuthenticationState
            {
                IsAuthenticated = true,
                UserId = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.UserId) ??
                        claims.GetValueOrDefault(AuthenticationConstants.StandardClaims.Subject),
                Email = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Email),
                Role = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Role),
                UserName = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Username),
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Admin,
                AuthenticatedAt = DateTime.UtcNow
            };

            // Store additional claims
            foreach (var claim in claims)
            {
                if (!_state.Claims.ContainsKey(claim.Key))
                {
                    _state.Claims[claim.Key] = claim.Value;
                }
            }
        }
        catch (Exception)
        {
            // If token parsing fails, logout
            await LogoutAsync();
        }
    }

    private async void HandleTokenExpired()
    {
        await LogoutAsync();
    }

    // Legacy methods for backward compatibility
    public async Task LoginAsync(string token, string email)
    {
        var authResult = new AuthenticationResult
        {
            IsSuccess = true,
            Token = token,
            User = new UserDto
            {
                Email = email,
                Id = GetUserIdFromToken(token),
                Role = GetUserRoleFromToken(token)
            }
        };

        await LoginAsync(authResult);
    }

    private static int GetUserIdFromToken(string token)
    {
        var userIdStr = JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.ClaimTypes.UserId) ??
                       JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.StandardClaims.Subject);
        return int.TryParse(userIdStr, out var userId) ? userId : 0;
    }

    private static UserRoleEnum GetUserRoleFromToken(string token)
    {
        var roleStr = JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.ClaimTypes.Role);
        return Enum.TryParse<UserRoleEnum>(roleStr, out var role) ? role : UserRoleEnum.Admin;
    }
}

/// <summary>
/// Legacy interface for backward compatibility
/// </summary>
public interface IAuthStateService
{
    bool IsAuthenticated { get; }
    string? UserEmail { get; }
    event Action? OnAuthenticationStateChanged;
    Task InitializeAsync();
    Task LoginAsync(string token, string email);
    Task LogoutAsync();
}