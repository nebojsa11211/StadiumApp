using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Hubs;
using StadiumDrinkOrdering.Shared.DTOs;
using StadiumDrinkOrdering.Shared.DTOs.Integration;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface ITicketIngestionService
{
    Task<TicketingWebhookResult> ProcessWebhookAsync(TicketingWebhookEnvelope envelope, CancellationToken ct = default);
    Task<EventSalesSnapshotDto?> GetEventSnapshotAsync(string externalEventId, CancellationToken ct = default);
    Task<EventSalesSnapshotDto?> GetEventOccupancyAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Links an event that isn't yet part of a season to the current season (when one is set) and
    /// materializes the derived per-seat season-pass tickets, so its sales card reflects season
    /// holders. Idempotent no-op when the event already belongs to a season or no current season
    /// exists. Returns whether a link was made, the resolved season id, and how many derived
    /// season-pass tickets were created.
    /// </summary>
    Task<(bool Linked, int? SeasonId, int DerivedCreated)> EnsureEventLinkedToCurrentSeasonAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Real physical stadium capacity = sum of the drawing-tool overlay sectors' seats
    /// (incl. variable seating). Returns 0 when no overlay stadium has been drawn yet, so
    /// callers can fall back to an event's stored <see cref="Event.TotalSeats"/>.
    /// </summary>
    Task<int> GetStadiumCapacityAsync(CancellationToken ct = default);

    /// <summary>
    /// Full per-seat map of one sector for an event: every real seat position (row/number) with
    /// its actual occupancy — free, single-match sold, or held by a season pass. Returns null when
    /// the event or sector is unknown. Read-only (never allocates seats or backing sections).
    /// </summary>
    Task<SectorSeatMapDto?> GetSectorSeatMapAsync(string externalEventId, string sectorCode, CancellationToken ct = default);

    /// <summary>
    /// The ticket occupying a specific seat (row/number) of a sector for an event, including its
    /// QR code (generated on demand if not yet materialised). Returns null when the event, sector,
    /// or seat has no active ticket.
    /// </summary>
    Task<SeatTicketDto?> GetSeatTicketAsync(string externalEventId, string sectorCode, int row, int seatNumber, CancellationToken ct = default);

    /// <summary>
    /// Generates the per-event access tickets for every active season pass in the given event's
    /// season (used when an event is created in / linked to a season from the Admin UI). Returns
    /// how many were created.
    /// </summary>
    Task<int> BackfillSeasonTicketsForEventAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Testing hook: places a randomised drink order against a currently-occupied seat of a
    /// <em>live</em> event (rejected unless <see cref="EventLifecycle.CanOrderDrinks"/> is true),
    /// so the external system/simulator can exercise the game-day drink-ordering flow and light up
    /// the Bar/Staff order queue. Broadcasts the new order over SignalR like a real customer order.
    /// </summary>
    Task<SimulatedDrinkOrderResult> SimulateDrinkOrderAsync(string externalEventId, CancellationToken ct = default);

    /// <summary>
    /// Testing hook for the admin Stadium Overview: fills <paramref name="numberOfTickets"/> seats of an
    /// event with simulated sold tickets. Seats are allocated from the drawing-tool overlays (the real
    /// stadium, source of truth) so each ticket's <see cref="Ticket.Section"/> is a genuine overlay
    /// <see cref="StadiumSectorOverlay.SectorCode"/> — which lets the admin ticket-detail blueprint
    /// locator resolve the seat instead of showing "location not available".
    /// </summary>
    Task<SimulatedTicketSalesResult> SimulateTicketSalesAsync(int eventId, int numberOfTickets, decimal basePrice, CancellationToken ct = default);
}

/// <summary>
/// Anti-corruption layer that ingests ticket-sales events from the external ticketing
/// system (the simulator during development) and maps them onto our Event/Seat/Ticket
/// domain. Processing is idempotent (see <see cref="IntegrationInboxEntry"/> +
/// unique External*Id indexes) and pushes live occupancy updates over SignalR.
/// </summary>
public class TicketIngestionService : ITicketIngestionService
{
    private readonly ApplicationDbContext _context;
    private readonly IHubContext<BartenderHub> _hub;
    private readonly IAnalyticsService _analytics;
    private readonly IQRCodeService _qrCode;
    private readonly IOrderService _orderService;
    private readonly IAccountProvisioningService _accountProvisioning;
    private readonly ILogger<TicketIngestionService> _logger;

    public TicketIngestionService(
        ApplicationDbContext context,
        IHubContext<BartenderHub> hub,
        IAnalyticsService analytics,
        IQRCodeService qrCode,
        IOrderService orderService,
        IAccountProvisioningService accountProvisioning,
        ILogger<TicketIngestionService> logger)
    {
        _context = context;
        _hub = hub;
        _analytics = analytics;
        _qrCode = qrCode;
        _orderService = orderService;
        _accountProvisioning = accountProvisioning;
        _logger = logger;
    }

    public async Task<TicketingWebhookResult> ProcessWebhookAsync(TicketingWebhookEnvelope envelope, CancellationToken ct = default)
    {
        if (envelope == null)
            return Rejected("Empty payload");
        if (string.IsNullOrWhiteSpace(envelope.EventType))
            return Rejected("Missing eventType");
        if (string.IsNullOrWhiteSpace(envelope.IdempotencyKey))
            return Rejected("Missing idempotencyKey");

        // Idempotency: skip anything we've already processed.
        var alreadyProcessed = await _context.IntegrationInboxEntries
            .AnyAsync(e => e.IdempotencyKey == envelope.IdempotencyKey, ct);
        if (alreadyProcessed)
        {
            _logger.LogInformation("Duplicate webhook ignored (idempotencyKey={Key}, type={Type})",
                envelope.IdempotencyKey, envelope.EventType);
            return new TicketingWebhookResult
            {
                Accepted = true,
                Duplicate = true,
                Message = "Already processed"
            };
        }

        try
        {
            return envelope.EventType switch
            {
                TicketingEventTypes.EventCreated or TicketingEventTypes.EventUpdated
                    => await HandleEventUpsertAsync(envelope, ct),
                TicketingEventTypes.EventWentLive
                    => await HandleEventWentLiveAsync(envelope, ct),
                TicketingEventTypes.EventEnded
                    => await HandleEventEndedAsync(envelope, ct),
                TicketingEventTypes.TicketSold
                    => await HandleTicketSoldAsync(envelope, ct),
                TicketingEventTypes.TicketRefunded
                    => await HandleTicketRefundedAsync(envelope, ct),
                TicketingEventTypes.SeasonCreated or TicketingEventTypes.SeasonUpdated
                    => await HandleSeasonUpsertAsync(envelope, ct),
                TicketingEventTypes.SeasonTicketSold
                    => await HandleSeasonTicketSoldAsync(envelope, ct),
                TicketingEventTypes.SeasonTicketRefunded
                    => await HandleSeasonTicketRefundedAsync(envelope, ct),
                _ => Rejected($"Unknown eventType '{envelope.EventType}'")
            };
        }
        catch (DbUpdateException ex)
        {
            // Most likely a concurrent duplicate hit the unique index — treat as duplicate.
            _logger.LogWarning(ex, "DbUpdateException processing webhook {Key}; treating as duplicate", envelope.IdempotencyKey);
            return new TicketingWebhookResult { Accepted = true, Duplicate = true, Message = "Already processed (concurrent)" };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing webhook {Key} ({Type})", envelope.IdempotencyKey, envelope.EventType);
            return Rejected($"Processing error: {ex.Message}");
        }
    }

    private async Task<TicketingWebhookResult> HandleEventUpsertAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.Event;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalEventId))
            return Rejected("Missing event payload");

        var evt = await _context.Events.FirstOrDefaultAsync(e => e.ExternalEventId == dto.ExternalEventId, ct);
        var totalSeats = dto.TotalSeats > 0 ? dto.TotalSeats : await GetStadiumCapacityOrDefaultAsync(ct);

        // Resolve the season this event belongs to (if the external system supplied one).
        Season? season = null;
        if (!string.IsNullOrWhiteSpace(dto.ExternalSeasonId))
            season = await ResolveOrCreateSeasonAsync(dto.ExternalSeasonId, null, null, null, false, envelope.SourceSystem, ct);
        var wasLinkedToSeason = evt?.SeasonId != null;

        if (evt == null)
        {
            var startUtc = EnsureUtc(dto.EventDate);
            var endUtc = dto.EventEndDate.HasValue ? EnsureUtc(dto.EventEndDate.Value) : (DateTime?)null;

            // An event ingested with a window that has already elapsed is historical — create it as
            // Completed (past) rather than OnSale, so a back-dated fixture never starts life "sellable"
            // and stuck. Mirrors Event.IsLiveAt's end-of-window rule (explicit end, else end of start day).
            var now = DateTime.UtcNow;
            var alreadyEnded = endUtc.HasValue ? endUtc.Value < now : startUtc.Date < now.Date;

            evt = new Event
            {
                ExternalEventId = dto.ExternalEventId,
                SourceSystem = envelope.SourceSystem,
                EventName = string.IsNullOrWhiteSpace(dto.EventName) ? $"External Event {dto.ExternalEventId}" : dto.EventName,
                EventType = string.IsNullOrWhiteSpace(dto.EventType) ? "Football" : dto.EventType,
                HomeTeam = string.IsNullOrWhiteSpace(dto.HomeTeam) ? null : dto.HomeTeam.Trim(),
                AwayTeam = string.IsNullOrWhiteSpace(dto.AwayTeam) ? null : dto.AwayTeam.Trim(),
                EventDate = startUtc,
                EventEndDate = endUtc,
                TotalSeats = totalSeats,
                BaseTicketPrice = dto.BaseTicketPrice,
                Description = dto.Description,
                SeasonId = season?.Id,
                IsActive = !alreadyEnded,
                // Future/current events are sellable (OnSale) so ingested sales apply; past events land as Completed.
                Status = alreadyEnded ? EventStatus.Completed : EventStatus.OnSale,
                CreatedAt = DateTime.UtcNow
            };
            _context.Events.Add(evt);
        }
        else
        {
            evt.EventName = string.IsNullOrWhiteSpace(dto.EventName) ? evt.EventName : dto.EventName;
            evt.EventType = string.IsNullOrWhiteSpace(dto.EventType) ? evt.EventType : dto.EventType;
            evt.HomeTeam = string.IsNullOrWhiteSpace(dto.HomeTeam) ? evt.HomeTeam : dto.HomeTeam.Trim();
            evt.AwayTeam = string.IsNullOrWhiteSpace(dto.AwayTeam) ? evt.AwayTeam : dto.AwayTeam.Trim();
            evt.EventDate = EnsureUtc(dto.EventDate);
            if (dto.EventEndDate.HasValue)
                evt.EventEndDate = EnsureUtc(dto.EventEndDate.Value);
            evt.TotalSeats = totalSeats;
            evt.BaseTicketPrice = dto.BaseTicketPrice ?? evt.BaseTicketPrice;
            evt.Description = dto.Description ?? evt.Description;
            if (season != null)
                evt.SeasonId = season.Id;
            evt.UpdatedAt = DateTime.UtcNow;
        }

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        // If this event is (now) part of a season, extend every active season pass to cover it
        // by generating a derived access ticket per pass seat. Only do this when the link is new
        // to avoid redundant work on repeated updates (the generator is idempotent regardless).
        var derivedCreated = 0;
        if (evt.SeasonId != null && !wasLinkedToSeason)
        {
            derivedCreated = await MaterializeSeasonTicketsForEventAsync(evt, ct);
            if (derivedCreated > 0)
                await BroadcastAndRefreshAsync(evt, null, "Sold", new TicketingWebhookResult(), ct);
        }

        _logger.LogInformation("Ingested {Type} for external event {ExternalId} -> internal event {EventId} (season={SeasonId}, derivedTickets={Derived})",
            envelope.EventType, dto.ExternalEventId, evt.Id, evt.SeasonId, derivedCreated);

        return new TicketingWebhookResult
        {
            Accepted = true,
            Message = envelope.EventType,
            EventId = evt.Id,
            SeasonId = evt.SeasonId,
            DerivedTicketsAffected = derivedCreated,
            TotalSoldForEvent = await CountEventSoldAsync(evt.Id, ct)
        };
    }

    /// <summary>
    /// Testing hook: slides the event window to "now" (preserving its original duration) and
    /// advances the lifecycle to <see cref="EventStatus.Active"/> so features gated on
    /// <see cref="EventLifecycle.CanOrderDrinks"/> (drink ordering, live ticket sessions) unlock.
    /// The simulator uses this to make a Future event testable as if it were live.
    /// </summary>
    private async Task<TicketingWebhookResult> HandleEventWentLiveAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.Event;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalEventId))
            return Rejected("Missing event payload");

        var evt = await _context.Events.FirstOrDefaultAsync(e => e.ExternalEventId == dto.ExternalEventId, ct);
        if (evt == null)
            return Rejected($"No event mapped to external id '{dto.ExternalEventId}'");

        if (EventLifecycle.IsTerminal(evt.Status))
            return Rejected($"Event is {evt.Status} (terminal) and cannot be made live.");

        var now = DateTime.UtcNow;

        // Slide the window so the event reads as happening right now, keeping its original length.
        var window = (evt.EventEndDate ?? evt.EventDate.AddHours(2)) - evt.EventDate;
        if (window <= TimeSpan.Zero) window = TimeSpan.FromHours(2);
        evt.EventDate = now;
        evt.EventEndDate = now.Add(window);

        // Advance the lifecycle to Active via legal transitions (e.g. Planned → OnSale → Active).
        if (evt.Status == EventStatus.Planned && EventLifecycle.CanTransition(evt.Status, EventStatus.OnSale))
            evt.Status = EventStatus.OnSale;
        if (evt.Status is not (EventStatus.Active or EventStatus.InProgress)
            && EventLifecycle.CanTransition(evt.Status, EventStatus.Active))
            evt.Status = EventStatus.Active;

        evt.IsActive = true;
        evt.UpdatedAt = now;

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Event {ExternalId} -> internal {EventId} made live: status={Status}, window {Start:o}..{End:o}",
            dto.ExternalEventId, evt.Id, evt.Status, evt.EventDate, evt.EventEndDate);

        return new TicketingWebhookResult
        {
            Accepted = true,
            Message = $"{envelope.EventType}:{evt.Status}",
            EventId = evt.Id,
            TotalSoldForEvent = await CountEventSoldAsync(evt.Id, ct)
        };
    }

    /// <summary>
    /// Counterpart to <see cref="HandleEventWentLiveAsync"/>: ends a live event by closing its window
    /// at "now" and moving its lifecycle to <see cref="EventStatus.Completed"/>. Idempotent — a repeat
    /// for an already-terminal event is accepted as a no-op rather than rejected.
    /// </summary>
    private async Task<TicketingWebhookResult> HandleEventEndedAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.Event;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalEventId))
            return Rejected("Missing event payload");

        var evt = await _context.Events.FirstOrDefaultAsync(e => e.ExternalEventId == dto.ExternalEventId, ct);
        if (evt == null)
            return Rejected($"No event mapped to external id '{dto.ExternalEventId}'");

        // Already finished/cancelled — nothing to do. Accept so a duplicate/late signal is harmless.
        if (EventLifecycle.IsTerminal(evt.Status))
        {
            RecordInbox(envelope);
            await _context.SaveChangesAsync(ct);
            return new TicketingWebhookResult
            {
                Accepted = true,
                Message = $"Already {evt.Status}",
                EventId = evt.Id,
                TotalSoldForEvent = await CountEventSoldAsync(evt.Id, ct)
            };
        }

        if (!EventLifecycle.CanTransition(evt.Status, EventStatus.Completed))
            return Rejected($"Event is {evt.Status} and cannot be ended — it must be live (Active/InProgress) first.");

        var now = DateTime.UtcNow;
        evt.Status = EventStatus.Completed;
        // Close the window at "now" so it reads as finished immediately (never leave an end in the future).
        if (!evt.EventEndDate.HasValue || evt.EventEndDate.Value > now)
            evt.EventEndDate = now;
        evt.IsActive = false; // no longer live/sellable
        evt.UpdatedAt = now;

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        // Close out any still-in-flight drink orders so none lingers (e.g. as OutForDelivery) after the
        // event has ended. Runs after the status commit because the wallet refunds inside the sweep clear
        // EF's change tracker. Mirrors the manual/auto path in EventService.TransitionEventStatusAsync.
        var cancelledOrders = await _orderService.CancelOpenOrdersForEventAsync(
            evt.Id, $"Auto-cancelled: event ended (event #{evt.Id})");
        if (cancelledOrders > 0)
            _logger.LogInformation("Cancelled {Count} in-flight order(s) while ending event {EventId}",
                cancelledOrders, evt.Id);

        _logger.LogInformation("Event {ExternalId} -> internal {EventId} ended: status=Completed, endedAt={Now:o}",
            dto.ExternalEventId, evt.Id, now);

        return new TicketingWebhookResult
        {
            Accepted = true,
            Message = $"{envelope.EventType}:{evt.Status}",
            EventId = evt.Id,
            TotalSoldForEvent = await CountEventSoldAsync(evt.Id, ct)
        };
    }

    private async Task<TicketingWebhookResult> HandleTicketSoldAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.Ticket;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalTicketId) || string.IsNullOrWhiteSpace(dto.ExternalEventId))
            return Rejected("Missing ticket payload");
        if (string.IsNullOrWhiteSpace(dto.SectionCode))
            return Rejected("Missing sectionCode");

        // Second idempotency guard: a ticket with this external id already exists.
        var existing = await _context.Tickets.AnyAsync(t => t.ExternalTicketId == dto.ExternalTicketId, ct);
        if (existing)
            return new TicketingWebhookResult { Accepted = true, Duplicate = true, Message = "Ticket already ingested" };

        // Resolve (or lazily create) the event — tolerates out-of-order delivery.
        var evt = await ResolveOrCreateEventAsync(dto.ExternalEventId, envelope.SourceSystem, ct);

        // Lifecycle + window gate: tickets may only be sold while the event is on sale AND inside its
        // configured ticket-sales window. Once it goes live (Active/InProgress), is sold out, has
        // ended, or the sales window has closed/not opened, reject the webhook so the external system
        // stops selling into it.
        if (!evt.AreTicketSalesOpenAt(DateTime.UtcNow))
            return Rejected(evt.TicketSalesBlockedReason(DateTime.UtcNow) ?? "Ticket sales are closed.");

        // The "real" stadium is defined by the drawing-tool overlays (source of truth).
        var overlay = await _context.StadiumSectorOverlays
            .FirstOrDefaultAsync(o => o.SectorCode == dto.SectionCode && !o.IsDeleted, ct);
        if (overlay == null)
            return Rejected($"Unknown sector '{dto.SectionCode}'");

        // Tickets bind to a Seat/StadiumSection, so map the overlay to a backing section (created on demand).
        var section = await ResolveBackingSectionAsync(overlay, ct);
        var capacity = overlay.TotalSeats;

        var seat = await AllocateSeatAsync(section, evt.Id, capacity, ct);
        if (seat == null)
            return Rejected($"Sector '{dto.SectionCode}' is full");

        var ticket = new Ticket
        {
            TicketNumber = BuildTicketNumber(dto.ExternalTicketId),
            EventId = evt.Id,
            Seat = seat, // navigation set so a possibly-new Seat is inserted in the same SaveChanges
            QRCode = string.Empty,
            QRCodeToken = Guid.NewGuid().ToString(),
            CustomerName = dto.CustomerName,
            CustomerEmail = dto.CustomerEmail,
            CustomerOib = dto.CustomerOib,
            CustomerDocumentNumber = dto.CustomerDocumentNumber,
            Price = dto.Price,
            PurchaseDate = EnsureUtc(dto.SoldAt),
            Status = TicketStatuses.Active,
            ExternalTicketId = dto.ExternalTicketId,
            SourceSystem = envelope.SourceSystem,
            SeatNumber = seat.SeatNumber.ToString(),
            Section = section.SectionCode,
            Row = seat.RowNumber.ToString(),
            EventName = evt.EventName,
            EventDate = evt.EventDate,
            IsActive = true
        };
        _context.Tickets.Add(ticket);

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        // Give the buyer a claimable account so they can load a wallet / be topped up at the bar.
        await _accountProvisioning.EnsureShellAccountAsync(dto.CustomerEmail, dto.CustomerName, null, "TicketSold", oib: dto.CustomerOib);

        var result = new TicketingWebhookResult
        {
            Accepted = true,
            Message = "Ticket sold",
            EventId = evt.Id,
            TicketId = ticket.Id
        };

        await BroadcastAndRefreshAsync(evt, section, "Sold", result, ct);
        return result;
    }

    private async Task<TicketingWebhookResult> HandleTicketRefundedAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.Ticket;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalTicketId))
            return Rejected("Missing ticket payload");

        var ticket = await _context.Tickets
            .Include(t => t.Seat)
            .FirstOrDefaultAsync(t => t.ExternalTicketId == dto.ExternalTicketId, ct);
        if (ticket == null)
            return Rejected("Ticket not found for refund");

        if (string.Equals(ticket.Status, TicketStatuses.Cancelled, StringComparison.OrdinalIgnoreCase))
            return new TicketingWebhookResult { Accepted = true, Duplicate = true, Message = "Ticket already refunded" };

        ticket.Status = TicketStatuses.Cancelled;
        ticket.IsActive = false;

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        var evt = await _context.Events.FirstAsync(e => e.Id == ticket.EventId, ct);
        var section = ticket.Seat != null
            ? await _context.StadiumSections.FirstOrDefaultAsync(s => s.Id == ticket.Seat.SectionId, ct)
            : null;

        var result = new TicketingWebhookResult
        {
            Accepted = true,
            Message = "Ticket refunded",
            EventId = evt.Id,
            TicketId = ticket.Id
        };

        await BroadcastAndRefreshAsync(evt, section, "Refunded", result, ct);
        return result;
    }

    // ---- Season / season-ticket (annual pass) ingestion -------------------------------------

    public async Task<int> BackfillSeasonTicketsForEventAsync(int eventId, CancellationToken ct = default)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, ct);
        if (evt?.SeasonId == null)
            return 0;

        var created = await MaterializeSeasonTicketsForEventAsync(evt, ct);
        if (created > 0)
            await BroadcastAndRefreshAsync(evt, null, "Sold", new TicketingWebhookResult(), ct);
        return created;
    }

    private async Task<TicketingWebhookResult> HandleSeasonUpsertAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.Season;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalSeasonId))
            return Rejected("Missing season payload");

        var season = await ResolveOrCreateSeasonAsync(
            dto.ExternalSeasonId, dto.Name, dto.StartDate, dto.EndDate, dto.IsCurrent, envelope.SourceSystem, ct);
        if (dto.IsCurrent)
            await SetCurrentSeasonAsync(season, ct);

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Ingested {Type} for external season {ExternalId} -> internal season {SeasonId}",
            envelope.EventType, dto.ExternalSeasonId, season.Id);

        return new TicketingWebhookResult { Accepted = true, Message = envelope.EventType, SeasonId = season.Id };
    }

    private async Task<TicketingWebhookResult> HandleSeasonTicketSoldAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.SeasonTicket;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalSeasonTicketId) || string.IsNullOrWhiteSpace(dto.ExternalSeasonId))
            return Rejected("Missing season ticket payload");
        if (string.IsNullOrWhiteSpace(dto.SectionCode))
            return Rejected("Missing sectionCode");

        // Idempotency: this pass was already ingested.
        var existing = await _context.SeasonTickets.AnyAsync(st => st.ExternalSeasonTicketId == dto.ExternalSeasonTicketId, ct);
        if (existing)
            return new TicketingWebhookResult { Accepted = true, Duplicate = true, Message = "Season ticket already ingested" };

        var season = await ResolveOrCreateSeasonAsync(dto.ExternalSeasonId, null, null, null, false, envelope.SourceSystem, ct);

        // The "real" stadium is defined by the drawing-tool overlays (source of truth).
        var overlay = await _context.StadiumSectorOverlays
            .FirstOrDefaultAsync(o => o.SectorCode == dto.SectionCode && !o.IsDeleted, ct);
        if (overlay == null)
            return Rejected($"Unknown sector '{dto.SectionCode}'");

        var section = await ResolveBackingSectionAsync(overlay, ct);

        // A season pass owns a fixed seat for the whole season.
        var seat = await AllocateSeasonSeatAsync(section, season.Id, overlay.TotalSeats, ct);
        if (seat == null)
            return Rejected($"Sector '{dto.SectionCode}' has no free seat for a season ticket");

        var pass = new SeasonTicket
        {
            SeasonId = season.Id,
            Seat = seat, // navigation set so a possibly-new Seat is inserted in the same SaveChanges
            SeasonTicketNumber = BuildSeasonTicketNumber(dto.ExternalSeasonTicketId),
            HolderName = dto.HolderName,
            HolderEmail = dto.HolderEmail,
            HolderOib = dto.HolderOib,
            HolderDocumentNumber = dto.HolderDocumentNumber,
            Price = dto.Price,
            Status = TicketStatuses.Active,
            PurchaseDate = EnsureUtc(dto.SoldAt),
            CreatedAt = DateTime.UtcNow,
            ExternalSeasonTicketId = dto.ExternalSeasonTicketId,
            SourceSystem = envelope.SourceSystem
        };
        _context.SeasonTickets.Add(pass);
        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct); // assigns seat.Id + pass.Id

        // Give the pass holder a claimable account (also links this pass to it by email).
        await _accountProvisioning.EnsureShellAccountAsync(dto.HolderEmail, dto.HolderName, null, "SeasonTicketSold", oib: dto.HolderOib);

        // Materialize a derived access ticket for every event already in the season.
        var events = await _context.Events.Where(e => e.SeasonId == season.Id).ToListAsync(ct);
        var affected = new List<Event>();
        foreach (var evt in events)
        {
            if (await CreateDerivedTicketAsync(pass, seat, section.SectionCode, evt, ct))
                affected.Add(evt);
        }
        if (affected.Count > 0)
            await _context.SaveChangesAsync(ct);

        // Live-update any admin currently viewing one of the affected events.
        foreach (var evt in affected)
            await BroadcastAndRefreshAsync(evt, section, "Sold", new TicketingWebhookResult(), ct);

        _logger.LogInformation("Ingested season ticket {ExternalId} -> pass {PassId}, seat {SeatId}, {Count} derived tickets",
            dto.ExternalSeasonTicketId, pass.Id, seat.Id, affected.Count);

        return new TicketingWebhookResult
        {
            Accepted = true,
            Message = "Season ticket sold",
            SeasonId = season.Id,
            SeasonTicketId = pass.Id,
            DerivedTicketsAffected = affected.Count
        };
    }

    private async Task<TicketingWebhookResult> HandleSeasonTicketRefundedAsync(TicketingWebhookEnvelope envelope, CancellationToken ct)
    {
        var dto = envelope.SeasonTicket;
        if (dto == null || string.IsNullOrWhiteSpace(dto.ExternalSeasonTicketId))
            return Rejected("Missing season ticket payload");

        var pass = await _context.SeasonTickets
            .Include(st => st.DerivedTickets)
            .FirstOrDefaultAsync(st => st.ExternalSeasonTicketId == dto.ExternalSeasonTicketId, ct);
        if (pass == null)
            return Rejected("Season ticket not found for refund");
        if (string.Equals(pass.Status, TicketStatuses.Cancelled, StringComparison.OrdinalIgnoreCase))
            return new TicketingWebhookResult { Accepted = true, Duplicate = true, Message = "Season ticket already refunded" };

        pass.Status = TicketStatuses.Cancelled;
        var affectedEventIds = new List<int>();
        foreach (var t in pass.DerivedTickets)
        {
            if (string.Equals(t.Status, TicketStatuses.Cancelled, StringComparison.OrdinalIgnoreCase))
                continue;
            t.Status = TicketStatuses.Cancelled;
            t.IsActive = false;
            affectedEventIds.Add(t.EventId);
        }

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        // Live-update affected events (occupancy went down for the pass's seat).
        var section = await _context.Seats
            .Where(s => s.Id == pass.SeatId)
            .Join(_context.StadiumSections, s => s.SectionId, sec => sec.Id, (s, sec) => sec)
            .FirstOrDefaultAsync(ct);
        var events = await _context.Events.Where(e => affectedEventIds.Contains(e.Id)).ToListAsync(ct);
        foreach (var evt in events)
            await BroadcastAndRefreshAsync(evt, section, "Refunded", new TicketingWebhookResult(), ct);

        _logger.LogInformation("Refunded season ticket {ExternalId} (pass {PassId}); cancelled {Count} derived tickets",
            dto.ExternalSeasonTicketId, pass.Id, affectedEventIds.Count);

        return new TicketingWebhookResult
        {
            Accepted = true,
            Message = "Season ticket refunded",
            SeasonId = pass.SeasonId,
            SeasonTicketId = pass.Id,
            DerivedTicketsAffected = affectedEventIds.Count
        };
    }

    private async Task<Season> ResolveOrCreateSeasonAsync(
        string externalSeasonId, string? name, DateTime? start, DateTime? end, bool isCurrent, string sourceSystem, CancellationToken ct)
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(s => s.ExternalSeasonId == externalSeasonId, ct);
        if (season != null)
        {
            // Patch descriptive fields when supplied (SeasonUpdated / a richer create arriving later).
            if (!string.IsNullOrWhiteSpace(name)) season.Name = name;
            if (start.HasValue) season.StartDate = EnsureUtc(start.Value);
            if (end.HasValue) season.EndDate = EnsureUtc(end.Value);
            if (!string.IsNullOrWhiteSpace(name) || start.HasValue || end.HasValue)
                season.UpdatedAt = DateTime.UtcNow;
            return season;
        }

        season = new Season
        {
            ExternalSeasonId = externalSeasonId,
            SourceSystem = sourceSystem,
            Name = string.IsNullOrWhiteSpace(name) ? $"Season {externalSeasonId}" : name,
            StartDate = start.HasValue ? EnsureUtc(start.Value) : DateTime.UtcNow,
            EndDate = end.HasValue ? EnsureUtc(end.Value) : DateTime.UtcNow.AddYears(1),
            IsCurrent = isCurrent,
            CreatedAt = DateTime.UtcNow
        };
        _context.Seasons.Add(season);
        await _context.SaveChangesAsync(ct); // assign season.Id for immediate linking
        return season;
    }

    private async Task SetCurrentSeasonAsync(Season season, CancellationToken ct)
    {
        var others = await _context.Seasons.Where(s => s.IsCurrent && s.Id != season.Id).ToListAsync(ct);
        foreach (var o in others)
            o.IsCurrent = false;
        season.IsCurrent = true;
    }

    /// <summary>
    /// Generates a derived access <see cref="Ticket"/> (kind <see cref="TicketKind.Season"/>)
    /// for one event from a season pass, unless one already exists for this pass+event or the
    /// pass's seat is already occupied for that event. Adds to the context; the caller saves.
    /// Returns true when a ticket was added.
    /// </summary>
    private async Task<bool> CreateDerivedTicketAsync(SeasonTicket pass, Seat seat, string sectionCode, Event evt, CancellationToken ct)
    {
        var alreadyDerived = await _context.Tickets.AnyAsync(t => t.SeasonTicketId == pass.Id && t.EventId == evt.Id, ct);
        if (alreadyDerived)
            return false;

        var occupied = await _context.Tickets.AnyAsync(
            t => t.EventId == evt.Id && t.SeatId == seat.Id && t.Status != TicketStatuses.Cancelled, ct);
        if (occupied)
        {
            _logger.LogWarning("Season pass {PassId} seat {SeatId} already occupied for event {EventId}; skipping derived ticket",
                pass.Id, seat.Id, evt.Id);
            return false;
        }

        _context.Tickets.Add(new Ticket
        {
            TicketNumber = $"SEA-{pass.Id}-E{evt.Id}",
            EventId = evt.Id,
            SeatId = seat.Id,
            Kind = TicketKind.Season,
            SeasonTicketId = pass.Id,
            QRCode = string.Empty,
            QRCodeToken = Guid.NewGuid().ToString(),
            CustomerName = pass.HolderName,
            CustomerEmail = pass.HolderEmail,
            CustomerOib = pass.HolderOib,
            CustomerDocumentNumber = pass.HolderDocumentNumber,
            Price = 0m, // access is paid for by the pass; per-event price is 0 to avoid double-counting revenue
            PurchaseDate = pass.PurchaseDate,
            Status = TicketStatuses.Active,
            SourceSystem = pass.SourceSystem,
            SeatNumber = seat.SeatNumber.ToString(),
            Section = sectionCode,
            Row = seat.RowNumber.ToString(),
            EventName = evt.EventName,
            EventDate = evt.EventDate,
            IsActive = true
        });
        return true;
    }

    /// <summary>
    /// Generates derived access tickets for a single event across every active season pass in the
    /// event's season (used when an event is created in / linked to a season). Saves and returns
    /// the number created.
    /// </summary>
    private async Task<int> MaterializeSeasonTicketsForEventAsync(Event evt, CancellationToken ct)
    {
        if (evt.SeasonId == null)
            return 0;

        var passes = await _context.SeasonTickets
            .Include(st => st.Seat)
            .Where(st => st.SeasonId == evt.SeasonId && st.Status != TicketStatuses.Cancelled)
            .ToListAsync(ct);
        if (passes.Count == 0)
            return 0;

        var sectionIds = passes.Where(p => p.Seat != null).Select(p => p.Seat.SectionId).Distinct().ToList();
        var sectionCodes = await _context.StadiumSections
            .Where(s => sectionIds.Contains(s.Id))
            .ToDictionaryAsync(s => s.Id, s => s.SectionCode, ct);

        var created = 0;
        foreach (var pass in passes)
        {
            if (pass.Seat == null)
                continue;
            var code = sectionCodes.TryGetValue(pass.Seat.SectionId, out var c) ? c : string.Empty;
            if (await CreateDerivedTicketAsync(pass, pass.Seat, code, evt, ct))
                created++;
        }
        if (created > 0)
            await _context.SaveChangesAsync(ct);
        return created;
    }

    /// <summary>
    /// Finds a seat in the section that is free for the whole season — not held by another active
    /// season pass and not occupied by any non-cancelled single-event ticket. Reuses such a seat
    /// first; otherwise creates the next unoccupied positional seat, respecting capacity. Returns
    /// null when the section is full.
    /// </summary>
    private async Task<Seat?> AllocateSeasonSeatAsync(StadiumSection section, int seasonId, int capacityOverride, CancellationToken ct)
    {
        var existingSeats = await _context.Seats
            .Where(s => s.SectionId == section.Id)
            .ToListAsync(ct);
        var existingIds = existingSeats.Select(s => s.Id).ToList();

        var heldSeatIds = await _context.SeasonTickets
            .Where(st => st.SeasonId == seasonId
                         && st.Status != TicketStatuses.Cancelled
                         && existingIds.Contains(st.SeatId))
            .Select(st => st.SeatId)
            .ToListAsync(ct);

        var ticketedSeatIds = await _context.Tickets
            .Where(t => t.SeatId != null
                        && t.Status != TicketStatuses.Cancelled
                        && existingIds.Contains(t.SeatId!.Value))
            .Select(t => t.SeatId!.Value)
            .ToListAsync(ct);

        var excluded = new HashSet<int>(heldSeatIds);
        excluded.UnionWith(ticketedSeatIds);

        var freeExisting = existingSeats
            .Where(s => !excluded.Contains(s.Id))
            .OrderBy(s => s.RowNumber).ThenBy(s => s.SeatNumber)
            .FirstOrDefault();
        if (freeExisting != null)
            return freeExisting;

        var occupied = new HashSet<(int, int)>(existingSeats.Select(s => (s.RowNumber, s.SeatNumber)));
        var capacity = Math.Max(capacityOverride > 0 ? capacityOverride : section.TotalRows * section.SeatsPerRow, 0);
        for (var i = 0; i < capacity; i++)
        {
            var row = i / section.SeatsPerRow + 1;
            var seatNum = i % section.SeatsPerRow + 1;
            if (occupied.Contains((row, seatNum)))
                continue;

            return new Seat
            {
                SectionId = section.Id,
                RowNumber = row,
                SeatNumber = seatNum,
                SeatCode = $"{section.SectionCode}-R{row}-S{seatNum}",
                XCoordinate = 0,
                YCoordinate = 0,
                IsAccessible = true
            };
        }

        return null; // section full
    }

    private static string BuildSeasonTicketNumber(string externalSeasonTicketId)
    {
        var candidate = $"SEA-{externalSeasonTicketId}";
        return candidate.Length <= 50 ? candidate : candidate[..50];
    }

    /// <summary>
    /// Finds a free seat in the section for this event. Reuses an existing un-ticketed seat
    /// first; otherwise creates the next unoccupied positional seat, respecting capacity.
    /// Returns null when the section is at capacity.
    /// </summary>
    private async Task<Seat?> AllocateSeatAsync(StadiumSection section, int eventId, int capacityOverride, CancellationToken ct)
    {
        var existingSeats = await _context.Seats
            .Where(s => s.SectionId == section.Id)
            .ToListAsync(ct);

        var usedSeatIds = await _context.Tickets
            .Where(t => t.EventId == eventId
                        && t.SeatId != null
                        && t.Status != TicketStatuses.Cancelled
                        && existingSeats.Select(s => s.Id).Contains(t.SeatId!.Value))
            .Select(t => t.SeatId!.Value)
            .ToListAsync(ct);
        var usedSet = new HashSet<int>(usedSeatIds);

        var freeExisting = existingSeats
            .Where(s => !usedSet.Contains(s.Id))
            .OrderBy(s => s.RowNumber).ThenBy(s => s.SeatNumber)
            .FirstOrDefault();
        if (freeExisting != null)
            return freeExisting;

        // All existing seats are taken — create a new one at the first unoccupied position.
        var occupied = new HashSet<(int, int)>(existingSeats.Select(s => (s.RowNumber, s.SeatNumber)));
        var capacity = Math.Max(capacityOverride > 0 ? capacityOverride : section.TotalRows * section.SeatsPerRow, 0);
        for (var i = 0; i < capacity; i++)
        {
            var row = i / section.SeatsPerRow + 1;
            var seatNum = i % section.SeatsPerRow + 1;
            if (occupied.Contains((row, seatNum)))
                continue;

            return new Seat
            {
                SectionId = section.Id,
                RowNumber = row,
                SeatNumber = seatNum,
                SeatCode = $"{section.SectionCode}-R{row}-S{seatNum}",
                XCoordinate = 0,
                YCoordinate = 0,
                IsAccessible = true
            };
        }

        return null; // section full
    }

    // ---- Live drink-order simulation --------------------------------------------------------

    private const string SimulatorCustomerEmail = "simulator+drinks@stadium.local";

    public async Task<SimulatedDrinkOrderResult> SimulateDrinkOrderAsync(string externalEventId, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(externalEventId))
            return new SimulatedDrinkOrderResult { Accepted = false, Message = "Missing externalEventId" };

        var evt = await _context.Events.FirstOrDefaultAsync(e => e.ExternalEventId == externalEventId, ct);
        if (evt == null)
            return new SimulatedDrinkOrderResult { Accepted = false, Message = $"No event mapped to external id '{externalEventId}'" };

        // Drink ordering is a game-day (live) feature — gate exactly like the real customer flow.
        if (!EventLifecycle.CanOrderDrinks(evt.Status))
            return new SimulatedDrinkOrderResult { Accepted = false, Message = EventLifecycle.OrderingBlockedReason(evt.Status) };

        // Attach the order to a real occupied seat in this event, so it carries a seat + section
        // just like a customer order placed from a seat.
        var occupiedSeats = await _context.Tickets
            .Where(t => t.EventId == evt.Id && t.Status != TicketStatuses.Cancelled && t.SeatId != null)
            .Select(t => new { t.SeatId, t.SeatNumber, t.TicketNumber, t.CustomerName })
            .ToListAsync(ct);
        if (occupiedSeats.Count == 0)
            return new SimulatedDrinkOrderResult { Accepted = false, Message = "No occupied seats to order from — sell some tickets first." };

        var drinks = await _context.Drinks
            .Where(d => d.IsAvailable && d.StockQuantity > 0)
            .ToListAsync(ct);
        if (drinks.Count == 0)
            return new SimulatedDrinkOrderResult { Accepted = false, Message = "No drinks available to order." };

        var seat = occupiedSeats[_rng.Next(occupiedSeats.Count)];
        var customerId = await ResolveSimulatorCustomerIdAsync(ct);

        // 1–3 distinct drinks, 1–2 of each.
        var chosen = drinks.OrderBy(_ => _rng.Next()).Take(_rng.Next(1, 4)).ToList();
        var order = new Order
        {
            TicketNumber = seat.TicketNumber,
            SeatNumber = seat.SeatNumber ?? string.Empty,
            CustomerId = customerId,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.UtcNow,
            EventId = evt.Id,
            SeatId = seat.SeatId,
            CustomerNotes = "Simulated live order"
        };

        decimal total = 0;
        var summaryParts = new List<string>();
        foreach (var drink in chosen)
        {
            var qty = Math.Min(_rng.Next(1, 3), drink.StockQuantity);
            if (qty <= 0) continue;
            order.OrderItems.Add(new OrderItem
            {
                DrinkId = drink.Id,
                Quantity = qty,
                UnitPrice = drink.Price,
                TotalPrice = drink.Price * qty
            });
            drink.StockQuantity -= qty;
            total += drink.Price * qty;
            summaryParts.Add($"{qty}× {drink.Name}");
        }

        if (order.OrderItems.Count == 0)
            return new SimulatedDrinkOrderResult { Accepted = false, Message = "Chosen drinks were out of stock." };

        order.TotalAmount = total;
        _context.Orders.Add(order);
        await _context.SaveChangesAsync(ct);

        var summary = string.Join(", ", summaryParts);
        await BroadcastNewOrderAsync(order, seat.CustomerName, summary, ct);

        _logger.LogInformation("Simulated drink order #{OrderId} for event {EventId} seat {Seat}: {Summary}",
            order.Id, evt.Id, order.SeatNumber, summary);

        return new SimulatedDrinkOrderResult
        {
            Accepted = true,
            Message = "Order placed",
            OrderId = order.Id,
            EventId = evt.Id,
            SeatNumber = order.SeatNumber,
            OrderSummary = summary,
            TotalAmount = total
        };
    }

    public async Task<SimulatedTicketSalesResult> SimulateTicketSalesAsync(int eventId, int numberOfTickets, decimal basePrice, CancellationToken ct = default)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, ct);
        if (evt == null)
            return new SimulatedTicketSalesResult { Accepted = false, Message = $"Event with ID {eventId} not found" };

        // The "real" stadium is defined by the drawing-tool overlays (source of truth). Allocating
        // simulated seats from these — instead of the legacy StadiumSeatsNew hierarchy — makes each
        // ticket's Section a genuine overlay SectorCode, so the admin blueprint locator can pin it.
        var overlays = await _context.StadiumSectorOverlays
            .Where(o => !o.IsDeleted)
            .OrderBy(o => o.Id)
            .ToListAsync(ct);
        if (overlays.Count == 0)
            return new SimulatedTicketSalesResult
            {
                Accepted = false,
                EventId = evt.Id,
                Message = "No stadium sectors have been drawn yet — draw the stadium in the drawing tool first."
            };

        var count = Math.Clamp(numberOfTickets, 1, 1000);
        var customerNames = new[] { "John Doe", "Jane Smith", "Mike Johnson", "Sarah Wilson", "Tom Brown", "Lisa Davis", "Chris Miller", "Anna Garcia" };

        var created = 0;
        // Track which simulated buyers we've already provisioned so 1000 tickets don't re-provision
        // the same 8 names — EnsureShellAccountAsync is idempotent anyway, but this avoids the wasted
        // DbContext scope per ticket.
        var provisionedEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (var i = 0; i < count; i++)
        {
            // Round-robin across sectors so the simulated sales spread over the whole stadium.
            var overlay = overlays[i % overlays.Count];
            var section = await ResolveBackingSectionAsync(overlay, ct);
            var seat = await AllocateSeatAsync(section, evt.Id, overlay.TotalSeats, ct);
            if (seat == null)
                continue; // this sector is full — skip it

            var customerName = customerNames[_rng.Next(customerNames.Length)];
            var customerEmail = $"{customerName.Replace(" ", "").ToLower()}@example.com";
            var customerOib = RandomOib();
            var ticket = new Ticket
            {
                TicketNumber = $"TK{DateTime.UtcNow.Ticks}{_rng.Next(1000, 9999)}",
                EventId = evt.Id,
                Seat = seat, // navigation set so a possibly-new Seat is inserted in the same SaveChanges
                QRCode = string.Empty,
                QRCodeToken = Guid.NewGuid().ToString(),
                CustomerName = customerName,
                CustomerEmail = customerEmail,
                CustomerPhone = $"+1-555-{_rng.Next(100, 999)}-{_rng.Next(1000, 9999)}",
                CustomerOib = customerOib,
                Price = basePrice + (decimal)(_rng.NextDouble() * 20), // some price variation
                PurchaseDate = DateTime.UtcNow.AddHours(-_rng.Next(0, 72)), // within the last 3 days
                Status = TicketStatuses.Active,
                SeatNumber = seat.SeatNumber.ToString(),
                Section = overlay.SectorCode, // real overlay code the blueprint locator matches on
                Row = seat.RowNumber.ToString(),
                EventName = evt.EventName,
                EventDate = evt.EventDate,
                IsActive = true
            };
            _context.Tickets.Add(ticket);

            // Save per ticket so the next AllocateSeatAsync (which queries the DB) sees this seat as taken.
            await _context.SaveChangesAsync(ct);
            created++;

            // Give the simulated buyer a claimable shell account too — exactly like the real webhook
            // sale path (HandleTicketSoldAsync) — so simulated fans appear under Admin → Customers and
            // can be topped up at the bar by OIB. No activation email is sent because these are throwaway
            // @example.com test addresses.
            if (provisionedEmails.Add(customerEmail))
                await _accountProvisioning.EnsureShellAccountAsync(
                    customerEmail, customerName, null, "SimulatedTicketSale", sendActivation: false, oib: customerOib);
        }

        _logger.LogInformation("Generated {Count} simulation tickets for event {EventId}", created, eventId);

        // Sold counts changed — refresh derived analytics best-effort (never fail the simulation on it).
        try
        {
            await _analytics.UpdateEventAnalyticsAsync(evt.Id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to refresh analytics after simulating sales for event {EventId}", evt.Id);
        }

        return new SimulatedTicketSalesResult
        {
            Accepted = true,
            EventId = evt.Id,
            TicketsCreated = created,
            Message = $"Generated {created} simulation tickets"
        };
    }

    /// <summary>Resolve-or-create the dedicated customer used to attribute simulated drink orders.</summary>
    private async Task<int> ResolveSimulatorCustomerIdAsync(CancellationToken ct)
    {
        var existing = await _context.Users
            .Where(u => u.Email == SimulatorCustomerEmail)
            .Select(u => u.Id)
            .FirstOrDefaultAsync(ct);
        if (existing != 0)
            return existing;

        var user = new User
        {
            Username = "Simulator Customer",
            Email = SimulatorCustomerEmail,
            // Not a real login — a non-empty placeholder just satisfies the required column.
            PasswordHash = Guid.NewGuid().ToString("N"),
            Role = UserRole.Customer,
            CreatedAt = DateTime.UtcNow
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync(ct);
        return user.Id;
    }

    /// <summary>Broadcasts a new order to staff exactly like a real customer order so the Bar/Staff queue updates live.</summary>
    private async Task BroadcastNewOrderAsync(Order order, string? customerName, string summary, CancellationToken ct)
    {
        var dto = new OrderDto
        {
            Id = order.Id,
            TicketNumber = order.TicketNumber,
            SeatNumber = order.SeatNumber,
            CustomerId = order.CustomerId,
            CustomerName = string.IsNullOrWhiteSpace(customerName) ? "Simulated customer" : customerName!,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            OrderDate = order.CreatedAt,
            CreatedAt = order.CreatedAt,
            EventId = order.EventId,
            SeatId = order.SeatId,
            CustomerNotes = order.CustomerNotes,
            OrderItems = order.OrderItems.Select(oi => new OrderItemDto
            {
                DrinkId = oi.DrinkId,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.TotalPrice
            }).ToList()
        };

        try
        {
            if (order.EventId is int eventId)
                await _hub.Clients.Group($"event-{eventId}").SendAsync("NewOrder", dto, ct);
            await _hub.Clients.Group("staff-all").SendAsync("NewOrder", dto, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to broadcast simulated NewOrder for order {OrderId}", order.Id);
        }
    }

    private readonly Random _rng = new();

    /// <summary>A random 11-digit string used as a simulated OIB for admin test tickets (format-only,
    /// no checksum — matches the capture rule used across the app).</summary>
    private string RandomOib()
    {
        var digits = new char[11];
        for (var i = 0; i < digits.Length; i++)
            digits[i] = (char)('0' + _rng.Next(0, 10));
        return new string(digits);
    }

    private async Task BroadcastAndRefreshAsync(Event evt, StadiumSection? section, string action, TicketingWebhookResult result, CancellationToken ct)
    {
        var totalSold = await CountEventSoldAsync(evt.Id, ct);
        result.TotalSoldForEvent = totalSold;

        // Total seats = the real stadium capacity (sum of drawing-tool overlay sectors).
        var totalSeats = await GetStadiumCapacityOrDefaultAsync(ct);

        var soldInSection = 0;
        var sectionCapacity = 0;
        if (section != null)
        {
            soldInSection = await CountSectionSoldAsync(evt.Id, section.Id, ct);
            sectionCapacity = section.TotalRows * section.SeatsPerRow;
            result.SoldInSection = soldInSection;
        }

        var notification = new TicketSoldNotification
        {
            EventId = evt.Id,
            ExternalEventId = evt.ExternalEventId ?? string.Empty,
            SectionCode = section?.SectionCode ?? string.Empty,
            SoldInSection = soldInSection,
            SectionCapacity = sectionCapacity,
            TotalSold = totalSold,
            TotalSeats = totalSeats,
            OccupancyPercentage = totalSeats > 0
                ? Math.Round((decimal)totalSold / totalSeats * 100, 2)
                : 0,
            Timestamp = DateTime.UtcNow,
            Action = action
        };

        try
        {
            await _hub.Clients.Group($"event-{evt.Id}").SendAsync("TicketSold", notification, ct);
            await _hub.Clients.Group("staff-all").SendAsync("TicketSold", notification, ct);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to broadcast TicketSold for event {EventId}", evt.Id);
        }

        // Derived analytics are best-effort; never fail ingestion because of them.
        try
        {
            await _analytics.UpdateEventAnalyticsAsync(evt.Id);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Failed to refresh analytics for event {EventId}", evt.Id);
        }
    }

    public async Task<EventSalesSnapshotDto?> GetEventSnapshotAsync(string externalEventId, CancellationToken ct = default)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.ExternalEventId == externalEventId, ct);
        return evt == null ? null : await BuildSnapshotAsync(evt, ct);
    }

    public async Task<EventSalesSnapshotDto?> GetEventOccupancyAsync(int eventId, CancellationToken ct = default)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, ct);
        return evt == null ? null : await BuildSnapshotAsync(evt, ct);
    }

    public async Task<(bool Linked, int? SeasonId, int DerivedCreated)> EnsureEventLinkedToCurrentSeasonAsync(int eventId, CancellationToken ct = default)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId, ct);
        if (evt == null)
            return (false, null, 0);

        // Already part of a season — nothing to link (idempotent).
        if (evt.SeasonId != null)
            return (false, evt.SeasonId, 0);

        var current = await _context.Seasons.FirstOrDefaultAsync(s => s.IsCurrent, ct);
        if (current == null)
            return (false, null, 0);

        evt.SeasonId = current.Id;
        evt.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync(ct);

        // Extend every active pass in the season to cover this event by generating a derived
        // access ticket per pass seat (same path used when an event is created into a season).
        var derived = await MaterializeSeasonTicketsForEventAsync(evt, ct);
        if (derived > 0)
            await BroadcastAndRefreshAsync(evt, null, "Sold", new TicketingWebhookResult(), ct);

        _logger.LogInformation("Linked event {EventId} to current season {SeasonId}; materialized {Derived} season-pass ticket(s)",
            evt.Id, current.Id, derived);

        return (true, current.Id, derived);
    }

    /// <summary>
    /// Builds a per-sector sales snapshot using the drawing-tool overlays (the real stadium),
    /// counting sold tickets via each overlay's backing StadiumSection.
    /// </summary>
    private async Task<EventSalesSnapshotDto> BuildSnapshotAsync(Event evt, CancellationToken ct)
    {
        var overlays = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .ToListAsync(ct);

        var snapshot = new EventSalesSnapshotDto
        {
            ExternalEventId = evt.ExternalEventId ?? string.Empty,
            EventId = evt.Id,
            GeneratedAt = DateTime.UtcNow
        };

        // Map overlay code -> backing section id (if one exists yet) in a single query.
        var codes = overlays.Select(o => o.SectorCode).ToList();
        var backing = await _context.StadiumSections
            .AsNoTracking()
            .Where(s => codes.Contains(s.SectionCode))
            .ToDictionaryAsync(s => s.SectionCode, s => s.Id, ct);

        var totalCapacity = 0;
        foreach (var overlay in overlays)
        {
            var capacity = overlay.TotalSeats;
            totalCapacity += capacity;
            var sold = 0;
            var seasonSold = 0;
            if (backing.TryGetValue(overlay.SectorCode, out var sectionId))
            {
                sold = await CountSectionSoldAsync(evt.Id, sectionId, ct);
                seasonSold = await CountSectionSeasonSoldAsync(evt.Id, sectionId, ct);
            }

            snapshot.Sectors.Add(new SectorSalesDto
            {
                SectionCode = overlay.SectorCode,
                SectionName = overlay.Name,
                Capacity = capacity,
                Sold = sold,
                SeasonSold = seasonSold
            });
        }

        snapshot.TotalSeats = totalCapacity;
        snapshot.TotalSold = await CountEventSoldAsync(evt.Id, ct);
        snapshot.TotalSeasonSold = await _context.Tickets.CountAsync(
            t => t.EventId == evt.Id && t.Status != TicketStatuses.Cancelled && t.Kind == TicketKind.Season, ct);
        return snapshot;
    }

    public async Task<SectorSeatMapDto?> GetSectorSeatMapAsync(string externalEventId, string sectorCode, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(externalEventId) || string.IsNullOrWhiteSpace(sectorCode))
            return null;

        var evt = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ExternalEventId == externalEventId, ct);
        if (evt == null)
            return null;

        // The overlay (drawing-tool sector) is the source of truth for the seat layout.
        var overlay = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.SectorCode == sectorCode && !o.IsDeleted, ct);
        if (overlay == null)
            return null;

        var map = new SectorSeatMapDto
        {
            ExternalEventId = evt.ExternalEventId ?? string.Empty,
            EventId = evt.Id,
            SectionCode = overlay.SectorCode,
            SectionName = overlay.Name,
            Rows = overlay.Rows
        };

        // Read-only resolve of the backing section (tickets bind to Seat → StadiumSection). When
        // none exists yet, nothing was ever sold/held here, so every seat is free.
        var sectionId = overlay.StadiumSectionId
            ?? await _context.StadiumSections
                .Where(s => s.SectionCode == overlay.SectorCode)
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync(ct);

        // (row, seat) → occupancy for this event, from the actual seats the API assigned.
        var occupied = new Dictionary<(int Row, int Seat), SectorSeatDto>();
        if (sectionId != null)
        {
            var held = await _context.Tickets
                .AsNoTracking()
                .Where(t => t.EventId == evt.Id
                            && t.Status != TicketStatuses.Cancelled
                            && t.SeatId != null
                            && t.Seat.SectionId == sectionId)
                .Select(t => new
                {
                    t.Seat.RowNumber,
                    t.Seat.SeatNumber,
                    t.Kind,
                    t.ExternalTicketId,
                    t.CustomerName
                })
                .ToListAsync(ct);

            foreach (var h in held)
            {
                var isSeason = h.Kind == TicketKind.Season;
                occupied[(h.RowNumber, h.SeatNumber)] = new SectorSeatDto
                {
                    Row = h.RowNumber,
                    Number = h.SeatNumber,
                    SeatCode = $"{overlay.SectorCode}-R{h.RowNumber}-S{h.SeatNumber}",
                    Status = isSeason ? SeatOccupancy.Season : SeatOccupancy.Sold,
                    ExternalTicketId = isSeason ? null : h.ExternalTicketId,
                    HolderName = h.CustomerName
                };
            }
        }

        // Walk the sector's real layout (honours variable seating), tagging each position.
        foreach (var (row, seats) in EnumerateRowLayout(overlay))
        {
            for (var seat = 1; seat <= seats; seat++)
            {
                if (occupied.Remove((row, seat), out var taken))
                {
                    map.Seats.Add(taken);
                    map.SoldSeats++;
                    if (taken.Status == SeatOccupancy.Season) map.SeasonSeats++;
                }
                else
                {
                    map.Seats.Add(new SectorSeatDto
                    {
                        Row = row,
                        Number = seat,
                        SeatCode = $"{overlay.SectorCode}-R{row}-S{seat}",
                        Status = SeatOccupancy.Free
                    });
                }
            }
        }

        // Any held seats outside the current layout (e.g. the sector's size shrank after sale) are
        // still real occupants — surface them so counts match the snapshot.
        foreach (var extra in occupied.Values.OrderBy(s => s.Row).ThenBy(s => s.Number))
        {
            map.Seats.Add(extra);
            map.SoldSeats++;
            if (extra.Status == SeatOccupancy.Season) map.SeasonSeats++;
        }

        map.TotalSeats = map.Seats.Count;
        map.FreeSeats = map.TotalSeats - map.SoldSeats;
        return map;
    }

    public async Task<SeatTicketDto?> GetSeatTicketAsync(string externalEventId, string sectorCode, int row, int seatNumber, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(externalEventId) || string.IsNullOrWhiteSpace(sectorCode))
            return null;

        var evt = await _context.Events
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.ExternalEventId == externalEventId, ct);
        if (evt == null)
            return null;

        var overlay = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.SectorCode == sectorCode && !o.IsDeleted, ct);
        if (overlay == null)
            return null;

        var sectionId = overlay.StadiumSectionId
            ?? await _context.StadiumSections
                .Where(s => s.SectionCode == overlay.SectorCode)
                .Select(s => (int?)s.Id)
                .FirstOrDefaultAsync(ct);
        if (sectionId == null)
            return null; // nothing ever sold here → no ticket at this seat

        // Tracked (not AsNoTracking): the QR service may need to generate + save the image below.
        var ticket = await _context.Tickets
            .Include(t => t.Seat)
            .FirstOrDefaultAsync(t => t.EventId == evt.Id
                                      && t.Status != TicketStatuses.Cancelled
                                      && t.SeatId != null
                                      && t.Seat.SectionId == sectionId
                                      && t.Seat.RowNumber == row
                                      && t.Seat.SeatNumber == seatNumber, ct);
        if (ticket == null)
            return null; // seat is free

        // Build the QR image for display. Ingested tickets carry a token but no stored image (the
        // QRCode column can't hold a base64 image), so we render it on demand without persisting.
        var qrDataUri = await _qrCode.GetQrImageDataUriAsync(ticket);

        return new SeatTicketDto
        {
            TicketId = ticket.Id,
            TicketNumber = ticket.TicketNumber,
            SectionCode = overlay.SectorCode,
            Row = row,
            Number = seatNumber,
            SeatCode = $"{overlay.SectorCode}-R{row}-S{seatNumber}",
            Status = ticket.Kind == TicketKind.Season ? SeatOccupancy.Season : SeatOccupancy.Sold,
            HolderName = ticket.CustomerName,
            ExternalTicketId = ticket.ExternalTicketId,
            Price = ticket.Price,
            PurchaseDate = ticket.PurchaseDate,
            EventName = ticket.EventName ?? evt.EventName,
            EventDate = ticket.EventDate ?? evt.EventDate,
            QRCode = qrDataUri,
            QRCodeToken = ticket.QRCodeToken
        };
    }

    /// <summary>Enumerates (rowNumber, seatsInRow) for a sector, honouring variable-seating patterns.</summary>
    private static IEnumerable<(int Row, int Seats)> EnumerateRowLayout(StadiumSectorOverlay overlay)
    {
        List<RowPattern>? patterns = null;
        if (overlay.UseVariableSeating && !string.IsNullOrEmpty(overlay.VariableSeatingData))
        {
            try { patterns = System.Text.Json.JsonSerializer.Deserialize<List<RowPattern>>(overlay.VariableSeatingData); }
            catch { patterns = null; }
        }

        for (var row = 1; row <= overlay.Rows; row++)
        {
            var seats = overlay.SeatsPerRow;
            var pattern = patterns?.FirstOrDefault(p => row >= p.FromRow && row <= p.ToRow);
            if (pattern != null)
                seats = pattern.SeatsPerRow;
            yield return (row, seats);
        }
    }

    /// <summary>
    /// Maps a drawing-tool overlay sector to a backing StadiumSection (tickets bind to Seat →
    /// StadiumSection). Creates the section on first use and links it back to the overlay.
    /// </summary>
    private async Task<StadiumSection> ResolveBackingSectionAsync(StadiumSectorOverlay overlay, CancellationToken ct)
    {
        if (overlay.StadiumSectionId is int sid)
        {
            var linked = await _context.StadiumSections.FirstOrDefaultAsync(s => s.Id == sid, ct);
            if (linked != null)
                return linked;
        }

        var existing = await _context.StadiumSections.FirstOrDefaultAsync(s => s.SectionCode == overlay.SectorCode, ct);
        if (existing != null)
        {
            if (overlay.StadiumSectionId == null)
            {
                overlay.StadiumSectionId = existing.Id;
                await _context.SaveChangesAsync(ct);
            }
            return existing;
        }

        var section = new StadiumSection
        {
            SectionCode = overlay.SectorCode,
            SectionName = overlay.Name,
            TotalRows = overlay.Rows,
            SeatsPerRow = overlay.SeatsPerRow,
            PriceMultiplier = 1.0m,
            IsActive = true,
            Color = overlay.Color
        };
        _context.StadiumSections.Add(section);
        await _context.SaveChangesAsync(ct);

        overlay.StadiumSectionId = section.Id;
        await _context.SaveChangesAsync(ct);
        return section;
    }

    private async Task<Event> ResolveOrCreateEventAsync(string externalEventId, string sourceSystem, CancellationToken ct)
    {
        var evt = await _context.Events.FirstOrDefaultAsync(e => e.ExternalEventId == externalEventId, ct);
        if (evt != null)
            return evt;

        _logger.LogWarning("Sold ticket for unknown external event {ExternalId}; creating placeholder", externalEventId);
        evt = new Event
        {
            ExternalEventId = externalEventId,
            SourceSystem = sourceSystem,
            EventName = $"External Event {externalEventId}",
            EventType = "Football",
            EventDate = DateTime.UtcNow,
            TotalSeats = await GetStadiumCapacityOrDefaultAsync(ct),
            IsActive = true,
            Status = EventStatus.OnSale,
            CreatedAt = DateTime.UtcNow
        };
        _context.Events.Add(evt);
        await _context.SaveChangesAsync(ct);
        return evt;
    }

    private Task<int> CountEventSoldAsync(int eventId, CancellationToken ct) =>
        _context.Tickets.CountAsync(t => t.EventId == eventId && t.Status != TicketStatuses.Cancelled, ct);

    private async Task<int> CountSectionSoldAsync(int eventId, int sectionId, CancellationToken ct)
    {
        var sectionSeatIds = _context.Seats.Where(s => s.SectionId == sectionId).Select(s => s.Id);
        return await _context.Tickets.CountAsync(
            t => t.EventId == eventId
                 && t.Status != TicketStatuses.Cancelled
                 && t.SeatId != null
                 && sectionSeatIds.Contains(t.SeatId!.Value), ct);
    }

    private async Task<int> CountSectionSeasonSoldAsync(int eventId, int sectionId, CancellationToken ct)
    {
        var sectionSeatIds = _context.Seats.Where(s => s.SectionId == sectionId).Select(s => s.Id);
        return await _context.Tickets.CountAsync(
            t => t.EventId == eventId
                 && t.Status != TicketStatuses.Cancelled
                 && t.Kind == TicketKind.Season
                 && t.SeatId != null
                 && sectionSeatIds.Contains(t.SeatId!.Value), ct);
    }

    public async Task<int> GetStadiumCapacityAsync(CancellationToken ct = default)
    {
        // Real stadium capacity = sum of the drawing-tool overlay sectors (TotalSeats is computed,
        // incl. variable seating, so sum in memory rather than in SQL). Returns 0 when no overlay
        // stadium has been drawn yet.
        var overlays = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .ToListAsync(ct);
        return overlays.Sum(o => o.TotalSeats);
    }

    /// <summary>
    /// Stadium capacity for ingestion placeholders — falls back to a sane 1000 when no overlay
    /// stadium exists yet, so auto-created events aren't seeded with a zero capacity.
    /// </summary>
    private async Task<int> GetStadiumCapacityOrDefaultAsync(CancellationToken ct)
    {
        var cap = await GetStadiumCapacityAsync(ct);
        return cap > 0 ? cap : 1000;
    }

    private void RecordInbox(TicketingWebhookEnvelope envelope) =>
        _context.IntegrationInboxEntries.Add(new IntegrationInboxEntry
        {
            IdempotencyKey = envelope.IdempotencyKey,
            EventType = envelope.EventType,
            SourceSystem = envelope.SourceSystem ?? string.Empty,
            ReceivedAt = DateTime.UtcNow,
            Status = "Processed"
        });

    private static string BuildTicketNumber(string externalTicketId)
    {
        var candidate = $"EXT-{externalTicketId}";
        return candidate.Length <= 50 ? candidate : candidate[..50];
    }

    private static DateTime EnsureUtc(DateTime value) => value.Kind switch
    {
        DateTimeKind.Utc => value,
        DateTimeKind.Local => value.ToUniversalTime(),
        _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
    };

    private static TicketingWebhookResult Rejected(string message) =>
        new() { Accepted = false, Message = message };
}
