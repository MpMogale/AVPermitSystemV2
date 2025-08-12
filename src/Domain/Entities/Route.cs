using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

// Epic 6: Routing & Trip Distances
public class Route
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Origin { get; set; } = string.Empty;

    [Required]
    [StringLength(200)]
    public string Destination { get; set; } = string.Empty;

    public decimal DistanceKm { get; set; }

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength(1000)]
    public string Waypoints { get; set; } = string.Empty; // JSON or comma-separated

    public bool IsActive { get; set; } = true;

    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<PermitRoute> PermitRoutes { get; set; } = new List<PermitRoute>();
}

public class PermitRoute
{
    public int Id { get; set; }

    [Required]
    public int PermitId { get; set; }

    [Required]
    public int RouteId { get; set; }

    public int Sequence { get; set; } = 1; // For multi-leg trips

    public DateTime? ScheduledDate { get; set; }

    [StringLength(200)]
    public string Notes { get; set; } = string.Empty;

    // Navigation properties
    public Permit Permit { get; set; } = null!;
    public Route Route { get; set; } = null!;
}
