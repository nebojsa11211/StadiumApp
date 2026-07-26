using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddReusableCupsFoundation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CupByocDiscountAmount",
                table: "Venues",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<bool>(
                name: "CupByocEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CupByocRequireApprovedCup",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CupDepositAmount",
                table: "Venues",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 2.00m);

            migrationBuilder.AddColumn<bool>(
                name: "CupDepositBindCupQr",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CupDepositBindReturnToken",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CupDepositBindTicketWallet",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "CupDepositModeEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CupHonorModeEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CupRefundToOriginalMethod",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CupRefundToWallet",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<int>(
                name: "CupRefundWindow",
                table: "Venues",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<bool>(
                name: "CupsEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "CupDepositAmount",
                table: "OrderItems",
                type: "numeric(10,2)",
                precision: 10,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CupMode",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CupTypeId",
                table: "OrderItems",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegisteredCupId",
                table: "OrderItems",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CupTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    VolumeMl = table.Column<int>(type: "integer", nullable: false),
                    UnitCost = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Logo = table.Column<byte[]>(type: "bytea", nullable: true),
                    LogoContentType = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CupTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CupDeposits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: true),
                    WalletId = table.Column<int>(type: "integer", nullable: true),
                    CupTypeId = table.Column<int>(type: "integer", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    ChargeTransactionId = table.Column<long>(type: "bigint", nullable: true),
                    RefundTransactionId = table.Column<long>(type: "bigint", nullable: true),
                    ReturnTokenQr = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ResolvedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CupDeposits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CupDeposits_CupTypes_CupTypeId",
                        column: x => x.CupTypeId,
                        principalTable: "CupTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CupDeposits_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CupMovements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CupTypeId = table.Column<int>(type: "integer", nullable: false),
                    Delta = table.Column<int>(type: "integer", nullable: false),
                    QuantityAfter = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Mode = table.Column<int>(type: "integer", nullable: false),
                    Note = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    UserEmail = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    OrderId = table.Column<int>(type: "integer", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: true),
                    CupDepositId = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CupMovements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CupMovements_CupTypes_CupTypeId",
                        column: x => x.CupTypeId,
                        principalTable: "CupTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CupMovements_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "RegisteredCups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    QrToken = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    CupTypeId = table.Column<int>(type: "integer", nullable: true),
                    OwnerType = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: true),
                    TicketId = table.Column<int>(type: "integer", nullable: true),
                    IsApproved = table.Column<bool>(type: "boolean", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    RegisteredAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegisteredCups", x => x.Id);
                    table.CheckConstraint("CK_RegisteredCups_AtMostOneOwner", "NOT (\"UserId\" IS NOT NULL AND \"TicketId\" IS NOT NULL)");
                    table.ForeignKey(
                        name: "FK_RegisteredCups_CupTypes_CupTypeId",
                        column: x => x.CupTypeId,
                        principalTable: "CupTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RegisteredCups_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_RegisteredCups_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "CupTypes",
                columns: new[] { "Id", "CreatedAt", "IsActive", "Logo", "LogoContentType", "Name", "UnitCost", "UpdatedAt", "VolumeMl" },
                values: new object[] { 1, new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), true, null, null, "Club Cup", 1.00m, null, 500 });

            migrationBuilder.UpdateData(
                table: "Venues",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CupByocEnabled", "CupByocRequireApprovedCup", "CupDepositAmount", "CupDepositBindCupQr", "CupDepositBindReturnToken", "CupDepositBindTicketWallet", "CupDepositModeEnabled", "CupHonorModeEnabled", "CupRefundToOriginalMethod", "CupRefundToWallet", "CupRefundWindow", "CupsEnabled" },
                values: new object[] { false, true, 2.00m, false, false, true, false, false, false, true, 1, false });

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_CupTypeId",
                table: "OrderItems",
                column: "CupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_RegisteredCupId",
                table: "OrderItems",
                column: "RegisteredCupId");

            migrationBuilder.CreateIndex(
                name: "IX_CupDeposits_CupTypeId",
                table: "CupDeposits",
                column: "CupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CupDeposits_OrderId",
                table: "CupDeposits",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CupDeposits_TicketId_Status",
                table: "CupDeposits",
                columns: new[] { "TicketId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_CupMovements_CupTypeId_CreatedAt",
                table: "CupMovements",
                columns: new[] { "CupTypeId", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_CupMovements_OrderId",
                table: "CupMovements",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCups_CupTypeId",
                table: "RegisteredCups",
                column: "CupTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCups_OwnerType_TicketId",
                table: "RegisteredCups",
                columns: new[] { "OwnerType", "TicketId" });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCups_OwnerType_UserId",
                table: "RegisteredCups",
                columns: new[] { "OwnerType", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCups_QrToken",
                table: "RegisteredCups",
                column: "QrToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCups_TicketId",
                table: "RegisteredCups",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_RegisteredCups_UserId",
                table: "RegisteredCups",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_CupTypes_CupTypeId",
                table: "OrderItems",
                column: "CupTypeId",
                principalTable: "CupTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_RegisteredCups_RegisteredCupId",
                table: "OrderItems",
                column: "RegisteredCupId",
                principalTable: "RegisteredCups",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_CupTypes_CupTypeId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_RegisteredCups_RegisteredCupId",
                table: "OrderItems");

            migrationBuilder.DropTable(
                name: "CupDeposits");

            migrationBuilder.DropTable(
                name: "CupMovements");

            migrationBuilder.DropTable(
                name: "RegisteredCups");

            migrationBuilder.DropTable(
                name: "CupTypes");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_CupTypeId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_RegisteredCupId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CupByocDiscountAmount",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupByocEnabled",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupByocRequireApprovedCup",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupDepositAmount",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupDepositBindCupQr",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupDepositBindReturnToken",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupDepositBindTicketWallet",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupDepositModeEnabled",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupHonorModeEnabled",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupRefundToOriginalMethod",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupRefundToWallet",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupRefundWindow",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupsEnabled",
                table: "Venues");

            migrationBuilder.DropColumn(
                name: "CupDepositAmount",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CupMode",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "CupTypeId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "RegisteredCupId",
                table: "OrderItems");

        }
    }
}
