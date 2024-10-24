using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TenantConsolidatedReports.Models.Entities
{
    public class AbpUser
    {
        [Key]
        public Guid Id { get; set; }

        public Guid? TenantId { get; set; }

        [Required, MaxLength(256)]
        public required string UserName { get; set; }

        [Required, MaxLength(256)]
        public required string NormalizedUserName { get; set; }

        [MaxLength(64)]
        public string? Name { get; set; }

        [MaxLength(64)]
        public string? Surname { get; set; }

        [Required, MaxLength(256)]
        public required string Email { get; set; }

        [Required, MaxLength(256)]
        public required string NormalizedEmail { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; } = false;

        [MaxLength(256)]
        public string? PasswordHash { get; set; }

        [Required, MaxLength(256)]
        public required string SecurityStamp { get; set; }

        [Required]
        public bool IsExternal { get; set; } = false;

        [MaxLength(16)]
        public string? PhoneNumber { get; set; }

        [Required]
        public bool PhoneNumberConfirmed { get; set; } = false;

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public bool TwoFactorEnabled { get; set; } = false;

        public DateTimeOffset? LockoutEnd { get; set; }

        [Required]
        public bool LockoutEnabled { get; set; } = false;

        [Required]
        public int AccessFailedCount { get; set; } = 0;

        [Required]
        public bool ShouldChangePasswordOnNextLogin { get; set; } = false;

        [Required]
        public int EntityVersion { get; set; } = 0;

        public DateTimeOffset? LastPasswordChangeTime { get; set; }

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

        public string? Address { get; set; }

        public Guid? AssuranceTeamId { get; set; }

        public Guid? BusinessUnitId { get; set; }

        public Guid? DesignationId { get; set; }

        public string? Extention { get; set; }

        public Guid? GradeLevelId { get; set; }

        public DateTime? JoinDate { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public DateTime? LastPasswordChangeDate { get; set; }

        public string? MiddleName { get; set; }

        public Guid? Role { get; set; }

        public string? StaffId { get; set; }

        [Required]
        public int Status { get; set; } = 0;

        public Guid? SupervisorId { get; set; }

        public string? Tenure { get; set; }

        public string? KeyResponsibility { get; set; }

        //[ForeignKey("BusinessUnitId")]
        //public virtual AbpOrganizationUnit BusinessUnit { get; set; }
    }
}
