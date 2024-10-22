using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TenantConsolidatedReports.Migrations.IdentityDB
{
    /// <inheritdoc />
    public partial class SecondIndentityMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrganizationUnits");

            migrationBuilder.CreateTable(
                name: "AbpOrganizationUnits",
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
                    table.PrimaryKey("PK_AbpOrganizationUnits", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbpOrganizationUnits");

            migrationBuilder.CreateTable(
                name: "OrganizationUnits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AssuranceParentTeamId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BusinessUnitWeight = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(95)", maxLength: 95, nullable: false),
                    ComplianceRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CurrentTaskId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeleterId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    EntityVersion = table.Column<int>(type: "int", nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAggregated = table.Column<bool>(type: "bit", nullable: true),
                    IsAssuranceTeam = table.Column<bool>(type: "bit", nullable: true),
                    IsControlUnit = table.Column<bool>(type: "bit", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ParentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RiskGradeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RiskManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Role = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SolId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Tag = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TenantId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitHeadId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UnitID = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrganizationUnits", x => x.Id);
                });
        }
    }
}
