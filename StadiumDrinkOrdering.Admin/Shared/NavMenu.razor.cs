using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class NavMenu : ComponentBase, IDisposable
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;

    private bool collapseNavMenu = true;

    private string? NavMenuCssClass => collapseNavMenu ? "collapse" : null;

    protected override void OnInitialized()
    {
        AuthStateService.OnAuthenticationStateChanged += StateHasChanged;
    }

    private void ToggleNavMenu()
    {
        collapseNavMenu = !collapseNavMenu;
    }

    public void Dispose()
    {
        AuthStateService.OnAuthenticationStateChanged -= StateHasChanged;
    }
}