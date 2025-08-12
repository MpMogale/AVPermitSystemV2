using System.ComponentModel.DataAnnotations;

namespace AVPermitSystemV2.Domain.Entities;

// Epic 7: Event Tracking & Auditing
public enum EventType
{
    VehicleRegistered = 1,
    OwnershipChanged = 2,
    PermitApplied = 3,
    PermitApproved = 4,
    PermitRejected = 5,
    InspectionCompleted = 6,
    ViolationRecorded = 7,
    MaintenancePerformed = 8,
    Other = 99
}

public class VehicleEvent
{
    public int Id { get; set; }

    [Required]
    public int VehicleId { get; set; }

    public EventType EventType { get; set; }

    [Required]
    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    public DateTime EventDate { get; set; }

    [StringLength(100)]
    public string Location { get; set; } = string.Empty;

    [StringLength(100)]
    public string RecordedBy { get; set; } = string.Empty;

    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;

    [StringLength(1000)]
    public string AdditionalData { get; set; } = string.Empty; // JSON for flexible data

    // Auditing
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }

    // Navigation properties
    public Vehicle Vehicle { get; set; } = null!;
}
