using AVPermitSystemV2.Application.DTOs;
using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints;

public class TruckSpecificationEndpoints : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/truck-specifications")
            .WithTags("Truck Specifications")
            .WithOpenApi();

        // Create truck specification
        group.MapPost("/", async (TruckSpecificationCreateDto dto, AppDbContext context) =>
        {
            // Validate vehicle exists
            var vehicleExists = await context.Vehicles.AnyAsync(v => v.Id == dto.VehicleId);
            if (!vehicleExists)
            {
                return Results.BadRequest($"Vehicle with ID {dto.VehicleId} not found");
            }

            // Check if truck specification already exists for this vehicle
            var existingSpec = await context.TruckSpecifications
                .FirstOrDefaultAsync(ts => ts.VehicleId == dto.VehicleId);
            if (existingSpec != null)
            {
                return Results.Conflict("Truck specification already exists for this vehicle");
            }

            var truckSpec = new TruckSpecification
            {
                VehicleId = dto.VehicleId,
                EngineType = dto.EngineType,
                EngineCapacityLitres = dto.EngineCapacityLitres,
                FuelType = dto.FuelType,
                PowerKw = dto.PowerKw,
                TorqueNm = dto.TorqueNm,
                TransmissionType = dto.TransmissionType,
                NumberOfGears = dto.NumberOfGears,
                DriveConfiguration = dto.DriveConfiguration,
                FuelTankCapacityLitres = dto.FuelTankCapacityLitres,
                EmissionStandard = dto.EmissionStandard,
                MaxTowingCapacityKg = dto.MaxTowingCapacityKg,
                BrakeType = dto.BrakeType,
                HasABS = dto.HasABS,
                HasESC = dto.HasESC,
                HasRetarder = dto.HasRetarder,
                CabType = dto.CabType,
                AdditionalFeatures = dto.AdditionalFeatures,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            context.TruckSpecifications.Add(truckSpec);
            await context.SaveChangesAsync();

            var responseDto = new TruckSpecificationResponseDto
            {
                Id = truckSpec.Id,
                VehicleId = truckSpec.VehicleId,
                EngineType = truckSpec.EngineType,
                EngineCapacityLitres = truckSpec.EngineCapacityLitres,
                FuelType = truckSpec.FuelType,
                PowerKw = truckSpec.PowerKw,
                TorqueNm = truckSpec.TorqueNm,
                TransmissionType = truckSpec.TransmissionType,
                NumberOfGears = truckSpec.NumberOfGears,
                DriveConfiguration = truckSpec.DriveConfiguration,
                FuelTankCapacityLitres = truckSpec.FuelTankCapacityLitres,
                EmissionStandard = truckSpec.EmissionStandard,
                MaxTowingCapacityKg = truckSpec.MaxTowingCapacityKg,
                BrakeType = truckSpec.BrakeType,
                HasABS = truckSpec.HasABS,
                HasESC = truckSpec.HasESC,
                HasRetarder = truckSpec.HasRetarder,
                CabType = truckSpec.CabType,
                AdditionalFeatures = truckSpec.AdditionalFeatures,
                CreatedAt = truckSpec.CreatedAt,
                CreatedBy = truckSpec.CreatedBy
            };

            return Results.Created($"/api/truck-specifications/{truckSpec.Id}", responseDto);
        })
        .WithSummary("Create truck specification")
        .Produces<TruckSpecificationResponseDto>(201)
        .ProducesValidationProblem()
        .ProducesProblem(400)
        .ProducesProblem(409);

        // Get truck specification by vehicle ID
        group.MapGet("/vehicle/{vehicleId:int}", async (int vehicleId, AppDbContext context) =>
        {
            var truckSpec = await context.TruckSpecifications
                .FirstOrDefaultAsync(ts => ts.VehicleId == vehicleId);

            if (truckSpec == null)
            {
                return Results.NotFound($"Truck specification not found for vehicle {vehicleId}");
            }

            var responseDto = new TruckSpecificationResponseDto
            {
                Id = truckSpec.Id,
                VehicleId = truckSpec.VehicleId,
                EngineType = truckSpec.EngineType,
                EngineCapacityLitres = truckSpec.EngineCapacityLitres,
                FuelType = truckSpec.FuelType,
                PowerKw = truckSpec.PowerKw,
                TorqueNm = truckSpec.TorqueNm,
                TransmissionType = truckSpec.TransmissionType,
                NumberOfGears = truckSpec.NumberOfGears,
                DriveConfiguration = truckSpec.DriveConfiguration,
                FuelTankCapacityLitres = truckSpec.FuelTankCapacityLitres,
                EmissionStandard = truckSpec.EmissionStandard,
                MaxTowingCapacityKg = truckSpec.MaxTowingCapacityKg,
                BrakeType = truckSpec.BrakeType,
                HasABS = truckSpec.HasABS,
                HasESC = truckSpec.HasESC,
                HasRetarder = truckSpec.HasRetarder,
                CabType = truckSpec.CabType,
                AdditionalFeatures = truckSpec.AdditionalFeatures,
                CreatedAt = truckSpec.CreatedAt,
                UpdatedAt = truckSpec.UpdatedAt,
                CreatedBy = truckSpec.CreatedBy,
                UpdatedBy = truckSpec.UpdatedBy
            };

            return Results.Ok(responseDto);
        })
        .WithSummary("Get truck specification by vehicle ID")
        .Produces<TruckSpecificationResponseDto>()
        .ProducesProblem(404);

        // Update truck specification
        group.MapPut("/{id:int}", async (int id, TruckSpecificationCreateDto dto, AppDbContext context) =>
        {
            var truckSpec = await context.TruckSpecifications.FindAsync(id);
            if (truckSpec == null)
            {
                return Results.NotFound($"Truck specification {id} not found");
            }

            // Update properties
            truckSpec.EngineType = dto.EngineType;
            truckSpec.EngineCapacityLitres = dto.EngineCapacityLitres;
            truckSpec.FuelType = dto.FuelType;
            truckSpec.PowerKw = dto.PowerKw;
            truckSpec.TorqueNm = dto.TorqueNm;
            truckSpec.TransmissionType = dto.TransmissionType;
            truckSpec.NumberOfGears = dto.NumberOfGears;
            truckSpec.DriveConfiguration = dto.DriveConfiguration;
            truckSpec.FuelTankCapacityLitres = dto.FuelTankCapacityLitres;
            truckSpec.EmissionStandard = dto.EmissionStandard;
            truckSpec.MaxTowingCapacityKg = dto.MaxTowingCapacityKg;
            truckSpec.BrakeType = dto.BrakeType;
            truckSpec.HasABS = dto.HasABS;
            truckSpec.HasESC = dto.HasESC;
            truckSpec.HasRetarder = dto.HasRetarder;
            truckSpec.CabType = dto.CabType;
            truckSpec.AdditionalFeatures = dto.AdditionalFeatures;
            truckSpec.UpdatedAt = DateTime.UtcNow;
            truckSpec.UpdatedBy = dto.CreatedBy; // Reusing CreatedBy for UpdatedBy

            await context.SaveChangesAsync();

            var responseDto = new TruckSpecificationResponseDto
            {
                Id = truckSpec.Id,
                VehicleId = truckSpec.VehicleId,
                EngineType = truckSpec.EngineType,
                EngineCapacityLitres = truckSpec.EngineCapacityLitres,
                FuelType = truckSpec.FuelType,
                PowerKw = truckSpec.PowerKw,
                TorqueNm = truckSpec.TorqueNm,
                TransmissionType = truckSpec.TransmissionType,
                NumberOfGears = truckSpec.NumberOfGears,
                DriveConfiguration = truckSpec.DriveConfiguration,
                FuelTankCapacityLitres = truckSpec.FuelTankCapacityLitres,
                EmissionStandard = truckSpec.EmissionStandard,
                MaxTowingCapacityKg = truckSpec.MaxTowingCapacityKg,
                BrakeType = truckSpec.BrakeType,
                HasABS = truckSpec.HasABS,
                HasESC = truckSpec.HasESC,
                HasRetarder = truckSpec.HasRetarder,
                CabType = truckSpec.CabType,
                AdditionalFeatures = truckSpec.AdditionalFeatures,
                CreatedAt = truckSpec.CreatedAt,
                UpdatedAt = truckSpec.UpdatedAt,
                CreatedBy = truckSpec.CreatedBy,
                UpdatedBy = truckSpec.UpdatedBy
            };

            return Results.Ok(responseDto);
        })
        .WithSummary("Update truck specification")
        .Produces<TruckSpecificationResponseDto>()
        .ProducesValidationProblem()
        .ProducesProblem(404);

        // Delete truck specification
        group.MapDelete("/{id:int}", async (int id, AppDbContext context) =>
        {
            var truckSpec = await context.TruckSpecifications.FindAsync(id);
            if (truckSpec == null)
            {
                return Results.NotFound($"Truck specification {id} not found");
            }

            context.TruckSpecifications.Remove(truckSpec);
            await context.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithSummary("Delete truck specification")
        .Produces(204)
        .ProducesProblem(404);

        // Get all truck specifications
        group.MapGet("/", async (AppDbContext context) =>
        {
            var truckSpecs = await context.TruckSpecifications
                .Include(ts => ts.Vehicle)
                .Select(ts => new TruckSpecificationResponseDto
                {
                    Id = ts.Id,
                    VehicleId = ts.VehicleId,
                    EngineType = ts.EngineType,
                    EngineCapacityLitres = ts.EngineCapacityLitres,
                    FuelType = ts.FuelType,
                    PowerKw = ts.PowerKw,
                    TorqueNm = ts.TorqueNm,
                    TransmissionType = ts.TransmissionType,
                    NumberOfGears = ts.NumberOfGears,
                    DriveConfiguration = ts.DriveConfiguration,
                    FuelTankCapacityLitres = ts.FuelTankCapacityLitres,
                    EmissionStandard = ts.EmissionStandard,
                    MaxTowingCapacityKg = ts.MaxTowingCapacityKg,
                    BrakeType = ts.BrakeType,
                    HasABS = ts.HasABS,
                    HasESC = ts.HasESC,
                    HasRetarder = ts.HasRetarder,
                    CabType = ts.CabType,
                    AdditionalFeatures = ts.AdditionalFeatures,
                    CreatedAt = ts.CreatedAt,
                    UpdatedAt = ts.UpdatedAt,
                    CreatedBy = ts.CreatedBy,
                    UpdatedBy = ts.UpdatedBy
                })
                .ToListAsync();

            return Results.Ok(truckSpecs);
        })
        .WithSummary("Get all truck specifications")
        .Produces<List<TruckSpecificationResponseDto>>();
    }
}
