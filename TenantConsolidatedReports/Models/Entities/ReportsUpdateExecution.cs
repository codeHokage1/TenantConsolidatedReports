namespace TenantConsolidatedReports.Models.Entities
{
    public class ReportsUpdateExecution
    {
        public Guid Id { get; set; }
        public required string ReportType { get; set; }
        public DateTime LastExecuted { get; set; }
        public int ReportsUpdated { get; set; } = 0;
        public int ReportsNewlyAdded { get; set; } = 0;
        public DateTime RecordCreatedAt { get; set; } = DateTime.Now;
        public DateTime? RecordUpdatedAt { get; set; } = null;
    }
}
