using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOverlaySeatOwnership : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UniqueCode",
                table: "StadiumSeatsNew",
                type: "character varying(60)",
                maxLength: 60,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "SectorId",
                table: "StadiumSeatsNew",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "StadiumSectorOverlayId",
                table: "StadiumSeatsNew",
                type: "integer",
                nullable: true);

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
                columns: new[] { "CreatedAt", "Email", "PasswordHash", "Username" },
                values: new object[] { new DateTime(2026, 7, 1, 12, 57, 43, 432, DateTimeKind.Utc).AddTicks(3337), "nebojsa.medancic+adminStadion@gmail.com", "$2a$11$0KMsfK01wDzqGgI7MActvev4cUZB6QNjNLMGsy73H94aOVN1C4mBi", "nebojsa.medancic+adminStadion@gmail.com" });

            migrationBuilder.CreateIndex(
                name: "IX_StadiumSeatsNew_StadiumSectorOverlayId_RowNumber_SeatNumber",
                table: "StadiumSeatsNew",
                columns: new[] { "StadiumSectorOverlayId", "RowNumber", "SeatNumber" });

            migrationBuilder.AddForeignKey(
                name: "FK_StadiumSeatsNew_StadiumSectorOverlays_StadiumSectorOverlayId",
                table: "StadiumSeatsNew",
                column: "StadiumSectorOverlayId",
                principalTable: "StadiumSectorOverlays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StadiumSeatsNew_StadiumSectorOverlays_StadiumSectorOverlayId",
                table: "StadiumSeatsNew");

            migrationBuilder.DropIndex(
                name: "IX_StadiumSeatsNew_StadiumSectorOverlayId_RowNumber_SeatNumber",
                table: "StadiumSeatsNew");

            migrationBuilder.DropColumn(
                name: "StadiumSectorOverlayId",
                table: "StadiumSeatsNew");

            migrationBuilder.AlterColumn<string>(
                name: "UniqueCode",
                table: "StadiumSeatsNew",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(60)",
                oldMaxLength: 60);

            migrationBuilder.AlterColumn<int>(
                name: "SectorId",
                table: "StadiumSeatsNew",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8502));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8531));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8534));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8537));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8539));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8541));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8545));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8550));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(8691), new DateTime(2026, 6, 30, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9408));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9409));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9410));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9411));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9412));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9467));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9469));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9470));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9584));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9590));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9594));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9595));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9597));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9598));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9599));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9601));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9602));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9603));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9604));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9606));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9607));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9609));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9611));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9613));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9615));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9616));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9617));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9743));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9751));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9758));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9759));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9760));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9761));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9763));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9765));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9766));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9767));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9768));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9770));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9772));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9773));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9775));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9777));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9785));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9786));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9906));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9913));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9914));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9921));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9924));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9927));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9928));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9929));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9930));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9932));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9990));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9991));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(1));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(3));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(4));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(5));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(6));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(8));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(9));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(10));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(11));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(13));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(14));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(15));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(17));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(18));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(19));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(21));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(22));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(23));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(25));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(26));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(27));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(29));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(35));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(37));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(38));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(39));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(40));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(42));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(43));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(44));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(46));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(47));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(48));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(49));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(51));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(52));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(55));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(56));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(57));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(58));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(59));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(60));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(62));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(63));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(64));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(65));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(67));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(68));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(69));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(70));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(71));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(73));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(74));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(76));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(77));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(79));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(80));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(81));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(82));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(84));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(85));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(87));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(88));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(89));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(91));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(92));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(93));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(95));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(96));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(97));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(98));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(160));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(164));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(167));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(169));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(170));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(171));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(172));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(173));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(175));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(176));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(177));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(180));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 6, 30, 17, 50, 8, 263, DateTimeKind.Utc).AddTicks(199));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9069), new DateTime(2026, 6, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9186), "16c14c38-2f2f-41c5-8b13-33b8aaa32437" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9189), new DateTime(2026, 6, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9200), "1d90a244-e213-44d2-933a-091bf7e63ad6" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9202), new DateTime(2026, 6, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9214), "ffd83a32-7f39-47b4-8b90-6527b1f03170" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9217), new DateTime(2026, 6, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9223), "1e958b4f-5ce6-4d5c-9394-6d5f082ed549" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9225), new DateTime(2026, 6, 30, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(9234), "216ed94e-5d9d-4366-9ea5-dd3b6a1f4c7c" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "Email", "PasswordHash", "Username" },
                values: new object[] { new DateTime(2026, 6, 30, 17, 50, 8, 262, DateTimeKind.Utc).AddTicks(6989), "admin@stadium.com", "$2a$11$SzyH/XhIQAZGH8TWLV6M4O95DCKoqJuK1fYr6.Z0TJpcrD0OTLJEC", "admin" });
        }
    }
}
