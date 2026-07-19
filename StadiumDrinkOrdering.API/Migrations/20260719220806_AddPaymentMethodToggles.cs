using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Per-installation switches for which payment methods the Customer app offers when ordering
    /// drinks. All default to true so existing installations keep accepting every method.
    /// (Scaffold-generated seed timestamp churn has been trimmed — only the real schema ops remain.)
    /// </summary>
    /// <inheritdoc />
    public partial class AddPaymentMethodToggles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "WalletPaymentEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CardPaymentEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CashPaymentEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "WalletPaymentEnabled", table: "Venues");
            migrationBuilder.DropColumn(name: "CardPaymentEnabled", table: "Venues");
            migrationBuilder.DropColumn(name: "CashPaymentEnabled", table: "Venues");
        }
    }
}
