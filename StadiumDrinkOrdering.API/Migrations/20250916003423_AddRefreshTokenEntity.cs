using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokenEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Token = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    JwtId = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsUsed = table.Column<bool>(type: "boolean", nullable: false),
                    IsRevoked = table.Column<bool>(type: "boolean", nullable: false),
                    InvalidatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    InvalidationReason = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    DeviceInfo = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    IpAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_ExpiresAt",
                table: "RefreshTokens",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_JwtId",
                table: "RefreshTokens",
                column: "JwtId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token",
                table: "RefreshTokens",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_Token_IsUsed_IsRevoked",
                table: "RefreshTokens",
                columns: new[] { "Token", "IsUsed", "IsRevoked" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId_IsUsed_IsRevoked",
                table: "RefreshTokens",
                columns: new[] { "UserId", "IsUsed", "IsRevoked" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3002));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3269));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3272));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3276));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3282));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3284));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3287));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3290));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3516), new DateTime(2025, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4011));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4061));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4062));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4063));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4065));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4238));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4240));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4241));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4243));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4244));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4245));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4247));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4248));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4249));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4250));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4251));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4253));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4254));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4255));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4256));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4258));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4261));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4262));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4263));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4265));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4266));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4268));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4269));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4270));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4272));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4273));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4274));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4275));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4277));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4279));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4280));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4281));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4283));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4284));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4285));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4286));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4288));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4289));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4291));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4292));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4293));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4295));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4296));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4297));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4299));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4300));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4301));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4302));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4305));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4307));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4308));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4309));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4310));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4312));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4313));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4314));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4315));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4316));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4318));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4319));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4320));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4321));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4323));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4324));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4325));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4327));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4329));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4405));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4406));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4407));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4409));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4410));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4411));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4413));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4414));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4415));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4416));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4418));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4419));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4420));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4421));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4422));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4424));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4426));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4428));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4430));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4431));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4432));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4433));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4435));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4438));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4448));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4455));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4458));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4461));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4467));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4470));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4474));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4485));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4488));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4489));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4490));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4549));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4555));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4563));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4574));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4579));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4582));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4587));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4590));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4592));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4594));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4595));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4596));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4597));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4599));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4601));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4602));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4604));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4605));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4607));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4609));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4610));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4613));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4614));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4616));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4617));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4619));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4621));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4622));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4624));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4625));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4626));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4628));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4629));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4630));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4632));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4633));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4634));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4636));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4637));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4638));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4639));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4641));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4642));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4643));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4645));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4646));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4647));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4649));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4650));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4651));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4653));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4656));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4657));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4659));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4660));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4661));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4663));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4664));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4716));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4718));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4719));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4721));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4736));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4737));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4738));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4740));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4741));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4742));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4744));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4753));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4756));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4757));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4759));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4760));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4762));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4763));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4765));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4766));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4768));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4769));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4770));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4772));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4830));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4837));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4851));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4852));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4855));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4856));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4857));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4859));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4860));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4862));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4864));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4865));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4866));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4869));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4870));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4871));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4873));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4875));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4878));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4879));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4881));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4882));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4883));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4884));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4886));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4887));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4889));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4890));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4891));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4893));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4894));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4895));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4896));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4898));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4899));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4901));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4902));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4903));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4905));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4906));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4907));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4908));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4910));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4913));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4914));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4915));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4917));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4918));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4921));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4926));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4927));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4930));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4931));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4933));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4934));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4935));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4936));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4938));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4939));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4940));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4997));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(4999));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5001));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5002));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5004));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5005));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5006));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5008));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5009));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5010));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5012));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5013));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5014));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5015));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5017));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5018));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5020));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5021));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5022));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5023));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5025));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5038));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(5046));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3749), new DateTime(2025, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3886), "bfb76006-ae72-43a3-953b-edc3dcc72d94" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3891), new DateTime(2025, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3899), "169cf0d9-bace-425e-beaa-cb0d2e76d700" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3901), new DateTime(2025, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3908), "11f0bb17-f6f4-4203-9217-f2aa3c4ce02f" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3913), new DateTime(2025, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3933), "f3e280d7-a00f-4a32-ac53-0dfa63ecbc0e" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3934), new DateTime(2025, 9, 15, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(3942), "37d41a08-c5e2-4029-bf9c-81789102e8a6" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 15, 23, 14, 11, 890, DateTimeKind.Utc).AddTicks(478), "$2a$11$Tww3rKFnOvbbpQ/JLl/cUefBlvM9kwqlZ4H/5voJttdtYpNaSdTka" });
        }
    }
}
