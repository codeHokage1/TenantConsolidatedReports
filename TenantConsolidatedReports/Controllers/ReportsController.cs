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

        public ReportsController(ReportDbContext reportDbContext)
        {
            _reportDbContext = reportDbContext;
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
    }
}
