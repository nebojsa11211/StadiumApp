using Microsoft.JSInterop;

namespace StadiumDrinkOrdering.Runner.Services;

/// <summary>A queued mutation awaiting sync. Id doubles as the API ClientActionId for idempotency.</summary>
public class OutboxAction
{
    public Guid Id { get; set; }
    public string Type { get; set; } = "deliver";
    public int OrderId { get; set; }
    public DateTime EnqueuedAt { get; set; }
    public int RetryCount { get; set; }
}

/// <summary>
/// Offline outbox for the Runner. Delivery confirmations are enqueued to IndexedDB and synced to the
/// API when connectivity allows — so a runner in a dead zone can still complete a delivery and have
/// it land later. Retries are safe because each action carries a stable ClientActionId and the API's
/// status update is idempotent (Delivered→Delivered is a no-op success).
/// </summary>
public class OutboxService : IAsyncDisposable
{
    private readonly IJSRuntime _js;
    private readonly RunnerApiService _api;
    private readonly RunnerAuthService _auth;

    private IJSObjectReference? _module;
    private DotNetObjectReference<OutboxService>? _self;
    private bool _flushing;

    public bool IsOnline { get; private set; } = true;
    public List<OutboxAction> Pending { get; private set; } = new();

    /// <summary>Fires when pending items or connectivity change, so the UI can update badges/lists.</summary>
    public event Action? OnChange;

    public OutboxService(IJSRuntime js, RunnerApiService api, RunnerAuthService auth)
    {
        _js = js;
        _api = api;
        _auth = auth;
    }

    public async Task InitializeAsync()
    {
        try
        {
            _module ??= await _js.InvokeAsync<IJSObjectReference>("import", "./js/outbox.js");
            _self ??= DotNetObjectReference.Create(this);
            IsOnline = await _module.InvokeAsync<bool>("registerConnectivity", _self);
            await RefreshPendingAsync();
            if (IsOnline) await FlushAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Outbox init failed: {ex.Message}");
        }
    }

    /// <summary>Queue a delivery confirmation and try to sync it immediately (no-op if offline).</summary>
    public async Task EnqueueDeliverAsync(int orderId)
    {
        var action = new OutboxAction
        {
            Id = Guid.NewGuid(),
            Type = "deliver",
            OrderId = orderId,
            EnqueuedAt = DateTime.UtcNow
        };

        try
        {
            if (_module != null) await _module.InvokeVoidAsync("add", action);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Outbox enqueue failed: {ex.Message}");
        }

        await RefreshPendingAsync();
        _ = FlushAsync(); // fire-and-forget; if offline the send fails and the item stays queued
    }

    public async Task FlushAsync()
    {
        if (_flushing || _module == null || !_auth.IsAuthenticated) return;
        _flushing = true;
        try
        {
            var actions = await _module.InvokeAsync<OutboxAction[]>("list");
            foreach (var a in actions.OrderBy(a => a.EnqueuedAt))
            {
                var sent = a.Type switch
                {
                    "deliver" => await _api.MarkDeliveredAsync(a.OrderId, a.Id),
                    _ => true // unknown action type — drop it rather than retry forever
                };
                if (sent)
                {
                    await _module.InvokeVoidAsync("remove", a.Id.ToString());
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Outbox flush failed: {ex.Message}");
        }
        finally
        {
            _flushing = false;
            await RefreshPendingAsync();
        }
    }

    public bool IsPendingDeliver(int orderId) =>
        Pending.Any(p => p.Type == "deliver" && p.OrderId == orderId);

    private async Task RefreshPendingAsync()
    {
        try
        {
            if (_module != null)
                Pending = (await _module.InvokeAsync<OutboxAction[]>("list")).ToList();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Outbox refresh failed: {ex.Message}");
        }
        OnChange?.Invoke();
    }

    [JSInvokable]
    public async Task OnConnectivityChanged(bool online)
    {
        IsOnline = online;
        OnChange?.Invoke();
        if (online) await FlushAsync();
    }

    [JSInvokable]
    public async Task OnResumed()
    {
        if (IsOnline) await FlushAsync();
    }

    public async ValueTask DisposeAsync()
    {
        _self?.Dispose();
        if (_module != null)
        {
            try { await _module.DisposeAsync(); } catch { /* ignore */ }
        }
    }
}
