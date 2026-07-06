using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddSectorPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Explicit per-sector ticket price. Nullable so existing sectors keep pricing by the
            // event base price × sector-type multiplier; when set, every seat in the sector is
            // sold at exactly this price.
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "StadiumSectorOverlays",
                type: "numeric",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "StadiumSectorOverlays");
        }
    }
}
