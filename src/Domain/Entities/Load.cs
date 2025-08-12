using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

// Epic 5: Load Management
public enum LoadType
{
    Normal = 1,
    Abnormal = 2,
    Dangerous = 3,
    Oversized = 4
}

public class Load
{
    public int Id { get; set; }

    [Required]
    public int PermitId { get; set; }

    public LoadType LoadType { get; set; }

    public decimal WeightKg { get; set; }

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [StringLength(100)]
    public string CargoType { get; set; } = string.Empty;

    public bool IsIndivisible { get; set; } = false;

    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Permit Permit { get; set; } = null!;
    public ICollection<LoadDimension> Dimensions { get; set; } = new List<LoadDimension>();
    public ICollection<LoadProjection> Projections { get; set; } = new List<LoadProjection>();
}

public class LoadDimension
{
    public int Id { get; set; }

    [Required]
    public int LoadId { get; set; }

    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }

    [StringLength(10)]
    public string Unit { get; set; } = "mm";

    // Navigation properties
    public Load Load { get; set; } = null!;
}

public class LoadProjection
{
    public int Id { get; set; }

    [Required]
    public int LoadId { get; set; }

    [StringLength(20)]
    public string Direction { get; set; } = string.Empty; // Front, Rear, Left, Right

    public decimal ProjectionMm { get; set; }

    [StringLength(10)]
    public string Unit { get; set; } = "mm";

    // Navigation properties
    public Load Load { get; set; } = null!;
}
