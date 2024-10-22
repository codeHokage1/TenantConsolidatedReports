using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TenantConsolidatedReports.Models.Entities
{
    public class OrganizationUnit
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        [MaxLength(95)]
        public string Code { get; set; }

        [Required]
        [MaxLength(128)]
        public string DisplayName { get; set; }

        [Required]
        public int EntityVersion { get; set; } = 0; // Default value

        public string ExtraProperties { get; set; } = string.Empty; // Default value

        [Required]
        [MaxLength(40)]
        public string ConcurrencyStamp { get; set; } = string.Empty; // Default value

        [Required]
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false; // Default value

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        public string Address { get; set; }

        [Required]
        public int BusinessUnitWeight { get; set; } = 0; // Default value

        public Guid? ComplianceRatingId { get; set; }

        public Guid? CurrentTaskId { get; set; }

        public bool? IsAggregated { get; set; }

        public bool? IsControlUnit { get; set; }

        public Guid? RiskGradeId { get; set; }

        public Guid? RiskManagerId { get; set; }

        [Required]
        public Guid Role { get; set; } = Guid.Empty; // Default value

        public string SolId { get; set; }

        [Required]
        public int Status { get; set; } = 0; // Default value

        public string Tag { get; set; }

        public Guid? UnitHeadId { get; set; }

        [Required]
        public string UnitID { get; set; } = string.Empty; // Default value

        public bool? IsAssuranceTeam { get; set; }

        public Guid? AssuranceParentTeamId { get; set; }

        //// Navigation Property (if you plan to use Entity Framework Core for relationships)
        //[ForeignKey("ParentId")]
        //public virtual AbpOrganizationUnit ParentUnit { get; set; }
    }
}
