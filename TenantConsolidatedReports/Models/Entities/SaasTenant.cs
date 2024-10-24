using System.ComponentModel.DataAnnotations;

namespace TenantConsolidatedReports.Models.Entities
{
    public class SaasTenant
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(64)]
        public required string Name { get; set; }

        public Guid? EditionId { get; set; }

        public DateTime? EditionEndDateUtc { get; set; }

        [Required]
        public byte ActivationState { get; set; }

        public DateTime? ActivationEndDate { get; set; }

        [Required]
        public int EntityVersion { get; set; }

        [Required]
        public string ExtraProperties { get; set; } = string.Empty;

        [Required, MaxLength(40)]
        public string ConcurrencyStamp { get; set; } = string.Empty;

        [Required]
        public DateTime CreationTime { get; set; }

        public Guid? CreatorId { get; set; }

        public DateTime? LastModificationTime { get; set; }

        public Guid? LastModifierId { get; set; }

        [Required]
        public bool IsDeleted { get; set; } = false;

        public Guid? DeleterId { get; set; }

        public DateTime? DeletionTime { get; set; }

        [Required, MaxLength(64)]
        public string NormalizedName { get; set; } = string.Empty;
    }
}
