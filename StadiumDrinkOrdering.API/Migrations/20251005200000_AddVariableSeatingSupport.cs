using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVariableSeatingSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseVariableSeating",
                table: "StadiumSectorOverlays",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "VariableSeatingData",
                table: "StadiumSectorOverlays",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(2997));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3011));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3013));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3015));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3016));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3018));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3019));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3021));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3580));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3584));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3587));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3590));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3592));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3594));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3595));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3596));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3597));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3598));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3601));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3602));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3603));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3604));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3605));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3607));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3609));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3610));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3610));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3611));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3613));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3614));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3615));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3615));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3616));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3617));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3618));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3620));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3623));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3676));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3677));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3678));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3679));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3680));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3681));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3682));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3683));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3684));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3685));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3686));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3687));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3688));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3689));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3690));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3691));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3692));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3693));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3693));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3694));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3695));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3696));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3697));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3780));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3785));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3785));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3786));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3794));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3800));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3817));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3818));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3830));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3832));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3836));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3837));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3848));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3892));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3897));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3900));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3904));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3906));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3913));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3914));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3915));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3917));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3918));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3921));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3921));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3924));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3926));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3927));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3928));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3956));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3959));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3961));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3963));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3965));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3966));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3967));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3968));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3971));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3973));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3974));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3977));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3978));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3979));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3980));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3981));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3982));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3983));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3984));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3986));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3988));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3990));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3991));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3992));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4000));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4116));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4140));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(4158));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3275), new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3370), "2f8acde1-0633-4162-9715-cddccf22d29e" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3372), new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3378), "2037a55d-8886-4a71-b5f4-953499032a55" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3379), new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3384), "403f4862-3869-4749-b23c-f2a4db367347" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3385), new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3389), "462b43b6-26cb-4975-8c5d-aa65deb14c68" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3390), new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(3394), "e589d781-ec06-4408-8398-ee402a2f1174" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 5, 19, 59, 57, 259, DateTimeKind.Utc).AddTicks(2120), "$2a$11$rVz6qXy2tgPIqSWV7lwqAueqxV33UnqFAHdPUZ.mK4ovqra3B1Snm" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseVariableSeating",
                table: "StadiumSectorOverlays");

            migrationBuilder.DropColumn(
                name: "VariableSeatingData",
                table: "StadiumSectorOverlays");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3035));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3053));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3056));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3059));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3061));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3063));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3065));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3067));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5830));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5832));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5848));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5852));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5856));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5985));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5986));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5987));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5988));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5989));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5990));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5991));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5992));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5993));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5994));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5995));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5996));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5998));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(5999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6000));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6077));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6079));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6079));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6080));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6081));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6082));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6083));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6084));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6085));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6086));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6087));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6087));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6088));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6089));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6091));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6092));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6093));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6094));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6095));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6096));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6097));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6097));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6098));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6099));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6100));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6102));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6105));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6106));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6116));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6131));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6230));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6232));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6233));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6234));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6235));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6236));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6237));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6238));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6238));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6239));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6240));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6241));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6242));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6243));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6244));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6245));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6246));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6246));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6247));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6248));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6249));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6250));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6251));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6252));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6254));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6257));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6259));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6261));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6262));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6263));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6264));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6373));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6374));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6375));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6376));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6377));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6380));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6419));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6422));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6466));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6467));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6469));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6470));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6489));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6490));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6491));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6491));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6529));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6529));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6530));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6531));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6532));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6533));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6534));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(6535));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3666), new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3778), "9852a6a9-b6e1-46b7-ad3e-66bf6bab2419" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3785), new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3794), "371a2d01-1952-42e1-80e9-a1de89d57c96" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3796), new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3802), "a2e21cb8-6ee2-4102-ac47-d91919277336" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3803), new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3809), "a5d2e9ec-9e8e-4cd7-b476-2e007fed9d15" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3811), new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(3827), "5e2457ef-9c21-4d30-8380-f0d1297dafb6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 5, 18, 47, 50, 588, DateTimeKind.Utc).AddTicks(2171), "$2a$11$8TfFxI77RePdFISj7.mjquYdljsNJdyABlOA4ngI8wAEAblIA6NtW" });
        }
    }
}
