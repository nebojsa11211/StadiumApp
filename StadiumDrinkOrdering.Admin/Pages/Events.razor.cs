using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.Pricing;
using StadiumDrinkOrdering.Shared.Services;
using StadiumDrinkOrdering.Admin.Services;

namespace StadiumDrinkOrdering.Admin.Pages;

public partial class Events : ComponentBase, IDisposable
{
    [Inject] private IAdminApiService ApiService { get; set; } = default!;
    [Inject] private SeasonStateService SeasonState { get; set; } = default!;
    [Inject] private IJSRuntime JSRuntime { get; set; } = default!;
    [Inject] private IImageGenerationClient ImageGen { get; set; } = default!;
    [Inject] private IConfiguration Configuration { get; set; } = default!;

    // ---- Event poster generation -------------------------------------------------------------

    /// <summary>Base64 of a poster generated in this modal session; null until one is generated.</summary>
    private string? posterBase64;
    /// <summary>Downscaled JPEG of <see cref="posterBase64"/>, saved for the customer fixture strip.</summary>
    private string? posterThumbnailBase64;
    /// <summary>Longest edge of the stored thumbnail — enough for a retina card background.</summary>
    private const int ThumbnailMaxDimension = 800;

    /// <summary>The exact prompt sent for the current poster, stored with it for later reference.</summary>
    private string? posterPromptUsed;
    /// <summary>Event facts baked into the current artwork, used to detect staleness later.</summary>
    private string? posterSignature;
    /// <summary>
    /// Whether the admin has ticked "the text is correct" for the poster currently in the modal.
    /// Until this is true the poster is stored pending review and fans keep seeing the plain card.
    /// </summary>
    private bool posterApproved;
    /// <summary>True when the stored poster is awaiting review (typically after auto-regeneration).</summary>
    private bool existingPosterPending;
    /// <summary>True when the stored poster's baked-in details no longer match the event.</summary>
    private bool existingPosterStale;

    /// <summary>Home crest (resident club logo), rasterized to PNG for the image API.</summary>
    private string? homeCrestBase64;
    /// <summary>Away crest, either remembered from a previous fixture or uploaded now.</summary>
    private string? awayCrestBase64;
    /// <summary>True when the away crest came from the remembered-crest store rather than an upload.</summary>
    private bool awayCrestIsRemembered;
    /// <summary>Square size crests are rasterized to before being sent as reference images.</summary>
    private const int CrestRasterSize = 512;
    private const long MaxCrestUploadBytes = 2 * 1024 * 1024;
    /// <summary>Set while an edit triggers an automatic regeneration, so the UI can explain the delay.</summary>
    private bool isRegeneratingPoster;
    private int? posterWidth;
    private int? posterHeight;
    private bool isGeneratingPoster;
    private string? posterError;
    /// <summary>True when editing an event that already has a stored poster we haven't replaced.</summary>
    private bool hasExistingPoster;
    /// <summary>Set when the admin removed an existing poster, so the update clears it server-side.</summary>
    private bool removeExistingPoster;

    /// <summary>Whether there is anything to show in the preview pane — new or already stored.</summary>
    private bool HasPosterPreview => posterBase64 != null || (hasExistingPoster && !removeExistingPoster);

    /// <summary>
    /// A poster needs at least the home crest; a versus fixture needs both, since the prompt places
    /// one crest on each side.
    /// </summary>
    private bool CanGeneratePoster =>
        homeCrestBase64 != null
        && (eventForm.EventType != "Match" || awayCrestBase64 != null);

    /// <summary>
    /// Whether the poster in front of the admin still needs its text checked — either freshly
    /// generated in this session, or stored earlier by an auto-regeneration and never reviewed.
    /// </summary>
    private bool NeedsPosterApproval =>
        HasPosterPreview && !posterApproved && (posterBase64 != null || existingPosterPending);

    /// <summary>
    /// A freshly generated poster renders straight from its base64; an unchanged existing one is
    /// fetched from the API's public image endpoint (cache-busted per modal open is unnecessary —
    /// the URL changes whenever a new poster replaces it).
    /// </summary>
    /// <summary>API root, used to build public image URLs the browser fetches directly.</summary>
    private string ApiBaseUrl => Configuration["ApiSettings:BaseUrl"]?.TrimEnd('/') ?? string.Empty;

    /// <summary>
    /// Small variant of an event's poster, used as the card background in the events grid. The grid
    /// can show many events at once, so it must never pull the multi-MB originals.
    /// </summary>
    private string PosterThumbUrl(int eventId) => $"{ApiBaseUrl}/events/{eventId}/image?variant=thumb";

    private string PosterPreviewSrc =>
        posterBase64 != null
            ? $"data:image/png;base64,{posterBase64}"
            : $"{ApiBaseUrl}/events/{editingEvent?.Id}/image";

    private List<EventDto>? events;
    private List<SeasonDto>? seasons;
    /// <summary>
    /// The configured venue. Every event is held in this one stadium. Null while loading / if the
    /// venue endpoint is unreachable. The source of <see cref="venueClubs"/>.
    /// </summary>
    private VenueDto? venue;
    /// <summary>
    /// The venue's resident clubs, used to populate the Home-team dropdown for "Match" events. Null
    /// while loading; empty when none are configured (which blocks creating a Match — see SaveEvent).
    /// </summary>
    private List<ClubDto>? venueClubs;

    /// <summary>
    /// The visiting-team directory (managed at /admin/teams), offered as suggestions on the
    /// away-team field so a known opponent brings its stored crest instead of being re-uploaded.
    /// Empty until loaded; the field stays free text either way.
    /// </summary>
    private List<TeamDto> knownTeams = new();

    /// <summary>
    /// Real stadium capacity (sum of the drawing-tool overlay sectors), loaded once. Every event is
    /// held in the same physical stadium, so this is shown read-only as the event capacity instead
    /// of a free-typed number. 0 means no stadium has been drawn yet.
    /// </summary>
    private int realStadiumCapacity;
    // The season scope comes from the shell banner in DashboardLayout, so it is shared with every
    // other admin page. "" = all, "none" = no season, else season id.
    private string seasonFilterValue => SeasonState.SelectedValue;
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
    /// Staff (Bartenders/Waiters) shown in the modal's "Staff" picker, each with an
    /// <see cref="EventStaffMemberDto.IsAssigned"/> flag bound to its checkbox. Null while loading.
    /// </summary>
    private List<EventStaffMemberDto>? eventStaff;

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
        SeasonState.OnChanged += OnSeasonScopeChanged;

        await LoadEvents();
        await LoadSeasons();
        await LoadStadiumCapacity();
        await LoadVenue();
        await LoadTeams();
    }

    /// <summary>The shell banner picked another season: re-filter the already-loaded grid.</summary>
    private void OnSeasonScopeChanged() => InvokeAsync(StateHasChanged);

    public void Dispose() => SeasonState.OnChanged -= OnSeasonScopeChanged;

    /// <summary>
    /// Loads the visiting-team directory that suggests opponents on the away-team field. Never
    /// throws: the field accepts free text regardless, so a failure here costs suggestions, not the
    /// ability to build a fixture.
    /// </summary>
    private async Task LoadTeams()
    {
        try
        {
            knownTeams = await ApiService.GetAsync<List<TeamDto>>("api/teams") ?? new List<TeamDto>();
        }
        catch
        {
            knownTeams = new List<TeamDto>();
        }
    }

    /// <summary>Loads the venue; its clubs feed the Match home-team picker. Never throws.</summary>
    private async Task LoadVenue()
    {
        try
        {
            venue = await ApiService.GetAsync<VenueDto>("api/venue");
            venueClubs = venue?.Clubs ?? new List<ClubDto>();
        }
        catch
        {
            venue = null;
            venueClubs = new List<ClubDto>();
        }
    }

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

    /// <summary>
    /// Refreshes the season list through the shared shell state, so the banner and this page's
    /// form dropdown never disagree (this is also called after a save, hence the reload).
    /// </summary>
    private async Task LoadSeasons()
    {
        await SeasonState.ReloadAsync();
        seasons = SeasonState.Seasons;
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
            // Sales open immediately (blank start) and close by default when the event starts.
            TicketSalesStartDate = null,
            TicketSalesEndDate = DateTime.Now.AddDays(30),
            IsActive = true,
            // Capacity is the real stadium seat count (read-only in the form); server is authoritative.
            Capacity = realStadiumCapacity,
            BasePrice = 50,
            SeasonId = seasons?.FirstOrDefault(s => s.IsCurrent)?.Id
        };
        sectorPrices = null;
        eventStaff = null;
        ResetPosterState();
        showEventModal = true;
        await LoadSectorPrices(null);
        await LoadEventStaff(null);
        // The home side defaults to the resident club, so its crest can be fetched right away.
        await LoadHomeCrestAsync();
    }

    /// <summary>
    /// Converts a UTC timestamp from the API into local wall-clock for the form's datetime-local
    /// inputs. Values already tagged Local (or Unspecified, which the JSON layer may produce) are
    /// left alone so a value never gets shifted twice.
    /// </summary>
    private static DateTime? ToLocal(DateTime? utc) =>
        utc?.Kind == DateTimeKind.Utc ? utc.Value.ToLocalTime() : utc;

    private async Task ShowEditEventModal(EventDto evt)
    {
        // Past/terminal events are frozen — the button is disabled, but guard the entry point too.
        if (!EventLifecycle.CanEdit(evt.Status))
        {
            ShowAlert(EditBlockedMessage(evt.Status), "danger");
            return;
        }

        // Events in an ended season are frozen too — same defence-in-depth as above.
        if (IsSeasonClosed(evt.SeasonId))
        {
            ShowAlert(SeasonClosedMessage(evt.SeasonId), "danger");
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
            // The API stores and returns UTC; the form's datetime-local inputs are wall-clock, and
            // the save path converts back with ToUtc. Without this the admin edits (and the poster
            // prints) UTC — a 20:00 kick-off would show, and be baked into the artwork, as 18:00.
            Date = ToLocal(evt.Date) ?? DateTime.Now,
            EndDate = ToLocal(evt.EndDate) ?? (ToLocal(evt.Date) ?? DateTime.Now).AddHours(2),
            TicketSalesStartDate = ToLocal(evt.TicketSalesStartDate),
            TicketSalesEndDate = ToLocal(evt.TicketSalesEndDate),
            DrinkSalesStartDate = ToLocal(evt.DrinkSalesStartDate),
            DrinkSalesEndDate = ToLocal(evt.DrinkSalesEndDate),
            // Read-only in the form; prefer the live stadium capacity, falling back to the stored value.
            Capacity = realStadiumCapacity > 0 ? realStadiumCapacity : evt.Capacity,
            BasePrice = evt.BasePrice,
            IsActive = evt.IsActive,
            SeasonId = evt.SeasonId
        };
        sectorPrices = null;
        eventStaff = null;
        ResetPosterState();
        hasExistingPoster = evt.HasPoster;
        existingPosterPending = evt.HasPoster && !evt.PosterApproved;
        existingPosterStale = evt.PosterIsStale;
        posterWidth = evt.PosterWidth;
        posterHeight = evt.PosterHeight;
        showEventModal = true;
        await LoadSectorPrices(evt.Id);
        await LoadEventStaff(evt.Id);
        await LoadHomeCrestAsync();
        await LoadRememberedAwayCrestAsync();
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

    /// <summary>Maps the editor rows to the per-sector config payload (price override + disabled) sent with a save.</summary>
    private List<EventSectorPriceInputDto>? BuildSectorPriceInputs()
        => sectorPrices?
            .Select(r => new EventSectorPriceInputDto
            {
                SectorOverlayId = r.SectorOverlayId,
                // A disabled sector is closed regardless of price, so don't also persist a stale override for it.
                Price = r.IsDisabled ? null : r.EventPrice,
                IsDisabled = r.IsDisabled
            })
            .ToList();

    /// <summary>
    /// Loads the staff-assignment rows (all Bartenders/Waiters + who is assigned) for the modal. Pass
    /// the event id when editing; null for a new event (nobody assigned yet).
    /// </summary>
    private async Task LoadEventStaff(int? eventId)
    {
        try
        {
            var url = eventId.HasValue ? $"events/staff?eventId={eventId.Value}" : "events/staff";
            eventStaff = await ApiService.GetAsync<List<EventStaffMemberDto>>(url) ?? new List<EventStaffMemberDto>();
        }
        catch
        {
            eventStaff = new List<EventStaffMemberDto>();
        }
    }

    /// <summary>The assignments (member + function + covered sectors) currently ticked, sent with a save.</summary>
    private List<EventStaffInputDto>? BuildStaffInputs()
        => eventStaff?
            .Where(s => s.IsAssigned)
            .Select(s => new EventStaffInputDto
            {
                StaffId = s.StaffId,
                EventRole = s.EventRole,
                // A barman works the bar, not sectors — don't persist stale sector picks for them.
                SectorOverlayIds = s.EventRole == EventStaffRoles.Bartender ? new List<int>() : s.SectorOverlayIds
            })
            .ToList();

    /// <summary>Count of staff currently ticked, shown in the section header.</summary>
    private int AssignedStaffCount => eventStaff?.Count(s => s.IsAssigned) ?? 0;

    /// <summary>Toggles an overlay sector in a staff member's covered-sectors set (used by the chips).</summary>
    private static void ToggleStaffSector(EventStaffMemberDto member, int sectorOverlayId)
    {
        if (!member.SectorOverlayIds.Remove(sectorOverlayId))
            member.SectorOverlayIds.Add(sectorOverlayId);
    }

    private void HideEventModal()
    {
        showEventModal = false;
        editingEvent = null;
        eventForm = new();
        sectorPrices = null;
        eventStaff = null;
        ResetPosterState();
    }

    private void ResetPosterState()
    {
        posterBase64 = null;
        posterThumbnailBase64 = null;
        posterWidth = null;
        posterHeight = null;
        posterError = null;
        posterPromptUsed = null;
        posterSignature = null;
        posterApproved = false;
        isGeneratingPoster = false;
        isRegeneratingPoster = false;
        hasExistingPoster = false;
        existingPosterPending = false;
        existingPosterStale = false;
        removeExistingPoster = false;
        homeCrestBase64 = null;
        awayCrestBase64 = null;
        awayCrestIsRemembered = false;
    }

    /// <summary>
    /// The poster prompt. Fixed wording, not admin-authored: the admin supplies the crests and the
    /// event supplies the facts, and this template turns them into a match poster. Keeping it in
    /// code means every poster shares one house style, and the wording that reliably produces a
    /// clean layout can be tuned in one place instead of being retyped per event.
    ///
    /// The instructions lean hard on exact spelling because the model composes the team names,
    /// venue and kick-off into the artwork itself and does sometimes get them wrong — which is why
    /// nothing generated here reaches fans until an admin has approved it.
    /// </summary>
    private string BuildPosterPrompt()
    {
        var venueName = string.IsNullOrWhiteSpace(venue?.Name) ? "the stadium" : venue!.Name;
        // The poster is Croatian-facing but the instructions are English; format the date
        // invariantly so the model is never handed a half-translated string.
        var kickOff = eventForm.Date.ToString("d MMMM yyyy", CultureInfo.InvariantCulture).ToUpperInvariant();
        var time = eventForm.Date.ToString("HH:mm", CultureInfo.InvariantCulture);

        var home = (eventForm.HomeTeam ?? string.Empty).Trim().ToUpperInvariant();
        var away = (eventForm.AwayTeam ?? string.Empty).Trim().ToUpperInvariant();
        var isVersus = home.Length > 0 && away.Length > 0;

        var subject = isVersus
            ? $"  Team names: '{home}' (left) and '{away}' (right)\n"
            : $"  Title: '{(eventForm.Name ?? string.Empty).Trim().ToUpperInvariant()}'\n";

        var crestInstruction = isVersus
            ? "Use the SUPPLIED CREST IMAGES exactly as given - do not redraw, restyle or invent crests. " +
              "Place the first crest on the left and the second crest on the right, facing each other, " +
              "large and clearly visible.\nBetween them render the text 'VS' in a bold modern sports typeface.\n"
            : "Use the SUPPLIED CREST IMAGE exactly as given - do not redraw, restyle or invent it. " +
              "Place it prominently in the centre.\n";

        return
            "Create a professional football match poster, 16:9 landscape.\n" +
            crestInstruction +
            "Render this text accurately and legibly, spelled EXACTLY as written, " +
            "preserving every accent and diacritic character:\n" +
            subject +
            $"  Venue: '{venueName.ToUpperInvariant()}'\n" +
            $"  Date and kick-off: '{kickOff} - {time}'\n" +
            "Background: a floodlit football stadium at dusk packed with fans, cinematic lighting, " +
            "dramatic atmosphere, high detail. Dark cinematic colour grade so white text stays readable. " +
            "Professional sports-marketing layout with clear visual hierarchy. " +
            "No spelling mistakes. No extra text beyond what is listed above.";
    }

    /// <summary>
    /// Generates the poster from the hardcoded template plus the crests. Requires a home crest
    /// (from the resident club) and, for a versus fixture, an away crest supplied by the admin.
    /// </summary>
    private async Task GeneratePosterAsync()
    {
        isGeneratingPoster = true;
        posterError = null;
        StateHasChanged();

        try
        {
            var crests = new List<string>();
            if (homeCrestBase64 != null) crests.Add(homeCrestBase64);
            if (awayCrestBase64 != null) crests.Add(awayCrestBase64);

            if (crests.Count == 0)
            {
                posterError = L["Events_PosterErrorNoCrests"];
                return;
            }

            var prompt = BuildPosterPrompt();
            var result = await ImageGen.GenerateAsync(new ImageGenerationRequest
            {
                Prompt = prompt,
                Width = 1920,
                Height = 1080,
                ReferenceImages = crests
            });

            if (result == null)
            {
                posterError = L["Events_PosterErrorUnreachable"];
                return;
            }

            // The API answers 200 with a 1x1 placeholder when generation failed upstream, so a
            // successful HTTP call is not on its own a successful image.
            if (ImageGen.IsPlaceholder(result))
            {
                posterError = L["Events_PosterErrorPlaceholder"];
                return;
            }

            posterBase64 = result.Base64Image;
            posterPromptUsed = prompt;
            // result.Width/Height only echo what we asked for; show what actually came back.
            (posterWidth, posterHeight) = ImageGen.GetActualDimensions(result);

            // Build the small variant the customer fixture strip will use. Failure is tolerable —
            // the image endpoint falls back to the full poster — so this never blocks the save.
            posterThumbnailBase64 = await BuildThumbnailAsync(posterBase64);

            // Fresh artwork always starts unapproved: its text has not been checked yet.
            posterApproved = false;
            posterSignature = PosterSignature.For(eventForm.HomeTeam, eventForm.AwayTeam, eventForm.Date);
            // A newly generated poster supersedes whatever was stored before.
            removeExistingPoster = false;
        }
        catch (Exception ex)
        {
            posterError = $"{L["Events_PosterErrorUnreachable"]} ({ex.Message})";
        }
        finally
        {
            isGeneratingPoster = false;
        }
    }

    // ---- Crests ------------------------------------------------------------------------------

    /// <summary>
    /// Loads the home crest from the resident club matching the selected home team. Club logos are
    /// stored as SVG, which the image API cannot take as a reference, so it is rasterized to PNG in
    /// the browser first.
    /// </summary>
    private async Task LoadHomeCrestAsync()
    {
        homeCrestBase64 = null;
        var teamName = eventForm.HomeTeam?.Trim();
        if (string.IsNullOrWhiteSpace(teamName))
            return;

        var club = venueClubs?.FirstOrDefault(c =>
            string.Equals(c.Name, teamName, StringComparison.OrdinalIgnoreCase) && c.HasLogo);
        if (club == null)
            return;

        try
        {
            // Fetch the bytes here rather than letting the browser load the API URL directly: the
            // API is a different origin to this app, and drawing a cross-origin image onto a canvas
            // taints it, so toDataURL would throw SecurityError. A data: URL is same-origin and
            // rasterizes cleanly.
            var response = await ApiService.Http.GetAsync($"api/venue/clubs/{club.Id}/logo");
            if (!response.IsSuccessStatusCode)
                return;

            var bytes = await response.Content.ReadAsByteArrayAsync();
            if (bytes.Length == 0)
                return;

            var contentType = response.Content.Headers.ContentType?.MediaType ?? "image/png";
            var dataUrl = $"data:{contentType};base64,{Convert.ToBase64String(bytes)}";

            homeCrestBase64 = await JSRuntime.InvokeAsync<string?>(
                "stadiumImageTools.rasterizeToPngBase64", dataUrl, CrestRasterSize);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Home crest load failed: {ex.Message}");
        }
    }

    /// <summary>
    /// Applies a new away-team name and pulls that opponent's stored crest straight away.
    ///
    /// Without this the lookup ran only when reopening a saved event, so building a <em>new</em>
    /// fixture against a team already in the directory still demanded the crest be uploaded again —
    /// the whole point of keeping the directory. Any crest already on the form is dropped first:
    /// crests belong to the team named in this field, so a different name must not keep the old badge.
    /// </summary>
    private async Task OnAwayTeamChangedAsync(ChangeEventArgs e)
    {
        eventForm.AwayTeam = e.Value?.ToString();
        await LoadRememberedAwayCrestAsync();
    }

    /// <summary>
    /// Looks up a crest previously stored for this away team, so a returning opponent does not have
    /// to be uploaded again. Silently does nothing when none is remembered.
    /// </summary>
    private async Task LoadRememberedAwayCrestAsync()
    {
        awayCrestBase64 = null;
        awayCrestIsRemembered = false;
        var teamName = eventForm.AwayTeam?.Trim();
        if (string.IsNullOrWhiteSpace(teamName))
            return;

        try
        {
            var crest = await ApiService.GetAsync<TeamCrestDto>(
                $"events/team-crest?teamName={Uri.EscapeDataString(teamName)}");
            if (crest != null && !string.IsNullOrEmpty(crest.ImageBase64))
            {
                awayCrestBase64 = crest.ImageBase64;
                awayCrestIsRemembered = true;
            }
        }
        catch
        {
            // No crest remembered for this opponent yet — the admin uploads one.
        }
    }

    /// <summary>
    /// Accepts an uploaded away crest, rasterizing whatever the admin picked (PNG/JPEG/SVG) into
    /// the PNG the image API requires.
    /// </summary>
    private async Task OnAwayCrestSelected(InputFileChangeEventArgs e)
    {
        var file = e.File;
        if (file == null)
            return;

        posterError = null;

        if (file.Size > MaxCrestUploadBytes)
        {
            posterError = L["Events_CrestTooLarge"];
            return;
        }

        try
        {
            await using var stream = file.OpenReadStream(MaxCrestUploadBytes);
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var dataUrl = $"data:{file.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";

            awayCrestBase64 = await JSRuntime.InvokeAsync<string?>(
                "stadiumImageTools.rasterizeToPngBase64", dataUrl, CrestRasterSize);
            awayCrestIsRemembered = false;

            if (awayCrestBase64 == null)
                posterError = L["Events_CrestUnreadable"];
        }
        catch (Exception ex)
        {
            posterError = $"{L["Events_CrestUnreadable"]} ({ex.Message})";
        }
    }

    /// <summary>
    /// Remembers the away crest under the opponent's name so the next fixture against them
    /// pre-fills it. Best-effort: a failure here must not fail the event save.
    /// </summary>
    private async Task RememberAwayCrestAsync()
    {
        var teamName = eventForm.AwayTeam?.Trim();
        if (string.IsNullOrWhiteSpace(teamName) || awayCrestBase64 == null || awayCrestIsRemembered)
            return;

        try
        {
            await ApiService.Http.PostAsync("events/team-crest", new SaveTeamCrestDto
            {
                TeamName = teamName,
                ImageBase64 = awayCrestBase64
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Could not remember away crest: {ex.Message}");
        }
    }

    /// <summary>
    /// Whether the facts baked into the stored poster differ from what the form now holds. Compares
    /// against the signature the API reported for the loaded event, so an edit that leaves teams and
    /// kick-off untouched (a price change, say) never triggers a costly regeneration.
    /// </summary>
    private bool PosterDetailsChanged()
    {
        if (editingEvent == null)
            return false;

        var current = PosterSignature.For(eventForm.HomeTeam, eventForm.AwayTeam, eventForm.Date);
        var original = PosterSignature.For(editingEvent.HomeTeam, editingEvent.AwayTeam,
            editingEvent.Date ?? eventForm.Date);
        return current != original;
    }

    /// <summary>
    /// Downscales the generated poster in the browser (canvas → JPEG) so the customer fixture strip
    /// has a small variant to serve. Returns null if the browser could not produce one, in which
    /// case the poster is saved without a thumbnail and the image endpoint falls back to full size.
    /// </summary>
    private async Task<string?> BuildThumbnailAsync(string base64Png)
    {
        try
        {
            return await JSRuntime.InvokeAsync<string?>(
                "stadiumImageTools.downscaleToBase64", base64Png, ThumbnailMaxDimension, 0.82);
        }
        catch (Exception ex)
        {
            // Not worth surfacing to the admin — the poster itself generated fine.
            Console.WriteLine($"Poster thumbnail generation failed: {ex.Message}");
            return null;
        }
    }

    private void RemovePoster()
    {
        posterBase64 = null;
        posterThumbnailBase64 = null;
        posterWidth = null;
        posterHeight = null;
        posterError = null;
        // Only meaningful when editing: tells the update request to clear the stored poster.
        if (hasExistingPoster)
            removeExistingPoster = true;
    }

    private async Task SaveEvent()
    {
        if (string.IsNullOrWhiteSpace(eventForm.Name) ||
            eventForm.BasePrice <= 0)
        {
            ShowAlert(L["Events_ValidationRequiredFields"], "danger");
            return;
        }

        // Capacity is the real stadium seat count (read-only); it is 0 only when no stadium exists yet.
        if (eventForm.Capacity <= 0)
        {
            ShowAlert(L["Events_ValidationNoSeats"], "danger");
            return;
        }

        if (eventForm.EndDate <= eventForm.Date)
        {
            ShowAlert(L["Events_EndAfterStartValidation"], "danger");
            return;
        }

        // When both sales bounds are set, the end must come after the start.
        if (eventForm.TicketSalesStartDate.HasValue && eventForm.TicketSalesEndDate.HasValue &&
            eventForm.TicketSalesEndDate.Value <= eventForm.TicketSalesStartDate.Value)
        {
            ShowAlert(L["Events_SalesEndAfterStartValidation"], "danger");
            return;
        }

        // When both drink-ordering bounds are set, the end must come after the start.
        if (eventForm.DrinkSalesStartDate.HasValue && eventForm.DrinkSalesEndDate.HasValue &&
            eventForm.DrinkSalesEndDate.Value <= eventForm.DrinkSalesStartDate.Value)
        {
            ShowAlert(L["Events_DrinkSalesEndAfterStartValidation"], "danger");
            return;
        }

        // A Match must name both sides; its home team is one of the venue's resident clubs.
        if (eventForm.EventType == "Match")
        {
            if (venueClubs is null || venueClubs.Count == 0)
            {
                ShowAlert(L["Events_ValidationNeedResidentClub"], "danger");
                return;
            }
            if (string.IsNullOrWhiteSpace(eventForm.HomeTeam) || string.IsNullOrWhiteSpace(eventForm.AwayTeam))
            {
                ShowAlert(L["Events_ValidationMatchNeedsTeams"], "danger");
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
                    TicketSalesStartDate = eventForm.TicketSalesStartDate,
                    TicketSalesEndDate = eventForm.TicketSalesEndDate,
                    DrinkSalesStartDate = eventForm.DrinkSalesStartDate,
                    DrinkSalesEndDate = eventForm.DrinkSalesEndDate,
                    Capacity = eventForm.Capacity,
                    BasePrice = eventForm.BasePrice,
                    IsActive = eventForm.IsActive,
                    SeasonId = eventForm.SeasonId,
                    SectorPrices = BuildSectorPriceInputs(),
                    Staff = BuildStaffInputs(),
                    // Null when nothing was generated — the event is simply created without a poster.
                    PosterImageBase64 = posterBase64,
                    PosterContentType = posterBase64 != null ? "image/png" : null,
                    PosterPrompt = posterBase64 != null ? posterPromptUsed : null,
                    PosterThumbnailBase64 = posterThumbnailBase64,
                    PosterApproved = posterBase64 != null && posterApproved,
                    PosterSourceSignature = posterBase64 != null ? posterSignature : null
                };

                var response = await ApiService.Http.PostAsync("events", createDto);
                if (response.IsSuccessStatusCode)
                {
                    await RememberAwayCrestAsync();
                    await LoadEvents();
                    await LoadSeasons();
                    HideEventModal();
                    ShowAlert(L["Events_CreatedAlert", createDto.Name], "success");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    ShowAlert(ExtractErrorMessage(body) ?? L["Events_CreateFailedAlert", (int)response.StatusCode], "danger");
                }
            }
            else
            {
                // The poster prints the teams and kick-off, so editing either leaves the stored
                // artwork showing details the event no longer has. Regenerate before saving; the
                // new image lands pending review, so fans fall back to the plain card rather than
                // seeing text nobody has checked.
                if (hasExistingPoster && posterBase64 == null && !removeExistingPoster && PosterDetailsChanged())
                {
                    isRegeneratingPoster = true;
                    StateHasChanged();
                    try
                    {
                        await GeneratePosterAsync();
                    }
                    finally
                    {
                        isRegeneratingPoster = false;
                    }
                }

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
                    TicketSalesStartDate = eventForm.TicketSalesStartDate,
                    TicketSalesEndDate = eventForm.TicketSalesEndDate,
                    DrinkSalesStartDate = eventForm.DrinkSalesStartDate,
                    DrinkSalesEndDate = eventForm.DrinkSalesEndDate,
                    Capacity = eventForm.Capacity,
                    BasePrice = eventForm.BasePrice,
                    IsActive = eventForm.IsActive,
                    SeasonId = eventForm.SeasonId,
                    SectorPrices = BuildSectorPriceInputs(),
                    Staff = BuildStaffInputs(),
                    // A new image replaces the stored one; otherwise RemovePoster clears it and
                    // leaving both unset keeps whatever is already there.
                    PosterImageBase64 = posterBase64,
                    PosterContentType = posterBase64 != null ? "image/png" : null,
                    PosterPrompt = posterBase64 != null ? posterPromptUsed : null,
                    PosterThumbnailBase64 = posterThumbnailBase64,
                    PosterApproved = posterBase64 != null && posterApproved,
                    PosterSourceSignature = posterBase64 != null ? posterSignature : null,
                    // Approving an already-stored poster (no new image in this session).
                    ApproveExistingPoster = posterBase64 == null && existingPosterPending && posterApproved,
                    RemovePoster = removeExistingPoster
                };

                var response = await ApiService.Http.PostAsync($"events/{editingEvent.Id}", updateDto);
                if (response.IsSuccessStatusCode)
                {
                    await RememberAwayCrestAsync();
                    await LoadEvents();
                    await LoadSeasons();
                    HideEventModal();
                    ShowAlert(L["Events_UpdatedAlert", updateDto.Name], "success");
                }
                else
                {
                    var body = await response.Content.ReadAsStringAsync();
                    ShowAlert(ExtractErrorMessage(body) ?? L["Events_UpdateFailedAlert", (int)response.StatusCode], "danger");
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
            // Two whole sentences rather than an interpolated verb — the verb inflects with the
            // sentence in Croatian, so a swapped-in word would not read correctly.
            var prompt = newStatus == EventStatus.Cancelled
                ? L["Events_ConfirmCancelTransition", evt.Name]
                : L["Events_ConfirmCompleteTransition", evt.Name];
            var confirmed = await JSRuntime.InvokeAsync<bool>("confirm", prompt.Value);
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
                ShowAlert(L["Events_StatusChangedAlert", evt.Name, StatusDisplay(newStatus)], "success");
            }
            else
            {
                var body = await response.Content.ReadAsStringAsync();
                ShowAlert(ExtractErrorMessage(body) ?? L["Events_StatusChangeFailedAlert", (int)response.StatusCode], "danger");
            }
        }
        catch (Exception ex)
        {
            ShowAlert(L["Events_StatusChangeErrorAlert", ex.Message], "danger");
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

    /// <summary>
    /// Formats an optional [from, to] window for the event card. An unset bound renders as an
    /// en-dash "—" (that side of the window is open), so e.g. a null start shows "— → Jul 14, 18:00".
    /// When both bounds are unset the whole window is open and this returns a single "—".
    /// </summary>
    private static string FormatWindow(DateTime? from, DateTime? to)
    {
        if (from is null && to is null)
            return "—";
        // API timestamps are UTC; render local wall-clock to match the rest of the card.
        static string Fmt(DateTime? d) => ToLocal(d)?.ToString("MMM dd, HH:mm") ?? "—";
        return $"{Fmt(from)} → {Fmt(to)}";
    }

    // --- Lifecycle display helpers (badge colour, button style, labels) ---

    /// <summary>
    /// Localized counterpart to <see cref="EventLifecycle.EditBlockedReason"/>, which returns English
    /// for API responses. The lifecycle rule stays in Shared; only the wording is localized here.
    /// </summary>
    private string EditBlockedMessage(EventStatus status) => status switch
    {
        EventStatus.Cancelled => L["Events_EditLockedCancelled"],
        _ => L["Events_EditLockedTooltip"]
    };

    /// <summary>
    /// True when the event belongs to a season that has already ended. A closed season's schedule is
    /// frozen, so the event cannot be edited, deleted or re-published — independently of its own
    /// lifecycle status. Resolved from the loaded <see cref="seasons"/> list; an event with no season,
    /// or one whose season hasn't loaded, is not treated as frozen (the API remains authoritative).
    /// </summary>
    private bool IsSeasonClosed(int? seasonId)
    {
        if (seasonId == null || seasons == null)
            return false;

        var season = seasons.FirstOrDefault(s => s.Id == seasonId.Value);
        return season != null && SeasonLifecycle.IsClosed(season.EndDate);
    }

    /// <summary>Localized counterpart to <see cref="SeasonLifecycle.ChangeBlockedReason"/>.</summary>
    private string SeasonClosedMessage(int? seasonId)
    {
        var season = seasons?.FirstOrDefault(s => s.Id == seasonId);
        return L["Events_SeasonClosedLocked", season?.Name ?? "", season?.EndDate.ToString("d") ?? ""];
    }

    /// <summary>
    /// Localized name for a lifecycle phase. Without this the card rendered the raw enum
    /// ("Future"/"Active"/"Past") in English beside otherwise-translated text.
    /// </summary>
    private string PhaseDisplay(EventPhase phase) => phase switch
    {
        EventPhase.Future => L["Events_Phase_Future"],
        EventPhase.Active => L["Events_Phase_Active"],
        EventPhase.Past => L["Events_Phase_Past"],
        _ => phase.ToString()
    };

    /// <summary>
    /// Localized name for a lifecycle status. Instance (not static) so it can reach the injected
    /// localizer — previously this returned hard-coded English beside translated card text.
    /// </summary>
    private string StatusDisplay(EventStatus status) => status switch
    {
        EventStatus.Planned => L["Events_Status_Planned"],
        EventStatus.OnSale => L["Events_Status_OnSale"],
        EventStatus.SoldOut => L["Events_Status_SoldOut"],
        EventStatus.Active => L["Events_Status_Active"],
        EventStatus.Completed => L["Events_Status_Completed"],
        EventStatus.Cancelled => L["Events_Status_Cancelled"],
        _ => status.ToString()
    };

    private static string StatusBadgeClass(EventStatus status) => status switch
    {
        EventStatus.Planned => "is-pending",
        EventStatus.OnSale => "is-accepted",
        EventStatus.SoldOut => "is-ready",
        EventStatus.Active => "is-active",
        EventStatus.Completed => "is-inactive",
        EventStatus.Cancelled => "is-cancelled",
        _ => "is-inactive"
    };

    // Card-level colour keyed to the lifecycle phase, so a panel full of events reads
    // as phases at a glance rather than needing the status pill on every card.
    private static string PhaseModifier(EventStatus status) => status switch
    {
        EventStatus.Planned => "planned",
        EventStatus.OnSale => "onsale",
        EventStatus.SoldOut => "soldout",
        EventStatus.Active => "active",
        EventStatus.Completed => "completed",
        EventStatus.Cancelled => "cancelled",
        _ => "planned"
    };

    private static string TransitionButtonClass(EventStatus target) => target switch
    {
        EventStatus.OnSale => "pill-btn pill-btn--primary",
        EventStatus.Active => "pill-btn pill-btn--success",
        EventStatus.Cancelled => "pill-btn pill-btn--danger",
        _ => "pill-btn"
    };

    private static (string Label, string Icon) TransitionMeta(EventStatus target) => target switch
    {
        EventStatus.Planned => ("Unpublish", "bi-arrow-counterclockwise"),
        EventStatus.OnSale => ("Put On Sale", "bi-megaphone"),
        EventStatus.SoldOut => ("Mark Sold Out", "bi-x-octagon"),
        EventStatus.Active => ("Go Live", "bi-broadcast"),
        EventStatus.Completed => ("Complete", "bi-check2-circle"),
        EventStatus.Cancelled => ("Cancel Event", "bi-x-circle"),
        _ => (target.ToString(), "bi-arrow-right")
    };

    private async Task DeleteEvent(int eventId)
    {
        // A closed season's schedule is frozen — deleting would erase settled history.
        var seasonId = events?.FirstOrDefault(e => e.Id == eventId)?.SeasonId;
        if (IsSeasonClosed(seasonId))
        {
            ShowAlert(SeasonClosedMessage(seasonId), "danger");
            return;
        }

        if (await JSRuntime.InvokeAsync<bool>("confirm", L["Events_ConfirmDelete"].Value))
        {
            var (success, errorMessage) = await ApiService.Http.DeleteAsync($"events/{eventId}");
            if (success)
            {
                await LoadEvents();
                ShowAlert(L["Events_DeletedAlert"], "success");
            }
            else
            {
                ShowAlert(L["Events_DeleteFailedAlert", errorMessage ?? string.Empty], "danger");
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
        /// <summary>Start of ticket sales. Null = sales open as soon as the event is put on sale.</summary>
        public DateTime? TicketSalesStartDate { get; set; }
        /// <summary>End of ticket sales. Null = sales stay open while the event is on sale.</summary>
        public DateTime? TicketSalesEndDate { get; set; }
        /// <summary>Start of drink ordering. Null = ordering opens as soon as the event goes live.</summary>
        public DateTime? DrinkSalesStartDate { get; set; }
        /// <summary>End of drink ordering. Null = ordering stays open while the event is live.</summary>
        public DateTime? DrinkSalesEndDate { get; set; }
        public int Capacity { get; set; }
        public decimal BasePrice { get; set; }
        public bool IsActive { get; set; } = true;
        public int? SeasonId { get; set; }
        /// <summary>Text prompt sent to the image API to generate this event's poster.</summary>
        public string? PosterPrompt { get; set; }
    }
}