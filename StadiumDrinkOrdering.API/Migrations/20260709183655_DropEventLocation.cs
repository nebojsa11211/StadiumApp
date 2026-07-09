using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class DropEventLocation : Migration
    {
        // Events no longer carry a per-event location: every event is held at the one configured
        // venue, so the location shown to users is derived from Venue Settings. This drops the now
        // unused Events.Location column. (Seed-timestamp UpdateData churn from the scaffolder has
        // been trimmed; only the real schema change is kept.)

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location",
                table: "Events");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Events",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }
    }
}
