using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <inheritdoc />
    public partial class AddVenueEmailSettings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailEnabled",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EmailFromAddress",
                table: "Venues",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailFromName",
                table: "Venues",
                type: "character varying(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpHost",
                table: "Venues",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpPassword",
                table: "Venues",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SmtpPort",
                table: "Venues",
                type: "integer",
                nullable: false,
                defaultValue: 587);

            migrationBuilder.AddColumn<bool>(
                name: "SmtpUseSsl",
                table: "Venues",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "SmtpUsername",
                table: "Venues",
                type: "character varying(200)",
                maxLength: 200,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(name: "EmailEnabled", table: "Venues");
            migrationBuilder.DropColumn(name: "EmailFromAddress", table: "Venues");
            migrationBuilder.DropColumn(name: "EmailFromName", table: "Venues");
            migrationBuilder.DropColumn(name: "SmtpHost", table: "Venues");
            migrationBuilder.DropColumn(name: "SmtpPassword", table: "Venues");
            migrationBuilder.DropColumn(name: "SmtpPort", table: "Venues");
            migrationBuilder.DropColumn(name: "SmtpUseSsl", table: "Venues");
            migrationBuilder.DropColumn(name: "SmtpUsername", table: "Venues");
        }
    }
}
