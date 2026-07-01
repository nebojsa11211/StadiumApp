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
    private bool showPassword;
    private string? returnUrl;

    private void TogglePasswordVisibility() => showPassword = !showPassword;

    protected override async Task OnInitializedAsync()
    {
        // Ensure authentication is initialized
        await AuthService.InitializeAsync();

        // Get return URL from query string first so it is available for any redirect below
        var uri = new Uri(NavigationManager.Uri);
        var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
        returnUrl = query["returnUrl"];

        // The server rejected the token (401 -> ShowAuthenticationErrorAsync redirected here
        // with sessionExpired=true). The token may STILL pass the client-side validity check
        // (expiry + format), so we must NOT auto-redirect back to the dashboard - that is the
        // infinite dashboard <-> login loop. Force a logout so the rejected token is gone and
        // show the login form.
        if (query["sessionExpired"] == "true")
        {
            await AuthService.LogoutAsync();
            errorMessage = "Your session has expired. Please log in again.";
            return;
        }

        // Only redirect away from /login if the TOKEN STORE actually holds a valid token -
        // the same check AuthRoute uses. Using the raw in-memory IsAuthenticated flag here
        // let Login and AuthRoute disagree (flag stale-true while the token was cleared),
        // producing an infinite dashboard <-> login bounce. ValidateAuthenticationAsync
        // reconciles the in-memory state with storage before we decide.
        if (await AuthService.ValidateAuthenticationAsync())
        {
            NavigationManager.NavigateTo(ResolveReturnUrl());
        }
    }

    // Resolves the post-login destination, preserving the originally requested page.
    // Absolute URLs are only honored when same-origin (prevents open-redirect); anything
    // else (or a loop back to /login) falls back to the dashboard.
    private string ResolveReturnUrl()
    {
        if (string.IsNullOrEmpty(returnUrl))
            return "/";

        var decoded = Uri.UnescapeDataString(returnUrl);

        if (Uri.TryCreate(decoded, UriKind.Absolute, out var absolute))
        {
            var sameOrigin = Uri.Compare(absolute, new Uri(NavigationManager.BaseUri),
                UriComponents.SchemeAndServer, UriFormat.Unescaped, StringComparison.OrdinalIgnoreCase) == 0;

            if (!sameOrigin)
                return "/";

            decoded = absolute.PathAndQuery; // keep only the local path (e.g. /orders)
        }

        if (decoded.StartsWith("/login", StringComparison.OrdinalIgnoreCase))
            return "/";

        return decoded;
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
                    // REMOVED: Centralized logging causing 62s timeout
                    // await ApiService.LogUserActionAsync("AdminLogin", "Authentication", details: $"Admin user {email} logged in successfully");

                    Console.WriteLine($"Login successful for {email}, navigating...");

                    // Navigate without force reload to maintain state
                    isLoading = false;
                    StateHasChanged();

                    // Use a small delay to ensure everything is updated
                    await Task.Delay(100);

                    // Navigate to the originally requested page (or dashboard as fallback)
                    NavigationManager.NavigateTo(ResolveReturnUrl());
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
                // REMOVED: Centralized logging causing 62s timeout
                // await ApiService.LogUserActionAsync("AdminLoginFailed", "Authentication", details: $"Failed login attempt for {email}");
            }
        }
        catch (Exception ex)
        {
            errorMessage = $"Login failed: {ex.Message}";
            Console.WriteLine($"Login error: {ex.Message}");
            // REMOVED: Centralized logging causing 62s timeout
            // await ApiService.LogUserActionAsync("AdminLoginError", "Authentication", details: $"Login error for {email}: {ex.Message}");
        }
        finally
        {
            isLoading = false;
            StateHasChanged(); // Force final UI update
        }
    }

}