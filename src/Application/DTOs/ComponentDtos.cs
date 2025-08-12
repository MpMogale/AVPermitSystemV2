using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Application.DTOs;

// Component DTOs
public class CreateVehicleComponentDto
{
    [Required]
    public int ComponentTypeId { get; set; }

    [StringLength(50)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [StringLength(100)]
    public string SerialNumber { get; set; } = string.Empty;

    [StringLength(50)]
    public string ManufacturerName { get; set; } = string.Empty;

    [StringLength(50)]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int? YearOfManufacture { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? Mass { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Position { get; set; }
}

public class VehicleComponentDto
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public int ComponentTypeId { get; set; }
    public string ComponentTypeName { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public string ManufacturerName { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? YearOfManufacture { get; set; }
    public decimal? Mass { get; set; }
    public int Position { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public ComponentDimensionDto? Dimensions { get; set; }
}

// Component Dimension DTOs
public class CreateComponentDimensionDto
{
    [Required]
    [Range(0, double.MaxValue)]
    public decimal LengthMm { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal WidthMm { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal HeightMm { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? FrontOverhangMm { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? RearOverhangMm { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? LeftOverhangMm { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? RightOverhangMm { get; set; }

    [StringLength(10)]
    public string Unit { get; set; } = "mm";
}

public class ComponentDimensionDto
{
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }
    public decimal? FrontOverhangMm { get; set; }
    public decimal? RearOverhangMm { get; set; }
    public decimal? LeftOverhangMm { get; set; }
    public decimal? RightOverhangMm { get; set; }
    public string Unit { get; set; } = "mm";
    public DateTime CreatedAt { get; set; }
}

// Axle Group DTOs
public class CreateAxleGroupDto
{
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal SpacingMm { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal UnladenMass { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Position { get; set; }
}

public class AxleGroupDto
{
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal SpacingMm { get; set; }
    public decimal UnladenMass { get; set; }
    public int Position { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public List<AxleDto> Axles { get; set; } = new();
}

// Axle DTOs
public class CreateAxleDto
{
    [Required]
    [Range(1, int.MaxValue)]
    public int TyreCount { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal LoadKg { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Position { get; set; }

    [StringLength(50)]
    public string TyreSize { get; set; } = string.Empty;
}

public class AxleDto
{
    public int Id { get; set; }
    public int AxleGroupId { get; set; }
    public int TyreCount { get; set; }
    public decimal LoadKg { get; set; }
    public int Position { get; set; }
    public string TyreSize { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}
