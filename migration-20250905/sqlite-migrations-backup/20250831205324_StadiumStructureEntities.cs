using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class StadiumStructureEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Tribunes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Code = table.Column<string>(type: "TEXT", maxLength: 1, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tribunes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TribuneId = table.Column<int>(type: "INTEGER", nullable: false),
                    Number = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                name: "Sectors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RingId = table.Column<int>(type: "INTEGER", nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 5, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    TotalRows = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatsPerRow = table.Column<int>(type: "INTEGER", nullable: false),
                    StartRow = table.Column<int>(type: "INTEGER", nullable: false),
                    StartSeat = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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
                name: "StadiumSeatsNew",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SectorId = table.Column<int>(type: "INTEGER", nullable: false),
                    RowNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    SeatNumber = table.Column<int>(type: "INTEGER", nullable: false),
                    UniqueCode = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    IsAvailable = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
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

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7089));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7099));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7100));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7102));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7104));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7105));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7107));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7108));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7232), new DateTime(2025, 8, 31, 19, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7666));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7669));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7674));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7675));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7676));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7677));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7678));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7679));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7681));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7682));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7684));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7685));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7686));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7690));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7691));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7692));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7693));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7694));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7695));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7696));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7696));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7697));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7751));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7753));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7757));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7758));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7760));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7761));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7762));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7763));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7765));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7766));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7767));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7768));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7769));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7773));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7775));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7777));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7780));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7785));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7786));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7786));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7851));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7852));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7856));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7967));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7968));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7971));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7973));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7974));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7977));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7978));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7979));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7982));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7983));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7988));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7990));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7991));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7992));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8000));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8084));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8085));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8086));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8087));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8087));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8088));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8089));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8090));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8091));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8092));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8092));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8093));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8094));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8095));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8096));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8096));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8097));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8098));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8099));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8102));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8105));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8106));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8116));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8161));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8164));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8169));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8170));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8171));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8172));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8173));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8174));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8174));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8175));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8176));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8177));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8179));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8180));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8200));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8230));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8231));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8232));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(8273));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7514), new DateTime(2025, 8, 31, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7513), "d1818e9d-0bb6-4bb2-86f5-781ac6041331" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7593), new DateTime(2025, 8, 31, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7593), "012b1ad1-9ec1-4cf7-a31b-28a2fbded507" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7602), new DateTime(2025, 8, 31, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7601), "97f119a8-a72b-4f2d-a42b-8787e70b8c93" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7609), new DateTime(2025, 8, 31, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7609), "897e96fb-7db2-48e8-853d-e4af69be7b11" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7636), new DateTime(2025, 8, 31, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(7636), "f732060d-1d99-4928-b83d-72339b7b5706" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 31, 20, 53, 22, 306, DateTimeKind.Utc).AddTicks(6314), "$2a$11$LiSYqQQSarXcQPQr7Pa.oe3ErP0TGNiv0qX7jPpZ/0ZyErYb8.Z4S" });

            migrationBuilder.CreateIndex(
                name: "IX_Rings_TribuneId_Number",
                table: "Rings",
                columns: new[] { "TribuneId", "Number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Sectors_RingId_Code",
                table: "Sectors",
                columns: new[] { "RingId", "Code" },
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
                name: "IX_Tribunes_Code",
                table: "Tribunes",
                column: "Code",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StadiumSeatsNew");

            migrationBuilder.DropTable(
                name: "Sectors");

            migrationBuilder.DropTable(
                name: "Rings");

            migrationBuilder.DropTable(
                name: "Tribunes");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6164));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6177));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6179));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6180));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6181));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6183));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6184));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6185));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6500), new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6934));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6937));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6937));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6939));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6940));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6943));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6947));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6950));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6952));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6953));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6954));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6955));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6955));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6956));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6957));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6959));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6961));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6961));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6963));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6965));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6966));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6966));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6967));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6968));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6971));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6973));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6973));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6974));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6977));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6978));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6979));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6979));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6982));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6983));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6986));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6988));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7061));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7062));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7064));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7065));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7066));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7066));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7067));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7068));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7069));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7070));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7071));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7071));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7072));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7073));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7073));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7074));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7075));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7116));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7160));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7161));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7164));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7167));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7169));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7170));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7170));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7171));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7172));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7173));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7173));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7174));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7175));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7175));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7230));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7231));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7231));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7233));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7234));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7234));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7235));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7236));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7237));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7238));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7238));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7239));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7241));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7241));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7304));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7305));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7311));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7419));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7422));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6683), new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6682), "4ee0dced-f9a1-46fd-b4a1-e073ebcf0cdf" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6771), new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6770), "e464fe6d-b2c4-4e77-96c1-33707b52120b" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6779), new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6779), "c2838b7d-fc76-4c94-bd39-a2eeaf432f6b" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6786), new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6785), "e0c5a531-7e4b-4670-ad35-096bb82fb717" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6801), new DateTime(2025, 8, 30, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(6800), "e8e85cc3-7487-4b6c-8d06-80d17a570dd3" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 8, 30, 0, 18, 38, 962, DateTimeKind.Utc).AddTicks(5224), "$2a$11$cnFDUR1doXuho784NiwgGO3WZoZ.J2VDMcRXXLoOlv6S0GvOQyifu" });
        }
    }
}
