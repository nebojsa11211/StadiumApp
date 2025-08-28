using Microsoft.EntityFrameworkCore;
using StadiumDrinkOrdering.Shared.Models;

namespace StadiumDrinkOrdering.API.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Drink> Drinks { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Ticket> Tickets { get; set; }
    public DbSet<StadiumSeat> StadiumSeats { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // Drink configuration
        modelBuilder.Entity<Drink>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Price).HasPrecision(10, 2);
        });

        // Order configuration
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.TotalAmount).HasPrecision(10, 2);
            entity.HasIndex(e => e.TicketNumber);

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
            entity.HasIndex(e => e.TransactionId).IsUnique();

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
        });

        // StadiumSeat configuration
        modelBuilder.Entity<StadiumSeat>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.Section, e.RowNumber, e.SeatNumber }).IsUnique();
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
                Username = "admin",
                Email = "admin@stadium.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                Role = UserRole.Admin,
                CreatedAt = DateTime.UtcNow
            }
        );

        // Seed some drinks
        modelBuilder.Entity<Drink>().HasData(
            new Drink { Id = 1, Name = "Coca Cola", Description = "Classic Coca Cola", Price = 3.50m, StockQuantity = 100, Category = DrinkCategory.SoftDrink },
            new Drink { Id = 2, Name = "Pepsi", Description = "Pepsi Cola", Price = 3.50m, StockQuantity = 100, Category = DrinkCategory.SoftDrink },
            new Drink { Id = 3, Name = "Water", Description = "Bottled Water", Price = 2.00m, StockQuantity = 200, Category = DrinkCategory.Water },
            new Drink { Id = 4, Name = "Beer", Description = "Local Draft Beer", Price = 6.00m, StockQuantity = 150, Category = DrinkCategory.Beer },
            new Drink { Id = 5, Name = "Coffee", Description = "Hot Coffee", Price = 4.00m, StockQuantity = 80, Category = DrinkCategory.Coffee },
            new Drink { Id = 6, Name = "Orange Juice", Description = "Fresh Orange Juice", Price = 4.50m, StockQuantity = 60, Category = DrinkCategory.Juice },
            new Drink { Id = 7, Name = "Red Bull", Description = "Energy Drink", Price = 5.00m, StockQuantity = 90, Category = DrinkCategory.EnergyDrink },
            new Drink { Id = 8, Name = "Green Tea", Description = "Hot Green Tea", Price = 3.00m, StockQuantity = 70, Category = DrinkCategory.Tea }
        );

        // Seed some tickets
        modelBuilder.Entity<Ticket>().HasData(
            new Ticket { Id = 1, TicketNumber = "TK001", SeatNumber = "A1", Section = "A", Row = "1", EventName = "Championship Match", EventDate = DateTime.Today.AddHours(19) },
            new Ticket { Id = 2, TicketNumber = "TK002", SeatNumber = "A2", Section = "A", Row = "1", EventName = "Championship Match", EventDate = DateTime.Today.AddHours(19) },
            new Ticket { Id = 3, TicketNumber = "TK003", SeatNumber = "B5", Section = "B", Row = "5", EventName = "Championship Match", EventDate = DateTime.Today.AddHours(19) },
            new Ticket { Id = 4, TicketNumber = "TK004", SeatNumber = "C10", Section = "C", Row = "10", EventName = "Championship Match", EventDate = DateTime.Today.AddHours(19) },
            new Ticket { Id = 5, TicketNumber = "TK005", SeatNumber = "D15", Section = "D", Row = "15", EventName = "Championship Match", EventDate = DateTime.Today.AddHours(19) }
        );

        // Seed stadium seats
        var stadiumSeats = new List<StadiumSeat>();
        int seatId = 1;

        // Section A: 10 rows × 10 seats = 100 seats
        for (int row = 1; row <= 10; row++)
        {
            for (int seat = 1; seat <= 10; seat++)
            {
                stadiumSeats.Add(new StadiumSeat
                {
                    Id = seatId++,
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
                    Id = seatId++,
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
                    Id = seatId++,
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
                    Id = seatId++,
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
