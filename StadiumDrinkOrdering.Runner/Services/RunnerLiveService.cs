using System.Text.Json;
using Microsoft.AspNetCore.SignalR.Client;

namespace StadiumDrinkOrdering.Runner.Services;

/// <summary>
/// Live push channel for the Runner: one SignalR connection to the API's BartenderHub. The hub puts
/// every authenticated staff connection in the "staff-all" group, which receives every order event
/// (new order, marked ready, claimed, delivered, failed), so the pool and My deliveries can refresh
/// the moment the board changes instead of waiting for their poll.
///
/// Pages only need <see cref="Changed"/>, a "something about orders changed — reload" signal. It is
/// debounced, so a burst of events (the bar marking several orders ready at once) causes one reload.
/// Polling stays in the pages as the backstop: a phone that is offline or reconnecting gets no pushes.
/// </summary>
public sealed class RunnerLiveService : IAsyncDisposable
{
    private static readonly TimeSpan Debounce = TimeSpan.FromMilliseconds(300);

    private readonly RunnerAuthService _auth;
    private readonly string _hubUrl;
    private readonly SemaphoreSlim _gate = new(1, 1);
    private HubConnection? _hub;
    private string? _connectedToken;
    private CancellationTokenSource? _debounceCts;

    public RunnerLiveService(RunnerAuthService auth, string apiBaseUrl)
    {
        _auth = auth;
        _hubUrl = apiBaseUrl.TrimEnd('/') + "/bartenderHub";
        // Log out → drop the connection; log in as someone else → reconnect with the new token.
        _auth.OnChange += () => _ = EnsureStartedAsync();
    }

    /// <summary>Raised (debounced) whenever any order event arrives.</summary>
    public event Action? Changed;

    public bool IsConnected => _hub?.State == HubConnectionState.Connected;

    /// <summary>Idempotent: connects when logged in, disconnects when not. Safe to call from every page.</summary>
    public async Task EnsureStartedAsync()
    {
        await _gate.WaitAsync();
        try
        {
            var token = _auth.IsAuthenticated ? _auth.Token : null;

            if (_hub != null && (token == null || token != _connectedToken))
            {
                await StopCoreAsync();
            }

            if (token == null || _hub != null)
                return;

            var hub = new HubConnectionBuilder()
                .WithUrl(_hubUrl, options => options.AccessTokenProvider = () => Task.FromResult(_auth.Token))
                .WithAutomaticReconnect(new[]
                {
                    TimeSpan.Zero, TimeSpan.FromSeconds(2), TimeSpan.FromSeconds(5),
                    TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(30)
                })
                .Build();

            // Payloads are irrelevant here — every event just means "reload from the API".
            hub.On<JsonElement>("NewOrder", _ => Signal());
            hub.On<JsonElement>("OrderUpdated", _ => Signal());
            hub.On<JsonElement>("OrderStatusChanged", _ => Signal());
            hub.On<int, int>("OrderAssignedToStaff", (_, _) => Signal());
            // Events missed while the connection was down are caught up by reloading on reconnect.
            hub.Reconnected += _ => { Signal(); return Task.CompletedTask; };

            _hub = hub;
            _connectedToken = token;
            try
            {
                await hub.StartAsync();
            }
            catch (Exception ex)
            {
                // Offline or API down: the pages keep polling. Drop this instance so the next
                // EnsureStartedAsync (next page visit / auth change) tries again from scratch.
                Console.WriteLine($"Runner live connection failed: {ex.Message}");
                await StopCoreAsync();
            }
        }
        finally
        {
            _gate.Release();
        }
    }

    private void Signal()
    {
        _debounceCts?.Cancel();
        var cts = _debounceCts = new CancellationTokenSource();
        _ = Task.Delay(Debounce, cts.Token).ContinueWith(t =>
        {
            if (!t.IsCanceled)
                Changed?.Invoke();
        }, TaskScheduler.Default);
    }

    private async Task StopCoreAsync()
    {
        var hub = _hub;
        _hub = null;
        _connectedToken = null;
        if (hub == null) return;
        try { await hub.DisposeAsync(); } catch { /* already gone */ }
    }

    public async ValueTask DisposeAsync()
    {
        _debounceCts?.Cancel();
        await StopCoreAsync();
    }
}
