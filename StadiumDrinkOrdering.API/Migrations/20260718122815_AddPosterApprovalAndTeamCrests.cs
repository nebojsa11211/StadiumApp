using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Poster review state on the event (approval timestamp + the match details baked into the
    /// artwork, for staleness detection), and the reusable team-crest store that lets an opponent's
    /// crest be uploaded once and remembered by name.
    ///
    /// As with the earlier poster migrations, the scaffolded version re-stamped every seeded
    /// CreatedAt/PasswordHash row (~5800 lines of seed-timestamp churn); those UpdateData calls are
    /// deliberately removed.
    /// </summary>
    public partial class AddPosterApprovalAndTeamCrests : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PosterApprovedAt",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterSourceSignature",
                table: "Events",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "TeamCrests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NormalizedName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    DisplayName = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false),
                    ContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamCrests", x => x.Id);
                });

            // One crest per team name; the name is stored normalized so lookups are case-insensitive.
            migrationBuilder.CreateIndex(
                name: "IX_TeamCrests_NormalizedName",
                table: "TeamCrests",
                column: "NormalizedName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TeamCrests");

            migrationBuilder.DropColumn(
                name: "PosterApprovedAt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PosterSourceSignature",
                table: "Events");
        }
    }
}
