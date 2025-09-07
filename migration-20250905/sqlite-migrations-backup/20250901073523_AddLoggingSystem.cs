using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLoggingSystem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Timestamp = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Level = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Category = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Action = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    UserId = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UserEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UserRole = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    IPAddress = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    UserAgent = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    RequestPath = table.Column<string>(type: "TEXT", maxLength: 100, nullable: true),
                    HttpMethod = table.Column<string>(type: "TEXT", maxLength: 20, nullable: true),
                    Message = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true),
                    Details = table.Column<string>(type: "TEXT", nullable: true),
                    ExceptionType = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true),
                    StackTrace = table.Column<string>(type: "TEXT", nullable: true),
                    Source = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogEntries", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8006));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8035));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8037));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8038));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8040));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8041));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8043));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8044));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8301), new DateTime(2025, 9, 1, 19, 0, 0, 0, DateTimeKind.Local) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8671));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8684));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8690));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8692));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8695));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8696));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8697));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8763));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8765));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8766));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8767));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8768));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8769));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8770));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8770));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8772));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8773));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8775));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8780));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8785));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8786));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8906));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8947));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8950));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8952));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8952));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8954));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8954));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8955));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8956));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8957));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8959));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8961));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8963));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8965));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8966));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8966));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8967));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8968));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8971));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8973));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8974));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8977));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8978));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8979));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8982));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8983));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8986));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8986));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8988));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8990));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8990));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8991));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8992));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9000));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9061));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9062));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9064));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9065));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9066));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9067));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9068));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9068));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9069));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9070));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9071));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9072));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9072));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9073));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9074));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9075));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9076));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9077));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9077));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9078));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9079));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9080));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9080));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9081));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9082));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9083));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9084));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9085));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9086));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9140));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9160));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9161));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9164));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9167));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9169));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9170));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9171));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9172));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9173));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9174));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9175));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9176));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9177));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9177));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9179));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9180));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(9226));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8516), new DateTime(2025, 9, 1, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8515), "46e087a1-5cc4-454c-8d83-1fcc910f86b4" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8597), new DateTime(2025, 9, 1, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8596), "40f82cfb-5275-4965-860a-3e289e269741" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8606), new DateTime(2025, 9, 1, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8605), "7c4040b9-1cbc-42a7-a1fb-d9b5a1c7899c" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8618), new DateTime(2025, 9, 1, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8617), "9fd13088-8c53-4ea8-961f-b6c5c7adbf80" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8629), new DateTime(2025, 9, 1, 19, 0, 0, 0, DateTimeKind.Local), new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(8628), "0fd8fb68-e6e4-4560-a188-eaf0d6595f9f" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 1, 7, 35, 21, 270, DateTimeKind.Utc).AddTicks(7088), "$2a$11$LKadlQGPjk6fR2qqzJ8MlO0cJ8pW2QJfxBTY7n7ON5wK/09Wm60MS" });

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogEntries");

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
        }
    }
}
