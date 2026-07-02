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

        if (evt == null)
        {
            evt = new Event
            {
                ExternalEventId = dto.ExternalEventId,
                SourceSystem = envelope.SourceSystem,
                EventName = string.IsNullOrWhiteSpace(dto.EventName) ? $"External Event {dto.ExternalEventId}" : dto.EventName,
                EventType = string.IsNullOrWhiteSpace(dto.EventType) ? "Football" : dto.EventType,
                EventDate = EnsureUtc(dto.EventDate),
                TotalSeats = totalSeats,
                BaseTicketPrice = dto.BaseTicketPrice,
                Description = dto.Description,
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
            evt.TotalSeats = totalSeats;
            evt.BaseTicketPrice = dto.BaseTicketPrice ?? evt.BaseTicketPrice;
            evt.Description = dto.Description ?? evt.Description;
            evt.UpdatedAt = DateTime.UtcNow;
        }

        RecordInbox(envelope);
        await _context.SaveChangesAsync(ct);

        _logger.LogInformation("Ingested {Type} for external event {ExternalId} -> internal event {EventId}",
            envelope.EventType, dto.ExternalEventId, evt.Id);

        return new TicketingWebhookResult
        {
            Accepted = true,
            Message = envelope.EventType,
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
            var sold = backing.TryGetValue(overlay.SectorCode, out var sectionId)
                ? await CountSectionSoldAsync(evt.Id, sectionId, ct)
                : 0;

            snapshot.Sectors.Add(new SectorSalesDto
            {
                SectionCode = overlay.SectorCode,
                SectionName = overlay.Name,
                Capacity = capacity,
                Sold = sold
            });
        }

        snapshot.TotalSeats = totalCapacity;
        snapshot.TotalSold = await CountEventSoldAsync(evt.Id, ct);
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
