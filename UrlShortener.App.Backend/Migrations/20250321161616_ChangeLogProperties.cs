using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UrlShortener.App.Backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangeLogProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "RedirectLogs");

            migrationBuilder.RenameColumn(
                name: "Zip",
                table: "RedirectLogs",
                newName: "OsVersion");

            migrationBuilder.RenameColumn(
                name: "Timezone",
                table: "RedirectLogs",
                newName: "OsName");

            migrationBuilder.RenameColumn(
                name: "RegionName",
                table: "RedirectLogs",
                newName: "OsFamily");

            migrationBuilder.RenameColumn(
                name: "Region",
                table: "RedirectLogs",
                newName: "DeviceType");

            migrationBuilder.RenameColumn(
                name: "Isp",
                table: "RedirectLogs",
                newName: "DeviceModel");

            migrationBuilder.RenameColumn(
                name: "CountryCode",
                table: "RedirectLogs",
                newName: "DeviceBrand");

            migrationBuilder.RenameColumn(
                name: "Country",
                table: "RedirectLogs",
                newName: "ClientType");

            migrationBuilder.RenameColumn(
                name: "City",
                table: "RedirectLogs",
                newName: "ClientName");

            migrationBuilder.AddColumn<string>(
                name: "BrowserFamily",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ClientEngine",
                table: "RedirectLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BrowserFamily",
                table: "RedirectLogs");

            migrationBuilder.DropColumn(
                name: "ClientEngine",
                table: "RedirectLogs");

            migrationBuilder.RenameColumn(
                name: "OsVersion",
                table: "RedirectLogs",
                newName: "Zip");

            migrationBuilder.RenameColumn(
                name: "OsName",
                table: "RedirectLogs",
                newName: "Timezone");

            migrationBuilder.RenameColumn(
                name: "OsFamily",
                table: "RedirectLogs",
                newName: "RegionName");

            migrationBuilder.RenameColumn(
                name: "DeviceType",
                table: "RedirectLogs",
                newName: "Region");

            migrationBuilder.RenameColumn(
                name: "DeviceModel",
                table: "RedirectLogs",
                newName: "Isp");

            migrationBuilder.RenameColumn(
                name: "DeviceBrand",
                table: "RedirectLogs",
                newName: "CountryCode");

            migrationBuilder.RenameColumn(
                name: "ClientType",
                table: "RedirectLogs",
                newName: "Country");

            migrationBuilder.RenameColumn(
                name: "ClientName",
                table: "RedirectLogs",
                newName: "City");

            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "RedirectLogs",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "RedirectLogs",
                type: "float",
                nullable: true);
        }
    }
}
