using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Login
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private string email = "";
    private string password = "";
    private string? errorMessage;
    private bool isLoading;
    private string? returnUrl;

    protected override void OnInitialized()
    {
        // Check if user is already authenticated
        if (AuthStateService.IsAuthenticated)
        {
            NavigateToReturnUrl();
            return;
        }

        // Get return URL from query string
        var uri = new Uri(NavigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        returnUrl = query["returnUrl"];
    }

    private async Task HandleLogin()
    {
        isLoading = true;
        errorMessage = null;

        try
        {
            var loginDto = new LoginDto { Email = email, Password = password };
            var result = await ApiService.LoginAsync(loginDto);
            
            if (!string.IsNullOrEmpty(result?.Token))
            {
                await AuthStateService.LoginAsync(result.Token, email);
                await ApiService.LogUserActionAsync("AdminLogin", "Authentication", $"Admin user {email} logged in successfully");
                NavigateToReturnUrl();
            }
            else
            {
                errorMessage = "Invalid credentials. Please try again.";
                await ApiService.LogUserActionAsync("AdminLoginFailed", "Authentication", $"Failed login attempt for {email}");
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Login failed: {ex.Message}";
            await ApiService.LogUserActionAsync("AdminLoginError", "Authentication", $"Login error for {email}: {ex.Message}");
        }
        finally
        {
            isLoading = false;
        }
    }

    private void NavigateToReturnUrl()
    {
        var targetUrl = "/";
        
        if (!string.IsNullOrEmpty(returnUrl))
        {
            try
            {
                targetUrl = Uri.UnescapeDataString(returnUrl);
                // Ensure it's a relative URL for security
                if (targetUrl.StartsWith("http"))
                {
                    targetUrl = "/";
                }
            }
            catch
            {
                targetUrl = "/";
            }
        }

        NavigationManager.NavigateTo(targetUrl, forceLoad: true);
    }
}