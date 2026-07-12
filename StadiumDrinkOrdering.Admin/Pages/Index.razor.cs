using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Admin.Common;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.DTOs.Integration;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Index : ComponentBase, IDisposable
{
    [Inject] private IAdminApiService AdminApiService { get; set; } = default!;
    [Inject] private ISignalRService SignalRService { get; set; } = default!;
    [Inject] private ILogger<Index> Logger { get; set; } = default!;

    private bool _loading = true;

    // Event scoping — the whole dashboard is scoped to one selected event.
    private List<EventDto> _events = new();
    private EventDto? _selectedEvent;
    private List<OrderDto> _allOrders = new();
    private List<SeasonDto> _seasons = new();

    // The season the banner (and the season-pass tile) is currently showing. Navigable by the admin;
    // also synced to the selected event's season whenever the event changes.
    private SeasonDto? _selectedSeason;

    // All seasons in chronological order — the order the banner navigates through.
    private List<SeasonDto> SeasonsOrdered =>
        _seasons.OrderBy(s => s.StartDate).ThenBy(s => s.Id).ToList();

    // The season currently in progress (flagged IsCurrent) — the "live" season to jump back to.
    private SeasonDto? LiveSeason => _seasons.FirstOrDefault(s => s.IsCurrent);

    // The season shown in the banner: the navigated selection, else the live season, else the first.
    private SeasonDto? DisplaySeason =>
        _selectedSeason ?? LiveSeason ?? SeasonsOrdered.FirstOrDefault();

    // Season tile follows the banner so the two stay consistent.
    private SeasonDto? DashboardSeason => DisplaySeason;
    private int SeasonPassCount => DashboardSeason?.SeasonTicketCount ?? 0;

    // ----- Season navigation -----
    private int SelectedSeasonIndex =>
        DisplaySeason == null ? -1 : SeasonsOrdered.FindIndex(s => s.Id == DisplaySeason.Id);
    private bool CanGoPrevSeason => SelectedSeasonIndex > 0;
    private bool CanGoNextSeason => SelectedSeasonIndex >= 0 && SelectedSeasonIndex < _seasons.Count - 1;
    private bool IsOnLiveSeason => DisplaySeason != null && LiveSeason?.Id == DisplaySeason.Id;

    /// <summary>Name of the selected event's season, or null when it has none.</summary>
    private string? SelectedEventSeasonName =>
        _selectedEvent?.SeasonId is int sid ? _seasons.FirstOrDefault(s => s.Id == sid)?.Name : null;

    /// <summary>True when the selected event belongs to a season other than the one shown in the banner.</summary>
    private bool SelectedEventInOtherSeason =>
        _selectedEvent?.SeasonId is int sid && DisplaySeason != null && sid != DisplaySeason.Id;

    /// <summary>Events that belong to the season shown in the banner. With no seasons, the whole list.</summary>
    private List<EventDto> SeasonEvents =>
        DisplaySeason == null
            ? _events
            : _events.Where(e => e.SeasonId == DisplaySeason.Id).ToList();

    // Scoped metrics
    private int _ticketsSold;
    private int _seasonTicketsSold;
    /// <summary>Sold seats that are ordinary single-event tickets (total minus season-derived).</summary>
    private int NormalTicketsSold => Math.Max(0, _ticketsSold - _seasonTicketsSold);
    private decimal _eventRevenue;
    private int _activeDrinkOrders;
    private List<OrderDto> _recentOrders = new();

    // Sorting for the recent-orders table.
    private readonly TableSortState sortState = new();
    private static readonly Dictionary<string, Func<OrderDto, object?>> SortSelectors = new()
    {
        ["id"] = o => o.Id,
        ["seat"] = o => o.SeatNumber,
        ["amount"] = o => o.TotalAmount,
        ["status"] = o => o.Status,
    };

    // Displayed orders: keep today's default order (newest-first) until a column is picked.
    private IEnumerable<OrderDto> DisplayedRecentOrders =>
        sortState.Column is null
            ? _recentOrders
            : sortState.Apply(_recentOrders, SortSelectors);

    private void SortBy(string column)
    {
        sortState.Toggle(column);
        StateHasChanged();
    }

    // SignalR event group we've currently joined (0 = none).
    private int _joinedEventId;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
        await InitializeSignalRAsync();
    }

    private async Task LoadAsync()
    {
        _loading = true;
        try
        {
            var events = await AdminApiService.GetEventsAsync();
            _events = (events ?? Enumerable.Empty<EventDto>())
                .OrderBy(e => e.Date ?? DateTime.MaxValue)
                .ThenBy(e => e.Id)
                .ToList();

            // Preserve the user's selection across reloads; otherwise pick the default.
            if (_selectedEvent != null && _events.Any(e => e.Id == _selectedEvent.Id))
                _selectedEvent = _events.First(e => e.Id == _selectedEvent.Id);
            else
                _selectedEvent = PickDefaultEvent(_events);

            var orders = await AdminApiService.GetOrdersAsync();
            _allOrders = orders?.ToList() ?? new List<OrderDto>();

            try { _seasons = await AdminApiService.GetAsync<List<SeasonDto>>("seasons") ?? new List<SeasonDto>(); }
            catch (Exception ex) { Logger.LogWarning(ex, "Failed to load seasons for dashboard tile"); }

            // Keep an in-range selection across reloads; default to the event's season, else the live season.
            if (_selectedSeason != null)
                _selectedSeason = _seasons.FirstOrDefault(s => s.Id == _selectedSeason.Id);
            SyncSeasonToEvent();

            // The season scopes the dashboard: make sure the selected event lives in the displayed season.
            EnsureEventInSeason();

            ApplyEventScope();
        }
        finally
        {
            _loading = false;
            StateHasChanged();
        }
    }

    // ---------------------------------------------------------------------
    // Live updates (SignalR / BartenderHub)
    // ---------------------------------------------------------------------

    private async Task InitializeSignalRAsync()
    {
        SignalRService.TicketSold += OnTicketSold;
        SignalRService.NewOrder += OnOrderUpserted;
        SignalRService.OrderUpdated += OnOrderUpserted;
        SignalRService.OrderStatusChanged += OnOrderStatusChanged;

        try
        {
            await SignalRService.StartAsync();
            await RejoinEventAsync();
        }
        catch (InvalidOperationException ex) when (ex.Message.Contains("No authentication token"))
        {
            // Token may not be ready yet immediately after login — retry shortly.
            _ = Task.Run(async () =>
            {
                await Task.Delay(1500);
                try
                {
                    await SignalRService.StartAsync();
                    await RejoinEventAsync();
                    await InvokeAsync(StateHasChanged);
                }
                catch (Exception retryEx)
                {
                    Logger.LogWarning(retryEx, "SignalR retry failed on admin dashboard");
                }
            });
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to start SignalR on admin dashboard");
        }
    }

    /// <summary>Authoritative tickets-sold update from the ticketing pipeline.</summary>
    private void OnTicketSold(TicketSoldNotification n)
    {
        if (_selectedEvent == null || n.EventId != _selectedEvent.Id)
            return;

        _ = InvokeAsync(() =>
        {
            _ticketsSold = n.TotalSold;
            // Keep the selected event coherent so a later re-scope recomputes the same value.
            _selectedEvent.AvailableSeats = Math.Max(0, _selectedEvent.Capacity - n.TotalSold);
            StateHasChanged();
        });
    }

    /// <summary>A new or changed drink order — upsert into the master list and re-scope.</summary>
    private void OnOrderUpserted(OrderDto order)
    {
        _ = InvokeAsync(() =>
        {
            var idx = _allOrders.FindIndex(o => o.Id == order.Id);
            if (idx >= 0)
                _allOrders[idx] = order;
            else
                _allOrders.Add(order);

            ApplyEventScope();
            StateHasChanged();
        });
    }

    private void OnOrderStatusChanged(OrderStatusChangedNotification n)
    {
        _ = InvokeAsync(() =>
        {
            var existing = _allOrders.FirstOrDefault(o => o.Id == n.OrderId);
            if (existing == null)
                return;

            existing.Status = n.Status;
            ApplyEventScope();
            StateHasChanged();
        });
    }

    /// <summary>Leave the previously joined event group and join the selected one (best effort).</summary>
    private async Task RejoinEventAsync()
    {
        if (!SignalRService.IsConnected)
            return;

        if (_joinedEventId > 0 && _joinedEventId != (_selectedEvent?.Id ?? 0))
        {
            try { await SignalRService.LeaveEvent(_joinedEventId); }
            catch { /* best effort */ }
            _joinedEventId = 0;
        }

        if (_selectedEvent != null && _joinedEventId != _selectedEvent.Id)
        {
            try
            {
                await SignalRService.JoinEvent(_selectedEvent.Id);
                _joinedEventId = _selectedEvent.Id;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to join event group {EventId}", _selectedEvent.Id);
            }
        }
    }

    /// <summary>
    /// Default event on first load: a live event, else the next upcoming, else the most recent past.
    /// "Live" is driven by the lifecycle phase (Active/InProgress), not timestamps.
    /// </summary>
    private static EventDto? PickDefaultEvent(List<EventDto> ordered)
    {
        if (ordered.Count == 0)
            return null;

        var live = ordered.FirstOrDefault(e => e.Phase == EventPhase.Active);
        if (live != null)
            return live;

        var now = DateTime.Now;
        var next = ordered.FirstOrDefault(e => (e.Date ?? DateTime.MaxValue) >= now);
        if (next != null)
            return next;

        return ordered[^1]; // most recent past (list is ordered ascending by date)
    }

    /// <summary>Recomputes all scoped metrics for the selected event from the loaded order set.</summary>
    private void ApplyEventScope()
    {
        var scoped = _selectedEvent == null
            ? _allOrders
            : _allOrders.Where(o => o.EventId == _selectedEvent.Id).ToList();

        _activeDrinkOrders = scoped.Count(o =>
            o.Status == OrderStatus.Pending ||
            o.Status == OrderStatus.Accepted ||
            o.Status == OrderStatus.InPreparation ||
            o.Status == OrderStatus.OutForDelivery);

        _eventRevenue = scoped.Sum(o => o.TotalAmount);
        _recentOrders = scoped.OrderByDescending(o => o.CreatedAt).Take(5).ToList();
        _ticketsSold = _selectedEvent == null
            ? 0
            : Math.Max(0, _selectedEvent.Capacity - _selectedEvent.AvailableSeats);
        _seasonTicketsSold = Math.Min(_selectedEvent?.SeasonTicketsSold ?? 0, _ticketsSold);
    }

    // ----- Event navigation (re-filters already-loaded orders, no network) -----
    private bool HasEvents => SeasonEvents.Count > 0;
    private int SelectedIndex => _selectedEvent == null ? -1 : SeasonEvents.FindIndex(e => e.Id == _selectedEvent.Id);
    private bool CanGoPrev => SelectedIndex > 0;
    private bool CanGoNext => SelectedIndex >= 0 && SelectedIndex < SeasonEvents.Count - 1;
    private bool IsOnDefaultEvent => _selectedEvent != null && PickDefaultEvent(SeasonEvents)?.Id == _selectedEvent.Id;

    /// <summary>Re-scope the dashboard to the selected event and move the live subscription with it.</summary>
    private async Task OnSelectedEventChangedAsync()
    {
        SyncSeasonToEvent();
        ApplyEventScope();
        StateHasChanged();
        await RejoinEventAsync();
    }

    /// <summary>Point the season banner at the selected event's season (when it has one).</summary>
    private void SyncSeasonToEvent()
    {
        if (_selectedEvent?.SeasonId is int sid)
        {
            var season = _seasons.FirstOrDefault(s => s.Id == sid);
            if (season != null)
                _selectedSeason = season;
        }
    }

    // ----- Season navigation (banner) -----
    private async Task SelectPrevSeason()
    {
        if (CanGoPrevSeason)
        {
            _selectedSeason = SeasonsOrdered[SelectedSeasonIndex - 1];
            await OnSeasonScopeChangedAsync();
        }
    }

    private async Task SelectNextSeason()
    {
        if (CanGoNextSeason)
        {
            _selectedSeason = SeasonsOrdered[SelectedSeasonIndex + 1];
            await OnSeasonScopeChangedAsync();
        }
    }

    private async Task OnSelectSeason(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var id))
        {
            var match = _seasons.FirstOrDefault(s => s.Id == id);
            if (match != null)
            {
                _selectedSeason = match;
                await OnSeasonScopeChangedAsync();
            }
        }
    }

    private async Task GoToCurrentSeason()
    {
        if (LiveSeason != null)
        {
            _selectedSeason = LiveSeason;
            await OnSeasonScopeChangedAsync();
        }
    }

    /// <summary>A new season was chosen in the banner: re-scope the whole dashboard to it.</summary>
    private async Task OnSeasonScopeChangedAsync()
    {
        // Selecting a season resets the event scope to a sensible default within that season.
        _selectedEvent = PickDefaultEvent(SeasonEvents);
        ApplyEventScope();
        StateHasChanged();
        await RejoinEventAsync();
    }

    /// <summary>Guarantee the selected event belongs to the displayed season; otherwise pick a default there.</summary>
    private void EnsureEventInSeason()
    {
        if (DisplaySeason == null)
            return; // no seasons configured → no season filtering
        if (_selectedEvent != null && _selectedEvent.SeasonId == DisplaySeason.Id)
            return;
        _selectedEvent = PickDefaultEvent(SeasonEvents);
    }

    private async Task SelectPrevEvent()
    {
        if (CanGoPrev)
        {
            _selectedEvent = SeasonEvents[SelectedIndex - 1];
            await OnSelectedEventChangedAsync();
        }
    }

    private async Task SelectNextEvent()
    {
        if (CanGoNext)
        {
            _selectedEvent = SeasonEvents[SelectedIndex + 1];
            await OnSelectedEventChangedAsync();
        }
    }

    private async Task OnSelectEvent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var id))
        {
            var match = _events.FirstOrDefault(x => x.Id == id);
            if (match != null)
            {
                _selectedEvent = match;
                await OnSelectedEventChangedAsync();
            }
        }
    }

    private async Task GoToCurrentEvent()
    {
        var def = PickDefaultEvent(SeasonEvents);
        if (def != null)
        {
            _selectedEvent = def;
            await OnSelectedEventChangedAsync();
        }
    }

    private static string GetPhaseModifier(EventPhase phase) => phase switch
    {
        EventPhase.Active => "live",
        EventPhase.Future => "future",
        _ => "past"
    };

    private static string StatusLabel(OrderStatus status) => status switch
    {
        OrderStatus.Pending => "Na čekanju",
        OrderStatus.Accepted => "Prihvaćeno",
        OrderStatus.InPreparation => "U pripremi",
        OrderStatus.Ready => "Spremno",
        OrderStatus.OutForDelivery => "U dostavi",
        OrderStatus.Delivered => "Dostavljeno",
        OrderStatus.Cancelled => "Otkazano",
        _ => status.ToString()
    };

    private static string StatusClass(OrderStatus status) => status switch
    {
        OrderStatus.Pending => "is-pending",
        OrderStatus.Accepted => "is-accepted",
        OrderStatus.InPreparation => "is-prep",
        OrderStatus.Ready => "is-ready",
        OrderStatus.OutForDelivery => "is-accepted",
        OrderStatus.Delivered => "is-done",
        OrderStatus.Cancelled => "is-cancelled",
        _ => "is-prep"
    };

    public void Dispose()
    {
        try
        {
            SignalRService.TicketSold -= OnTicketSold;
            SignalRService.NewOrder -= OnOrderUpserted;
            SignalRService.OrderUpdated -= OnOrderUpserted;
            SignalRService.OrderStatusChanged -= OnOrderStatusChanged;

            if (_joinedEventId > 0)
                _ = SignalRService.LeaveEvent(_joinedEventId);
        }
        catch (ObjectDisposedException) { /* already gone */ }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error disposing admin dashboard");
        }
    }
}
