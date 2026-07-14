using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddTicketWalletOwner : Migration
    {
        // Makes the Wallet owner polymorphic: a wallet is owned by a User (fan wallet) XOR a Ticket
        // (anonymous bearer wallet). UserId becomes nullable, TicketId is added, an OwnerType
        // discriminator column is added, the UserId unique index becomes filtered (so many ticket
        // wallets with NULL UserId don't collide), a filtered unique TicketId index + FK are added,
        // and a check constraint enforces the exactly-one-owner rule.
        //
        // NOTE: hand-trimmed of the ~2,900 lines of seed-timestamp UpdateData churn EF scaffolds on
        // every migration in this repo (see the ef-migration-seed-churn practice) — only the real
        // schema operations remain. Existing rows (UserId set, TicketId NULL) satisfy the check
        // constraint and default OwnerType = 0 (User).

        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Wallets",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<int>(
                name: "OwnerType",
                table: "Wallets",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TicketId",
                table: "Wallets",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_TicketId",
                table: "Wallets",
                column: "TicketId",
                unique: true,
                filter: "\"TicketId\" IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId",
                unique: true,
                filter: "\"UserId\" IS NOT NULL");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Wallets_OneOwner",
                table: "Wallets",
                sql: "(\"UserId\" IS NULL) <> (\"TicketId\" IS NULL)");

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_Tickets_TicketId",
                table: "Wallets",
                column: "TicketId",
                principalTable: "Tickets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Rollback assumes no ticket-owned wallets exist yet (UserId is restored to NOT NULL).
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_Tickets_TicketId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_TicketId",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Wallets_OneOwner",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "OwnerType",
                table: "Wallets");

            migrationBuilder.DropColumn(
                name: "TicketId",
                table: "Wallets");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Wallets",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_UserId",
                table: "Wallets",
                column: "UserId",
                unique: true);
        }
    }
}
