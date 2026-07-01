using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Events : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<EventDto>? events;
    private EventDto? editingEvent;
    private bool showEventModal = false;
    private bool isSaving = false;
    private string alertMessage = "";
    private string alertType = "";
    private bool loadingFailed = false;
    private string loadingError = "";

    private EventFormModel eventForm = new();

    protected override async Task OnInitializedAsync()
    {
        await LoadEvents();
    }

    private async Task LoadEvents()
    {
        try
        {
            loadingFailed = false;
            loadingError = "";
            var result = await ApiService.GetEventsAsync();

            if (result == null)
            {
                loadingFailed = true;
                loadingError = "Failed to load events. The server returned an error.";
                events = new List<EventDto>();
            }
            else
            {
                events = result.ToList();
            }
        }
        catch (Exception ex)
        {
            loadingFailed = true;
            loadingError = $"An error occurred while loading events: {ex.Message}";
            events = new List<EventDto>();
        }
    }

    private void ShowCreateEventModal()
    {
        editingEvent = null;
        eventForm = new EventFormModel
        {
            Date = DateTime.Now.AddDays(30),
            IsActive = true,
            Capacity = 50000,
            BasePrice = 50
        };
        showEventModal = true;
    }

    private void ShowEditEventModal(EventDto evt)
    {
        editingEvent = evt;
        eventForm = new EventFormModel
        {
            Name = evt.Name,
            Description = evt.Description,
            Date = evt.Date ?? DateTime.Now,
            Location = evt.Location ?? "",
            Capacity = evt.Capacity,
            BasePrice = evt.BasePrice,
            IsActive = evt.IsActive
        };
        showEventModal = true;
    }

    private void HideEventModal()
    {
        showEventModal = false;
        editingEvent = null;
        eventForm = new();
    }

    private async Task SaveEvent()
    {
        if (string.IsNullOrWhiteSpace(eventForm.Name) ||
            string.IsNullOrWhiteSpace(eventForm.Location) ||
            eventForm.Capacity <= 0 ||
            eventForm.BasePrice <= 0)
        {
            ShowAlert("Please fill in all required fields", "danger");
            return;
        }

        isSaving = true;
        try
        {
            if (editingEvent == null)
            {
                // Create new event
                var createDto = new CreateEventDto
                {
                    Name = eventForm.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(eventForm.Description) ? null : eventForm.Description.Trim(),
                    Date = eventForm.Date,
                    Location = eventForm.Location.Trim(),
                    Capacity = eventForm.Capacity,
                    BasePrice = eventForm.BasePrice,
                    IsActive = eventForm.IsActive
                };

                var response = await ApiService.Http.PostAsync<EventDto>("events", createDto);
                if (response != null)
                {
                    await LoadEvents();
                    HideEventModal();
                    ShowAlert($"Event '{response.Name}' created successfully", "success");
                }
                else
                {
                    ShowAlert("Failed to create event", "danger");
                }
            }
            else
            {
                // Update existing event
                var updateDto = new UpdateEventDto
                {
                    Name = eventForm.Name.Trim(),
                    Description = string.IsNullOrWhiteSpace(eventForm.Description) ? null : eventForm.Description.Trim(),
                    Date = eventForm.Date,
                    Location = eventForm.Location.Trim(),
                    Capacity = eventForm.Capacity,
                    BasePrice = eventForm.BasePrice,
                    IsActive = eventForm.IsActive
                };

                var response = await ApiService.Http.PostAsync<EventDto>($"events/{editingEvent.Id}", updateDto);
                if (response != null)
                {
                    await LoadEvents();
                    HideEventModal();
                    ShowAlert($"Event '{eventForm.Name}' updated successfully", "success");
                }
                else
                {
                    ShowAlert("Failed to update event", "danger");
                }
            }
        }
        finally
        {
            isSaving = false;
        }
    }

    private async Task TransitionStatus(EventDto evt, EventStatus newStatus)
    {
        // Confirm irreversible (terminal) transitions, which also invalidate ticket sessions.
        if (newStatus is EventStatus.Cancelled or EventStatus.Completed)
        {
            var verb = newStatus == EventStatus.Cancelled ? "cancel" : "complete";
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm",
                $"Are you sure you want to {verb} '{evt.Name}'? This invalidates any active ticket sessions and cannot be undone.");
            if (!confirmed)
                return;
        }

        try
        {
            // Use the raw HttpResponseMessage overload so we can surface the API's
            // state-machine rejection reason (400 { "message": ... }) instead of a generic error.
            var response = await ApiService.Http.PostAsync(
                $"events/{evt.Id}/status",
                new TransitionEventStatusRequest { NewStatus = newStatus });

            if (response.IsSuccessStatusCode)
            {
                await LoadEvents();
                ShowAlert($"'{evt.Name}' is now {StatusDisplay(newStatus)}", "success");
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                ShowAlert(ExtractErrorMessage(body) ?? $"Could not change status ({(int)response.StatusCode})", "danger");
            }
        }
        catch (Exception ex)
        {
            ShowAlert($"Could not change status: {ex.Message}", "danger");
        }
    }

    private static string? ExtractErrorMessage(string body)
    {
        if (string.IsNullOrWhiteSpace(body))
            return null;
        try
        {
            using var doc = JsonDocument.Parse(body);
            if (doc.RootElement.ValueKind == JsonValueKind.Object &&
                doc.RootElement.TryGetProperty("message", out var message))
            {
                return message.GetString();
            }
        }
        catch (JsonException)
        {
            // Non-JSON body — fall through.
        }
        return null;
    }

    // --- Lifecycle display helpers (badge colour, button style, labels) ---

    private static string StatusDisplay(EventStatus status) => status switch
    {
        EventStatus.OnSale => "On Sale",
        EventStatus.SoldOut => "Sold Out",
        EventStatus.InProgress => "In Progress",
        _ => status.ToString()
    };

    private static string StatusBadgeClass(EventStatus status) => status switch
    {
        EventStatus.Planned => "is-pending",
        EventStatus.OnSale => "is-accepted",
        EventStatus.SoldOut => "is-ready",
        EventStatus.Active => "is-active",
        EventStatus.InProgress => "is-active",
        EventStatus.Completed => "is-inactive",
        EventStatus.Cancelled => "is-cancelled",
        _ => "is-inactive"
    };

    private static string TransitionButtonClass(EventStatus target) => target switch
    {
        EventStatus.OnSale => "pill-btn pill-btn--primary",
        EventStatus.Active or EventStatus.InProgress => "pill-btn pill-btn--success",
        EventStatus.Cancelled => "pill-btn pill-btn--danger",
        _ => "pill-btn"
    };

    private static (string Label, string Icon) TransitionMeta(EventStatus target) => target switch
    {
        EventStatus.Planned => ("Unpublish", "bi-arrow-counterclockwise"),
        EventStatus.OnSale => ("Put On Sale", "bi-megaphone"),
        EventStatus.SoldOut => ("Mark Sold Out", "bi-x-octagon"),
        EventStatus.Active => ("Go Live", "bi-broadcast"),
        EventStatus.InProgress => ("Start Play", "bi-play-circle"),
        EventStatus.Completed => ("Complete", "bi-check2-circle"),
        EventStatus.Cancelled => ("Cancel Event", "bi-x-circle"),
        _ => (target.ToString(), "bi-arrow-right")
    };

    private async Task DeleteEvent(int eventId)
    {
        if (await JSRuntime.InvokeAsync<bool>("confirm", "Are you sure you want to delete this event? This action cannot be undone."))
        {
            var (success, errorMessage) = await ApiService.Http.DeleteAsync($"events/{eventId}");
            if (success)
            {
                await LoadEvents();
                ShowAlert("Event deleted successfully", "success");
            }
            else
            {
                ShowAlert($"Failed to delete event: {errorMessage}", "danger");
            }
        }
    }

    private void ShowAlert(string message, string type)
    {
        alertMessage = message;
        alertType = type;
        StateHasChanged();

        // Auto-hide after 5 seconds
        _ = Task.Delay(5000).ContinueWith(_ =>
        {
            alertMessage = "";
            InvokeAsync(StateHasChanged);
        });
    }

    private void ClearAlert()
    {
        alertMessage = "";
    }

    private class EventFormModel
    {
        public string Name { get; set; } = "";
        public string? Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.AddDays(30);
        public string Location { get; set; } = "";
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; } = true;
    }
}