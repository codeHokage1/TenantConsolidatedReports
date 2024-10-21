namespace TenantConsolidatedReports.Models.Entities
{
    public class BusinessUnitReport
    {
        public int Id { get; set; }
        public int TenantId { get; set; }
        public string TenantName { get; set; }
        public string DisplayName { get; set; }
        public int ParentId { get; set; }
        public string ParentName { get; set; }
        public int UnitHeadId { get; set; }
        public string UnitHeadFullName { get; set; }
        public int UnitId { get; set; }
        public Boolean IsDeleted { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
