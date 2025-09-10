using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin;

public partial class App : ComponentBase
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;

    private bool isInitialized = false;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await AuthStateService.InitializeAsync();
            isInitialized = true;
            StateHasChanged();
        }
    }

    private bool IsLoginPage(Type? pageType)
    {
        return pageType?.Name == "Login";
    }
}