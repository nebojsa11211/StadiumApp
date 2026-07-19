using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Tracks the event currently in its game-day phase (<see cref="EventPhase.Active"/>) so the admin
/// shell can show a read-only event bar on every page while a match is running. Scoped, like
/// <see cref="SeasonStateService"/>, so one instance serves a circuit.
/// </summary>
/// <remarks>
/// This is deliberately read-only: the dashboard owns the interactive event selector (and the
/// <c>admin-dashboard-event-bar</c> id on that page), so the shell never renders a second one there.
/// </remarks>
public class LiveEventService
{
    private readonly IAdminApiService _api;
    private readonly SeasonStateService _seasonState;
    private readonly ILogger<LiveEventService> _logger;
    private List<EventDto> _events = new();
    private Task? _loadTask;

    public LiveEventService(IAdminApiService api, SeasonStateService seasonState, ILogger<LiveEventService> logger)
    {
        _api = api;
        _seasonState = seasonState;
        _logger = logger;

        // A different season can have a different live event.
        _seasonState.OnChanged += () => OnChanged?.Invoke();
    }

    /// <summary>Raised when the event list changes, so subscribers can re-render.</summary>
    public event Action? OnChanged;

    /// <summary>
    /// The live event in the selected season, or null when nothing is on. With "all seasons"
    /// selected, any live event qualifies.
    /// </summary>
    public EventDto? LiveEvent
    {
        get
        {
            var seasonId = _seasonState.SelectedSeason?.Id;
            return _events
                .Where(e => e.Phase == EventPhase.Active)
                .Where(e => seasonId == null || e.SeasonId == seasonId)
                .OrderBy(e => e.Date ?? DateTime.MaxValue)
                .FirstOrDefault();
        }
    }

    public Task EnsureLoadedAsync() => _loadTask ??= LoadAsync();

    /// <summary>Re-reads the event list, e.g. after an event's status changed.</summary>
    public Task ReloadAsync() => _loadTask = LoadAsync();

    private async Task LoadAsync()
    {
        try
        {
            var events = await _api.GetEventsAsync();
            _events = events?.ToList() ?? new List<EventDto>();
        }
        catch (Exception ex)
        {
            // No events means no bar — never take the shell down over it.
            _logger.LogWarning(ex, "Failed to load events for the admin live-event bar");
            _events = new List<EventDto>();
        }

        OnChanged?.Invoke();
    }
}
