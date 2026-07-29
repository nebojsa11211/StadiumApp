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
    [Inject] private SeasonStateService SeasonState { get; set; } = default!;

    /// <summary>
    /// Deep link to a specific event (<c>/?eventId=123</c>), used by the shell's live-event bar so
    /// clicking a running match lands on the dashboard already scoped to it. Wins over the shell's
    /// season selection on load — see <see cref="TryApplyEventFromQuery"/>.
    /// </summary>
    [Parameter, SupplyParameterFromQuery(Name = "eventId")]
    public int? EventIdParam { get; set; }

    private bool _loading = true;

    // Event scoping — the whole dashboard is scoped to one selected event.
    private List<EventDto> _events = new();
    private EventDto? _selectedEvent;
    private List<OrderDto> _allOrders = new();

    // Seasons and the selected season come from the shell banner in DashboardLayout, so the
    // dashboard stays scoped to whatever season the admin picked on any other page.
    private List<SeasonDto> _seasons => SeasonState.Seasons;

    // All seasons in chronological order — the order the banner navigates through.
    private List<SeasonDto> SeasonsOrdered =>
        _seasons.OrderBy(s => s.StartDate).ThenBy(s => s.Id).ToList();

    // The season currently in progress (flagged IsCurrent) — the "live" season to jump back to.
    private SeasonDto? LiveSeason => _seasons.FirstOrDefault(s => s.IsCurrent);

    /// <summary>
    /// The shell banner's raw scope, using its convention: "" = all seasons, "none" = events with no
    /// season, otherwise a season id.
    /// </summary>
    private string SeasonScope => SeasonState.SelectedValue ?? "";

    // The concrete season the dashboard is scoped to, or null in the "all seasons"/"no season"
    // states. This used to fall back to the live season for those two, which silently re-applied a
    // season filter and made events belonging to no season (one-off matches) unreachable here.
    private SeasonDto? DisplaySeason => SeasonState.SelectedSeason;

    // Season tile follows the banner so the two stay consistent.
    private SeasonDto? DashboardSeason => DisplaySeason;

    /// <summary>Label for the season tile, covering the banner's two season-less scopes.</summary>
    private string SeasonScopeLabel => SeasonScope switch
    {
        "none" => L["Index_NoSeason"],
        "" => L["Seasons_AllSeasons"],
        _ => DashboardSeason?.Name ?? L["Index_NoSeason"]
    };

    /// <summary>True when season passes are meaningful for the current scope ("no season" is not).</summary>
    private bool HasSeasonScope => DashboardSeason != null || SeasonScope == "";

    private int SeasonPassCount =>
        DashboardSeason?.SeasonTicketCount
        ?? (SeasonScope == "" ? _seasons.Sum(s => s.SeasonTicketCount) : 0);

    /// <summary>Name of the selected event's season, or null when it has none.</summary>
    private string? SelectedEventSeasonName =>
        _selectedEvent?.SeasonId is int sid ? _seasons.FirstOrDefault(s => s.Id == sid)?.Name : null;

    /// <summary>True when the selected event belongs to a season other than the one shown in the banner.</summary>
    private bool SelectedEventInOtherSeason =>
        _selectedEvent?.SeasonId is int sid && DisplaySeason != null && sid != DisplaySeason.Id;

    /// <summary>
    /// Events in scope for the banner's selection: everything under "all seasons", the season-less
    /// events under "no season", otherwise that season's events. With no seasons configured at all,
    /// the whole list.
    /// </summary>
    private List<EventDto> SeasonEvents =>
        _seasons.Count == 0
            ? _events
            : SeasonScope switch
            {
                "" => _events,
                "none" => _events.Where(e => e.SeasonId == null).ToList(),
                _ => DisplaySeason is { } s ? _events.Where(e => e.SeasonId == s.Id).ToList() : _events
            };

    // Scoped metrics
    private int _ticketsSold;
    private int _seasonTicketsSold;
    /// <summary>Sold seats that are ordinary single-event tickets (total minus season-derived).</summary>
    private int NormalTicketsSold => Math.Max(0, _ticketsSold - _seasonTicketsSold);
    /// <summary>
    /// Seats still on sale for the selected event. Derived from the same capacity/sold pair the KPI
    /// value uses (rather than <c>EventDto.AvailableSeats</c> directly) so it stays in step with the
    /// live TicketSold push, which only updates the sold count.
    /// </summary>
    private int FreeSeats => Math.Max(0, (_selectedEvent?.Capacity ?? 0) - _ticketsSold);
    /// <summary>
    /// Realised ticket takings for the selected event: its non-cancelled single-event tickets plus
    /// this fixture's amortized share of season-pass revenue. Server-computed (see
    /// <c>EventDto.TicketRevenue</c>/<c>SeasonTicketRevenue</c>), so it refreshes on load rather than
    /// on the live TicketSold push — the sold *count* updates live, this figure does not.
    /// </summary>
    private decimal _ticketRevenue;

    /// <summary>
    /// Realised drink takings, summed from the live order set so SignalR order pushes move it
    /// immediately. Cancelled orders are excluded, matching the server's definition of realised
    /// revenue in <c>EventService.GetEventStatisticsAsync</c>.
    /// </summary>
    private decimal _drinksRevenue;

    /// <summary>
    /// Headline for the revenue card. Computed from the two figures above rather than read from
    /// <c>EventDto.TotalRevenue</c> so the headline always equals the sum of the chips shown under
    /// it — the server's total carries its own (load-time) drinks figure and would drift from the
    /// live one between refreshes.
    /// </summary>
    private decimal TotalEventRevenue => _ticketRevenue + _drinksRevenue;
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

    // True while this page is pushing the selected event's season into the shell banner.
    private bool _syncingSeasonToEvent;

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
        // Subscribe after the initial load so LoadAsync's own season sync isn't echoed back.
        SeasonState.OnChanged += OnSeasonStateChanged;
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

            // Seasons are owned by the shell banner; this joins its load rather than re-fetching.
            await SeasonState.EnsureLoadedAsync();

            // An explicit ?eventId= deep link is the one case that may re-point the banner: the
            // admin asked for that event, so the scope follows it. Otherwise the shell's season
            // wins — arriving at the dashboard must not silently re-point the banner at whatever
            // event happened to be picked.
            if (!TryApplyEventFromQuery())
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
    /// "Live" is driven by the lifecycle phase (Active), not timestamps.
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

    /// <summary>True when the selected event is actually in progress (drives the "not live" note).</summary>
    private bool SelectedEventIsLive => _selectedEvent?.Phase == EventPhase.Active;

    /// <summary>Recomputes all scoped metrics for the selected event from the loaded order set.</summary>
    private void ApplyEventScope()
    {
        // No event selected means nothing is in scope. Falling back to every order here used to
        // mix orders from all events together while the empty state still said "select an event".
        var scoped = _selectedEvent == null
            ? new List<OrderDto>()
            : _allOrders.Where(o => o.EventId == _selectedEvent.Id).ToList();

        _activeDrinkOrders = scoped.Count(o =>
            o.Status == OrderStatus.Pending ||
            o.Status == OrderStatus.Accepted ||
            o.Status == OrderStatus.InPreparation ||
            o.Status == OrderStatus.OutForDelivery);

        // Cancelled orders are excluded so this reflects realised sales, as the server does. The old
        // single "total revenue" figure summed every scoped order and counted cancellations as income.
        _drinksRevenue = scoped
            .Where(o => o.Status != OrderStatus.Cancelled)
            .Sum(o => o.TotalAmount);

        _ticketRevenue = (_selectedEvent?.TicketRevenue ?? 0m)
                         + (_selectedEvent?.SeasonTicketRevenue ?? 0m);

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

    /// <summary>Point the shell season banner at the selected event's season (when it has one).</summary>
    private void SyncSeasonToEvent()
    {
        // "All seasons" and "no season" are deliberate browse scopes: picking an event inside them
        // must not snap the banner onto that event's season and re-filter the list underneath.
        if (SeasonState.SelectedSeason == null)
            return;

        if (_selectedEvent?.SeasonId is not int sid || !_seasons.Any(s => s.Id == sid))
            return;

        // Publishing the season re-enters OnSeasonStateChanged; the flag stops that from
        // resetting the event we just synced *from*.
        _syncingSeasonToEvent = true;
        try { SeasonState.SetSelected(sid.ToString()); }
        finally { _syncingSeasonToEvent = false; }
    }

    /// <summary>The shell banner changed season: re-scope the dashboard, unless we caused it.</summary>
    private void OnSeasonStateChanged()
    {
        if (_syncingSeasonToEvent)
            return;

        _ = InvokeAsync(OnSeasonScopeChangedAsync);
    }

    /// <summary>A new season was chosen in the shell banner: re-scope the whole dashboard to it.</summary>
    private async Task OnSeasonScopeChangedAsync()
    {
        // Selecting a season resets the event scope to a sensible default within that season.
        _selectedEvent = PickDefaultEvent(SeasonEvents);
        ApplyEventScope();
        StateHasChanged();
        await RejoinEventAsync();
    }

    /// <summary>Guarantee the selected event is inside the banner's scope; otherwise pick a default there.</summary>
    private void EnsureEventInSeason()
    {
        if (_selectedEvent != null && SeasonEvents.Any(e => e.Id == _selectedEvent.Id))
            return;
        _selectedEvent = PickDefaultEvent(SeasonEvents);
    }

    /// <summary>
    /// Applies a <c>?eventId=</c> deep link: selects that event and moves the banner onto its scope
    /// (its season, or "no season" for a one-off event) so it stays visible in the picker.
    /// Returns false when there is no such parameter or no matching event.
    /// </summary>
    private bool TryApplyEventFromQuery()
    {
        if (EventIdParam is not int id)
            return false;

        var match = _events.FirstOrDefault(e => e.Id == id);
        if (match == null)
            return false;

        _selectedEvent = match;

        _syncingSeasonToEvent = true;
        try { SeasonState.SetSelected(match.SeasonId?.ToString() ?? "none"); }
        finally { _syncingSeasonToEvent = false; }

        return true;
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
        SeasonState.OnChanged -= OnSeasonStateChanged;

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
