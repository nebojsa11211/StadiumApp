using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddInPreparationStepTracking : Migration
    {
        // Records who moved an order into preparation and when, mirroring the existing
        // Accepted/Prepared/Delivered step-tracking columns. The scaffolded migration also emitted
        // ~5800 lines of seed-timestamp UpdateData churn (re-stamping every seeded Drink/Seat/User
        // row with the scaffold time); that noise was hand-removed — only the real schema changes
        // remain. See the [[ef-migration-seed-churn]] memory.
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<System.DateTime>(
                name: "InPreparationAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InPreparationByUserId",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_InPreparationByUserId",
                table: "Orders",
                column: "InPreparationByUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_InPreparationByUserId",
                table: "Orders",
                column: "InPreparationByUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_InPreparationByUserId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_InPreparationByUserId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InPreparationAt",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "InPreparationByUserId",
                table: "Orders");
        }
    }
}
