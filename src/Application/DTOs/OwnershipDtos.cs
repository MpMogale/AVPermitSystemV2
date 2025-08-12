using System.ComponentModel.DataAnnotations;
using AVPermitSystemV2.Domain.Entities;

namespace AVPermitSystemV2.Application.DTOs;

// Owner DTOs
public class CreateOwnerDto
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required]
    public OwnerType OwnerType { get; set; }

    [StringLength(50)]
    public string RegistrationNumber { get; set; } = string.Empty;

    [StringLength(200)]
    public string Address { get; set; } = string.Empty;

    [StringLength(50)]
    public string ContactPhone { get; set; } = string.Empty;

    [EmailAddress]
    [StringLength(100)]
    public string ContactEmail { get; set; } = string.Empty;
}

public class OwnerDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public OwnerType OwnerType { get; set; }
    public string OwnerTypeName { get; set; } = string.Empty;
    public string RegistrationNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
}

// Vehicle Ownership DTOs
public class CreateVehicleOwnershipDto
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    public int OwnerId { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public bool IsPrimaryOwner { get; set; } = true;

    [StringLength(200)]
    public string Notes { get; set; } = string.Empty;
}

public class VehicleOwnershipDto
{
    public int Id { get; set; }
    public int VehicleId { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public string VehicleVIN { get; set; } = string.Empty;
    public int OwnerId { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public OwnerType OwnerType { get; set; }
    public string OwnerTypeName { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsPrimaryOwner { get; set; }
    public string Notes { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public bool IsActive => !EndDate.HasValue || EndDate > DateTime.UtcNow;
}
