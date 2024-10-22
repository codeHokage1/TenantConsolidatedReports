using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantConsolidatedReports.Migrations.IdentityDB
{
    /// <inheritdoc />
    public partial class InitialIdentityMutation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EntityVersion = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BusinessUnitWeight = table.Column<int>(type: "int", nullable: false),
                    ComplianceRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsAggregated = table.Column<bool>(type: "bit", nullable: true),
                    IsControlUnit = table.Column<bool>(type: "bit", nullable: true),
                    RiskGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RiskManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Role = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SolId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAssuranceTeam = table.Column<bool>(type: "bit", nullable: true),
                    AssuranceParentTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUnits", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationUnits");
        }
    }
}
