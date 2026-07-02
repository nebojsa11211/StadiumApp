using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.DTOs.Integration;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class StadiumOverview : ComponentBase, IDisposable
{
    [Inject] public IAdminApiService ApiService { get; set; } = default!;
    [Inject] public ISignalRService SignalRService { get; set; } = default!;
    [Inject] public ILogger<StadiumOverview> Logger { get; set; } = default!;

    /// <summary>Optional deep-link from the events list: /admin/stadium-overview?eventId=N</summary>
    [Parameter, SupplyParameterFromQuery(Name = "eventId")]
    public int? EventIdParam { get; set; }

    private List<EventDto> events = new();
    private EventSalesSnapshotDto? snapshot;

    private int _selectedEventId;
    public int SelectedEventId
    {
        get => _selectedEventId;
        set => _selectedEventId = value;
    }

    private int _joinedEventId;
    private bool isLoading;
    private string? errorMessage;
    private DateTime? lastUpdate;

    // Capacity-planning assumptions (editable in the UI)
    public double LitersPerAttendee { get; set; } = 1.5;
    public int AttendeesPerStaff { get; set; } = 100;
    private const double ServingLiters = 0.5;

    public bool IsLive => SignalRService.IsConnected;

    private IEnumerable<SectorSalesDto> Sectors =>
        snapshot?.Sectors.OrderBy(s => s.SectionCode) ?? Enumerable.Empty<SectorSalesDto>();

    private int TotalCapacity => Sectors.Sum(s => s.Capacity);
    private int TotalSold => Sectors.Sum(s => s.Sold);
    private decimal OverallOccupancy => TotalCapacity > 0
        ? Math.Round((decimal)TotalSold / TotalCapacity * 100, 1)
        : 0;

    private double BeerLiters => TotalSold * LitersPerAttendee;
    private double BeerServings => ServingLiters > 0 ? BeerLiters / ServingLiters : 0;
    private int RecommendedStaff => (int)Math.Ceiling(TotalSold / (double)Math.Max(1, AttendeesPerStaff));

    private static decimal OccupancyOf(SectorSalesDto s) =>
        s.Capacity > 0 ? Math.Round((decimal)s.Sold / s.Capacity * 100, 1) : 0;

    private double SectorBeer(SectorSalesDto s) => s.Sold * LitersPerAttendee;
    private int SectorStaff(SectorSalesDto s) =>
        s.Sold <= 0 ? 0 : (int)Math.Ceiling(s.Sold / (double)Math.Max(1, AttendeesPerStaff));

    private static string OccupancyClass(decimal pct) => pct switch
    {
        >= 90 => "occ-high",
        >= 50 => "occ-mid",
        _ => "occ-low"
    };

    protected override async Task OnInitializedAsync()
    {
        await LoadEventsAsync();
        await InitializeSignalRAsync();

        // Deep-link support: preselect the event passed via ?eventId=N
        if (EventIdParam is > 0 && events.Any(e => e.Id == EventIdParam.Value))
        {
            SelectedEventId = EventIdParam.Value;
            await OnEventChanged();
        }
    }

    private async Task LoadEventsAsync()
    {
        isLoading = true;
        try
        {
            var result = await ApiService.GetAsync<List<EventDto>>("events/active");
            events = (result ?? new List<EventDto>())
                .OrderByDescending(e => e.Date ?? DateTime.MinValue)
                .ToList();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load events for stadium overview");
            errorMessage = "Failed to load events.";
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task InitializeSignalRAsync()
    {
        SignalRService.TicketSold += OnTicketSold;
        try
        {
            await SignalRService.StartAsync();
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
                    Logger.LogWarning(retryEx, "SignalR retry failed on stadium overview");
                }
            });
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to start SignalR on stadium overview");
        }
    }

    private async Task OnEventChanged()
    {
        errorMessage = null;
        snapshot = null;

        if (SelectedEventId <= 0)
        {
            await LeaveCurrentEventAsync();
            return;
        }

        await LoadSeatStatusAsync(SelectedEventId);
        await RejoinEventAsync();
    }

    private async Task LoadSeatStatusAsync(int eventId)
    {
        isLoading = true;
        try
        {
            // Real (overlay) sectors + sold counts for this event.
            snapshot = await ApiService.GetAsync<EventSalesSnapshotDto>($"api/integration/ticketing/occupancy/{eventId}");
            lastUpdate = DateTime.Now;
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load seat status for event {EventId}", eventId);
            errorMessage = "Failed to load seat status for this event.";
        }
        finally
        {
            isLoading = false;
        }
    }

    private async Task RejoinEventAsync()
    {
        if (!SignalRService.IsConnected)
            return;

        await LeaveCurrentEventAsync();

        if (SelectedEventId > 0)
        {
            try
            {
                await SignalRService.JoinEvent(SelectedEventId);
                _joinedEventId = SelectedEventId;
            }
            catch (Exception ex)
            {
                Logger.LogWarning(ex, "Failed to join event group {EventId}", SelectedEventId);
            }
        }
    }

    private async Task LeaveCurrentEventAsync()
    {
        if (_joinedEventId > 0 && SignalRService.IsConnected)
        {
            try { await SignalRService.LeaveEvent(_joinedEventId); }
            catch { /* best effort */ }
        }
        _joinedEventId = 0;
    }

    private void OnTicketSold(TicketSoldNotification n)
    {
        if (n.EventId != SelectedEventId || snapshot == null)
            return;

        _ = InvokeAsync(() =>
        {
            var sector = snapshot.Sectors.FirstOrDefault(s => s.SectionCode == n.SectionCode);
            if (sector != null)
            {
                sector.Sold = n.SoldInSection;
                if (n.SectionCapacity > 0)
                    sector.Capacity = n.SectionCapacity;
            }
            else if (!string.IsNullOrEmpty(n.SectionCode))
            {
                snapshot.Sectors.Add(new SectorSalesDto
                {
                    SectionCode = n.SectionCode,
                    Capacity = n.SectionCapacity,
                    Sold = n.SoldInSection
                });
            }

            snapshot.TotalSold = n.TotalSold;
            lastUpdate = DateTime.Now;
            StateHasChanged();
        });
    }

    public void Dispose()
    {
        try
        {
            SignalRService.TicketSold -= OnTicketSold;
            _ = LeaveCurrentEventAsync();
        }
        catch (ObjectDisposedException) { /* already gone */ }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Error disposing stadium overview");
        }
    }
}
