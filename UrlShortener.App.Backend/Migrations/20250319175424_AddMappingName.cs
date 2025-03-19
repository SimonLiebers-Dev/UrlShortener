using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.App.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMappingName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "UrlMappings",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "UrlMappings");
        }
    }
}
