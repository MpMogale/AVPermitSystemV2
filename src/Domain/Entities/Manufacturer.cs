using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

public class Manufacturer
{
    public int Id { get; set; }

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

    public bool IsActive { get; set; } = true;

    // Auditing
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}
