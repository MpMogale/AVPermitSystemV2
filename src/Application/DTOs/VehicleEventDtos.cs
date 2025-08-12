using System.ComponentModel.DataAnnotations;
using AVPermitSystemV2.Domain.Entities;

namespace AVPermitSystemV2.Application.DTOs;

// Vehicle Event DTOs
public class CreateVehicleEventDto
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    public EventType EventType { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public DateTime EventDate { get; set; } = DateTime.UtcNow;

    [StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [StringLength(100)]
    public string RecordedBy { get; set; } = string.Empty;

    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

    [StringLength(1000)]
    public string AdditionalData { get; set; } = string.Empty;
}

public class VehicleEventDto
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public string VehicleVIN { get; set; } = string.Empty;
    public EventType EventType { get; set; }
    public string EventTypeName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = string.Empty;
    public string RecordedBy { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public string AdditionalData { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}
