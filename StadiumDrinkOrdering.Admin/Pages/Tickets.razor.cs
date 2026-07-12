using System.IO;
using System.Net.Http.Json;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Common;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Tickets : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private HttpClient HttpClient { get; set; } = default!;
    [Inject] private IConfiguration Configuration { get; set; } = default!;

    private List<TicketDto>? allTickets;
    private List<TicketDto>? filteredTickets;
    private List<EventDto>? events;
    private bool isLoading = true;
    private string? errorMessage;
    private string? searchTerm;
    private int? selectedSeasonId;
    private int? selectedEventId;
    private string? selectedStatus;

    // Season scope (mirrors the dashboard/orders): narrows the event dropdown + the ticket list.
    private List<SeasonDto> seasons = new();
    private List<(int Id, string Name)> seasonOptions = new();
    // EventId -> SeasonId, so a ticket (which carries only EventId) maps to its season.
    private Dictionary<int, int?> eventSeason = new();

    // Events offered in the event dropdown: all of them, or only the selected season's.
    private IEnumerable<EventDto> EventOptionsForSeason =>
        events == null ? Enumerable.Empty<EventDto>()
        : selectedSeasonId.HasValue ? events.Where(e => e.SeasonId == selectedSeasonId.Value)
        : events;

    /// <summary>The lifecycle states an admin cares about, in sort order.</summary>
    private enum TicketState { Active, Expired, Used, Cancelled }

    /// <summary>
    /// Collapses a ticket's <see cref="TicketDto.Status"/>/<see cref="TicketDto.IsUsed"/>/
    /// <see cref="TicketDto.IsActive"/> flags into a single display state. A refund/cancel is
    /// the only thing that clears <c>IsActive</c>, so treat that (or a Cancelled status) as
    /// cancelled; otherwise a used/scanned ticket shows as Used. A ticket that is neither used
    /// nor cancelled shows as Expired once its event day has passed (see <see cref="IsExpired"/>),
    /// else Active.
    /// </summary>
    private static TicketState StateOf(TicketDto t)
    {
        if (!t.IsActive || string.Equals(t.Status, TicketStatuses.Cancelled, StringComparison.OrdinalIgnoreCase))
            return TicketState.Cancelled;
        if (t.IsUsed || string.Equals(t.Status, TicketStatuses.Used, StringComparison.OrdinalIgnoreCase))
            return TicketState.Used;
        if (IsExpired(t))
            return TicketState.Expired;
        return TicketState.Active;
    }

    /// <summary>
    /// A single-event ticket is expired once its event day has passed while it was never used or
    /// cancelled — the passage of the event date, not any stored flag, is what makes it expired.
    /// Season passes span a whole season (a single <see cref="TicketDto.EventDate"/> doesn't bound
    /// their validity) so they never expire here, and tickets without an event date can't expire.
    /// Compared at UTC day granularity to match how event dates are stored/filtered elsewhere, so a
    /// ticket for an event happening today still counts as Active until the day rolls over.
    /// </summary>
    private static bool IsExpired(TicketDto t) =>
        t.Kind != TicketKind.Season
        && t.EventDate.HasValue
        && t.EventDate.Value.Date < DateTime.UtcNow.Date;

    // Ticket detail drill-down
    private bool showDetail;
    private bool detailLoading;
    private string? detailError;
    private TicketDetailDto? selectedDetail;
    private bool pdfDownloading;
    private string? pdfError;
    private int detailTicketId;

    // Stadium blueprint locator: cached sector overlays + the overlay matching the open ticket's seat.
    private List<StadiumSectorOverlay>? sectorOverlays;
    private StadiumSectorOverlay? matchedOverlay;
    // Exact pin for the individual seat within the matched sector (blueprint %). Null => pin the sector centre.
    private double? seatPinXPercent;
    private double? seatPinYPercent;

    // Sorting
    private readonly TableSortState sortState = new();
    private readonly PagedView<TicketDto> pager = new();
    private static readonly Dictionary<string, Func<TicketDto, object?>> SortSelectors = new()
    {
        ["number"] = t => t.TicketNumber,
        ["event"] = t => t.EventName,
        ["date"] = t => t.EventDate,
        ["seat"] = t => t.Section,
        ["customer"] = t => t.CustomerName,
        ["purchase"] = t => t.PurchaseDate,
        ["price"] = t => t.Price,
        ["status"] = t => (int)StateOf(t),
    };

    private void SortBy(string column)
    {
        sortState.Toggle(column);
        ApplyFilters();
    }

    protected override async Task OnInitializedAsync()
    {
        // Check if eventId query parameter is provided
        var uri = new Uri(Navigation.Uri);
        var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery(uri.Query);
        if (query.TryGetValue("eventId", out var eventIdValue) && int.TryParse(eventIdValue, out var eventId))
        {
            selectedEventId = eventId;
        }

        await LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            isLoading = true;
            errorMessage = null;

            // Load tickets (with optional event filter if coming from event details)
            var ticketsResponse = await ApiService.GetTicketsAsync(selectedEventId);
            if (ticketsResponse != null)
            {
                allTickets = ticketsResponse.ToList();
                ApplyFilters();
            }

            // Load events for filter dropdown
            var eventsResponse = await ApiService.GetEventsAsync();
            if (eventsResponse != null)
            {
                events = eventsResponse.Select(e => new EventDto
                {
                    Id = e.Id,
                    Name = e.Name,
                    Date = e.Date,
                    Description = e.Description,
                    Capacity = e.Capacity,
                    AvailableSeats = e.AvailableSeats,
                    BasePrice = e.BasePrice,
                    IsActive = e.IsActive,
                    CreatedAt = e.CreatedAt,
                    // Keep the season link so tickets can be scoped by season.
                    SeasonId = e.SeasonId,
                    SeasonName = e.SeasonName
                }).ToList();
                eventSeason = events.ToDictionary(e => e.Id, e => e.SeasonId);
            }

            // Load seasons for the season filter dropdown.
            try { seasons = await ApiService.GetAsync<List<SeasonDto>>("seasons") ?? new(); }
            catch (Exception ex) { Console.WriteLine($"Failed to load seasons: {ex.Message}"); }
            BuildSeasonOptions();
        }
        catch (Exception ex)
        {
            errorMessage = $"Error loading tickets: {ex.Message}";
        }
        finally
        {
            isLoading = false;
        }
    }

    private void BuildSeasonOptions()
    {
        // Only surface seasons that actually have tickets (matches how events are listed elsewhere).
        var seasonIdsWithTickets = (allTickets ?? new List<TicketDto>())
            .Where(t => t.EventId.HasValue)
            .Select(t => eventSeason.GetValueOrDefault(t.EventId!.Value))
            .Where(s => s.HasValue)
            .Select(s => s!.Value)
            .ToHashSet();

        seasonOptions = seasons
            .Where(s => seasonIdsWithTickets.Contains(s.Id))
            .OrderBy(s => s.StartDate)
            .ThenBy(s => s.Id)
            .Select(s => (s.Id, s.Name))
            .ToList();

        if (selectedSeasonId.HasValue && !seasonOptions.Any(s => s.Id == selectedSeasonId.Value))
            selectedSeasonId = null;
    }

    private Task FilterBySeason(string? seasonIdStr)
    {
        selectedSeasonId = string.IsNullOrEmpty(seasonIdStr) ? (int?)null
            : int.TryParse(seasonIdStr, out var sid) ? sid : (int?)null;

        // If the currently selected event isn't in the new season, drop it so the two stay consistent.
        if (selectedSeasonId.HasValue && selectedEventId.HasValue
            && eventSeason.GetValueOrDefault(selectedEventId.Value) != selectedSeasonId.Value)
        {
            selectedEventId = null;
        }
        ApplyFilters();
        return Task.CompletedTask;
    }

    private Task FilterByEvent(string? eventIdStr)
    {
        if (string.IsNullOrEmpty(eventIdStr))
        {
            selectedEventId = null;
        }
        else if (int.TryParse(eventIdStr, out var eventId))
        {
            selectedEventId = eventId;
        }
        ApplyFilters();
        return Task.CompletedTask;
    }

    private Task FilterByStatus(string? statusStr)
    {
        selectedStatus = string.IsNullOrEmpty(statusStr) ? null : statusStr;
        ApplyFilters();
        return Task.CompletedTask;
    }

    private Task SearchTickets()
    {
        ApplyFilters();
        return Task.CompletedTask;
    }

    private void ApplyFilters()
    {
        if (allTickets == null)
        {
            filteredTickets = null;
            return;
        }

        var filtered = allTickets.AsEnumerable();

        // Apply season filter (scopes the whole list to one season's events)
        if (selectedSeasonId.HasValue)
        {
            filtered = filtered.Where(t => t.EventId.HasValue
                && eventSeason.GetValueOrDefault(t.EventId.Value) == selectedSeasonId.Value);
        }

        // Apply event filter
        if (selectedEventId.HasValue)
        {
            filtered = filtered.Where(t => t.EventId == selectedEventId.Value);
        }

        // Apply status filter (Active / Used / Cancelled)
        if (!string.IsNullOrEmpty(selectedStatus)
            && Enum.TryParse<TicketState>(selectedStatus, ignoreCase: true, out var wantedState))
        {
            filtered = filtered.Where(t => StateOf(t) == wantedState);
        }

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var term = searchTerm.ToLower();
            filtered = filtered.Where(t =>
                t.TicketNumber.ToLower().Contains(term) ||
                (t.CustomerName?.ToLower().Contains(term) ?? false) ||
                (t.CustomerEmail?.ToLower().Contains(term) ?? false) ||
                (t.EventName?.ToLower().Contains(term) ?? false)
            );
        }

        var ordered = sortState.Column is null
            ? filtered
            : sortState.Apply(filtered, SortSelectors);
        filteredTickets = ordered.ToList();
        pager.Source = filteredTickets;
        pager.Reset();
    }

    private Task ResetFilters()
    {
        searchTerm = null;
        selectedSeasonId = null;
        selectedEventId = null;
        selectedStatus = null;
        filteredTickets = allTickets;
        pager.Source = filteredTickets ?? new List<TicketDto>();
        pager.Reset();
        return Task.CompletedTask;
    }

    private void ViewOrder(int orderId)
    {
        Navigation.NavigateTo($"/orders?id={orderId}");
    }

    private async Task ValidateTicket(string ticketNumber)
    {
        try
        {
            var isValid = await ApiService.ValidateTicketAsync(ticketNumber);

            if (isValid)
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Ticket '{ticketNumber}' is VALID!");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("alert", $"Ticket '{ticketNumber}' is INVALID!");
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Error validating ticket: {ex.Message}");
        }
    }

    private async Task DeactivateTicket(int ticketId)
    {
        try
        {
            var success = await ApiService.UpdateTicketStatusAsync(ticketId, false);
            if (success)
            {
                await LoadData();
                await JSRuntime.InvokeVoidAsync("alert", "Ticket deactivated successfully!");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("alert", "Failed to deactivate ticket.");
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Error deactivating ticket: {ex.Message}");
        }
    }

    private async Task ActivateTicket(int ticketId)
    {
        try
        {
            var success = await ApiService.UpdateTicketStatusAsync(ticketId, true);
            if (success)
            {
                await LoadData();
                await JSRuntime.InvokeVoidAsync("alert", "Ticket activated successfully!");
            }
            else
            {
                await JSRuntime.InvokeVoidAsync("alert", "Failed to activate ticket.");
            }
        }
        catch (Exception ex)
        {
            await JSRuntime.InvokeVoidAsync("alert", $"Error activating ticket: {ex.Message}");
        }
    }

    // ---- Ticket detail drill-down ----

    private async Task OpenDetail(int ticketId)
    {
        detailTicketId = ticketId;
        showDetail = true;
        detailLoading = true;
        detailError = null;
        pdfError = null;
        selectedDetail = null;
        StateHasChanged();

        try
        {
            selectedDetail = await ApiService.GetTicketDetailsAsync(ticketId);
            if (selectedDetail == null)
            {
                detailError = "Could not load ticket details.";
            }
            else
            {
                await ResolveSeatLocationAsync(selectedDetail);
            }
        }
        catch (Exception ex)
        {
            detailError = $"Error loading ticket details: {ex.Message}";
        }
        finally
        {
            detailLoading = false;
        }
    }

    private void CloseDetail()
    {
        showDetail = false;
        selectedDetail = null;
        pdfError = null;
        matchedOverlay = null;
        seatPinXPercent = null;
        seatPinYPercent = null;
    }

    /// <summary>
    /// Locates the ticket's seat on the stadium blueprint by matching its <see cref="TicketDetailDto.Section"/>
    /// against the drawing-tool sector overlays (same source the admin drawing tool renders). The matched
    /// overlay carries percentage-based coordinates so the modal can drop a pin on the real blueprint image.
    /// Overlays are fetched once per page and cached; failures degrade silently to "location not available".
    /// </summary>
    private async Task ResolveSeatLocationAsync(TicketDetailDto detail)
    {
        matchedOverlay = null;

        var section = detail.Section?.Trim();
        if (string.IsNullOrEmpty(section))
            return;

        try
        {
            if (sectorOverlays == null)
            {
                var apiBaseUrl = Configuration.GetValue<string>("ApiSettings:BaseUrl")?.TrimEnd('/') ?? "https://localhost:7010";
                var jsonOptions = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                sectorOverlays = await HttpClient.GetFromJsonAsync<List<StadiumSectorOverlay>>(
                    $"{apiBaseUrl}/api/StadiumSectorOverlay", jsonOptions) ?? new();
            }

            matchedOverlay = sectorOverlays.FirstOrDefault(o =>
                string.Equals(o.SectorCode?.Trim(), section, StringComparison.OrdinalIgnoreCase));

            ComputeSeatPin(matchedOverlay, detail);
        }
        catch
        {
            // Blueprint locator is a nice-to-have; never block the ticket detail on it.
            matchedOverlay = null;
            seatPinXPercent = null;
            seatPinYPercent = null;
        }
    }

    /// <summary>
    /// Computes the individual seat's position on the blueprint (as blueprint-wide percentages) by
    /// treating the sector as the uniform Rows × SeatsPerRow grid the drawing tool generates: row 1 is
    /// the front row, seat 1 the first seat. The seat is placed at the centre of its grid cell. Leaves
    /// the pin null (so the caller falls back to the sector centre) when the row/seat can't be parsed.
    /// </summary>
    private void ComputeSeatPin(StadiumSectorOverlay? overlay, TicketDetailDto detail)
    {
        seatPinXPercent = null;
        seatPinYPercent = null;

        if (overlay == null || overlay.Rows < 1 || overlay.SeatsPerRow < 1)
            return;

        if (!int.TryParse(detail.Row, out var row) || !int.TryParse(detail.SeatNumber, out var seat))
            return;

        // Clamp into the sector's grid so an out-of-range seat still lands inside the footprint.
        row = Math.Clamp(row, 1, overlay.Rows);
        seat = Math.Clamp(seat, 1, overlay.SeatsPerRow);

        var colFraction = (seat - 0.5) / overlay.SeatsPerRow;
        var rowFraction = (row - 0.5) / overlay.Rows;

        seatPinXPercent = overlay.LeftPercent + colFraction * overlay.WidthPercent;
        seatPinYPercent = overlay.TopPercent + rowFraction * overlay.HeightPercent;
    }

    private async Task DownloadCardPdf()
    {
        if (pdfDownloading) return;
        pdfDownloading = true;
        pdfError = null;
        StateHasChanged();

        try
        {
            var bytes = await ApiService.GetTicketCardPdfAsync(detailTicketId);
            if (bytes == null || bytes.Length == 0)
            {
                pdfError = L["Tickets_DetailDownloadFailed"];
                return;
            }

            var fileName = $"ticket-{selectedDetail?.TicketNumber ?? detailTicketId.ToString()}.pdf";
            using var stream = new MemoryStream(bytes);
            using var streamRef = new DotNetStreamReference(stream);
            await JSRuntime.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
        }
        catch (Exception)
        {
            pdfError = L["Tickets_DetailDownloadFailed"];
        }
        finally
        {
            pdfDownloading = false;
        }
    }

    // ---- View helpers ----

    private static string Dash(string? value) => string.IsNullOrWhiteSpace(value) ? "—" : value!;

    private static string Money(decimal amount) => $"€{amount:0.00}";

    private static string SeatText(TicketDetailDto d)
    {
        var parts = new List<string>();
        if (!string.IsNullOrWhiteSpace(d.Section)) parts.Add(d.Section!);
        if (!string.IsNullOrWhiteSpace(d.Row)) parts.Add($"Row {d.Row}");
        if (!string.IsNullOrWhiteSpace(d.SeatNumber)) parts.Add($"Seat {d.SeatNumber}");
        return parts.Count == 0 ? "—" : string.Join(" · ", parts);
    }

    // ---- Stadium blueprint locator helpers ----

    /// <summary>Centre X of the matched sector as a percentage of blueprint width (for pin placement).</summary>
    private static double OverlayCenterX(StadiumSectorOverlay o) => o.LeftPercent + o.WidthPercent / 2.0;

    /// <summary>Centre Y of the matched sector as a percentage of blueprint height (for pin placement).</summary>
    private static double OverlayCenterY(StadiumSectorOverlay o) => o.TopPercent + o.HeightPercent / 2.0;

    /// <summary>Formats a percentage for inline CSS using invariant culture (avoids comma decimals).</summary>
    private static string Pct(double value) => value.ToString("0.###", System.Globalization.CultureInfo.InvariantCulture);
}