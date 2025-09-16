using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.Authentication.Constants;
using StadiumDrinkOrdering.Shared.Authentication.Utilities;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using System.Text.Json;
using UserRoleEnum = StadiumDrinkOrdering.Shared.Models.UserRole;

namespace StadiumDrinkOrdering.Staff.Services;

/// <summary>
/// Staff-specific implementation of the unified authentication state service
/// </summary>
public class AuthStateService : IAuthenticationStateService, IAuthStateService
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly IStaffApiService _apiService;
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
        add => _legacyAuthStateChanged += value;
        remove => _legacyAuthStateChanged -= value;
    }

    private event Action? _legacyAuthStateChanged;

    public AuthStateService(ITokenStorageService tokenStorage, IStaffApiService apiService, IJSRuntime jsRuntime)
    {
        _tokenStorage = tokenStorage;
        _apiService = apiService;
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
                // Set token in API service for backward compatibility
                _apiService.Token = tokenInfo.Token;

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
        _legacyAuthStateChanged?.Invoke();
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

            // Set token in API service for backward compatibility
            _apiService.Token = authResult.Token;

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
            _legacyAuthStateChanged?.Invoke();
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
            _apiService.Token = null;

            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Staff
            };

            OnAuthenticationStateChanged?.Invoke(_state);
            _legacyAuthStateChanged?.Invoke();
        }
        catch (Exception)
        {
            // Even if localStorage operations fail, clear the in-memory state
            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Staff
            };
            OnAuthenticationStateChanged?.Invoke(_state);
            _legacyAuthStateChanged?.Invoke();
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
                Role = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Role) ??
                          AuthenticationConstants.Roles.Staff,
                UserName = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Username),
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Staff,
                AuthenticatedAt = DateTime.UtcNow,
                Claims = claims
            };
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
        var userId = JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.ClaimTypes.UserId) ??
                     JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.StandardClaims.Subject);
        var roleString = JwtTokenValidator.GetClaimValue(token, AuthenticationConstants.ClaimTypes.Role) ??
                         "Bartender";

        var authResult = new AuthenticationResult
        {
            IsSuccess = true,
            Token = token,
            User = new UserDto
            {
                Email = email,
                Id = int.TryParse(userId, out var id) ? id : 0,
                Role = Enum.TryParse<UserRoleEnum>(roleString, true, out var role) ? role : UserRoleEnum.Bartender
            }
        };

        await LoginAsync(authResult);
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