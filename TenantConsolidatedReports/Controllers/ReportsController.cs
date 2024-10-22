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

        public ReportsController(ReportDbContext reportDbContext, IdentityDBContext identityDbContext)
        {
            _reportDbContext = reportDbContext;
            _identityDbContext = identityDbContext;
        }

        // Endpoint to get business-report
        [HttpGet]
        [Route("business-report")]
        async public Task<IActionResult> GetBusinessReports()
        {
            var reports = await _reportDbContext.Reports.ToListAsync();
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
            var createdBusinessReport = await _reportDbContext.Reports.AddAsync(businessReport);
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
                // check if the business unit is already in the reportDb
                var foundBusinessUnit = await _reportDbContext.Reports.FirstOrDefaultAsync(x => x.BusinessUnitId == unit.Id);
                // if no, add the business unit details to the reportDb
                if(foundBusinessUnit == null)
                {
                    Console.WriteLine(unit.DisplayName + " does not exist in the reportDb");
                    var newBusinessUnitReport = new BusinessUnitReport
                    {
                        BusinessUnitId = unit.Id,
                        TenantId =  unit.TenantId ?? Guid.Empty,
                        TenantName = "tempTenantName",
                        DisplayName = unit.DisplayName,
                        ParentId = unit.ParentId ?? Guid.Empty,
                        ParentName = "tempParentName",
                        UnitHeadId = unit.UnitHeadId ?? Guid.Empty,
                        UnitHeadFullName = "tempUnitHeadName",
                        UnitId = unit.UnitID ?? String.Empty

                    };
                    await _reportDbContext.Reports.AddAsync(newBusinessUnitReport);
                    await _reportDbContext.SaveChangesAsync();
                    newlyAddedReport++;                    
                } else
                {
                    // if yes, find that businessUnit details in the report and update the info from the identityDb
                    Console.WriteLine(unit.DisplayName + " exists in the reportDb");
                    foundBusinessUnit.TenantId = unit.TenantId ?? Guid.Empty;
                    foundBusinessUnit.TenantName = "tempTenantName";
                    foundBusinessUnit.DisplayName = unit.DisplayName;
                    foundBusinessUnit.ParentId = unit.ParentId ?? Guid.Empty;
                    foundBusinessUnit.ParentName = "tempParentName";
                    foundBusinessUnit.UnitHeadId = unit.UnitHeadId ?? Guid.Empty;
                    foundBusinessUnit.UnitHeadFullName = "tempUnitHeadName";
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
