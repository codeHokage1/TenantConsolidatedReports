namespace TenantConsolidatedReports.Models.Entities
{
    public class BusinessUnitReport
    {
        public Guid Id { get; set; }
        public Guid BusinessUnitId { get; set; }
        public Guid TenantId { get; set; }
        public string TenantName { get; set; }
        public string DisplayName { get; set; }
        public Guid ParentId { get; set; }
        public string ParentName { get; set; }
        public Guid UnitHeadId { get; set; }
        public string UnitHeadFullName { get; set; }
        public string UnitId { get; set; }
        public Boolean IsDeleted { get; set; } = false;
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? LastUpdated { get; set; }
    }
}
