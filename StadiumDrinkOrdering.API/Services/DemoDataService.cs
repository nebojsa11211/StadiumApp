using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.API.Data;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Services;

public interface IDemoDataService
{
    Task<bool> GenerateComprehensiveDemoDataAsync();
    Task<bool> GenerateEventWithTicketsAsync(string eventName, DateTime eventDate, int ticketCount = 50);
    Task<bool> GenerateTestOrdersAsync(int orderCount = 20);
    Task<bool> GenerateStaffAssignmentsAsync();
    Task<bool> ClearDemoDataAsync();
    Task<bool> GenerateDemoDataForEventAsync(int eventId);

    /// <summary>
    /// Ensures every <see cref="EventStatus.Completed"/> event has some realised drink sales so the
    /// post-event statistics page shows meaningful numbers. Idempotent: an event that already has any
    /// non-cancelled drink order is skipped, so re-running does not pile up duplicate sales. Returns
    /// the number of completed events that had sales generated for them.
    /// </summary>
    Task<int> GenerateDrinkSalesForCompletedEventsAsync();

    /// <summary>
    /// One-shot backfill that gives already-existing tickets of past (finished) events a realistic
    /// lifecycle mix — ~85% Used (attended), ~10% Active (no-show), ~5% Cancelled (refunded) — instead
    /// of the flat "Active" they were originally seeded with. Only touches tickets whose event date is
    /// in the past; future-event tickets are left untouched. Returns the number of tickets updated.
    /// </summary>
    Task<int> BackfillPastEventTicketStatusesAsync();

    /// <summary>
    /// Seeds the real GNK Dinamo 2025/26 SuperSport HNL season: makes GNK Dinamo the venue's primary
    /// resident club, creates the "2025/26" <see cref="Season"/>, and adds one <see cref="Event"/> per
    /// Dinamo home match (all 18 already played — final scores from HNS), linked to that season and
    /// marked <see cref="EventStatus.Completed"/>. No tickets or orders are generated. Idempotent:
    /// fixtures already present (matched by event name) are skipped, so re-running adds nothing.
    /// </summary>
    Task<DinamoSeasonSeedResult> GenerateDinamoSeason2025Async();

    /// <summary>
    /// Gives events <em>varied</em> per-event sales so the dashboard KPIs actually change as you step
    /// through events: for each target event it tops occupancy up to a random target with ordinary
    /// single-event tickets (making "Tickets Sold" and the regular/season split differ per event) and
    /// adds a random number of realised (Delivered) drink orders (making "Total Revenue" differ per
    /// event). Pass <paramref name="seasonId"/> to limit it to one season; when null it processes every
    /// event that is still "flat" (no single-event tickets and/or no drink orders yet). Idempotent: the
    /// ticket top-up is skipped for an event that already has single-event tickets, and the order top-up
    /// for an event that already has non-cancelled orders, so re-running never doubles up.
    /// </summary>
    Task<VariedSalesSeedResult> GenerateVariedPerEventSalesAsync(int? seasonId = null);

    /// <summary>
    /// Generates a match-day's worth of drink orders for ONE event, modelled on how a real crowd
    /// actually buys rather than a flat random count (which is what
    /// <see cref="GenerateDrinkSalesForCompletedEventsAsync"/> does for bulk seeding):
    /// <list type="bullet">
    /// <item>volume scales with the seats actually occupied — roughly a third of attendees buy,
    /// and some of those come back for a second round;</item>
    /// <item>orders are placed from real occupied seats and attributed to the ticket holder's
    /// account when one exists;</item>
    /// <item>timing follows a match profile — a build-up, a large half-time spike, then a tail —
    /// instead of being spread evenly over the whole window;</item>
    /// <item>baskets are beer-led and small, the way in-seat delivery orders are;</item>
    /// <item>outcomes are terminal (the match is over) but not uniformly happy: a few orders were
    /// cancelled or failed to reach the fan.</item>
    /// </list>
    /// Refuses an event that already has orders unless <paramref name="replaceExisting"/> is set,
    /// in which case its existing orders are removed first so figures never silently double.
    /// </summary>
    Task<EventDrinkSalesResult> GenerateMatchDayDrinkSalesForEventAsync(int eventId, bool replaceExisting = false);
}

/// <summary>Outcome of generating a match-day's drink orders for a single event.</summary>
public record EventDrinkSalesResult(
    bool Success,
    string Message,
    int EventId = 0,
    string EventName = "",
    int Attendance = 0,
    int Orders = 0,
    int LineItems = 0,
    int DrinksSold = 0,
    decimal Revenue = 0m,
    int Delivered = 0,
    int Cancelled = 0,
    int DeliveryFailed = 0);

/// <summary>Outcome of seeding the GNK Dinamo 2025/26 home-fixture sample.</summary>
public record DinamoSeasonSeedResult(
    int SeasonId,
    string SeasonName,
    string HomeClub,
    int TotalHomeMatches,
    int EventsCreated,
    int EventsSkipped);

/// <summary>Outcome of seeding varied per-event ticket sales and drink orders.</summary>
public record VariedSalesSeedResult(
    int EventsProcessed,
    int TicketsAdded,
    int OrdersAdded);

public class DemoDataService : IDemoDataService
{
    private readonly ApplicationDbContext _context;
    private readonly IQRCodeService _qrCodeService;
    private readonly ILogger<DemoDataService> _logger;

    /// <summary>
    /// The per-ticket balance cap a seeded anonymous wallet must respect, read from the same
    /// <c>TicketWallet:MaxBalance</c> setting <see cref="IWalletService"/> enforces at the counter —
    /// generated data that breaks the venue's own limit would be worse than no data.
    /// </summary>
    private readonly decimal _ticketWalletMaxBalance;

    public DemoDataService(
        ApplicationDbContext context,
        IQRCodeService qrCodeService,
        IConfiguration configuration,
        ILogger<DemoDataService> logger)
    {
        _context = context;
        _qrCodeService = qrCodeService;
        _logger = logger;
        _ticketWalletMaxBalance = configuration.GetValue<decimal?>("TicketWallet:MaxBalance") ?? 100m;
    }

    public async Task<bool> GenerateComprehensiveDemoDataAsync()
    {
        try
        {
            _logger.LogInformation("Starting comprehensive demo data generation...");

            // Generate staff users
            await GenerateStaffUsersAsync();
            
            // Generate multiple events
            await GenerateEventsAsync();
            
            // Skip additional seat generation - stadium structure already preserved
            _logger.LogInformation("Skipping seat generation - stadium structure already preserved");
            
            // Generate tickets with QR codes for all events
            await GenerateTicketsForAllEventsAsync();
            
            // Generate staff assignments
            await GenerateStaffAssignmentsAsync();
            
            // Generate test orders
            await GenerateTestOrdersAsync(30);
            
            // Generate order sessions
            await GenerateOrderSessionsAsync();
            
            // Generate notifications
            await GenerateNotificationsAsync();
            
            // Generate analytics data
            await GenerateAnalyticsDataAsync();

            _logger.LogInformation("Comprehensive demo data generation completed successfully");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating comprehensive demo data");
            return false;
        }
    }

    private async Task GenerateStaffUsersAsync()
    {
        var staffUsers = new[]
        {
            // Staff Users
            new User { Id = 10, Username = "alex.bartender", Email = "alex.martinez@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Bartender, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 11, Username = "sarah.bartender", Email = "sarah.johnson@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Bartender, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 12, Username = "mike.waiter", Email = "mike.wilson@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Waiter, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 13, Username = "emma.waiter", Email = "emma.davis@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Waiter, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 14, Username = "david.waiter", Email = "david.brown@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Waiter, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(30, 365)) },
            new User { Id = 15, Username = "lisa.supervisor", Email = "lisa.supervisor@stadium.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Admin, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(60, 365)) },
            
            // Customer Users
            new User { Id = 20, Username = "john.customer", Email = "john.smith@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 7)) },
            new User { Id = 21, Username = "mary.johnson", Email = "mary.johnson@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 14)) },
            new User { Id = 22, Username = "robert.williams", Email = "robert.williams@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30)) },
            new User { Id = 23, Username = "jennifer.garcia", Email = "jennifer.garcia@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 5)) },
            new User { Id = 24, Username = "michael.davis", Email = "michael.davis@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 21)) },
            new User { Id = 25, Username = "jessica.martinez", Email = "jessica.martinez@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 3)) },
            new User { Id = 26, Username = "william.anderson", Email = "william.anderson@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 10)) },
            new User { Id = 27, Username = "ashley.taylor", Email = "ashley.taylor@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 15)) },
            new User { Id = 28, Username = "james.thomas", Email = "james.thomas@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 8)) },
            new User { Id = 29, Username = "amanda.rodriguez", Email = "amanda.rodriguez@example.com", PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"), Role = UserRole.Customer, CreatedAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 180)), LastLoginAt = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 12)) }
        };

        foreach (var user in staffUsers)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser == null)
            {
                _context.Users.Add(user);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Generated {Count} users (staff + customers)", staffUsers.Length);
    }

    private async Task GenerateEventsAsync()
    {
        // UTC-based "today" so every EventDate has Kind=Utc. PostgreSQL 'timestamp with time zone'
        // rejects Local/Unspecified DateTimes, so DateTime.Today (Local) must not be used here.
        var today = DateTime.UtcNow.Date;
        // A "Match" always has two sides: the home team is the venue's primary resident club
        // (seeded if the venue has none yet); each away team is a plausible visitor.
        var homeClub = await EnsureHomeClubAsync();
        var events = new[]
        {
            // Upcoming Events
            new Event { Id = 10, EventName = "Premier League Derby Match", EventType = "Match", HomeTeam = homeClub, AwayTeam = "Hajduk Split", EventDate = today.AddDays(3).AddHours(15), TotalSeats = 350, BaseTicketPrice = 85.00m, Description = "Intense local derby match between city rivals", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-14) },
            new Event { Id = 11, EventName = "Champions League Semi-Final", EventType = "Match", HomeTeam = homeClub, AwayTeam = "Bayern München", EventDate = today.AddDays(8).AddHours(20), TotalSeats = 400, BaseTicketPrice = 120.00m, Description = "European Champions League semi-final showdown", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-21) },
            new Event { Id = 12, EventName = "Rock Legends World Tour", EventType = "Concert", EventDate = today.AddDays(12).AddHours(19), TotalSeats = 280, BaseTicketPrice = 95.00m, Description = "World-famous rock band performing greatest hits", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-28) },
            new Event { Id = 13, EventName = "NBA Basketball Exhibition", EventType = "Match", HomeTeam = homeClub, AwayTeam = "Barcelona", EventDate = today.AddDays(18).AddHours(19), TotalSeats = 250, BaseTicketPrice = 75.00m, Description = "Professional basketball exhibition match", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-10) },
            new Event { Id = 14, EventName = "International Rugby Final", EventType = "Match", HomeTeam = homeClub, AwayTeam = "Leinster", EventDate = today.AddDays(25).AddHours(16), TotalSeats = 320, BaseTicketPrice = 90.00m, Description = "International rugby championship final", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-35) },
            
            // Recent Past Events (for analytics data)
            new Event { Id = 15, EventName = "Summer Festival Concert", EventType = "Concert", EventDate = today.AddDays(-5).AddHours(18), TotalSeats = 300, BaseTicketPrice = 65.00m, Description = "Summer music festival main event", IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-42) },
            new Event { Id = 16, EventName = "Cup Quarter-Final", EventType = "Match", HomeTeam = homeClub, AwayTeam = "HNK Rijeka", EventDate = today.AddDays(-12).AddHours(15), TotalSeats = 280, BaseTicketPrice = 55.00m, Description = "Exciting cup quarter-final match", IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-49) },
            new Event { Id = 17, EventName = "Jazz Night Special", EventType = "Concert", EventDate = today.AddDays(-8).AddHours(20), TotalSeats = 200, BaseTicketPrice = 45.00m, Description = "Special jazz performance evening", IsActive = false, CreatedAt = DateTime.UtcNow.AddDays(-38) },
            
            // Far Future Events  
            new Event { Id = 18, EventName = "Season Finale Championship", EventType = "Match", HomeTeam = homeClub, AwayTeam = "NK Osijek", EventDate = today.AddDays(45).AddHours(17), TotalSeats = 400, BaseTicketPrice = 110.00m, Description = "End of season championship match", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-7) },
            new Event { Id = 19, EventName = "Pop Stars United Concert", EventType = "Concert", EventDate = today.AddDays(60).AddHours(19), TotalSeats = 350, BaseTicketPrice = 80.00m, Description = "Multiple pop stars collaboration concert", IsActive = true, CreatedAt = DateTime.UtcNow.AddDays(-14) }
        };

        foreach (var evt in events)
        {
            var existingEvent = await _context.Events.FindAsync(evt.Id);
            if (existingEvent == null)
            {
                _context.Events.Add(evt);
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Generated {Count} events with realistic dates", events.Length);
    }

    /// <summary>
    /// Returns the name of the venue's home club — its primary club, else its first — to use as the
    /// home side of the demo fixtures. Seeds a default club (and the singleton venue, if missing) when
    /// none exist yet, so the demo matches always reference a real resident club.
    /// </summary>
    private async Task<string> EnsureHomeClubAsync()
    {
        var venue = await _context.Venues.Include(v => v.Clubs).FirstOrDefaultAsync();
        if (venue == null)
        {
            venue = new Venue { Name = "Stadium", CreatedAt = DateTime.UtcNow };
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        var homeClub = venue.Clubs
            .OrderByDescending(c => c.IsPrimary)
            .ThenBy(c => c.DisplayOrder)
            .ThenBy(c => c.Name)
            .FirstOrDefault();

        if (homeClub == null)
        {
            homeClub = new Club
            {
                VenueId = venue.Id,
                Name = "FC Stadium",
                ShortName = "FCS",
                IsPrimary = true,
                DisplayOrder = 0,
                PrimaryColor = "#1d4ed8",
                SecondaryColor = "#ffffff",
                CreatedAt = DateTime.UtcNow
            };
            _context.Clubs.Add(homeClub);
            await _context.SaveChangesAsync();
            _logger.LogInformation("Seeded default resident club '{Club}' for the venue", homeClub.Name);
        }

        return homeClub.Name;
    }

    private async Task GenerateAdditionalSeatsAsync()
    {
        // Skip seat generation since stadium structure is already preserved
        var existingSeatCount = await _context.Seats.CountAsync();
        _logger.LogInformation("Stadium already has {SeatCount} seats, skipping additional seat generation", existingSeatCount);
        return;
    }

    private async Task GenerateTicketsForAllEventsAsync()
    {
        var events = await _context.Events.ToListAsync();
        var seats = await _context.Seats.ToListAsync();
        var maxTicketId = await _context.Tickets.MaxAsync(t => (int?)t.Id) ?? 5;

        foreach (var evt in events)
        {
            var ticketsToGenerate = Math.Min(20, seats.Count); // Generate up to 20 tickets per event
            var selectedSeats = seats.Take(ticketsToGenerate).ToList();

            var isPastEvent = evt.EventDate < DateTime.UtcNow;

            foreach (var seat in selectedSeats)
            {
                maxTicketId++;

                // Default: an active ticket bought for an upcoming event.
                var status = TicketStatuses.Active;
                var isActive = true;
                var isUsed = false;
                DateTime? usedAt = null;
                var purchaseDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(1, 30));

                if (isPastEvent)
                {
                    // Ticket was bought before the event actually took place.
                    purchaseDate = evt.EventDate.AddDays(-Random.Shared.Next(1, 30));

                    // Realistic lifecycle mix for a finished event:
                    // 85% attended (scanned), 10% no-show (still active, never scanned), 5% refunded.
                    var roll = Random.Shared.Next(100); // 0-99
                    if (roll < 85)
                    {
                        status = TicketStatuses.Used;
                        isUsed = true;
                        usedAt = evt.EventDate.AddMinutes(-Random.Shared.Next(5, 90)); // entered around kickoff
                    }
                    else if (roll < 95)
                    {
                        // No-show: valid ticket that was simply never scanned.
                        status = TicketStatuses.Active;
                    }
                    else
                    {
                        // Refunded/cancelled before the event.
                        status = TicketStatuses.Cancelled;
                        isActive = false;
                    }
                }

                var ticket = new Ticket
                {
                    Id = maxTicketId,
                    TicketNumber = $"TK{evt.Id:D2}{maxTicketId:D3}",
                    EventId = evt.Id,
                    SeatId = seat.Id,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    Price = evt.BaseTicketPrice ?? 50.00m,
                    PurchaseDate = purchaseDate,
                    CustomerName = $"Customer {maxTicketId}",
                    CustomerEmail = $"customer{maxTicketId}@example.com",
                    Status = status,
                    IsActive = isActive,
                    IsUsed = isUsed,
                    UsedAt = usedAt,
                    CreatedAt = DateTime.UtcNow,
                    // Legacy fields for compatibility
                    SeatNumber = seat.SeatCode,
                    Section = seat.Section?.SectionCode ?? "A",
                    Row = seat.RowNumber.ToString(),
                    EventName = evt.EventName,
                    EventDate = evt.EventDate
                };

                _context.Tickets.Add(ticket);
            }
        }

        await _context.SaveChangesAsync();

        // QR images are rendered on demand from each ticket's QRCodeToken (already set above) via
        // IQRCodeService.GetQrImageDataUriAsync. We intentionally do NOT persist a base64 image here:
        // the Ticket.QRCode column is varchar(500) and a PNG data URI overflows it, which previously
        // faulted the DbContext and cascaded failures through the rest of demo generation.
    }

    public async Task<int> BackfillPastEventTicketStatusesAsync()
    {
        var now = DateTime.UtcNow;

        // Only tickets whose event has already finished. Join Events so we can key off the real
        // event date and stamp a plausible UsedAt near kickoff.
        var pastTickets = await _context.Tickets
            .Join(_context.Events,
                t => t.EventId,
                e => e.Id,
                (t, e) => new { Ticket = t, e.EventDate })
            .Where(x => x.EventDate < now)
            .ToListAsync();

        foreach (var x in pastTickets)
        {
            var ticket = x.Ticket;

            // Same 85% Used / 10% Active (no-show) / 5% Cancelled split the seed generator uses.
            var roll = Random.Shared.Next(100); // 0-99
            if (roll < 85)
            {
                ticket.Status = TicketStatuses.Used;
                ticket.IsUsed = true;
                ticket.IsActive = true;
                ticket.UsedAt = x.EventDate.AddMinutes(-Random.Shared.Next(5, 90)); // entered around kickoff
            }
            else if (roll < 95)
            {
                // No-show: valid ticket that was simply never scanned.
                ticket.Status = TicketStatuses.Active;
                ticket.IsUsed = false;
                ticket.IsActive = true;
                ticket.UsedAt = null;
            }
            else
            {
                // Refunded/cancelled.
                ticket.Status = TicketStatuses.Cancelled;
                ticket.IsUsed = false;
                ticket.IsActive = false;
                ticket.UsedAt = null;
            }
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Backfilled lifecycle status for {Count} past-event ticket(s)", pastTickets.Count);
        return pastTickets.Count;
    }

    public async Task<bool> GenerateStaffAssignmentsAsync()
    {
        try
        {
            var events = await _context.Events.ToListAsync();
            var staff = await _context.Users.Where(u => u.Role == UserRole.Bartender || u.Role == UserRole.Waiter || u.Role == UserRole.Admin).ToListAsync();

            foreach (var evt in events)
            {
                foreach (var staffMember in staff.Take(3)) // Assign first 3 staff to each event
                {
                    var assignment = new EventStaffAssignment
                    {
                        EventId = evt.Id,
                        StaffId = staffMember.Id,
                        Role = staffMember.Username?.Contains("bartender") == true ? "Bartender" : 
                               staffMember.Username?.Contains("supervisor") == true ? "Supervisor" : "Waiter",
                        ShiftStart = evt.EventDate.AddHours(-2),
                        ShiftEnd = evt.EventDate.AddHours(4),
                        AssignedSections = "[\"A\", \"B\"]",
                        IsActive = true
                    };

                    var existingAssignment = await _context.EventStaffAssignments
                        .FirstOrDefaultAsync(esa => esa.EventId == evt.Id && esa.StaffId == staffMember.Id);

                    if (existingAssignment == null)
                    {
                        _context.EventStaffAssignments.Add(assignment);
                    }
                }
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating staff assignments");
            return false;
        }
    }

    public async Task<bool> GenerateTestOrdersAsync(int orderCount = 20)
    {
        try
        {
            var now = DateTime.UtcNow;
            var customers = await _context.Users.Where(u => u.Role == UserRole.Customer).ToListAsync();
            var drinks = await _context.Drinks.Where(d => d.IsAvailable).ToListAsync();

            // Drink ordering is a game-day activity, so demo orders may only attach to events that
            // have already started. This mirrors the live-event invariant and guarantees no order is
            // ever dated before its event's start (a future event gets no orders at all).
            var tickets = await _context.Tickets
                .Include(t => t.Event)
                .Where(t => t.Event != null && t.Event.EventDate <= now)
                .Take(20)
                .ToListAsync();

            if (tickets.Count == 0)
            {
                _logger.LogInformation("No started events found; skipping demo drink order generation.");
                return true;
            }

            for (int i = 0; i < orderCount; i++)
            {
                var customer = customers[Random.Shared.Next(customers.Count)];
                var ticket = tickets[Random.Shared.Next(tickets.Count)];
                var evt = ticket.Event!;

                // Stamp the order somewhere inside the event's live window: never before the start,
                // never after the event ended, and never in the future.
                var windowEnd = evt.EventEndDate ?? evt.EventDate.AddHours(4);
                if (windowEnd > now) windowEnd = now;
                var spanSeconds = Math.Max(0, (windowEnd - evt.EventDate).TotalSeconds);
                var createdAt = DateTime.SpecifyKind(
                    evt.EventDate.AddSeconds(Random.Shared.NextDouble() * spanSeconds),
                    DateTimeKind.Utc);

                var order = new Order
                {
                    TicketNumber = ticket.TicketNumber,
                    SeatNumber = ticket.SeatNumber ?? "A1",
                    CustomerId = customer.Id,
                    TotalAmount = 0, // Will be calculated
                    Status = (OrderStatus)Random.Shared.Next(1, 8),
                    CreatedAt = createdAt,
                    EventId = ticket.EventId,
                    SeatId = ticket.SeatId
                };

                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                // Add order items
                var itemCount = Random.Shared.Next(1, 4);
                decimal totalAmount = 0;

                for (int j = 0; j < itemCount; j++)
                {
                    var drink = drinks[Random.Shared.Next(drinks.Count)];
                    var quantity = Random.Shared.Next(1, 3);
                    var totalPrice = drink.Price * quantity;
                    totalAmount += totalPrice;

                    var orderItem = new OrderItem
                    {
                        OrderId = order.Id,
                        DrinkId = drink.Id,
                        Quantity = quantity,
                        UnitPrice = drink.Price,
                        TotalPrice = totalPrice
                    };

                    _context.OrderItems.Add(orderItem);
                }

                order.TotalAmount = totalAmount;
                await _context.SaveChangesAsync();
            }

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating test orders");
            return false;
        }
    }

    private async Task GenerateOrderSessionsAsync()
    {
        var tickets = await _context.Tickets.Take(5).ToListAsync();

        foreach (var ticket in tickets)
        {
            var session = new OrderSession
            {
                TicketId = ticket.Id,
                SessionToken = Guid.NewGuid().ToString(),
                CreatedAt = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(10, 60)),
                ExpiresAt = DateTime.UtcNow.AddHours(Random.Shared.Next(1, 3)),
                IsActive = Random.Shared.NextDouble() > 0.3, // 70% active
                IpAddress = $"192.168.1.{Random.Shared.Next(1, 255)}",
                UserAgent = "Mozilla/5.0 (Mobile)",
                CartData = "[]",
                CartTotal = 0,
                ItemCount = 0,
                LastActivity = DateTime.UtcNow.AddMinutes(-Random.Shared.Next(1, 30))
            };

            _context.OrderSessions.Add(session);
        }

        await _context.SaveChangesAsync();
    }

    private async Task GenerateNotificationsAsync()
    {
        var users = await _context.Users.ToListAsync();
        var events = await _context.Events.ToListAsync();

        var notifications = new[]
        {
            new Notification { UserId = null, Type = "SystemUpdate", Title = "System Maintenance", Message = "System will be updated tonight at 2 AM", Priority = "High", CreatedAt = DateTime.UtcNow.AddHours(-2) },
            new Notification { UserId = users.First().Id, Type = "OrderReady", Title = "Order Ready", Message = "Your order #123 is ready for pickup", Priority = "Normal", CreatedAt = DateTime.UtcNow.AddMinutes(-30) },
            new Notification { UserId = null, Type = "EventAlert", Title = "New Event", Message = "New concert added this weekend!", Priority = "Normal", EventId = events.First().Id, CreatedAt = DateTime.UtcNow.AddHours(-1) }
        };

        foreach (var notification in notifications)
        {
            _context.Notifications.Add(notification);
        }

        await _context.SaveChangesAsync();
    }

    private async Task GenerateAnalyticsDataAsync()
    {
        _logger.LogInformation("Generating analytics data...");
        
        var events = await _context.Events.ToListAsync();

        foreach (var evt in events)
        {
            // Check if analytics already exists
            var existingAnalytics = await _context.EventAnalytics
                .FirstOrDefaultAsync(a => a.EventId == evt.Id);
                
            if (existingAnalytics != null)
                continue;

            var analytics = new EventAnalytics
            {
                EventId = evt.Id,
                TotalTicketsSold = Random.Shared.Next(50, Math.Min(300, evt.TotalSeats)),
                TotalOrders = Random.Shared.Next(20, 100),
                TotalDrinksSold = Random.Shared.Next(100, 500),
                TicketRevenue = Random.Shared.Next(3000, 20000),
                DrinksRevenue = Random.Shared.Next(800, 4000),
                AverageOrderValue = Random.Shared.Next(25, 150),
                PeakOrderTime = evt.EventDate.AddHours(-2).AddMinutes(Random.Shared.Next(0, 120)),
                MostPopularDrink = new[] { "Beer", "Soft Drink", "Water", "Coffee" }[Random.Shared.Next(4)],
                CalculatedAt = DateTime.UtcNow
            };
            
            // Calculate total revenue from individual components
            analytics.TotalRevenue = analytics.TicketRevenue + analytics.DrinksRevenue;

            _context.EventAnalytics.Add(analytics);
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation("Analytics data generated for {Count} events", events.Count);
    }

    public async Task<bool> GenerateEventWithTicketsAsync(string eventName, DateTime eventDate, int ticketCount = 50)
    {
        try
        {
            var evt = new Event
            {
                EventName = eventName,
                EventType = "Custom",
                EventDate = eventDate,
                TotalSeats = ticketCount,
                BaseTicketPrice = 50.00m,
                Description = $"Custom event: {eventName}",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.Events.Add(evt);
            await _context.SaveChangesAsync();

            // Generate tickets
            var seats = await _context.Seats.Take(ticketCount).ToListAsync();
            for (int i = 0; i < Math.Min(ticketCount, seats.Count); i++)
            {
                var ticket = new Ticket
                {
                    TicketNumber = $"CUSTOM{evt.Id:D2}{i + 1:D3}",
                    EventId = evt.Id,
                    SeatId = seats[i].Id,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    Price = evt.BaseTicketPrice ?? 50.00m,
                    PurchaseDate = DateTime.UtcNow,
                    CustomerName = $"Customer {i + 1}",
                    CustomerEmail = $"customer{i + 1}@example.com",
                    Status = "Active",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    SeatNumber = seats[i].SeatCode,
                    Section = seats[i].Section?.SectionCode ?? "A",
                    Row = seats[i].RowNumber.ToString(),
                    EventName = evt.EventName,
                    EventDate = evt.EventDate
                };

                _context.Tickets.Add(ticket);
            }

            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating event with tickets");
            return false;
        }
    }

    public async Task<DinamoSeasonSeedResult> GenerateDinamoSeason2025Async()
    {
        const string homeTeam = "GNK Dinamo";

        // Every GNK Dinamo home fixture of the 2025/26 SuperSport HNL, in kickoff order. Kickoff
        // times are Zagreb local time; final scores are the real results (source: HNS semafor).
        // Tuple: (year, month, day, hour, minute, awayTeam, dinamoGoals, awayGoals).
        var fixtures = new (int Y, int Mo, int D, int H, int Min, string Away, int Gf, int Ga)[]
        {
            (2025,  8,  8, 21,  0, "HNK Vukovar 1991",     3, 0),
            (2025,  8, 23, 21,  0, "NK Istra 1961",        3, 0),
            (2025,  9, 14, 19, 15, "HNK Gorica",           1, 2),
            (2025,  9, 28, 18, 15, "NK Slaven Belupo",     4, 1),
            (2025, 10, 18, 18,  0, "NK Osijek",            2, 1),
            (2025, 11,  1, 16,  0, "HNK Rijeka",           2, 1),
            (2025, 11, 22, 15,  0, "NK Varaždin",          3, 1),
            (2025, 12,  6, 15,  0, "HNK Hajduk Split",     1, 1),
            (2025, 12, 20, 17, 45, "NK Lokomotiva Zagreb", 2, 0),
            (2026,  2,  2, 18,  0, "HNK Vukovar 1991",     3, 1),
            (2026,  2, 14, 15,  0, "NK Istra 1961",        4, 0),
            (2026,  3,  1, 17, 15, "HNK Gorica",           4, 2),
            (2026,  3, 14, 17, 15, "NK Slaven Belupo",     4, 2),
            (2026,  4,  4, 15,  0, "NK Osijek",            7, 0),
            (2026,  4, 18, 16,  0, "HNK Rijeka",           2, 2),
            (2026,  4, 26, 18, 45, "NK Varaždin",          2, 1),
            (2026,  5,  9, 16,  0, "HNK Hajduk Split",     2, 0),
            (2026,  5, 23, 18, 45, "NK Lokomotiva Zagreb", 0, 0),
        };

        var homeClub = await EnsureDinamoHomeClubAsync();
        var season = await EnsureDinamoSeasonAsync();
        var zagreb = ResolveZagrebTimeZone();

        // Capacity barely matters for display (the event API recomputes it from the drawn stadium),
        // but store a sensible figure: the real seat count when a stadium exists, else Maksimir's.
        var seatCount = await _context.Seats.CountAsync();
        var totalSeats = seatCount > 0 ? seatCount : 24851;

        int created = 0, skipped = 0;
        foreach (var f in fixtures)
        {
            var kickoffLocal = new DateTime(f.Y, f.Mo, f.D, f.H, f.Min, 0, DateTimeKind.Unspecified);
            var kickoffUtc = TimeZoneInfo.ConvertTimeToUtc(kickoffLocal, zagreb);

            // Date makes the name unique across the two home meetings with each opponent.
            var name = $"GNK Dinamo – {f.Away} ({kickoffLocal:dd.MM.yyyy.})";
            if (await _context.Events.AnyAsync(e => e.EventName == name))
            {
                skipped++;
                continue;
            }

            _context.Events.Add(new Event
            {
                EventName = name,
                EventType = "Match",
                HomeTeam = homeTeam,
                AwayTeam = f.Away,
                EventDate = kickoffUtc,
                EventEndDate = kickoffUtc.AddHours(2),
                VenueId = homeClub.VenueId,
                TotalSeats = totalSeats,
                BaseTicketPrice = f.Away.Contains("Hajduk") ? 25.00m : 15.00m,
                Description = "SuperSport HNL 2025/26 · domaća utakmica na Maksimiru · " +
                              $"konačni rezultat: GNK Dinamo {f.Gf}:{f.Ga} {f.Away}.",
                IsActive = false,               // past fixture — not published for sale
                Status = EventStatus.Completed, // already played
                SeasonId = season.Id,
                SourceSystem = "SuperSport HNL 2025/26",
                CreatedAt = DateTime.UtcNow
            });
            created++;
        }

        await _context.SaveChangesAsync();
        _logger.LogInformation(
            "Seeded GNK Dinamo 2025/26 season {Season}: {Created} events created, {Skipped} already present.",
            season.Name, created, skipped);

        return new DinamoSeasonSeedResult(season.Id, season.Name, homeClub.Name, fixtures.Length, created, skipped);
    }

    /// <summary>
    /// Ensures GNK Dinamo is the venue's primary resident club (creating the singleton venue and the
    /// club if missing), and gives the venue a Maksimir identity when it still carries default values.
    /// Returns the Dinamo <see cref="Club"/>.
    /// </summary>
    private async Task<Club> EnsureDinamoHomeClubAsync()
    {
        var venue = await _context.Venues.Include(v => v.Clubs).FirstOrDefaultAsync();
        if (venue == null)
        {
            venue = new Venue { Name = "Stadion Maksimir", CreatedAt = DateTime.UtcNow };
            _context.Venues.Add(venue);
            await _context.SaveChangesAsync();
        }

        // Only stamp venue identity when it's still the seeded default, so a customised venue is kept.
        if (string.IsNullOrWhiteSpace(venue.Name) || venue.Name == "Stadium")
            venue.Name = "Stadion Maksimir";
        if (string.IsNullOrWhiteSpace(venue.ClubName))
            venue.ClubName = "GNK Dinamo";
        if (string.IsNullOrWhiteSpace(venue.City)) venue.City = "Zagreb";
        if (string.IsNullOrWhiteSpace(venue.Country)) venue.Country = "Hrvatska";

        var dinamo = venue.Clubs.FirstOrDefault(c =>
            c.Name.Contains("Dinamo", StringComparison.OrdinalIgnoreCase));
        if (dinamo == null)
        {
            dinamo = new Club
            {
                VenueId = venue.Id,
                Name = "GNK Dinamo",
                ShortName = "DIN",
                PrimaryColor = "#0a3d91", // Dinamo blue
                SecondaryColor = "#ffffff",
                FoundedYear = 1945,
                Website = "https://gnkdinamo.hr",
                CreatedAt = DateTime.UtcNow
            };
            _context.Clubs.Add(dinamo);
        }

        // Make Dinamo the one primary club at the venue.
        foreach (var c in venue.Clubs)
            c.IsPrimary = false;
        dinamo.IsPrimary = true;
        dinamo.DisplayOrder = 0;

        await _context.SaveChangesAsync();
        return dinamo;
    }

    /// <summary>
    /// Finds the "2025/26" season, creating it if absent. This is a <em>historical</em> (already
    /// finished) season, so it is never flagged current and the "current" flag on other seasons is
    /// left untouched — seeding past data must not change which season the UIs default to.
    /// </summary>
    private async Task<Season> EnsureDinamoSeasonAsync()
    {
        var season = await _context.Seasons.FirstOrDefaultAsync(s => s.Name == "2025/26");
        if (season != null)
            return season;

        season = new Season
        {
            Name = "2025/26",
            StartDate = new DateTime(2025, 8, 1, 0, 0, 0, DateTimeKind.Utc),
            EndDate = new DateTime(2026, 5, 31, 0, 0, 0, DateTimeKind.Utc),
            IsCurrent = false,
            SourceSystem = "SuperSport HNL 2025/26",
            CreatedAt = DateTime.UtcNow
        };
        _context.Seasons.Add(season);
        await _context.SaveChangesAsync();
        return season;
    }

    /// <summary>
    /// Resolves the Zagreb time zone across OSes (Windows uses "Central European Standard Time",
    /// Linux/containers use the IANA "Europe/Zagreb"), so kickoff times convert to UTC with correct
    /// summer/winter offsets. Falls back to a fixed UTC+1 zone if neither id is available.
    /// </summary>
    private static TimeZoneInfo ResolveZagrebTimeZone()
    {
        foreach (var id in new[] { "Europe/Zagreb", "Central European Standard Time" })
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById(id); }
            catch (TimeZoneNotFoundException) { }
            catch (InvalidTimeZoneException) { }
        }
        return TimeZoneInfo.CreateCustomTimeZone("CET-fallback", TimeSpan.FromHours(1), "CET", "CET");
    }

    public async Task<bool> ClearDemoDataAsync()
    {
        try
        {
            _logger.LogInformation("Starting comprehensive data cleanup...");

            // Clear in proper dependency order
            _context.OrderItems.RemoveRange(await _context.OrderItems.ToListAsync());
            _context.Orders.RemoveRange(await _context.Orders.ToListAsync());
            _context.Payments.RemoveRange(await _context.Payments.ToListAsync());
            _context.CartItems.RemoveRange(await _context.CartItems.ToListAsync());
            _context.ShoppingCarts.RemoveRange(await _context.ShoppingCarts.ToListAsync());
            _context.SeatReservations.RemoveRange(await _context.SeatReservations.ToListAsync());
            _context.TicketSessions.RemoveRange(await _context.TicketSessions.ToListAsync());
            _context.OrderSessions.RemoveRange(await _context.OrderSessions.ToListAsync());
            _context.Notifications.RemoveRange(await _context.Notifications.ToListAsync());
            _context.EventAnalytics.RemoveRange(await _context.EventAnalytics.ToListAsync());
            _context.EventStaffAssignments.RemoveRange(await _context.EventStaffAssignments.ToListAsync());
            _context.Tickets.RemoveRange(await _context.Tickets.ToListAsync());
            _context.Events.RemoveRange(await _context.Events.ToListAsync());
            
            // Clear all users except admin (ID = 1)
            _context.Users.RemoveRange(await _context.Users.Where(u => u.Id != 1).ToListAsync());
            
            // Clear drinks except seeded ones (let's keep all seeded drinks)
            _context.Drinks.RemoveRange(await _context.Drinks.Where(d => d.Id > 8).ToListAsync()); 

            // Clear logs but keep recent system logs
            var oldLogs = await _context.LogEntries.Where(l => l.Timestamp < DateTime.UtcNow.AddHours(-24)).ToListAsync();
            _context.LogEntries.RemoveRange(oldLogs);

            await _context.SaveChangesAsync();

            // Keep stadium structure intact:
            // - Tribunes, Rings, Sectors, StadiumSeatsNew
            // - StadiumSeats, StadiumSections, Seats
            _logger.LogInformation("Comprehensive data cleanup completed. Stadium structure preserved.");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error clearing data comprehensively");
            return false;
        }
    }

    public async Task<bool> GenerateDemoDataForEventAsync(int eventId)
    {
        try
        {
            _logger.LogInformation("Generating demo data for event {EventId}", eventId);

            var eventExists = await _context.Events.AnyAsync(e => e.Id == eventId);
            if (!eventExists)
            {
                _logger.LogWarning("Event {EventId} not found", eventId);
                return false;
            }

            // Generate demo tickets for the event
            var availableSeats = await _context.Seats.Where(s => !_context.Tickets.Any(t => t.SeatId == s.Id && t.EventId == eventId)).Take(20).ToListAsync();
            
            for (int i = 0; i < availableSeats.Count; i++)
            {
                var seat = availableSeats[i];
                var ticket = new Ticket
                {
                    TicketNumber = $"DEMO{eventId:D2}{i + 1:D3}",
                    EventId = eventId,
                    SeatId = seat.Id,
                    QRCodeToken = Guid.NewGuid().ToString(),
                    Price = 45.00m + (i % 3 * 15.00m), // Varying prices
                    PurchaseDate = DateTime.UtcNow.AddDays(-Random.Shared.Next(0, 30)),
                    CustomerName = $"Demo Customer {i + 1}",
                    CustomerEmail = $"demo{i + 1}@example.com",
                    Status = "Active",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    SeatNumber = seat.SeatCode,
                    Section = seat.Section?.SectionCode ?? "A",
                    Row = seat.RowNumber.ToString()
                };

                // Set event details from the event
                var eventEntity = await _context.Events.FindAsync(eventId);
                if (eventEntity != null)
                {
                    ticket.EventName = eventEntity.EventName;
                    ticket.EventDate = eventEntity.EventDate;
                }

                _context.Tickets.Add(ticket);
            }

            // Generate demo orders for the event
            for (int i = 0; i < 5; i++)
            {
                var order = new Order
                {
                    EventId = eventId,
                    TicketNumber = $"ORD{eventId:D2}{i + 1:D3}",
                    SeatNumber = $"A{i + 1}",
                    CustomerId = 104 + (i % 2), // Use demo customers
                    TotalAmount = 25.50m + (i * 5.25m),
                    Status = (OrderStatus)((i % 5) + 1), // Various statuses (1-5)
                    CreatedAt = DateTime.UtcNow.AddHours(-Random.Shared.Next(1, 24)),
                    Notes = $"Demo order {i + 1} for event {eventId}",
                    CustomerNotes = $"Generated demo order",
                    DeliveryNotes = $"Section {(char)('A' + (i % 4))}, Row {i + 1}"
                };

                _context.Orders.Add(order);
            }

            await _context.SaveChangesAsync();
            _logger.LogInformation("Demo data generated successfully for event {EventId}", eventId);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating demo data for event {EventId}", eventId);
            return false;
        }
    }

    public async Task<int> GenerateDrinkSalesForCompletedEventsAsync()
    {
        try
        {
            var completedEvents = await _context.Events
                .Where(e => e.Status == EventStatus.Completed)
                .ToListAsync();

            if (completedEvents.Count == 0)
            {
                _logger.LogInformation("No completed events found; no drink sales generated.");
                return 0;
            }

            var drinks = await _context.Drinks.Where(d => d.IsAvailable).ToListAsync();
            if (drinks.Count == 0)
            {
                _logger.LogWarning("No available drinks found; cannot generate drink sales for completed events.");
                return 0;
            }

            // Orders reference a real customer (FK to Users). Prefer customers; fall back to any user so
            // seeding still works on a fresh DB that only has the admin account.
            var customers = await _context.Users.Where(u => u.Role == UserRole.Customer).ToListAsync();
            if (customers.Count == 0)
                customers = await _context.Users.ToListAsync();
            if (customers.Count == 0)
            {
                _logger.LogWarning("No users found; cannot attribute drink sales to a customer.");
                return 0;
            }

            // Idempotency: skip any completed event that already has a non-cancelled drink order, so
            // re-running this never doubles up sales.
            var completedIds = completedEvents.Select(e => e.Id).ToList();
            var eventsWithSales = (await _context.Orders
                    .Where(o => o.EventId != null
                                && completedIds.Contains(o.EventId.Value)
                                && o.Status != OrderStatus.Cancelled)
                    .Select(o => o.EventId!.Value)
                    .Distinct()
                    .ToListAsync())
                .ToHashSet();

            var eventsSeeded = 0;

            foreach (var evt in completedEvents)
            {
                if (eventsWithSales.Contains(evt.Id))
                    continue;

                // A completed event's sales all happened inside its live window (never before the
                // start, never after it ended). Kind=Utc keeps Npgsql's timestamptz columns happy.
                var windowStart = DateTime.SpecifyKind(evt.EventDate, DateTimeKind.Utc);
                var windowEnd = DateTime.SpecifyKind(evt.EventEndDate ?? evt.EventDate.AddHours(4), DateTimeKind.Utc);
                var spanSeconds = Math.Max(0, (windowEnd - windowStart).TotalSeconds);

                var orderCount = Random.Shared.Next(6, 16); // some, but varied, sales per event
                for (int i = 0; i < orderCount; i++)
                {
                    var customer = customers[Random.Shared.Next(customers.Count)];
                    var createdAt = DateTime.SpecifyKind(
                        windowStart.AddSeconds(Random.Shared.NextDouble() * spanSeconds),
                        DateTimeKind.Utc);

                    var order = new Order
                    {
                        EventId = evt.Id,
                        CustomerId = customer.Id,
                        TicketNumber = $"DRK{evt.Id:D2}-{i + 1:D3}",
                        SeatNumber = $"{(char)('A' + Random.Shared.Next(0, 4))}{Random.Shared.Next(1, 30)}",
                        TotalAmount = 0, // set once items are priced
                        Status = OrderStatus.Delivered, // the event is over, so sales are fulfilled
                        CreatedAt = createdAt,
                        DeliveredAt = createdAt.AddMinutes(Random.Shared.Next(5, 20)),
                        Notes = "Seeded drink sale for completed event"
                    };

                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();

                    decimal totalAmount = 0;
                    var itemCount = Random.Shared.Next(1, 4);
                    for (int j = 0; j < itemCount; j++)
                    {
                        var drink = drinks[Random.Shared.Next(drinks.Count)];
                        var quantity = Random.Shared.Next(1, 4);
                        var totalPrice = drink.Price * quantity;
                        totalAmount += totalPrice;

                        _context.OrderItems.Add(new OrderItem
                        {
                            OrderId = order.Id,
                            DrinkId = drink.Id,
                            Quantity = quantity,
                            UnitPrice = drink.Price,
                            TotalPrice = totalPrice
                        });
                    }

                    order.TotalAmount = totalAmount;
                    await _context.SaveChangesAsync();
                }

                eventsSeeded++;
            }

            _logger.LogInformation(
                "Generated drink sales for {Seeded} of {Total} completed events (others already had sales).",
                eventsSeeded, completedEvents.Count);
            return eventsSeeded;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating drink sales for completed events");
            return 0;
        }
    }

    // ---- Match-day drink sales for one event ----------------------------------------------------

    /// <summary>Marks orders produced by the match-day generator, so a re-run can recognise them.</summary>
    private const string MatchDaySalesNote = "Match-day simulated drink order";

    /// <summary>
    /// Share of attending fans who order at least one drink to their seat, and the share of those
    /// buyers who come back for a second round. In-seat delivery is a minority behaviour even at a
    /// busy match — most of the crowd either uses the concourse or doesn't drink.
    /// </summary>
    private const double BuyerShare = 0.34;
    private const double SecondRoundShare = 0.22;

    /// <summary>
    /// When during the match orders land, as (fromMinute, toMinute, weight) after kick-off. Half-time
    /// is the spike every stadium bar plans for; the rest builds up to it and tails off after.
    /// </summary>
    private static readonly (int From, int To, int Weight)[] MatchDayProfile =
    {
        (0, 15, 18),    // early arrivals settling in
        (15, 45, 22),   // first half
        (45, 60, 35),   // half-time rush
        (60, 90, 20),   // second half
        (90, 115, 5)    // final stretch / full time
    };

    /// <summary>
    /// Relative popularity by drink category at a football match. Keyed by category name so a newly
    /// added drink inherits its category's weight; anything uncategorised gets a small share rather
    /// than being excluded.
    /// </summary>
    private static readonly Dictionary<string, int> CategoryPopularity = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Beer"] = 40,
        ["SoftDrink"] = 20,
        ["Water"] = 15,
        ["Juice"] = 8,
        ["EnergyDrink"] = 7,
        ["Coffee"] = 6,
        ["Tea"] = 4
    };
    private const int DefaultCategoryPopularity = 5;

    public async Task<EventDrinkSalesResult> GenerateMatchDayDrinkSalesForEventAsync(int eventId, bool replaceExisting = false)
    {
        try
        {
            var evt = await _context.Events.FirstOrDefaultAsync(e => e.Id == eventId);
            if (evt == null)
                return new EventDrinkSalesResult(false, $"Event {eventId} not found");

            // Read the catalogue before anything is deleted below: a venue with no drinks can't be
            // given a match day, and finding that out after the previous run's orders were dropped
            // would leave the fixture with neither the old sales nor new ones.
            var drinks = await _context.Drinks
                .Include(d => d.Category)
                .Where(d => d.IsAvailable)
                .ToListAsync();
            if (drinks.Count == 0)
                return new EventDrinkSalesResult(false, "No available drinks to sell", eventId, evt.EventName);

            var existing = await _context.Orders.Where(o => o.EventId == eventId).ToListAsync();
            if (existing.Count > 0)
            {
                if (!replaceExisting)
                {
                    return new EventDrinkSalesResult(false,
                        $"Event {eventId} already has {existing.Count} order(s). Pass replaceExisting to regenerate.",
                        eventId, evt.EventName);
                }

                var staleIds = existing.Select(o => o.Id).ToList();
                await UndoStockLedgerAsync(staleIds);
                await UndoWalletLedgerAsync(staleIds);
                _context.OrderItems.RemoveRange(
                    await _context.OrderItems.Where(i => staleIds.Contains(i.OrderId)).ToListAsync());
                _context.Orders.RemoveRange(existing);
                await _context.SaveChangesAsync();
                _logger.LogInformation("Removed {Count} existing orders for event {EventId} before regenerating",
                    existing.Count, eventId);
            }

            // The crowd: seats actually occupied for this match (season-derived tickets included —
            // a pass holder buys drinks like anyone else).
            var attendees = await _context.Tickets
                .AsNoTracking()
                .Where(t => t.EventId == eventId && t.Status != TicketStatuses.Cancelled)
                .Select(t => new
                {
                    TicketId = t.Id,
                    t.TicketNumber,
                    t.SeatId,
                    t.CustomerEmail,
                    Row = t.Seat != null ? t.Seat.RowNumber : 0,
                    Number = t.Seat != null ? t.Seat.SeatNumber : 0
                })
                .ToListAsync();

            if (attendees.Count == 0)
                return new EventDrinkSalesResult(false, "Event has no attendees to order drinks", eventId, evt.EventName);

            // Orders carry a customer FK. Match the ticket holder's account by email where we have one;
            // otherwise the order is attributed to some customer account (a walk-up buying at the seat).
            // Grouped rather than keyed directly: nothing guarantees one account per email address,
            // and a duplicate would otherwise throw here instead of just picking the first match.
            var customersByEmail = (await _context.Users
                    .Where(u => u.Role == UserRole.Customer)
                    .Select(u => new { u.Email, u.Id })
                    .ToListAsync())
                .GroupBy(u => u.Email, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(g => g.Key, g => g.First().Id, StringComparer.OrdinalIgnoreCase);
            var customerIds = customersByEmail.Values.ToList();
            if (customerIds.Count == 0)
                customerIds = await _context.Users.Select(u => u.Id).ToListAsync();
            if (customerIds.Count == 0)
                return new EventDrinkSalesResult(false, "No user to attribute orders to", eventId, evt.EventName);

            // The match window: kick-off to full time, clamped inside the event's own window so no
            // order is ever timestamped outside the event it belongs to.
            var kickOff = DateTime.SpecifyKind(evt.EventDate, DateTimeKind.Utc);
            var windowEnd = DateTime.SpecifyKind(evt.EventEndDate ?? evt.EventDate.AddHours(2), DateTimeKind.Utc);
            var matchMinutes = Math.Min(115, Math.Max(1, (windowEnd - kickOff).TotalMinutes));
            var profileScale = matchMinutes / 115.0;

            // Who buys, and who comes back for a second round.
            var buyerCount = Math.Max(1, (int)Math.Round(attendees.Count * BuyerShare));
            var buyers = attendees.OrderBy(_ => Random.Shared.Next()).Take(buyerCount).ToList();
            var repeatCount = (int)Math.Round(buyerCount * SecondRoundShare);
            var orderSeats = buyers
                .Concat(buyers.OrderBy(_ => Random.Shared.Next()).Take(repeatCount))
                .ToList();

            var drinkWeights = drinks
                .Select(d => CategoryPopularity.GetValueOrDefault(d.Category?.Name ?? "", DefaultCategoryPopularity))
                .ToArray();

            var orders = new List<Order>();
            var items = new List<OrderItem>();

            // Which ticket each order was placed from. Carried by reference rather than looked up from
            // Order.TicketNumber later, because a ticket-owned wallet is addressed by ticket id and the
            // order only keeps the (truncatable) number.
            var ticketIdByOrder = new Dictionary<Order, int>();

            int delivered = 0, cancelled = 0, deliveryFailed = 0, drinksSold = 0;
            decimal revenue = 0m;

            // Orders that couldn't be matched to their seat's ticket holder and had to be attributed to
            // an arbitrary account. A handful is normal (a ticket with no email); all of them means the
            // holders were never provisioned accounts, and every order in the ground lands on whoever
            // the fallback happens to pick — worth saying out loud rather than shipping silently.
            var unattributed = 0;

            for (var i = 0; i < orderSeats.Count; i++)
            {
                var seat = orderSeats[i];
                var placedAt = kickOff.AddMinutes(PickMatchMinute() * profileScale);

                var order = new Order
                {
                    EventId = eventId,
                    SeatId = seat.SeatId,
                    CustomerId = ResolveCustomerId(seat.CustomerEmail),
                    TicketNumber = Truncate(seat.TicketNumber, 50),
                    SeatNumber = Truncate($"R{seat.Row}S{seat.Number}", 10),
                    TotalAmount = 0, // priced once its items exist
                    Status = OrderStatus.Delivered,
                    CreatedAt = placedAt,
                    Notes = MatchDaySalesNote
                };

                // A finished match leaves no order mid-flight: each one either reached the fan, was
                // cancelled, or came back to the bar as a failed delivery.
                var outcome = Random.Shared.Next(100);
                if (outcome < 4)
                {
                    order.Status = OrderStatus.Cancelled;
                    order.AcceptedAt = placedAt.AddMinutes(Random.Shared.Next(1, 3));
                    order.CancelledAt = placedAt.AddMinutes(Random.Shared.Next(3, 9));
                    cancelled++;
                }
                else if (outcome < 7)
                {
                    order.Status = OrderStatus.DeliveryFailed;
                    order.AcceptedAt = placedAt.AddMinutes(Random.Shared.Next(1, 3));
                    order.InPreparationAt = placedAt.AddMinutes(Random.Shared.Next(3, 6));
                    order.PreparedAt = placedAt.AddMinutes(Random.Shared.Next(6, 10));
                    order.DeliveryAttempts = Random.Shared.Next(1, 3);
                    order.LastDeliveryAttemptAt = placedAt.AddMinutes(Random.Shared.Next(10, 20));
                    order.LastDeliveryFailureReason = Random.Shared.Next(10) < 7
                        ? DeliveryFailureReason.CustomerNotAtSeat
                        : DeliveryFailureReason.CustomerRefused;
                    deliveryFailed++;
                }
                else
                {
                    order.AcceptedAt = placedAt.AddMinutes(Random.Shared.Next(1, 3));
                    order.InPreparationAt = placedAt.AddMinutes(Random.Shared.Next(3, 6));
                    order.PreparedAt = placedAt.AddMinutes(Random.Shared.Next(6, 11));
                    order.DeliveredAt = placedAt.AddMinutes(Random.Shared.Next(11, 22));
                    order.ActualDeliveryTime = order.DeliveredAt;
                    delivered++;
                }

                orders.Add(order);
                ticketIdByOrder[order] = seat.TicketId;
            }

            _context.Orders.AddRange(orders);
            await _context.SaveChangesAsync(); // assigns order ids

            foreach (var order in orders)
            {
                // In-seat baskets are small: usually one line, sometimes a couple for the row.
                var lineCount = WeightedPick(new[] { 1, 2, 3 }, new[] { 55, 32, 13 });
                var chosen = new HashSet<int>();
                decimal orderTotal = 0m;

                for (var line = 0; line < lineCount; line++)
                {
                    var drink = drinks[WeightedIndex(drinkWeights)];
                    if (!chosen.Add(drink.Id))
                        continue; // same drink twice in one basket is just a bigger quantity

                    var quantity = WeightedPick(new[] { 1, 2, 3, 4 }, new[] { 45, 35, 14, 6 });
                    var lineTotal = drink.Price * quantity;
                    orderTotal += lineTotal;
                    drinksSold += quantity;

                    items.Add(new OrderItem
                    {
                        OrderId = order.Id,
                        DrinkId = drink.Id,
                        Quantity = quantity,
                        UnitPrice = drink.Price,
                        TotalPrice = lineTotal
                    });
                }

                order.TotalAmount = orderTotal;
                // Cancelled orders never became revenue; the statistics already exclude them, but keep
                // the running total here honest too.
                if (order.Status != OrderStatus.Cancelled)
                    revenue += orderTotal;
            }

            // Every line moves real stock, exactly as a live order does — the bar is drawn down as the
            // match is played and topped up by a delivery beforehand. Without this a fixture would sell
            // thousands of drinks out of thin air: stock levels never move and the drink's history in
            // Admin → Drinks stays empty, which is the one place the sales should be most visible.
            // Added to the same SaveChanges as the lines, so orders, items and ledger commit together.
            var movements = BuildMatchDayStockLedger(evt, kickOff, orders, items, drinks);
            _context.StockMovements.AddRange(movements);

            _context.OrderItems.AddRange(items);
            await _context.SaveChangesAsync();

            // Part of the crowd pays from a stored-value balance rather than at the bar, so the fixture
            // leaves funded wallets and a spend ledger behind it. Runs last because it prices its
            // deposits from the orders' final totals, which only exist once the lines above are priced.
            var walletSeed = await SeedMatchDayWalletsAsync(evt, kickOff, orders, ticketIdByOrder);

            _logger.LogInformation(
                "Generated {Orders} match-day drink orders ({Items} lines, {Drinks} drinks, {Revenue:C}) for event {EventId}, " +
                "{Movements} stock movement(s), {Wallets} wallet(s) funded with {Funded:C} across {Payments} wallet payment(s)",
                orders.Count, items.Count, drinksSold, revenue, eventId, movements.Count,
                walletSeed.Wallets, walletSeed.Funded, walletSeed.Payments);

            if (unattributed > 0)
                _logger.LogWarning(
                    "{Unattributed} of {Orders} drink orders for event {EventId} could not be matched to the " +
                    "seat's ticket holder and were attributed to one of {Candidates} arbitrary customer " +
                    "account(s) — the holders have no account for their email. Every order lands on the same " +
                    "customer when only one candidate exists.",
                    unattributed, orders.Count, eventId, customerIds.Count);

            return new EventDrinkSalesResult(true, "Match-day drink sales generated",
                eventId, evt.EventName, attendees.Count, orders.Count, items.Count, drinksSold,
                revenue, delivered, cancelled, deliveryFailed);

            int ResolveCustomerId(string? email)
            {
                if (email != null && customersByEmail.TryGetValue(email, out var id))
                    return id;

                unattributed++;
                return customerIds[Random.Shared.Next(customerIds.Count)];
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating match-day drink sales for event {EventId}", eventId);
            return new EventDrinkSalesResult(false, $"Error: {ex.Message}", eventId);
        }
    }

    /// <summary>
    /// Stock a match-day delivery leaves over once the fixture has sold what it is going to sell, as a
    /// share of that fixture's demand and as an absolute floor. A bar that ends every match on exactly
    /// zero is as obviously seeded as one whose stock never moves.
    /// </summary>
    private const int MatchDayRestockTailPercent = 25;
    private const int MatchDayRestockMinimumTail = 12;

    /// <summary>How long before kick-off the match-day delivery lands, so it precedes the first sale.</summary>
    private const int MatchDayDeliveryHoursBeforeKickOff = 3;

    /// <summary>
    /// Writes the inventory ledger for a generated match day and leaves each drink on the balance the
    /// fixture ends with. Three kinds of movement come out of it, the same three a live match produces:
    /// a <see cref="StockMovementType.Restock"/> delivery before the gates open, a
    /// <see cref="StockMovementType.Sale"/> per order line, and a
    /// <see cref="StockMovementType.OrderCancelled"/> return for anything the fan sent back.
    ///
    /// The delivery is sized from the day's actual demand rather than guessed, which is why the ledger
    /// is built after the lines exist: a fixture must not drive stock negative, and topping up per sale
    /// as stock ran out would litter the history with dribbles instead of one delivery.
    ///
    /// Movements are applied in timestamp order so each row's <see cref="StockMovement.QuantityAfter"/>
    /// is the balance that actually followed it — the history table reads as a running total, and the
    /// drink's final <c>StockQuantity</c> is the last row's balance.
    /// </summary>
    private static List<StockMovement> BuildMatchDayStockLedger(
        Event evt, DateTime kickOff, List<Order> orders, List<OrderItem> items, List<Drink> drinks)
    {
        var drinksById = drinks.ToDictionary(d => d.Id);
        var ordersById = orders.ToDictionary(o => o.Id);

        var movements = new List<StockMovement>(items.Count + drinksById.Count);
        var balances = new Dictionary<int, int>();

        // The delivery: one row per drink the fixture will sell, timestamped before the first sale.
        var deliveredAt = kickOff.AddHours(-MatchDayDeliveryHoursBeforeKickOff);
        foreach (var line in items.GroupBy(i => i.DrinkId))
        {
            var drink = drinksById[line.Key];
            var demand = line.Sum(i => i.Quantity);
            var balance = drink.StockQuantity;

            var target = demand + Math.Max(MatchDayRestockMinimumTail, demand * MatchDayRestockTailPercent / 100);
            if (balance < target)
            {
                var delta = target - balance;
                balance += delta;
                movements.Add(new StockMovement
                {
                    DrinkId = drink.Id,
                    Delta = delta,
                    QuantityAfter = balance,
                    Type = StockMovementType.Restock,
                    Note = Truncate($"Match-day delivery — {evt.EventName}", 500),
                    CreatedAt = deliveredAt
                });
            }

            balances[drink.Id] = balance;
        }

        // The match: each line is reserved when its order is placed, and handed back if that order was
        // cancelled. A failed delivery is not a return — the drink was poured and never came back into
        // stock — which is exactly how the live order path treats it.
        var timeline = new List<(DateTime At, OrderItem Item, StockMovementType Type)>(items.Count);
        foreach (var item in items)
        {
            var order = ordersById[item.OrderId];
            timeline.Add((order.CreatedAt, item, StockMovementType.Sale));
            if (order.Status == OrderStatus.Cancelled)
                timeline.Add((order.CancelledAt ?? order.CreatedAt, item, StockMovementType.OrderCancelled));
        }

        foreach (var entry in timeline.OrderBy(e => e.At))
        {
            var delta = entry.Type == StockMovementType.Sale ? -entry.Item.Quantity : entry.Item.Quantity;
            var balance = balances[entry.Item.DrinkId] + delta;
            balances[entry.Item.DrinkId] = balance;

            movements.Add(new StockMovement
            {
                DrinkId = entry.Item.DrinkId,
                Delta = delta,
                QuantityAfter = balance,
                Type = entry.Type,
                OrderId = entry.Item.OrderId,
                CreatedAt = entry.At
            });
        }

        // Leave the catalogue on the balance the ledger just described.
        foreach (var (drinkId, balance) in balances)
            drinksById[drinkId].StockQuantity = balance;

        return movements;
    }

    /// <summary>
    /// Takes back the stock a set of soon-to-be-deleted orders moved, and drops their ledger rows.
    /// Regenerating a fixture must not drain the bar a second time, and history pointing at orders that
    /// no longer exist is worse than no history at all (the order link is nulled, not cascaded, so the
    /// rows would otherwise survive as unattributable sales).
    ///
    /// Only the order-linked rows go. The match-day delivery that fed them stays: that stock was really
    /// delivered, and leaving it means the replacement run finds enough on the shelf and doesn't write a
    /// second delivery for the same fixture.
    /// </summary>
    private async Task UndoStockLedgerAsync(List<int> orderIds)
    {
        if (orderIds.Count == 0)
            return;

        var stale = await _context.StockMovements
            .Where(m => m.OrderId != null && orderIds.Contains(m.OrderId.Value))
            .ToListAsync();
        if (stale.Count == 0)
            return;

        // Loaded by id rather than reusing the available-drinks list: a drink can have been withdrawn
        // from sale since the orders were generated, and its stock still has to come back.
        var affectedIds = stale.Select(m => m.DrinkId).Distinct().ToList();
        var affected = await _context.Drinks
            .Where(d => affectedIds.Contains(d.Id))
            .ToDictionaryAsync(d => d.Id);

        foreach (var movement in stale)
        {
            if (affected.TryGetValue(movement.DrinkId, out var drink))
                drink.StockQuantity -= movement.Delta;
        }

        _context.StockMovements.RemoveRange(stale);
    }

    // ---- Match-day stored-value wallets ---------------------------------------------------------

    /// <summary>
    /// Share of the fixture's drink buyers who pay from a stored-value balance instead of settling at
    /// the bar. Well short of everyone: the wallet is a convenience some of the crowd has adopted, and
    /// a ground where every single fan pays by wallet tests nothing about the mixed-tender reality.
    /// </summary>
    private const double WalletPayerShare = 0.45;

    /// <summary>
    /// Of those, the share paying from the anonymous balance loaded on their ticket rather than from a
    /// registered account wallet — the walk-up who put cash on the ticket at the gate.
    /// </summary>
    private const double TicketWalletPayerShare = 0.3;

    /// <summary>Spare change a fan leaves on the balance beyond what the round costs, in euro.</summary>
    private static readonly int[] TopUpFloatSteps = { 0, 10, 20 };

    /// <summary>What one call to <see cref="SeedMatchDayWalletsAsync"/> ended up creating.</summary>
    private sealed record MatchDayWalletSeed(int Wallets, int Payments, decimal Funded);

    /// <summary>One wallet's generated match day, held in memory until the whole batch is written.</summary>
    private sealed class WalletDraft
    {
        public required Wallet Wallet { get; init; }
        public required bool IsTicketOwned { get; init; }
        public required List<WalletTransaction> Entries { get; init; }
        /// <summary>The top-up entry, if this match day needed one — it gets its own funding Payment.</summary>
        public WalletTransaction? Deposit { get; init; }
        /// <summary>Order behind each spend entry, so the Payment rows can be built once ids exist.</summary>
        public required List<(WalletTransaction Entry, Order Order)> Spends { get; init; }
    }

    /// <summary>
    /// Gives part of a played fixture's crowd a stored-value balance they topped up before kick-off and
    /// paid their rounds from, so a generated season leaves funded wallets and a spend ledger behind —
    /// which is what Admin → Wallets reads. Both owner kinds are produced: registered fans' account
    /// wallets and the anonymous bearer balances loaded onto a ticket.
    ///
    /// Written in bulk rather than through <c>IWalletService</c>: that service opens a guarded
    /// transaction per entry, which is the right shape for one fan at a counter and the wrong one for a
    /// season's worth of fixtures over a remote database. The invariants it protects are honoured here
    /// instead — one wallet per owner, a chronological ledger whose running
    /// <see cref="WalletTransaction.BalanceAfter"/> ends on the wallet's <see cref="Wallet.Balance"/>,
    /// no balance ever negative, unique idempotency keys, and the ticket-wallet cap.
    /// </summary>
    private async Task<MatchDayWalletSeed> SeedMatchDayWalletsAsync(
        Event evt, DateTime kickOff, List<Order> orders, Dictionary<Order, int> ticketIdByOrder)
    {
        // Only a priced order can be paid for. (A basket can come out empty when its lines all landed
        // on the same drink and collapsed into one.)
        var payable = orders.Where(o => o.TotalAmount > 0m).ToList();
        if (payable.Count == 0)
            return new MatchDayWalletSeed(0, 0, 0m);

        // One decision per seat, not per order: a fan who is paying by balance pays that way all match.
        // Grouping by ticket rather than by customer is what makes the anonymous wallet expressible at
        // all — that balance belongs to the seat, not to an account.
        var userPayers = new Dictionary<int, List<Order>>();
        var ticketPayers = new Dictionary<int, List<Order>>();

        foreach (var seat in payable.GroupBy(o => ticketIdByOrder.GetValueOrDefault(o)))
        {
            if (seat.Key == 0 || Random.Shared.NextDouble() >= WalletPayerShare)
                continue;

            var seatOrders = seat.ToList();
            if (Random.Shared.NextDouble() < TicketWalletPayerShare)
            {
                ticketPayers[seat.Key] = seatOrders;
            }
            else
            {
                // A simulated fan can hold more than one seat at a fixture; their single account wallet
                // pays for all of them, because a user has exactly one wallet.
                var customerId = seatOrders[0].CustomerId;
                if (!userPayers.TryGetValue(customerId, out var forCustomer))
                    userPayers[customerId] = forCustomer = new List<Order>();
                forCustomer.AddRange(seatOrders);
            }
        }

        if (userPayers.Count == 0 && ticketPayers.Count == 0)
            return new MatchDayWalletSeed(0, 0, 0m);

        // Reuse whatever these fans already hold. A fan recurs across the fixtures of a season, so a
        // second wallet for the same owner is not merely wrong — it breaks the unique-owner index.
        var userIds = userPayers.Keys.ToList();
        var ticketIds = ticketPayers.Keys.ToList();
        var held = await _context.Wallets
            .Where(w => (w.UserId != null && userIds.Contains(w.UserId.Value))
                     || (w.TicketId != null && ticketIds.Contains(w.TicketId.Value)))
            .ToListAsync();
        var byUser = held.Where(w => w.UserId != null).ToDictionary(w => w.UserId!.Value);
        var byTicket = held.Where(w => w.TicketId != null).ToDictionary(w => w.TicketId!.Value);

        // What those wallets have already done. Needed because a fixture is not always generated after
        // everything else that touched the wallet — an admin can regenerate an old match — and a block
        // of entries inserted into the middle of a ledger has to be priced against the position at that
        // moment and leave the rows after it still telling the truth. Tracked, so the shift sticks.
        var heldIds = held.Select(w => w.Id).ToList();
        var priorByWallet = heldIds.Count == 0
            ? new Dictionary<int, List<WalletTransaction>>()
            : (await _context.WalletTransactions.Where(t => heldIds.Contains(t.WalletId)).ToListAsync())
                .GroupBy(t => t.WalletId)
                .ToDictionary(g => g.Key, g => g.ToList());

        // Cash onto a ticket is handed over a counter, so the top-up is attributed to bar staff the way
        // the real one is. Null when the venue has no staff account yet — the entry stands either way.
        var barStaffId = await _context.Users
            .Where(u => u.Role == UserRole.Bartender || u.Role == UserRole.Waiter)
            .Select(u => (int?)u.Id)
            .FirstOrDefaultAsync();

        var drafts = new List<WalletDraft>();

        foreach (var (userId, forCustomer) in userPayers)
        {
            var wallet = byUser.GetValueOrDefault(userId);
            var draft = BuildWalletDraft(
                wallet, PriorFor(wallet), WalletOwnerType.User, userId, forCustomer,
                evt, maxBalance: null, actorUserId: userId);
            if (draft != null)
                drafts.Add(draft);
        }

        foreach (var (ticketId, forSeat) in ticketPayers)
        {
            var wallet = byTicket.GetValueOrDefault(ticketId);
            var draft = BuildWalletDraft(
                wallet, PriorFor(wallet), WalletOwnerType.Ticket, ticketId, forSeat,
                evt, maxBalance: _ticketWalletMaxBalance, actorUserId: barStaffId);
            if (draft != null)
                drafts.Add(draft);
        }

        List<WalletTransaction> PriorFor(Wallet? wallet) =>
            wallet == null ? new List<WalletTransaction>() : priorByWallet.GetValueOrDefault(wallet.Id, new());

        if (drafts.Count == 0)
            return new MatchDayWalletSeed(0, 0, 0m);

        foreach (var draft in drafts)
        {
            if (draft.Wallet.Id == 0)
                _context.Wallets.Add(draft.Wallet);
            _context.WalletTransactions.AddRange(draft.Entries);
        }
        // Committed before the Payment rows because a funding Payment is keyed to the ledger row it
        // funded, and that identity only exists once the entry is inserted.
        await _context.SaveChangesAsync();

        foreach (var draft in drafts)
        {
            if (draft.Deposit is { } deposit)
            {
                _context.Payments.Add(new Payment
                {
                    WalletTransactionId = deposit.Id,
                    // Cash over the counter for a ticket; the fan's card in the app for an account.
                    PaymentMethod = draft.IsTicketOwned ? PaymentMethod.Cash : PaymentMethod.CreditCard,
                    Direction = PaymentDirection.In,
                    // Cash has no gateway reference, matching the counter top-up path.
                    TransactionId = draft.IsTicketOwned ? null : $"SIMDEP-{deposit.Id}",
                    Amount = deposit.Amount,
                    Currency = deposit.Currency,
                    Status = PaymentStatus.Completed,
                    PaymentDate = deposit.CreatedAt,
                    CreatedAt = deposit.CreatedAt,
                    ProcessedAt = deposit.CreatedAt
                });
            }

            foreach (var (entry, order) in draft.Spends)
            {
                _context.Payments.Add(new Payment
                {
                    OrderId = order.Id,
                    WalletTransactionId = entry.Id,
                    PaymentMethod = draft.IsTicketOwned ? PaymentMethod.TicketWallet : PaymentMethod.DigitalWallet,
                    Direction = PaymentDirection.In,
                    TransactionId = entry.Id.ToString(),
                    Amount = order.TotalAmount,
                    Currency = entry.Currency,
                    Status = PaymentStatus.Completed,
                    PaymentDate = order.CreatedAt,
                    CreatedAt = order.CreatedAt,
                    ProcessedAt = order.CreatedAt
                });
            }
        }
        await _context.SaveChangesAsync();

        return new MatchDayWalletSeed(
            Wallets: drafts.Count,
            Payments: drafts.Sum(d => d.Spends.Count),
            Funded: drafts.Sum(d => d.Deposit?.Amount ?? 0m));
    }

    /// <summary>
    /// Lays out one wallet's match day: a top-up before the first round (only for what the balance
    /// can't already cover), a spend per order, and a refund for anything cancelled — chained in time
    /// so every entry's running balance is the one that actually followed it.
    ///
    /// <paramref name="prior"/> is the wallet's existing ledger. The new block is priced and chained
    /// from the position at <i>its own</i> moment rather than from today's balance, and anything dated
    /// after it is shifted by the block's net, so inserting a backdated fixture (an admin regenerating
    /// an old match) leaves every running balance correct instead of only the last one.
    ///
    /// Returns null when this owner can't sensibly pay by balance, in which case the seat just pays at
    /// the bar: a frozen or closed wallet, or a bearer balance whose cap can't stretch to the round.
    /// </summary>
    private static WalletDraft? BuildWalletDraft(
        Wallet? held, List<WalletTransaction> prior, WalletOwnerType ownerType, int ownerId,
        List<Order> paidOrders, Event evt, decimal? maxBalance, int? actorUserId)
    {
        if (held is { Status: not WalletStatus.Active })
            return null;

        var charges = paidOrders.Sum(o => o.TotalAmount);
        var firstRound = paidOrders.Min(o => o.CreatedAt);
        var toppedUpAt = firstRound.AddMinutes(-Random.Shared.Next(20, 120));

        var settled = prior.Where(t => t.Status == WalletTransactionStatus.Completed).ToList();
        var opening = settled.Where(t => t.CreatedAt < toppedUpAt).Sum(t => t.Amount);
        var after = settled.Where(t => t.CreatedAt >= toppedUpAt).ToList();

        // What this block may spend without pushing the balance below zero at any point in the ledger:
        // the position when it starts, and the lowest the wallet ever gets afterwards. With nothing
        // dated later — the normal case, a fixture generated in order — this is simply the balance.
        var spendable = after.Count == 0 ? opening : Math.Min(opening, after.Min(t => t.BalanceAfter));

        // People load a note, not exact change — and leave a little on for next time.
        var deposit = 0m;
        if (spendable < charges)
        {
            deposit = Math.Ceiling((charges - spendable) / 10m) * 10m
                      + TopUpFloatSteps[Random.Shared.Next(TopUpFloatSteps.Length)];
        }
        else if (Random.Shared.Next(4) == 0)
        {
            // Already in funds, but topped up anyway — the reason a wallet's balance drifts upward.
            deposit = 10m * Random.Shared.Next(1, 4);
        }

        if (maxBalance is decimal cap)
        {
            // A bearer balance is capped at the counter. Trim the top-up so no point in the ledger ends
            // up over the limit; if the cap can't cover the round at all, this seat pays at the bar
            // rather than leaving an over-limit wallet behind.
            var peak = after.Count == 0 ? opening : Math.Max(opening, after.Max(t => t.BalanceAfter));
            deposit = Math.Min(deposit, Math.Max(0m, cap - peak));
            if (spendable + deposit < charges)
                return null;
        }

        var wallet = held ?? new Wallet
        {
            OwnerType = ownerType,
            UserId = ownerType == WalletOwnerType.User ? ownerId : null,
            TicketId = ownerType == WalletOwnerType.Ticket ? ownerId : null,
            Balance = 0m,
            Currency = "EUR",
            Status = WalletStatus.Active,
            CreatedAt = toppedUpAt
        };

        var entries = new List<WalletTransaction>();
        var spends = new List<(WalletTransaction, Order)>();
        WalletTransaction? topUp = null;

        if (deposit > 0m)
        {
            topUp = new WalletTransaction
            {
                Type = WalletTransactionType.Deposit,
                Amount = deposit,
                CreatedAt = toppedUpAt,
                // Unique per run: this is generated data, so there is no earlier attempt to collapse
                // onto, and a stable key would collide the moment a fixture is regenerated.
                IdempotencyKey = $"seed-topup-{Guid.NewGuid():N}",
                ReferenceType = ownerType == WalletOwnerType.Ticket ? "TicketTopup" : "Deposit",
                ReferenceId = ownerType == WalletOwnerType.Ticket ? ownerId : null,
                Description = ownerType == WalletOwnerType.Ticket
                    ? $"Cash top-up on ticket #{ownerId} before {evt.EventName}"
                    : $"Deposit before {evt.EventName}",
                CreatedByUserId = actorUserId
            };
            entries.Add(topUp);
        }

        foreach (var order in paidOrders)
        {
            // The order path's own key shape, so a later cancel/refund of this order is recognised as
            // the same money rather than posted twice.
            var spend = new WalletTransaction
            {
                Type = WalletTransactionType.Payment,
                Amount = -order.TotalAmount,
                CreatedAt = order.CreatedAt,
                IdempotencyKey = $"order-{order.Id}",
                ReferenceType = "Order",
                ReferenceId = order.Id,
                Description = $"Drink order #{order.Id}",
                CreatedByUserId = actorUserId
            };
            entries.Add(spend);
            spends.Add((spend, order));

            if (order.Status == OrderStatus.Cancelled)
            {
                entries.Add(new WalletTransaction
                {
                    Type = WalletTransactionType.Refund,
                    Amount = order.TotalAmount,
                    CreatedAt = order.CancelledAt ?? order.CreatedAt,
                    IdempotencyKey = $"refund-order-{order.Id}",
                    ReferenceType = "Order",
                    ReferenceId = order.Id,
                    Description = $"Refund for cancelled order #{order.Id}",
                    CreatedByUserId = actorUserId
                });
            }
        }

        foreach (var entry in entries)
        {
            entry.Currency = wallet.Currency;
            entry.Status = WalletTransactionStatus.Completed;
            entry.CompletedAt = entry.CreatedAt;
            // Set through the navigation so a wallet being inserted in this same batch has its
            // generated id fixed up onto the entry.
            entry.Wallet = wallet;
        }

        // Rewrite the whole chain rather than just appending: this block may have landed in the middle
        // of an existing history, and everything after it now follows a different position.
        RechainWallet(wallet, prior.Concat(entries));

        var newest = entries.Max(e => e.CreatedAt);
        wallet.UpdatedAt = wallet.UpdatedAt is { } stamped && stamped > newest ? stamped : newest;

        return new WalletDraft
        {
            Wallet = wallet,
            IsTicketOwned = ownerType == WalletOwnerType.Ticket,
            Entries = entries,
            Deposit = topUp,
            Spends = spends
        };
    }

    /// <summary>
    /// Rewrites a wallet's running-balance snapshots from its own ledger and leaves the wallet on the
    /// closing figure. The ledger is the source of truth, so the chain is just the settled entries
    /// summed in time order — which is what keeps every snapshot honest when a block of entries is
    /// inserted into, or lifted out of, the middle of a history rather than appended to the end.
    /// Entries still awaiting settlement are left alone; they are not part of the balance yet.
    /// </summary>
    private static void RechainWallet(Wallet wallet, IEnumerable<WalletTransaction> entries)
    {
        var balance = 0m;
        foreach (var entry in entries
                     .Where(e => e.Status == WalletTransactionStatus.Completed)
                     .OrderBy(e => e.CreatedAt).ThenBy(e => e.Id))
        {
            balance += entry.Amount;
            entry.BalanceAfter = balance;
        }

        wallet.Balance = balance;
    }

    /// <summary>
    /// Takes back what a set of soon-to-be-deleted orders moved through the wallets, and clears the
    /// Payment rows that recorded it. Both are required before the orders themselves can go: a Payment
    /// points at its order, and at its ledger row through a Restrict FK.
    ///
    /// As with the stock ledger, only the order-linked entries go. The top-ups stay — the fan really
    /// did load that money — so the balance returns to what it was before the fixture, and the
    /// replacement run finds them in funds and tops up only what it needs to.
    /// </summary>
    private async Task UndoWalletLedgerAsync(List<int> orderIds)
    {
        if (orderIds.Count == 0)
            return;

        var payments = await _context.Payments
            .Where(p => p.OrderId != null && orderIds.Contains(p.OrderId.Value))
            .ToListAsync();
        _context.Payments.RemoveRange(payments);

        var stale = await _context.WalletTransactions
            .Where(t => t.ReferenceType == "Order" && t.ReferenceId != null && orderIds.Contains(t.ReferenceId.Value))
            .ToListAsync();
        if (stale.Count == 0)
            return;

        var walletIds = stale.Select(t => t.WalletId).Distinct().ToList();
        var wallets = await _context.Wallets
            .Where(w => walletIds.Contains(w.Id))
            .ToDictionaryAsync(w => w.Id);

        // Every entry of every affected wallet, so what survives can be re-chained: removing a spend
        // from the middle of a history moves the balance of everything that followed it, and leaving
        // those snapshots at their old values would be the same lie as never restoring the balance.
        var remaining = (await _context.WalletTransactions
                .Where(t => walletIds.Contains(t.WalletId))
                .ToListAsync())
            .Except(stale)
            .GroupBy(t => t.WalletId)
            .ToDictionary(g => g.Key, g => g.ToList());

        _context.WalletTransactions.RemoveRange(stale);

        foreach (var (walletId, wallet) in wallets)
            RechainWallet(wallet, remaining.GetValueOrDefault(walletId, new List<WalletTransaction>()));
    }

    /// <summary>Picks a minute after kick-off from <see cref="MatchDayProfile"/>.</summary>
    private static double PickMatchMinute()
    {
        var segment = MatchDayProfile[WeightedIndex(MatchDayProfile.Select(s => s.Weight).ToArray())];
        return segment.From + Random.Shared.NextDouble() * (segment.To - segment.From);
    }

    /// <summary>Index into <paramref name="weights"/>, chosen proportionally to the weights.</summary>
    private static int WeightedIndex(int[] weights)
    {
        var total = weights.Sum();
        if (total <= 0)
            return Random.Shared.Next(weights.Length);

        var roll = Random.Shared.Next(total);
        for (var i = 0; i < weights.Length; i++)
        {
            roll -= weights[i];
            if (roll < 0)
                return i;
        }
        return weights.Length - 1;
    }

    /// <summary>Weighted pick from a small set of values.</summary>
    private static T WeightedPick<T>(T[] values, int[] weights) => values[WeightedIndex(weights)];

    private static string Truncate(string? value, int max) =>
        string.IsNullOrEmpty(value) ? "" : value.Length <= max ? value : value[..max];

    // Marks tickets/orders this generator creates so re-runs can recognise (and skip) them.
    private const string VariedSalesSource = "DemoVariedSales";

    public async Task<VariedSalesSeedResult> GenerateVariedPerEventSalesAsync(int? seasonId = null)
    {
        // Target events: one season when asked, otherwise every non-cancelled event.
        var events = await _context.Events
            .Where(e => e.Status != EventStatus.Cancelled
                        && (seasonId == null || e.SeasonId == seasonId))
            .OrderBy(e => e.EventDate)
            .ToListAsync();

        if (events.Count == 0)
        {
            _logger.LogInformation("No events matched for varied per-event sales (seasonId={SeasonId}).", seasonId);
            return new VariedSalesSeedResult(0, 0, 0);
        }

        // Physical seat pool is shared by every event; load it once. Without seats we can still add
        // orders (revenue), just not seat-bound tickets.
        var allSeats = await _context.Seats.AsNoTracking().ToListAsync();

        var drinks = await _context.Drinks.Where(d => d.IsAvailable).ToListAsync();
        var customers = await _context.Users.Where(u => u.Role == UserRole.Customer).ToListAsync();
        if (customers.Count == 0)
            customers = await _context.Users.ToListAsync();

        var now = DateTime.UtcNow;
        int eventsProcessed = 0, ticketsAdded = 0, ordersAdded = 0;

        foreach (var evt in events)
        {
            var touched = false;

            // ---- 1) Varied single-event tickets (drives Tickets Sold + regular/season split) ----
            // Idempotent: only top up an event that has no single-event tickets yet.
            var hasSingles = await _context.Tickets.AnyAsync(t =>
                t.EventId == evt.Id && t.Kind == TicketKind.SingleEvent && t.Status != TicketStatuses.Cancelled);

            if (!hasSingles && allSeats.Count > 0)
            {
                // Seats already taken by a non-cancelled ticket for THIS event (e.g. season passes).
                var takenSeatIds = (await _context.Tickets
                        .Where(t => t.EventId == evt.Id && t.SeatId != null && t.Status != TicketStatuses.Cancelled)
                        .Select(t => t.SeatId!.Value)
                        .ToListAsync())
                    .ToHashSet();

                var existingSold = takenSeatIds.Count;
                var freeSeats = allSeats.Where(s => !takenSeatIds.Contains(s.Id)).ToList();

                // Aim for a per-event occupancy between 45% and 95% of the physical stadium; the random
                // target is what makes each event's number different. Never exceed the free seats.
                var targetFraction = 0.45 + Random.Shared.NextDouble() * 0.50;
                var targetTotal = (int)Math.Round(targetFraction * allSeats.Count);
                var singlesToAdd = Math.Clamp(targetTotal - existingSold, 0, freeSeats.Count);

                var isPast = evt.EventDate < now;
                for (int i = 0; i < singlesToAdd; i++)
                {
                    var seat = freeSeats[i];

                    // A finished event's ticket was mostly attended; an upcoming one is simply Active.
                    var status = TicketStatuses.Active;
                    var isUsed = false;
                    DateTime? usedAt = null;
                    if (isPast && Random.Shared.Next(100) < 90) // ~90% attended
                    {
                        status = TicketStatuses.Used;
                        isUsed = true;
                        usedAt = DateTime.SpecifyKind(evt.EventDate.AddMinutes(-Random.Shared.Next(5, 90)), DateTimeKind.Utc);
                    }

                    var purchaseDate = DateTime.SpecifyKind(
                        (isPast ? evt.EventDate : now).AddDays(-Random.Shared.Next(1, 30)), DateTimeKind.Utc);

                    _context.Tickets.Add(new Ticket
                    {
                        TicketNumber = $"SGL{evt.Id:D3}-{i + 1:D4}",
                        EventId = evt.Id,
                        SeatId = seat.Id,
                        QRCodeToken = Guid.NewGuid().ToString(),
                        Price = evt.BaseTicketPrice ?? 15.00m,
                        PurchaseDate = purchaseDate,
                        CustomerName = $"Customer {evt.Id}-{i + 1}",
                        CustomerEmail = $"fan{evt.Id}_{i + 1}@example.com",
                        Status = status,
                        Kind = TicketKind.SingleEvent,
                        IsActive = status != TicketStatuses.Cancelled,
                        IsUsed = isUsed,
                        UsedAt = usedAt,
                        CreatedAt = now,
                        // Legacy SeatNumber column is varchar(10); full-stadium seat codes can be
                        // longer (e.g. "N1A-R25S120"), so clamp to the column width.
                        SeatNumber = seat.SeatCode is { Length: > 10 } sc ? sc[..10] : seat.SeatCode,
                        Section = seat.Section?.SectionCode ?? "A",
                        Row = seat.RowNumber.ToString(),
                        EventName = evt.EventName,
                        EventDate = evt.EventDate,
                        SourceSystem = VariedSalesSource
                    });
                    ticketsAdded++;
                    touched = true;
                }

                if (singlesToAdd > 0)
                    await _context.SaveChangesAsync();
            }

            // ---- 2) Varied drink orders (drives Total Revenue) ----
            // Idempotent: only add orders to an event that has none yet.
            var hasOrders = await _context.Orders.AnyAsync(o =>
                o.EventId == evt.Id && o.Status != OrderStatus.Cancelled);

            if (!hasOrders && drinks.Count > 0 && customers.Count > 0)
            {
                // Drink ordering only happens once an event has started; a future event gets none.
                if (evt.EventDate <= now)
                {
                    var windowStart = DateTime.SpecifyKind(evt.EventDate, DateTimeKind.Utc);
                    var rawEnd = DateTime.SpecifyKind(evt.EventEndDate ?? evt.EventDate.AddHours(4), DateTimeKind.Utc);
                    var windowEnd = rawEnd > now ? now : rawEnd;
                    var spanSeconds = Math.Max(0, (windowEnd - windowStart).TotalSeconds);

                    // Random 8–40 orders per event: this spread is what makes revenue differ per event.
                    var orderCount = Random.Shared.Next(8, 41);
                    for (int i = 0; i < orderCount; i++)
                    {
                        var customer = customers[Random.Shared.Next(customers.Count)];
                        var createdAt = DateTime.SpecifyKind(
                            windowStart.AddSeconds(Random.Shared.NextDouble() * spanSeconds), DateTimeKind.Utc);

                        var order = new Order
                        {
                            EventId = evt.Id,
                            CustomerId = customer.Id,
                            TicketNumber = $"DRK{evt.Id:D3}-{i + 1:D3}",
                            SeatNumber = $"{(char)('A' + Random.Shared.Next(0, 4))}{Random.Shared.Next(1, 30)}",
                            TotalAmount = 0, // priced once items are added
                            Status = OrderStatus.Delivered, // completed/started event → sale is realised
                            CreatedAt = createdAt,
                            DeliveredAt = createdAt.AddMinutes(Random.Shared.Next(5, 20)),
                            Notes = "Seeded varied per-event drink sale"
                        };

                        _context.Orders.Add(order);
                        await _context.SaveChangesAsync();

                        decimal totalAmount = 0;
                        var itemCount = Random.Shared.Next(1, 4);
                        for (int j = 0; j < itemCount; j++)
                        {
                            var drink = drinks[Random.Shared.Next(drinks.Count)];
                            var quantity = Random.Shared.Next(1, 4);
                            var totalPrice = drink.Price * quantity;
                            totalAmount += totalPrice;

                            _context.OrderItems.Add(new OrderItem
                            {
                                OrderId = order.Id,
                                DrinkId = drink.Id,
                                Quantity = quantity,
                                UnitPrice = drink.Price,
                                TotalPrice = totalPrice
                            });
                        }

                        order.TotalAmount = totalAmount;
                        await _context.SaveChangesAsync();
                        ordersAdded++;
                        touched = true;
                    }
                }
            }

            if (touched)
                eventsProcessed++;
        }

        _logger.LogInformation(
            "Varied per-event sales: processed {Events} event(s), added {Tickets} single-event ticket(s) and {Orders} drink order(s).",
            eventsProcessed, ticketsAdded, ordersAdded);

        return new VariedSalesSeedResult(eventsProcessed, ticketsAdded, ordersAdded);
    }
}