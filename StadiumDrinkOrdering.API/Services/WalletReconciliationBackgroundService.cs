using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Periodically verifies the wallet invariant <c>Balance == Σ(Completed ledger amounts)</c> for every
/// wallet and reports any drift as a Security/Audit log event. Read-only — it never mutates balances;
/// drift indicates a bug or out-of-band DB edit and is surfaced for a human to investigate. Mirrors
/// the <see cref="LogRetentionBackgroundService"/> shape.
/// </summary>
public class WalletReconciliationBackgroundService : BackgroundService
{
    private readonly ILogger<WalletReconciliationBackgroundService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IConfiguration _configuration;

    public WalletReconciliationBackgroundService(
        ILogger<WalletReconciliationBackgroundService> logger,
        IServiceProvider serviceProvider,
        IConfiguration configuration)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _configuration = configuration;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Off by default: the codebase currently disables background services (they caused startup DB
        // pool exhaustion). Set WalletReconciliation:Enabled=true to turn this read-only check on.
        if (!_configuration.GetValue("WalletReconciliation:Enabled", false))
        {
            _logger.LogInformation("Wallet Reconciliation Background Service is disabled (WalletReconciliation:Enabled=false).");
            return;
        }

        var intervalHours = _configuration.GetValue("WalletReconciliation:IntervalHours", 6);
        var interval = TimeSpan.FromHours(intervalHours <= 0 ? 6 : intervalHours);

        _logger.LogInformation("Wallet Reconciliation Background Service started (interval: {Interval}h)", interval.TotalHours);

        // Let the app finish starting before the first pass.
        try { await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); }
        catch (TaskCanceledException) { return; }

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await ReconcileAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during wallet reconciliation");
            }

            try { await Task.Delay(interval, stoppingToken); }
            catch (TaskCanceledException) { break; }
        }
    }

    private async Task ReconcileAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var loggingService = scope.ServiceProvider.GetRequiredService<ILoggingService>();

        // One query returns only the wallets whose cached balance disagrees with their completed ledger sum.
        var drifts = await context.Wallets.AsNoTracking()
            .Select(w => new
            {
                w.Id,
                w.UserId,
                w.Balance,
                LedgerSum = context.WalletTransactions
                    .Where(t => t.WalletId == w.Id && t.Status == WalletTransactionStatus.Completed)
                    .Sum(t => (decimal?)t.Amount) ?? 0m
            })
            .Where(x => x.Balance != x.LedgerSum)
            .ToListAsync();

        if (drifts.Count == 0)
        {
            _logger.LogInformation("Wallet reconciliation: all wallets consistent.");
            return;
        }

        foreach (var d in drifts)
        {
            var msg = $"Wallet {d.Id} (user {d.UserId}) balance {d.Balance:0.00} != ledger sum {d.LedgerSum:0.00} (drift {d.Balance - d.LedgerSum:0.00}).";
            _logger.LogError("WALLET DRIFT: {Message}", msg);
            await loggingService.LogWarningAsync(
                message: "Wallet balance drift detected",
                action: "WalletReconciliation",
                category: "Security",
                details: msg,
                source: "WalletReconciliationService");
        }

        _logger.LogError("Wallet reconciliation found {Count} drifting wallet(s).", drifts.Count);
    }
}
