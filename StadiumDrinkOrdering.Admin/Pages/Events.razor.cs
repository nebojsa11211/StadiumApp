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

    private async Task ActivateEvent(int eventId)
    {
        var response = await ApiService.Http.PostAsync<bool>($"events/{eventId}/activate", null);
        if (response)
        {
            await LoadEvents();
            ShowAlert("Event activated successfully", "success");
        }
        else
        {
            ShowAlert("Failed to activate event", "danger");
        }
    }

    private async Task DeactivateEvent(int eventId)
    {
        var response = await ApiService.Http.PostAsync<bool>($"events/{eventId}/deactivate", null);
        if (response)
        {
            await LoadEvents();
            ShowAlert("Event deactivated successfully", "success");
        }
        else
        {
            ShowAlert("Failed to deactivate event", "danger");
        }
    }

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