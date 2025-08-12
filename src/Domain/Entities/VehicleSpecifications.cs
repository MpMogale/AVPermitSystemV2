using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

// Epic 8: Specification Modules
public class TruckSpecification
{
    public int Id { get; set; }

    [Required]
    public int VehicleId { get; set; }

    [StringLength(50)]
    public string EngineType { get; set; } = string.Empty;

    public decimal? EngineCapacityLitres { get; set; }

    [StringLength(30)]
    public string FuelType { get; set; } = string.Empty; // Diesel, Petrol, Electric, Hybrid

    public decimal? PowerKw { get; set; }
    public decimal? TorqueNm { get; set; }

    [StringLength(50)]
    public string TransmissionType { get; set; } = string.Empty; // Manual, Automatic, AMT

    public int? NumberOfGears { get; set; }

    [StringLength(50)]
    public string DriveConfiguration { get; set; } = string.Empty; // 4x2, 6x4, 8x4, etc.

    public decimal? FuelTankCapacityLitres { get; set; }

    [StringLength(50)]
    public string EmissionStandard { get; set; } = string.Empty; // Euro 5, Euro 6, etc.

    public decimal? MaxTowingCapacityKg { get; set; }

    [StringLength(50)]
    public string BrakeType { get; set; } = string.Empty; // Air, Hydraulic, Electronic

    public bool HasABS { get; set; } = false;
    public bool HasESC { get; set; } = false; // Electronic Stability Control
    public bool HasRetarder { get; set; } = false;

    [StringLength(50)]
    public string CabType { get; set; } = string.Empty; // Day cab, Sleeper cab

    [StringLength(500)]
    public string AdditionalFeatures { get; set; } = string.Empty;

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Vehicle Vehicle { get; set; } = null!;
}

public class CraneSpecification
{
    public int Id { get; set; }

    [Required]
    public int VehicleId { get; set; }

    [StringLength(50)]
    public string CraneType { get; set; } = string.Empty; // Mobile, Tower, Crawler, etc.

    public decimal? MaxLiftingCapacityKg { get; set; }
    public decimal? MaxReachM { get; set; }
    public decimal? MaxLiftingHeightM { get; set; }

    [StringLength(50)]
    public string BoomType { get; set; } = string.Empty; // Telescopic, Lattice, Articulating

    public int? NumberOfAxles { get; set; }
    public decimal? CounterweightKg { get; set; }

    [StringLength(50)]
    public string OutriggerType { get; set; } = string.Empty; // H-Style, A-Frame, Box

    public decimal? OutriggerExtensionM { get; set; }

    [StringLength(50)]
    public string ControlType { get; set; } = string.Empty; // Joystick, Lever, Computer

    public bool HasLoadMomentIndicator { get; set; } = false;
    public bool HasAntiTwoBlocking { get; set; } = false;

    [StringLength(50)]
    public string CertificationStandard { get; set; } = string.Empty; // EN, ANSI, etc.

    public DateTime? LastInspectionDate { get; set; }
    public DateTime? CertificationExpiryDate { get; set; }

    [StringLength(500)]
    public string SafetyFeatures { get; set; } = string.Empty;

    [StringLength(500)]
    public string OperationalLimitations { get; set; } = string.Empty;

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Vehicle Vehicle { get; set; } = null!;
}

public class TrailerSpecification
{
    public int Id { get; set; }

    [Required]
    public int VehicleId { get; set; }

    [StringLength(50)]
    public string TrailerType { get; set; } = string.Empty; // Flatbed, Lowboy, Step deck, etc.

    public decimal? DeckLengthMm { get; set; }
    public decimal? DeckWidthMm { get; set; }
    public decimal? DeckHeightFromGroundMm { get; set; }

    public decimal? LoadingRampLengthMm { get; set; }
    public decimal? LoadingRampWidthMm { get; set; }

    public int? NumberOfAxles { get; set; }
    public decimal? AxleSpacingMm { get; set; }

    [StringLength(50)]
    public string SuspensionType { get; set; } = string.Empty; // Air, Mechanical, Hydraulic

    [StringLength(50)]
    public string BrakeType { get; set; } = string.Empty; // Air, Electric, Hydraulic

    public bool HasABS { get; set; } = false;

    [StringLength(50)]
    public string TyreSize { get; set; } = string.Empty;

    public int? NumberOfTyres { get; set; }

    [StringLength(50)]
    public string CouplingType { get; set; } = string.Empty; // Fifth wheel, Pintle, Ball

    public decimal? KingpinToRearAxleMm { get; set; }

    public bool HasTieDownPoints { get; set; } = false;
    public int? NumberOfTieDownPoints { get; set; }

    [StringLength(50)]
    public string SideBoardType { get; set; } = string.Empty; // Fixed, Removable, Folding

    public bool HasTarps { get; set; } = false;
    public bool HasWinch { get; set; } = false;

    [StringLength(50)]
    public string FloorMaterial { get; set; } = string.Empty; // Steel, Aluminum, Wood

    public decimal? FloorThicknessMm { get; set; }

    [StringLength(500)]
    public string SpecialFeatures { get; set; } = string.Empty;

    [StringLength(500)]
    public string LoadingInstructions { get; set; } = string.Empty;

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Vehicle Vehicle { get; set; } = null!;
}
