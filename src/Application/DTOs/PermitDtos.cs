using System.ComponentModel.DataAnnotations;
using AVPermitSystemV2.Domain.Entities;

namespace AVPermitSystemV2.Application.DTOs;

// Permit Type DTOs
public class CreatePermitTypeDto
{
    [Required]
    [StringLength(50)]
    public string Name { get; set; } = string.Empty;

    [StringLength(10)]
    public string Code { get; set; } = string.Empty;

    [StringLength(200)]
    public string Description { get; set; } = string.Empty;

    [Range(0, double.MaxValue)]
    public decimal Fee { get; set; }

    [Range(1, int.MaxValue)]
    public int ValidityDays { get; set; }
}

public class PermitTypeDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public int ValidityDays { get; set; }
    public bool IsActive { get; set; }
}

// Permit DTOs
public class CreatePermitDto
{
    [Required]
    public int VehicleId { get; set; }

    [Required]
    public int PermitTypeId { get; set; }

    [Required]
    public DateTime ValidFromDate { get; set; }

    [Required]
    public DateTime ValidToDate { get; set; }

    [StringLength(200)]
    public string Purpose { get; set; } = string.Empty;

    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
}

public class PermitDto
{
    public int Id { get; set; }
    public string PermitNumber { get; set; } = string.Empty;
    public int VehicleId { get; set; }
    public string VehicleName { get; set; } = string.Empty;
    public string VehicleVIN { get; set; } = string.Empty;
    public int PermitTypeId { get; set; }
    public string PermitTypeName { get; set; } = string.Empty;
    public string PermitTypeCode { get; set; } = string.Empty;
    public PermitStatus Status { get; set; }
    public string StatusName { get; set; } = string.Empty;
    public DateTime ApplicationDate { get; set; }
    public DateTime? ApprovalDate { get; set; }
    public DateTime ValidFromDate { get; set; }
    public DateTime ValidToDate { get; set; }
    public string Purpose { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public decimal Fee { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string CreatedBy { get; set; } = string.Empty;
    public string? UpdatedBy { get; set; }
    public bool IsActive => Status != PermitStatus.Cancelled && Status != PermitStatus.Expired;
    public bool IsValid => Status == PermitStatus.Approved && 
                          DateTime.UtcNow >= ValidFromDate && 
                          DateTime.UtcNow <= ValidToDate;
}

// Permit Constraint DTOs
public class CreatePermitConstraintDto
{
    [Required]
    [StringLength(100)]
    public string ConstraintType { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; } = string.Empty;

    [StringLength(200)]
    public string Value { get; set; } = string.Empty;
}

public class PermitConstraintDto
{
    public int Id { get; set; }
    public int PermitId { get; set; }
    public string ConstraintType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Value { get; set; } = string.Empty;
}

// Update Permit Status DTO
public class UpdatePermitStatusDto
{
    [Required]
    public PermitStatus Status { get; set; }

    [StringLength(500)]
    public string Notes { get; set; } = string.Empty;
}
