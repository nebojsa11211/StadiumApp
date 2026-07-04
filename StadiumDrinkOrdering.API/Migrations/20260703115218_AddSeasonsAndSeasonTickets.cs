using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSeasonsAndSeasonTickets : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "Tickets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SeasonTicketId",
                table: "Tickets",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SeasonId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Seasons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StartDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsCurrent = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ExternalSeasonId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SourceSystem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeasonTickets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SeasonId = table.Column<int>(type: "integer", nullable: false),
                    SeatId = table.Column<int>(type: "integer", nullable: false),
                    SeasonTicketNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    HolderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HolderEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    HolderPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Price = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExternalSeasonTicketId = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SourceSystem = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeasonTickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeasonTickets_Seasons_SeasonId",
                        column: x => x.SeasonId,
                        principalTable: "Seasons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SeasonTickets_Seats_SeatId",
                        column: x => x.SeatId,
                        principalTable: "Seats",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9039));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9060));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9062));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9063));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9064));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9065));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9067));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9068));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "SeasonId" },
                values: new object[] { new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9162), null });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9530));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9548));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9598));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9599));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9601));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9601));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9602));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9602));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9603));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9603));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9604));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9604));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9606));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9606));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9607));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9609));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9609));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9610));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9610));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9611));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9611));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9611));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9613));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9613));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9614));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9614));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9615));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9615));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9616));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9616));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9617));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9617));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9618));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9618));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9623));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9623));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9658));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9659));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9661));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9662));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9663));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9664));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9664));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9665));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9665));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9665));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9666));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9666));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9667));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9667));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9671));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9671));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9675));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9675));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9676));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9677));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9677));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9678));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9678));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9679));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9679));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9680));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9680));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9681));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9681));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9682));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9682));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9684));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9684));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9686));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9686));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9688));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9688));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9856));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9856));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Kind", "PurchaseDate", "QRCodeToken", "SeasonTicketId" },
                values: new object[] { new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9385), 0, new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9462), "2ea306fe-1f47-4add-b841-8282a571e64d", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "Kind", "PurchaseDate", "QRCodeToken", "SeasonTicketId" },
                values: new object[] { new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9463), 0, new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9469), "c3632498-8ee8-4f02-a809-4c01bd53d55d", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "Kind", "PurchaseDate", "QRCodeToken", "SeasonTicketId" },
                values: new object[] { new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9470), 0, new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9479), "e785e33b-3793-49cc-af73-3fed2f61c75e", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "Kind", "PurchaseDate", "QRCodeToken", "SeasonTicketId" },
                values: new object[] { new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9480), 0, new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9493), "7c808f7d-225c-4df3-864d-c188e5c8d6bc", null });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "Kind", "PurchaseDate", "QRCodeToken", "SeasonTicketId" },
                values: new object[] { new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9494), 0, new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(9499), "95e3cc93-3d1b-457d-924d-f9276ef947f5", null });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 3, 11, 52, 15, 989, DateTimeKind.Utc).AddTicks(8249), "$2a$11$2mR5d6eDCcaLM8nnLZsru.Jgz9wz4P2Z510oTZThUJcMFNk3Ce2Ke" });

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_SeasonTicketId",
                table: "Tickets",
                column: "SeasonTicketId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_SeasonId",
                table: "Events",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_ExternalSeasonId",
                table: "Seasons",
                column: "ExternalSeasonId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Seasons_Name",
                table: "Seasons",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTickets_ExternalSeasonTicketId",
                table: "SeasonTickets",
                column: "ExternalSeasonTicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTickets_HolderEmail",
                table: "SeasonTickets",
                column: "HolderEmail");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTickets_SeasonId",
                table: "SeasonTickets",
                column: "SeasonId");

            migrationBuilder.CreateIndex(
                name: "IX_SeasonTickets_SeatId",
                table: "SeasonTickets",
                column: "SeatId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Seasons_SeasonId",
                table: "Events",
                column: "SeasonId",
                principalTable: "Seasons",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Tickets_SeasonTickets_SeasonTicketId",
                table: "Tickets",
                column: "SeasonTicketId",
                principalTable: "SeasonTickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Seasons_SeasonId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Tickets_SeasonTickets_SeasonTicketId",
                table: "Tickets");

            migrationBuilder.DropTable(
                name: "SeasonTickets");

            migrationBuilder.DropTable(
                name: "Seasons");

            migrationBuilder.DropIndex(
                name: "IX_Tickets_SeasonTicketId",
                table: "Tickets");

            migrationBuilder.DropIndex(
                name: "IX_Events_SeasonId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SeasonTicketId",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SeasonId",
                table: "Events");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9219));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9226));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9228));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9229));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9231));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9232));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9233));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9234));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9580));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9584));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9584));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9587));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9590));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9590));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9592));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9592));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9618));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9618));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9620));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9623));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9623));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9640));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9640));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9641));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9641));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9648));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9648));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9650));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9650));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9678));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9679));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9680));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9681));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9681));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9682));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9684));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9685));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9686));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9688));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9688));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9690));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9690));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9691));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9691));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9692));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9692));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9693));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9693));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9694));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9694));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9695));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9695));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9696));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9697));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9743));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9743));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9749));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9749));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9751));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9753));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9753));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9757));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9757));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9758));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9760));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9761));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9761));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9762));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9762));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9763));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9763));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9765));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9766));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9766));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9767));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9767));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9768));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9768));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9769));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9769));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9772));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9775));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9775));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9777));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9777));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9780));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9780));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9785));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9848));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9851));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9852));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9852));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9856));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9940));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9943));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9943));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9947));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9950));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9950));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9952));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9464), new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9532), "bf120662-8b45-4e92-8e91-7b35653736cc" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9533), new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9539), "9809d4c1-d555-4ea2-b008-4c1e7b4e0db9" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9540), new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9545), "7b7b019a-a6e2-46bb-a141-e4054825bb7b" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9546), new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9551), "70bd3c3b-500b-40ee-a985-80c0ff0cced7" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9552), new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(9557), "07e8a89f-6e32-4767-b2ca-a13814126074" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 3, 10, 41, 9, 517, DateTimeKind.Utc).AddTicks(8651), "$2a$11$PIxExWj8tbTz1FH/tQphQOQC1syGn0qTZmXIRH0PreWaPKJ.BD.N6" });
        }
    }
}
