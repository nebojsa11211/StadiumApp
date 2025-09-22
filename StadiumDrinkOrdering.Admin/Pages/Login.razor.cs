using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Login
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private IAuthenticationStateService AuthService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    private string email = "";
    private string password = "";
    private string? errorMessage;
    private bool isLoading;
    private string? returnUrl;

    protected override async Task OnInitializedAsync()
    {
        // Ensure authentication is initialized
        await AuthService.InitializeAsync();

        // Check if user is already authenticated
        if (AuthService.IsAuthenticated)
        {
            var targetUrl = string.IsNullOrEmpty(returnUrl) ? "/" : Uri.UnescapeDataString(returnUrl);
            if (targetUrl.StartsWith("http"))
            {
                targetUrl = "/";
            }
            NavigationManager.NavigateTo(targetUrl);
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
        StateHasChanged(); // Force UI update

        try
        {
            var loginDto = new LoginDto { Email = email, Password = password };
            var loginResponse = await ApiService.LoginFullAsync(loginDto);

            if (loginResponse != null && !string.IsNullOrEmpty(loginResponse.Token))
            {
                Console.WriteLine($"Login response received: Token={loginResponse.Token[..10]}..., ExpiresAt={loginResponse.ExpiresAt}");

                // Create proper AuthenticationResult with expiration date
                var authResult = new AuthenticationResult
                {
                    IsSuccess = true,
                    Token = loginResponse.Token,
                    ExpiresAt = loginResponse.ExpiresAt,
                    User = loginResponse.User ?? new UserDto
                    {
                        Email = email,
                        // Additional user info from the response
                    }
                };

                Console.WriteLine($"AuthResult created with ExpiresAt: {authResult.ExpiresAt}");

                // Use the proper authentication service to store the token
                var loginSuccess = await AuthService.LoginAsync(authResult);

                if (loginSuccess)
                {
                    // Log the successful login
                    await ApiService.LogUserActionAsync("AdminLogin", "Authentication", details: $"Admin user {email} logged in successfully");

                    Console.WriteLine($"Login successful for {email}, navigating...");

                    // Navigate without force reload to maintain state
                    isLoading = false;
                    StateHasChanged();

                    // Use a small delay to ensure everything is updated
                    await Task.Delay(100);

                    // Navigate to the return URL or dashboard
                    var targetUrl = string.IsNullOrEmpty(returnUrl) ? "/" : Uri.UnescapeDataString(returnUrl);
                    if (targetUrl.StartsWith("http"))
                    {
                        targetUrl = "/";
                    }

                    NavigationManager.NavigateTo(targetUrl);
                }
                else
                {
                    errorMessage = "Failed to store authentication. Please try again.";
                    Console.WriteLine("Failed to store authentication state");
                }
            }
            else
            {
                errorMessage = "Invalid credentials. Please try again.";
                Console.WriteLine("Login failed: No valid response from API");
                await ApiService.LogUserActionAsync("AdminLoginFailed", "Authentication", details: $"Failed login attempt for {email}");
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Login failed: {ex.Message}";
            Console.WriteLine($"Login error: {ex.Message}");
            await ApiService.LogUserActionAsync("AdminLoginError", "Authentication", details: $"Login error for {email}: {ex.Message}");
        }
        finally
        {
            isLoading = false;
            StateHasChanged(); // Force final UI update
        }
    }

}