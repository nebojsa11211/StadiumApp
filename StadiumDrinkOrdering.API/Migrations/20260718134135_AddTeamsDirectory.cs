using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Replaces the name-keyed <c>TeamCrests</c> cache with a first-class <c>Teams</c> directory
    /// that an admin can manage in its own right, and links events to the club/team identities
    /// behind their <c>HomeTeam</c>/<c>AwayTeam</c> name columns.
    ///
    /// Hand-trimmed to the real schema operations: the scaffolder emits ~2,900 further lines of
    /// <c>UpdateData</c> re-stamping seed <c>CreatedAt</c> values and the seeded password hash,
    /// which are noise on every scaffold and are deliberately not applied.
    /// </summary>
    public partial class AddTeamsDirectory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Teams",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ShortName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    LogoContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    PrimaryColor = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    SecondaryColor = table.Column<string>(type: "character varying(16)", maxLength: 16, nullable: true),
                    FoundedYear = table.Column<int>(type: "integer", nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Website = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Teams", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Teams_NormalizedName",
                table: "Teams",
                column: "NormalizedName",
                unique: true);

            // Carry every remembered crest across before the old table goes: these were uploaded by
            // hand, one opponent at a time, and are not recoverable from anywhere else.
            migrationBuilder.Sql(@"
                INSERT INTO ""Teams"" (""Name"", ""NormalizedName"", ""Logo"", ""LogoContentType"", ""CreatedAt"", ""UpdatedAt"")
                SELECT ""DisplayName"", ""NormalizedName"", ""ImageData"", ""ContentType"", ""CreatedAt"", ""UpdatedAt""
                FROM ""TeamCrests"";
            ");

            migrationBuilder.DropTable(
                name: "TeamCrests");

            migrationBuilder.AddColumn<int>(
                name: "HomeClubId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AwayTeamId",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Events_AwayTeamId",
                table: "Events",
                column: "AwayTeamId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_HomeClubId",
                table: "Events",
                column: "HomeClubId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Clubs_HomeClubId",
                table: "Events",
                column: "HomeClubId",
                principalTable: "Clubs",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Events_Teams_AwayTeamId",
                table: "Events",
                column: "AwayTeamId",
                principalTable: "Teams",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            // Backfill the links for events that already exist, matching on the same normalized
            // (trimmed, lower-cased) name the application uses.
            migrationBuilder.Sql(@"
                UPDATE ""Events"" e
                SET ""HomeClubId"" = c.""Id""
                FROM ""Clubs"" c
                WHERE e.""HomeTeam"" IS NOT NULL
                  AND lower(btrim(e.""HomeTeam"")) = lower(btrim(c.""Name""));
            ");

            migrationBuilder.Sql(@"
                UPDATE ""Events"" e
                SET ""AwayTeamId"" = t.""Id""
                FROM ""Teams"" t
                WHERE e.""AwayTeam"" IS NOT NULL
                  AND lower(btrim(e.""AwayTeam"")) = t.""NormalizedName"";
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_Clubs_HomeClubId",
                table: "Events");

            migrationBuilder.DropForeignKey(
                name: "FK_Events_Teams_AwayTeamId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_AwayTeamId",
                table: "Events");

            migrationBuilder.DropIndex(
                name: "IX_Events_HomeClubId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "AwayTeamId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "HomeClubId",
                table: "Events");

            migrationBuilder.CreateTable(
                name: "TeamCrests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false),
                    NormalizedName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamCrests", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TeamCrests_NormalizedName",
                table: "TeamCrests",
                column: "NormalizedName",
                unique: true);

            // Return the crests to the old cache. Teams that never had one are dropped along with
            // the rest of the directory — the old table had no way to represent them.
            migrationBuilder.Sql(@"
                INSERT INTO ""TeamCrests"" (""DisplayName"", ""NormalizedName"", ""ImageData"", ""ContentType"", ""CreatedAt"", ""UpdatedAt"")
                SELECT ""Name"", ""NormalizedName"", ""Logo"", COALESCE(""LogoContentType"", 'image/png'), ""CreatedAt"", ""UpdatedAt""
                FROM ""Teams""
                WHERE ""Logo"" IS NOT NULL;
            ");

            migrationBuilder.DropTable(
                name: "Teams");
        }
    }
}
