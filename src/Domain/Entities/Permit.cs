using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

// Epic 4: Permit Management
public enum PermitStatus
{
    Draft = 1,
    Submitted = 2,
    UnderReview = 3,
    Approved = 4,
    Rejected = 5,
    Expired = 6,
    Cancelled = 7
}

public class PermitType
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(10)]
    public string Code { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public decimal Fee { get; set; }
    public int ValidityDays { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<Permit> Permits { get; set; } = new List<Permit>();
}

public class Permit
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string PermitNumber { get; set; } = string.Empty;

    [Required]
    public int VehicleId { get; set; }

    [Required]
    public int PermitTypeId { get; set; }

    public PermitStatus Status { get; set; } = PermitStatus.Draft;

    public DateTime ApplicationDate { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovalDate { get; set; }
    public DateTime ValidFromDate { get; set; }
    public DateTime ValidToDate { get; set; }

    [StringLength(200)]
    public string Purpose { get; set; } = string.Empty;

    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

    public decimal Fee { get; set; }

    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Vehicle Vehicle { get; set; } = null!;
    public PermitType PermitType { get; set; } = null!;
    public ICollection<PermitConstraint> Constraints { get; set; } = new List<PermitConstraint>();
    public ICollection<PermitRoute> Routes { get; set; } = new List<PermitRoute>();
    public Load? Load { get; set; }
}

public class PermitConstraint
{
    public int Id { get; set; }

    [Required]
    public int PermitId { get; set; }

    [Required]
    [StringLength(100)]
    public string ConstraintType { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength(200)]
    public string Value { get; set; } = string.Empty;

    // Navigation properties
    public Permit Permit { get; set; } = null!;
}
