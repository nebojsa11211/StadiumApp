using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using System.Globalization;

namespace StadiumDrinkOrdering.Admin.Shared;

public partial class DashboardLayout : LayoutComponentBase, IDisposable
{
    [Inject] private IAuthStateService AuthStateService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private SeasonStateService SeasonState { get; set; } = default!;
    [Inject] private LiveEventService LiveEvents { get; set; } = default!;

    private DateTime _now = DateTime.Now;
    private System.Timers.Timer? _clock;
    private string _userEmail = "admin@stadium.com";
    private string _lang = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

    protected override void OnInitialized()
    {
        if (!string.IsNullOrWhiteSpace(AuthStateService.UserEmail))
            _userEmail = AuthStateService.UserEmail!;

        _clock = new System.Timers.Timer(1000);
        _clock.Elapsed += (_, _) => InvokeAsync(() =>
        {
            _now = DateTime.Now;
            StateHasChanged();
        });
        _clock.Start();

        Navigation.LocationChanged += OnLocationChanged;
        SeasonState.OnChanged += OnSeasonStateChanged;
        LiveEvents.OnChanged += OnSeasonStateChanged;
    }

    protected override async Task OnInitializedAsync()
    {
        await SeasonState.EnsureLoadedAsync();
        await LiveEvents.EnsureLoadedAsync();
    }

    /// <summary>
    /// True on the dashboard, which renders its own interactive event bar. The shell's read-only
    /// bar stands down there so #admin-dashboard-event-bar stays unique on the page.
    /// </summary>
    private bool IsDashboardRoute =>
        Navigation.ToBaseRelativePath(Navigation.Uri).Split('?', '#')[0].Trim('/').Length == 0;

    private void OnSeasonStateChanged() => InvokeAsync(StateHasChanged);

    /// <summary>Banner selection changed: publish it so every page sees the same season.</summary>
    private void OnSeasonChanged(string value) => SeasonState.SetSelected(value);

    // Highlight the nav item matching the current route.
    private string NavClass(string href, string? extra = null)
    {
        var baseClass = "dash-nav-item";
        if (!string.IsNullOrEmpty(extra))
            baseClass += " " + extra;

        var rel = Navigation.ToBaseRelativePath(Navigation.Uri);
        var path = rel.Split('?', '#')[0].Trim('/');

        bool active = href.Length == 0
            ? path.Length == 0
            : path.Equals(href, StringComparison.OrdinalIgnoreCase)
              || path.StartsWith(href + "/", StringComparison.OrdinalIgnoreCase);

        return active ? baseClass + " active" : baseClass;
    }

    private void OnLocationChanged(object? sender, Microsoft.AspNetCore.Components.Routing.LocationChangedEventArgs e)
        => InvokeAsync(StateHasChanged);

    private string LangClass(string lang) => _lang == lang ? "dash-lang-btn active" : "dash-lang-btn";

    private async Task SetLang(string lang)
    {
        if (_lang == lang)
            return;

        _lang = lang;

        // Persist culture in cookie, then force reload so the new culture is applied.
        await JSRuntime.InvokeVoidAsync("blazorCulture.set", lang);
        Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
    }

    private string GetInitials()
    {
        var name = _userEmail.Split('@').FirstOrDefault() ?? "A";
        return string.IsNullOrEmpty(name) ? "A" : name[..1].ToUpperInvariant();
    }

    private async Task HandleLogout()
    {
        await AuthStateService.LogoutAsync();
        Navigation.NavigateTo("/login");
    }

    public void Dispose()
    {
        _clock?.Stop();
        _clock?.Dispose();
        Navigation.LocationChanged -= OnLocationChanged;
        SeasonState.OnChanged -= OnSeasonStateChanged;
        LiveEvents.OnChanged -= OnSeasonStateChanged;
    }
}
