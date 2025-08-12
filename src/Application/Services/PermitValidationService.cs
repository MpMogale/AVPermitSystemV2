using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.Application.Services;

public interface IPermitValidationService
{
    Task<ValidationResult> ValidatePermitApplicationAsync(int vehicleId, int permitTypeId, DateTime validFromDate, DateTime validToDate);
    Task<ValidationResult> ValidateVehicleForPermitAsync(int vehicleId, PermitType permitType);
    Task<ValidationResult> ValidatePermitStatusTransitionAsync(int permitId, PermitStatus newStatus);
}

public class PermitValidationService : IPermitValidationService
{
    private readonly AppDbContext _context;

    public PermitValidationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ValidationResult> ValidatePermitApplicationAsync(int vehicleId, int permitTypeId, DateTime validFromDate, DateTime validToDate)
    {
        var result = new ValidationResult();

        // Validate vehicle exists and is active
        var vehicle = await _context.Vehicles
            .Include(v => v.VehicleCategory)
            .FirstOrDefaultAsync(v => v.Id == vehicleId && v.IsActive);
        
        if (vehicle == null)
        {
            result.AddError("Vehicle not found or inactive");
            return result;
        }

        // Validate permit type
        var permitType = await _context.PermitTypes.FirstOrDefaultAsync(pt => pt.Id == permitTypeId && pt.IsActive);
        if (permitType == null)
        {
            result.AddError("Permit type not found or inactive");
            return result;
        }

        // Validate date range
        if (validFromDate >= validToDate)
        {
            result.AddError("Valid from date must be before valid to date");
        }

        if (validFromDate < DateTime.UtcNow.Date)
        {
            result.AddError("Valid from date cannot be in the past");
        }

        // Check for overlapping permits of the same type
        var hasOverlappingPermits = await _context.Permits
            .AnyAsync(p => p.VehicleId == vehicleId &&
                          p.PermitTypeId == permitTypeId &&
                          p.Status != PermitStatus.Cancelled &&
                          p.Status != PermitStatus.Rejected &&
                          p.Status != PermitStatus.Expired &&
                          ((validFromDate >= p.ValidFromDate && validFromDate <= p.ValidToDate) ||
                           (validToDate >= p.ValidFromDate && validToDate <= p.ValidToDate) ||
                           (validFromDate <= p.ValidFromDate && validToDate >= p.ValidToDate)));

        if (hasOverlappingPermits)
        {
            result.AddError("Vehicle already has an active or pending permit of this type for the specified period");
        }

        // Validate vehicle against permit type requirements
        var vehicleValidation = await ValidateVehicleForPermitAsync(vehicleId, permitType);
        result.Merge(vehicleValidation);

        return result;
    }

    public async Task<ValidationResult> ValidateVehicleForPermitAsync(int vehicleId, PermitType permitType)
    {
        var result = new ValidationResult();

        var vehicle = await _context.Vehicles
            .Include(v => v.VehicleCategory)
            .Include(v => v.Components)
            .ThenInclude(c => c.Dimensions)
            .FirstOrDefaultAsync(v => v.Id == vehicleId);

        if (vehicle == null)
        {
            result.AddError("Vehicle not found");
            return result;
        }

        // Check if vehicle has required ownership
        var hasActiveOwnership = await _context.VehicleOwnerships
            .AnyAsync(vo => vo.VehicleId == vehicleId &&
                           (!vo.EndDate.HasValue || vo.EndDate > DateTime.UtcNow));

        if (!hasActiveOwnership)
        {
            result.AddError("Vehicle must have an active owner");
        }

        // Validate dimensions for abnormal load permits
        if (permitType.Code == "ABN" && vehicle.VehicleCategory != null)
        {
            if (vehicle.LengthMm > vehicle.VehicleCategory.MaxLengthMm)
            {
                result.AddWarning($"Vehicle length ({vehicle.LengthMm}mm) exceeds category limit ({vehicle.VehicleCategory.MaxLengthMm}mm)");
            }
            if (vehicle.WidthMm > vehicle.VehicleCategory.MaxWidthMm)
            {
                result.AddWarning($"Vehicle width ({vehicle.WidthMm}mm) exceeds category limit ({vehicle.VehicleCategory.MaxWidthMm}mm)");
            }
            if (vehicle.HeightMm > vehicle.VehicleCategory.MaxHeightMm)
            {
                result.AddWarning($"Vehicle height ({vehicle.HeightMm}mm) exceeds category limit ({vehicle.VehicleCategory.MaxHeightMm}mm)");
            }
        }

        // Check if vehicle has required components
        if (!vehicle.Components.Any())
        {
            result.AddWarning("Vehicle has no registered components");
        }

        return result;
    }

    public async Task<ValidationResult> ValidatePermitStatusTransitionAsync(int permitId, PermitStatus newStatus)
    {
        var result = new ValidationResult();

        var permit = await _context.Permits.FindAsync(permitId);
        if (permit == null)
        {
            result.AddError("Permit not found");
            return result;
        }

        var isValidTransition = permit.Status switch
        {
            PermitStatus.Draft => newStatus is PermitStatus.Submitted or PermitStatus.Cancelled,
            PermitStatus.Submitted => newStatus is PermitStatus.UnderReview or PermitStatus.Cancelled,
            PermitStatus.UnderReview => newStatus is PermitStatus.Approved or PermitStatus.Rejected,
            PermitStatus.Approved => newStatus is PermitStatus.Expired or PermitStatus.Cancelled,
            PermitStatus.Rejected => newStatus is PermitStatus.Draft,
            PermitStatus.Expired => false,
            PermitStatus.Cancelled => false,
            _ => false
        };

        if (!isValidTransition)
        {
            result.AddError($"Invalid status transition from {permit.Status} to {newStatus}");
        }

        return result;
    }
}

public class ValidationResult
{
    public bool IsValid => !Errors.Any();
    public List<string> Errors { get; } = new();
    public List<string> Warnings { get; } = new();

    public void AddError(string error)
    {
        Errors.Add(error);
    }

    public void AddWarning(string warning)
    {
        Warnings.Add(warning);
    }

    public void Merge(ValidationResult other)
    {
        Errors.AddRange(other.Errors);
        Warnings.AddRange(other.Warnings);
    }
}
