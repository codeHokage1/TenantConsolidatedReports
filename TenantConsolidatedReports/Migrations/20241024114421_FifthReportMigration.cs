using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantConsolidatedReports.Migrations
{
    /// <inheritdoc />
    public partial class FifthReportMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Reports",
                table: "Reports");

            migrationBuilder.RenameTable(
                name: "Reports",
                newName: "BusinessReports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BusinessReports",
                table: "BusinessReports",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_BusinessReports",
                table: "BusinessReports");

            migrationBuilder.RenameTable(
                name: "BusinessReports",
                newName: "Reports");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reports",
                table: "Reports",
                column: "Id");
        }
    }
}
