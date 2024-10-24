using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenantConsolidatedReports.Data;
using TenantConsolidatedReports.Models.DTOs;
using TenantConsolidatedReports.Models.Entities;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TenantConsolidatedReports.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly ReportDbContext _reportDbContext;
        private readonly IdentityDBContext _identityDbContext;
        private readonly SaasDbContext _saasDbContext;

        public ReportsController(ReportDbContext reportDbContext, IdentityDBContext identityDbContext, SaasDbContext saasDbContext)
        {
            _reportDbContext = reportDbContext;
            _identityDbContext = identityDbContext;
            _saasDbContext = saasDbContext;
        }

        // Endpoint to get business-report
        [HttpGet]
        [Route("business-report")]
        async public Task<IActionResult> GetBusinessReports()
        {
            var reports = await _reportDbContext.BusinessReports.ToListAsync();
            return Ok(reports);
        }


        // Endpoint to generate business-report
        [HttpPost]
        [Route("business-report")]
        async public Task<IActionResult> GenerateBusinessReports(AddBusinessUnitReportDTO businessUnitInput)
        {
            // Map from DTO to Entity
            var businessReport = new BusinessUnitReport
            {
                BusinessUnitId = businessUnitInput.BusinessUnitId,
                TenantId = businessUnitInput.TenantId,
                TenantName = businessUnitInput.TenantName,
                DisplayName = businessUnitInput.DisplayName,
                ParentId = businessUnitInput.ParentId,
                ParentName = businessUnitInput.ParentName,
                UnitHeadId = businessUnitInput.UnitHeadId,
                UnitHeadFullName = businessUnitInput.UnitHeadFullName,
                UnitId = businessUnitInput.UnitId
            };

            // Create businessReport from the businessReportInput
            var createdBusinessReport = await _reportDbContext.BusinessReports.AddAsync(businessReport);
            await _reportDbContext.SaveChangesAsync();

            // Map the created businessReport to the BusinessUnitReportDTO
            var reportToSend = new BusinessUnitReportDTO
            {
                Id = createdBusinessReport.Entity.Id,
                BusinessUnitId = createdBusinessReport.Entity.BusinessUnitId,
                TenantId = createdBusinessReport.Entity.TenantId,
                TenantName = createdBusinessReport.Entity.TenantName,
                DisplayName = createdBusinessReport.Entity.DisplayName,
                ParentId = createdBusinessReport.Entity.ParentId,
                ParentName = createdBusinessReport.Entity.ParentName,
                UnitHeadId = createdBusinessReport.Entity.UnitHeadId,
                UnitHeadFullName = createdBusinessReport.Entity.UnitHeadFullName,
                UnitId = createdBusinessReport.Entity.UnitId,
                IsDeleted = createdBusinessReport.Entity.IsDeleted,
                CreatedDate = createdBusinessReport.Entity.CreatedDate,
            };

            // Return the DTO
            return Ok(reportToSend);

        }

        // Endpoint to automatically update report
        [HttpPost]
        [Route("business-report/manual-update")]
        async public Task<IActionResult> UpdateBusinessReport()
        {
            // Create object with the number of business units updated and added
            var updatedReport = 0;
            var newlyAddedReport = 0;

            // Get details of the business unit from the IdentitDbContext
            var businessUnitsDetails = await _identityDbContext.AbpOrganizationUnits.ToListAsync();
            Console.WriteLine("Total Business units: " + businessUnitsDetails.Count);
            // Loop through each business unit detail check if the business unit is already in the reportDb
            foreach(var unit in  businessUnitsDetails)
            {
                var parentName = "tempParentName";
                var unitHeadName = "tempUnitHeadName";
                var tenantName = "tempTenantName";

                // check if the business unit is already in the reportDb
                var foundBusinessUnit = await _reportDbContext.BusinessReports.FirstOrDefaultAsync(x => x.BusinessUnitId == unit.Id);
                // if no, add the business unit details to the reportDb
                if(foundBusinessUnit == null)
                {
                    // Get the ParentName: the DisplayName of a BusinessUnit with the same Id as the ParentId of the current BusinessUnit
                    var parentDetails = await _identityDbContext.AbpOrganizationUnits.FirstOrDefaultAsync(x => x.Id == unit.ParentId);
                    if (parentDetails != null)
                    {
                        parentName = parentDetails.DisplayName;
                    }

                    // Get UnitHeadFullName from AbpUser table, where the Id is the UnitHeadId of the current BusinessUnit
                    var unitHeadDetails = await _identityDbContext.AbpUsers.FirstOrDefaultAsync(x => x.Id == unit.UnitHeadId);
                    if(unitHeadDetails != null)
                    {
                        unitHeadName = unitHeadDetails.Name + " " + unitHeadDetails.Surname;
                    }

                    // Get the TenantName from the SaasTenant table, where the Id is the TenantId of the current BusinessUnit
                    var tenantDetails = await _saasDbContext.SaasTenants.FirstOrDefaultAsync(x => x.Id == unit.TenantId);
                    if(tenantDetails != null)
                    {
                        tenantName = tenantDetails.Name;
                    }

                    Console.WriteLine(unit.DisplayName + " does not exist in the reportDb");
                    var newBusinessUnitReport = new BusinessUnitReport
                    {
                        BusinessUnitId = unit.Id,
                        TenantId =  unit.TenantId ?? Guid.Empty,
                        TenantName = tenantName,
                        DisplayName = unit.DisplayName,
                        ParentId = unit.ParentId ?? Guid.Empty,
                        ParentName = parentName,
                        UnitHeadId = unit.UnitHeadId ?? Guid.Empty,
                        UnitHeadFullName = unitHeadName,
                        UnitId = unit.UnitID ?? String.Empty

                    };
                    await _reportDbContext.BusinessReports.AddAsync(newBusinessUnitReport);
                    await _reportDbContext.SaveChangesAsync();
                    newlyAddedReport++;                    
                } else
                {
                    // if yes, find that businessUnit details in the report and update the info from the identityDb

                    // Get the ParentName: the DisplayName of a BusinessUnit with the same Id as the ParentId of the current BusinessUnit
                    var parentDetails = await _identityDbContext.AbpOrganizationUnits.FirstOrDefaultAsync(x => x.Id == unit.ParentId);
                    if (parentDetails != null)
                    {
                        parentName = parentDetails.DisplayName;
                    }

                    // Get UnitHeadFullName from AbpUser table, where the Id is the UnitHeadId of the current BusinessUnit
                    var unitHeadDetails = await _identityDbContext.AbpUsers.FirstOrDefaultAsync(x => x.Id == unit.UnitHeadId);
                    if (unitHeadDetails != null)
                    {
                        unitHeadName = unitHeadDetails.Name + " " + unitHeadDetails.Surname;
                    }

                    // Get the TenantName from the SaasTenant table, where the Id is the TenantId of the current BusinessUnit
                    var tenantDetails = await _saasDbContext.SaasTenants.FirstOrDefaultAsync(x => x.Id == unit.TenantId);
                    if (tenantDetails != null)
                    {
                        tenantName = tenantDetails.Name;
                    }

                    Console.WriteLine(unit.DisplayName + " exists in the reportDb");
                    foundBusinessUnit.TenantId = unit.TenantId ?? Guid.Empty;
                    foundBusinessUnit.TenantName = tenantName;
                    foundBusinessUnit.DisplayName = unit.DisplayName;
                    foundBusinessUnit.ParentId = unit.ParentId ?? Guid.Empty;
                    foundBusinessUnit.ParentName = parentName;
                    foundBusinessUnit.UnitHeadId = unit.UnitHeadId ?? Guid.Empty;
                    foundBusinessUnit.UnitHeadFullName = unitHeadName;
                    foundBusinessUnit.UnitId = unit.UnitID ?? String.Empty;
                    foundBusinessUnit.LastUpdated = DateTime.Now;
                    await _reportDbContext.SaveChangesAsync();
                    updatedReport++;
                }
            }

            // Check for Business Report in the ReportsUpdateExecution table
            var reportUpdateExecution = await _reportDbContext.ReportsUpdateExecutions.FirstOrDefaultAsync(x => x.ReportType == "BusinessUnitReport");
            if (reportUpdateExecution == null)
            {
                var newReportUpdateExecution = new ReportsUpdateExecution
                {
                    ReportType = "BusinessUnitReport",
                    LastExecuted = DateTime.Now,
                    ReportsUpdated = updatedReport,
                    ReportsNewlyAdded = newlyAddedReport
                };
                await _reportDbContext.ReportsUpdateExecutions.AddAsync(newReportUpdateExecution);
                await _reportDbContext.SaveChangesAsync();
            } else
            {
                reportUpdateExecution.LastExecuted = DateTime.Now;
                reportUpdateExecution.ReportsUpdated = updatedReport;
                reportUpdateExecution.ReportsNewlyAdded = newlyAddedReport;
                reportUpdateExecution.RecordUpdatedAt = DateTime.Now;
                await _reportDbContext.SaveChangesAsync();
            }


            // return a simple object that shows the number of business units updated and added
            return Ok(new { updatedReport, newlyAddedReport });
        }
    }
}
