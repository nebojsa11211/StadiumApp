using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class ExpandLogEntryFieldLimits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3732));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3800));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3802));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3804));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3805));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3807));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3808));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3810));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(3909), new DateTime(2025, 9, 27, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4400));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4402));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4408));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4409));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4410));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4412));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4419));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4422));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4580));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4582));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4584));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4587));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4590));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4592));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4594));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4596));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4597));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4598));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4599));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4601));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4603));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4604));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4605));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4606));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4607));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4609));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4610));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4611));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4613));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4614));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4615));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4616));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4617));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4618));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4618));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4669));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4670));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4671));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4672));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4674));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4675));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4675));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4676));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4677));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4678));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4679));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4679));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4680));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4681));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4682));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4684));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4685));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4686));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4688));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4690));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4691));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4692));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4692));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4693));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4694));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4695));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4696));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4697));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4780));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4786));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4906));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4913));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4914));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4915));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4915));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4917));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4918));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5061));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5062));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(5063));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4130), new DateTime(2025, 9, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4209), "f78e41ca-20d4-48a0-80cb-84992434df51" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4210), new DateTime(2025, 9, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4216), "6211547f-2f8f-4d93-bc7c-fc14cef79af4" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4217), new DateTime(2025, 9, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4232), "9a1d0bd2-72c7-4592-a17e-705b78802368" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4233), new DateTime(2025, 9, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4347), "07da16d3-f28b-4a77-95b4-c419e45ea15e" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4348), new DateTime(2025, 9, 27, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(4352), "70503dd0-11f7-488c-9ad0-bfa1b4ba2501" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 27, 20, 1, 33, 786, DateTimeKind.Utc).AddTicks(2698), "$2a$11$no5hZ9tO2yZd2nuA7MrtCOb5Y3YM8vTqLKizURdAdmWDFdZlF20o." });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9108));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9116));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9118));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9119));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9120));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9121));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9123));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9124));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9200), new DateTime(2025, 9, 16, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9500));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9638));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9641));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9641));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9648));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9650));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9652));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9653));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9654));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9655));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9656));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9657));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9658));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9658));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9659));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9660));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9661));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9661));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9662));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9663));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9663));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9664));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9665));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9666));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9666));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9667));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9668));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9669));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9670));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9671));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9671));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9672));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9673));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9674));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9735));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9739));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9743));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9744));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9747));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9749));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9751));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9753));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9756));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9830));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9832));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9836));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9885));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9888));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(57));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(59));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(59));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(60));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(61));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(62));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(62));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(63));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(63));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(64));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(65));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(65));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(66));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(67));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(67));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(68));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(69));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(69));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(70));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(71));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(71));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(72));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(73));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(73));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(74));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(75));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(76));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(76));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(77));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(78));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(78));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(79));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(80));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(81));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(81));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(82));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(82));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(83));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(84));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(84));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(85));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(86));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(87));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(88));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(89));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(90));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(90));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(91));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(92));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(92));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(93));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(94));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(94));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(95));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(96));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(96));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(97));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(98));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(99));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(99));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(102));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(105));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(105));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(106));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(200));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 16, 0, 34, 22, 319, DateTimeKind.Utc).AddTicks(223));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9383), new DateTime(2025, 9, 16, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9450), "0c54b01f-9300-4347-b583-74df19183b37" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9452), new DateTime(2025, 9, 16, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9457), "0f12adc2-d120-4c2c-aef0-90724e9473cd" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9458), new DateTime(2025, 9, 16, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9462), "a3cf1a56-c81e-4554-b41b-2c6137a0d4b2" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9463), new DateTime(2025, 9, 16, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9475), "fa85fe79-d626-46a2-b414-8b1f2ecec44c" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9476), new DateTime(2025, 9, 16, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(9480), "b3aebd75-ec5b-483c-b352-d1e3ae5ad1f7" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 16, 0, 34, 22, 318, DateTimeKind.Utc).AddTicks(8420), "$2a$11$axCI4IFViZt0pg8kg3JkbO8x0KWbuuEU1SRBQnI1KGjQOQXLv2tN6" });
        }
    }
}
