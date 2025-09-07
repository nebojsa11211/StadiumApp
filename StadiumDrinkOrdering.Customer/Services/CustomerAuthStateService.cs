using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Customer.Services;

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

public class CustomerAuthStateService : ICustomerAuthStateService
{
    private readonly ICustomerTokenStorageService _tokenStorage;
    private readonly IApiService _apiService;
    private readonly IJSRuntime _jsRuntime;
    private bool _initialized = false;
    
    private const string TokenKey = "customer_auth_token";
    private const string UserDataKey = "customer_user_data";

    public bool IsAuthenticated => !string.IsNullOrEmpty(_tokenStorage.Token) && CurrentUser != null;
    public UserDto? CurrentUser { get; private set; }
    public string? UserEmail => CurrentUser?.Email;

    public event Action? OnAuthenticationStateChanged;

    public CustomerAuthStateService(
        ICustomerTokenStorageService tokenStorage, 
        IApiService apiService,
        IJSRuntime jsRuntime)
    {
        _tokenStorage = tokenStorage;
        _apiService = apiService;
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
            var userDataJson = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", UserDataKey);
            
            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userDataJson))
            {
                var userData = System.Text.Json.JsonSerializer.Deserialize<UserDto>(userDataJson, new System.Text.Json.JsonSerializerOptions
                {
                    PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                });

                if (userData != null)
                {
                    _tokenStorage.Token = token;
                    _apiService.Token = token;
                    CurrentUser = userData;
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing auth state: {ex.Message}");
            await LogoutAsync();
        }

        _initialized = true;
        OnAuthenticationStateChanged?.Invoke();
    }

    public async Task LoginAsync(string token, UserDto user)
    {
        try
        {
            _tokenStorage.Token = token;
            _apiService.Token = token;
            CurrentUser = user;
            
            var userDataJson = System.Text.Json.JsonSerializer.Serialize(user, new System.Text.Json.JsonSerializerOptions
            {
                PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
            });

            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserDataKey, userDataJson);
            
            OnAuthenticationStateChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during login: {ex.Message}");
            throw;
        }
    }

    public async Task LogoutAsync()
    {
        try
        {
            _tokenStorage.ClearToken();
            _apiService.Token = null;
            CurrentUser = null;
            
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", UserDataKey);
            
            OnAuthenticationStateChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during logout: {ex.Message}");
        }
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
                    var userDataJson = System.Text.Json.JsonSerializer.Serialize(refreshedUser, new System.Text.Json.JsonSerializerOptions
                    {
                        PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase
                    });
                    await _jsRuntime.InvokeVoidAsync("localStorage.setItem", UserDataKey, userDataJson);
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
}