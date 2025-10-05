using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCustomShapeSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShapeType",
                table: "StadiumSectorOverlays");

            migrationBuilder.AddColumn<int>(
                name: "ShapeTypeEnum",
                table: "StadiumSectorOverlays",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "VertexCoordinatesJson",
                table: "StadiumSectorOverlays",
                type: "character varying(4000)",
                maxLength: 4000,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6485));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6494));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6496));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6497));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6499));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6504));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6505));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6506));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6587));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7003));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7016));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7024));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7028));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7050));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7101));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7102));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7103));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7104));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7105));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7106));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7107));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7108));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7109));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7110));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7111));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7112));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7113));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7114));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7115));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7116));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7117));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7118));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7119));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7120));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7121));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7122));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7123));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7124));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7125));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7126));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7127));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7128));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7129));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7130));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7132));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7133));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7134));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7135));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7136));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7137));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7138));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7139));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7140));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7141));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7142));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7143));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7144));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7145));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7146));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7147));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7148));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7149));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7150));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7151));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7152));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7153));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7154));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7181));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7182));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7183));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7184));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7185));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7186));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7187));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7188));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7189));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7190));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7191));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7192));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7193));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7194));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7195));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7196));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7197));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7198));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7199));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7200));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7201));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7202));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7203));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7204));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7205));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7206));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7207));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7208));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7209));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7210));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7211));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7212));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7213));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7215));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7216));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7217));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7218));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7219));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7220));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7221));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7222));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7223));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7224));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7225));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7226));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7227));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7228));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7229));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7230));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7231));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7231));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7232));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7233));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7234));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7235));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7235));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7236));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7237));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7238));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7239));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7240));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7241));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7242));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7243));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7331));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7332));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7333));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7341));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7347));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7356));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7365));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7423));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7425));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7427));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7429));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7434));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7466));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7467));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7469));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7470));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7519));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7520));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7521));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7522));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7522));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7523));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7524));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7525));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7526));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7527));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7527));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7528));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7529));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7530));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7531));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7531));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7532));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7533));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7534));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7534));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7535));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7536));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7537));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7538));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7538));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7539));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7540));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7541));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7541));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7542));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7543));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7544));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7544));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7545));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7546));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7547));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7548));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7548));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7549));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(7569));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6738), new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6810), "f532b16c-ddcf-4ecc-b281-ef094581f02c" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6812), new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6817), "09ef5389-259a-4a41-8be0-ea4a79de8cb7" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6818), new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6822), "8be36700-3f56-48aa-93ec-b195b4df3251" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6823), new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6827), "e01eca63-c2e3-485f-aff2-4a0a4ec9c975" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6828), new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(6959), "5ca85b22-e821-4a86-82f3-e149de9d59a1" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 10, 5, 21, 59, 4, 808, DateTimeKind.Utc).AddTicks(5800), "$2a$11$2aFRvyRBfQ1nO33id8kuCOWWtJ.m/9lmbYvqPp0G0npadv5CeygES" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShapeTypeEnum",
                table: "StadiumSectorOverlays");

            migrationBuilder.DropColumn(
                name: "VertexCoordinatesJson",
                table: "StadiumSectorOverlays");

            migrationBuilder.AddColumn<string>(
                name: "ShapeType",
                table: "StadiumSectorOverlays",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

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
    }
}
