namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Periodically advances events across the clock-driven ends of their lifecycle:
/// <list type="bullet">
/// <item>Closes out events whose window has elapsed to <see cref="Shared.Models.EventStatus.Completed"/> —
/// both stale-live events (Active/InProgress) and ones that never went live (Planned/OnSale/SoldOut) but
/// whose date has passed (<see cref="IEventService.AutoCompleteEndedEventsAsync"/>).</item>
/// <item>Brings a sellable event (OnSale/SoldOut) live to <see cref="Shared.Models.EventStatus.Active"/>
/// once its window opens, so drink ordering unlocks at kickoff without a manual "make live" click
/// (<see cref="IEventService.AutoActivateStartedEventsAsync"/>).</item>
/// </list>
/// These are the scheduled counterparts to the same logic that otherwise only runs opportunistically on
/// read — so a finished event doesn't linger as "live", and a started event doesn't sit at OnSale, until
/// something queries it.
///
/// Both halves are enabled by default; gate completion off with <c>EventLifecycle:AutoCompleteEnabled=false</c>
/// and activation off with <c>EventLifecycle:AutoActivateEnabled=false</c>. Each pass runs in its own DI
/// scope so it never holds a DbContext/connection open between runs. Completion runs before activation so a
/// stale-live event is closed first, freeing the single-live slot for a newly started one.
/// </summary>
public class EventStatusTransitionService : BackgroundService
{
    private readonly ILogger<EventStatusTransitionService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public EventStatusTransitionService(
        ILogger<EventStatusTransitionService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var autoCompleteEnabled = _configuration.GetValue("EventLifecycle:AutoCompleteEnabled", true);
        var autoActivateEnabled = _configuration.GetValue("EventLifecycle:AutoActivateEnabled", true);

        if (!autoCompleteEnabled && !autoActivateEnabled)
        {
            _logger.LogInformation("Event Status Transition Service is disabled " +
                "(EventLifecycle:AutoCompleteEnabled=false and EventLifecycle:AutoActivateEnabled=false).");
            return;
        }

        var intervalMinutes = _configuration.GetValue("EventLifecycle:AutoCompleteIntervalMinutes", 5);
        var interval = TimeSpan.FromMinutes(intervalMinutes <= 0 ? 5 : intervalMinutes);

        _logger.LogInformation("Event Status Transition Service started (interval: {Interval}m, autoComplete: {Complete}, autoActivate: {Activate})",
            interval.TotalMinutes, autoCompleteEnabled, autoActivateEnabled);

        // Let the app finish starting (DB warm-up, Kestrel bind) before the first pass.
        try { await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); }
        catch (TaskCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RunTransitionsAsync(autoCompleteEnabled, autoActivateEnabled);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during scheduled event status transitions");
            }

            try { await Task.Delay(interval, stoppingToken); }
            catch (TaskCanceledException) { break; }
        }
    }

    private async Task RunTransitionsAsync(bool autoCompleteEnabled, bool autoActivateEnabled)
    {
        using var scope = _serviceProvider.CreateScope();
        var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();

        // Close ended events first so a stale-live event frees the single-live slot before we try to
        // bring a newly started event live.
        if (autoCompleteEnabled)
            await eventService.AutoCompleteEndedEventsAsync();
        if (autoActivateEnabled)
            await eventService.AutoActivateStartedEventsAsync();
    }
}
