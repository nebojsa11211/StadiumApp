namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Periodically closes out events whose time window has elapsed but that are still stuck in a live
/// phase (Active/InProgress), transitioning them to <see cref="Shared.Models.EventStatus.Completed"/>.
/// This is the scheduled counterpart to <see cref="IEventService.AutoCompleteEndedEventsAsync"/>,
/// which otherwise only runs opportunistically on read — so a finished match no longer lingers as
/// "live" (or as the season's "next fixture") until something happens to query the active set.
///
/// Enabled by default; gate off with <c>EventLifecycle:AutoCompleteEnabled=false</c>. The work is
/// cheap (it scans only the handful of currently-live events), and each pass runs in its own DI
/// scope so it never holds a DbContext/connection open between runs.
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
        if (!_configuration.GetValue("EventLifecycle:AutoCompleteEnabled", true))
        {
            _logger.LogInformation("Event Status Transition Service is disabled (EventLifecycle:AutoCompleteEnabled=false).");
            return;
        }

        var intervalMinutes = _configuration.GetValue("EventLifecycle:AutoCompleteIntervalMinutes", 5);
        var interval = TimeSpan.FromMinutes(intervalMinutes <= 0 ? 5 : intervalMinutes);

        _logger.LogInformation("Event Status Transition Service started (interval: {Interval}m)", interval.TotalMinutes);

        // Let the app finish starting (DB warm-up, Kestrel bind) before the first pass.
        try { await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); }
        catch (TaskCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await CompleteEndedEventsAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during scheduled event auto-completion");
            }

            try { await Task.Delay(interval, stoppingToken); }
            catch (TaskCanceledException) { break; }
        }
    }

    private async Task CompleteEndedEventsAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
        await eventService.AutoCompleteEndedEventsAsync();
    }
}
