using System.IO;
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

    private List<TicketDto>? allTickets;
    private List<TicketDto>? filteredTickets;
    private List<EventDto>? events;
    private bool isLoading = true;
    private string? errorMessage;
    private string? searchTerm;
    private int? selectedEventId;
    private string? selectedStatus;

    /// <summary>The three real lifecycle states an admin cares about, in sort order.</summary>
    private enum TicketState { Active, Used, Cancelled }

    /// <summary>
    /// Collapses a ticket's <see cref="TicketDto.Status"/>/<see cref="TicketDto.IsUsed"/>/
    /// <see cref="TicketDto.IsActive"/> flags into a single display state. A refund/cancel is
    /// the only thing that clears <c>IsActive</c>, so treat that (or a Cancelled status) as
    /// cancelled; otherwise a used/scanned ticket shows as Used, else Active.
    /// </summary>
    private static TicketState StateOf(TicketDto t)
    {
        if (!t.IsActive || string.Equals(t.Status, TicketStatuses.Cancelled, StringComparison.OrdinalIgnoreCase))
            return TicketState.Cancelled;
        if (t.IsUsed || string.Equals(t.Status, TicketStatuses.Used, StringComparison.OrdinalIgnoreCase))
            return TicketState.Used;
        return TicketState.Active;
    }

    // Ticket detail drill-down
    private bool showDetail;
    private bool detailLoading;
    private string? detailError;
    private TicketDetailDto? selectedDetail;
    private bool pdfDownloading;
    private string? pdfError;
    private int detailTicketId;

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
                    CreatedAt = e.CreatedAt
                }).ToList();
            }
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
}