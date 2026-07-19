using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Adds the event poster: display metadata on Events, and the image bytes in their own
    /// EventPosters table so listing events never loads them.
    ///
    /// The scaffolded version of this migration also re-stamped every seeded CreatedAt/PasswordHash
    /// row (~5800 lines of noise from the seed data's DateTime.UtcNow defaults). Those UpdateData
    /// calls are deliberately removed — they are not part of this schema change.
    /// </summary>
    public partial class AddEventPoster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PosterContentType",
                table: "Events",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PosterHeight",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PosterPrompt",
                table: "Events",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PosterWidth",
                table: "Events",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "EventPosters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EventId = table.Column<int>(type: "integer", nullable: false),
                    ImageData = table.Column<byte[]>(type: "bytea", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventPosters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EventPosters_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Unique: an event has at most one poster.
            migrationBuilder.CreateIndex(
                name: "IX_EventPosters_EventId",
                table: "EventPosters",
                column: "EventId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventPosters");

            migrationBuilder.DropColumn(
                name: "PosterContentType",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PosterHeight",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PosterPrompt",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "PosterWidth",
                table: "Events");
        }
    }
}
