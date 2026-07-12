using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class SeasonTicketUniquePerUserSeason : Migration
    {
        // Enforces "a customer holds at most one season ticket per season" at the DB level.
        // Scaffolded UpdateData seed-timestamp churn was hand-trimmed away, leaving only the real op.
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SeasonTickets_UserId_SeasonId",
                table: "SeasonTickets",
                columns: new[] { "UserId", "SeasonId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SeasonTickets_UserId_SeasonId",
                table: "SeasonTickets");
        }
    }
}
