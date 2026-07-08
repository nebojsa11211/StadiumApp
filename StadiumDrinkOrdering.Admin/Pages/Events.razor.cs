using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Pricing;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Events : ComponentBase
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;

    private List<EventDto>? events;
    private List<SeasonDto>? seasons;
    /// <summary>
    /// The configured venue. Every event is held in this one stadium, so its address is the shared
    /// event location (events no longer carry a free-typed location). Null while loading / if the
    /// venue endpoint is unreachable. Also the source of <see cref="venueClubs"/>.
    /// </summary>
    private VenueDto? venue;
    /// <summary>
    /// The venue's resident clubs, used to populate the Home-team dropdown for "Match" events. Null
    /// while loading; empty when none are configured (which blocks creating a Match — see SaveEvent).
    /// </summary>
    private List<ClubDto>? venueClubs;
    /// <summary>
    /// Real stadium capacity (sum of the drawing-tool overlay sectors), loaded once. Every event is
    /// held in the same physical stadium, so this is shown read-only as the event capacity instead
    /// of a free-typed number. 0 means no stadium has been drawn yet.
    /// </summary>
    private int realStadiumCapacity;
    private string seasonFilterValue = ""; // "" = all, "none" = no season, else season id
    private EventDto? editingEvent;
    private bool showEventModal = false;
    private bool isSaving = false;
    private string alertMessage = "";
    private string alertType = "";
    private bool loadingFailed = false;
    private string loadingError = "";

    private EventFormModel eventForm = new();

    /// <summary>
    /// Per-sector price rows shown in the modal's "Sector prices" editor. Null while loading. Each
    /// row's <see cref="EventSectorPriceDto.EventPrice"/> is bound to its input; null there means the
    /// sector uses its default price for this event.
    /// </summary>
    private List<EventSectorPriceDto>? sectorPrices;

    /// <summary>
    /// Event types offered directly in the dropdown. A value of "Other" (not listed here) reveals a
    /// free-text box so any other type can be entered. "Match" is the default/typical value.
    /// </summary>
    private static readonly string[] KnownEventTypes = { "Match", "Concert" };

    /// <summary>
    /// The type string to persist: the dropdown selection, unless "Other" is chosen, in which case
    /// the free-text value is used (falling back to the literal "Other" when left blank).
    /// </summary>
    private string ResolveEventType()
    {
        if (eventForm.EventType == "Other")
            return string.IsNullOrWhiteSpace(eventForm.EventTypeCustom) ? "Other" : eventForm.EventTypeCustom.Trim();
        return string.IsNullOrWhiteSpace(eventForm.EventType) ? "Match" : eventForm.EventType;
    }

    protected override async Task OnInitializedAsync()
    {
        await LoadEvents();
        await LoadSeasons();
        await LoadStadiumCapacity();
        await LoadVenue();
    }

    /// <summary>
    /// Loads the venue: its clubs feed the Match home-team picker, and its address is the shared
    /// location shown for every event. Never throws.
    /// </summary>
    private async Task LoadVenue()
    {
        try
        {
            venue = await ApiService.GetAsync<VenueDto>("venue");
            venueClubs = venue?.Clubs ?? new List<ClubDto>();
        }
        catch
        {
            venue = null;
            venueClubs = new List<ClubDto>();
        }
    }

    /// <summary>
    /// The shared event location: the venue name plus city from Venue Settings (e.g.
    /// "Stadion Rujevica · Rijeka"), falling back to just the name, or "—" if the venue is unavailable.
    /// </summary>
    private string VenueLocation
    {
        get
        {
            var name = venue?.Name;
            if (string.IsNullOrWhiteSpace(name)) return "—";
            return string.IsNullOrWhiteSpace(venue?.City) ? name : $"{name} · {venue!.City}";
        }
    }

    /// <summary>
    /// The location to display for an event: its own stored value (e.g. an externally-ingested
    /// event that carries one), otherwise the shared venue location.
    /// </summary>
    private string DisplayLocation(EventDto evt) =>
        string.IsNullOrWhiteSpace(evt.Location) ? VenueLocation : evt.Location!;

    /// <summary>The venue's primary club name, or the first club, used as the default home team.</summary>
    private string? DefaultHomeTeam =>
        venueClubs?.FirstOrDefault(c => c.IsPrimary)?.Name ?? venueClubs?.FirstOrDefault()?.Name;

    private async Task LoadStadiumCapacity()
    {
        try
        {
            realStadiumCapacity = await ApiService.GetAsync<int>("events/stadium-capacity");
        }
        catch
        {
            realStadiumCapacity = 0;
        }
    }

    private async Task LoadSeasons()
    {
        try
        {
            seasons = await ApiService.GetAsync<List<SeasonDto>>("seasons") ?? new List<SeasonDto>();
        }
        catch
        {
            seasons = new List<SeasonDto>();
        }
    }

    private IEnumerable<EventDto> FilteredEvents
    {
        get
        {
            if (events == null) return Enumerable.Empty<EventDto>();
            if (seasonFilterValue == "") return events;
            if (seasonFilterValue == "none") return events.Where(e => e.SeasonId == null);
            return int.TryParse(seasonFilterValue, out var id) ? events.Where(e => e.SeasonId == id) : events;
        }
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

    private async Task ShowCreateEventModal()
    {
        editingEvent = null;
        eventForm = new EventFormModel
        {
            EventType = "Match",
            // Default the home team to the venue's primary/first resident club (a Match is home-hosted).
            HomeTeam = DefaultHomeTeam,
            Date = DateTime.Now.AddDays(30),
            EndDate = DateTime.Now.AddDays(30).AddHours(2),
            IsActive = true,
            // Capacity is the real stadium seat count (read-only in the form); server is authoritative.
            Capacity = realStadiumCapacity,
            BasePrice = 50,
            SeasonId = seasons?.FirstOrDefault(s => s.IsCurrent)?.Id
        };
        sectorPrices = null;
        showEventModal = true;
        await LoadSectorPrices(null);
    }

    private async Task ShowEditEventModal(EventDto evt)
    {
        // Past/terminal events are frozen — the button is disabled, but guard the entry point too.
        if (!EventLifecycle.CanEdit(evt.Status))
        {
            ShowAlert(EventLifecycle.EditBlockedReason(evt.Status), "danger");
            return;
        }

        editingEvent = evt;
        var type = string.IsNullOrWhiteSpace(evt.EventType) ? "Match" : evt.EventType;
        var isKnownType = KnownEventTypes.Contains(type);
        eventForm = new EventFormModel
        {
            Name = evt.Name,
            // Known types map to the dropdown; anything else falls under "Other" with the value in the custom box.
            EventType = isKnownType ? type : "Other",
            EventTypeCustom = isKnownType ? "" : type,
            HomeTeam = evt.HomeTeam,
            AwayTeam = evt.AwayTeam,
            Description = evt.Description,
            Date = evt.Date ?? DateTime.Now,
            EndDate = evt.EndDate ?? (evt.Date ?? DateTime.Now).AddHours(2),
            // Read-only in the form; prefer the live stadium capacity, falling back to the stored value.
            Capacity = realStadiumCapacity > 0 ? realStadiumCapacity : evt.Capacity,
            BasePrice = evt.BasePrice,
            IsActive = evt.IsActive,
            SeasonId = evt.SeasonId
        };
        sectorPrices = null;
        showEventModal = true;
        await LoadSectorPrices(evt.Id);
    }

    /// <summary>
    /// Loads the sector-price rows (defaults + any per-event overrides) for the modal. Pass the
    /// event id when editing; null for a new event (defaults only).
    /// </summary>
    private async Task LoadSectorPrices(int? eventId)
    {
        try
        {
            var url = eventId.HasValue ? $"events/sector-prices?eventId={eventId.Value}" : "events/sector-prices";
            sectorPrices = await ApiService.GetAsync<List<EventSectorPriceDto>>(url) ?? new List<EventSectorPriceDto>();
        }
        catch
        {
            sectorPrices = new List<EventSectorPriceDto>();
        }
    }

    /// <summary>
    /// The default price shown for a sector row: the sector's own explicit price, or the current
    /// form base price × the sector-type multiplier. Recomputes live as the admin edits base price.
    /// </summary>
    private decimal EffectiveDefault(EventSectorPriceDto row)
        => SectorPricing.Default(row.SectorDefaultPrice, eventForm.BasePrice, row.Type);

    /// <summary>Maps the editor rows to the price-override payload sent with a save.</summary>
    private List<EventSectorPriceInputDto>? BuildSectorPriceInputs()
        => sectorPrices?
            .Select(r => new EventSectorPriceInputDto { SectorOverlayId = r.SectorOverlayId, Price = r.EventPrice })
            .ToList();

    private void HideEventModal()
    {
        showEventModal = false;
        editingEvent = null;
        eventForm = new();
        sectorPrices = null;
    }

    private async Task SaveEvent()
    {
        if (string.IsNullOrWhiteSpace(eventForm.Name) ||
            eventForm.BasePrice <= 0)
        {
            ShowAlert("Please fill in all required fields", "danger");
            return;
        }

        // Capacity is the real stadium seat count (read-only); it is 0 only when no stadium exists yet.
        if (eventForm.Capacity <= 0)
        {
            ShowAlert("The stadium has no seats yet. Draw the stadium layout before creating events.", "danger");
            return;
        }

        if (eventForm.EndDate <= eventForm.Date)
        {
            ShowAlert("The event end time must be after the start time", "danger");
            return;
        }

        // A Match must name both sides; its home team is one of the venue's resident clubs.
        if (eventForm.EventType == "Match")
        {
            if (venueClubs is null || venueClubs.Count == 0)
            {
                ShowAlert("Add a resident club in Venue Settings before creating a Match event.", "danger");
                return;
            }
            if (string.IsNullOrWhiteSpace(eventForm.HomeTeam) || string.IsNullOrWhiteSpace(eventForm.AwayTeam))
            {
                ShowAlert("A Match event requires both a home team and an away team.", "danger");
                return;
            }
        }

        // Teams only belong to a Match; drop them for other types.
        var homeTeam = eventForm.EventType == "Match" ? eventForm.HomeTeam?.Trim() : null;
        var awayTeam = eventForm.EventType == "Match" ? eventForm.AwayTeam?.Trim() : null;

        isSaving = true;
        try
        {
            if (editingEvent == null)
            {
                // Create new event
                var createDto = new CreateEventDto
                {
                    Name = eventForm.Name.Trim(),
                    EventType = ResolveEventType(),
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    Description = string.IsNullOrWhiteSpace(eventForm.Description) ? null : eventForm.Description.Trim(),
                    Date = eventForm.Date,
                    EndDate = eventForm.EndDate,
                    // Location is intentionally not set: every event is at the one venue, so the UI
                    // derives it from Venue Settings rather than storing a per-event copy.
                    Capacity = eventForm.Capacity,
                    BasePrice = eventForm.BasePrice,
                    IsActive = eventForm.IsActive,
                    SeasonId = eventForm.SeasonId,
                    SectorPrices = BuildSectorPriceInputs()
                };

                var response = await ApiService.Http.PostAsync("events", createDto);
                if (response.IsSuccessStatusCode)
                {
                    await LoadEvents();
                    await LoadSeasons();
                    HideEventModal();
                    ShowAlert($"Event '{createDto.Name}' created successfully", "success");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    ShowAlert(ExtractErrorMessage(body) ?? $"Failed to create event ({(int)response.StatusCode})", "danger");
                }
            }
            else
            {
                // Update existing event
                var updateDto = new UpdateEventDto
                {
                    Name = eventForm.Name.Trim(),
                    EventType = ResolveEventType(),
                    HomeTeam = homeTeam,
                    AwayTeam = awayTeam,
                    Description = string.IsNullOrWhiteSpace(eventForm.Description) ? null : eventForm.Description.Trim(),
                    Date = eventForm.Date,
                    EndDate = eventForm.EndDate,
                    // Location left null on purpose: the API preserves any existing value (e.g. an
                    // externally-ingested location) and the UI otherwise shows the venue location.
                    Capacity = eventForm.Capacity,
                    BasePrice = eventForm.BasePrice,
                    IsActive = eventForm.IsActive,
                    SeasonId = eventForm.SeasonId,
                    SectorPrices = BuildSectorPriceInputs()
                };

                var response = await ApiService.Http.PostAsync($"events/{editingEvent.Id}", updateDto);
                if (response.IsSuccessStatusCode)
                {
                    await LoadEvents();
                    await LoadSeasons();
                    HideEventModal();
                    ShowAlert($"Event '{updateDto.Name}' updated successfully", "success");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    ShowAlert(ExtractErrorMessage(body) ?? $"Failed to update event ({(int)response.StatusCode})", "danger");
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
        /// <summary>Dropdown selection: "Match", "Concert", or "Other" (which reveals <see cref="EventTypeCustom"/>).</summary>
        public string EventType { get; set; } = "Match";
        /// <summary>Free-text type used only when <see cref="EventType"/> is "Other".</summary>
        public string EventTypeCustom { get; set; } = "";
        /// <summary>Home team for a "Match" (a resident club's name). Ignored for other types.</summary>
        public string? HomeTeam { get; set; }
        /// <summary>Away/visiting team for a "Match" (free text). Ignored for other types.</summary>
        public string? AwayTeam { get; set; }
        public string? Description { get; set; }
        public DateTime Date { get; set; } = DateTime.Now.AddDays(30);
        public DateTime EndDate { get; set; } = DateTime.Now.AddDays(30).AddHours(2);
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; } = true;
        public int? SeasonId { get; set; }
    }
}