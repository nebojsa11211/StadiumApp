using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketAndSeasonTicketOib : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerDocumentNumber",
                table: "Tickets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerOib",
                table: "Tickets",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HolderDocumentNumber",
                table: "SeasonTickets",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HolderOib",
                table: "SeasonTickets",
                type: "character varying(11)",
                maxLength: 11,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerDocumentNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CustomerOib",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "HolderDocumentNumber",
                table: "SeasonTickets");

            migrationBuilder.DropColumn(
                name: "HolderOib",
                table: "SeasonTickets");
        }
    }
}
