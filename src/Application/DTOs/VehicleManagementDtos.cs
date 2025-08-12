using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Application.DTOs;

// Vehicle DTOs
public class CreateVehicleDto
{
    [Required]
    [StringLength(17, MinimumLength = 17)]
    public string VIN { get; set; } = string.Empty;

    [StringLength(20)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public int ManufacturerId { get; set; }

    [Required]
    public int VehicleTypeId { get; set; }

    public int? VehicleCategoryId { get; set; }

    [StringLength(50)]
    public string Model { get; set; } = string.Empty;

    [Range(1900, 2100)]
    public int? YearOfManufacture { get; set; }

    [StringLength(30)]
    public string Color { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal? GrossVehicleMass { get; set; }

    [Range(0, double.MaxValue)]
    public decimal? UnladenMass { get; set; }

    [Range(0, int.MaxValue)]
    public int LengthMm { get; set; }

    [Range(0, int.MaxValue)]
    public int WidthMm { get; set; }

    [Range(0, int.MaxValue)]
    public int HeightMm { get; set; }

    [Range(0, int.MaxValue)]
    public int WheelbaseMm { get; set; }

    [Range(0, int.MaxValue)]
    public int FrontOverhangMm { get; set; }

    [Range(0, int.MaxValue)]
    public int RearOverhangMm { get; set; }
}

public class VehicleDto
{
    public int Id { get; set; }
    public string VIN { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int ManufacturerId { get; set; }
    public string ManufacturerName { get; set; } = string.Empty;
    public int VehicleTypeId { get; set; }
    public string VehicleTypeName { get; set; } = string.Empty;
    public int? VehicleCategoryId { get; set; }
    public string? VehicleCategoryName { get; set; }
    public string Model { get; set; } = string.Empty;
    public int? YearOfManufacture { get; set; }
    public string Color { get; set; } = string.Empty;
    public decimal? GrossVehicleMass { get; set; }
    public decimal? UnladenMass { get; set; }
    public int LengthMm { get; set; }
    public int WidthMm { get; set; }
    public int HeightMm { get; set; }
    public int WheelbaseMm { get; set; }
    public int FrontOverhangMm { get; set; }
    public int RearOverhangMm { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}

// Manufacturer DTOs
public class CreateManufacturerDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [StringLength(50)]
    public string ContactPhone { get; set; } = string.Empty;

    [StringLength(100)]
    public string ContactEmail { get; set; } = string.Empty;

    [StringLength(2)]
    public string CountryCode { get; set; } = string.Empty;
}

public class ManufacturerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public string CountryCode { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
