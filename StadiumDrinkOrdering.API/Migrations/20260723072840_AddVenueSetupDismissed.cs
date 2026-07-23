using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueSetupDismissed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "SetupDismissed",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SetupDismissed",
                table: "Venues");
        }
    }
}
