using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using StadiumDrinkOrdering.API.Data;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    // Attributes are placed here (rather than in a separate .Designer.cs) so EF still discovers the
    // migration. Hand-written to avoid the seed-timestamp churn a scaffold would produce, since the
    // Event seed uses DateTime.UtcNow. The columns are simple additive, nullable timestamps.
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260712130000_AddDrinkSalesWindow")]
    public partial class AddDrinkSalesWindow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Optional drink-ordering window. Both nullable so legacy and externally-ingested events
            // without a configured window keep ordering open for the whole live phase. A null start
            // means ordering opens as soon as the event goes live; a null end means it stays open
            // until the event ends.
            migrationBuilder.AddColumn<DateTime>(
                name: "DrinkSalesStartDate",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DrinkSalesEndDate",
                table: "Events",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrinkSalesStartDate",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "DrinkSalesEndDate",
                table: "Events");
        }
    }
}
