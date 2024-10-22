using Microsoft.EntityFrameworkCore;
using TenantConsolidatedReports.Models.Entities;

namespace TenantConsolidatedReports.Data
{
    public class ReportDbContext : DbContext
    {
        public ReportDbContext(DbContextOptions<ReportDbContext> dbContextOptions) : base(dbContextOptions)
        {            
        }

        public DbSet<BusinessUnitReport> Reports { get; set; }
        public DbSet<ReportsUpdateExecution> ReportsUpdateExecutions { get; set; }
    }
}
