using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

// Epic 2: Axle & Configuration Management
public class AxleGroup
{
    public int Id { get; set; }

    [Required]
    public int ComponentId { get; set; }

    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    public decimal SpacingMm { get; set; }
    public decimal UnladenMass { get; set; }
    public int Position { get; set; }

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public VehicleComponent Component { get; set; } = null!;
    public ICollection<Axle> Axles { get; set; } = new List<Axle>();
}

public class Axle
{
    public int Id { get; set; }

    [Required]
    public int AxleGroupId { get; set; }

    public int TyreCount { get; set; }
    public decimal LoadKg { get; set; }
    public int Position { get; set; }

    [StringLength(50)]
    public string TyreSize { get; set; } = string.Empty;

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public AxleGroup AxleGroup { get; set; } = null!;
}
