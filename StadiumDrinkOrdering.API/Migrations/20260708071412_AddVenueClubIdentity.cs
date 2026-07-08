using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueClubIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ClubLogo",
                table: "Venues",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClubLogoContentType",
                table: "Venues",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClubName",
                table: "Venues",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2152));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2184));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2185));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2187));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2188));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2189));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2191));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2192));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2390), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3244));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3257));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3262));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3263));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3263));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3264));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3380));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3389));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3389));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3390));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3391));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3391));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3392));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3393));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3394));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3394));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3395));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3395));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3396));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3397));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3397));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3398));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3399));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3399));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3400));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3400));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3400));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3401));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3401));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3402));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3402));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3403));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3404));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3406));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3406));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3408));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3408));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3409));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3410));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3412));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3412));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3413));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3413));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3417));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3419));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3422));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3422));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3422));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3489));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3489));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3490));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3490));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3491));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3491));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3492));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3492));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3493));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3494));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3494));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3495));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3495));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3496));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3496));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3497));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3497));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3498));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3498));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3499));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3500));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3500));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3501));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3501));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3502));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3502));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3503));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3503));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3504));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3504));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3505));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3505));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3506));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3506));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3507));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3507));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3508));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3509));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3509));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3510));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3510));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3547));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3548));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3549));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3620));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3623));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3623));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3627));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3631));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3635));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3638));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3638));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3640));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3644));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3648));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3648));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3650));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3650));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3652));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3652));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3653));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3653));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3697));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3698));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3699));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3700));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3701));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3702));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3703));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3704));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3705));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3706));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3707));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3708));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3709));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3710));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3711));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3712));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3713));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3714));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3715));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3717));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3722));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3732));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2754), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2991), "a8464c5f-79ec-4fcc-903d-f2c49377367d" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(2993), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3010), "ce6402e3-161c-4755-80a6-d47b66a2d5e2" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3011), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3017), "b5b89f71-6a13-4620-bfdf-7b96956db038" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3177), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3183), "9f17317f-b17c-4cd8-a282-a9c280467cb4" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3184), new DateTime(2026, 7, 8, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 8, 7, 14, 10, 720, DateTimeKind.Utc).AddTicks(3189), "ba03f1c9-e91c-4a56-b5eb-347fa2603339" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 8, 7, 14, 10, 719, DateTimeKind.Utc).AddTicks(9704), "$2a$11$HGSvECutMbYl7ENT7c93Ee5mvNnhQ7RKJxpSQwtvat7TZmKWljWHS" });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ClubLogo", "ClubLogoContentType", "ClubName" },
                values: new object[] { null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClubLogo",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "ClubLogoContentType",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "ClubName",
                table: "Venues");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(1905));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(1912));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(1914));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(1916));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(1917));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2025));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2028));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2029));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2151), new DateTime(2026, 7, 7, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2735));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2743));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2747));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2749));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2751));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2756));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2757));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2826));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2830));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2836));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2837));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2845));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2848));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2851));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2852));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2854));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2856));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2858));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2861));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2863));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2867));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2872));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2877));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2880));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2921));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2924));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2926));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2927));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2929));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2930));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2931));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2931));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2932));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2933));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2934));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2935));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2935));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2936));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2938));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2938));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2939));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2940));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2943));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2945));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2947));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2950));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2953));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2954));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2955));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2956));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2957));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2957));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2959));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2961));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2963));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2965));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2966));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2967));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2967));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2968));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2971));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2973));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2974));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3007));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3019));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3061));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3062));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3064));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3064));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3065));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3066));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3067));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3067));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3068));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3155));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3156));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3157));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3158));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3159));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3160));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3161));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3162));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3163));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3164));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3165));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3166));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3167));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3168));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3170));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3171));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3172));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3172));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3173));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3174));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3175));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3176));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3176));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3177));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3178));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3179));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3180));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3246));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3247));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3248));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3249));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3250));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3250));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3251));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3252));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3254));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3257));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3259));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3260));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3261));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3262));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3264));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3267));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3271));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3276));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3278));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3282));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3287));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3290));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3294));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3298));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3303));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3304));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3305));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3306));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3311));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3317));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3373));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3374));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3374));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3375));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3376));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3377));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3378));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3380));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(3382));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2420), new DateTime(2026, 7, 7, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2633), "126a0a94-03bd-4601-9f0b-2542f1904c9f" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2638), new DateTime(2026, 7, 7, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2649), "5f4567bc-f7d3-4b21-a185-0fad93487dd7" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2651), new DateTime(2026, 7, 7, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2655), "8319c482-2dc7-401a-abd8-25f28171b27d" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2656), new DateTime(2026, 7, 7, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2661), "058cfc01-7958-4386-b928-fde78e307509" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2662), new DateTime(2026, 7, 7, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(2679), "c9e01056-2203-4266-9e80-263b86941cfc" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2026, 7, 7, 22, 34, 14, 744, DateTimeKind.Utc).AddTicks(880), "$2a$11$4BfVnkmGIo1msLN2GhaMO.HniJsXsmBFG5S5ezra7hlhjiq7Ki7ym" });
        }
    }
}
