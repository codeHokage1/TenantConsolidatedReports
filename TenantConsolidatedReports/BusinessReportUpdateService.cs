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

                // Wait until 12 AM
                await Task.Delay(timeUntilMidnight, stoppingToken);

                // Run the update logic at midnight
                await UpdateBusinessReports();
            }
        }

        private async Task UpdateBusinessReports()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var _reportDbContext = scope.ServiceProvider.GetRequiredService<ReportDbContext>();
                var _identityDbContext = scope.ServiceProvider.GetRequiredService<IdentityDBContext>();

                // Reuse your existing logic here (the same as in the controller)
                // You can refactor the logic out of the controller into a separate method for reuse.

                // Create object with the number of business units updated and added
                var updatedReport = 0;
                var newlyAddedReport = 0;

                // Get details of the business unit from the IdentitDbContext
                var businessUnitsDetails = await _identityDbContext.AbpOrganizationUnits.ToListAsync();
                Console.WriteLine("Total Business units: " + businessUnitsDetails.Count);
                // Loop through each business unit detail check if the business unit is already in the reportDb
                foreach (var unit in businessUnitsDetails)
                {
                    // check if the business unit is already in the reportDb
                    var foundBusinessUnit = await _reportDbContext.Reports.FirstOrDefaultAsync(x => x.BusinessUnitId == unit.Id);
                    // if no, add the business unit details to the reportDb
                    if (foundBusinessUnit == null)
                    {
                        Console.WriteLine(unit.DisplayName + " does not exist in the reportDb");
                        var newBusinessUnitReport = new BusinessUnitReport
                        {
                            BusinessUnitId = unit.Id,
                            TenantId = unit.TenantId ?? Guid.Empty,
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
                    }
                    else
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
