using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgreSQLMigrationFixed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "integer", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EventType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    VenueId = table.Column<int>(type: "integer", nullable: true),
                    TotalSeats = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    BaseTicketPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Level = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UserId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserRole = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    IPAddress = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RequestPath = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HttpMethod = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Details = table.Column<string>(type: "text", nullable: true),
                    ExceptionType = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StackTrace = table.Column<string>(type: "text", nullable: true),
                    Source = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    BusinessEntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BusinessEntityId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    BusinessEntityName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    RelatedEntityType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RelatedEntityId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MonetaryAmount = table.Column<decimal>(type: "numeric", nullable: true),
                    Currency = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: true),
                    LocationInfo = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    StatusBefore = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    StatusAfter = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    MetadataJson = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StadiumSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Section = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false),
                    XCoordinate = table.Column<int>(type: "integer", nullable: false),
                    YCoordinate = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StadiumSeats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StadiumSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionCode = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    SectionName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TotalRows = table.Column<int>(type: "integer", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "integer", nullable: false),
                    PriceMultiplier = table.Column<decimal>(type: "numeric(5,2)", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Color = table.Column<string>(type: "character varying(7)", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StadiumSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tribunes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Code = table.Column<string>(type: "character varying(1)", maxLength: 1, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tribunes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Role = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    TotalTicketsSold = table.Column<int>(type: "integer", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TicketRevenue = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    DrinksRevenue = table.Column<decimal>(type: "numeric(12,2)", precision: 12, scale: 2, nullable: false),
                    TotalOrders = table.Column<int>(type: "integer", nullable: false),
                    TotalDrinksSold = table.Column<int>(type: "integer", nullable: false),
                    AverageOrderValue = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    PeakOrderTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    MostPopularDrink = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventAnalytics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventAnalytics_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectionId = table.Column<int>(type: "integer", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsAccessible = table.Column<bool>(type: "boolean", nullable: false),
                    XCoordinate = table.Column<int>(type: "integer", nullable: false),
                    YCoordinate = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seats", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seats_StadiumSections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "StadiumSections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TribuneId = table.Column<int>(type: "integer", nullable: false),
                    Number = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rings_Tribunes_TribuneId",
                        column: x => x.TribuneId,
                        principalTable: "Tribunes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EventStaffAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    StaffId = table.Column<int>(type: "integer", nullable: false),
                    AssignedSections = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ShiftStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ShiftEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventStaffAssignments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventStaffAssignments_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EventStaffAssignments_Users_StaffId",
                        column: x => x.StaffId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    Type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Data = table.Column<string>(type: "text", nullable: true),
                    IsRead = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Priority = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    TargetRole = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EventId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Notifications_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Notifications_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SeatReservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    SectorId = table.Column<int>(type: "integer", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    ReservedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReservedUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeatReservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeatReservations_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeatReservations_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "ShoppingCarts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShoppingCarts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShoppingCarts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    SeatId = table.Column<int>(type: "integer", nullable: true),
                    QRCode = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    QRCodeToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    CustomerPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    SeatNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    Section = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Row = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    EventName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Tickets_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RingId = table.Column<int>(type: "integer", nullable: false),
                    Code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TotalRows = table.Column<int>(type: "integer", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "integer", nullable: false),
                    StartRow = table.Column<int>(type: "integer", nullable: false),
                    StartSeat = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Sectors_Rings_RingId",
                        column: x => x.RingId,
                        principalTable: "Rings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShoppingCartId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    SectorId = table.Column<int>(type: "integer", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatCode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    AddedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ReservedUntil = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_ShoppingCarts_ShoppingCartId",
                        column: x => x.ShoppingCartId,
                        principalTable: "ShoppingCarts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    SessionToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IpAddress = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CartData = table.Column<string>(type: "text", nullable: true),
                    CartTotal = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    ItemCount = table.Column<int>(type: "integer", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderSessions_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TicketSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    QRCodeToken = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    TicketId = table.Column<int>(type: "integer", nullable: false),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    SeatId = table.Column<int>(type: "integer", nullable: true),
                    SeatNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Section = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Row = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CustomerEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastAccessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TicketSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TicketSessions_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TicketSessions_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TicketSessions_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StadiumSeatsNew",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SectorId = table.Column<int>(type: "integer", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    SeatNumber = table.Column<int>(type: "integer", nullable: false),
                    UniqueCode = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StadiumSeatsNew", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StadiumSeatsNew_Sectors_SectorId",
                        column: x => x.SectorId,
                        principalTable: "Sectors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: false),
                    DrinkId = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    SpecialInstructions = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Drinks_DrinkId",
                        column: x => x.DrinkId,
                        principalTable: "Drinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: true),
                    SeatId = table.Column<int>(type: "integer", nullable: true),
                    PaymentId = table.Column<int>(type: "integer", nullable: true),
                    SessionId = table.Column<int>(type: "integer", nullable: true),
                    TicketSessionId = table.Column<int>(type: "integer", nullable: true),
                    DeliveryNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ActualDeliveryTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AssignedStaffId = table.Column<int>(type: "integer", nullable: true),
                    TicketNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SeatNumber = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    CustomerId = table.Column<int>(type: "integer", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    PreparedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    AcceptedByUserId = table.Column<int>(type: "integer", nullable: true),
                    PreparedByUserId = table.Column<int>(type: "integer", nullable: true),
                    DeliveredByUserId = table.Column<int>(type: "integer", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CustomerNotes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    StadiumSeatId = table.Column<int>(type: "integer", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: true),
                    TicketSessionId1 = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_OrderSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "OrderSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_StadiumSeats_StadiumSeatId",
                        column: x => x.StadiumSeatId,
                        principalTable: "StadiumSeats",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_TicketSessions_TicketSessionId",
                        column: x => x.TicketSessionId,
                        principalTable: "TicketSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_TicketSessions_TicketSessionId1",
                        column: x => x.TicketSessionId1,
                        principalTable: "TicketSessions",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Users_AcceptedByUserId",
                        column: x => x.AcceptedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_AssignedStaffId",
                        column: x => x.AssignedStaffId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Orders_Users_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_DeliveredByUserId",
                        column: x => x.DeliveredByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Users_PreparedByUserId",
                        column: x => x.PreparedByUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    PaymentMethod = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PaymentGatewayResponse = table.Column<string>(type: "text", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true),
                    RefundDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    RefundReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    FailureReason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Drinks",
                columns: new[] { "Id", "Category", "CreatedAt", "Description", "ImageUrl", "IsAvailable", "Name", "Price", "StockQuantity", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 2, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5866), "Classic Coca Cola", null, true, "Coca Cola", 3.50m, 100, null },
                    { 2, 2, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5891), "Pepsi Cola", null, true, "Pepsi", 3.50m, 100, null },
                    { 3, 3, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5893), "Bottled Water", null, true, "Water", 2.00m, 200, null },
                    { 4, 1, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5895), "Local Draft Beer", null, true, "Beer", 6.00m, 150, null },
                    { 5, 4, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5898), "Hot Coffee", null, true, "Coffee", 4.00m, 80, null },
                    { 6, 6, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5899), "Fresh Orange Juice", null, true, "Orange Juice", 4.50m, 60, null },
                    { 7, 7, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5901), "Energy Drink", null, true, "Red Bull", 5.00m, 90, null },
                    { 8, 5, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5903), "Hot Green Tea", null, true, "Green Tea", 3.00m, 70, null }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "BaseTicketPrice", "CreatedAt", "Description", "EventDate", "EventName", "EventType", "ImageUrl", "IsActive", "TotalSeats", "UpdatedAt", "VenueId" },
                values: new object[] { 1, 50.00m, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6054), "Championship final match", new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), "Championship Match", "Football", null, true, 100, null, null });

            migrationBuilder.InsertData(
                table: "StadiumSeats",
                columns: new[] { "Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6745), true, 1, 1, "A", 50, 50 },
                    { 7, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6773), true, 1, 2, "A", 65, 50 },
                    { 8, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6774), true, 1, 3, "A", 80, 50 },
                    { 9, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6776), true, 1, 4, "A", 95, 50 },
                    { 10, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6777), true, 1, 5, "A", 110, 50 },
                    { 11, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6779), true, 1, 6, "A", 125, 50 },
                    { 12, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6787), true, 1, 7, "A", 140, 50 },
                    { 13, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6788), true, 1, 8, "A", 155, 50 },
                    { 14, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6789), true, 1, 9, "A", 170, 50 },
                    { 15, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6791), true, 1, 10, "A", 185, 50 },
                    { 16, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6792), true, 2, 1, "A", 50, 65 },
                    { 17, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6793), true, 2, 2, "A", 65, 65 },
                    { 18, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6795), true, 2, 3, "A", 80, 65 },
                    { 19, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6796), true, 2, 4, "A", 95, 65 },
                    { 20, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6797), true, 2, 5, "A", 110, 65 },
                    { 21, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6798), true, 2, 6, "A", 125, 65 },
                    { 22, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6799), true, 2, 7, "A", 140, 65 },
                    { 23, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6801), true, 2, 8, "A", 155, 65 },
                    { 24, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6802), true, 2, 9, "A", 170, 65 },
                    { 25, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6803), true, 2, 10, "A", 185, 65 },
                    { 26, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6804), true, 3, 1, "A", 50, 80 },
                    { 27, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6805), true, 3, 2, "A", 65, 80 },
                    { 28, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6806), true, 3, 3, "A", 80, 80 },
                    { 29, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6807), true, 3, 4, "A", 95, 80 },
                    { 30, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6808), true, 3, 5, "A", 110, 80 },
                    { 31, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6809), true, 3, 6, "A", 125, 80 },
                    { 32, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6810), true, 3, 7, "A", 140, 80 },
                    { 33, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6811), true, 3, 8, "A", 155, 80 },
                    { 34, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6812), true, 3, 9, "A", 170, 80 },
                    { 35, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6813), true, 3, 10, "A", 185, 80 },
                    { 36, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6814), true, 4, 1, "A", 50, 95 },
                    { 37, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6815), true, 4, 2, "A", 65, 95 },
                    { 38, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6816), true, 4, 3, "A", 80, 95 },
                    { 39, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6819), true, 4, 4, "A", 95, 95 },
                    { 40, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6820), true, 4, 5, "A", 110, 95 },
                    { 41, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6821), true, 4, 6, "A", 125, 95 },
                    { 42, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6822), true, 4, 7, "A", 140, 95 },
                    { 43, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6823), true, 4, 8, "A", 155, 95 },
                    { 44, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6824), true, 4, 9, "A", 170, 95 },
                    { 45, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6825), true, 4, 10, "A", 185, 95 },
                    { 46, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6827), true, 5, 1, "A", 50, 110 },
                    { 47, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6828), true, 5, 2, "A", 65, 110 },
                    { 48, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6829), true, 5, 3, "A", 80, 110 },
                    { 49, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6830), true, 5, 4, "A", 95, 110 },
                    { 50, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6831), true, 5, 5, "A", 110, 110 },
                    { 51, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6832), true, 5, 6, "A", 125, 110 },
                    { 52, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6833), true, 5, 7, "A", 140, 110 },
                    { 53, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6834), true, 5, 8, "A", 155, 110 },
                    { 54, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6835), true, 5, 9, "A", 170, 110 },
                    { 55, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6837), true, 5, 10, "A", 185, 110 },
                    { 56, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6838), true, 6, 1, "A", 50, 125 },
                    { 57, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6839), true, 6, 2, "A", 65, 125 },
                    { 58, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6840), true, 6, 3, "A", 80, 125 },
                    { 59, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6841), true, 6, 4, "A", 95, 125 },
                    { 60, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6842), true, 6, 5, "A", 110, 125 },
                    { 61, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6843), true, 6, 6, "A", 125, 125 },
                    { 62, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6844), true, 6, 7, "A", 140, 125 },
                    { 63, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6846), true, 6, 8, "A", 155, 125 },
                    { 64, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6847), true, 6, 9, "A", 170, 125 },
                    { 65, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6848), true, 6, 10, "A", 185, 125 },
                    { 66, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6849), true, 7, 1, "A", 50, 140 },
                    { 67, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6850), true, 7, 2, "A", 65, 140 },
                    { 68, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6851), true, 7, 3, "A", 80, 140 },
                    { 69, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6852), true, 7, 4, "A", 95, 140 },
                    { 70, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6853), true, 7, 5, "A", 110, 140 },
                    { 71, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6909), true, 7, 6, "A", 125, 140 },
                    { 72, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6911), true, 7, 7, "A", 140, 140 },
                    { 73, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6912), true, 7, 8, "A", 155, 140 },
                    { 74, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6913), true, 7, 9, "A", 170, 140 },
                    { 75, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6914), true, 7, 10, "A", 185, 140 },
                    { 76, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6915), true, 8, 1, "A", 50, 155 },
                    { 77, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6916), true, 8, 2, "A", 65, 155 },
                    { 78, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6917), true, 8, 3, "A", 80, 155 },
                    { 79, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6918), true, 8, 4, "A", 95, 155 },
                    { 80, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6919), true, 8, 5, "A", 110, 155 },
                    { 81, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6920), true, 8, 6, "A", 125, 155 },
                    { 82, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6922), true, 8, 7, "A", 140, 155 },
                    { 83, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6923), true, 8, 8, "A", 155, 155 },
                    { 84, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6923), true, 8, 9, "A", 170, 155 },
                    { 85, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6924), true, 8, 10, "A", 185, 155 },
                    { 86, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6926), true, 9, 1, "A", 50, 170 },
                    { 87, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6927), true, 9, 2, "A", 65, 170 },
                    { 88, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6928), true, 9, 3, "A", 80, 170 },
                    { 89, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6929), true, 9, 4, "A", 95, 170 },
                    { 90, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6930), true, 9, 5, "A", 110, 170 },
                    { 91, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6931), true, 9, 6, "A", 125, 170 },
                    { 92, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6931), true, 9, 7, "A", 140, 170 },
                    { 93, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6933), true, 9, 8, "A", 155, 170 },
                    { 94, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6933), true, 9, 9, "A", 170, 170 },
                    { 95, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6935), true, 9, 10, "A", 185, 170 },
                    { 96, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6936), true, 10, 1, "A", 50, 185 },
                    { 97, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6937), true, 10, 2, "A", 65, 185 },
                    { 98, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6938), true, 10, 3, "A", 80, 185 },
                    { 99, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6939), true, 10, 4, "A", 95, 185 },
                    { 100, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6940), true, 10, 5, "A", 110, 185 },
                    { 101, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6941), true, 10, 6, "A", 125, 185 },
                    { 102, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6942), true, 10, 7, "A", 140, 185 },
                    { 103, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6943), true, 10, 8, "A", 155, 185 },
                    { 104, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6944), true, 10, 9, "A", 170, 185 },
                    { 105, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6944), true, 10, 10, "A", 185, 185 },
                    { 106, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6946), true, 1, 1, "B", 300, 50 },
                    { 107, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6948), true, 1, 2, "B", 315, 50 },
                    { 108, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6949), true, 1, 3, "B", 330, 50 },
                    { 109, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6951), true, 1, 4, "B", 345, 50 },
                    { 110, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6952), true, 1, 5, "B", 360, 50 },
                    { 111, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6953), true, 1, 6, "B", 375, 50 },
                    { 112, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6953), true, 1, 7, "B", 390, 50 },
                    { 113, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6954), true, 1, 8, "B", 405, 50 },
                    { 114, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6955), true, 1, 9, "B", 420, 50 },
                    { 115, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6957), true, 1, 10, "B", 435, 50 },
                    { 116, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6958), true, 2, 1, "B", 300, 65 },
                    { 117, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6959), true, 2, 2, "B", 315, 65 },
                    { 118, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6960), true, 2, 3, "B", 330, 65 },
                    { 119, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6961), true, 2, 4, "B", 345, 65 },
                    { 120, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6962), true, 2, 5, "B", 360, 65 },
                    { 121, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6963), true, 2, 6, "B", 375, 65 },
                    { 122, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6964), true, 2, 7, "B", 390, 65 },
                    { 123, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6965), true, 2, 8, "B", 405, 65 },
                    { 124, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6966), true, 2, 9, "B", 420, 65 },
                    { 125, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6967), true, 2, 10, "B", 435, 65 },
                    { 126, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6968), true, 3, 1, "B", 300, 80 },
                    { 127, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6969), true, 3, 2, "B", 315, 80 },
                    { 128, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6970), true, 3, 3, "B", 330, 80 },
                    { 129, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6971), true, 3, 4, "B", 345, 80 },
                    { 130, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6972), true, 3, 5, "B", 360, 80 },
                    { 131, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6974), true, 3, 6, "B", 375, 80 },
                    { 132, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6975), true, 3, 7, "B", 390, 80 },
                    { 133, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6976), true, 3, 8, "B", 405, 80 },
                    { 134, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6977), true, 3, 9, "B", 420, 80 },
                    { 135, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7026), true, 3, 10, "B", 435, 80 },
                    { 136, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7027), true, 4, 1, "B", 300, 95 },
                    { 137, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7029), true, 4, 2, "B", 315, 95 },
                    { 138, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7030), true, 4, 3, "B", 330, 95 },
                    { 139, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7031), true, 4, 4, "B", 345, 95 },
                    { 140, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7032), true, 4, 5, "B", 360, 95 },
                    { 141, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7033), true, 4, 6, "B", 375, 95 },
                    { 142, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7034), true, 4, 7, "B", 390, 95 },
                    { 143, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7035), true, 4, 8, "B", 405, 95 },
                    { 144, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7036), true, 4, 9, "B", 420, 95 },
                    { 145, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7037), true, 4, 10, "B", 435, 95 },
                    { 146, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7039), true, 5, 1, "B", 300, 110 },
                    { 147, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7040), true, 5, 2, "B", 315, 110 },
                    { 148, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7041), true, 5, 3, "B", 330, 110 },
                    { 149, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7042), true, 5, 4, "B", 345, 110 },
                    { 150, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7043), true, 5, 5, "B", 360, 110 },
                    { 151, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7044), true, 5, 6, "B", 375, 110 },
                    { 152, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7045), true, 5, 7, "B", 390, 110 },
                    { 153, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7046), true, 5, 8, "B", 405, 110 },
                    { 154, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7047), true, 5, 9, "B", 420, 110 },
                    { 155, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7048), true, 5, 10, "B", 435, 110 },
                    { 156, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7049), true, 6, 1, "B", 300, 125 },
                    { 157, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7051), true, 6, 2, "B", 315, 125 },
                    { 158, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7052), true, 6, 3, "B", 330, 125 },
                    { 159, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7053), true, 6, 4, "B", 345, 125 },
                    { 160, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7054), true, 6, 5, "B", 360, 125 },
                    { 161, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7055), true, 6, 6, "B", 375, 125 },
                    { 162, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7056), true, 6, 7, "B", 390, 125 },
                    { 163, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7057), true, 6, 8, "B", 405, 125 },
                    { 164, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7058), true, 6, 9, "B", 420, 125 },
                    { 165, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7059), true, 6, 10, "B", 435, 125 },
                    { 166, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7060), true, 7, 1, "B", 300, 140 },
                    { 167, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7331), true, 7, 2, "B", 315, 140 },
                    { 168, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7332), true, 7, 3, "B", 330, 140 },
                    { 169, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7334), true, 7, 4, "B", 345, 140 },
                    { 170, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7335), true, 7, 5, "B", 360, 140 },
                    { 171, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7336), true, 7, 6, "B", 375, 140 },
                    { 172, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7337), true, 7, 7, "B", 390, 140 },
                    { 173, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7338), true, 7, 8, "B", 405, 140 },
                    { 174, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7339), true, 7, 9, "B", 420, 140 },
                    { 175, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7340), true, 7, 10, "B", 435, 140 },
                    { 176, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7342), true, 8, 1, "B", 300, 155 },
                    { 177, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7343), true, 8, 2, "B", 315, 155 },
                    { 178, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7344), true, 8, 3, "B", 330, 155 },
                    { 179, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7345), true, 8, 4, "B", 345, 155 },
                    { 180, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7346), true, 8, 5, "B", 360, 155 },
                    { 181, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7348), true, 8, 6, "B", 375, 155 },
                    { 182, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7349), true, 8, 7, "B", 390, 155 },
                    { 183, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7350), true, 8, 8, "B", 405, 155 },
                    { 184, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7351), true, 8, 9, "B", 420, 155 },
                    { 185, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7352), true, 8, 10, "B", 435, 155 },
                    { 186, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7353), true, 9, 1, "B", 300, 170 },
                    { 187, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7354), true, 9, 2, "B", 315, 170 },
                    { 188, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7355), true, 9, 3, "B", 330, 170 },
                    { 189, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7357), true, 9, 4, "B", 345, 170 },
                    { 190, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7358), true, 9, 5, "B", 360, 170 },
                    { 191, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7359), true, 9, 6, "B", 375, 170 },
                    { 192, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7360), true, 9, 7, "B", 390, 170 },
                    { 193, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7361), true, 9, 8, "B", 405, 170 },
                    { 194, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7362), true, 9, 9, "B", 420, 170 },
                    { 195, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7363), true, 9, 10, "B", 435, 170 },
                    { 196, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7364), true, 10, 1, "B", 300, 185 },
                    { 197, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7366), true, 10, 2, "B", 315, 185 },
                    { 198, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7367), true, 10, 3, "B", 330, 185 },
                    { 199, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7368), true, 10, 4, "B", 345, 185 },
                    { 200, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7369), true, 10, 5, "B", 360, 185 },
                    { 201, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7370), true, 10, 6, "B", 375, 185 },
                    { 202, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7371), true, 10, 7, "B", 390, 185 },
                    { 203, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7372), true, 10, 8, "B", 405, 185 },
                    { 204, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7373), true, 10, 9, "B", 420, 185 },
                    { 205, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7375), true, 10, 10, "B", 435, 185 },
                    { 206, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7376), true, 1, 1, "C", 550, 50 },
                    { 207, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7378), true, 1, 2, "C", 565, 50 },
                    { 208, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7379), true, 1, 3, "C", 580, 50 },
                    { 209, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7381), true, 1, 4, "C", 595, 50 },
                    { 210, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7382), true, 1, 5, "C", 610, 50 },
                    { 211, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7383), true, 1, 6, "C", 625, 50 },
                    { 212, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7384), true, 1, 7, "C", 640, 50 },
                    { 213, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7385), true, 1, 8, "C", 655, 50 },
                    { 214, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7386), true, 1, 9, "C", 670, 50 },
                    { 215, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7387), true, 1, 10, "C", 685, 50 },
                    { 216, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7436), true, 2, 1, "C", 550, 65 },
                    { 217, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7437), true, 2, 2, "C", 565, 65 },
                    { 218, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7439), true, 2, 3, "C", 580, 65 },
                    { 219, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7440), true, 2, 4, "C", 595, 65 },
                    { 220, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7441), true, 2, 5, "C", 610, 65 },
                    { 221, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7442), true, 2, 6, "C", 625, 65 },
                    { 222, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7443), true, 2, 7, "C", 640, 65 },
                    { 223, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7444), true, 2, 8, "C", 655, 65 },
                    { 224, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7445), true, 2, 9, "C", 670, 65 },
                    { 225, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7446), true, 2, 10, "C", 685, 65 },
                    { 226, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7447), true, 3, 1, "C", 550, 80 },
                    { 227, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7449), true, 3, 2, "C", 565, 80 },
                    { 228, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7450), true, 3, 3, "C", 580, 80 },
                    { 229, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7451), true, 3, 4, "C", 595, 80 },
                    { 230, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7452), true, 3, 5, "C", 610, 80 },
                    { 231, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7453), true, 3, 6, "C", 625, 80 },
                    { 232, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7454), true, 3, 7, "C", 640, 80 },
                    { 233, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7456), true, 3, 8, "C", 655, 80 },
                    { 234, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7457), true, 3, 9, "C", 670, 80 },
                    { 235, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7457), true, 3, 10, "C", 685, 80 },
                    { 236, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7459), true, 4, 1, "C", 550, 95 },
                    { 237, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7459), true, 4, 2, "C", 565, 95 },
                    { 238, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7460), true, 4, 3, "C", 580, 95 },
                    { 239, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7462), true, 4, 4, "C", 595, 95 },
                    { 240, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7463), true, 4, 5, "C", 610, 95 },
                    { 241, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7464), true, 4, 6, "C", 625, 95 },
                    { 242, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7465), true, 4, 7, "C", 640, 95 },
                    { 243, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7466), true, 4, 8, "C", 655, 95 },
                    { 244, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7467), true, 4, 9, "C", 670, 95 },
                    { 245, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7468), true, 4, 10, "C", 685, 95 },
                    { 246, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7469), true, 5, 1, "C", 550, 110 },
                    { 247, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7470), true, 5, 2, "C", 565, 110 },
                    { 248, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7471), true, 5, 3, "C", 580, 110 },
                    { 249, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7472), true, 5, 4, "C", 595, 110 },
                    { 250, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7473), true, 5, 5, "C", 610, 110 },
                    { 251, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7475), true, 5, 6, "C", 625, 110 },
                    { 252, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7476), true, 5, 7, "C", 640, 110 },
                    { 253, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7477), true, 5, 8, "C", 655, 110 },
                    { 254, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7478), true, 5, 9, "C", 670, 110 },
                    { 255, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7479), true, 5, 10, "C", 685, 110 },
                    { 256, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7480), true, 6, 1, "C", 550, 125 },
                    { 257, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7481), true, 6, 2, "C", 565, 125 },
                    { 258, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7482), true, 6, 3, "C", 580, 125 },
                    { 259, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7483), true, 6, 4, "C", 595, 125 },
                    { 260, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7484), true, 6, 5, "C", 610, 125 },
                    { 261, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7486), true, 6, 6, "C", 625, 125 },
                    { 262, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7487), true, 6, 7, "C", 640, 125 },
                    { 263, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7526), true, 6, 8, "C", 655, 125 },
                    { 264, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7528), true, 6, 9, "C", 670, 125 },
                    { 265, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7529), true, 6, 10, "C", 685, 125 },
                    { 266, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7530), true, 7, 1, "C", 550, 140 },
                    { 267, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7531), true, 7, 2, "C", 565, 140 },
                    { 268, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7532), true, 7, 3, "C", 580, 140 },
                    { 269, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7533), true, 7, 4, "C", 595, 140 },
                    { 270, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7534), true, 7, 5, "C", 610, 140 },
                    { 271, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7535), true, 7, 6, "C", 625, 140 },
                    { 272, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7537), true, 7, 7, "C", 640, 140 },
                    { 273, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7538), true, 7, 8, "C", 655, 140 },
                    { 274, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7539), true, 7, 9, "C", 670, 140 },
                    { 275, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7540), true, 7, 10, "C", 685, 140 },
                    { 276, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7541), true, 8, 1, "C", 550, 155 },
                    { 277, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7542), true, 8, 2, "C", 565, 155 },
                    { 278, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7543), true, 8, 3, "C", 580, 155 },
                    { 279, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7544), true, 8, 4, "C", 595, 155 },
                    { 280, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7545), true, 8, 5, "C", 610, 155 },
                    { 281, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7546), true, 8, 6, "C", 625, 155 },
                    { 282, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7548), true, 8, 7, "C", 640, 155 },
                    { 283, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7549), true, 8, 8, "C", 655, 155 },
                    { 284, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7550), true, 8, 9, "C", 670, 155 },
                    { 285, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7551), true, 8, 10, "C", 685, 155 },
                    { 286, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7552), true, 9, 1, "C", 550, 170 },
                    { 287, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7553), true, 9, 2, "C", 565, 170 },
                    { 288, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7554), true, 9, 3, "C", 580, 170 },
                    { 289, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7556), true, 9, 4, "C", 595, 170 },
                    { 290, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7557), true, 9, 5, "C", 610, 170 },
                    { 291, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7558), true, 9, 6, "C", 625, 170 },
                    { 292, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7559), true, 9, 7, "C", 640, 170 },
                    { 293, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7560), true, 9, 8, "C", 655, 170 },
                    { 294, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7561), true, 9, 9, "C", 670, 170 },
                    { 295, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7562), true, 9, 10, "C", 685, 170 },
                    { 296, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7564), true, 10, 1, "C", 550, 185 },
                    { 297, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7565), true, 10, 2, "C", 565, 185 },
                    { 298, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7566), true, 10, 3, "C", 580, 185 },
                    { 299, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7567), true, 10, 4, "C", 595, 185 },
                    { 300, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7568), true, 10, 5, "C", 610, 185 },
                    { 301, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7569), true, 10, 6, "C", 625, 185 },
                    { 302, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7570), true, 10, 7, "C", 640, 185 },
                    { 303, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7571), true, 10, 8, "C", 655, 185 },
                    { 304, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7572), true, 10, 9, "C", 670, 185 },
                    { 305, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7573), true, 10, 10, "C", 685, 185 },
                    { 306, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7575), true, 1, 1, "D", 350, 370 },
                    { 307, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7576), true, 1, 2, "D", 365, 370 },
                    { 308, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7577), true, 1, 3, "D", 380, 370 },
                    { 309, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7578), true, 1, 4, "D", 395, 370 },
                    { 310, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7580), true, 1, 5, "D", 410, 370 },
                    { 311, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7581), true, 1, 6, "D", 425, 370 },
                    { 312, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7582), true, 1, 7, "D", 440, 370 },
                    { 313, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7583), true, 1, 8, "D", 455, 370 },
                    { 314, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7584), true, 1, 9, "D", 470, 370 },
                    { 315, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7585), true, 1, 10, "D", 485, 370 },
                    { 316, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7586), true, 1, 11, "D", 500, 370 },
                    { 317, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7587), true, 1, 12, "D", 515, 370 },
                    { 318, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7588), true, 1, 13, "D", 530, 370 },
                    { 319, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7589), true, 1, 14, "D", 545, 370 },
                    { 320, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7591), true, 2, 1, "D", 350, 385 },
                    { 321, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7592), true, 2, 2, "D", 365, 385 },
                    { 322, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7593), true, 2, 3, "D", 380, 385 },
                    { 323, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7594), true, 2, 4, "D", 395, 385 },
                    { 324, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7595), true, 2, 5, "D", 410, 385 },
                    { 325, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7596), true, 2, 6, "D", 425, 385 },
                    { 326, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7597), true, 2, 7, "D", 440, 385 },
                    { 327, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7598), true, 2, 8, "D", 455, 385 },
                    { 328, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7599), true, 2, 9, "D", 470, 385 },
                    { 329, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7600), true, 2, 10, "D", 485, 385 },
                    { 330, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7601), true, 2, 11, "D", 500, 385 },
                    { 331, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7602), true, 2, 12, "D", 515, 385 },
                    { 332, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7603), true, 2, 13, "D", 530, 385 },
                    { 333, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7604), true, 2, 14, "D", 545, 385 },
                    { 334, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7606), true, 3, 1, "D", 350, 400 },
                    { 335, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7607), true, 3, 2, "D", 365, 400 },
                    { 336, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7608), true, 3, 3, "D", 380, 400 },
                    { 337, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7609), true, 3, 4, "D", 395, 400 },
                    { 338, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7610), true, 3, 5, "D", 410, 400 },
                    { 339, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7611), true, 3, 6, "D", 425, 400 },
                    { 340, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7612), true, 3, 7, "D", 440, 400 },
                    { 341, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7720), true, 3, 8, "D", 455, 400 },
                    { 342, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7723), true, 3, 9, "D", 470, 400 },
                    { 343, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7724), true, 3, 10, "D", 485, 400 },
                    { 344, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7725), true, 3, 11, "D", 500, 400 },
                    { 345, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7726), true, 3, 12, "D", 515, 400 },
                    { 346, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7727), true, 3, 13, "D", 530, 400 },
                    { 347, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7728), true, 3, 14, "D", 545, 400 },
                    { 348, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7729), true, 4, 1, "D", 350, 415 },
                    { 349, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7730), true, 4, 2, "D", 365, 415 },
                    { 350, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7731), true, 4, 3, "D", 380, 415 },
                    { 351, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7732), true, 4, 4, "D", 395, 415 },
                    { 352, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7732), true, 4, 5, "D", 410, 415 },
                    { 353, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7733), true, 4, 6, "D", 425, 415 },
                    { 354, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7734), true, 4, 7, "D", 440, 415 },
                    { 355, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7745), true, 4, 8, "D", 455, 415 },
                    { 356, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7746), true, 4, 9, "D", 470, 415 },
                    { 357, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7747), true, 4, 10, "D", 485, 415 },
                    { 358, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7748), true, 4, 11, "D", 500, 415 },
                    { 359, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7749), true, 4, 12, "D", 515, 415 },
                    { 360, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7750), true, 4, 13, "D", 530, 415 },
                    { 361, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7751), true, 4, 14, "D", 545, 415 },
                    { 362, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7752), true, 5, 1, "D", 350, 430 },
                    { 363, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7753), true, 5, 2, "D", 365, 430 },
                    { 364, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7754), true, 5, 3, "D", 380, 430 },
                    { 365, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7755), true, 5, 4, "D", 395, 430 },
                    { 366, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7756), true, 5, 5, "D", 410, 430 },
                    { 367, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7757), true, 5, 6, "D", 425, 430 },
                    { 368, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7758), true, 5, 7, "D", 440, 430 },
                    { 369, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7759), true, 5, 8, "D", 455, 430 },
                    { 370, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7760), true, 5, 9, "D", 470, 430 },
                    { 371, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7761), true, 5, 10, "D", 485, 430 },
                    { 372, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7762), true, 5, 11, "D", 500, 430 },
                    { 373, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7763), true, 5, 12, "D", 515, 430 },
                    { 374, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7764), true, 5, 13, "D", 530, 430 },
                    { 375, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7765), true, 5, 14, "D", 545, 430 },
                    { 376, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7766), true, 6, 1, "D", 350, 445 },
                    { 377, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7767), true, 6, 2, "D", 365, 445 },
                    { 378, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7768), true, 6, 3, "D", 380, 445 },
                    { 379, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7769), true, 6, 4, "D", 395, 445 },
                    { 380, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7770), true, 6, 5, "D", 410, 445 },
                    { 381, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7771), true, 6, 6, "D", 425, 445 },
                    { 382, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7771), true, 6, 7, "D", 440, 445 },
                    { 383, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7772), true, 6, 8, "D", 455, 445 },
                    { 384, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7773), true, 6, 9, "D", 470, 445 },
                    { 385, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7774), true, 6, 10, "D", 485, 445 },
                    { 386, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7775), true, 6, 11, "D", 500, 445 },
                    { 387, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7776), true, 6, 12, "D", 515, 445 },
                    { 388, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7778), true, 6, 13, "D", 530, 445 },
                    { 389, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7778), true, 6, 14, "D", 545, 445 },
                    { 390, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7779), true, 7, 1, "D", 350, 460 },
                    { 391, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7781), true, 7, 2, "D", 365, 460 },
                    { 392, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7782), true, 7, 3, "D", 380, 460 },
                    { 393, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7783), true, 7, 4, "D", 395, 460 },
                    { 394, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7784), true, 7, 5, "D", 410, 460 },
                    { 395, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7784), true, 7, 6, "D", 425, 460 },
                    { 396, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7785), true, 7, 7, "D", 440, 460 },
                    { 397, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7786), true, 7, 8, "D", 455, 460 },
                    { 398, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7787), true, 7, 9, "D", 470, 460 },
                    { 399, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7788), true, 7, 10, "D", 485, 460 },
                    { 400, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7789), true, 7, 11, "D", 500, 460 },
                    { 401, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7790), true, 7, 12, "D", 515, 460 },
                    { 402, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7791), true, 7, 13, "D", 530, 460 },
                    { 403, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7792), true, 7, 14, "D", 545, 460 }
                });

            migrationBuilder.InsertData(
                table: "StadiumSections",
                columns: new[] { "Id", "Color", "IsActive", "PriceMultiplier", "SeatsPerRow", "SectionCode", "SectionName", "TotalRows" },
                values: new object[,]
                {
                    { 1, "#007bff", true, 1.0m, 10, "A", "Section A", 10 },
                    { 2, "#28a745", true, 1.2m, 10, "B", "Section B", 10 },
                    { 3, "#ffc107", true, 1.5m, 10, "C", "Section C", 10 },
                    { 4, "#dc3545", true, 2.0m, 14, "D", "Section D VIP", 7 }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "LastLoginAt", "PasswordHash", "Role", "Username" },
                values: new object[] { 1, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5007), "admin@stadium.com", null, "$2a$11$nxtbrbLFzp4QlNEdXhI4XuSEXFM.Xcb2cee2iqe8qagj2VP4uQDFS", 2, "admin" });

            migrationBuilder.InsertData(
                table: "Seats",
                columns: new[] { "Id", "IsAccessible", "RowNumber", "SeatCode", "SeatNumber", "SectionId", "XCoordinate", "YCoordinate" },
                values: new object[,]
                {
                    { 1, false, 1, "A-R1-S1", 1, 1, 50, 50 },
                    { 2, false, 1, "A-R1-S2", 2, 1, 65, 50 },
                    { 3, false, 5, "B-R5-S5", 5, 2, 375, 110 },
                    { 4, false, 10, "C-R10-S10", 10, 3, 685, 185 },
                    { 5, false, 3, "D-R3-S7", 7, 4, 440, 400 }
                });

            migrationBuilder.InsertData(
                table: "Tickets",
                columns: new[] { "Id", "CreatedAt", "CustomerEmail", "CustomerName", "CustomerPhone", "EventDate", "EventId", "EventName", "IsActive", "IsUsed", "Price", "PurchaseDate", "QRCode", "QRCodeToken", "Row", "SeatId", "SeatNumber", "Section", "Status", "TicketNumber", "UsedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6348), null, null, null, new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), 1, "Championship Match", true, false, 50.00m, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6484), "", "adecc671-b14b-4168-8528-b3a60c5a6fec", "1", 1, "A1", "A", "Active", "TK001", null },
                    { 2, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6487), null, null, null, new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), 1, "Championship Match", true, false, 50.00m, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6495), "", "959d776f-67cc-4781-9289-b6143a306892", "1", 2, "A2", "A", "Active", "TK002", null },
                    { 3, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6496), null, null, null, new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), 1, "Championship Match", true, false, 60.00m, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6510), "", "5e1c6dc4-f3c5-4607-9fdc-6e5514791c3c", "5", 3, "B5", "B", "Active", "TK003", null },
                    { 4, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6512), null, null, null, new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), 1, "Championship Match", true, false, 75.00m, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6518), "", "b74def6b-34e3-4569-bb64-aad300ad6282", "10", 4, "C10", "C", "Active", "TK004", null },
                    { 5, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6519), null, null, null, new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), 1, "Championship Match", true, false, 100.00m, new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6525), "", "87cc1c21-931b-4cc8-b917-fe545444ddb8", "15", 5, "D15", "D", "Active", "TK005", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_EventId_SectorId_RowNumber_SeatNumber",
                table: "CartItems",
                columns: new[] { "EventId", "SectorId", "RowNumber", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ReservedUntil",
                table: "CartItems",
                column: "ReservedUntil");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_ShoppingCartId",
                table: "CartItems",
                column: "ShoppingCartId");

            migrationBuilder.CreateIndex(
                name: "IX_EventAnalytics_EventId",
                table: "EventAnalytics",
                column: "EventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_EventName",
                table: "Events",
                column: "EventName");

            migrationBuilder.CreateIndex(
                name: "IX_EventStaffAssignments_EventId_StaffId",
                table: "EventStaffAssignments",
                columns: new[] { "EventId", "StaffId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventStaffAssignments_StaffId",
                table: "EventStaffAssignments",
                column: "StaffId");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_Level_Category",
                table: "LogEntries",
                columns: new[] { "Level", "Category" });

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_Source",
                table: "LogEntries",
                column: "Source");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_Timestamp",
                table: "LogEntries",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_LogEntries_UserId",
                table: "LogEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CreatedAt",
                table: "Notifications",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_EventId",
                table: "Notifications",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId_IsRead",
                table: "Notifications",
                columns: new[] { "UserId", "IsRead" });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_DrinkId",
                table: "OrderItems",
                column: "DrinkId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AcceptedByUserId",
                table: "Orders",
                column: "AcceptedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AssignedStaffId",
                table: "Orders",
                column: "AssignedStaffId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedAt",
                table: "Orders",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId_CreatedAt",
                table: "Orders",
                columns: new[] { "CustomerId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId_TotalAmount",
                table: "Orders",
                columns: new[] { "CustomerId", "TotalAmount" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveredByUserId",
                table: "Orders",
                column: "DeliveredByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_EventId",
                table: "Orders",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentId",
                table: "Orders",
                column: "PaymentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PreparedByUserId",
                table: "Orders",
                column: "PreparedByUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SeatId",
                table: "Orders",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SessionId",
                table: "Orders",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_StadiumSeatId",
                table: "Orders",
                column: "StadiumSeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Status_CreatedAt",
                table: "Orders",
                columns: new[] { "Status", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TicketId",
                table: "Orders",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TicketNumber",
                table: "Orders",
                column: "TicketNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TicketSessionId",
                table: "Orders",
                column: "TicketSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TicketSessionId1",
                table: "Orders",
                column: "TicketSessionId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderSessions_SessionToken",
                table: "OrderSessions",
                column: "SessionToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderSessions_TicketId",
                table: "OrderSessions",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentDate",
                table: "Payments",
                column: "PaymentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethod",
                table: "Payments",
                column: "PaymentMethod");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_PaymentMethod_PaymentDate",
                table: "Payments",
                columns: new[] { "PaymentMethod", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_Status_PaymentDate",
                table: "Payments",
                columns: new[] { "Status", "PaymentDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rings_TribuneId_Number",
                table: "Rings",
                columns: new[] { "TribuneId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_EventId_SectorId_RowNumber_SeatNumber",
                table: "SeatReservations",
                columns: new[] { "EventId", "SectorId", "RowNumber", "SeatNumber" });

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_ReservedUntil",
                table: "SeatReservations",
                column: "ReservedUntil");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_SessionId",
                table: "SeatReservations",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_Status",
                table: "SeatReservations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_SeatReservations_UserId",
                table: "SeatReservations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SectionId_RowNumber_SeatNumber",
                table: "Seats",
                columns: new[] { "SectionId", "RowNumber", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_RingId_Code",
                table: "Sectors",
                columns: new[] { "RingId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_ExpiresAt",
                table: "ShoppingCarts",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_SessionId",
                table: "ShoppingCarts",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCarts_UserId",
                table: "ShoppingCarts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSeats_Section_RowNumber_SeatNumber",
                table: "StadiumSeats",
                columns: new[] { "Section", "RowNumber", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSeatsNew_SectorId_RowNumber_SeatNumber",
                table: "StadiumSeatsNew",
                columns: new[] { "SectorId", "RowNumber", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSeatsNew_UniqueCode",
                table: "StadiumSeatsNew",
                column: "UniqueCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSections_SectionName",
                table: "StadiumSections",
                column: "SectionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerEmail",
                table: "Tickets",
                column: "CustomerEmail");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerEmail_Price",
                table: "Tickets",
                columns: new[] { "CustomerEmail", "Price" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_CustomerEmail_PurchaseDate",
                table: "Tickets",
                columns: new[] { "CustomerEmail", "PurchaseDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId_CustomerEmail",
                table: "Tickets",
                columns: new[] { "EventId", "CustomerEmail" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_PurchaseDate",
                table: "Tickets",
                column: "PurchaseDate");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_QRCodeToken",
                table: "Tickets",
                column: "QRCodeToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SeatId",
                table: "Tickets",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_Status_PurchaseDate",
                table: "Tickets",
                columns: new[] { "Status", "PurchaseDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_TicketNumber",
                table: "Tickets",
                column: "TicketNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketSessions_EventId",
                table: "TicketSessions",
                column: "EventId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSessions_ExpiresAt",
                table: "TicketSessions",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSessions_QRCodeToken",
                table: "TicketSessions",
                column: "QRCodeToken");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSessions_QRCodeToken_IsActive",
                table: "TicketSessions",
                columns: new[] { "QRCodeToken", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_TicketSessions_SeatId",
                table: "TicketSessions",
                column: "SeatId");

            migrationBuilder.CreateIndex(
                name: "IX_TicketSessions_SessionId",
                table: "TicketSessions",
                column: "SessionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TicketSessions_TicketId",
                table: "TicketSessions",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Tribunes_Code",
                table: "Tribunes",
                column: "Code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_Orders_OrderId",
                table: "OrderItems",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentId",
                table: "Orders",
                column: "PaymentId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Events_EventId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_Events_EventId",
                table: "Tickets");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketSessions_Events_EventId",
                table: "TicketSessions");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_AcceptedByUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_AssignedStaffId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_CustomerId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_DeliveredByUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_PreparedByUserId",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Orders_OrderId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "EventAnalytics");

            migrationBuilder.DropTable(
                name: "EventStaffAssignments");

            migrationBuilder.DropTable(
                name: "LogEntries");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "SeatReservations");

            migrationBuilder.DropTable(
                name: "StadiumSeatsNew");

            migrationBuilder.DropTable(
                name: "ShoppingCarts");

            migrationBuilder.DropTable(
                name: "Drinks");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Rings");

            migrationBuilder.DropTable(
                name: "Tribunes");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "OrderSessions");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "StadiumSeats");

            migrationBuilder.DropTable(
                name: "TicketSessions");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "StadiumSections");
        }
    }
}
