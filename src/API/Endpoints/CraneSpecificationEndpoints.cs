using AVPermitSystemV2.Application.DTOs;
using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints;

public class CraneSpecificationEndpoints : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/crane-specifications")
            .WithTags("Crane Specifications")
            .WithOpenApi();

        // Create crane specification
        group.MapPost("/", async (CraneSpecificationCreateDto dto, AppDbContext context) =>
        {
            // Validate vehicle exists
            var vehicleExists = await context.Vehicles.AnyAsync(v => v.Id == dto.VehicleId);
            if (!vehicleExists)
            {
                return Results.BadRequest($"Vehicle with ID {dto.VehicleId} not found");
            }

            // Check if crane specification already exists for this vehicle
            var existingSpec = await context.CraneSpecifications
                .FirstOrDefaultAsync(cs => cs.VehicleId == dto.VehicleId);
            if (existingSpec != null)
            {
                return Results.Conflict("Crane specification already exists for this vehicle");
            }

            var craneSpec = new CraneSpecification
            {
                VehicleId = dto.VehicleId,
                CraneType = dto.CraneType,
                MaxLiftingCapacityKg = dto.MaxLiftingCapacityKg,
                MaxReachM = dto.MaxReachM,
                MaxLiftingHeightM = dto.MaxLiftingHeightM,
                BoomType = dto.BoomType,
                NumberOfAxles = dto.NumberOfAxles,
                CounterweightKg = dto.CounterweightKg,
                OutriggerType = dto.OutriggerType,
                OutriggerExtensionM = dto.OutriggerExtensionM,
                ControlType = dto.ControlType,
                HasLoadMomentIndicator = dto.HasLoadMomentIndicator,
                HasAntiTwoBlocking = dto.HasAntiTwoBlocking,
                CertificationStandard = dto.CertificationStandard,
                LastInspectionDate = dto.LastInspectionDate,
                CertificationExpiryDate = dto.CertificationExpiryDate,
                SafetyFeatures = dto.SafetyFeatures,
                OperationalLimitations = dto.OperationalLimitations,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            context.CraneSpecifications.Add(craneSpec);
            await context.SaveChangesAsync();

            var responseDto = new CraneSpecificationResponseDto
            {
                Id = craneSpec.Id,
                VehicleId = craneSpec.VehicleId,
                CraneType = craneSpec.CraneType,
                MaxLiftingCapacityKg = craneSpec.MaxLiftingCapacityKg,
                MaxReachM = craneSpec.MaxReachM,
                MaxLiftingHeightM = craneSpec.MaxLiftingHeightM,
                BoomType = craneSpec.BoomType,
                NumberOfAxles = craneSpec.NumberOfAxles,
                CounterweightKg = craneSpec.CounterweightKg,
                OutriggerType = craneSpec.OutriggerType,
                OutriggerExtensionM = craneSpec.OutriggerExtensionM,
                ControlType = craneSpec.ControlType,
                HasLoadMomentIndicator = craneSpec.HasLoadMomentIndicator,
                HasAntiTwoBlocking = craneSpec.HasAntiTwoBlocking,
                CertificationStandard = craneSpec.CertificationStandard,
                LastInspectionDate = craneSpec.LastInspectionDate,
                CertificationExpiryDate = craneSpec.CertificationExpiryDate,
                SafetyFeatures = craneSpec.SafetyFeatures,
                OperationalLimitations = craneSpec.OperationalLimitations,
                CreatedAt = craneSpec.CreatedAt,
                CreatedBy = craneSpec.CreatedBy
            };

            return Results.Created($"/api/crane-specifications/{craneSpec.Id}", responseDto);
        })
        .WithSummary("Create crane specification")
        .Produces<CraneSpecificationResponseDto>(201)
        .ProducesValidationProblem()
        .ProducesProblem(400)
        .ProducesProblem(409);

        // Get crane specification by vehicle ID
        group.MapGet("/vehicle/{vehicleId:int}", async (int vehicleId, AppDbContext context) =>
        {
            var craneSpec = await context.CraneSpecifications
                .FirstOrDefaultAsync(cs => cs.VehicleId == vehicleId);

            if (craneSpec == null)
            {
                return Results.NotFound($"Crane specification not found for vehicle {vehicleId}");
            }

            var responseDto = new CraneSpecificationResponseDto
            {
                Id = craneSpec.Id,
                VehicleId = craneSpec.VehicleId,
                CraneType = craneSpec.CraneType,
                MaxLiftingCapacityKg = craneSpec.MaxLiftingCapacityKg,
                MaxReachM = craneSpec.MaxReachM,
                MaxLiftingHeightM = craneSpec.MaxLiftingHeightM,
                BoomType = craneSpec.BoomType,
                NumberOfAxles = craneSpec.NumberOfAxles,
                CounterweightKg = craneSpec.CounterweightKg,
                OutriggerType = craneSpec.OutriggerType,
                OutriggerExtensionM = craneSpec.OutriggerExtensionM,
                ControlType = craneSpec.ControlType,
                HasLoadMomentIndicator = craneSpec.HasLoadMomentIndicator,
                HasAntiTwoBlocking = craneSpec.HasAntiTwoBlocking,
                CertificationStandard = craneSpec.CertificationStandard,
                LastInspectionDate = craneSpec.LastInspectionDate,
                CertificationExpiryDate = craneSpec.CertificationExpiryDate,
                SafetyFeatures = craneSpec.SafetyFeatures,
                OperationalLimitations = craneSpec.OperationalLimitations,
                CreatedAt = craneSpec.CreatedAt,
                UpdatedAt = craneSpec.UpdatedAt,
                CreatedBy = craneSpec.CreatedBy,
                UpdatedBy = craneSpec.UpdatedBy
            };

            return Results.Ok(responseDto);
        })
        .WithSummary("Get crane specification by vehicle ID")
        .Produces<CraneSpecificationResponseDto>()
        .ProducesProblem(404);

        // Update crane specification
        group.MapPut("/{id:int}", async (int id, CraneSpecificationCreateDto dto, AppDbContext context) =>
        {
            var craneSpec = await context.CraneSpecifications.FindAsync(id);
            if (craneSpec == null)
            {
                return Results.NotFound($"Crane specification {id} not found");
            }

            // Update properties
            craneSpec.CraneType = dto.CraneType;
            craneSpec.MaxLiftingCapacityKg = dto.MaxLiftingCapacityKg;
            craneSpec.MaxReachM = dto.MaxReachM;
            craneSpec.MaxLiftingHeightM = dto.MaxLiftingHeightM;
            craneSpec.BoomType = dto.BoomType;
            craneSpec.NumberOfAxles = dto.NumberOfAxles;
            craneSpec.CounterweightKg = dto.CounterweightKg;
            craneSpec.OutriggerType = dto.OutriggerType;
            craneSpec.OutriggerExtensionM = dto.OutriggerExtensionM;
            craneSpec.ControlType = dto.ControlType;
            craneSpec.HasLoadMomentIndicator = dto.HasLoadMomentIndicator;
            craneSpec.HasAntiTwoBlocking = dto.HasAntiTwoBlocking;
            craneSpec.CertificationStandard = dto.CertificationStandard;
            craneSpec.LastInspectionDate = dto.LastInspectionDate;
            craneSpec.CertificationExpiryDate = dto.CertificationExpiryDate;
            craneSpec.SafetyFeatures = dto.SafetyFeatures;
            craneSpec.OperationalLimitations = dto.OperationalLimitations;
            craneSpec.UpdatedAt = DateTime.UtcNow;
            craneSpec.UpdatedBy = dto.CreatedBy; // Reusing CreatedBy for UpdatedBy

            await context.SaveChangesAsync();

            var responseDto = new CraneSpecificationResponseDto
            {
                Id = craneSpec.Id,
                VehicleId = craneSpec.VehicleId,
                CraneType = craneSpec.CraneType,
                MaxLiftingCapacityKg = craneSpec.MaxLiftingCapacityKg,
                MaxReachM = craneSpec.MaxReachM,
                MaxLiftingHeightM = craneSpec.MaxLiftingHeightM,
                BoomType = craneSpec.BoomType,
                NumberOfAxles = craneSpec.NumberOfAxles,
                CounterweightKg = craneSpec.CounterweightKg,
                OutriggerType = craneSpec.OutriggerType,
                OutriggerExtensionM = craneSpec.OutriggerExtensionM,
                ControlType = craneSpec.ControlType,
                HasLoadMomentIndicator = craneSpec.HasLoadMomentIndicator,
                HasAntiTwoBlocking = craneSpec.HasAntiTwoBlocking,
                CertificationStandard = craneSpec.CertificationStandard,
                LastInspectionDate = craneSpec.LastInspectionDate,
                CertificationExpiryDate = craneSpec.CertificationExpiryDate,
                SafetyFeatures = craneSpec.SafetyFeatures,
                OperationalLimitations = craneSpec.OperationalLimitations,
                CreatedAt = craneSpec.CreatedAt,
                UpdatedAt = craneSpec.UpdatedAt,
                CreatedBy = craneSpec.CreatedBy,
                UpdatedBy = craneSpec.UpdatedBy
            };

            return Results.Ok(responseDto);
        })
        .WithSummary("Update crane specification")
        .Produces<CraneSpecificationResponseDto>()
        .ProducesValidationProblem()
        .ProducesProblem(404);

        // Delete crane specification
        group.MapDelete("/{id:int}", async (int id, AppDbContext context) =>
        {
            var craneSpec = await context.CraneSpecifications.FindAsync(id);
            if (craneSpec == null)
            {
                return Results.NotFound($"Crane specification {id} not found");
            }

            context.CraneSpecifications.Remove(craneSpec);
            await context.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithSummary("Delete crane specification")
        .Produces(204)
        .ProducesProblem(404);

        // Get all crane specifications
        group.MapGet("/", async (AppDbContext context) =>
        {
            var craneSpecs = await context.CraneSpecifications
                .Include(cs => cs.Vehicle)
                .Select(cs => new CraneSpecificationResponseDto
                {
                    Id = cs.Id,
                    VehicleId = cs.VehicleId,
                    CraneType = cs.CraneType,
                    MaxLiftingCapacityKg = cs.MaxLiftingCapacityKg,
                    MaxReachM = cs.MaxReachM,
                    MaxLiftingHeightM = cs.MaxLiftingHeightM,
                    BoomType = cs.BoomType,
                    NumberOfAxles = cs.NumberOfAxles,
                    CounterweightKg = cs.CounterweightKg,
                    OutriggerType = cs.OutriggerType,
                    OutriggerExtensionM = cs.OutriggerExtensionM,
                    ControlType = cs.ControlType,
                    HasLoadMomentIndicator = cs.HasLoadMomentIndicator,
                    HasAntiTwoBlocking = cs.HasAntiTwoBlocking,
                    CertificationStandard = cs.CertificationStandard,
                    LastInspectionDate = cs.LastInspectionDate,
                    CertificationExpiryDate = cs.CertificationExpiryDate,
                    SafetyFeatures = cs.SafetyFeatures,
                    OperationalLimitations = cs.OperationalLimitations,
                    CreatedAt = cs.CreatedAt,
                    UpdatedAt = cs.UpdatedAt,
                    CreatedBy = cs.CreatedBy,
                    UpdatedBy = cs.UpdatedBy
                })
                .ToListAsync();

            return Results.Ok(craneSpecs);
        })
        .WithSummary("Get all crane specifications")
        .Produces<List<CraneSpecificationResponseDto>>();
    }
}
