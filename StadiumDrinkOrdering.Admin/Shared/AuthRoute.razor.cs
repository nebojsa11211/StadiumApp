using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Shared.Authentication.Interfaces;
using StadiumDrinkOrdering.Shared.Authentication.Models;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class AuthRoute : ComponentBase, IDisposable
{
    [Inject] private IAuthenticationStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    private bool isChecking = true;

    protected override async Task OnInitializedAsync()
    {
        // App.razor.cs already kicked off InitializeAsync(). Listen for later state
        // changes (logout / token expiry) BEFORE deciding, so we don't miss any.
        AuthStateService.OnAuthenticationStateChanged += HandleAuthStateChanged;

        // Decide based on the actual token, not just the in-memory flag. If the flag
        // is already true we trust it; otherwise re-validate against the token store.
        // This recovers from a transient init read (slow JSInterop/session) that would
        // otherwise wrongly redirect an authenticated user to /login.
        var isAuthenticated = AuthStateService.IsAuthenticated
            || await AuthStateService.ValidateAuthenticationAsync();

        isChecking = false;

        if (!isAuthenticated)
        {
            RedirectToLogin();
        }
    }

    private void HandleAuthStateChanged(AuthenticationState state)
    {
        // If authentication state changes and user is no longer authenticated, redirect to login
        if (!state.IsAuthenticated)
        {
            RedirectToLogin();
        }
        else
        {
            InvokeAsync(StateHasChanged);
        }
    }

    private void RedirectToLogin()
    {
        // Pass a relative return path (e.g. /orders) so login can send the user back to the
        // page they actually requested instead of the dashboard.
        var relativePath = "/" + NavigationManager.ToBaseRelativePath(NavigationManager.Uri);
        var returnUrl = Uri.EscapeDataString(relativePath);
        NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
    }

    public void Dispose()
    {
        AuthStateService.OnAuthenticationStateChanged -= HandleAuthStateChanged;
    }
}
