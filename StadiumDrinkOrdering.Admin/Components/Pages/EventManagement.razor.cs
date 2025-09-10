using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Admin.Services;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;

namespace StadiumDrinkOrdering.Admin.Components.Pages;

[Authorize(Roles = "Admin")]
public partial class EventManagement : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private ILogger<EventManagement> Logger { get; set; } = default!;

    private List<Event> events = new();
    private List<Event> filteredEvents = new();
    private string selectedEventType = "";
    private string selectedStatus = "";
    private string searchQuery = "";
    private string viewMode = "grid";
    private DateTime currentCalendarDate = DateTime.Now;
    
    private bool showEventModal = false;
    private bool showDetailsModal = false;
    private bool isLoading = true;
    private bool isSaving = false;
    
    private Event? editingEvent = null;
    private Event? selectedEvent = null;
    private EventForm eventForm = new();
    
    private decimal totalRevenue = 0;
    private int totalTicketsSold = 0;
    
    private bool showToast = false;
    private string toastMessage = "";
    private string toastType = "";

    protected override async Task OnInitializedAsync()
    {
        await LoadEvents();
    }

    private async Task LoadEvents()
    {
        try
        {
            isLoading = true;
            events = await ApiService.GetEventsAsync() ?? new List<Event>();
            CalculateStats();
            FilterEvents();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to load events");
            ShowToast("Failed to load events", "error");
        }
        finally
        {
            isLoading = false;
            StateHasChanged();
        }
    }

    private void CalculateStats()
    {
        totalRevenue = (events ?? new List<Event>()).Where(e => e.Analytics != null).Sum(e => e.Analytics.TotalRevenue);
        totalTicketsSold = (events ?? new List<Event>()).Where(e => e.Analytics != null).Sum(e => e.Analytics.TotalTicketsSold);
    }

    private void FilterEvents()
    {
        filteredEvents = (events ?? new List<Event>()).Where(e =>
        {
            var matchesType = string.IsNullOrEmpty(selectedEventType) || e.EventType.Contains(selectedEventType, StringComparison.OrdinalIgnoreCase);
            
            var matchesStatus = selectedStatus switch
            {
                "active" => e.IsActive,
                "inactive" => !e.IsActive,
                "upcoming" => e.EventDate > DateTime.UtcNow,
                "past" => e.EventDate < DateTime.UtcNow,
                _ => true
            };
            
            var matchesSearch = string.IsNullOrEmpty(searchQuery) || 
                              e.EventName.Contains(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                              (e.Description ?? "").Contains(searchQuery, StringComparison.OrdinalIgnoreCase);
            
            return matchesType && matchesStatus && matchesSearch;
        }).OrderByDescending(e => e.EventDate).ToList();
        
        StateHasChanged();
    }

    private void SetViewMode(string mode)
    {
        viewMode = mode;
        StateHasChanged();
    }

    private void SetGridView()
    {
        SetViewMode("grid");
    }

    private void SetCalendarView()
    {
        SetViewMode("calendar");
    }

    private void ShowCreateEventModal()
    {
        editingEvent = null;
        eventForm = new EventForm();
        showEventModal = true;
        StateHasChanged();
    }

    private void EditEvent(Event eventItem)
    {
        editingEvent = eventItem;
        eventForm = new EventForm
        {
            EventName = eventItem.EventName,
            EventType = eventItem.EventType,
            EventDate = eventItem.EventDate,
            TotalSeats = eventItem.TotalSeats,
            BaseTicketPrice = eventItem.BaseTicketPrice,
            ImageUrl = eventItem.ImageUrl,
            Description = eventItem.Description
        };
        showEventModal = true;
        if (showDetailsModal) showDetailsModal = false;
        StateHasChanged();
    }

    private void CloseEventModal()
    {
        showEventModal = false;
        editingEvent = null;
        StateHasChanged();
    }

    private async Task SaveEvent()
    {
        try
        {
            isSaving = true;
            StateHasChanged();

            if (editingEvent == null)
            {
                // Create new event
                var newEvent = new Event
                {
                    EventName = eventForm.EventName,
                    EventType = eventForm.EventType,
                    EventDate = eventForm.EventDate,
                    TotalSeats = eventForm.TotalSeats,
                    BaseTicketPrice = eventForm.BaseTicketPrice,
                    ImageUrl = eventForm.ImageUrl,
                    Description = eventForm.Description
                };

                await ApiService.CreateEventAsync(newEvent);
                ShowToast("Event created successfully!", "success");
            }
            else
            {
                // Update existing event
                editingEvent.EventName = eventForm.EventName;
                editingEvent.EventType = eventForm.EventType;
                editingEvent.EventDate = eventForm.EventDate;
                editingEvent.TotalSeats = eventForm.TotalSeats;
                editingEvent.BaseTicketPrice = eventForm.BaseTicketPrice;
                editingEvent.ImageUrl = eventForm.ImageUrl;
                editingEvent.Description = eventForm.Description;

                await ApiService.UpdateEventAsync(editingEvent.Id, editingEvent);
                ShowToast("Event updated successfully!", "success");
            }

            await LoadEvents();
            CloseEventModal();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to save event");
            ShowToast("Failed to save event", "error");
        }
        finally
        {
            isSaving = false;
            StateHasChanged();
        }
    }

    private void ShowEventDetails(Event eventItem)
    {
        selectedEvent = eventItem;
        showDetailsModal = true;
        StateHasChanged();
    }

    private void CloseDetailsModal()
    {
        showDetailsModal = false;
        selectedEvent = null;
        StateHasChanged();
    }

    private async Task ActivateEvent(Event eventItem)
    {
        try
        {
            await ApiService.ActivateEventAsync(eventItem.Id);
            eventItem.IsActive = true;
            ShowToast($"{eventItem.EventName} activated!", "success");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to activate event {EventId}", eventItem.Id);
            ShowToast("Failed to activate event", "error");
        }
    }

    private async Task DeactivateEvent(Event eventItem)
    {
        try
        {
            await ApiService.DeactivateEventAsync(eventItem.Id);
            eventItem.IsActive = false;
            ShowToast($"{eventItem.EventName} deactivated!", "warning");
            StateHasChanged();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to deactivate event {EventId}", eventItem.Id);
            ShowToast("Failed to deactivate event", "error");
        }
    }

    private async Task GenerateDemoData()
    {
        try
        {
            // Ensure events are loaded
            await LoadEvents();
            
            if (events == null || !events.Any())
            {
                Logger.LogInformation("No events available. Creating demo events first...");
                
                // Create demo events first
                var demoEvents = new[]
                {
                    new Event 
                    { 
                        EventName = "Champions League Final", 
                        EventType = "Football", 
                        EventDate = DateTime.Today.AddDays(7).AddHours(20), 
                        TotalSeats = 65000, 
                        BaseTicketPrice = 150.00m, 
                        Description = "The ultimate football showdown",
                        IsActive = true
                    },
                    new Event 
                    { 
                        EventName = "Summer Music Festival", 
                        EventType = "Concert", 
                        EventDate = DateTime.Today.AddDays(14).AddHours(18), 
                        TotalSeats = 50000, 
                        BaseTicketPrice = 85.00m, 
                        Description = "Three days of amazing music",
                        IsActive = true
                    },
                    new Event 
                    { 
                        EventName = "NBA Finals Game 7", 
                        EventType = "Basketball", 
                        EventDate = DateTime.Today.AddDays(3).AddHours(19), 
                        TotalSeats = 20000, 
                        BaseTicketPrice = 200.00m, 
                        Description = "The decisive game of the championship",
                        IsActive = true
                    }
                };

                foreach (var evt in demoEvents)
                {
                    await ApiService.CreateEventAsync(evt);
                }
                
                ShowToast("Demo events created successfully! Generating demo data...", "success");
                await LoadEvents();
            }
            
            // Generate demo data for the first event or selected event
            if (selectedEvent != null)
            {
                await ApiService.GenerateDemoDataAsync(selectedEvent.Id);
                ShowToast($"Demo data generated for {selectedEvent.EventName}!", "success");
            }
            else if (events != null && events.Any())
            {
                await ApiService.GenerateDemoDataAsync(events.First().Id);
                ShowToast($"Demo data generated for {events.First().EventName}!", "success");
            }
            
            await LoadEvents();
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Failed to generate demo data");
            ShowToast($"Failed to generate demo data: {ex.Message}", "error");
        }
    }

    private async Task RefreshEvents()
    {
        await LoadEvents();
        ShowToast("Events refreshed!", "success");
    }

    private void ViewEventOrders(Event eventItem)
    {
        Navigation.NavigateTo($"/orders?eventId={eventItem.Id}");
    }

    private void ViewEventTickets(Event eventItem)
    {
        Navigation.NavigateTo($"/tickets?eventId={eventItem.Id}");
    }

    // Calendar methods
    private void PreviousMonth()
    {
        currentCalendarDate = currentCalendarDate.AddMonths(-1);
        StateHasChanged();
    }

    private void NextMonth()
    {
        currentCalendarDate = currentCalendarDate.AddMonths(1);
        StateHasChanged();
    }

    private List<CalendarDay> GetCalendarDays()
    {
        var days = new List<CalendarDay>();
        var firstDayOfMonth = new DateTime(currentCalendarDate.Year, currentCalendarDate.Month, 1);
        var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
        
        // Add days from previous month to fill the week
        var startDate = firstDayOfMonth.AddDays(-(int)firstDayOfMonth.DayOfWeek);
        
        // Add days until we have complete weeks
        var endDate = lastDayOfMonth;
        while ((endDate - startDate).Days % 7 != 6)
        {
            endDate = endDate.AddDays(1);
        }
        
        for (var date = startDate; date <= endDate; date = date.AddDays(1))
        {
            var eventsOnDate = events.Where(e => e.EventDate.Date == date.Date).ToList();
            days.Add(new CalendarDay
            {
                Date = date,
                IsCurrentMonth = date.Month == currentCalendarDate.Month,
                IsToday = date.Date == DateTime.Today,
                Events = eventsOnDate
            });
        }
        
        return days;
    }

    // Helper methods
    private string GetEventTypeIcon(string eventType)
    {
        return eventType.ToLower() switch
        {
            "football" => "⚽",
            "concert" => "🎵",
            "basketball" => "🏀",
            "boxing" => "🥊",
            "baseball" => "⚾",
            "hockey" => "🏒",
            _ => "🎪"
        };
    }

    private void ShowToast(string message, string type)
    {
        toastMessage = message;
        toastType = type;
        showToast = true;
        StateHasChanged();
        
        // Auto-hide after 3 seconds
        Task.Delay(3000).ContinueWith(_ =>
        {
            InvokeAsync(() =>
            {
                showToast = false;
                StateHasChanged();
            });
        });
    }

    private void HideToast()
    {
        showToast = false;
        StateHasChanged();
    }

    // Event form model
    public class EventForm
    {
        public string EventName { get; set; } = "";
        public string EventType { get; set; } = "";
        public DateTime EventDate { get; set; } = DateTime.Now.AddDays(7);
        public int TotalSeats { get; set; } = 50000;
        public decimal? BaseTicketPrice { get; set; }
        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
    }

    // Calendar day model
    public class CalendarDay
    {
        public DateTime Date { get; set; }
        public bool IsCurrentMonth { get; set; }
        public bool IsToday { get; set; }
        public List<Event> Events { get; set; } = new();
    }
}