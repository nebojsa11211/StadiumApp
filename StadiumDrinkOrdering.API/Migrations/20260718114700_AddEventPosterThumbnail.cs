using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StadiumDrinkOrdering.API.Migrations
{
    /// <summary>
    /// Adds the downscaled poster variant served to the customer fixture strip, which renders up to
    /// 20 cards and must not fetch the multi-MB originals.
    ///
    /// As with AddEventPoster, the scaffolded version re-stamped every seeded CreatedAt/PasswordHash
    /// row (~5800 lines of seed-timestamp churn). Those UpdateData calls are deliberately removed.
    /// </summary>
    public partial class AddEventPosterThumbnail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ThumbnailContentType",
                table: "EventPosters",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ThumbnailData",
                table: "EventPosters",
                type: "bytea",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ThumbnailContentType",
                table: "EventPosters");

            migrationBuilder.DropColumn(
                name: "ThumbnailData",
                table: "EventPosters");
        }
    }
}
