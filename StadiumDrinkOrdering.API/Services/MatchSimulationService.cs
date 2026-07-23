using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.DTOs.Integration;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

/// <summary>
/// Stands up one complete fixture on request from the external ticketing system (the simulator's
/// season generator). Everything it does is composed from the paths a real feed already uses —
/// the ingestion webhook creates the event and extends season passes to it, the ticket simulator
/// sells it a crowd — so a generated season behaves like one that arrived match by match.
/// </summary>
public interface IMatchSimulationService
{
    Task<SimulateMatchResult> SimulateMatchAsync(SimulateMatchRequest request, CancellationToken ct = default);
}

public class MatchSimulationService : IMatchSimulationService
{
    private readonly ApplicationDbContext _context;
    private readonly ITicketIngestionService _ingestion;
    private readonly IDemoDataService _demoData;
    private readonly ILogger<MatchSimulationService> _logger;

    public MatchSimulationService(
        ApplicationDbContext context,
        ITicketIngestionService ingestion,
        IDemoDataService demoData,
        ILogger<MatchSimulationService> logger)
    {
        _context = context;
        _ingestion = ingestion;
        _demoData = demoData;
        _logger = logger;
    }

    /// <summary>
    /// How a played fixture's tickets end up: most holders turned up, some never came, a few got
    /// their money back. Leaving them all "Active" is the tell-tale sign of seeded data.
    /// </summary>
    private const int AttendedPercent = 85;
    private const int RefundedPercent = 5; // the remainder are no-shows

    public async Task<SimulateMatchResult> SimulateMatchAsync(SimulateMatchRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(request.ExternalSeasonId))
            return new SimulateMatchResult { Accepted = false, Message = "ExternalSeasonId is required" };

        var kickOff = DateTime.SpecifyKind(
            request.KickOff.Kind == DateTimeKind.Utc ? request.KickOff : request.KickOff.ToUniversalTime(),
            DateTimeKind.Utc);
        var duration = request.DurationMinutes > 0 ? request.DurationMinutes : 120;
        var endsAt = kickOff.AddMinutes(duration);
        var isPast = endsAt < DateTime.UtcNow;

        var home = string.IsNullOrWhiteSpace(request.HomeTeam) ? null : request.HomeTeam.Trim();
        var away = string.IsNullOrWhiteSpace(request.AwayTeam) ? null : request.AwayTeam.Trim();
        var name = !string.IsNullOrWhiteSpace(request.EventName)
            ? request.EventName.Trim()
            : home != null && away != null ? $"{home} - {away}" : "Simulated fixture";

        var externalEventId = "EVT-" + Guid.NewGuid().ToString("N")[..12].ToUpperInvariant();

        // Create the fixture through the ordinary ingestion path: it decides Completed-vs-OnSale from
        // the date, links the season, and extends every existing season pass to this new match.
        var envelope = new TicketingWebhookEnvelope
        {
            EventType = TicketingEventTypes.EventCreated,
            IdempotencyKey = Guid.NewGuid().ToString(),
            OccurredAt = DateTime.UtcNow,
            SourceSystem = "TicketingSimulator",
            Event = new ExternalEventDto
            {
                ExternalEventId = externalEventId,
                ExternalSeasonId = request.ExternalSeasonId,
                EventName = name,
                EventType = "Match",
                HomeTeam = home,
                AwayTeam = away,
                EventDate = kickOff,
                EventEndDate = endsAt,
                BaseTicketPrice = request.BaseTicketPrice,
                Description = "Simulated fixture"
            }
        };

        var created = await _ingestion.ProcessWebhookAsync(envelope, ct);
        if (!created.Accepted || created.EventId is not int eventId)
            return new SimulateMatchResult { Accepted = false, Message = created.Message, ExternalEventId = externalEventId };

        var result = new SimulateMatchResult
        {
            Accepted = true,
            Message = "Fixture simulated",
            EventId = eventId,
            ExternalEventId = externalEventId,
            EventName = name,
            KickOff = kickOff,
            IsPast = isPast,
            SeasonTicketsCovered = created.DerivedTicketsAffected
        };

        // Sell it a crowd of ordinary single-event tickets on top of the season-pass seats.
        if (request.TicketsToSell > 0)
            result.TicketsSold = await SellCrowdAsync(eventId, name, kickOff, request.TicketsToSell, request.BaseTicketPrice, ct);

        if (isPast)
        {
            // A ticket for a match that has been played was bought BEFORE it, not in the last few
            // hours — which is what the generic sales simulator stamps. Re-date them across the
            // month leading up to kick-off so the purchase curve reads like a real on-sale period.
            await BackdatePurchasesAsync(eventId, kickOff, ct);

            if (request.SettleAttendance)
            {
                var (attended, refunded) = await SettleAttendanceAsync(eventId, ct);
                result.TicketsAttended = attended;
                result.TicketsRefunded = refunded;
            }

            if (request.GenerateDrinkOrders)
            {
                var drinks = await _demoData.GenerateMatchDayDrinkSalesForEventAsync(eventId, replaceExisting: true);
                result.DrinkOrders = drinks.Orders;
                result.DrinkRevenue = drinks.Revenue;
            }
        }

        var status = await _context.Events
            .Where(e => e.Id == eventId)
            .Select(e => e.Status)
            .FirstOrDefaultAsync(ct);
        result.Status = status.ToString();

        _logger.LogInformation(
            "Simulated fixture {Name} ({ExternalId}) -> event {EventId}: {Tickets} tickets, {Season} season seats, {Orders} drink orders",
            name, externalEventId, eventId, result.TicketsSold, result.SeasonTicketsCovered, result.DrinkOrders);

        return result;
    }

    /// <summary>
    /// Fills a fixture with single-event tickets, spreading them over the drawn sectors.
    ///
    /// Deliberately not <see cref="ITicketIngestionService.SimulateTicketSalesAsync"/>: that allocates
    /// one seat at a time and re-reads the sector's seats and occupancy for every ticket, which is
    /// quadratic and takes minutes at full-house volumes. A whole season means a dozen full houses, so
    /// here the seat map is read once, seats are picked in memory, and everything is written in one go.
    /// Sold-out sectors are skipped, and the season-pass seats already held for this match are treated
    /// as taken so a pass holder's seat is never sold twice.
    /// </summary>
    private async Task<int> SellCrowdAsync(int eventId, string eventName, DateTime kickOff, int count, decimal basePrice, CancellationToken ct)
    {
        var overlays = await _context.StadiumSectorOverlays
            .Where(o => !o.IsDeleted)
            .OrderBy(o => o.Id)
            .ToListAsync(ct);
        if (overlays.Count == 0)
            return 0;

        // Seats already spoken for at this fixture (season passes materialised when it was created).
        var takenSeatIds = (await _context.Tickets
                .Where(t => t.EventId == eventId && t.SeatId != null && t.Status != TicketStatuses.Cancelled)
                .Select(t => t.SeatId!.Value)
                .ToListAsync(ct))
            .ToHashSet();

        // Per sector: the section that backs it, its existing seat rows, and the next free position.
        var sectors = new List<SectorFill>();
        foreach (var overlay in overlays)
        {
            var section = await _context.StadiumSections
                .FirstOrDefaultAsync(s => s.Id == overlay.StadiumSectionId || s.SectionCode == overlay.SectorCode, ct);
            if (section == null)
                continue; // never sold into — the ingestion path creates it on the first real sale

            var seats = await _context.Seats
                .Where(s => s.SectionId == section.Id)
                .OrderBy(s => s.RowNumber).ThenBy(s => s.SeatNumber)
                .ToListAsync(ct);

            sectors.Add(new SectorFill
            {
                Overlay = overlay,
                Section = section,
                FreeSeats = new Queue<Seat>(seats.Where(s => !takenSeatIds.Contains(s.Id))),
                Occupied = seats.Select(s => (s.RowNumber, s.SeatNumber)).ToHashSet(),
                Capacity = overlay.TotalSeats > 0 ? overlay.TotalSeats : section.TotalRows * section.SeatsPerRow
            });
        }

        if (sectors.Count == 0)
            return 0;

        var names = new[] { "Ivan Horvat", "Marko Kovačević", "Ana Babić", "Petra Novak", "Luka Marić",
                            "Josip Jurić", "Marija Vuković", "Tomislav Perić", "Ivana Šimić", "Filip Barišić" };
        var tickets = new List<Ticket>(count);
        var stamp = DateTime.UtcNow.Ticks;

        for (var i = 0; i < count; i++)
        {
            // Round-robin so a crowd spreads across the ground rather than filling one end first.
            var sector = sectors[i % sectors.Count];
            var seat = TakeSeat(sector);
            if (seat == null)
            {
                // That sector is full — try the others before giving up on this ticket.
                sector = sectors.FirstOrDefault(s => HasRoom(s));
                if (sector == null)
                    break; // whole ground sold out
                seat = TakeSeat(sector)!;
            }

            var person = names[Random.Shared.Next(names.Length)];
            tickets.Add(new Ticket
            {
                TicketNumber = $"TK{stamp}{i:D5}",
                EventId = eventId,
                Seat = seat, // navigation, so a newly built Seat row is inserted alongside
                QRCode = string.Empty,
                QRCodeToken = Guid.NewGuid().ToString(),
                CustomerName = person,
                CustomerEmail = $"{person.Replace(" ", ".").ToLowerInvariant()}@example.com",
                CustomerOib = Random.Shared.NextInt64(10000000000, 99999999999).ToString(),
                Price = decimal.Round(basePrice + (decimal)(Random.Shared.NextDouble() * 20), 2),
                PurchaseDate = DateTime.UtcNow,
                Status = TicketStatuses.Active,
                SeatNumber = seat.SeatNumber.ToString(),
                Section = sector.Overlay.SectorCode,
                Row = seat.RowNumber.ToString(),
                EventName = eventName,
                EventDate = kickOff,
                IsActive = true,
                Kind = TicketKind.SingleEvent
            });
        }

        _context.Tickets.AddRange(tickets);
        await _context.SaveChangesAsync(ct);
        return tickets.Count;

        static bool HasRoom(SectorFill s) => s.FreeSeats.Count > 0 || s.Occupied.Count < s.Capacity;

        static Seat? TakeSeat(SectorFill sector)
        {
            if (sector.FreeSeats.Count > 0)
                return sector.FreeSeats.Dequeue();

            // No spare seat row exists yet — build the next unoccupied position in the sector.
            var perRow = Math.Max(1, sector.Section.SeatsPerRow);
            for (var i = 0; i < sector.Capacity; i++)
            {
                var row = i / perRow + 1;
                var number = i % perRow + 1;
                if (!sector.Occupied.Add((row, number)))
                    continue;

                return new Seat
                {
                    SectionId = sector.Section.Id,
                    RowNumber = row,
                    SeatNumber = number,
                    SeatCode = $"{sector.Section.SectionCode}-R{row}-S{number}",
                    IsAccessible = true
                };
            }
            return null;
        }
    }

    /// <summary>Working state for filling one sector: its free seats and next buildable position.</summary>
    private sealed class SectorFill
    {
        public required StadiumSectorOverlay Overlay { get; init; }
        public required StadiumSection Section { get; init; }
        public required Queue<Seat> FreeSeats { get; init; }
        public required HashSet<(int Row, int Number)> Occupied { get; init; }
        public required int Capacity { get; init; }
    }

    /// <summary>
    /// Moves the simulated purchases of a played fixture into the four weeks before kick-off, with a
    /// bias toward the final week — the shape of a real on-sale period, and it keeps a ticket from
    /// appearing to have been bought after the match it admits to.
    /// </summary>
    private async Task BackdatePurchasesAsync(int eventId, DateTime kickOff, CancellationToken ct)
    {
        var tickets = await _context.Tickets
            .Where(t => t.EventId == eventId && t.Kind == TicketKind.SingleEvent)
            .ToListAsync(ct);

        foreach (var ticket in tickets)
        {
            // Squaring a 0..1 roll clusters values near 0 — i.e. near kick-off — so most sales land
            // in the last days rather than being spread flat over the month.
            var roll = Random.Shared.NextDouble();
            var daysBefore = 28 * roll * roll;
            ticket.PurchaseDate = DateTime.SpecifyKind(kickOff.AddDays(-daysBefore), DateTimeKind.Utc);
        }

        if (tickets.Count > 0)
            await _context.SaveChangesAsync(ct);
    }

    /// <summary>
    /// Settles a played fixture's tickets into attended / no-show / refunded, and returns how many
    /// ended up attended and refunded. Season-derived tickets are settled too — a pass holder either
    /// turned up or didn't — but are never refunded here, since refunding those belongs to the pass.
    /// </summary>
    private async Task<(int Attended, int Refunded)> SettleAttendanceAsync(int eventId, CancellationToken ct)
    {
        var tickets = await _context.Tickets
            .Where(t => t.EventId == eventId && t.Status != TicketStatuses.Cancelled)
            .ToListAsync(ct);

        var attended = 0;
        var refunded = 0;

        foreach (var ticket in tickets)
        {
            var roll = Random.Shared.Next(100);
            if (roll < AttendedPercent)
            {
                ticket.Status = TicketStatuses.Used;
                ticket.IsUsed = true;
                ticket.UsedAt = DateTime.SpecifyKind(
                    (ticket.EventDate ?? DateTime.UtcNow).AddMinutes(-Random.Shared.Next(5, 75)), DateTimeKind.Utc);
                attended++;
            }
            else if (roll < AttendedPercent + RefundedPercent && ticket.Kind == TicketKind.SingleEvent)
            {
                ticket.Status = TicketStatuses.Cancelled;
                ticket.IsActive = false;
                refunded++;
            }
            // else: a no-show — the ticket stays Active and unused.
        }

        if (tickets.Count > 0)
            await _context.SaveChangesAsync(ct);

        return (attended, refunded);
    }
}
