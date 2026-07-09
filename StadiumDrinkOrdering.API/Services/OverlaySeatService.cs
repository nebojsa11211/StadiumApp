using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Seat availability + resolution for the customer purchase flow over the REAL stadium
/// (the drawing-tool overlays, SECT1/2/3…). This is the same overlay → backing
/// <see cref="StadiumSection"/> → <see cref="Seat"/> → <see cref="Ticket"/> model that the
/// external/season ingestion uses, so seats held by a season pass (or any non-cancelled
/// ticket) are counted as taken here automatically. All "sold" checks funnel through
/// <see cref="TicketStatuses.CountsAsSold"/> via a non-cancelled filter.
///
/// NOTE: the customer flow addresses a sector by its OVERLAY id (StadiumSectorOverlay.Id),
/// carried through the existing DTOs' <c>SectorId</c> field.
/// </summary>
public interface IOverlaySeatService
{
    Task<StadiumSectorOverlay?> GetOverlayAsync(int overlaySectorId, CancellationToken ct = default);

    /// <summary>Per-overlay sold/available summary for an event, keyed by overlay id.</summary>
    Task<Dictionary<int, OverlaySectionSummary>> GetSectionSummariesAsync(int eventId, CancellationToken ct = default);

    /// <summary>Bookable (not sold, not reserved) seats in an overlay sector for an event.</summary>
    Task<List<OverlaySeatInfo>> GetAvailableSeatsAsync(int eventId, int overlaySectorId, CancellationToken ct = default);

    /// <summary>Every seat in an overlay sector for an event, each flagged available/taken so the
    /// full stand can be drawn (sold + reserved seats come back with <c>IsAvailable = false</c>).</summary>
    Task<List<OverlaySeatInfo>> GetAllSeatsAsync(int eventId, int overlaySectorId, CancellationToken ct = default);

    /// <summary>True when a non-cancelled ticket (incl. a season pass) already occupies the seat for the event.</summary>
    Task<bool> IsSeatSoldAsync(int eventId, int overlaySectorId, int rowNumber, int seatNumber, CancellationToken ct = default);

    /// <summary>Resolves (creating on demand) the backing <see cref="Seat"/> for a specific overlay position.</summary>
    Task<Seat?> ResolveSeatAsync(int overlaySectorId, int rowNumber, int seatNumber, CancellationToken ct = default);
}

public class OverlaySectionSummary
{
    public int OverlayId { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Type { get; set; } = "standard";
    /// <summary>Explicit per-sector default price if the admin set one; null means fall back to the type multiplier.</summary>
    public decimal? Price { get; set; }
    /// <summary>Per-event override for this sector's price, if one exists; wins over <see cref="Price"/>.</summary>
    public decimal? EventPrice { get; set; }
    public int TotalSeats { get; set; }
    public int SoldSeats { get; set; }
    public int AvailableSeats { get; set; }
    /// <summary>True when this sector is disabled (closed for sale) for the event — no seats bookable.</summary>
    public bool IsDisabled { get; set; }
}

public class OverlaySeatInfo
{
    public int RowNumber { get; set; }
    public int SeatNumber { get; set; }
    public string SeatCode { get; set; } = string.Empty;
    /// <summary>False when the seat is already sold or currently reserved for the event.</summary>
    public bool IsAvailable { get; set; } = true;
}

public class OverlaySeatService : IOverlaySeatService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OverlaySeatService> _logger;

    public OverlaySeatService(ApplicationDbContext context, ILogger<OverlaySeatService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public Task<StadiumSectorOverlay?> GetOverlayAsync(int overlaySectorId, CancellationToken ct = default)
        => _context.StadiumSectorOverlays
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == overlaySectorId && !o.IsDeleted, ct);

    public async Task<Dictionary<int, OverlaySectionSummary>> GetSectionSummariesAsync(int eventId, CancellationToken ct = default)
    {
        var overlays = await _context.StadiumSectorOverlays
            .AsNoTracking()
            .Where(o => !o.IsDeleted)
            .ToListAsync(ct);

        var codes = overlays.Select(o => o.SectorCode).ToList();
        var backing = await _context.StadiumSections
            .AsNoTracking()
            .Where(s => codes.Contains(s.SectionCode))
            .ToDictionaryAsync(s => s.SectionCode, s => s.Id, ct);

        // Per-event sector configuration (price override + disabled flag), keyed by overlay sector id
        // (empty when the event has none).
        var perEvent = await _context.EventSectorPrices
            .AsNoTracking()
            .Where(p => p.EventId == eventId)
            .ToDictionaryAsync(p => p.SectorOverlayId, ct);

        var result = new Dictionary<int, OverlaySectionSummary>();
        foreach (var o in overlays)
        {
            var total = o.TotalSeats;
            var sold = 0;
            if (backing.TryGetValue(o.SectorCode, out var sectionId))
                sold = await CountSectionSoldAsync(eventId, sectionId, ct);

            perEvent.TryGetValue(o.Id, out var cfg);
            var disabled = cfg?.IsDisabled ?? false;

            result[o.Id] = new OverlaySectionSummary
            {
                OverlayId = o.Id,
                Code = o.SectorCode,
                Name = o.Name,
                Type = o.Type,
                Price = o.Price,
                EventPrice = cfg?.Price,
                TotalSeats = total,
                SoldSeats = sold,
                // A disabled sector has no bookable seats for this event, regardless of how many are sold.
                AvailableSeats = disabled ? 0 : Math.Max(0, total - sold),
                IsDisabled = disabled
            };
        }
        return result;
    }

    public async Task<List<OverlaySeatInfo>> GetAvailableSeatsAsync(int eventId, int overlaySectorId, CancellationToken ct = default)
    {
        var overlay = await GetOverlayAsync(overlaySectorId, ct);
        if (overlay == null)
            return new List<OverlaySeatInfo>();

        var sectionId = await GetBackingSectionIdAsync(overlay, ct);
        var taken = sectionId != null ? await TakenPositionsAsync(eventId, sectionId.Value, ct) : new HashSet<(int, int)>();
        var reserved = await ReservedPositionsAsync(eventId, overlaySectorId, ct);

        var result = new List<OverlaySeatInfo>();
        foreach (var (row, seats) in RowLayout(overlay))
        {
            for (var seat = 1; seat <= seats; seat++)
            {
                if (taken.Contains((row, seat)) || reserved.Contains((row, seat)))
                    continue;
                result.Add(new OverlaySeatInfo
                {
                    RowNumber = row,
                    SeatNumber = seat,
                    SeatCode = $"{overlay.SectorCode}-R{row}-S{seat}"
                });
            }
        }
        return result;
    }

    public async Task<List<OverlaySeatInfo>> GetAllSeatsAsync(int eventId, int overlaySectorId, CancellationToken ct = default)
    {
        var overlay = await GetOverlayAsync(overlaySectorId, ct);
        if (overlay == null)
            return new List<OverlaySeatInfo>();

        var sectionId = await GetBackingSectionIdAsync(overlay, ct);
        var taken = sectionId != null ? await TakenPositionsAsync(eventId, sectionId.Value, ct) : new HashSet<(int, int)>();
        var reserved = await ReservedPositionsAsync(eventId, overlaySectorId, ct);

        var result = new List<OverlaySeatInfo>();
        foreach (var (row, seats) in RowLayout(overlay))
        {
            for (var seat = 1; seat <= seats; seat++)
            {
                result.Add(new OverlaySeatInfo
                {
                    RowNumber = row,
                    SeatNumber = seat,
                    SeatCode = $"{overlay.SectorCode}-R{row}-S{seat}",
                    IsAvailable = !taken.Contains((row, seat)) && !reserved.Contains((row, seat))
                });
            }
        }
        return result;
    }

    public async Task<bool> IsSeatSoldAsync(int eventId, int overlaySectorId, int rowNumber, int seatNumber, CancellationToken ct = default)
    {
        var overlay = await GetOverlayAsync(overlaySectorId, ct);
        if (overlay == null)
            return false;

        var sectionId = await GetBackingSectionIdAsync(overlay, ct);
        if (sectionId == null)
            return false;

        return await _context.Tickets.AnyAsync(t =>
            t.EventId == eventId
            && t.Status != TicketStatuses.Cancelled
            && t.SeatId != null
            && t.Seat.SectionId == sectionId
            && t.Seat.RowNumber == rowNumber
            && t.Seat.SeatNumber == seatNumber, ct);
    }

    public async Task<Seat?> ResolveSeatAsync(int overlaySectorId, int rowNumber, int seatNumber, CancellationToken ct = default)
    {
        var overlay = await _context.StadiumSectorOverlays
            .FirstOrDefaultAsync(o => o.Id == overlaySectorId && !o.IsDeleted, ct);
        if (overlay == null)
            return null;

        var section = await EnsureBackingSectionAsync(overlay, ct);

        var seat = await _context.Seats
            .FirstOrDefaultAsync(s => s.SectionId == section.Id && s.RowNumber == rowNumber && s.SeatNumber == seatNumber, ct);
        if (seat != null)
            return seat;

        seat = new Seat
        {
            SectionId = section.Id,
            RowNumber = rowNumber,
            SeatNumber = seatNumber,
            SeatCode = $"{section.SectionCode}-R{rowNumber}-S{seatNumber}",
            XCoordinate = 0,
            YCoordinate = 0,
            IsAccessible = true
        };
        _context.Seats.Add(seat);
        await _context.SaveChangesAsync(ct);
        return seat;
    }

    // ---- helpers -------------------------------------------------------------------------

    private Task<int> CountSectionSoldAsync(int eventId, int sectionId, CancellationToken ct)
        => _context.Tickets.CountAsync(t =>
            t.EventId == eventId
            && t.Status != TicketStatuses.Cancelled
            && t.SeatId != null
            && t.Seat.SectionId == sectionId, ct);

    private async Task<int?> GetBackingSectionIdAsync(StadiumSectorOverlay overlay, CancellationToken ct)
    {
        if (overlay.StadiumSectionId is int sid)
            return sid;
        return await _context.StadiumSections
            .Where(s => s.SectionCode == overlay.SectorCode)
            .Select(s => (int?)s.Id)
            .FirstOrDefaultAsync(ct);
    }

    private async Task<HashSet<(int, int)>> TakenPositionsAsync(int eventId, int sectionId, CancellationToken ct)
    {
        var list = await _context.Tickets
            .Where(t => t.EventId == eventId
                        && t.Status != TicketStatuses.Cancelled
                        && t.SeatId != null
                        && t.Seat.SectionId == sectionId)
            .Select(t => new { t.Seat.RowNumber, t.Seat.SeatNumber })
            .ToListAsync(ct);
        return new HashSet<(int, int)>(list.Select(x => (x.RowNumber, x.SeatNumber)));
    }

    private async Task<HashSet<(int, int)>> ReservedPositionsAsync(int eventId, int overlaySectorId, CancellationToken ct)
    {
        var now = DateTime.UtcNow;
        var list = await _context.SeatReservations
            .Where(r => r.EventId == eventId
                        && r.SectorId == overlaySectorId
                        && r.Status == ReservationStatus.Active
                        && r.ReservedUntil > now)
            .Select(r => new { r.RowNumber, r.SeatNumber })
            .ToListAsync(ct);
        return new HashSet<(int, int)>(list.Select(x => (x.RowNumber, x.SeatNumber)));
    }

    /// <summary>
    /// Maps an overlay to a backing <see cref="StadiumSection"/> (tickets bind to Seat →
    /// StadiumSection), creating + back-linking it on first use. Mirrors the ingestion path so
    /// customer- and externally-sold seats share the same section.
    /// </summary>
    private async Task<StadiumSection> EnsureBackingSectionAsync(StadiumSectorOverlay overlay, CancellationToken ct)
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
                var trackedOverlay = await _context.StadiumSectorOverlays.FirstOrDefaultAsync(o => o.Id == overlay.Id, ct);
                if (trackedOverlay != null)
                {
                    trackedOverlay.StadiumSectionId = existing.Id;
                    await _context.SaveChangesAsync(ct);
                }
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

        var trackedForLink = await _context.StadiumSectorOverlays.FirstOrDefaultAsync(o => o.Id == overlay.Id, ct);
        if (trackedForLink != null)
        {
            trackedForLink.StadiumSectionId = section.Id;
            await _context.SaveChangesAsync(ct);
        }
        return section;
    }

    private static IEnumerable<(int row, int seats)> RowLayout(StadiumSectorOverlay overlay)
    {
        List<RowPattern>? patterns = null;
        if (overlay.UseVariableSeating && !string.IsNullOrEmpty(overlay.VariableSeatingData))
        {
            try { patterns = JsonSerializer.Deserialize<List<RowPattern>>(overlay.VariableSeatingData); }
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
}
