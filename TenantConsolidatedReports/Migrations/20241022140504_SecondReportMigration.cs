using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantConsolidatedReports.Migrations
{
    /// <inheritdoc />
    public partial class SecondReportMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReportsUpdateExecutions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReportType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastExecuted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReportsUpdated = table.Column<int>(type: "int", nullable: false),
                    ReportsNewlyAdded = table.Column<int>(type: "int", nullable: false),
                    RecordCreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RecordUpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReportsUpdateExecutions", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReportsUpdateExecutions");
        }
    }
}
