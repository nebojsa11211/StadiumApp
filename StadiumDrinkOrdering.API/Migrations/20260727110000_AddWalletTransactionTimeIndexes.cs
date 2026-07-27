using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using StadiumDrinkOrdering.API.Data;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Indexes the wallet ledger for the way it is actually read: newest-first by time.
    ///
    /// Every ledger view orders by <c>CreatedAt</c> (the running <c>BalanceAfter</c> column only makes
    /// sense along that timeline — ordering by Id interleaves a seeded or back-dated match day). Without
    /// a matching index each read filtered on an unsorted set and sorted it afterwards.
    ///
    /// <c>IX_WalletTransactions_WalletId</c> is dropped rather than kept: the new composite leads with
    /// the same column, so it serves plain WalletId lookups too and the old one is pure overhead.
    /// </summary>
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20260727110000_AddWalletTransactionTimeIndexes")]
    public partial class AddWalletTransactionTimeIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions");

            // One wallet's ledger, newest first (Admin wallet modal, customer wallet history).
            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId_CreatedAt",
                table: "WalletTransactions",
                columns: new[] { "WalletId", "CreatedAt" });

            // Bar cash reconciliation: CashTopup / TicketTopup / TicketCashOut, newest first.
            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_ReferenceType_CreatedAt",
                table: "WalletTransactions",
                columns: new[] { "ReferenceType", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_ReferenceType_CreatedAt",
                table: "WalletTransactions");

            migrationBuilder.DropIndex(
                name: "IX_WalletTransactions_WalletId_CreatedAt",
                table: "WalletTransactions");

            migrationBuilder.CreateIndex(
                name: "IX_WalletTransactions_WalletId",
                table: "WalletTransactions",
                column: "WalletId");
        }
    }
}
