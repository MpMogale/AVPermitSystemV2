using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

// Epic 3: Ownership Lifecycle
public enum OwnerType
{
    Individual = 1,
    Company = 2,
    Government = 3,
    Other = 4
}

public class Owner
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public OwnerType OwnerType { get; set; }

    [StringLength(50)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [StringLength(50)]
    public string ContactPhone { get; set; } = string.Empty;

    [StringLength(100)]
    public string ContactEmail { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<VehicleOwnership> VehicleOwnerships { get; set; } = new List<VehicleOwnership>();
}

public class VehicleOwnership
{
    public int Id { get; set; }

    [Required]
    public int VehicleId { get; set; }

    [Required]
    public int OwnerId { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public bool IsPrimaryOwner { get; set; } = true;

    [StringLength(200)]
    public string Notes { get; set; } = string.Empty;

    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Vehicle Vehicle { get; set; } = null!;
    public Owner Owner { get; set; } = null!;
}
