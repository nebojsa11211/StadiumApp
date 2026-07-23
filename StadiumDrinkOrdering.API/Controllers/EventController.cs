using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Services;
using StadiumDrinkOrdering.API.Authorization;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.Shared.DTOs;
using System.ComponentModel.DataAnnotations;

namespace StadiumDrinkOrdering.API.Controllers;

[ApiController]
[Route("events")]
public class EventController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ISeasonService _seasonService;
    private readonly ITicketIngestionService _ingestion;
    private readonly ApplicationDbContext _db;
    private readonly IWebHostEnvironment _env;
    private readonly ILogger<EventController> _logger;

    public EventController(
        IEventService eventService,
        ISeasonService seasonService,
        ITicketIngestionService ingestion,
        ApplicationDbContext db,
        IWebHostEnvironment env,
        ILogger<EventController> logger)
    {
        _eventService = eventService;
        _seasonService = seasonService;
        _ingestion = ingestion;
        _db = db;
        _env = env;
        _logger = logger;
    }

    /// <summary>
    /// Get all events with optional filtering
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetEvents([FromQuery] bool activeOnly = false)
    {
        try
        {
            var events = await _eventService.GetEventsAsync(activeOnly);
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get currently active events
    /// </summary>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetActiveEvents()
    {
        try
        {
            var events = await _eventService.GetActiveEventsAsync();
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// The single event that is currently live — Active and still within its time window
    /// (<see cref="Event.IsLiveAt"/> / <see cref="EventDto.IsCurrentlyLive"/>). Returns 204 No Content
    /// when nothing is live right now. This is the "is the venue open for game-day operations?" signal
    /// the Bar and Runner staff apps gate on: when no event is live those tools have nothing to do and
    /// are blocked.
    /// </summary>
    [HttpGet("live")]
    public async Task<ActionResult<EventDto>> GetCurrentLiveEvent()
    {
        try
        {
            // GetActiveEventsAsync first auto-completes any elapsed events, so a stale-Active event
            // never lingers as "live". Pick the single earliest-starting live event.
            var events = await _eventService.GetActiveEventsAsync();
            var now = DateTime.UtcNow;
            var liveEvent = events
                .Where(e => e.IsLiveAt(now))
                .OrderBy(e => e.EventDate)
                .FirstOrDefault();

            if (liveEvent is null)
            {
                return NoContent();
            }

            // Map only the one live event (cheap) rather than the whole active set.
            var dto = (await MapEventsWithSoldCountsAsync(new[] { liveEvent })).First();
            return Ok(dto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving current live event");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// DEV-ONLY testing helper: the active tickets of the currently-live event, so the Customer scan
    /// page can offer a pick-a-ticket combobox instead of forcing a developer to type/scan a real code.
    /// 404s outside the Development environment so it can never leak ticket codes in production, and
    /// returns an empty list when nothing is live. Unauthenticated on purpose — the public scan page
    /// (fans scanning their own ticket) has no login at that point.
    /// </summary>
    [HttpGet("live/test-tickets")]
    public async Task<ActionResult<IEnumerable<TestTicketDto>>> GetLiveEventTestTickets()
    {
        if (!_env.IsDevelopment())
            return NotFound();

        try
        {
            var events = await _eventService.GetActiveEventsAsync();
            var now = DateTime.UtcNow;
            var liveEvent = events
                .Where(e => e.IsLiveAt(now))
                .OrderBy(e => e.EventDate)
                .FirstOrDefault();

            if (liveEvent is null)
                return Ok(Array.Empty<TestTicketDto>());

            var tickets = await _db.Tickets
                .AsNoTracking()
                .Where(t => t.EventId == liveEvent.Id && t.IsActive)
                .OrderBy(t => t.TicketNumber)
                .Select(t => new TestTicketDto
                {
                    TicketNumber = t.TicketNumber,
                    Label = t.SeatNumber != null
                        ? $"{t.TicketNumber} — {t.Section} {t.SeatNumber}"
                        : t.TicketNumber
                })
                .Take(200)
                .ToListAsync();

            return Ok(tickets);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving live-event test tickets");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get upcoming events
    /// </summary>
    [HttpGet("upcoming")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetUpcomingEvents()
    {
        try
        {
            var events = await _eventService.GetUpcomingEventsAsync();
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving upcoming events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get past events
    /// </summary>
    [HttpGet("past")]
    public async Task<ActionResult<IEnumerable<EventDto>>> GetPastEvents()
    {
        try
        {
            var events = await _eventService.GetPastEventsAsync();
            var eventDtos = await MapEventsWithSoldCountsAsync(events);
            return Ok(eventDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving past events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Real stadium capacity — the sum of the drawing-tool overlay sectors' seats. This is the
    /// authoritative per-event capacity (every event is held in the same physical stadium), so the
    /// Admin event form surfaces it read-only rather than letting an arbitrary number be typed.
    /// Returns 0 when no stadium has been drawn yet.
    /// </summary>
    [HttpGet("stadium-capacity")]
    public async Task<ActionResult<int>> GetStadiumCapacity()
    {
        try
        {
            return Ok(await _ingestion.GetStadiumCapacityAsync());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving stadium capacity");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Lists overlay sectors with their default price and the per-event override for each, used by
    /// the Admin event modal's "Sector prices" editor. Pass <paramref name="eventId"/> to prefill the
    /// existing event's overrides; omit it for a new (unsaved) event.
    /// </summary>
    [HttpGet("sector-prices")]
    public async Task<ActionResult<List<EventSectorPriceDto>>> GetSectorPrices([FromQuery] int? eventId = null)
    {
        try
        {
            return Ok(await _eventService.GetSectorPricingAsync(eventId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving sector prices for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Lists staff (Bartenders/Waiters) with a flag for whether each is assigned to the event, used by
    /// the Admin event modal's "Staff" picker. Pass <paramref name="eventId"/> to prefill the existing
    /// event's assignments; omit it for a new (unsaved) event.
    /// </summary>
    [HttpGet("staff")]
    public async Task<ActionResult<List<EventStaffMemberDto>>> GetEventStaff([FromQuery] int? eventId = null)
    {
        try
        {
            return Ok(await _eventService.GetEventStaffAsync(eventId));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving staff for event {EventId}", eventId);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get event by ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<EventDto>> GetEvent(int id)
    {
        try
        {
            var eventItem = await _eventService.GetEventByIdAsync(id);
            if (eventItem == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();
            return Ok(MapEventToDto(eventItem, stadiumCapacity: stadiumCapacity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get event analytics
    /// </summary>
    [HttpGet("{id}/analytics")]
    public async Task<ActionResult<EventAnalyticsResponse>> GetEventAnalytics(int id)
    {
        try
        {
            var analytics = await _eventService.GetEventAnalyticsAsync(id);
            if (analytics == null)
            {
                return NotFound($"Analytics for event {id} not found");
            }

            return Ok(analytics);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving analytics for event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Get post-event statistics (ticketing/occupancy + drink ordering + combined revenue) for a
    /// single event. Powers the admin event-statistics page opened from a completed event's card.
    /// </summary>
    [HttpGet("{id}/statistics")]
    public async Task<ActionResult<EventStatisticsDto>> GetEventStatistics(int id)
    {
        try
        {
            var stats = await _eventService.GetEventStatisticsAsync(id);
            if (stats == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(stats);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving statistics for event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Create new event
    /// </summary>
    [HttpPost]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDto>> CreateEvent([FromBody] CreateEventDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!IsValidWindow(request.Date, request.EndDate, out var windowError))
            {
                return BadRequest(new { message = windowError });
            }

            if (!IsValidSalesWindow(request.TicketSalesStartDate, request.TicketSalesEndDate, out var salesWindowError))
            {
                return BadRequest(new { message = salesWindowError });
            }

            if (!IsValidDrinkSalesWindow(request.DrinkSalesStartDate, request.DrinkSalesEndDate, out var drinkWindowError))
            {
                return BadRequest(new { message = drinkWindowError });
            }

            // An event cannot be scheduled into a season that has already ended.
            var closedSeasonError = await GetClosedSeasonErrorAsync(request.SeasonId);
            if (closedSeasonError != null)
            {
                return BadRequest(new { message = closedSeasonError });
            }

            var name = request.Name.Trim();
            if (await _eventService.IsEventNameTakenAsync(name))
            {
                return Conflict(new { message = $"An event named \"{name}\" already exists. Event names must be unique." });
            }

            // Capacity is not client-supplied trivia: it is the real stadium seat count (sum of the
            // drawing-tool overlays). Compute it server-side so the stored TotalSeats always matches
            // the actual seating layout; only fall back to the request value before a stadium exists.
            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();

            var eventType = string.IsNullOrWhiteSpace(request.EventType) ? "Match" : request.EventType.Trim();
            if (!ResolveTeams(eventType, request.HomeTeam, request.AwayTeam, out var homeTeam, out var awayTeam, out var teamError))
            {
                return BadRequest(new { message = teamError });
            }

            var eventItem = new Event
            {
                EventName = name,
                EventType = eventType,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                // The form posts local wall-clock times; PostgreSQL timestamptz only accepts UTC.
                EventDate = ToUtc(request.Date!.Value),
                EventEndDate = ToUtc(request.EndDate),
                TicketSalesStartDate = ToUtc(request.TicketSalesStartDate),
                TicketSalesEndDate = ToUtc(request.TicketSalesEndDate),
                DrinkSalesStartDate = ToUtc(request.DrinkSalesStartDate),
                DrinkSalesEndDate = ToUtc(request.DrinkSalesEndDate),
                TotalSeats = stadiumCapacity > 0 ? stadiumCapacity : request.Capacity,
                Description = request.Description,
                BaseTicketPrice = request.BasePrice,
                IsActive = request.IsActive,
                SeasonId = request.SeasonId
            };

            var createdEvent = await _eventService.CreateEventAsync(eventItem);

            // Resolve the team names to directory entries so crests survive a later rename.
            await LinkTeamsAsync(createdEvent.Id);

            // Attach the poster generated in the admin form, if any. A bad image is not worth
            // failing the whole creation over — the event is saved either way.
            if (!string.IsNullOrWhiteSpace(request.PosterImageBase64))
            {
                var posterError = await SavePosterAsync(createdEvent.Id, request.PosterImageBase64,
                    request.PosterContentType, request.PosterPrompt, request.PosterThumbnailBase64,
                    request.PosterApproved, request.PosterSourceSignature);
                if (posterError != null)
                    _logger.LogWarning("Event {EventId} created but its poster was rejected: {Error}", createdEvent.Id, posterError);
            }

            // Persist any per-sector price overrides supplied with the new event.
            await _eventService.SaveSectorPricesAsync(createdEvent.Id, request.SectorPrices);

            // Persist the staff assigned to work the new event (function + covered sectors).
            await _eventService.SaveEventStaffAsync(createdEvent.Id, request.Staff);

            // If linked to a season, extend existing season passes to cover this new event.
            if (createdEvent.SeasonId != null)
                await _ingestion.BackfillSeasonTicketsForEventAsync(createdEvent.Id);

            return CreatedAtAction(nameof(GetEvent), new { id = createdEvent.Id }, await MapEventWithSeasonAsync(createdEvent));
        }
        catch (SeasonClosedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating event");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Update existing event
    /// </summary>
    [HttpPut("{id}")]
    [HttpPost("{id}")] // Alias: the Admin HTTP helper only issues POST/GET/DELETE.
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDto>> UpdateEvent(int id, [FromBody] UpdateEventDto request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Load first so unspecified (null) fields retain their existing values.
            var existing = await _eventService.GetEventByIdAsync(id);
            if (existing == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            // Past/terminal events are frozen: their record can no longer be edited.
            if (!EventLifecycle.CanEdit(existing.Status))
            {
                return BadRequest(new { message = EventLifecycle.EditBlockedReason(existing.Status) });
            }

            // Events in a season that has ended are frozen too. The service enforces this as well
            // (so ingestion is covered); checking here first means the season message wins over
            // incidental validation errors further down.
            var closedSeasonError = await GetClosedSeasonErrorAsync(existing.SeasonId)
                ?? await GetClosedSeasonErrorAsync(request.SeasonId);
            if (closedSeasonError != null)
            {
                return BadRequest(new { message = closedSeasonError });
            }

            // A sector with tickets already sold for this event can't be disabled (would orphan buyers).
            // Validate before persisting anything so a rejected disable leaves the event untouched.
            var disableBlock = await _eventService.ValidateSectorDisablesAsync(id, request.SectorPrices);
            if (disableBlock != null)
            {
                return BadRequest(new { message = disableBlock });
            }

            // Request values are local wall-clock from the form; stored values are already UTC.
            var newStart = ToUtc(request.Date ?? existing.EventDate);
            var newEnd = ToUtc(request.EndDate ?? existing.EventEndDate);
            if (!IsValidWindow(newStart, newEnd, out var windowError))
            {
                return BadRequest(new { message = windowError });
            }

            // Null in the request means "unchanged" (matches the event-window merge above), so validate
            // the effective (merged) sales window.
            var newSalesStart = ToUtc(request.TicketSalesStartDate ?? existing.TicketSalesStartDate);
            var newSalesEnd = ToUtc(request.TicketSalesEndDate ?? existing.TicketSalesEndDate);
            if (!IsValidSalesWindow(newSalesStart, newSalesEnd, out var salesWindowError))
            {
                return BadRequest(new { message = salesWindowError });
            }

            // Null in the request means "unchanged", so validate the effective (merged) drink window.
            var newDrinkStart = ToUtc(request.DrinkSalesStartDate ?? existing.DrinkSalesStartDate);
            var newDrinkEnd = ToUtc(request.DrinkSalesEndDate ?? existing.DrinkSalesEndDate);
            if (!IsValidDrinkSalesWindow(newDrinkStart, newDrinkEnd, out var drinkWindowError))
            {
                return BadRequest(new { message = drinkWindowError });
            }

            var newSeasonId = request.SeasonId ?? existing.SeasonId;
            var seasonLinkIsNew = newSeasonId != null && newSeasonId != existing.SeasonId;

            var newName = string.IsNullOrWhiteSpace(request.Name) ? existing.EventName : request.Name.Trim();
            if (await _eventService.IsEventNameTakenAsync(newName, id))
            {
                return Conflict(new { message = $"An event named \"{newName}\" already exists. Event names must be unique." });
            }

            // Re-derive capacity from the real stadium (sum of overlays) rather than trusting the
            // request; the Admin form shows it read-only. Fall back to the existing value only when
            // no stadium has been drawn yet.
            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();

            // Null in the request means "unchanged", so validate the effective (merged) type/teams.
            var newType = string.IsNullOrWhiteSpace(request.EventType) ? existing.EventType : request.EventType.Trim();
            if (!ResolveTeams(newType, request.HomeTeam ?? existing.HomeTeam, request.AwayTeam ?? existing.AwayTeam,
                    out var homeTeam, out var awayTeam, out var teamError))
            {
                return BadRequest(new { message = teamError });
            }

            var eventItem = new Event
            {
                EventName = newName,
                EventType = newType,
                HomeTeam = homeTeam,
                AwayTeam = awayTeam,
                EventDate = newStart,
                EventEndDate = newEnd,
                TicketSalesStartDate = newSalesStart,
                TicketSalesEndDate = newSalesEnd,
                DrinkSalesStartDate = newDrinkStart,
                DrinkSalesEndDate = newDrinkEnd,
                VenueId = existing.VenueId,
                TotalSeats = stadiumCapacity > 0 ? stadiumCapacity : (request.Capacity ?? existing.TotalSeats),
                Description = request.Description ?? existing.Description,
                ImageUrl = existing.ImageUrl,
                BaseTicketPrice = request.BasePrice ?? existing.BaseTicketPrice,
                SeasonId = newSeasonId
            };

            var updatedEvent = await _eventService.UpdateEventAsync(id, eventItem);
            if (updatedEvent == null)
            {
                return NotFound($"Event with ID {id} not found");
            }

            // The teams may have changed with this edit; re-resolve the directory links.
            await LinkTeamsAsync(id);

            // Poster: a supplied image replaces any existing one; RemovePoster clears it. Neither
            // means "leave the current poster alone", matching how the other fields merge.
            if (!string.IsNullOrWhiteSpace(request.PosterImageBase64))
            {
                var posterError = await SavePosterAsync(id, request.PosterImageBase64,
                    request.PosterContentType, request.PosterPrompt, request.PosterThumbnailBase64,
                    request.PosterApproved, request.PosterSourceSignature);
                if (posterError != null)
                    return BadRequest(new { message = posterError });
            }
            else if (request.RemovePoster)
            {
                await DeletePosterAsync(id);
            }
            else if (request.ApproveExistingPoster)
            {
                // "Text looks correct" on a poster that was already stored (typically one produced
                // by an auto-regeneration, which always lands pending review).
                var posterOwner = await _db.Events.FirstOrDefaultAsync(e => e.Id == id);
                if (posterOwner is { HasPoster: true, PosterApprovedAt: null })
                {
                    posterOwner.PosterApprovedAt = DateTime.UtcNow;
                    await _db.SaveChangesAsync();
                }
            }

            // Apply per-sector price overrides (null = leave existing overrides unchanged).
            await _eventService.SaveSectorPricesAsync(id, request.SectorPrices);

            // Apply staff assignments — function + covered sectors (null = leave existing unchanged).
            await _eventService.SaveEventStaffAsync(id, request.Staff);

            // Newly linking an event to a season backfills its existing season passes.
            if (seasonLinkIsNew)
                await _ingestion.BackfillSeasonTicketsForEventAsync(updatedEvent.Id);

            return Ok(await MapEventWithSeasonAsync(updatedEvent));
        }
        catch (SeasonClosedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Returns the user-facing reason when <paramref name="seasonId"/> names a season that has ended
    /// (and is therefore closed to schedule changes), or null when the season is open, unknown, or not
    /// set. Mirrors the authoritative guard in <see cref="IEventService"/> so the API can fail early
    /// with the most relevant message.
    /// </summary>
    private async Task<string?> GetClosedSeasonErrorAsync(int? seasonId)
    {
        if (seasonId == null)
            return null;

        var season = await _db.Seasons
            .AsNoTracking()
            .Where(s => s.Id == seasonId.Value)
            .Select(s => new { s.Name, s.EndDate })
            .FirstOrDefaultAsync();

        return season != null && SeasonLifecycle.IsClosed(season.EndDate)
            ? SeasonLifecycle.ChangeBlockedReason(season.Name, season.EndDate)
            : null;
    }

    /// <summary>
    /// Validates the optional event window: an end must come strictly after the start.
    /// A missing start or end is allowed (open-ended window).
    /// </summary>
    private static bool IsValidWindow(DateTime? start, DateTime? end, out string error)
    {
        if (start.HasValue && end.HasValue && end.Value <= start.Value)
        {
            error = "The event end time must be after the start time.";
            return false;
        }
        error = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the optional ticket-sales window: when both bounds are supplied the end must come
    /// strictly after the start. A missing start or end is allowed (that side of the window is open).
    /// </summary>
    private static bool IsValidSalesWindow(DateTime? start, DateTime? end, out string error)
    {
        if (start.HasValue && end.HasValue && end.Value <= start.Value)
        {
            error = "The ticket sales end time must be after the sales start time.";
            return false;
        }
        error = string.Empty;
        return true;
    }

    /// <summary>
    /// Validates the optional drink-ordering window: when both bounds are supplied the end must come
    /// strictly after the start. A missing start or end is allowed (that side of the window is open).
    /// </summary>
    private static bool IsValidDrinkSalesWindow(DateTime? start, DateTime? end, out string error)
    {
        if (start.HasValue && end.HasValue && end.Value <= start.Value)
        {
            error = "The drink ordering end time must be after the drink ordering start time.";
            return false;
        }
        error = string.Empty;
        return true;
    }

    // ---- Event poster ---------------------------------------------------------------------

    /// <summary>
    /// Serves an event's poster image. Anonymous so it can be used directly as an <c>&lt;img src&gt;</c>
    /// (browsers do not attach the bearer token to image requests) — a poster is public promotional
    /// artwork, not privileged data.
    /// </summary>
    /// <param name="variant">
    /// "thumb" serves the small JPEG variant used by the customer fixture strip; anything else
    /// serves the full-size original. A poster saved before thumbnails existed has none, so the
    /// thumb request transparently falls back to the full image rather than 404ing.
    /// </param>
    [HttpGet("{id}/image")]
    [AllowAnonymous]
    public async Task<IActionResult> GetEventPoster(int id, [FromQuery] string? variant = null)
    {
        var poster = await _db.EventPosters
            .AsNoTracking()
            .Include(p => p.Event)
            .FirstOrDefaultAsync(p => p.EventId == id);

        if (poster == null || poster.ImageData.Length == 0)
            return NotFound();

        // Posters only change when an admin regenerates one, so let clients cache briefly.
        Response.Headers.CacheControl = "public, max-age=300";

        if (string.Equals(variant, "thumb", StringComparison.OrdinalIgnoreCase)
            && poster.ThumbnailData is { Length: > 0 } thumb)
        {
            return File(thumb, poster.ThumbnailContentType ?? "image/jpeg");
        }

        return File(poster.ImageData, poster.Event?.PosterContentType ?? "image/png");
    }

    // ---- Reusable team crests -------------------------------------------------------------

    /// <summary>
    /// Looks up the stored crest for a team name, so an opponent's crest only ever has to be
    /// uploaded once. Backed by the <see cref="Team"/> directory (managed at /admin/teams); returns
    /// 404 when the team is unknown or has no crest yet.
    ///
    /// Kept on this controller, and returning base64 rather than raw bytes, because the event form
    /// feeds the crest straight into poster generation, which takes images as base64.
    /// </summary>
    [HttpGet("team-crest")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    public async Task<ActionResult<TeamCrestDto>> GetTeamCrest([FromQuery] string teamName)
    {
        var key = Team.Normalize(teamName);
        if (key == null)
            return BadRequest(new { message = "A team name is required." });

        var team = await _db.Teams.AsNoTracking().FirstOrDefaultAsync(t => t.NormalizedName == key);
        if (team?.Logo == null || team.Logo.Length == 0)
            return NotFound();

        return Ok(new TeamCrestDto
        {
            DisplayName = team.Name,
            ImageBase64 = Convert.ToBase64String(team.Logo)
        });
    }

    /// <summary>
    /// Stores or replaces the crest for a team name, creating the directory entry if this opponent
    /// has not been seen before. Called when the admin uploads an away crest on the event form, so
    /// building a fixture still populates the directory without a detour through the Teams page.
    /// </summary>
    [HttpPost("team-crest")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> SaveTeamCrest([FromBody] SaveTeamCrestDto request)
    {
        var key = Team.Normalize(request.TeamName);
        if (key == null)
            return BadRequest(new { message = "A team name is required." });

        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(request.ImageBase64?.Trim() ?? string.Empty);
        }
        catch (FormatException)
        {
            return BadRequest(new { message = "The crest image was not valid base64." });
        }

        if (bytes.Length == 0)
            return BadRequest(new { message = "The crest image was empty." });
        if (bytes.Length > MaxCrestBytes)
            return BadRequest(new { message = $"The crest image is too large; the limit is {MaxCrestBytes / 1024} KB." });

        var team = await _db.Teams.FirstOrDefaultAsync(t => t.NormalizedName == key);
        if (team == null)
        {
            team = new Team
            {
                NormalizedName = key,
                Name = request.TeamName.Trim(),
                CreatedAt = DateTime.UtcNow
            };
            _db.Teams.Add(team);
        }
        else
        {
            // Only the crest is being set here; leave the curated name/colours from the Teams page
            // intact rather than letting a fixture's casing overwrite them.
            team.UpdatedAt = DateTime.UtcNow;
        }

        // The admin rasterises crests to PNG before upload (the image API only accepts PNG).
        team.Logo = bytes;
        team.LogoContentType = "image/png";

        await _db.SaveChangesAsync();
        return Ok(new { message = "Crest saved." });
    }

    /// <summary>
    /// Resolves an event's free-text team names to their <see cref="Club"/>/<see cref="Team"/>
    /// directory entries and stores the ids. The names remain authoritative for display; these
    /// links exist so a later rename in the directory does not orphan the crest.
    ///
    /// Runs after the event has been saved (rather than as part of building the entity) so it also
    /// covers events created through paths that bypass the form, and so a failure to match is never
    /// able to fail the save itself.
    /// </summary>
    private async Task LinkTeamsAsync(int eventId)
    {
        var evt = await _db.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (evt == null) return;

        var homeKey = Team.Normalize(evt.HomeTeam);
        var awayKey = Team.Normalize(evt.AwayTeam);

        evt.HomeClubId = homeKey == null
            ? null
            : (await _db.Clubs.AsNoTracking()
                .FirstOrDefaultAsync(c => c.Name.ToLower() == homeKey))?.Id;

        // Set the navigation (not the raw FK) so a freshly created, not-yet-saved directory entry
        // still has its Id fixed up onto the event by the single SaveChanges below.
        if (awayKey == null)
        {
            evt.AwayTeamId = null;
            evt.AwayTeamProfile = null;
        }
        else
        {
            evt.AwayTeamProfile = await EnsureAwayTeamAsync(awayKey, evt.AwayTeam);
        }

        await _db.SaveChangesAsync();
    }

    /// <summary>
    /// Returns the directory <see cref="Team"/> for a normalized away-team name, creating a
    /// name-only entry (no crest, no curated colours) when a fixture names an opponent that is not
    /// yet in the directory. This keeps the Teams page reflecting every opponent that appears on a
    /// fixture; a crest and branding can be filled in later on that page.
    /// </summary>
    private async Task<Team> EnsureAwayTeamAsync(string normalizedName, string? displayName)
    {
        var team = await _db.Teams.FirstOrDefaultAsync(t => t.NormalizedName == normalizedName);
        if (team == null)
        {
            team = new Team
            {
                NormalizedName = normalizedName,
                Name = (displayName ?? normalizedName).Trim(),
                CreatedAt = DateTime.UtcNow
            };
            _db.Teams.Add(team);
        }
        return team;
    }

    /// <summary>Crests are small logos, not photographs — cap them well below the poster limit.</summary>
    private const int MaxCrestBytes = 2 * 1024 * 1024;

    /// <summary>
    /// Stores (or replaces) an event's poster: bytes into <see cref="EventPoster"/>, display metadata
    /// onto the event itself. Returns null on success, or a user-facing error message when the
    /// supplied image is unusable.
    /// </summary>
    private async Task<string?> SavePosterAsync(int eventId, string base64, string? contentType, string? prompt,
        string? thumbnailBase64 = null, bool approved = false, string? sourceSignature = null)
    {
        byte[] bytes;
        try
        {
            // Tolerate a full data URI as well as raw base64, so callers can pass either.
            var payload = base64.Contains("base64,", StringComparison.OrdinalIgnoreCase)
                ? base64[(base64.IndexOf("base64,", StringComparison.OrdinalIgnoreCase) + 7)..]
                : base64;
            bytes = Convert.FromBase64String(payload.Trim());
        }
        catch (FormatException)
        {
            return "The poster image was not valid base64.";
        }

        if (bytes.Length == 0)
            return "The poster image was empty.";

        if (bytes.Length > MaxPosterBytes)
            return $"The poster image is too large ({bytes.Length / 1024 / 1024} MB); the limit is {MaxPosterBytes / 1024 / 1024} MB.";

        var evt = await _db.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (evt == null)
            return $"Event with ID {eventId} not found.";

        var (width, height) = ReadPngDimensions(bytes);
        evt.PosterContentType = string.IsNullOrWhiteSpace(contentType) ? "image/png" : contentType.Trim();
        evt.PosterWidth = width;
        evt.PosterHeight = height;
        evt.PosterPrompt = string.IsNullOrWhiteSpace(prompt) ? null : prompt.Trim();
        // New artwork means new text to check: approval never carries over from the previous image.
        evt.PosterApprovedAt = approved ? DateTime.UtcNow : null;
        evt.PosterSourceSignature = sourceSignature;

        // A missing/undecodable thumbnail is not fatal — the image endpoint falls back to the full
        // poster, so we store what we can rather than rejecting the whole save.
        byte[]? thumbBytes = null;
        if (!string.IsNullOrWhiteSpace(thumbnailBase64))
        {
            try
            {
                thumbBytes = Convert.FromBase64String(thumbnailBase64.Trim());
                if (thumbBytes.Length == 0 || thumbBytes.Length > MaxPosterBytes)
                    thumbBytes = null;
            }
            catch (FormatException)
            {
                _logger.LogWarning("Poster thumbnail for event {EventId} was not valid base64; storing without it.", eventId);
            }
        }

        var poster = await _db.EventPosters.FirstOrDefaultAsync(p => p.EventId == eventId);
        if (poster == null)
        {
            _db.EventPosters.Add(new EventPoster
            {
                EventId = eventId,
                ImageData = bytes,
                ThumbnailData = thumbBytes,
                ThumbnailContentType = thumbBytes != null ? "image/jpeg" : null
            });
        }
        else
        {
            poster.ImageData = bytes;
            // Replace the thumbnail alongside the image; never leave the old one pointing at new art.
            poster.ThumbnailData = thumbBytes;
            poster.ThumbnailContentType = thumbBytes != null ? "image/jpeg" : null;
            poster.CreatedAt = DateTime.UtcNow;
        }

        await _db.SaveChangesAsync();
        return null;
    }

    /// <summary>Removes an event's poster (bytes and metadata). No-op when it has none.</summary>
    private async Task DeletePosterAsync(int eventId)
    {
        var evt = await _db.Events.FirstOrDefaultAsync(e => e.Id == eventId);
        if (evt != null)
        {
            evt.PosterContentType = null;
            evt.PosterWidth = null;
            evt.PosterHeight = null;
            evt.PosterPrompt = null;
            evt.PosterApprovedAt = null;
            evt.PosterSourceSignature = null;
        }

        var poster = await _db.EventPosters.FirstOrDefaultAsync(p => p.EventId == eventId);
        if (poster != null)
            _db.EventPosters.Remove(poster);

        await _db.SaveChangesAsync();
    }

    /// <summary>Upper bound on a stored poster, guarding against an oversized or malicious payload.</summary>
    private const int MaxPosterBytes = 8 * 1024 * 1024;

    /// <summary>
    /// Normalises an incoming date to UTC before it reaches PostgreSQL. The admin form binds
    /// <c>datetime-local</c> inputs, which arrive as Kind=Local (or Unspecified after JSON
    /// round-tripping), and Npgsql rejects both for a <c>timestamp with time zone</c> column with
    /// "Cannot write DateTime with Kind=Local". Local values are converted; Unspecified ones are
    /// assumed to already be local wall-clock time and treated the same way.
    /// </summary>
    private static DateTime? ToUtc(DateTime? value) => value.HasValue ? ToUtc(value.Value) : null;

    private static DateTime ToUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Local).ToUniversalTime()
    };

    /// <summary>
    /// Reads pixel dimensions from a PNG's IHDR chunk: an 8-byte signature, a 4-byte chunk length,
    /// the "IHDR" tag, then big-endian width and height. Returns (null, null) for anything that
    /// isn't a PNG we can parse — the dimensions are display metadata, not a correctness concern.
    /// </summary>
    private static (int? Width, int? Height) ReadPngDimensions(byte[] image)
    {
        if (image.Length < 24 || image[0] != 0x89 || image[1] != 0x50 || image[2] != 0x4E || image[3] != 0x47)
            return (null, null);

        int width = (image[16] << 24) | (image[17] << 16) | (image[18] << 8) | image[19];
        int height = (image[20] << 24) | (image[21] << 16) | (image[22] << 8) | image[23];
        return (width > 0 ? width : null, height > 0 ? height : null);
    }

    /// <summary>
    /// Delete event
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteEvent(int id)
    {
        try
        {
            var result = await _eventService.DeleteEventAsync(id);
            if (!result)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return NoContent();
        }
        catch (SeasonClosedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Activate event
    /// </summary>
    [HttpPost("{id}/activate")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ActivateEvent(int id)
    {
        try
        {
            var result = await _eventService.ActivateEventAsync(id);
            if (!result)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(new { message = "Event activated successfully" });
        }
        catch (SeasonClosedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error activating event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Deactivate event
    /// </summary>
    [HttpPost("{id}/deactivate")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeactivateEvent(int id)
    {
        try
        {
            var result = await _eventService.DeactivateEventAsync(id);
            if (!result)
            {
                return NotFound($"Event with ID {id} not found");
            }

            return Ok(new { message = "Event deactivated successfully" });
        }
        catch (SeasonClosedException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deactivating event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Transition an event to a new lifecycle status (e.g. OnSale → Active → Completed).
    /// Validated against the allowed state machine; closing an event invalidates its ticket sessions.
    /// </summary>
    [HttpPost("{id}/status")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<EventDto>> TransitionStatus(int id, [FromBody] TransitionEventStatusRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _eventService.TransitionEventStatusAsync(id, request.NewStatus);
            if (result.NotFound)
            {
                return NotFound($"Event with ID {id} not found");
            }

            if (!result.Success)
            {
                return BadRequest(new { message = result.ErrorMessage });
            }

            var updated = await _eventService.GetEventByIdAsync(id);
            var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();
            return Ok(MapEventToDto(updated!, stadiumCapacity: stadiumCapacity));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error transitioning status for event {EventId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// One-time maintenance: reconcile orders on events that are already Completed/Cancelled but still
    /// have in-flight drink orders (historical orphans left before the on-completion sweep existed).
    /// Reuses the same idempotent, refund-correct cancellation path, so it is safe to run repeatedly.
    /// Optional <paramref name="eventId"/> limits the sweep to a single (terminal) event; omit it to
    /// reconcile every terminal event. Returns the number of orders reconciled.
    /// </summary>
    [HttpPost("reconcile-orders")]
    [Authorize(Policy = AuthorizationPolicies.CanManageEvents)]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> ReconcileTerminalEventOrders([FromQuery] int? eventId = null)
    {
        try
        {
            var reconciled = await _eventService.ReconcileTerminalEventOrdersAsync(eventId);
            return Ok(new { reconciledOrders = reconciled });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error reconciling orders for terminal events");
            return StatusCode(500, "Internal server error");
        }
    }

    /// <summary>
    /// Maps Event entity to EventDto
    /// </summary>
    /// <summary>
    /// Maps a list of events to DTOs, batch-fetching sold-seat counts in a single query
    /// because the list data source (raw SQL) does not load each event's Tickets navigation.
    /// </summary>
    private async Task<List<EventDto>> MapEventsWithSoldCountsAsync(IEnumerable<Event> events)
    {
        var eventList = events.ToList();
        var ids = eventList.Select(e => e.Id).ToList();
        var soldCounts = await _eventService.GetSoldSeatCountsAsync(ids);
        var seasonSoldCounts = await _eventService.GetSeasonSoldSeatCountsAsync(ids);
        var seasonNames = await _seasonService.GetSeasonNamesAsync(
            eventList.Where(e => e.SeasonId != null).Select(e => e.SeasonId!.Value));
        var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();
        // Ticket and drink takings, so a finished event's card can state its result without a
        // per-card statistics call (two grouped aggregates for the whole page).
        var revenues = await _eventService.GetRevenueSummariesAsync(ids);

        return eventList
            .Select(e => MapEventToDto(
                e,
                soldCounts.GetValueOrDefault(e.Id, 0),
                e.SeasonId != null ? seasonNames.GetValueOrDefault(e.SeasonId.Value) : null,
                seasonSoldCounts.GetValueOrDefault(e.Id, 0),
                stadiumCapacity,
                revenues.GetValueOrDefault(e.Id)))
            .ToList();
    }

    /// <summary>
    /// Normalizes and validates the home/away teams against the event type. A "Match" must carry both
    /// a home side (a resident club) and an away side; any other type carries no teams (both cleared).
    /// Returns false with <paramref name="error"/> set when a Match is missing a team.
    /// </summary>
    private static bool ResolveTeams(string eventType, string? home, string? away,
        out string? homeTeam, out string? awayTeam, out string? error)
    {
        error = null;
        if (!string.Equals(eventType, "Match", StringComparison.OrdinalIgnoreCase))
        {
            // Non-match events are single-title: never keep stale team labels.
            homeTeam = null;
            awayTeam = null;
            return true;
        }

        homeTeam = string.IsNullOrWhiteSpace(home) ? null : home.Trim();
        awayTeam = string.IsNullOrWhiteSpace(away) ? null : away.Trim();
        if (homeTeam == null || awayTeam == null)
        {
            error = "A Match event requires both a home team and an away team.";
            return false;
        }
        return true;
    }

    /// <summary>Maps a single event, resolving its season name (used by create/update responses).</summary>
    private async Task<EventDto> MapEventWithSeasonAsync(Event evt)
    {
        string? seasonName = null;
        if (evt.SeasonId != null)
        {
            var names = await _seasonService.GetSeasonNamesAsync(new[] { evt.SeasonId.Value });
            seasonName = names.GetValueOrDefault(evt.SeasonId.Value);
        }
        var stadiumCapacity = await _ingestion.GetStadiumCapacityAsync();
        return MapEventToDto(evt, seasonName: seasonName, stadiumCapacity: stadiumCapacity);
    }

    /// <summary>
    /// Maps an <see cref="Event"/> to <see cref="EventDto"/>.
    /// </summary>
    /// <param name="soldSeatsOverride">
    /// Sold-seat count to use when the event's Tickets navigation is not loaded (list
    /// endpoints use raw SQL that omits it). When null, sold seats are counted from the
    /// loaded Tickets navigation. Both paths funnel through <see cref="TicketStatuses.CountsAsSold"/>.
    /// </param>
    private EventDto MapEventToDto(Event evt, int? soldSeatsOverride = null, string? seasonName = null, int? seasonSoldOverride = null, int? stadiumCapacity = null, EventRevenueSummary? revenue = null)
    {
        if (evt == null)
        {
            return null;
        }

        // Capacity reflects the REAL stadium (sum of the drawing-tool overlay sectors) whenever a
        // stadium has been drawn; otherwise fall back to the event's stored TotalSeats. This keeps
        // the dashboard "od X mjesta" figure in sync with the actual seating layout.
        int capacity = stadiumCapacity is > 0 ? stadiumCapacity.Value : evt.TotalSeats;

        // Calculate available seats based on sold tickets. List endpoints pass an explicit
        // count (their Tickets navigation isn't loaded); single-event endpoints load Tickets.
        int soldSeats = soldSeatsOverride
            ?? evt.Tickets?.Count(t => TicketStatuses.CountsAsSold(t.Status))
            ?? 0;

        // Of those sold seats, how many are season-pass–derived (the rest are single-event).
        // Same override/loaded-navigation split as above; clamp so it never exceeds total sold.
        int seasonSold = seasonSoldOverride
            ?? evt.Tickets?.Count(t => t.Kind == TicketKind.Season && TicketStatuses.CountsAsSold(t.Status))
            ?? 0;

        return new EventDto
        {
            Id = evt.Id,
            Name = !string.IsNullOrWhiteSpace(evt.EventName) ? evt.EventName : $"Event {evt.Id}",
            EventType = string.IsNullOrWhiteSpace(evt.EventType) ? "Match" : evt.EventType,
            HomeTeam = evt.HomeTeam,
            AwayTeam = evt.AwayTeam,
            Date = evt.EventDate,
            EndDate = evt.EventEndDate,
            TicketSalesStartDate = evt.TicketSalesStartDate,
            TicketSalesEndDate = evt.TicketSalesEndDate,
            DrinkSalesStartDate = evt.DrinkSalesStartDate,
            DrinkSalesEndDate = evt.DrinkSalesEndDate,
            Description = evt.Description,
            Capacity = capacity,
            AvailableSeats = Math.Max(0, capacity - soldSeats),
            SeasonTicketsSold = Math.Min(seasonSold, soldSeats),
            BasePrice = evt.BaseTicketPrice ?? 0m,
            IsActive = evt.IsActive,
            CreatedAt = evt.CreatedAt,
            // Realised takings, batched by the list endpoints. Single-event endpoints don't pass a
            // summary — they leave these at zero rather than firing extra aggregates per request.
            TicketRevenue = revenue?.TicketRevenue ?? 0m,
            SeasonTicketRevenue = revenue?.SeasonTicketRevenue ?? 0m,
            DrinkOrders = revenue?.DrinkOrders ?? 0,
            DrinksRevenue = revenue?.DrinksRevenue ?? 0m,
            Status = evt.Status,
            StatusName = evt.Status.ToString(),
            Phase = EventLifecycle.PhaseOf(evt.Status),
            CanSellTickets = evt.AreTicketSalesOpenAt(DateTime.UtcNow),
            CanOrderDrinks = evt.AreDrinkSalesOpenAt(DateTime.UtcNow),
            IsCurrentlyLive = evt.IsLiveAt(DateTime.UtcNow),
            SeasonId = evt.SeasonId,
            SeasonName = seasonName ?? evt.Season?.Name,
            // Derived from the metadata column, so this stays correct in list queries where the
            // poster bytes (a separate table) are deliberately not loaded.
            HasPoster = evt.HasPoster,
            PosterApproved = evt.IsPosterApproved,
            PosterIsStale = evt.IsPosterStale,
            PosterWidth = evt.PosterWidth,
            PosterHeight = evt.PosterHeight,
            PosterPrompt = evt.PosterPrompt
        };
    }
}