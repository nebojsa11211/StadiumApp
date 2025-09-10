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
        // Simulate a brief authentication check delay
        await Task.Delay(100);
        isChecking = false;
        StateHasChanged();

        // Listen for authentication state changes
        AuthStateService.OnAuthenticationStateChanged += StateHasChanged;
    }

    private void RedirectToLogin()
    {
        var returnUrl = Uri.EscapeDataString(NavigationManager.Uri);
        NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
    }

    public void Dispose()
    {
        AuthStateService.OnAuthenticationStateChanged -= StateHasChanged;
    }
}