using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;

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
    private bool? selectedStatus;

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
                    Location = "Stadium", // Default location since Event model doesn't have Location
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
        if (string.IsNullOrEmpty(statusStr))
        {
            selectedStatus = null;
        }
        else if (bool.TryParse(statusStr, out var status))
        {
            selectedStatus = status;
        }
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

        // Apply status filter
        if (selectedStatus.HasValue)
        {
            filtered = filtered.Where(t => t.IsActive == selectedStatus.Value);
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

        filteredTickets = filtered.ToList();
    }

    private Task ResetFilters()
    {
        searchTerm = null;
        selectedEventId = null;
        selectedStatus = null;
        filteredTickets = allTickets;
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
        // This would require adding an endpoint to update ticket status
        await JSRuntime.InvokeVoidAsync("alert", "Deactivate ticket functionality not yet implemented in API");
    }

    private async Task ActivateTicket(int ticketId)
    {
        // This would require adding an endpoint to update ticket status
        await JSRuntime.InvokeVoidAsync("alert", "Activate ticket functionality not yet implemented in API");
    }
}