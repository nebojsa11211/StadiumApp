using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEventManagementSystemV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Drinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    StockQuantity = table.Column<int>(type: "INTEGER", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: true),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Drinks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    EventType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    VenueId = table.Column<int>(type: "INTEGER", nullable: true),
                    TotalSeats = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: true),
                    ImageUrl = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    BaseTicketPrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StadiumSeats",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Section = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    RowNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    XCoordinate = table.Column<int>(type: "INTEGER", nullable: false),
                    YCoordinate = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StadiumSeats", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StadiumSections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SectionCode = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    SectionName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TotalRows = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "INTEGER", nullable: false),
                    PriceMultiplier = table.Column<decimal>(type: "TEXT", precision: 5, scale: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    Color = table.Column<string>(type: "TEXT", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StadiumSections", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Username = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    PasswordHash = table.Column<string>(type: "TEXT", nullable: false),
                    Role = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginAt = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EventAnalytics",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalTicketsSold = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "TEXT", precision: 12, scale: 2, nullable: false),
                    TotalOrders = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalDrinksSold = table.Column<int>(type: "INTEGER", nullable: false),
                    AverageOrderValue = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    PeakOrderTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    MostPopularDrink = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CalculatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SectionId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsAccessible = table.Column<bool>(type: "INTEGER", nullable: false),
                    XCoordinate = table.Column<int>(type: "INTEGER", nullable: false),
                    YCoordinate = table.Column<int>(type: "INTEGER", nullable: false)
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
                name: "EventStaffAssignments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    StaffId = table.Column<int>(type: "INTEGER", nullable: false),
                    AssignedSections = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Role = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ShiftStart = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ShiftEnd = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Type = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Title = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Data = table.Column<string>(type: "TEXT", nullable: true),
                    IsRead = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ReadAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Priority = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    TargetRole = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: true)
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
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EventId = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatId = table.Column<int>(type: "INTEGER", nullable: false),
                    QRCode = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    QRCodeToken = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CustomerName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CustomerEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    CustomerPhone = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    PurchaseDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Price = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    IsUsed = table.Column<bool>(type: "INTEGER", nullable: false),
                    UsedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    SeatNumber = table.Column<string>(type: "TEXT", maxLength: 10, nullable: true),
                    Section = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Row = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    EventName = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                name: "OrderSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TicketId = table.Column<int>(type: "INTEGER", nullable: false),
                    SessionToken = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    IpAddress = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CartData = table.Column<string>(type: "TEXT", nullable: true),
                    CartTotal = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    ItemCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LastActivity = table.Column<DateTime>(type: "TEXT", nullable: true)
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
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    DrinkId = table.Column<int>(type: "INTEGER", nullable: false),
                    Quantity = table.Column<int>(type: "INTEGER", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    SpecialInstructions = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EventId = table.Column<int>(type: "INTEGER", nullable: true),
                    SeatId = table.Column<int>(type: "INTEGER", nullable: true),
                    PaymentId = table.Column<int>(type: "INTEGER", nullable: true),
                    SessionId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeliveryNotes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    EstimatedDeliveryTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ActualDeliveryTime = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AssignedStaffId = table.Column<int>(type: "INTEGER", nullable: true),
                    TicketNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SeatNumber = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CustomerId = table.Column<int>(type: "INTEGER", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AcceptedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    PreparedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    DeliveredAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CancelledAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    AcceptedByUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    PreparedByUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    DeliveredByUserId = table.Column<int>(type: "INTEGER", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CustomerNotes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    StadiumSeatId = table.Column<int>(type: "INTEGER", nullable: true),
                    TicketId = table.Column<int>(type: "INTEGER", nullable: true)
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
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    OrderId = table.Column<int>(type: "INTEGER", nullable: false),
                    PaymentMethod = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    TransactionId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    Amount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: false),
                    Currency = table.Column<string>(type: "TEXT", maxLength: 3, nullable: false),
                    Status = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    PaymentGatewayResponse = table.Column<string>(type: "TEXT", nullable: true),
                    RefundAmount = table.Column<decimal>(type: "TEXT", precision: 10, scale: 2, nullable: true),
                    RefundDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    RefundReason = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ProcessedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FailedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    FailureReason = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
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
                    { 1, 2, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6164), "Classic Coca Cola", null, true, "Coca Cola", 3.50m, 100, null },
                    { 2, 2, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6177), "Pepsi Cola", null, true, "Pepsi", 3.50m, 100, null },
                    { 3, 3, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6179), "Bottled Water", null, true, "Water", 2.00m, 200, null },
                    { 4, 1, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6180), "Local Draft Beer", null, true, "Beer", 6.00m, 150, null },
                    { 5, 4, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6181), "Hot Coffee", null, true, "Coffee", 4.00m, 80, null },
                    { 6, 6, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6183), "Fresh Orange Juice", null, true, "Orange Juice", 4.50m, 60, null },
                    { 7, 7, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6184), "Energy Drink", null, true, "Red Bull", 5.00m, 90, null },
                    { 8, 5, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6185), "Hot Green Tea", null, true, "Green Tea", 3.00m, 70, null }
                });

            migrationBuilder.InsertData(
                table: "Events",
                columns: new[] { "Id", "BaseTicketPrice", "CreatedAt", "Description", "EventDate", "EventName", "EventType", "ImageUrl", "IsActive", "TotalSeats", "UpdatedAt", "VenueId" },
                values: new object[] { 1, 50.00m, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6500), "Championship final match", new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), "Championship Match", "Football", null, true, 100, null, null });

            migrationBuilder.InsertData(
                table: "StadiumSeats",
                columns: new[] { "Id", "CreatedAt", "IsActive", "RowNumber", "SeatNumber", "Section", "XCoordinate", "YCoordinate" },
                values: new object[,]
                {
                    { 6, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6849), true, 1, 1, "A", 50, 50 },
                    { 7, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6855), true, 1, 2, "A", 65, 50 },
                    { 8, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6862), true, 1, 3, "A", 80, 50 },
                    { 9, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6863), true, 1, 4, "A", 95, 50 },
                    { 10, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6864), true, 1, 5, "A", 110, 50 },
                    { 11, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6865), true, 1, 6, "A", 125, 50 },
                    { 12, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6866), true, 1, 7, "A", 140, 50 },
                    { 13, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6867), true, 1, 8, "A", 155, 50 },
                    { 14, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6868), true, 1, 9, "A", 170, 50 },
                    { 15, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6869), true, 1, 10, "A", 185, 50 },
                    { 16, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6870), true, 2, 1, "A", 50, 65 },
                    { 17, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6871), true, 2, 2, "A", 65, 65 },
                    { 18, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6872), true, 2, 3, "A", 80, 65 },
                    { 19, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6873), true, 2, 4, "A", 95, 65 },
                    { 20, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6874), true, 2, 5, "A", 110, 65 },
                    { 21, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6874), true, 2, 6, "A", 125, 65 },
                    { 22, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6875), true, 2, 7, "A", 140, 65 },
                    { 23, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6877), true, 2, 8, "A", 155, 65 },
                    { 24, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6878), true, 2, 9, "A", 170, 65 },
                    { 25, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6879), true, 2, 10, "A", 185, 65 },
                    { 26, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6879), true, 3, 1, "A", 50, 80 },
                    { 27, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6880), true, 3, 2, "A", 65, 80 },
                    { 28, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6881), true, 3, 3, "A", 80, 80 },
                    { 29, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6882), true, 3, 4, "A", 95, 80 },
                    { 30, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6882), true, 3, 5, "A", 110, 80 },
                    { 31, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6883), true, 3, 6, "A", 125, 80 },
                    { 32, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6884), true, 3, 7, "A", 140, 80 },
                    { 33, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6885), true, 3, 8, "A", 155, 80 },
                    { 34, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6885), true, 3, 9, "A", 170, 80 },
                    { 35, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6934), true, 3, 10, "A", 185, 80 },
                    { 36, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6936), true, 4, 1, "A", 50, 95 },
                    { 37, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6937), true, 4, 2, "A", 65, 95 },
                    { 38, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6937), true, 4, 3, "A", 80, 95 },
                    { 39, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6939), true, 4, 4, "A", 95, 95 },
                    { 40, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6940), true, 4, 5, "A", 110, 95 },
                    { 41, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6941), true, 4, 6, "A", 125, 95 },
                    { 42, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6941), true, 4, 7, "A", 140, 95 },
                    { 43, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6942), true, 4, 8, "A", 155, 95 },
                    { 44, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6943), true, 4, 9, "A", 170, 95 },
                    { 45, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6944), true, 4, 10, "A", 185, 95 },
                    { 46, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6945), true, 5, 1, "A", 50, 110 },
                    { 47, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6945), true, 5, 2, "A", 65, 110 },
                    { 48, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6946), true, 5, 3, "A", 80, 110 },
                    { 49, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6947), true, 5, 4, "A", 95, 110 },
                    { 50, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6948), true, 5, 5, "A", 110, 110 },
                    { 51, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6948), true, 5, 6, "A", 125, 110 },
                    { 52, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6949), true, 5, 7, "A", 140, 110 },
                    { 53, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6950), true, 5, 8, "A", 155, 110 },
                    { 54, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6951), true, 5, 9, "A", 170, 110 },
                    { 55, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6951), true, 5, 10, "A", 185, 110 },
                    { 56, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6952), true, 6, 1, "A", 50, 125 },
                    { 57, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6953), true, 6, 2, "A", 65, 125 },
                    { 58, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6954), true, 6, 3, "A", 80, 125 },
                    { 59, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6955), true, 6, 4, "A", 95, 125 },
                    { 60, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6955), true, 6, 5, "A", 110, 125 },
                    { 61, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6956), true, 6, 6, "A", 125, 125 },
                    { 62, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6957), true, 6, 7, "A", 140, 125 },
                    { 63, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6958), true, 6, 8, "A", 155, 125 },
                    { 64, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6958), true, 6, 9, "A", 170, 125 },
                    { 65, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6959), true, 6, 10, "A", 185, 125 },
                    { 66, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6960), true, 7, 1, "A", 50, 140 },
                    { 67, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6961), true, 7, 2, "A", 65, 140 },
                    { 68, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6961), true, 7, 3, "A", 80, 140 },
                    { 69, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6962), true, 7, 4, "A", 95, 140 },
                    { 70, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6963), true, 7, 5, "A", 110, 140 },
                    { 71, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6964), true, 7, 6, "A", 125, 140 },
                    { 72, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6965), true, 7, 7, "A", 140, 140 },
                    { 73, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6966), true, 7, 8, "A", 155, 140 },
                    { 74, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6966), true, 7, 9, "A", 170, 140 },
                    { 75, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6967), true, 7, 10, "A", 185, 140 },
                    { 76, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6968), true, 8, 1, "A", 50, 155 },
                    { 77, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6969), true, 8, 2, "A", 65, 155 },
                    { 78, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6969), true, 8, 3, "A", 80, 155 },
                    { 79, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6970), true, 8, 4, "A", 95, 155 },
                    { 80, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6971), true, 8, 5, "A", 110, 155 },
                    { 81, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6972), true, 8, 6, "A", 125, 155 },
                    { 82, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6973), true, 8, 7, "A", 140, 155 },
                    { 83, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6973), true, 8, 8, "A", 155, 155 },
                    { 84, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6974), true, 8, 9, "A", 170, 155 },
                    { 85, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6975), true, 8, 10, "A", 185, 155 },
                    { 86, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6975), true, 9, 1, "A", 50, 170 },
                    { 87, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6976), true, 9, 2, "A", 65, 170 },
                    { 88, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6977), true, 9, 3, "A", 80, 170 },
                    { 89, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6978), true, 9, 4, "A", 95, 170 },
                    { 90, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6979), true, 9, 5, "A", 110, 170 },
                    { 91, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6979), true, 9, 6, "A", 125, 170 },
                    { 92, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6980), true, 9, 7, "A", 140, 170 },
                    { 93, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6981), true, 9, 8, "A", 155, 170 },
                    { 94, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6981), true, 9, 9, "A", 170, 170 },
                    { 95, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6982), true, 9, 10, "A", 185, 170 },
                    { 96, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6983), true, 10, 1, "A", 50, 185 },
                    { 97, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6984), true, 10, 2, "A", 65, 185 },
                    { 98, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6984), true, 10, 3, "A", 80, 185 },
                    { 99, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6985), true, 10, 4, "A", 95, 185 },
                    { 100, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6986), true, 10, 5, "A", 110, 185 },
                    { 101, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6987), true, 10, 6, "A", 125, 185 },
                    { 102, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6987), true, 10, 7, "A", 140, 185 },
                    { 103, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6988), true, 10, 8, "A", 155, 185 },
                    { 104, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7024), true, 10, 9, "A", 170, 185 },
                    { 105, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7026), true, 10, 10, "A", 185, 185 },
                    { 106, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7027), true, 1, 1, "B", 300, 50 },
                    { 107, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7029), true, 1, 2, "B", 315, 50 },
                    { 108, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7029), true, 1, 3, "B", 330, 50 },
                    { 109, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7030), true, 1, 4, "B", 345, 50 },
                    { 110, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7031), true, 1, 5, "B", 360, 50 },
                    { 111, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7032), true, 1, 6, "B", 375, 50 },
                    { 112, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7032), true, 1, 7, "B", 390, 50 },
                    { 113, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7033), true, 1, 8, "B", 405, 50 },
                    { 114, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7034), true, 1, 9, "B", 420, 50 },
                    { 115, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7035), true, 1, 10, "B", 435, 50 },
                    { 116, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7036), true, 2, 1, "B", 300, 65 },
                    { 117, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7037), true, 2, 2, "B", 315, 65 },
                    { 118, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7037), true, 2, 3, "B", 330, 65 },
                    { 119, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7038), true, 2, 4, "B", 345, 65 },
                    { 120, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7039), true, 2, 5, "B", 360, 65 },
                    { 121, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7040), true, 2, 6, "B", 375, 65 },
                    { 122, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7040), true, 2, 7, "B", 390, 65 },
                    { 123, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7041), true, 2, 8, "B", 405, 65 },
                    { 124, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7042), true, 2, 9, "B", 420, 65 },
                    { 125, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7043), true, 2, 10, "B", 435, 65 },
                    { 126, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7043), true, 3, 1, "B", 300, 80 },
                    { 127, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7044), true, 3, 2, "B", 315, 80 },
                    { 128, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7045), true, 3, 3, "B", 330, 80 },
                    { 129, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7046), true, 3, 4, "B", 345, 80 },
                    { 130, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7046), true, 3, 5, "B", 360, 80 },
                    { 131, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7047), true, 3, 6, "B", 375, 80 },
                    { 132, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7048), true, 3, 7, "B", 390, 80 },
                    { 133, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7049), true, 3, 8, "B", 405, 80 },
                    { 134, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7049), true, 3, 9, "B", 420, 80 },
                    { 135, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7051), true, 3, 10, "B", 435, 80 },
                    { 136, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7052), true, 4, 1, "B", 300, 95 },
                    { 137, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7053), true, 4, 2, "B", 315, 95 },
                    { 138, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7053), true, 4, 3, "B", 330, 95 },
                    { 139, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7054), true, 4, 4, "B", 345, 95 },
                    { 140, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7055), true, 4, 5, "B", 360, 95 },
                    { 141, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7056), true, 4, 6, "B", 375, 95 },
                    { 142, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7057), true, 4, 7, "B", 390, 95 },
                    { 143, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7057), true, 4, 8, "B", 405, 95 },
                    { 144, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7058), true, 4, 9, "B", 420, 95 },
                    { 145, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7059), true, 4, 10, "B", 435, 95 },
                    { 146, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7060), true, 5, 1, "B", 300, 110 },
                    { 147, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7060), true, 5, 2, "B", 315, 110 },
                    { 148, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7061), true, 5, 3, "B", 330, 110 },
                    { 149, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7062), true, 5, 4, "B", 345, 110 },
                    { 150, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7063), true, 5, 5, "B", 360, 110 },
                    { 151, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7063), true, 5, 6, "B", 375, 110 },
                    { 152, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7064), true, 5, 7, "B", 390, 110 },
                    { 153, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7065), true, 5, 8, "B", 405, 110 },
                    { 154, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7066), true, 5, 9, "B", 420, 110 },
                    { 155, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7066), true, 5, 10, "B", 435, 110 },
                    { 156, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7067), true, 6, 1, "B", 300, 125 },
                    { 157, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7068), true, 6, 2, "B", 315, 125 },
                    { 158, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7068), true, 6, 3, "B", 330, 125 },
                    { 159, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7069), true, 6, 4, "B", 345, 125 },
                    { 160, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7070), true, 6, 5, "B", 360, 125 },
                    { 161, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7071), true, 6, 6, "B", 375, 125 },
                    { 162, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7071), true, 6, 7, "B", 390, 125 },
                    { 163, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7072), true, 6, 8, "B", 405, 125 },
                    { 164, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7073), true, 6, 9, "B", 420, 125 },
                    { 165, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7073), true, 6, 10, "B", 435, 125 },
                    { 166, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7074), true, 7, 1, "B", 300, 140 },
                    { 167, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7075), true, 7, 2, "B", 315, 140 },
                    { 168, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7111), true, 7, 3, "B", 330, 140 },
                    { 169, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7113), true, 7, 4, "B", 345, 140 },
                    { 170, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7113), true, 7, 5, "B", 360, 140 },
                    { 171, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7114), true, 7, 6, "B", 375, 140 },
                    { 172, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7115), true, 7, 7, "B", 390, 140 },
                    { 173, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7115), true, 7, 8, "B", 405, 140 },
                    { 174, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7116), true, 7, 9, "B", 420, 140 },
                    { 175, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7117), true, 7, 10, "B", 435, 140 },
                    { 176, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7118), true, 8, 1, "B", 300, 155 },
                    { 177, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7118), true, 8, 2, "B", 315, 155 },
                    { 178, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7119), true, 8, 3, "B", 330, 155 },
                    { 179, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7120), true, 8, 4, "B", 345, 155 },
                    { 180, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7120), true, 8, 5, "B", 360, 155 },
                    { 181, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7121), true, 8, 6, "B", 375, 155 },
                    { 182, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7122), true, 8, 7, "B", 390, 155 },
                    { 183, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7123), true, 8, 8, "B", 405, 155 },
                    { 184, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7123), true, 8, 9, "B", 420, 155 },
                    { 185, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7124), true, 8, 10, "B", 435, 155 },
                    { 186, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7125), true, 9, 1, "B", 300, 170 },
                    { 187, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7126), true, 9, 2, "B", 315, 170 },
                    { 188, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7126), true, 9, 3, "B", 330, 170 },
                    { 189, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7127), true, 9, 4, "B", 345, 170 },
                    { 190, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7128), true, 9, 5, "B", 360, 170 },
                    { 191, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7128), true, 9, 6, "B", 375, 170 },
                    { 192, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7129), true, 9, 7, "B", 390, 170 },
                    { 193, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7130), true, 9, 8, "B", 405, 170 },
                    { 194, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7130), true, 9, 9, "B", 420, 170 },
                    { 195, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7131), true, 9, 10, "B", 435, 170 },
                    { 196, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7132), true, 10, 1, "B", 300, 185 },
                    { 197, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7133), true, 10, 2, "B", 315, 185 },
                    { 198, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7133), true, 10, 3, "B", 330, 185 },
                    { 199, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7134), true, 10, 4, "B", 345, 185 },
                    { 200, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7135), true, 10, 5, "B", 360, 185 },
                    { 201, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7135), true, 10, 6, "B", 375, 185 },
                    { 202, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7136), true, 10, 7, "B", 390, 185 },
                    { 203, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7137), true, 10, 8, "B", 405, 185 },
                    { 204, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7137), true, 10, 9, "B", 420, 185 },
                    { 205, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7138), true, 10, 10, "B", 435, 185 },
                    { 206, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7139), true, 1, 1, "C", 550, 50 },
                    { 207, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7141), true, 1, 2, "C", 565, 50 },
                    { 208, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7141), true, 1, 3, "C", 580, 50 },
                    { 209, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7142), true, 1, 4, "C", 595, 50 },
                    { 210, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7143), true, 1, 5, "C", 610, 50 },
                    { 211, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7143), true, 1, 6, "C", 625, 50 },
                    { 212, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7144), true, 1, 7, "C", 640, 50 },
                    { 213, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7145), true, 1, 8, "C", 655, 50 },
                    { 214, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7146), true, 1, 9, "C", 670, 50 },
                    { 215, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7146), true, 1, 10, "C", 685, 50 },
                    { 216, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7147), true, 2, 1, "C", 550, 65 },
                    { 217, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7148), true, 2, 2, "C", 565, 65 },
                    { 218, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7149), true, 2, 3, "C", 580, 65 },
                    { 219, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7150), true, 2, 4, "C", 595, 65 },
                    { 220, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7150), true, 2, 5, "C", 610, 65 },
                    { 221, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7151), true, 2, 6, "C", 625, 65 },
                    { 222, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7152), true, 2, 7, "C", 640, 65 },
                    { 223, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7153), true, 2, 8, "C", 655, 65 },
                    { 224, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7153), true, 2, 9, "C", 670, 65 },
                    { 225, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7154), true, 2, 10, "C", 685, 65 },
                    { 226, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7155), true, 3, 1, "C", 550, 80 },
                    { 227, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7156), true, 3, 2, "C", 565, 80 },
                    { 228, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7156), true, 3, 3, "C", 580, 80 },
                    { 229, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7157), true, 3, 4, "C", 595, 80 },
                    { 230, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7158), true, 3, 5, "C", 610, 80 },
                    { 231, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7159), true, 3, 6, "C", 625, 80 },
                    { 232, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7159), true, 3, 7, "C", 640, 80 },
                    { 233, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7160), true, 3, 8, "C", 655, 80 },
                    { 234, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7161), true, 3, 9, "C", 670, 80 },
                    { 235, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7162), true, 3, 10, "C", 685, 80 },
                    { 236, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7163), true, 4, 1, "C", 550, 95 },
                    { 237, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7163), true, 4, 2, "C", 565, 95 },
                    { 238, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7164), true, 4, 3, "C", 580, 95 },
                    { 239, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7165), true, 4, 4, "C", 595, 95 },
                    { 240, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7166), true, 4, 5, "C", 610, 95 },
                    { 241, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7166), true, 4, 6, "C", 625, 95 },
                    { 242, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7167), true, 4, 7, "C", 640, 95 },
                    { 243, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7168), true, 4, 8, "C", 655, 95 },
                    { 244, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7169), true, 4, 9, "C", 670, 95 },
                    { 245, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7170), true, 4, 10, "C", 685, 95 },
                    { 246, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7170), true, 5, 1, "C", 550, 110 },
                    { 247, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7171), true, 5, 2, "C", 565, 110 },
                    { 248, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7172), true, 5, 3, "C", 580, 110 },
                    { 249, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7173), true, 5, 4, "C", 595, 110 },
                    { 250, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7173), true, 5, 5, "C", 610, 110 },
                    { 251, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7174), true, 5, 6, "C", 625, 110 },
                    { 252, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7175), true, 5, 7, "C", 640, 110 },
                    { 253, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7175), true, 5, 8, "C", 655, 110 },
                    { 254, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7208), true, 5, 9, "C", 670, 110 },
                    { 255, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7209), true, 5, 10, "C", 685, 110 },
                    { 256, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7210), true, 6, 1, "C", 550, 125 },
                    { 257, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7210), true, 6, 2, "C", 565, 125 },
                    { 258, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7211), true, 6, 3, "C", 580, 125 },
                    { 259, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7212), true, 6, 4, "C", 595, 125 },
                    { 260, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7213), true, 6, 5, "C", 610, 125 },
                    { 261, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7214), true, 6, 6, "C", 625, 125 },
                    { 262, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7214), true, 6, 7, "C", 640, 125 },
                    { 263, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7216), true, 6, 8, "C", 655, 125 },
                    { 264, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7217), true, 6, 9, "C", 670, 125 },
                    { 265, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7218), true, 6, 10, "C", 685, 125 },
                    { 266, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7219), true, 7, 1, "C", 550, 140 },
                    { 267, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7220), true, 7, 2, "C", 565, 140 },
                    { 268, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7221), true, 7, 3, "C", 580, 140 },
                    { 269, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7221), true, 7, 4, "C", 595, 140 },
                    { 270, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7222), true, 7, 5, "C", 610, 140 },
                    { 271, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7223), true, 7, 6, "C", 625, 140 },
                    { 272, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7224), true, 7, 7, "C", 640, 140 },
                    { 273, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7224), true, 7, 8, "C", 655, 140 },
                    { 274, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7225), true, 7, 9, "C", 670, 140 },
                    { 275, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7226), true, 7, 10, "C", 685, 140 },
                    { 276, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7227), true, 8, 1, "C", 550, 155 },
                    { 277, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7228), true, 8, 2, "C", 565, 155 },
                    { 278, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7228), true, 8, 3, "C", 580, 155 },
                    { 279, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7229), true, 8, 4, "C", 595, 155 },
                    { 280, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7230), true, 8, 5, "C", 610, 155 },
                    { 281, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7231), true, 8, 6, "C", 625, 155 },
                    { 282, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7231), true, 8, 7, "C", 640, 155 },
                    { 283, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7232), true, 8, 8, "C", 655, 155 },
                    { 284, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7233), true, 8, 9, "C", 670, 155 },
                    { 285, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7234), true, 8, 10, "C", 685, 155 },
                    { 286, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7234), true, 9, 1, "C", 550, 170 },
                    { 287, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7235), true, 9, 2, "C", 565, 170 },
                    { 288, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7236), true, 9, 3, "C", 580, 170 },
                    { 289, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7237), true, 9, 4, "C", 595, 170 },
                    { 290, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7238), true, 9, 5, "C", 610, 170 },
                    { 291, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7238), true, 9, 6, "C", 625, 170 },
                    { 292, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7239), true, 9, 7, "C", 640, 170 },
                    { 293, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7240), true, 9, 8, "C", 655, 170 },
                    { 294, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7241), true, 9, 9, "C", 670, 170 },
                    { 295, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7241), true, 9, 10, "C", 685, 170 },
                    { 296, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7265), true, 10, 1, "C", 550, 185 },
                    { 297, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7267), true, 10, 2, "C", 565, 185 },
                    { 298, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7267), true, 10, 3, "C", 580, 185 },
                    { 299, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7268), true, 10, 4, "C", 595, 185 },
                    { 300, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7269), true, 10, 5, "C", 610, 185 },
                    { 301, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7270), true, 10, 6, "C", 625, 185 },
                    { 302, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7270), true, 10, 7, "C", 640, 185 },
                    { 303, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7271), true, 10, 8, "C", 655, 185 },
                    { 304, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7272), true, 10, 9, "C", 670, 185 },
                    { 305, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7273), true, 10, 10, "C", 685, 185 },
                    { 306, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7274), true, 1, 1, "D", 350, 370 },
                    { 307, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7275), true, 1, 2, "D", 365, 370 },
                    { 308, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7276), true, 1, 3, "D", 380, 370 },
                    { 309, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7277), true, 1, 4, "D", 395, 370 },
                    { 310, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7278), true, 1, 5, "D", 410, 370 },
                    { 311, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7278), true, 1, 6, "D", 425, 370 },
                    { 312, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7279), true, 1, 7, "D", 440, 370 },
                    { 313, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7280), true, 1, 8, "D", 455, 370 },
                    { 314, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7281), true, 1, 9, "D", 470, 370 },
                    { 315, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7282), true, 1, 10, "D", 485, 370 },
                    { 316, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7282), true, 1, 11, "D", 500, 370 },
                    { 317, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7283), true, 1, 12, "D", 515, 370 },
                    { 318, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7284), true, 1, 13, "D", 530, 370 },
                    { 319, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7285), true, 1, 14, "D", 545, 370 },
                    { 320, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7286), true, 2, 1, "D", 350, 385 },
                    { 321, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7287), true, 2, 2, "D", 365, 385 },
                    { 322, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7287), true, 2, 3, "D", 380, 385 },
                    { 323, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7288), true, 2, 4, "D", 395, 385 },
                    { 324, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7289), true, 2, 5, "D", 410, 385 },
                    { 325, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7289), true, 2, 6, "D", 425, 385 },
                    { 326, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7290), true, 2, 7, "D", 440, 385 },
                    { 327, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7291), true, 2, 8, "D", 455, 385 },
                    { 328, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7292), true, 2, 9, "D", 470, 385 },
                    { 329, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7292), true, 2, 10, "D", 485, 385 },
                    { 330, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7293), true, 2, 11, "D", 500, 385 },
                    { 331, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7294), true, 2, 12, "D", 515, 385 },
                    { 332, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7295), true, 2, 13, "D", 530, 385 },
                    { 333, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7295), true, 2, 14, "D", 545, 385 },
                    { 334, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7296), true, 3, 1, "D", 350, 400 },
                    { 335, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7297), true, 3, 2, "D", 365, 400 },
                    { 336, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7298), true, 3, 3, "D", 380, 400 },
                    { 337, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7299), true, 3, 4, "D", 395, 400 },
                    { 338, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7299), true, 3, 5, "D", 410, 400 },
                    { 339, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7300), true, 3, 6, "D", 425, 400 },
                    { 340, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7301), true, 3, 7, "D", 440, 400 },
                    { 341, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7301), true, 3, 8, "D", 455, 400 },
                    { 342, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7302), true, 3, 9, "D", 470, 400 },
                    { 343, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7303), true, 3, 10, "D", 485, 400 },
                    { 344, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7304), true, 3, 11, "D", 500, 400 },
                    { 345, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7304), true, 3, 12, "D", 515, 400 },
                    { 346, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7305), true, 3, 13, "D", 530, 400 },
                    { 347, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7306), true, 3, 14, "D", 545, 400 },
                    { 348, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7307), true, 4, 1, "D", 350, 415 },
                    { 349, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7307), true, 4, 2, "D", 365, 415 },
                    { 350, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7308), true, 4, 3, "D", 380, 415 },
                    { 351, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7309), true, 4, 4, "D", 395, 415 },
                    { 352, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7310), true, 4, 5, "D", 410, 415 },
                    { 353, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7310), true, 4, 6, "D", 425, 415 },
                    { 354, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7311), true, 4, 7, "D", 440, 415 },
                    { 355, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7312), true, 4, 8, "D", 455, 415 },
                    { 356, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7313), true, 4, 9, "D", 470, 415 },
                    { 357, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7313), true, 4, 10, "D", 485, 415 },
                    { 358, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7314), true, 4, 11, "D", 500, 415 },
                    { 359, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7315), true, 4, 12, "D", 515, 415 },
                    { 360, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7315), true, 4, 13, "D", 530, 415 },
                    { 361, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7316), true, 4, 14, "D", 545, 415 },
                    { 362, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7317), true, 5, 1, "D", 350, 430 },
                    { 363, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7318), true, 5, 2, "D", 365, 430 },
                    { 364, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7318), true, 5, 3, "D", 380, 430 },
                    { 365, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7319), true, 5, 4, "D", 395, 430 },
                    { 366, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7320), true, 5, 5, "D", 410, 430 },
                    { 367, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7321), true, 5, 6, "D", 425, 430 },
                    { 368, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7321), true, 5, 7, "D", 440, 430 },
                    { 369, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7322), true, 5, 8, "D", 455, 430 },
                    { 370, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7323), true, 5, 9, "D", 470, 430 },
                    { 371, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7324), true, 5, 10, "D", 485, 430 },
                    { 372, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7324), true, 5, 11, "D", 500, 430 },
                    { 373, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7325), true, 5, 12, "D", 515, 430 },
                    { 374, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7326), true, 5, 13, "D", 530, 430 },
                    { 375, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7327), true, 5, 14, "D", 545, 430 },
                    { 376, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7327), true, 6, 1, "D", 350, 445 },
                    { 377, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7328), true, 6, 2, "D", 365, 445 },
                    { 378, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7329), true, 6, 3, "D", 380, 445 },
                    { 379, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7414), true, 6, 4, "D", 395, 445 },
                    { 380, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7415), true, 6, 5, "D", 410, 445 },
                    { 381, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7416), true, 6, 6, "D", 425, 445 },
                    { 382, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7417), true, 6, 7, "D", 440, 445 },
                    { 383, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7417), true, 6, 8, "D", 455, 445 },
                    { 384, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7418), true, 6, 9, "D", 470, 445 },
                    { 385, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7419), true, 6, 10, "D", 485, 445 },
                    { 386, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7420), true, 6, 11, "D", 500, 445 },
                    { 387, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7420), true, 6, 12, "D", 515, 445 },
                    { 388, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7421), true, 6, 13, "D", 530, 445 },
                    { 389, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7422), true, 6, 14, "D", 545, 445 },
                    { 390, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7423), true, 7, 1, "D", 350, 460 },
                    { 391, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7423), true, 7, 2, "D", 365, 460 },
                    { 392, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7424), true, 7, 3, "D", 380, 460 },
                    { 393, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7425), true, 7, 4, "D", 395, 460 },
                    { 394, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7426), true, 7, 5, "D", 410, 460 },
                    { 395, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7426), true, 7, 6, "D", 425, 460 },
                    { 396, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7427), true, 7, 7, "D", 440, 460 },
                    { 397, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7428), true, 7, 8, "D", 455, 460 },
                    { 398, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7429), true, 7, 9, "D", 470, 460 },
                    { 399, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7429), true, 7, 10, "D", 485, 460 },
                    { 400, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7430), true, 7, 11, "D", 500, 460 },
                    { 401, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7431), true, 7, 12, "D", 515, 460 },
                    { 402, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7431), true, 7, 13, "D", 530, 460 },
                    { 403, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7432), true, 7, 14, "D", 545, 460 }
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
                values: new object[] { 1, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(5224), "admin@stadium.com", null, "$2a$11$cnFDUR1doXuho784NiwgGO3WZoZ.J2VDMcRXXLoOlv6S0GvOQyifu", 2, "admin" });

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
                    { 1, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6683), null, null, null, new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), 1, "Championship Match", true, false, 50.00m, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6682), "", "4ee0dced-f9a1-46fd-b4a1-e073ebcf0cdf", "1", 1, "A1", "A", "Active", "TK001", null },
                    { 2, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6771), null, null, null, new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), 1, "Championship Match", true, false, 50.00m, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6770), "", "e464fe6d-b2c4-4e77-96c1-33707b52120b", "1", 2, "A2", "A", "Active", "TK002", null },
                    { 3, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6779), null, null, null, new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), 1, "Championship Match", true, false, 60.00m, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6779), "", "c2838b7d-fc76-4c94-bd39-a2eeaf432f6b", "5", 3, "B5", "B", "Active", "TK003", null },
                    { 4, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6786), null, null, null, new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), 1, "Championship Match", true, false, 75.00m, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6785), "", "e0c5a531-7e4b-4670-ad35-096bb82fb717", "10", 4, "C10", "C", "Active", "TK004", null },
                    { 5, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6801), null, null, null, new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), 1, "Championship Match", true, false, 100.00m, new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6800), "", "e8e85cc3-7487-4b6c-8d06-80d17a570dd3", "15", 5, "D15", "D", "Active", "TK005", null }
                });

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
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

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
                name: "IX_Orders_TicketId",
                table: "Orders",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_TicketNumber",
                table: "Orders",
                column: "TicketNumber");

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
                name: "IX_Payments_TransactionId",
                table: "Payments",
                column: "TransactionId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seats_SectionId_RowNumber_SeatNumber",
                table: "Seats",
                columns: new[] { "SectionId", "RowNumber", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSeats_Section_RowNumber_SeatNumber",
                table: "StadiumSeats",
                columns: new[] { "Section", "RowNumber", "SeatNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSections_SectionName",
                table: "StadiumSections",
                column: "SectionName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_EventId",
                table: "Tickets",
                column: "EventId");

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
                name: "IX_Tickets_TicketNumber",
                table: "Tickets",
                column: "TicketNumber",
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
                name: "EventAnalytics");

            migrationBuilder.DropTable(
                name: "EventStaffAssignments");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Drinks");

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
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Seats");

            migrationBuilder.DropTable(
                name: "StadiumSections");
        }
    }
}
