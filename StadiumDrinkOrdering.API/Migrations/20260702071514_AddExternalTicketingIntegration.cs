using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddExternalTicketingIntegration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExternalTicketId",
                table: "Tickets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceSystem",
                table: "Tickets",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExternalEventId",
                table: "Events",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SourceSystem",
                table: "Events",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "IntegrationInboxEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IdempotencyKey = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    EventType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SourceSystem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    ReceivedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationInboxEntries", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4669));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4744));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4746));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4747));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4749));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4750));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4751));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4752));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "ExternalEventId", "SourceSystem" },
                values: new object[] { new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(4837), new DateTime(2026, 7, 2, 19, 0, 0, 0, DateTimeKind.Utc), null, null });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6968));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6979));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6982));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6983));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6983));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6986));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(6987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7061));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7061));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7062));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7062));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7065));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7065));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7140));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7176));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7177));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7179));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7179));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7180));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7180));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7200));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7200));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7241));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7242));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7242));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7243));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7243));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7244));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7244));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7245));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7245));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7246));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7246));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7246));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7247));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7247));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7248));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7248));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7249));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7249));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7250));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7250));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7250));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7251));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7252));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7254));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7257));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7257));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7259));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7259));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7261));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7305));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7305));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7311));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7311));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(7364));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "ExternalTicketId", "PurchaseDate", "QRCodeToken", "SourceSystem" },
                values: new object[] { new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5056), new DateTime(2026, 7, 2, 19, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5142), "2ae45e53-0ccd-420b-929d-17d75e4147a4", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "ExternalTicketId", "PurchaseDate", "QRCodeToken", "SourceSystem" },
                values: new object[] { new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5143), new DateTime(2026, 7, 2, 19, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5149), "767a8432-644f-4ebe-b155-fa6666c738e6", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "ExternalTicketId", "PurchaseDate", "QRCodeToken", "SourceSystem" },
                values: new object[] { new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5150), new DateTime(2026, 7, 2, 19, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5182), "693ebb1f-0ea8-4b0d-944e-582f5f9a4f7c", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "ExternalTicketId", "PurchaseDate", "QRCodeToken", "SourceSystem" },
                values: new object[] { new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5182), new DateTime(2026, 7, 2, 19, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5188), "05869eac-2181-44c9-b1a0-1789d3a88c80", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "ExternalTicketId", "PurchaseDate", "QRCodeToken", "SourceSystem" },
                values: new object[] { new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5189), new DateTime(2026, 7, 2, 19, 0, 0, 0, DateTimeKind.Utc), null, new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(5194), "2c775746-0cac-48d1-97b1-ab75ae2b1f88", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 2, 7, 15, 12, 279, DateTimeKind.Utc).AddTicks(3841), "$2a$11$.R98KaRswuGrDwMjbEGNU.jnwYe5uJ9EkyWdE43kXhWlh9dXx1PoW" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_ExternalTicketId",
                table: "Tickets",
                column: "ExternalTicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_ExternalEventId",
                table: "Events",
                column: "ExternalEventId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationInboxEntries_IdempotencyKey",
                table: "IntegrationInboxEntries",
                column: "IdempotencyKey",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationInboxEntries_ReceivedAt",
                table: "IntegrationInboxEntries",
                column: "ReceivedAt");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationInboxEntries");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_ExternalTicketId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Events_ExternalEventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "ExternalTicketId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SourceSystem",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ExternalEventId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "SourceSystem",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9151));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9163));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9165));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9167));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9171));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9243), new DateTime(2026, 7, 1, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9640));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9640));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9641));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9641));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9648));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9648));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9650));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9943));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9943));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9950));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9950));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9953));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9953));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9954));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9954));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9955));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9955));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9956));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9956));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9956));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9957));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9957));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9959));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9959));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9963));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9963));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(2));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(2));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(3));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(3));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(3));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(4));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(4));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(5));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(5));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(6));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(6));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(6));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(7));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(7));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(8));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(8));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(8));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(9));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(9));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(10));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(10));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(11));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(11));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(12));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(12));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(12));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(13));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(13));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(14));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(14));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(15));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(15));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(15));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(16));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(16));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(17));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(17));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(17));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(18));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(18));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(19));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(19));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(20));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(21));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(21));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(22));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(22));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(23));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(23));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(23));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(24));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(24));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(25));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(25));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(27));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(27));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(28));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(28));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(28));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(29));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(29));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(30));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(30));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(31));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(31));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(65));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(66));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(66));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(67));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 1, 12, 57, 43, 433, DateTimeKind.Utc).AddTicks(68));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9478), new DateTime(2026, 7, 1, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9573), "6a1894bb-bd29-4a5b-af2e-c5530cfbb499" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9574), new DateTime(2026, 7, 1, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9580), "d0cee18e-9608-4c2b-8ee3-296210209e18" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9581), new DateTime(2026, 7, 1, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9586), "d89066cb-ed0b-4d96-9490-457b707948c9" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9587), new DateTime(2026, 7, 1, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9592), "004917d2-6988-4917-81d2-1d3aa974f568" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9593), new DateTime(2026, 7, 1, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(9611), "1bde334f-dd65-4c23-9e4a-0ad19b7af8ff" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(3337), "$2a$11$0KMsfK01wDzqGgI7MActvev4cUZB6QNjNLMGsy73H94aOVN1C4mBi" });
        }
    }
}
