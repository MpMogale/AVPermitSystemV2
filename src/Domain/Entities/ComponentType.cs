using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

public class ComponentType
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(10)]
    public string Code { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<VehicleComponent> VehicleComponents { get; set; } = new List<VehicleComponent>();
}
