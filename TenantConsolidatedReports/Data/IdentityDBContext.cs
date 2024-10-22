using Microsoft.EntityFrameworkCore;
using TenantConsolidatedReports.Models.Entities;

namespace TenantConsolidatedReports.Data
{
    public class IdentityDBContext : DbContext
    {
        public IdentityDBContext(DbContextOptions<IdentityDBContext> dbContextOptions) : base(dbContextOptions)
        {
            
        }

        public DbSet<AbpOrganizationUnit> AbpOrganizationUnits { get; set; }
    }
}
