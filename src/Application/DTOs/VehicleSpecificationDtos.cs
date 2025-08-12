using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Application.DTOs;

// Epic 8: Specification Module DTOs
public record TruckSpecificationCreateDto
{
    [Required]
    public int VehicleId { get; init; }

    [StringLength(50)]
    public string EngineType { get; init; } = string.Empty;

    [Range(0.1, 50.0)]
    public decimal? EngineCapacityLitres { get; init; }

    [StringLength(30)]
    public string FuelType { get; init; } = string.Empty;

    [Range(1, 2000)]
    public decimal? PowerKw { get; init; }

    [Range(1, 10000)]
    public decimal? TorqueNm { get; init; }

    [StringLength(50)]
    public string TransmissionType { get; init; } = string.Empty;

    [Range(1, 24)]
    public int? NumberOfGears { get; init; }

    [StringLength(50)]
    public string DriveConfiguration { get; init; } = string.Empty;

    [Range(10, 2000)]
    public decimal? FuelTankCapacityLitres { get; init; }

    [StringLength(50)]
    public string EmissionStandard { get; init; } = string.Empty;

    [Range(0, 200000)]
    public decimal? MaxTowingCapacityKg { get; init; }

    [StringLength(50)]
    public string BrakeType { get; init; } = string.Empty;

    public bool HasABS { get; init; } = false;
    public bool HasESC { get; init; } = false;
    public bool HasRetarder { get; init; } = false;

    [StringLength(50)]
    public string CabType { get; init; } = string.Empty;

    [StringLength(500)]
    public string AdditionalFeatures { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string CreatedBy { get; init; } = string.Empty;
}

public record TruckSpecificationResponseDto
{
    public int Id { get; init; }
    public int VehicleId { get; init; }
    public string EngineType { get; init; } = string.Empty;
    public decimal? EngineCapacityLitres { get; init; }
    public string FuelType { get; init; } = string.Empty;
    public decimal? PowerKw { get; init; }
    public decimal? TorqueNm { get; init; }
    public string TransmissionType { get; init; } = string.Empty;
    public int? NumberOfGears { get; init; }
    public string DriveConfiguration { get; init; } = string.Empty;
    public decimal? FuelTankCapacityLitres { get; init; }
    public string EmissionStandard { get; init; } = string.Empty;
    public decimal? MaxTowingCapacityKg { get; init; }
    public string BrakeType { get; init; } = string.Empty;
    public bool HasABS { get; init; }
    public bool HasESC { get; init; }
    public bool HasRetarder { get; init; }
    public string CabType { get; init; } = string.Empty;
    public string AdditionalFeatures { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? UpdatedBy { get; init; }
}

public record CraneSpecificationCreateDto
{
    [Required]
    public int VehicleId { get; init; }

    [StringLength(50)]
    public string CraneType { get; init; } = string.Empty;

    [Range(100, 2000000)]
    public decimal? MaxLiftingCapacityKg { get; init; }

    [Range(1, 200)]
    public decimal? MaxReachM { get; init; }

    [Range(1, 500)]
    public decimal? MaxLiftingHeightM { get; init; }

    [StringLength(50)]
    public string BoomType { get; init; } = string.Empty;

    [Range(2, 20)]
    public int? NumberOfAxles { get; init; }

    [Range(0, 500000)]
    public decimal? CounterweightKg { get; init; }

    [StringLength(50)]
    public string OutriggerType { get; init; } = string.Empty;

    [Range(0, 20)]
    public decimal? OutriggerExtensionM { get; init; }

    [StringLength(50)]
    public string ControlType { get; init; } = string.Empty;

    public bool HasLoadMomentIndicator { get; init; } = false;
    public bool HasAntiTwoBlocking { get; init; } = false;

    [StringLength(50)]
    public string CertificationStandard { get; init; } = string.Empty;

    public DateTime? LastInspectionDate { get; init; }
    public DateTime? CertificationExpiryDate { get; init; }

    [StringLength(500)]
    public string SafetyFeatures { get; init; } = string.Empty;

    [StringLength(500)]
    public string OperationalLimitations { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string CreatedBy { get; init; } = string.Empty;
}

public record CraneSpecificationResponseDto
{
    public int Id { get; init; }
    public int VehicleId { get; init; }
    public string CraneType { get; init; } = string.Empty;
    public decimal? MaxLiftingCapacityKg { get; init; }
    public decimal? MaxReachM { get; init; }
    public decimal? MaxLiftingHeightM { get; init; }
    public string BoomType { get; init; } = string.Empty;
    public int? NumberOfAxles { get; init; }
    public decimal? CounterweightKg { get; init; }
    public string OutriggerType { get; init; } = string.Empty;
    public decimal? OutriggerExtensionM { get; init; }
    public string ControlType { get; init; } = string.Empty;
    public bool HasLoadMomentIndicator { get; init; }
    public bool HasAntiTwoBlocking { get; init; }
    public string CertificationStandard { get; init; } = string.Empty;
    public DateTime? LastInspectionDate { get; init; }
    public DateTime? CertificationExpiryDate { get; init; }
    public string SafetyFeatures { get; init; } = string.Empty;
    public string OperationalLimitations { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? UpdatedBy { get; init; }
}

public record TrailerSpecificationCreateDto
{
    [Required]
    public int VehicleId { get; init; }

    [StringLength(50)]
    public string TrailerType { get; init; } = string.Empty;

    [Range(1000, 50000)]
    public decimal? DeckLengthMm { get; init; }

    [Range(1000, 5000)]
    public decimal? DeckWidthMm { get; init; }

    [Range(100, 2000)]
    public decimal? DeckHeightFromGroundMm { get; init; }

    [Range(500, 10000)]
    public decimal? LoadingRampLengthMm { get; init; }

    [Range(1000, 5000)]
    public decimal? LoadingRampWidthMm { get; init; }

    [Range(1, 20)]
    public int? NumberOfAxles { get; init; }

    [Range(500, 5000)]
    public decimal? AxleSpacingMm { get; init; }

    [StringLength(50)]
    public string SuspensionType { get; init; } = string.Empty;

    [StringLength(50)]
    public string BrakeType { get; init; } = string.Empty;

    public bool HasABS { get; init; } = false;

    [StringLength(50)]
    public string TyreSize { get; init; } = string.Empty;

    [Range(4, 40)]
    public int? NumberOfTyres { get; init; }

    [StringLength(50)]
    public string CouplingType { get; init; } = string.Empty;

    [Range(1000, 15000)]
    public decimal? KingpinToRearAxleMm { get; init; }

    public bool HasTieDownPoints { get; init; } = false;

    [Range(0, 100)]
    public int? NumberOfTieDownPoints { get; init; }

    [StringLength(50)]
    public string SideBoardType { get; init; } = string.Empty;

    public bool HasTarps { get; init; } = false;
    public bool HasWinch { get; init; } = false;

    [StringLength(50)]
    public string FloorMaterial { get; init; } = string.Empty;

    [Range(1, 100)]
    public decimal? FloorThicknessMm { get; init; }

    [StringLength(500)]
    public string SpecialFeatures { get; init; } = string.Empty;

    [StringLength(500)]
    public string LoadingInstructions { get; init; } = string.Empty;

    [Required, StringLength(100)]
    public string CreatedBy { get; init; } = string.Empty;
}

public record TrailerSpecificationResponseDto
{
    public int Id { get; init; }
    public int VehicleId { get; init; }
    public string TrailerType { get; init; } = string.Empty;
    public decimal? DeckLengthMm { get; init; }
    public decimal? DeckWidthMm { get; init; }
    public decimal? DeckHeightFromGroundMm { get; init; }
    public decimal? LoadingRampLengthMm { get; init; }
    public decimal? LoadingRampWidthMm { get; init; }
    public int? NumberOfAxles { get; init; }
    public decimal? AxleSpacingMm { get; init; }
    public string SuspensionType { get; init; } = string.Empty;
    public string BrakeType { get; init; } = string.Empty;
    public bool HasABS { get; init; }
    public string TyreSize { get; init; } = string.Empty;
    public int? NumberOfTyres { get; init; }
    public string CouplingType { get; init; } = string.Empty;
    public decimal? KingpinToRearAxleMm { get; init; }
    public bool HasTieDownPoints { get; init; }
    public int? NumberOfTieDownPoints { get; init; }
    public string SideBoardType { get; init; } = string.Empty;
    public bool HasTarps { get; init; }
    public bool HasWinch { get; init; }
    public string FloorMaterial { get; init; } = string.Empty;
    public decimal? FloorThicknessMm { get; init; }
    public string SpecialFeatures { get; init; } = string.Empty;
    public string LoadingInstructions { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public DateTime? UpdatedAt { get; init; }
    public string CreatedBy { get; init; } = string.Empty;
    public string? UpdatedBy { get; init; }
}
