using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Services;

/// <summary>
/// Loads and caches the first-run <see cref="SetupStatusDto"/> for the admin shell, so the dashboard
/// layout can show a "setup incomplete" banner and the <c>/admin/setup</c> page can render the
/// checklist off one source. Scoped, like <see cref="LiveEventService"/>, so a circuit shares a
/// single load.
/// </summary>
public class SetupStatusService
{
    private readonly IAdminApiService _api;
    private readonly ILogger<SetupStatusService> _logger;
    private Task? _loadTask;

    public SetupStatusService(IAdminApiService api, ILogger<SetupStatusService> logger)
    {
        _api = api;
        _logger = logger;
    }

    /// <summary>Raised whenever the status changes, so subscribers can re-render.</summary>
    public event Action? OnChanged;

    /// <summary>Last loaded status, or null before the first successful load / when unauthenticated.</summary>
    public SetupStatusDto? Status { get; private set; }

    /// <summary>Show the setup banner whenever setup is incomplete. The banner is not dismissible.</summary>
    public bool ShouldShowBanner => Status is { IsComplete: false };

    public Task EnsureLoadedAsync() => _loadTask ??= LoadAsync();

    /// <summary>Force a fresh read, e.g. after the admin finished a setup step on another page.</summary>
    public Task ReloadAsync() => _loadTask = LoadAsync();

    private async Task LoadAsync()
    {
        try
        {
            Status = await _api.GetAsync<SetupStatusDto>("api/setup/status");
        }
        catch (Exception ex)
        {
            // Never take the shell down over a setup check — just don't show the banner.
            _logger.LogWarning(ex, "Failed to load setup status");
            Status = null;
        }

        OnChanged?.Invoke();
    }
}
