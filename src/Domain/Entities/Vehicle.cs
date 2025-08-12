
using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

public class Vehicle
{
    public int Id { get; set; }

    [Required]
    [StringLength(17, MinimumLength = 17)]
    public string VIN { get; set; } = string.Empty;

    [StringLength(20)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int ManufacturerId { get; set; }
    public Manufacturer Manufacturer { get; set; } = null!;

    public int VehicleTypeId { get; set; }
    public VehicleType VehicleType { get; set; } = null!;

    public int? VehicleCategoryId { get; set; }
    public VehicleCategory? VehicleCategory { get; set; }

    [StringLength(50)]
    public string Model { get; set; } = string.Empty;

    public int? YearOfManufacture { get; set; }

    [StringLength(30)]
    public string Color { get; set; } = string.Empty;

    public decimal? GrossVehicleMass { get; set; }
    public decimal? UnladenMass { get; set; }

    public int LengthMm { get; set; }
    public int WidthMm { get; set; }
    public int HeightMm { get; set; }
    public int WheelbaseMm { get; set; }
    public int FrontOverhangMm { get; set; }
    public int RearOverhangMm { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<VehicleComponent> Components { get; set; } = new List<VehicleComponent>();
    public ICollection<VehicleOwnership> Ownerships { get; set; } = new List<VehicleOwnership>();
    public ICollection<Permit> Permits { get; set; } = new List<Permit>();
    public ICollection<VehicleEvent> Events { get; set; } = new List<VehicleEvent>();
}

public class VehicleType
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

public class VehicleCategory
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(200)]
    public string Description { get; set; } = string.Empty;

    public decimal MaxLengthMm { get; set; }
    public decimal MaxWidthMm { get; set; }
    public decimal MaxHeightMm { get; set; }
    public decimal MaxWeightKg { get; set; }

    public bool IsActive { get; set; } = true;
}

public class VehicleComponent
{
    public int Id { get; set; }

    public int VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;

    public int ComponentTypeId { get; set; }
    public ComponentType ComponentType { get; set; } = null!;

    [MaxLength(50)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [MaxLength(100)]
    public string SerialNumber { get; set; } = string.Empty;

    [MaxLength(50)]
    public string ManufacturerName { get; set; } = string.Empty;

    [MaxLength(50)]
    public string Model { get; set; } = string.Empty;

    public int? YearOfManufacture { get; set; }

    public decimal? Mass { get; set; }

    public int Position { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ComponentDimension? Dimensions { get; set; }
    public ICollection<AxleGroup> AxleGroups { get; set; } = new List<AxleGroup>();
}


