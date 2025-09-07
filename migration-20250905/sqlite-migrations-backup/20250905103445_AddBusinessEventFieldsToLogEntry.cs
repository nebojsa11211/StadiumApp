using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBusinessEventFieldsToLogEntry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BusinessEntityId",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessEntityName",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BusinessEntityType",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationInfo",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MetadataJson",
                table: "LogEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MonetaryAmount",
                table: "LogEntries",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "LogEntries",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedEntityId",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RelatedEntityType",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusAfter",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StatusBefore",
                table: "LogEntries",
                type: "TEXT",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(607));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(622));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(625));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(627));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(630));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(634));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(636));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(642));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1528));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1535));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1539));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1541));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1541));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1543));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1545));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1546));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1548));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1580));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1582));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1584));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1587));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1590));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1592));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1693));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1694));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1695));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1697));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1735));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1743));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1744));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1747));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1749));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1751));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1830));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1832));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1836));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1837));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1837));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1906));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1913));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1914));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1915));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1917));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1918));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1921));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1924));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1926));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2000));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2079));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2080));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2081));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2082));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2082));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2083));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2084));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2085));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2086));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2086));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2087));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2088));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2089));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2090));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2091));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2091));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2092));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2093));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2094));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2095));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2095));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2096));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2097));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2098));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2099));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2102));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2105));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2106));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2116));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(2135));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1116), new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1115), "da35122b-e4d6-475c-a7a9-5376486067d8" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1217), new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1217), "218719d9-8683-4339-94c9-e73f36d107ad" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1231), new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1230), "fdacd21d-5e71-40a3-ad56-d8d72df3dedb" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1441), new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1441), "89846e9d-b5cd-4c3e-a1c2-cbd28b2b8196" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1480), new DateTime(2025, 9, 5, 10, 34, 43, 973, DateTimeKind.Utc).AddTicks(1480), "b56df158-7ff2-47a9-bc6b-8ffe69dc883e" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 5, 10, 34, 43, 972, DateTimeKind.Utc).AddTicks(9572), "$2a$11$/BY8xT7eEjvM1F7FltHjwuLbwhA7qvgnhfJUha95hVU6z2FjKYQLG" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BusinessEntityId",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "BusinessEntityName",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "BusinessEntityType",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "LocationInfo",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "MetadataJson",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "MonetaryAmount",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "RelatedEntityId",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "RelatedEntityType",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "StatusAfter",
                table: "LogEntries");

            migrationBuilder.DropColumn(
                name: "StatusBefore",
                table: "LogEntries");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4279));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4301));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4303));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4304));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4305));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4306));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4308));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4309));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4832));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4836));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4837));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4848));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4906));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4913));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4915));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4917));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4918));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4921));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4924));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4926));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4927));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4928));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4930));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4931));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4932));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4932));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4933));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4934));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4935));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4936));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4936));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4937));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4938));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4939));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4939));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4940));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4971));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4973));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4974));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4977));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4978));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4978));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4979));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4982));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4983));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4986));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4988));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4990));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4991));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4992));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4992));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5000));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5085));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5087));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5087));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5088));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5089));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5090));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5091));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5092));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5092));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5093));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5094));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5094));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5095));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5096));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5097));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5098));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5098));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5099));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5102));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5102));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5105));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5106));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5106));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5116));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5140));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5230));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5230));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5231));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5232));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5233));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5234));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5235));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5236));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5236));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5237));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5238));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5239));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5304));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5305));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5311));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5322));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5326));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5328));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5330));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5331));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5331));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5332));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5333));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5373));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5374));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5375));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5376));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5377));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5377));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5378));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5380));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(5383));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4660), new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4659), "6963ac7d-ebdf-4852-bcbf-6ac2f2504984" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4743), new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4743), "baa27186-045e-467c-99ca-bfdf4e895263" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4751), new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4751), "31981b3b-98c7-451b-844c-59594d831fb8" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4758), new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4758), "fc0ef398-deeb-494d-9018-dabde86373fb" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4773), new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(4773), "b657a25f-ef71-47ef-b047-4dc6c8f9b729" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 5, 5, 55, 25, 421, DateTimeKind.Utc).AddTicks(3653), "$2a$11$vxUlCWYFsk.mGXJk3GkZsecjUpf73HM6ues8OeNvJ2QVnoTS8FIsi" });
        }
    }
}
