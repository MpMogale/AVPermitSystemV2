using System.ComponentModel.DataAnnotations;
using AVPermitSystemV2.Domain.Entities;

namespace AVPermitSystemV2.Application.DTOs;

// Load DTOs
public class CreateLoadDto
{
    [Required]
    public LoadType LoadType { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal WeightKg { get; set; }

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [StringLength(100)]
    public string CargoType { get; set; } = string.Empty;

    public bool IsIndivisible { get; set; } = false;
}

public class LoadDto
{
    public int Id { get; set; }
    public int PermitId { get; set; }
    public LoadType LoadType { get; set; }
    public string LoadTypeName { get; set; } = string.Empty;
    public decimal WeightKg { get; set; }
    public string Description { get; set; } = string.Empty;
    public string CargoType { get; set; } = string.Empty;
    public bool IsIndivisible { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public List<LoadDimensionDto> Dimensions { get; set; } = new();
    public List<LoadProjectionDto> Projections { get; set; } = new();
}

// Load Dimension DTOs
public class CreateLoadDimensionDto
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

    [StringLength(10)]
    public string Unit { get; set; } = "mm";
}

public class LoadDimensionDto
{
    public int Id { get; set; }
    public int LoadId { get; set; }
    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }
    public string Unit { get; set; } = "mm";
}

// Load Projection DTOs
public class CreateLoadProjectionDto
{
    [Required]
    [StringLength(20)]
    public string Direction { get; set; } = string.Empty; // Front, Rear, Left, Right

    [Required]
    [Range(0, double.MaxValue)]
    public decimal ProjectionMm { get; set; }

    [StringLength(10)]
    public string Unit { get; set; } = "mm";
}

public class LoadProjectionDto
{
    public int Id { get; set; }
    public int LoadId { get; set; }
    public string Direction { get; set; } = string.Empty;
    public decimal ProjectionMm { get; set; }
    public string Unit { get; set; } = "mm";
}

// Route DTOs
public class CreateRouteDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Origin { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Destination { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal DistanceKm { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Waypoints { get; set; } = string.Empty;
}

public class RouteDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Origin { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public decimal DistanceKm { get; set; }
    public string Description { get; set; } = string.Empty;
    public string Waypoints { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}

// Permit Route DTOs
public class CreatePermitRouteDto
{
    [Required]
    public int RouteId { get; set; }

    [Range(1, int.MaxValue)]
    public int Sequence { get; set; } = 1;

    public DateTime? ScheduledDate { get; set; }

    [StringLength(200)]
    public string Notes { get; set; } = string.Empty;
}

public class PermitRouteDto
{
    public int Id { get; set; }
    public int PermitId { get; set; }
    public int RouteId { get; set; }
    public string RouteName { get; set; } = string.Empty;
    public string RouteOrigin { get; set; } = string.Empty;
    public string RouteDestination { get; set; } = string.Empty;
    public decimal RouteDistanceKm { get; set; }
    public int Sequence { get; set; }
    public DateTime? ScheduledDate { get; set; }
    public string Notes { get; set; } = string.Empty;
}
