using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddCupDepositCupQrToken : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CupQrToken",
                table: "CupDeposits",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CupDeposits_CupQrToken",
                table: "CupDeposits",
                column: "CupQrToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CupDeposits_CupQrToken",
                table: "CupDeposits");

            migrationBuilder.DropColumn(
                name: "CupQrToken",
                table: "CupDeposits");

        }
    }
}
