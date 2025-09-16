using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRateLimitingEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountLockouts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    LockoutStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LockoutEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    FailedAttemptCount = table.Column<int>(type: "integer", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountLockouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FailedAttempts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IPAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    Email = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    AttemptTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    AttemptType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    UserAgent = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Context = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FailedAttempts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IPBans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IPAddress = table.Column<string>(type: "character varying(45)", maxLength: 45, nullable: false),
                    BanStart = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    BanEnd = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    Reason = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ViolationCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IPBans", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_AccountLockouts_Email",
                table: "AccountLockouts",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_AccountLockouts_Email_IsActive",
                table: "AccountLockouts",
                columns: new[] { "Email", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_AccountLockouts_LockoutEnd",
                table: "AccountLockouts",
                column: "LockoutEnd");

            migrationBuilder.CreateIndex(
                name: "IX_FailedAttempts_AttemptTime",
                table: "FailedAttempts",
                column: "AttemptTime");

            migrationBuilder.CreateIndex(
                name: "IX_FailedAttempts_AttemptType_AttemptTime",
                table: "FailedAttempts",
                columns: new[] { "AttemptType", "AttemptTime" });

            migrationBuilder.CreateIndex(
                name: "IX_FailedAttempts_Email",
                table: "FailedAttempts",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_FailedAttempts_Email_AttemptTime",
                table: "FailedAttempts",
                columns: new[] { "Email", "AttemptTime" });

            migrationBuilder.CreateIndex(
                name: "IX_FailedAttempts_IPAddress",
                table: "FailedAttempts",
                column: "IPAddress");

            migrationBuilder.CreateIndex(
                name: "IX_FailedAttempts_IPAddress_AttemptTime",
                table: "FailedAttempts",
                columns: new[] { "IPAddress", "AttemptTime" });

            migrationBuilder.CreateIndex(
                name: "IX_IPBans_BanEnd",
                table: "IPBans",
                column: "BanEnd");

            migrationBuilder.CreateIndex(
                name: "IX_IPBans_IPAddress",
                table: "IPBans",
                column: "IPAddress");

            migrationBuilder.CreateIndex(
                name: "IX_IPBans_IPAddress_IsActive",
                table: "IPBans",
                columns: new[] { "IPAddress", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AccountLockouts");

            migrationBuilder.DropTable(
                name: "FailedAttempts");

            migrationBuilder.DropTable(
                name: "IPBans");

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5866));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5891));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5893));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5895));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5898));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5899));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5901));

            migrationBuilder.UpdateData(
                table: "Drinks",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5903));

            migrationBuilder.UpdateData(
                table: "Events",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate" },
                values: new object[] { new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6054), new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6773));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 8,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 9,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 10,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6777));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 11,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 12,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 13,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 14,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 15,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 16,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6792));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 17,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6793));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 18,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6795));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 19,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6796));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 20,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6797));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 21,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6798));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 22,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6799));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 23,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6801));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 24,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6802));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 25,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6803));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 26,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6804));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 27,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6805));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 28,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6806));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 29,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6807));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 30,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6808));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 31,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6809));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 32,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6810));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 33,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6811));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 34,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6812));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 35,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6813));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 36,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6814));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 37,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6815));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 38,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6816));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 39,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6819));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 40,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6820));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 41,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6821));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 42,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6822));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 43,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6823));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 44,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6824));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 45,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6825));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 46,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6827));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 47,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6828));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 48,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6829));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 49,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6830));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 50,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6831));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 51,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6832));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 52,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6833));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 53,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6834));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 54,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6835));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 55,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6837));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 56,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6838));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 57,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6839));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 58,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6840));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 59,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6841));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 60,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6842));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 61,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6843));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 62,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6844));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 63,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6846));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 64,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6847));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 65,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6848));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 66,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6849));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 67,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6850));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 68,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6851));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 69,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6852));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 70,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6853));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 71,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6909));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 72,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6911));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 73,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6912));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 74,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6913));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 75,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6914));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 76,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6915));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 77,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6916));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 78,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6917));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 79,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6918));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 80,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6919));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 81,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6920));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 82,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6922));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 83,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 84,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6923));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 85,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6924));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 86,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6926));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 87,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6927));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 88,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6928));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 89,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6929));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 90,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6930));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 91,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6931));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 92,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6931));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 93,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6933));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 94,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6933));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 95,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6935));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 96,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6936));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 97,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6937));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 98,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6938));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 99,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6939));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 100,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6940));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 101,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6941));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 102,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6942));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 103,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6943));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 104,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 105,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6944));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 106,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6946));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 107,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6948));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 108,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6949));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 109,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6951));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 110,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6952));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 111,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6953));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 112,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6953));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 113,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6954));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 114,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6955));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 115,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6957));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 116,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6958));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 117,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6959));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 118,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6960));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 119,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6961));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 120,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6962));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 121,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6963));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 122,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6964));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 123,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6965));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 124,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6966));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 125,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6967));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 126,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6968));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 127,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6969));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 128,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6970));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 129,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6971));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 130,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6972));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 131,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6974));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 132,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6975));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 133,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6976));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 134,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6977));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 135,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7026));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 136,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7027));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 137,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7029));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 138,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7030));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 139,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7031));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 140,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7032));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 141,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7033));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 142,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7034));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 143,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7035));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 144,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7036));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 145,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7037));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 146,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7039));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 147,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7040));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 148,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7041));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 149,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7042));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 150,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7043));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 151,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7044));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 152,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7045));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 153,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7046));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 154,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7047));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 155,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7048));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 156,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7049));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 157,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7051));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 158,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7052));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 159,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7053));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 160,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7054));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 161,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7055));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 162,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7056));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 163,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7057));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 164,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7058));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 165,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7059));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 166,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7060));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 167,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7331));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 168,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7332));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 169,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7334));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 170,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7335));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 171,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7336));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 172,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7337));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 173,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7338));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 174,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7339));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 175,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7340));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 176,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7342));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 177,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7343));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 178,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7344));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 179,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7345));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 180,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7346));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 181,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7348));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 182,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7349));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 183,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7350));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 184,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7351));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 185,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7352));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 186,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7353));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 187,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7354));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 188,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7355));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 189,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7357));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 190,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7358));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 191,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7359));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 192,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7360));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 193,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7361));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 194,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7362));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 195,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7363));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 196,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7364));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 197,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7366));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 198,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7367));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 199,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7368));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 200,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7369));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 201,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7370));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 202,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7371));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 203,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7372));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 204,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7373));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 205,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7375));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 206,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7376));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 207,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7378));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 208,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7379));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 209,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7381));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 210,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7382));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 211,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7383));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 212,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7384));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 213,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7385));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 214,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7386));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 215,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7387));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 216,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7436));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 217,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7437));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 218,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7439));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 219,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7440));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 220,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7441));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 221,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7442));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 222,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7443));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 223,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7444));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 224,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7445));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 225,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7446));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 226,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7447));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 227,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7449));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 228,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7450));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 229,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7451));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 230,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7452));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 231,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7453));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 232,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7454));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 233,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7456));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 234,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 235,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7457));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 236,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 237,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7459));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 238,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7460));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 239,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7462));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 240,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7463));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 241,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7464));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 242,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7465));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 243,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7466));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 244,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7467));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 245,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7468));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 246,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7469));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 247,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7470));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 248,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7471));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 249,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7472));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 250,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7473));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 251,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7475));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 252,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7476));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 253,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7477));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 254,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7478));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 255,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7479));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 256,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7480));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 257,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7481));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 258,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7482));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 259,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7483));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 260,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7484));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 261,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7486));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 262,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7487));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 263,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7526));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 264,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7528));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 265,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7529));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 266,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7530));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 267,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7531));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 268,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7532));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 269,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7533));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 270,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7534));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 271,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7535));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 272,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7537));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 273,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7538));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 274,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7539));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 275,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7540));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 276,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7541));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 277,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7542));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 278,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7543));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 279,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7544));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 280,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7545));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 281,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7546));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 282,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7548));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 283,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7549));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 284,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7550));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 285,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7551));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 286,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7552));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 287,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7553));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 288,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7554));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 289,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7556));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 290,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7557));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 291,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7558));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 292,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7559));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 293,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7560));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 294,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7561));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 295,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7562));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 296,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7564));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 297,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7565));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 298,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7566));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 299,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7567));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 300,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7568));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 301,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7569));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 302,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7570));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 303,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7571));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 304,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7572));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 305,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7573));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 306,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7575));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 307,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7576));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 308,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7577));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 309,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7578));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 310,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7580));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 311,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7581));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 312,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7582));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 313,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7583));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 314,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7584));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 315,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7585));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 316,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7586));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 317,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7587));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 318,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7588));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 319,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7589));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 320,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7591));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 321,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7592));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 322,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7593));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 323,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7594));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 324,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7595));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 325,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7596));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 326,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7597));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 327,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7598));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 328,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7599));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 329,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7600));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 330,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7601));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 331,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7602));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 332,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7603));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 333,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7604));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 334,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7606));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 335,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7607));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 336,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7608));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 337,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7609));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 338,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7610));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 339,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7611));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 340,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7612));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 341,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7720));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 342,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7723));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 343,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7724));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 344,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7725));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 345,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7726));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 346,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7727));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 347,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7728));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 348,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7729));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 349,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7730));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 350,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7731));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 351,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 352,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7732));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 353,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7733));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 354,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7734));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 355,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7745));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 356,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7746));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 357,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7747));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 358,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7748));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 359,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7749));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 360,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7750));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 361,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7751));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 362,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7752));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 363,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7753));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 364,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7754));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 365,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7755));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 366,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7756));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 367,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7757));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 368,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7758));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 369,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7759));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 370,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7760));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 371,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7761));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 372,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7762));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 373,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7763));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 374,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7764));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 375,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7765));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 376,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7766));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 377,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7767));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 378,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7768));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 379,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7769));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 380,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7770));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 381,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 382,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7771));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 383,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 384,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7773));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 385,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7774));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 386,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7775));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 387,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7776));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 388,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 389,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7778));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 390,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7779));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 391,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7781));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 392,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7782));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 393,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7783));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 394,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 395,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7784));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 396,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7785));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 397,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7786));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 398,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7787));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 399,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7788));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 400,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7789));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 401,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7790));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 402,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7791));

            migrationBuilder.UpdateData(
                table: "StadiumSeats",
                keyColumn: "Id",
                keyValue: 403,
                column: "CreatedAt",
                value: new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(7792));

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6348), new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6484), "adecc671-b14b-4168-8528-b3a60c5a6fec" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6487), new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6495), "959d776f-67cc-4781-9289-b6143a306892" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6496), new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6510), "5e1c6dc4-f3c5-4607-9fdc-6e5514791c3c" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6512), new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6518), "b74def6b-34e3-4569-bb64-aad300ad6282" });

            migrationBuilder.UpdateData(
                table: "Tickets",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedAt", "EventDate", "PurchaseDate", "QRCodeToken" },
                values: new object[] { new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6519), new DateTime(2025, 9, 6, 19, 0, 0, 0, DateTimeKind.Utc), new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(6525), "87cc1c21-931b-4cc8-b917-fe545444ddb8" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedAt", "PasswordHash" },
                values: new object[] { new DateTime(2025, 9, 6, 21, 17, 12, 304, DateTimeKind.Utc).AddTicks(5007), "$2a$11$nxtbrbLFzp4QlNEdXhI4XuSEXFM.Xcb2cee2iqe8qagj2VP4uQDFS" });
        }
    }
}
