using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

public class ComponentDimension
{
    public int Id { get; set; }

    [Required]
    public int ComponentId { get; set; }

    public decimal LengthMm { get; set; }
    public decimal WidthMm { get; set; }
    public decimal HeightMm { get; set; }

    public decimal? FrontOverhangMm { get; set; }
    public decimal? RearOverhangMm { get; set; }
    public decimal? LeftOverhangMm { get; set; }
    public decimal? RightOverhangMm { get; set; }

    [StringLength(10)]
    public string Unit { get; set; } = "mm";

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public VehicleComponent Component { get; set; } = null!;
}
