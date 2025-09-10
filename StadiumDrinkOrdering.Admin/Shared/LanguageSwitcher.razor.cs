using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Globalization;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class LanguageSwitcher : ComponentBase
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private string CurrentCulture = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

    private async Task OnLanguageChange(ChangeEventArgs e)
    {
        var culture = e.Value?.ToString() ?? "hr";
        
        // Set culture in cookie
        await JSRuntime.InvokeVoidAsync("blazorCulture.set", culture);
        
        // Force reload to apply new culture
        NavigationManager.NavigateTo(NavigationManager.Uri, forceLoad: true);
    }
}