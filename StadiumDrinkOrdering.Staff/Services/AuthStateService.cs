using Microsoft.JSInterop;

namespace StadiumDrinkOrdering.Staff.Services;

public interface IAuthStateService
{
    bool IsAuthenticated { get; }
    string? UserEmail { get; }
    event Action? OnAuthenticationStateChanged;
    Task InitializeAsync();
    Task LoginAsync(string token, string email);
    Task LogoutAsync();
}

public class AuthStateService : IAuthStateService
{
    private readonly IStaffApiService _apiService;
    private readonly IJSRuntime _jsRuntime;
    private bool _initialized = false;
    
    private const string TokenKey = "staff_auth_token";
    private const string EmailKey = "staff_user_email";

    public bool IsAuthenticated => !string.IsNullOrEmpty(_apiService.Token);
    public string? UserEmail { get; private set; }

    public event Action? OnAuthenticationStateChanged;

    public AuthStateService(IStaffApiService apiService, IJSRuntime jsRuntime)
    {
        _apiService = apiService;
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeAsync()
    {
        if (_initialized) return;

        try
        {
            var token = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", TokenKey);
            var email = await _jsRuntime.InvokeAsync<string?>("localStorage.getItem", EmailKey);
            
            if (!string.IsNullOrEmpty(token))
            {
                _apiService.Token = token;
                UserEmail = email;
            }
        }
        catch (Exception)
        {
            // Handle cases where localStorage is not available (e.g., prerendering)
        }

        _initialized = true;
        OnAuthenticationStateChanged?.Invoke();
    }

    public async Task LoginAsync(string token, string email)
    {
        _apiService.Token = token;
        UserEmail = email;
        
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", TokenKey, token);
            await _jsRuntime.InvokeVoidAsync("localStorage.setItem", EmailKey, email);
        }
        catch (Exception)
        {
            // Handle localStorage errors silently
        }
        
        OnAuthenticationStateChanged?.Invoke();
    }

    public async Task LogoutAsync()
    {
        _apiService.Token = null;
        UserEmail = null;
        
        try
        {
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", TokenKey);
            await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", EmailKey);
        }
        catch (Exception)
        {
            // Handle localStorage errors silently
        }
        
        OnAuthenticationStateChanged?.Invoke();
    }
}