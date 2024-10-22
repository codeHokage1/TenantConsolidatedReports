using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TenantConsolidatedReports.Data;

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
    }
}
