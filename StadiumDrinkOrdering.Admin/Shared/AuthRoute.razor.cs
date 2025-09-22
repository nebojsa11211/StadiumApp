using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class AuthRoute : ComponentBase, IDisposable
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;

    [Parameter] public RenderFragment ChildContent { get; set; } = default!;

    private bool isChecking = true;

    protected override async Task OnInitializedAsync()
    {
        // DO NOT call InitializeAsync here - it's already called in App.razor.cs
        // This prevents multiple initialization calls that can cause loops

        // Wait a moment to ensure App.razor has initialized authentication
        await Task.Delay(50);

        // Now it's safe to check authentication state
        isChecking = false;

        // Listen for authentication state changes BEFORE checking authentication
        // This ensures we don't miss any state updates
        AuthStateService.OnAuthenticationStateChanged += HandleAuthStateChanged;

        // If not authenticated, redirect to login
        if (!AuthStateService.IsAuthenticated)
        {
            RedirectToLogin();
        }
    }

    private void HandleAuthStateChanged()
    {
        // If authentication state changes and user is no longer authenticated, redirect to login
        if (!AuthStateService.IsAuthenticated)
        {
            RedirectToLogin();
        }
        else
        {
            StateHasChanged();
        }
    }

    private void RedirectToLogin()
    {
        var returnUrl = Uri.EscapeDataString(NavigationManager.Uri);
        NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
    }

    public void Dispose()
    {
        AuthStateService.OnAuthenticationStateChanged -= HandleAuthStateChanged;
    }
}