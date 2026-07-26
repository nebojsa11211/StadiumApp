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
    private readonly IAccountProvisioningService _accountProvisioning;
    private readonly ILogger<MatchSimulationService> _logger;

    public MatchSimulationService(
        ApplicationDbContext context,
        ITicketIngestionService ingestion,
        IDemoDataService demoData,
        IAccountProvisioningService accountProvisioning,
        ILogger<MatchSimulationService> logger)
    {
        _context = context;
        _ingestion = ingestion;
        _demoData = demoData;
        _accountProvisioning = accountProvisioning;
        _logger = logger;
    }

    /// <summary>
    /// How a played fixture's tickets end up: most holders turned up, some never came, a few got
    /// their money back. Leaving them all "Active" is the tell-tale sign of seeded data.
    /// </summary>
    private const int AttendedPercent = 85;
    private const int RefundedPercent = 5; // the remainder are no-shows

    /// <summary>
    /// How many distinct people a simulated crowd is drawn from. A whole season is thousands of
    /// tickets; drawing them from a handful of names makes every drink order in the ground belong to
    /// the same few accounts, which is useless for testing anything per-customer. Capped rather than
    /// unbounded so a generated season doesn't bury Admin → Customers under a new fan per ticket.
    /// </summary>
    private const int FanPoolSize = 200;

    /// <summary>One simulated fan: a stable identity, so the same person recurs across fixtures and
    /// seasons instead of a fresh account being minted per ticket.</summary>
    private sealed record SimulatedFan(string Name, string Email, string Oib);

    private static readonly string[] FanFirstNames =
    {
        "Ivan", "Marko", "Ana", "Petra", "Luka", "Josip", "Marija", "Tomislav", "Ivana", "Filip",
        "Sara", "Nikola", "Maja", "Stjepan", "Lucija", "Antonio", "Katarina", "Domagoj", "Nina", "Mislav"
    };

    private static readonly string[] FanLastNames =
    {
        "Horvat", "Kovacevic", "Babic", "Novak", "Maric", "Juric", "Vukovic", "Peric", "Simic", "Barisic"
    };

    /// <summary>
    /// The fan pool, built once and deterministically so a rerun reuses the same people (and therefore
    /// the same provisioned accounts) rather than doubling the customer list every time.
    /// </summary>
    private static readonly IReadOnlyList<SimulatedFan> FanPool = BuildFanPool();

    private static List<SimulatedFan> BuildFanPool()
    {
        var fans = new List<SimulatedFan>(FanPoolSize);
        foreach (var last in FanLastNames)
        {
            foreach (var first in FanFirstNames)
            {
                if (fans.Count >= FanPoolSize)
                    return fans;

                // OIB derived from the position in the pool: 11 digits, stable per fan, so the fan's
                // ticket and their provisioned account always agree on who they are.
                var oib = (10_000_000_000L + fans.Count).ToString();
                fans.Add(new SimulatedFan(
                    $"{first} {last}",
                    $"{first}.{last}@example.com".ToLowerInvariant(),
                    oib));
            }
        }
        return fans;
    }

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
        {
            result.TicketsSold = await SellCrowdAsync(eventId, name, kickOff, request.TicketsToSell, request.BaseTicketPrice, ct);

            // A crowd was asked for and none could be seated: the fixture exists but is empty, which
            // is not a success worth reporting silently. The only cause left is an undrawn stadium.
            if (result.TicketsSold == 0)
            {
                result.Message = "Fixture created but no tickets could be sold: no stadium sectors are " +
                                 "drawn. Draw the stadium in Admin → Stadium Drawing Tool first.";
                _logger.LogWarning(
                    "Simulated fixture {Name} sold 0 of {Requested} requested tickets — no drawn sectors to sell into.",
                    name, request.TicketsToSell);
            }
        }

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

                // Drink generation declining the fixture is the one outcome a caller can't infer from
                // the numbers: zero orders looks identical whether the bar sold nothing or the
                // generator never ran. Carry its reason back rather than reporting a silent zero.
                if (!drinks.Success)
                {
                    result.DrinkOrdersMessage = drinks.Message;
                    _logger.LogWarning(
                        "Simulated fixture {Name} (event {EventId}) generated no drink orders: {Reason}",
                        name, eventId, drinks.Message);
                }
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
        // A sector that has never been sold into has no backing section yet, so stand it up through
        // the ingestion path's own resolver — skipping it instead would silently sell nothing at all
        // on a stadium that has been drawn but not yet sold into.
        var sectors = new List<SectorFill>();
        foreach (var overlay in overlays)
        {
            var section = await _ingestion.EnsureBackingSectionAsync(overlay, ct);

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

        var tickets = new List<Ticket>(count);
        var stamp = DateTime.UtcNow.Ticks;

        // Who actually turned out, so only these get an account provisioned below.
        var fansInAttendance = new Dictionary<string, SimulatedFan>(StringComparer.OrdinalIgnoreCase);

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

            var fan = FanPool[Random.Shared.Next(FanPool.Count)];
            fansInAttendance[fan.Email] = fan;

            tickets.Add(new Ticket
            {
                TicketNumber = $"TK{stamp}{i:D5}",
                EventId = eventId,
                Seat = seat, // navigation, so a newly built Seat row is inserted alongside
                QRCode = string.Empty,
                QRCodeToken = Guid.NewGuid().ToString(),
                CustomerName = fan.Name,
                CustomerEmail = fan.Email,
                CustomerOib = fan.Oib,
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

        await ProvisionFansAsync(fansInAttendance.Values, ct);

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

    /// <summary>
    /// Gives every simulated fan in the crowd a claimable shell account — exactly as the real webhook
    /// sale path does (<see cref="ITicketIngestionService"/>'s TicketSold handler). Without this the
    /// fan's email matches no account, and everything downstream that resolves a ticket to a customer
    /// — drink orders above all — falls back to whatever account it can find, which is how a whole
    /// season's orders end up on one customer.
    ///
    /// Existing accounts are filtered out in a single query first: provisioning opens its own DbContext
    /// scope per call, so re-offering the same 200 fans on every fixture of a season would be hundreds
    /// of pointless scopes. No activation mail is sent — these are throwaway @example.com addresses.
    /// </summary>
    private async Task ProvisionFansAsync(IEnumerable<SimulatedFan> fans, CancellationToken ct)
    {
        var byEmail = fans
            .GroupBy(f => f.Email, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(g => g.Key, g => g.First(), StringComparer.OrdinalIgnoreCase);
        if (byEmail.Count == 0)
            return;

        var emails = byEmail.Keys.ToList();
        var existing = (await _context.Users
                .Where(u => emails.Contains(u.Email))
                .Select(u => u.Email)
                .ToListAsync(ct))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var provisioned = 0;
        foreach (var fan in byEmail.Values.Where(f => !existing.Contains(f.Email)))
        {
            await _accountProvisioning.EnsureShellAccountAsync(
                fan.Email, fan.Name, null, "SimulatedSeasonCrowd", sendActivation: false, oib: fan.Oib);
            provisioned++;
        }

        if (provisioned > 0)
            _logger.LogInformation(
                "Provisioned {Count} new simulated fan account(s) of {Total} in the crowd", provisioned, byEmail.Count);
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
