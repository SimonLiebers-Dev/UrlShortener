using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.App.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddPropertiesToRedirectLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Country",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Isp",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Timezone",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "Country",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "Isp",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "Region",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "Timezone",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "RedirectLogs");
        }
    }
}
