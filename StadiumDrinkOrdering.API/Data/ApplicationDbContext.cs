using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.Shared.Models;
using StadiumDrinkOrdering.API.Models;

namespace StadiumDrinkOrdering.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<StockMovement> StockMovements { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<StadiumSeat> StadiumSeats { get; set; }
    
    // New Event Management entities
    public DbSet<Event> Events { get; set; }
    public DbSet<EventPoster> EventPosters { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<Season> Seasons { get; set; }
    public DbSet<SeasonTicket> SeasonTickets { get; set; }
    public DbSet<StadiumSection> StadiumSections { get; set; }
    public DbSet<Seat> Seats { get; set; }
    public DbSet<EventStaffAssignment> EventStaffAssignments { get; set; }
    public DbSet<EventAnalytics> EventAnalytics { get; set; }
    public DbSet<OrderSession> OrderSessions { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    
    // Stadium Structure entities
    public DbSet<Tribune> Tribunes { get; set; }
    public DbSet<Ring> Rings { get; set; }
    public DbSet<Sector> Sectors { get; set; }
    public DbSet<StadiumSeatNew> StadiumSeatsNew { get; set; }

    // Stadium Drawing Tool entities
    public DbSet<StadiumSectorOverlay> StadiumSectorOverlays { get; set; }

    // Per-event, per-sector ticket-price overrides
    public DbSet<EventSectorPrice> EventSectorPrices { get; set; }

    // Logging entities
    public DbSet<LogEntry> LogEntries { get; set; }
    
    // Customer Ticketing entities
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
    public DbSet<SeatReservation> SeatReservations { get; set; }
    
    // Ticket Authentication entities
    public DbSet<TicketSession> TicketSessions { get; set; }

    // Rate Limiting and Security entities
    public DbSet<FailedAttempt> FailedAttempts { get; set; }
    public DbSet<AccountLockout> AccountLockouts { get; set; }
    public DbSet<IPBan> IPBans { get; set; }

    // JWT Refresh Token entities
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AccountActivationToken> AccountActivationTokens { get; set; }

    // External integration (ticketing webhook idempotency ledger)
    public DbSet<IntegrationInboxEntry> IntegrationInboxEntries { get; set; }

    // Fan digital wallet (stored value + append-only ledger)
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<WalletTransaction> WalletTransactions { get; set; }

    // Venue / resident-club identity (branding surfaced to customers)
    public DbSet<Venue> Venues { get; set; }

    // Admin overrides of transactional email templates (defaults live in EmailTemplateCatalog)
    public DbSet<EmailTemplate> EmailTemplates { get; set; }
    public DbSet<Club> Clubs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure PostgreSQL DateTime properties to use UTC
        // This ensures all DateTime properties are stored as timestamp with time zone in PostgreSQL
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetColumnType("timestamp with time zone");
                }
            }
        }

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.IsActive).HasDefaultValue(true);
        });

        // Drink configuration
        modelBuilder.Entity<Drink>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(10, 2);

            // Drinks belong to a category; block deleting a category still in use.
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Drinks)
                .HasForeignKey(e => e.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Stock movement (append-only inventory ledger) configuration
        modelBuilder.Entity<StockMovement>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Note).HasMaxLength(500);
            entity.Property(e => e.UserEmail).HasMaxLength(256);

            // Fast lookup of a drink's history in reverse-chronological order.
            entity.HasIndex(e => new { e.DrinkId, e.CreatedAt });

            // A movement always belongs to a drink; drinks are only ever soft-deleted, so Restrict
            // keeps the ledger intact and prevents an accidental hard delete of a drink with history.
            entity.HasOne(e => e.Drink)
                .WithMany()
                .HasForeignKey(e => e.DrinkId)
                .OnDelete(DeleteBehavior.Restrict);

            // Sale/cancel movements reference their order; if that order is ever hard-deleted
            // (e.g. an unpaid order rolled back), null the link rather than losing the ledger row.
            entity.HasOne(e => e.Order)
                .WithMany()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Icon).HasMaxLength(16);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
            entity.HasIndex(e => e.TicketNumber);
            
            // Customer analytics performance indexes
            entity.HasIndex(e => e.CustomerId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.CustomerId, e.CreatedAt });
            entity.HasIndex(e => new { e.CustomerId, e.TotalAmount });
            entity.HasIndex(e => new { e.Status, e.CreatedAt });

            // Customer relationship
            entity.HasOne(e => e.Customer)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Staff relationships
            entity.HasOne(e => e.AcceptedByUser)
                .WithMany()
                .HasForeignKey(e => e.AcceptedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.InPreparationByUser)
                .WithMany()
                .HasForeignKey(e => e.InPreparationByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.PreparedByUser)
                .WithMany()
                .HasForeignKey(e => e.PreparedByUserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.DeliveredByUser)
                .WithMany()
                .HasForeignKey(e => e.DeliveredByUserId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // OrderItem configuration
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasPrecision(10, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);

            entity.HasOne(e => e.Order)
                .WithMany(o => o.OrderItems)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Drink)
                .WithMany(d => d.OrderItems)
                .HasForeignKey(e => e.DrinkId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Payment configuration
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(10, 2);
            entity.Property(e => e.RefundAmount).HasPrecision(10, 2);
            entity.HasIndex(e => e.TransactionId).IsUnique();
            
            // Customer analytics performance indexes
            entity.HasIndex(e => e.PaymentDate);
            entity.HasIndex(e => e.PaymentMethod);
            entity.HasIndex(e => new { e.Status, e.PaymentDate });
            entity.HasIndex(e => new { e.PaymentMethod, e.PaymentDate });

            entity.HasOne(e => e.Order)
                .WithOne(o => o.Payment)
                .HasForeignKey<Payment>(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Ticket configuration
        modelBuilder.Entity<Ticket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TicketNumber).IsUnique();
            entity.HasIndex(e => e.QRCodeToken).IsUnique();
            entity.Property(e => e.Price).HasPrecision(10, 2);
            
            // Customer analytics performance indexes
            entity.HasIndex(e => e.CustomerEmail);
            entity.HasIndex(e => e.PurchaseDate);
            entity.HasIndex(e => new { e.CustomerEmail, e.PurchaseDate });
            entity.HasIndex(e => new { e.CustomerEmail, e.Price });
            entity.HasIndex(e => new { e.EventId, e.CustomerEmail });
            entity.HasIndex(e => new { e.Status, e.PurchaseDate });

            entity.HasOne(e => e.Event)
                .WithMany(ev => ev.Tickets)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Seat)
                .WithMany(s => s.Tickets)
                .HasForeignKey(e => e.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique mapping to the external ticketing system (NULLs distinct in PostgreSQL).
            entity.HasIndex(e => e.ExternalTicketId).IsUnique();

            // Season-pass–derived tickets link back to the pass that generated them.
            entity.HasOne(e => e.SeasonTicket)
                .WithMany(st => st.DerivedTickets)
                .HasForeignKey(e => e.SeasonTicketId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // StadiumSeat configuration
        modelBuilder.Entity<StadiumSeat>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Section, e.RowNumber, e.SeatNumber }).IsUnique();
        });

        // Event configuration
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.EventName);
            entity.Property(e => e.BaseTicketPrice).HasPrecision(10, 2);
            // Unique mapping to the external ticketing system (PostgreSQL treats NULLs as
            // distinct, so internally-created events without an external id are unaffected).
            entity.HasIndex(e => e.ExternalEventId).IsUnique();

            // Optional link to the season this event belongs to.
            entity.HasOne(e => e.Season)
                .WithMany(s => s.Events)
                .HasForeignKey(e => e.SeasonId)
                .OnDelete(DeleteBehavior.SetNull);

            // Optional links to the club/team identities behind HomeTeam/AwayTeam. SetNull on
            // delete: those name columns are the fixture's own record of who played, so removing a
            // team from the directory must drop the crest link without rewriting match history.
            entity.HasOne(e => e.HomeClub)
                .WithMany()
                .HasForeignKey(e => e.HomeClubId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.AwayTeamProfile)
                .WithMany()
                .HasForeignKey(e => e.AwayTeamId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(e => e.HomeClubId);
            entity.HasIndex(e => e.AwayTeamId);

            // HasPoster is computed from PosterContentType; it is not a column.
            entity.Ignore(e => e.HasPoster);
        });

        // Poster image bytes, split into their own table so listing events never loads them.
        modelBuilder.Entity<EventPoster>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.EventId).IsUnique();

            entity.HasOne(p => p.Event)
                .WithOne(e => e.Poster)
                .HasForeignKey<EventPoster>(p => p.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Visiting-team directory, looked up by normalized team name.
        modelBuilder.Entity<Team>(entity =>
        {
            entity.HasKey(t => t.Id);
            entity.HasIndex(t => t.NormalizedName).IsUnique();
            entity.Property(t => t.Name).HasMaxLength(150).IsRequired();
            entity.Property(t => t.NormalizedName).HasMaxLength(150).IsRequired();
        });

        // Per-event, per-sector ticket-price override configuration
        modelBuilder.Entity<EventSectorPrice>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(10, 2);

            // At most one override per (event, sector).
            entity.HasIndex(e => new { e.EventId, e.SectorOverlayId }).IsUnique();

            // Overrides are meaningless without their event; remove them with it.
            entity.HasOne(e => e.Event)
                .WithMany(ev => ev.SectorPrices)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            // Removing a sector clears its per-event overrides.
            entity.HasOne(e => e.SectorOverlay)
                .WithMany()
                .HasForeignKey(e => e.SectorOverlayId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Season configuration
        modelBuilder.Entity<Season>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
            // Unique mapping to the external ticketing system (NULLs distinct in PostgreSQL).
            entity.HasIndex(e => e.ExternalSeasonId).IsUnique();
        });

        // SeasonTicket (annual pass) configuration
        modelBuilder.Entity<SeasonTicket>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(10, 2);
            entity.HasIndex(e => e.HolderEmail);
            // Unique mapping to the external ticketing system (NULLs distinct in PostgreSQL).
            entity.HasIndex(e => e.ExternalSeasonTicketId).IsUnique();

            entity.HasOne(e => e.Season)
                .WithMany(s => s.SeasonTickets)
                .HasForeignKey(e => e.SeasonId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Seat)
                .WithMany()
                .HasForeignKey(e => e.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            // Linked fan account (populated by the email-match linker). Nullable; if the user is
            // deleted the pass survives with its link cleared rather than cascading the delete.
            entity.HasIndex(e => e.UserId);
            // A customer may hold at most one season ticket per season. NULL UserIds are distinct in
            // PostgreSQL, so unlinked passes are unaffected.
            entity.HasIndex(e => new { e.UserId, e.SeasonId })
                .IsUnique()
                .HasDatabaseName("IX_SeasonTickets_UserId_SeasonId");
            entity.HasOne(e => e.User)
                .WithMany(u => u.SeasonTickets)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Wallet (stored-value account — user- or ticket-owned) configuration
        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Balance).HasPrecision(10, 2);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.OwnerType).HasConversion<int>();

            // Exactly one owner: a wallet is owned by a user XOR a ticket, never both/neither.
            entity.ToTable(t => t.HasCheckConstraint(
                "CK_Wallets_OneOwner", "(\"UserId\" IS NULL) <> (\"TicketId\" IS NULL)"));

            // One wallet per user — filtered so the many ticket wallets (UserId NULL) don't collide.
            entity.HasIndex(e => e.UserId).IsUnique().HasFilter("\"UserId\" IS NOT NULL");
            entity.HasOne(e => e.User)
                .WithOne(u => u.Wallet)
                .HasForeignKey<Wallet>(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // One wallet per ticket — filtered for the same reason (user wallets have TicketId NULL).
            entity.HasIndex(e => e.TicketId).IsUnique().HasFilter("\"TicketId\" IS NOT NULL");
            // Restrict: a ticket carrying a stored-value balance must not be deletable out from under it.
            entity.HasOne(e => e.Ticket)
                .WithMany()
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Restrict);

            // Map the PostgreSQL xmin system column as the optimistic-concurrency token. Npgsql
            // recognises xmin/xid and does NOT emit a physical column for it in the migration.
            entity.Property(e => e.Version)
                .HasColumnName("xmin")
                .HasColumnType("xid")
                .ValueGeneratedOnAddOrUpdate()
                .IsConcurrencyToken();
        });

        // WalletTransaction (append-only ledger) configuration
        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Amount).HasPrecision(10, 2);
            entity.Property(e => e.BalanceAfter).HasPrecision(10, 2);
            entity.Property(e => e.Currency).HasMaxLength(3);
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.IdempotencyKey).HasMaxLength(100);

            // The anti-double-charge guard: a given idempotency key posts at most one ledger row.
            entity.HasIndex(e => e.IdempotencyKey).IsUnique();
            entity.HasIndex(e => e.WalletId);

            entity.HasOne(e => e.Wallet)
                .WithMany(w => w.Transactions)
                .HasForeignKey(e => e.WalletId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // A Payment may fund a wallet deposit; the ledger is append-only so never cascade from it.
        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasOne(e => e.WalletTransaction)
                .WithMany()
                .HasForeignKey(e => e.WalletTransactionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // StadiumSection configuration
        modelBuilder.Entity<StadiumSection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SectionName).IsUnique();
            entity.Property(e => e.PriceMultiplier).HasPrecision(5, 2);
        });

        // Seat configuration
        modelBuilder.Entity<Seat>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.SectionId, e.RowNumber, e.SeatNumber }).IsUnique();

            entity.HasOne(e => e.Section)
                .WithMany(s => s.Seats)
                .HasForeignKey(e => e.SectionId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // EventStaffAssignment configuration
        modelBuilder.Entity<EventStaffAssignment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.StaffId }).IsUnique();

            entity.HasOne(e => e.Event)
                .WithMany(ev => ev.StaffAssignments)
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Staff)
                .WithMany()
                .HasForeignKey(e => e.StaffId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // EventAnalytics configuration
        modelBuilder.Entity<EventAnalytics>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalRevenue).HasPrecision(12, 2);
            entity.Property(e => e.TicketRevenue).HasPrecision(12, 2);
            entity.Property(e => e.DrinksRevenue).HasPrecision(12, 2);
            entity.Property(e => e.AverageOrderValue).HasPrecision(10, 2);

            entity.HasOne(e => e.Event)
                .WithOne(ev => ev.Analytics)
                .HasForeignKey<EventAnalytics>(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // OrderSession configuration
        modelBuilder.Entity<OrderSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionToken).IsUnique();
            entity.Property(e => e.CartTotal).HasPrecision(10, 2);

            entity.HasOne(e => e.Ticket)
                .WithMany(t => t.OrderSessions)
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Notification configuration
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => new { e.UserId, e.IsRead });

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Update Order configuration for new relationships
        modelBuilder.Entity<Order>(order =>
        {
            // Existing configurations remain the same...
            
            // Add new event-based relationships
            order.HasOne(o => o.Event)
                .WithMany(e => e.Orders)
                .HasForeignKey(o => o.EventId)
                .OnDelete(DeleteBehavior.SetNull);

            order.HasOne(o => o.Seat)
                .WithMany()
                .HasForeignKey(o => o.SeatId)
                .OnDelete(DeleteBehavior.SetNull);

            order.HasOne(o => o.PaymentDetails)
                .WithOne()
                .HasForeignKey<Order>(o => o.PaymentId)
                .OnDelete(DeleteBehavior.SetNull);

            order.HasOne(o => o.Session)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.SessionId)
                .OnDelete(DeleteBehavior.SetNull);

            order.HasOne(o => o.AssignedStaff)
                .WithMany()
                .HasForeignKey(o => o.AssignedStaffId)
                .OnDelete(DeleteBehavior.SetNull);

            order.HasOne(o => o.TicketSession)
                .WithMany()
                .HasForeignKey(o => o.TicketSessionId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Stadium Structure configurations
        modelBuilder.Entity<Tribune>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(1);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Ring>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.TribuneId, e.Number }).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(e => e.Tribune)
                .WithMany(t => t.Rings)
                .HasForeignKey(e => e.TribuneId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Sector>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.RingId, e.Code }).IsUnique();
            entity.Property(e => e.Code).HasMaxLength(5);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(e => e.Ring)
                .WithMany(r => r.Sectors)
                .HasForeignKey(e => e.RingId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<StadiumSeatNew>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.SectorId, e.RowNumber, e.SeatNumber }).IsUnique();
            entity.HasIndex(e => e.UniqueCode).IsUnique();
            entity.HasIndex(e => new { e.StadiumSectorOverlayId, e.RowNumber, e.SeatNumber });
            entity.Property(e => e.UniqueCode).HasMaxLength(60);

            // Legacy hierarchy sector (optional now that overlays own seats too)
            entity.HasOne(e => e.Sector)
                .WithMany(s => s.Seats)
                .HasForeignKey(e => e.SectorId)
                .OnDelete(DeleteBehavior.Cascade);

            // Drawing-tool sector overlay ownership (source of truth)
            entity.HasOne(e => e.StadiumSectorOverlay)
                .WithMany()
                .HasForeignKey(e => e.StadiumSectorOverlayId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // LogEntry configuration
        modelBuilder.Entity<LogEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Timestamp);
            entity.HasIndex(e => new { e.Level, e.Category });
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Source);
            entity.Property(e => e.Level).HasMaxLength(50);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Action).HasMaxLength(200);
            entity.Property(e => e.UserId).HasMaxLength(100);
            entity.Property(e => e.UserEmail).HasMaxLength(100);
            entity.Property(e => e.UserRole).HasMaxLength(50);
            entity.Property(e => e.IPAddress).HasMaxLength(100);
            entity.Property(e => e.UserAgent).HasMaxLength(200);
            entity.Property(e => e.RequestPath).HasMaxLength(100);
            entity.Property(e => e.HttpMethod).HasMaxLength(20);
            entity.Property(e => e.Message).HasMaxLength(500);
            entity.Property(e => e.ExceptionType).HasMaxLength(200);
            entity.Property(e => e.Source).HasMaxLength(100);
        });

        // ShoppingCart configuration
        modelBuilder.Entity<ShoppingCart>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ExpiresAt);
            entity.Property(e => e.SessionId).HasMaxLength(100);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // CartItem configuration
        modelBuilder.Entity<CartItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.SectorId, e.RowNumber, e.SeatNumber }).IsUnique();
            entity.HasIndex(e => e.ReservedUntil);
            entity.Property(e => e.SeatCode).HasMaxLength(50);
            entity.Property(e => e.Price).HasPrecision(10, 2);

            entity.HasOne(e => e.ShoppingCart)
                .WithMany(c => c.Items)
                .HasForeignKey(e => e.ShoppingCartId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // SeatReservation configuration
        modelBuilder.Entity<SeatReservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.EventId, e.SectorId, e.RowNumber, e.SeatNumber });
            entity.HasIndex(e => e.SessionId);
            entity.HasIndex(e => e.ReservedUntil);
            entity.HasIndex(e => e.Status);
            entity.Property(e => e.SeatCode).HasMaxLength(50);
            entity.Property(e => e.SessionId).HasMaxLength(100);

            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // TicketSession configuration
        modelBuilder.Entity<TicketSession>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.SessionId).IsUnique();
            entity.HasIndex(e => e.QRCodeToken);
            entity.HasIndex(e => new { e.QRCodeToken, e.IsActive });
            entity.HasIndex(e => e.ExpiresAt);
            
            entity.HasOne(e => e.Ticket)
                .WithMany()
                .HasForeignKey(e => e.TicketId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Event)
                .WithMany()
                .HasForeignKey(e => e.EventId)
                .OnDelete(DeleteBehavior.Restrict);
                
            entity.HasOne(e => e.Seat)
                .WithMany()
                .HasForeignKey(e => e.SeatId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Rate Limiting and Security entity configurations
        modelBuilder.Entity<FailedAttempt>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.IPAddress);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => e.AttemptTime);
            entity.HasIndex(e => new { e.IPAddress, e.AttemptTime });
            entity.HasIndex(e => new { e.Email, e.AttemptTime });
            entity.HasIndex(e => new { e.AttemptType, e.AttemptTime });
            entity.Property(e => e.IPAddress).HasMaxLength(45).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.AttemptType).HasMaxLength(50).IsRequired();
            entity.Property(e => e.UserAgent).HasMaxLength(500);
            entity.Property(e => e.Context).HasMaxLength(1000);
        });

        modelBuilder.Entity<AccountLockout>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email);
            entity.HasIndex(e => new { e.Email, e.IsActive });
            entity.HasIndex(e => e.LockoutEnd);
            entity.Property(e => e.Email).HasMaxLength(255).IsRequired();
            entity.Property(e => e.Reason).HasMaxLength(500);
        });

        modelBuilder.Entity<IPBan>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.IPAddress);
            entity.HasIndex(e => new { e.IPAddress, e.IsActive });
            entity.HasIndex(e => e.BanEnd);
            entity.Property(e => e.IPAddress).HasMaxLength(45).IsRequired();
            entity.Property(e => e.Reason).HasMaxLength(500);
        });

        // RefreshToken configuration
        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.JwtId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ExpiresAt);
            entity.HasIndex(e => new { e.UserId, e.IsUsed, e.IsRevoked });
            entity.HasIndex(e => new { e.Token, e.IsUsed, e.IsRevoked });
            entity.Property(e => e.Token).HasMaxLength(512).IsRequired();
            entity.Property(e => e.JwtId).HasMaxLength(256).IsRequired();
            entity.Property(e => e.InvalidationReason).HasMaxLength(200);
            entity.Property(e => e.DeviceInfo).HasMaxLength(500);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(1000);

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // AccountActivationToken configuration (single-use shell-account claim token)
        modelBuilder.Entity<AccountActivationToken>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Token).IsUnique();
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ExpiresAt);
            entity.Property(e => e.Token).HasMaxLength(512).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // IntegrationInboxEntry configuration (webhook idempotency ledger)
        modelBuilder.Entity<IntegrationInboxEntry>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.IdempotencyKey).IsUnique();
            entity.HasIndex(e => e.ReceivedAt);
            entity.Property(e => e.IdempotencyKey).HasMaxLength(200).IsRequired();
            entity.Property(e => e.EventType).HasMaxLength(50);
            entity.Property(e => e.SourceSystem).HasMaxLength(100);
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.Notes).HasMaxLength(500);
        });

        // Venue configuration (singleton stadium identity)
        modelBuilder.Entity<Venue>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.ClubName).HasMaxLength(150);
            entity.Property(e => e.ClubLogoContentType).HasMaxLength(100);
            // Existing installations should keep selling once the column is added.
            entity.Property(e => e.TicketSalesEnabled).HasDefaultValue(true);
            // Existing installations keep accepting every method once the columns are added.
            entity.Property(e => e.WalletPaymentEnabled).HasDefaultValue(true);
            entity.Property(e => e.CardPaymentEnabled).HasDefaultValue(true);
            entity.Property(e => e.CashPaymentEnabled).HasDefaultValue(true);
            entity.Property(e => e.SmtpHost).HasMaxLength(200);
            entity.Property(e => e.SmtpUsername).HasMaxLength(200);
            entity.Property(e => e.SmtpPassword).HasMaxLength(500);
            entity.Property(e => e.EmailFromAddress).HasMaxLength(200);
            entity.Property(e => e.EmailFromName).HasMaxLength(150);
            entity.Property(e => e.SmtpPort).HasDefaultValue(587);
            entity.Property(e => e.SmtpUseSsl).HasDefaultValue(true);
        });

        // Email template overrides (one row per customized template)
        modelBuilder.Entity<EmailTemplate>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TemplateKey).IsUnique();
            entity.Property(e => e.TemplateKey).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Subject).HasMaxLength(300).IsRequired();
            entity.Property(e => e.HtmlBody).IsRequired();
            entity.Property(e => e.UpdatedBy).HasMaxLength(200);
        });

        // Club configuration (resident clubs at the venue)
        modelBuilder.Entity<Club>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.VenueId);
            entity.Property(e => e.Name).HasMaxLength(150).IsRequired();

            entity.HasOne(e => e.Venue)
                .WithMany(v => v.Clubs)
                .HasForeignKey(e => e.VenueId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        // Seed admin user
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = 1,
                Username = "nebojsa.medancic+adminStadion@gmail.com",
                Email = "nebojsa.medancic+adminStadion@gmail.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed the singleton venue so the branding profile always exists to edit.
        // Fixed timestamp (not DateTime.UtcNow) so re-scaffolded migrations don't churn.
        modelBuilder.Entity<Venue>().HasData(
            new Venue
            {
                Id = 1,
                Name = "Stadium",
                Country = "Croatia",
                CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );

        // Seed drink categories. Ids intentionally match the legacy DrinkCategory enum values
        // (Beer=1 ... Cocktail=8) so existing drink rows keep their category with no data migration.
        var categorySeedDate = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Name = "Beer", DisplayName = "Pivo", Icon = "🍺", IsActive = true, SortOrder = 1, CreatedAt = categorySeedDate },
            new Category { Id = 2, Name = "SoftDrink", DisplayName = "Gazirano", Icon = "🥤", IsActive = true, SortOrder = 2, CreatedAt = categorySeedDate },
            new Category { Id = 3, Name = "Water", DisplayName = "Voda", Icon = "💧", IsActive = true, SortOrder = 3, CreatedAt = categorySeedDate },
            new Category { Id = 4, Name = "Coffee", DisplayName = "Kava", Icon = "☕", IsActive = true, SortOrder = 4, CreatedAt = categorySeedDate },
            new Category { Id = 5, Name = "Tea", DisplayName = "Čaj", Icon = "🍵", IsActive = true, SortOrder = 5, CreatedAt = categorySeedDate },
            new Category { Id = 6, Name = "Juice", DisplayName = "Sok", Icon = "🧃", IsActive = true, SortOrder = 6, CreatedAt = categorySeedDate },
            new Category { Id = 7, Name = "EnergyDrink", DisplayName = "Energetsko", Icon = "⚡", IsActive = true, SortOrder = 7, CreatedAt = categorySeedDate },
            new Category { Id = 8, Name = "Cocktail", DisplayName = "Koktel", Icon = "🍸", IsActive = true, SortOrder = 8, CreatedAt = categorySeedDate }
        );

        // Seed some drinks
        modelBuilder.Entity<Drink>().HasData(
            new Drink { Id = 1, Name = "Coca Cola", Description = "Classic Coca Cola", Price = 3.50m, StockQuantity = 100, CategoryId = 2 },
            new Drink { Id = 2, Name = "Pepsi", Description = "Pepsi Cola", Price = 3.50m, StockQuantity = 100, CategoryId = 2 },
            new Drink { Id = 3, Name = "Water", Description = "Bottled Water", Price = 2.00m, StockQuantity = 200, CategoryId = 3 },
            new Drink { Id = 4, Name = "Beer", Description = "Local Draft Beer", Price = 6.00m, StockQuantity = 150, CategoryId = 1 },
            new Drink { Id = 5, Name = "Coffee", Description = "Hot Coffee", Price = 4.00m, StockQuantity = 80, CategoryId = 4 },
            new Drink { Id = 6, Name = "Orange Juice", Description = "Fresh Orange Juice", Price = 4.50m, StockQuantity = 60, CategoryId = 6 },
            new Drink { Id = 7, Name = "Red Bull", Description = "Energy Drink", Price = 5.00m, StockQuantity = 90, CategoryId = 7 },
            new Drink { Id = 8, Name = "Green Tea", Description = "Hot Green Tea", Price = 3.00m, StockQuantity = 70, CategoryId = 5 }
        );

        // Seed sample event first
        modelBuilder.Entity<Event>().HasData(
            new Event 
            { 
                Id = 1, 
                EventName = "Championship Match", 
                EventType = "Football",
                EventDate = DateTime.UtcNow.Date.AddHours(19), // Use UTC date with 19:00 time
                TotalSeats = 100,
                BaseTicketPrice = 50.00m,
                Description = "Championship final match",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed stadium sections first
        modelBuilder.Entity<StadiumSection>().HasData(
            new StadiumSection { Id = 1, SectionCode = "A", SectionName = "Section A", TotalRows = 10, SeatsPerRow = 10, PriceMultiplier = 1.0m, Color = "#007bff" },
            new StadiumSection { Id = 2, SectionCode = "B", SectionName = "Section B", TotalRows = 10, SeatsPerRow = 10, PriceMultiplier = 1.2m, Color = "#28a745" },
            new StadiumSection { Id = 3, SectionCode = "C", SectionName = "Section C", TotalRows = 10, SeatsPerRow = 10, PriceMultiplier = 1.5m, Color = "#ffc107" },
            new StadiumSection { Id = 4, SectionCode = "D", SectionName = "Section D VIP", TotalRows = 7, SeatsPerRow = 14, PriceMultiplier = 2.0m, Color = "#dc3545" }
        );

        // Seed some seats
        modelBuilder.Entity<Seat>().HasData(
            new Seat { Id = 1, SectionId = 1, RowNumber = 1, SeatNumber = 1, SeatCode = "A-R1-S1", XCoordinate = 50, YCoordinate = 50 },
            new Seat { Id = 2, SectionId = 1, RowNumber = 1, SeatNumber = 2, SeatCode = "A-R1-S2", XCoordinate = 65, YCoordinate = 50 },
            new Seat { Id = 3, SectionId = 2, RowNumber = 5, SeatNumber = 5, SeatCode = "B-R5-S5", XCoordinate = 375, YCoordinate = 110 },
            new Seat { Id = 4, SectionId = 3, RowNumber = 10, SeatNumber = 10, SeatCode = "C-R10-S10", XCoordinate = 685, YCoordinate = 185 },
            new Seat { Id = 5, SectionId = 4, RowNumber = 3, SeatNumber = 7, SeatCode = "D-R3-S7", XCoordinate = 440, YCoordinate = 400 }
        );

        // Seed some tickets with proper foreign key references
        var eventDate = DateTime.UtcNow.Date.AddHours(19);
        modelBuilder.Entity<Ticket>().HasData(
            new Ticket { Id = 1, TicketNumber = "TK001", EventId = 1, SeatId = 1, QRCodeToken = Guid.NewGuid().ToString(), Price = 50.00m, SeatNumber = "A1", Section = "A", Row = "1", EventName = "Championship Match", EventDate = eventDate, PurchaseDate = DateTime.UtcNow },
            new Ticket { Id = 2, TicketNumber = "TK002", EventId = 1, SeatId = 2, QRCodeToken = Guid.NewGuid().ToString(), Price = 50.00m, SeatNumber = "A2", Section = "A", Row = "1", EventName = "Championship Match", EventDate = eventDate, PurchaseDate = DateTime.UtcNow },
            new Ticket { Id = 3, TicketNumber = "TK003", EventId = 1, SeatId = 3, QRCodeToken = Guid.NewGuid().ToString(), Price = 60.00m, SeatNumber = "B5", Section = "B", Row = "5", EventName = "Championship Match", EventDate = eventDate, PurchaseDate = DateTime.UtcNow },
            new Ticket { Id = 4, TicketNumber = "TK004", EventId = 1, SeatId = 4, QRCodeToken = Guid.NewGuid().ToString(), Price = 75.00m, SeatNumber = "C10", Section = "C", Row = "10", EventName = "Championship Match", EventDate = eventDate, PurchaseDate = DateTime.UtcNow },
            new Ticket { Id = 5, TicketNumber = "TK005", EventId = 1, SeatId = 5, QRCodeToken = Guid.NewGuid().ToString(), Price = 100.00m, SeatNumber = "D15", Section = "D", Row = "15", EventName = "Championship Match", EventDate = eventDate, PurchaseDate = DateTime.UtcNow }
        );

        // Seed stadium seats
        var stadiumSeats = new List<StadiumSeat>();
        int stadiumSeatId = 6; // Start after the Seat IDs

        // Section A: 10 rows × 10 seats = 100 seats
        for (int row = 1; row <= 10; row++)
        {
            for (int seat = 1; seat <= 10; seat++)
            {
                stadiumSeats.Add(new StadiumSeat
                {
                    Id = stadiumSeatId++,
                    Section = "A",
                    RowNumber = row,
                    SeatNumber = seat,
                    XCoordinate = 50 + (seat - 1) * 15,
                    YCoordinate = 50 + (row - 1) * 15,
                    IsActive = true
                });
            }
        }

        // Section B: 10 rows × 10 seats = 100 seats
        for (int row = 1; row <= 10; row++)
        {
            for (int seat = 1; seat <= 10; seat++)
            {
                stadiumSeats.Add(new StadiumSeat
                {
                    Id = stadiumSeatId++,
                    Section = "B",
                    RowNumber = row,
                    SeatNumber = seat,
                    XCoordinate = 300 + (seat - 1) * 15,
                    YCoordinate = 50 + (row - 1) * 15,
                    IsActive = true
                });
            }
        }

        // Section C: 10 rows × 10 seats = 100 seats
        for (int row = 1; row <= 10; row++)
        {
            for (int seat = 1; seat <= 10; seat++)
            {
                stadiumSeats.Add(new StadiumSeat
                {
                    Id = stadiumSeatId++,
                    Section = "C",
                    RowNumber = row,
                    SeatNumber = seat,
                    XCoordinate = 550 + (seat - 1) * 15,
                    YCoordinate = 50 + (row - 1) * 15,
                    IsActive = true
                });
            }
        }

        // VIP Section D: 7 rows × 14 seats = 98 seats
        for (int row = 1; row <= 7; row++)
        {
            for (int seat = 1; seat <= 14; seat++)
            {
                stadiumSeats.Add(new StadiumSeat
                {
                    Id = stadiumSeatId++,
                    Section = "D",
                    RowNumber = row,
                    SeatNumber = seat,
                    XCoordinate = 350 + (seat - 1) * 15,
                    YCoordinate = 370 + (row - 1) * 15,
                    IsActive = true
                });
            }
        }

        modelBuilder.Entity<StadiumSeat>().HasData(stadiumSeats);
    }
}
