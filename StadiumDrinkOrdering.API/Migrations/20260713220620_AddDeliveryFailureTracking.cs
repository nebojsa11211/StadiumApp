using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddDeliveryFailureTracking : Migration
    {
        // Delivery-exception tracking on Orders: a runner who can't hand an order over at the seat
        // records the attempt (reason/time/runner) and the order parks as DeliveryFailed (OrderStatus=8)
        // for Bar triage. Only the four real column additions are kept — the scaffolder's seed-timestamp
        // UpdateData churn (~5800 lines) is intentionally trimmed (see ef-migration-seed-churn convention).
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DeliveryAttempts",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<System.DateTime>(
                name: "LastDeliveryAttemptAt",
                table: "Orders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastDeliveryAttemptByUserId",
                table: "Orders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastDeliveryFailureReason",
                table: "Orders",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "DeliveryAttempts", table: "Orders");
            migrationBuilder.DropColumn(name: "LastDeliveryAttemptAt", table: "Orders");
            migrationBuilder.DropColumn(name: "LastDeliveryAttemptByUserId", table: "Orders");
            migrationBuilder.DropColumn(name: "LastDeliveryFailureReason", table: "Orders");
        }
    }
}
