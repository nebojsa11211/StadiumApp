using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSeasonTicketPriceToSectorOverlay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "SeasonTicketPrice",
                table: "StadiumSectorOverlays",
                type: "numeric",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(631));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(638));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(640));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(641));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(642));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(643));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(644));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(645));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(767));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1160));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1161));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1161));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1161));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1164));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1164));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1167));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1167));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1167));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1200));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1200));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1251));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1252));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1254));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1254));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1257));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1257));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1259));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1259));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1261));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1261));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1262));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1262));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1262));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1263));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1263));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1263));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1264));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1264));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1304));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1311));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1311));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1460));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(925), new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1011), "f4858443-6b32-452a-9c7e-929a7299871f" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1072), new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1091), "4f3e03b2-9d46-4161-8c80-892104c0795c" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1092), new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1097), "465d0042-ddfd-4579-8edf-4f1cb079810e" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1098), new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1103), "87b50458-8bb2-40a2-ba6b-134825754ffe" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1104), new DateTime(2026, 7, 8, 9, 3, 54, 848, DateTimeKind.Utc).AddTicks(1108), "8c61c361-e272-412b-b61f-ac4bef59c9cc" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 8, 9, 3, 54, 847, DateTimeKind.Utc).AddTicks(9655), "$2a$11$Qla1KMEzy.pYCyWeLftyMOBcLYttijBtbZq1l/rpIh69y.gjEOihi" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeasonTicketPrice",
                table: "StadiumSectorOverlays");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1532));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1540));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1541));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1543));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1545));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1547));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1548));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2140));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2179));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2179));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2180));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2330));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2330));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2330));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2331));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2332));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2333));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2383));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2383));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2384));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2384));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2385));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2385));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2386));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2386));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2387));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2387));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2387));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2388));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2388));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2389));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2390));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2390));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2391));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2391));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2392));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2392));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2392));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2393));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2393));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2394));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2395));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2395));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2397));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2397));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2399));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2399));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2400));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2401));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2402));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2402));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2406));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2408));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2466));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1831), new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(1930), "8c402cd8-4bdd-45c0-9c69-b50aa5b247ad" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2063), new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2076), "e59e6a06-4104-4655-ba69-9ac031adac0f" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2076), new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2082), "68f3773e-79f2-4312-9cb0-8528dfb9a75e" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2082), new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2087), "90efd30d-40ae-4ea3-9348-b676faace7f0" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2088), new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(2093), "b297197d-98e5-423a-9e5c-318adf86acb9" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 8, 8, 24, 4, 646, DateTimeKind.Utc).AddTicks(503), "$2a$11$w8/1DHRIURrgWxQ3bSo.jefkqJud.tuyHWbQyCuc8XsGVf7r5mAdW" });
        }
    }
}
