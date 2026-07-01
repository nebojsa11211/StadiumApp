using Microsoft.AspNetCore.Components;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Index : ComponentBase
{
    [Inject] private IAdminApiService AdminApiService { get; set; } = default!;

    private bool _loading = true;

    // Event scoping — the whole dashboard is scoped to one selected event.
    private List<EventDto> _events = new();
    private EventDto? _selectedEvent;
    private List<OrderDto> _allOrders = new();

    // Scoped metrics
    private int _ticketsSold;
    private decimal _eventRevenue;
    private int _activeDrinkOrders;
    private List<OrderDto> _recentOrders = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadAsync();
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

            ApplyEventScope();
        }
        finally
        {
            _loading = false;
            StateHasChanged();
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
    }

    // ----- Event navigation (re-filters already-loaded orders, no network) -----
    private bool HasEvents => _events.Count > 0;
    private int SelectedIndex => _selectedEvent == null ? -1 : _events.FindIndex(e => e.Id == _selectedEvent.Id);
    private bool CanGoPrev => SelectedIndex > 0;
    private bool CanGoNext => SelectedIndex >= 0 && SelectedIndex < _events.Count - 1;
    private bool IsOnDefaultEvent => _selectedEvent != null && PickDefaultEvent(_events)?.Id == _selectedEvent.Id;

    private void SelectPrevEvent()
    {
        if (CanGoPrev)
        {
            _selectedEvent = _events[SelectedIndex - 1];
            ApplyEventScope();
        }
    }

    private void SelectNextEvent()
    {
        if (CanGoNext)
        {
            _selectedEvent = _events[SelectedIndex + 1];
            ApplyEventScope();
        }
    }

    private void OnSelectEvent(ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var id))
        {
            var match = _events.FirstOrDefault(x => x.Id == id);
            if (match != null)
            {
                _selectedEvent = match;
                ApplyEventScope();
            }
        }
    }

    private void GoToCurrentEvent()
    {
        var def = PickDefaultEvent(_events);
        if (def != null)
        {
            _selectedEvent = def;
            ApplyEventScope();
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
}
