using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using StadiumDrinkOrdering.API.Data;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Adds the per-venue stadium seat-map image (bytea) + its content type. Replaces the previously
    /// hardcoded stadium-blueprint.png default: each club now uploads its own image (part of first-run
    /// setup), so both columns are nullable and default to null (empty canvas until uploaded).
    /// </summary>
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260724090000_AddVenueStadiumImage")]
    public partial class AddVenueStadiumImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "StadiumImage",
                table: "Venues",
                type: "bytea",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "StadiumImageContentType",
                table: "Venues",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StadiumImage",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "StadiumImageContentType",
                table: "Venues");
        }
    }
}
