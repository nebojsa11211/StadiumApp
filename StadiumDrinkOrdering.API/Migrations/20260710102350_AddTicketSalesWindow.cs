using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketSalesWindow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Optional ticket-sales window. Both nullable so legacy and externally-ingested events
            // without a configured window keep selling whenever their status allows. A null start
            // means sales open as soon as the event is on sale; a null end means they stay open.
            migrationBuilder.AddColumn<DateTime>(
                name: "TicketSalesStartDate",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TicketSalesEndDate",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketSalesStartDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "TicketSalesEndDate",
                table: "Events");
        }
    }
}
