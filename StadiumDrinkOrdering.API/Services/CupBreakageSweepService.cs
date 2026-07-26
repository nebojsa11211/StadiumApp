namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Periodically forfeits reusable-cup deposits whose refund window has elapsed (their event is over),
/// turning the held liability into breakage revenue. Mirrors the other scheduled maintenance services:
/// each pass runs in its own DI scope so it never holds a DbContext open between runs. Respects the
/// venue's configured refund window (a NoExpiry window forfeits nothing). Disable with
/// <c>Cups:BreakageSweepEnabled=false</c>; interval via <c>Cups:BreakageSweepIntervalMinutes</c>
/// (default 60). See docs/reusable-cups-design.md.
/// </summary>
public class CupBreakageSweepService : BackgroundService
{
    private readonly ILogger<CupBreakageSweepService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public CupBreakageSweepService(
        ILogger<CupBreakageSweepService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        if (!_configuration.GetValue("Cups:BreakageSweepEnabled", true))
        {
            _logger.LogInformation("Cup breakage sweep is disabled (Cups:BreakageSweepEnabled=false).");
            return;
        }

        var intervalMinutes = _configuration.GetValue("Cups:BreakageSweepIntervalMinutes", 60);
        var interval = TimeSpan.FromMinutes(intervalMinutes <= 0 ? 60 : intervalMinutes);
        _logger.LogInformation("Cup breakage sweep started (interval: {Interval}m).", interval.TotalMinutes);

        // Let the app finish starting (DB warm-up, Kestrel bind) before the first pass.
        try { await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); }
        catch (TaskCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var cups = scope.ServiceProvider.GetRequiredService<ICupService>();
                var forfeited = await cups.SweepForfeitedDepositsAsync();
                if (forfeited > 0)
                    _logger.LogInformation("Cup breakage sweep forfeited {Count} deposits.", forfeited);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cup breakage sweep");
            }

            try { await Task.Delay(interval, stoppingToken); }
            catch (TaskCanceledException) { break; }
        }
    }
}
