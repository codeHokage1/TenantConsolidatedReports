namespace TenantConsolidatedReports.Models.DTOs
{
    public class AddBusinessUnitReportDTO
    {
        public Guid BusinessUnitId { get; set; }
        public Guid TenantId { get; set; }
        public string TenantName { get; set; }
        public string DisplayName { get; set; }
        public Guid ParentId { get; set; }
        public string ParentName { get; set; }
        public Guid UnitHeadId { get; set; }
        public string UnitHeadFullName { get; set; }
        public Guid UnitId { get; set; }
    }
}
