using Microsoft.EntityFrameworkCore;
using TenantConsolidatedReports.Data;
using TenantConsolidatedReports.Models.Entities;

namespace TenantConsolidatedReports
{
    public class BusinessReportUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public BusinessReportUpdateService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Calculate the time until midnight
                var now = DateTime.Now;
                var nextMidnight = now.Date.AddDays(1);
                var timeUntilMidnight = nextMidnight - now;

                //// Wait until 12 AM
                await Task.Delay(timeUntilMidnight, stoppingToken);

                // Tesing: Wait for 2 minutes before running again
                //await Task.Delay(TimeSpan.FromMinutes(2), stoppingToken);

                // Run the update logic at midnight
                await UpdateBusinessReports();
            }
        }

        private async Task UpdateBusinessReports()
        {
            Console.WriteLine("Updating Business Reports from Background Service");
            using (var scope = _serviceProvider.CreateScope())
            {
                var _reportDbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
                var _identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDBContext>();
                var _saasDbContext = scope.ServiceProvider.GetRequiredService<SaasDbContext>();

                // Create object with the number of business units updated and added
                var updatedReport = 0;
                var newlyAddedReport = 0;

                // Get details of the business unit from the IdentitDbContext
                var businessUnitsDetails = await _identityDbContext.AbpOrganizationUnits.ToListAsync();
                Console.WriteLine("Total Business units: " + businessUnitsDetails.Count);
                // Loop through each business unit detail check if the business unit is already in the reportDb
                foreach (var unit in businessUnitsDetails)
                {
                    var parentName = "tempParentName";
                    var unitHeadName = "tempUnitHeadName";
                    var tenantName = "tempTenantName";

                    // check if the business unit is already in the reportDb
                    var foundBusinessUnit = await _reportDbContext.BusinessReports.FirstOrDefaultAsync(x => x.BusinessUnitId == unit.Id);
                    // if no, add the business unit details to the reportDb
                    if (foundBusinessUnit == null)
                    {
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

                        Console.WriteLine(unit.DisplayName + " does not exist in the reportDb");
                        var newBusinessUnitReport = new BusinessUnitReport
                        {
                            BusinessUnitId = unit.Id,
                            TenantId = unit.TenantId ?? Guid.Empty,
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
                    }
                    else
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
                }
                else
                {
                    reportUpdateExecution.LastExecuted = DateTime.Now;
                    reportUpdateExecution.ReportsUpdated = updatedReport;
                    reportUpdateExecution.ReportsNewlyAdded = newlyAddedReport;
                    reportUpdateExecution.RecordUpdatedAt = DateTime.Now;
                    await _reportDbContext.SaveChangesAsync();
                }
            }
        }
    }

}
