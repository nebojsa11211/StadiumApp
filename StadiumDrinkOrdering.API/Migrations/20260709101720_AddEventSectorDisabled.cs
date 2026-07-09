using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEventSectorDisabled : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "EventSectorPrices",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2);

            migrationBuilder.AddColumn<bool>(
                name: "IsDisabled",
                table: "EventSectorPrices",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8789));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8794));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8796));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8797));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8798));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8799));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8800));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8802));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(8919), new DateTime(2026, 7, 9, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9261));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9330));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9331));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9331));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9332));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9332));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9332));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9333));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9373));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9373));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9374));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9374));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9375));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9375));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9376));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9376));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9377));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9378));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9380));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9380));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9383));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9383));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9383));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9384));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9385));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9386));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9386));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9387));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9388));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9388));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9388));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9389));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9389));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9390));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9390));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9391));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9392));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9392));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9393));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9394));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9394));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9395));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9395));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9397));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9397));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9399));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9399));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9400));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9400));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9401));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9489));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9489));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9490));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9490));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9490));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9491));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9491));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9492));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9493));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9494));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9494));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9495));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9495));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9496));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9496));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9544));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9545));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9545));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9546));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9546));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9547));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9547));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9548));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9549));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9580));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9580));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9667));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9127), new DateTime(2026, 7, 9, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9213), "51846ea1-5d0b-474e-96b9-e06318d8e6e1" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9214), new DateTime(2026, 7, 9, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9221), "99a70ea6-2283-4f44-a7f1-4d470c5b437d" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9222), new DateTime(2026, 7, 9, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9227), "3d96cf1f-ffbb-41c5-9506-0c3eb9746ade" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9228), new DateTime(2026, 7, 9, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9233), "d05016c0-4253-4ee4-9113-e483c4a557e7" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9234), new DateTime(2026, 7, 9, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(9239), "9b704ff7-7e79-479c-9d26-3cb7bac2c9a9" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 9, 10, 17, 19, 102, DateTimeKind.Utc).AddTicks(7591), "$2a$11$9s5lZTiu82yd9esut7yoYOh1cPBkzFhQ7buOc6bJTIzUSsP2ytfvy" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDisabled",
                table: "EventSectorPrices");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "EventSectorPrices",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric(10,2)",
                oldPrecision: 10,
                oldScale: 2,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7623));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7639));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7640));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7641));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7642));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7645));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7848));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(7850));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(8063), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(22));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(35));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(36));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(36));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(37));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(47));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(47));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(48));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(48));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(50));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(50));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(51));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(51));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(52));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(53));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(58));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(58));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(58));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(59));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(59));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(230));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(333));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(401));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(402));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(402));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(406));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(406));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(408));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(408));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(409));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(410));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(411));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(411));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(412));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(413));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(419));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(419));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(466));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(466));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(467));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(467));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(469));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(469));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(470));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(470));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(517));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(517));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(518));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(518));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(519));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(519));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(520));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(520));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(520));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(521));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(521));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(522));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(522));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(523));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(523));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(523));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(524));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(524));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(525));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(525));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(526));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(526));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(526));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(527));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(527));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(528));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(528));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(528));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(529));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 10, 30, 30, 38, DateTimeKind.Utc).AddTicks(529));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(8280), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9852), "57cdc301-db02-4e26-9d05-b850fb16d453" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9862), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9871), "c3707351-6c26-443d-a4ac-22f251a40618" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9873), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9878), "129fef8e-b164-4fad-978f-507bbb73573b" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9879), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9884), "7305df1a-d8b5-4c2c-8261-241531e984ad" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9885), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(9889), "b8177f6d-3861-4892-a96c-8e4be162ed69" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 8, 10, 30, 30, 37, DateTimeKind.Utc).AddTicks(5652), "$2a$11$WxbzQxi0gZq8LfaXMh4dGuCCZpHYe.Hy2Wy6dgH60fuht2LQq2sLK" });
        }
    }
}
