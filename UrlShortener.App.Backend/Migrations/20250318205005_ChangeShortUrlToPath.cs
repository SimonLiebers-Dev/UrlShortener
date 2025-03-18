using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.App.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeShortUrlToPath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortUrl",
                table: "UrlMappings",
                newName: "Path");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Path",
                table: "UrlMappings",
                newName: "ShortUrl");
        }
    }
}
