using Microsoft.EntityFrameworkCore;
using TenantConsolidatedReports.Models;
using TenantConsolidatedReports.Models.Entities;

namespace TenantConsolidatedReports.Data
{
    public class SaasDbContext : DbContext
    {
        public SaasDbContext(DbContextOptions<SaasDbContext> dbContextOptions) : base(dbContextOptions) 
        {
            
        }

        public DbSet<SaasTenant> SaasTenants { get; set; }
    }
}
