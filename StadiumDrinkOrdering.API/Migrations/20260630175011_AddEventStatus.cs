using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddEventStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Authoritative lifecycle state. Default 1 = EventStatus.Planned.
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "Events",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            // Backfill existing rows from the previously-implicit lifecycle signals
            // (EventDate + IsActive) so historical data lands in a sensible status:
            //   - past events            -> Completed (6)
            //   - published, future      -> OnSale    (2)
            //   - unpublished, future    -> Planned   (1, the column default)
            migrationBuilder.Sql(
                "UPDATE \"Events\" SET \"Status\" = 6 WHERE \"EventDate\" < NOW();");
            migrationBuilder.Sql(
                "UPDATE \"Events\" SET \"Status\" = 2 WHERE \"EventDate\" >= NOW() AND \"IsActive\" = true;");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "Events");
        }
    }
}
