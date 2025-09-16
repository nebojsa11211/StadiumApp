using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;
using StadiumDrinkOrdering.Shared.Authentication.Constants;
using StadiumDrinkOrdering.Shared.Authentication.Utilities;
using System.Text.Json;

namespace StadiumDrinkOrdering.Customer.Services;

/// <summary>
/// Customer-specific implementation of the unified authentication state service
/// </summary>
public class CustomerAuthStateService : IAuthenticationStateService, ICustomerAuthStateService
{
    private readonly ITokenStorageService _tokenStorage;
    private readonly IApiService _apiService;
    private readonly IJSRuntime _jsRuntime;
    private bool _initialized = false;
    private AuthenticationState _state = new();

    public AuthenticationState State => _state;
    public bool IsAuthenticated => _state.IsAuthenticated;
    public string? UserEmail => _state.Email;
    public string? UserRole => _state.Role;
    public string? UserId => _state.UserId;
    public UserDto? CurrentUser { get; private set; }

    public event Action<AuthenticationState>? OnAuthenticationStateChanged;

    // Legacy event for backward compatibility
    event Action? ICustomerAuthStateService.OnAuthenticationStateChanged
    {
        add => _legacyAuthChanged += value;
        remove => _legacyAuthChanged -= value;
    }
    private event Action? _legacyAuthChanged;

    public CustomerAuthStateService(
        ITokenStorageService tokenStorage,
        IApiService apiService,
        IJSRuntime jsRuntime)
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
                await LoadUserProfileAsync();
            }
            else
            {
                await LogoutAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing auth state: {ex.Message}");
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

            // Set token in API service for backward compatibility
            _apiService.Token = authResult.Token;

            // Store user data if available
            if (authResult.User != null)
            {
                var userDto = new UserDto
                {
                    Id = authResult.User.Id,
                    Email = authResult.User.Email ?? "",
                    Username = authResult.User.Username ?? "",
                    Role = authResult.User.Role
                };

                CurrentUser = userDto;

                var userDataJson = JsonSerializer.Serialize(userDto, new JsonSerializerOptions
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
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
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
            CurrentUser = null;

            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Customer
            };

            OnAuthenticationStateChanged?.Invoke(_state);
            _legacyAuthChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during logout: {ex.Message}");
            // Even if localStorage operations fail, clear the in-memory state
            _state = new AuthenticationState
            {
                IsAuthenticated = false,
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Customer
            };
            OnAuthenticationStateChanged?.Invoke(_state);
            _legacyAuthChanged?.Invoke();
        }
    }

    public async Task<bool> RefreshTokenAsync()
    {
        // For this implementation, we don't have refresh token functionality
        // Just validate the current token and refresh user data
        if (!await _tokenStorage.IsTokenValidAsync())
        {
            await LogoutAsync();
            return false;
        }

        // Refresh user profile data
        await LoadUserProfileAsync();
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

    public async Task<UserDto?> GetCurrentUserAsync()
    {
        if (!IsAuthenticated) return null;

        try
        {
            if (CurrentUser != null)
            {
                var refreshedUser = await _apiService.GetUserByIdAsync(CurrentUser.Id);
                if (refreshedUser != null)
                {
                    CurrentUser = refreshedUser;
                    var userDataJson = JsonSerializer.Serialize(refreshedUser, new JsonSerializerOptions
                    {
                        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                    });
                    await _tokenStorage.StoreUserDataAsync("profile", userDataJson);
                }
                return CurrentUser;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error refreshing user data: {ex.Message}");
        }

        return CurrentUser;
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
                          AuthenticationConstants.Roles.Customer,
                UserName = claims.GetValueOrDefault(AuthenticationConstants.ClaimTypes.Username),
                ApplicationContext = AuthenticationConstants.ApplicationContexts.Customer,
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

    private async Task LoadUserProfileAsync()
    {
        try
        {
            var userDataJson = await _tokenStorage.GetUserDataAsync("profile");
            if (!string.IsNullOrEmpty(userDataJson))
            {
                CurrentUser = JsonSerializer.Deserialize<UserDto>(userDataJson, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading user profile: {ex.Message}");
        }
    }

    private async void HandleTokenExpired()
    {
        await LogoutAsync();
    }

    // Legacy methods for backward compatibility
    public async Task LoginAsync(string token, UserDto user)
    {
        var authResult = new AuthenticationResult
        {
            IsSuccess = true,
            Token = token,
            User = user
        };

        await LoginAsync(authResult);
    }
}

/// <summary>
/// Legacy interface for backward compatibility
/// </summary>
public interface ICustomerAuthStateService
{
    bool IsAuthenticated { get; }
    UserDto? CurrentUser { get; }
    string? UserEmail { get; }
    event Action? OnAuthenticationStateChanged;
    Task InitializeAsync();
    Task LoginAsync(string token, UserDto user);
    Task LogoutAsync();
    Task<UserDto?> GetCurrentUserAsync();
}