using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.API.Hubs;
using StadiumDrinkOrdering.Shared.DTOs.Integration;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface ITicketIngestionService
{
    Task<TicketingWebhookResult> ProcessWebhookAsync(TicketingWebhookEnvelope envelope, CancellationToken ct = default);
    Task<EventSalesSnapshotDto?> GetEventSnapshotAsync(string externalEventId, CancellationToken ct = default);
    Task<EventSalesSnapshotDto?> GetEventOccupancyAsync(int eventId, CancellationToken ct = default);

    /// <summary>
    /// Generates the per-event access tickets for every active season pass in the given event's
    /// season (used when an event is created in / linked to a season from the Admin UI). Returns
    /// how many were created.
    /// </summary>
    Task<int> BackfillSeasonTicketsForEventAsync(int eventId, CancellationToken ct = default);
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
    private readonly ILogger<TicketIngestionService> _logger;

    public TicketIngestionService(
        ApplicationDbContext context,
        IHubContext<BartenderHub> hub,
        IAnalyticsService analytics,
        ILogger<TicketIngestionService> logger)
    {
        _context = context;
        _hub = hub;
        _analytics = analytics;
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
        var totalSeats = dto.TotalSeats > 0 ? dto.TotalSeats : await GetStadiumCapacityAsync(ct);

        // Resolve the season this event belongs to (if the external system supplied one).
        Season? season = null;
        if (!string.IsNullOrWhiteSpace(dto.ExternalSeasonId))
            season = await ResolveOrCreateSeasonAsync(dto.ExternalSeasonId, null, null, null, false, envelope.SourceSystem, ct);
        var wasLinkedToSeason = evt?.SeasonId != null;

        if (evt == null)
        {
            evt = new Event
            {
                ExternalEventId = dto.ExternalEventId,
                SourceSystem = envelope.SourceSystem,
                EventName = string.IsNullOrWhiteSpace(dto.EventName) ? $"External Event {dto.ExternalEventId}" : dto.EventName,
                EventType = string.IsNullOrWhiteSpace(dto.EventType) ? "Football" : dto.EventType,
                EventDate = EnsureUtc(dto.EventDate),
                EventEndDate = dto.EventEndDate.HasValue ? EnsureUtc(dto.EventEndDate.Value) : null,
                TotalSeats = totalSeats,
                BaseTicketPrice = dto.BaseTicketPrice,
                Description = dto.Description,
                SeasonId = season?.Id,
                IsActive = true,
                Status = EventStatus.OnSale, // externally-created events are sellable so ingested sales apply
                CreatedAt = DateTime.UtcNow
            };
            _context.Events.Add(evt);
        }
        else
        {
            evt.EventName = string.IsNullOrWhiteSpace(dto.EventName) ? evt.EventName : dto.EventName;
            evt.EventType = string.IsNullOrWhiteSpace(dto.EventType) ? evt.EventType : dto.EventType;
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

    private async Task BroadcastAndRefreshAsync(Event evt, StadiumSection? section, string action, TicketingWebhookResult result, CancellationToken ct)
    {
        var totalSold = await CountEventSoldAsync(evt.Id, ct);
        result.TotalSoldForEvent = totalSold;

        // Total seats = the real stadium capacity (sum of drawing-tool overlay sectors).
        var totalSeats = await GetStadiumCapacityAsync(ct);

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
            TotalSeats = await GetStadiumCapacityAsync(ct),
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

    private async Task<int> GetStadiumCapacityAsync(CancellationToken ct)
    {
        // Real stadium capacity = sum of the drawing-tool overlay sectors (TotalSeats is computed,
        // incl. variable seating, so sum in memory rather than in SQL).
        var overlays = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .ToListAsync(ct);
        var cap = overlays.Sum(o => o.TotalSeats);
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
