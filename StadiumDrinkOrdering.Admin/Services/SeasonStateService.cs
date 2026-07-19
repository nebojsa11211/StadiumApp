using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// The admin shell's season scope, shared by the banner in <c>DashboardLayout</c> and by every
/// page that cares which season is selected. Registered scoped, so in Blazor Server there is one
/// instance per circuit and the selection survives navigation between admin pages.
/// </summary>
/// <remarks>
/// The selection uses the same convention as <c>SeasonFilterBanner</c>:
/// <c>""</c> = all seasons, <c>"none"</c> = no season, otherwise the season id.
/// </remarks>
public class SeasonStateService
{
    private readonly IAdminApiService _api;
    private readonly ILogger<SeasonStateService> _logger;
    private Task? _loadTask;

    public SeasonStateService(IAdminApiService api, ILogger<SeasonStateService> logger)
    {
        _api = api;
        _logger = logger;
    }

    /// <summary>Every season, as loaded from the API. Empty until <see cref="EnsureLoadedAsync"/> completes.</summary>
    public List<SeasonDto> Seasons { get; private set; } = new();

    /// <summary>"" = all seasons, "none" = no season, else the selected season id.</summary>
    public string SelectedValue { get; private set; } = "";

    /// <summary>The season currently in progress, or null when none is flagged.</summary>
    public SeasonDto? LiveSeason => Seasons.FirstOrDefault(s => s.IsCurrent);

    /// <summary>The concrete selected season, or null for the "all seasons"/"no season" states.</summary>
    public SeasonDto? SelectedSeason =>
        int.TryParse(SelectedValue, out var id) ? Seasons.FirstOrDefault(s => s.Id == id) : null;

    /// <summary>Raised after the selection or the season list changes, so subscribers can re-render.</summary>
    public event Action? OnChanged;

    /// <summary>
    /// Loads the season list once per circuit. Safe to await from several components — later callers
    /// join the first load rather than issuing another request.
    /// </summary>
    public Task EnsureLoadedAsync() => _loadTask ??= LoadAsync();

    /// <summary>Re-reads the season list, e.g. after a season was created or edited.</summary>
    public Task ReloadAsync() => _loadTask = LoadAsync();

    private async Task LoadAsync()
    {
        try
        {
            Seasons = await _api.GetAsync<List<SeasonDto>>("seasons") ?? new List<SeasonDto>();
        }
        catch (Exception ex)
        {
            // A missing season list must not take the whole admin shell down: the banner simply
            // renders nothing and pages fall back to their unscoped behaviour.
            _logger.LogWarning(ex, "Failed to load seasons for the admin season banner");
            Seasons = new List<SeasonDto>();
        }

        // Default the shell to the live season so every page opens on the season in progress.
        if (string.IsNullOrEmpty(SelectedValue) && LiveSeason != null)
            SelectedValue = LiveSeason.Id.ToString();
        else if (SelectedSeason == null && SelectedValue is not ("" or "none"))
            SelectedValue = LiveSeason?.Id.ToString() ?? ""; // selected season disappeared

        OnChanged?.Invoke();
    }

    public void SetSelected(string value)
    {
        if (SelectedValue == value)
            return;

        SelectedValue = value ?? "";
        OnChanged?.Invoke();
    }
}
