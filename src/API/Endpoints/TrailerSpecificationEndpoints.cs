using AVPermitSystemV2.Application.DTOs;
using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints;

public class TrailerSpecificationEndpoints : IEndpoint
{
    public void RegisterEndpoints(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/trailer-specifications")
            .WithTags("Trailer Specifications")
            .WithOpenApi();

        // Create trailer specification
        group.MapPost("/", async (TrailerSpecificationCreateDto dto, AppDbContext context) =>
        {
            // Validate vehicle exists
            var vehicleExists = await context.Vehicles.AnyAsync(v => v.Id == dto.VehicleId);
            if (!vehicleExists)
            {
                return Results.BadRequest($"Vehicle with ID {dto.VehicleId} not found");
            }

            // Check if trailer specification already exists for this vehicle
            var existingSpec = await context.TrailerSpecifications
                .FirstOrDefaultAsync(ts => ts.VehicleId == dto.VehicleId);
            if (existingSpec != null)
            {
                return Results.Conflict("Trailer specification already exists for this vehicle");
            }

            var trailerSpec = new TrailerSpecification
            {
                VehicleId = dto.VehicleId,
                TrailerType = dto.TrailerType,
                DeckLengthMm = dto.DeckLengthMm,
                DeckWidthMm = dto.DeckWidthMm,
                DeckHeightFromGroundMm = dto.DeckHeightFromGroundMm,
                LoadingRampLengthMm = dto.LoadingRampLengthMm,
                LoadingRampWidthMm = dto.LoadingRampWidthMm,
                NumberOfAxles = dto.NumberOfAxles,
                AxleSpacingMm = dto.AxleSpacingMm,
                SuspensionType = dto.SuspensionType,
                BrakeType = dto.BrakeType,
                HasABS = dto.HasABS,
                TyreSize = dto.TyreSize,
                NumberOfTyres = dto.NumberOfTyres,
                CouplingType = dto.CouplingType,
                KingpinToRearAxleMm = dto.KingpinToRearAxleMm,
                HasTieDownPoints = dto.HasTieDownPoints,
                NumberOfTieDownPoints = dto.NumberOfTieDownPoints,
                SideBoardType = dto.SideBoardType,
                HasTarps = dto.HasTarps,
                HasWinch = dto.HasWinch,
                FloorMaterial = dto.FloorMaterial,
                FloorThicknessMm = dto.FloorThicknessMm,
                SpecialFeatures = dto.SpecialFeatures,
                LoadingInstructions = dto.LoadingInstructions,
                CreatedBy = dto.CreatedBy,
                CreatedAt = DateTime.UtcNow
            };

            context.TrailerSpecifications.Add(trailerSpec);
            await context.SaveChangesAsync();

            var responseDto = new TrailerSpecificationResponseDto
            {
                Id = trailerSpec.Id,
                VehicleId = trailerSpec.VehicleId,
                TrailerType = trailerSpec.TrailerType,
                DeckLengthMm = trailerSpec.DeckLengthMm,
                DeckWidthMm = trailerSpec.DeckWidthMm,
                DeckHeightFromGroundMm = trailerSpec.DeckHeightFromGroundMm,
                LoadingRampLengthMm = trailerSpec.LoadingRampLengthMm,
                LoadingRampWidthMm = trailerSpec.LoadingRampWidthMm,
                NumberOfAxles = trailerSpec.NumberOfAxles,
                AxleSpacingMm = trailerSpec.AxleSpacingMm,
                SuspensionType = trailerSpec.SuspensionType,
                BrakeType = trailerSpec.BrakeType,
                HasABS = trailerSpec.HasABS,
                TyreSize = trailerSpec.TyreSize,
                NumberOfTyres = trailerSpec.NumberOfTyres,
                CouplingType = trailerSpec.CouplingType,
                KingpinToRearAxleMm = trailerSpec.KingpinToRearAxleMm,
                HasTieDownPoints = trailerSpec.HasTieDownPoints,
                NumberOfTieDownPoints = trailerSpec.NumberOfTieDownPoints,
                SideBoardType = trailerSpec.SideBoardType,
                HasTarps = trailerSpec.HasTarps,
                HasWinch = trailerSpec.HasWinch,
                FloorMaterial = trailerSpec.FloorMaterial,
                FloorThicknessMm = trailerSpec.FloorThicknessMm,
                SpecialFeatures = trailerSpec.SpecialFeatures,
                LoadingInstructions = trailerSpec.LoadingInstructions,
                CreatedAt = trailerSpec.CreatedAt,
                CreatedBy = trailerSpec.CreatedBy
            };

            return Results.Created($"/api/trailer-specifications/{trailerSpec.Id}", responseDto);
        })
        .WithSummary("Create trailer specification")
        .Produces<TrailerSpecificationResponseDto>(201)
        .ProducesValidationProblem()
        .ProducesProblem(400)
        .ProducesProblem(409);

        // Get trailer specification by vehicle ID
        group.MapGet("/vehicle/{vehicleId:int}", async (int vehicleId, AppDbContext context) =>
        {
            var trailerSpec = await context.TrailerSpecifications
                .FirstOrDefaultAsync(ts => ts.VehicleId == vehicleId);

            if (trailerSpec == null)
            {
                return Results.NotFound($"Trailer specification not found for vehicle {vehicleId}");
            }

            var responseDto = new TrailerSpecificationResponseDto
            {
                Id = trailerSpec.Id,
                VehicleId = trailerSpec.VehicleId,
                TrailerType = trailerSpec.TrailerType,
                DeckLengthMm = trailerSpec.DeckLengthMm,
                DeckWidthMm = trailerSpec.DeckWidthMm,
                DeckHeightFromGroundMm = trailerSpec.DeckHeightFromGroundMm,
                LoadingRampLengthMm = trailerSpec.LoadingRampLengthMm,
                LoadingRampWidthMm = trailerSpec.LoadingRampWidthMm,
                NumberOfAxles = trailerSpec.NumberOfAxles,
                AxleSpacingMm = trailerSpec.AxleSpacingMm,
                SuspensionType = trailerSpec.SuspensionType,
                BrakeType = trailerSpec.BrakeType,
                HasABS = trailerSpec.HasABS,
                TyreSize = trailerSpec.TyreSize,
                NumberOfTyres = trailerSpec.NumberOfTyres,
                CouplingType = trailerSpec.CouplingType,
                KingpinToRearAxleMm = trailerSpec.KingpinToRearAxleMm,
                HasTieDownPoints = trailerSpec.HasTieDownPoints,
                NumberOfTieDownPoints = trailerSpec.NumberOfTieDownPoints,
                SideBoardType = trailerSpec.SideBoardType,
                HasTarps = trailerSpec.HasTarps,
                HasWinch = trailerSpec.HasWinch,
                FloorMaterial = trailerSpec.FloorMaterial,
                FloorThicknessMm = trailerSpec.FloorThicknessMm,
                SpecialFeatures = trailerSpec.SpecialFeatures,
                LoadingInstructions = trailerSpec.LoadingInstructions,
                CreatedAt = trailerSpec.CreatedAt,
                UpdatedAt = trailerSpec.UpdatedAt,
                CreatedBy = trailerSpec.CreatedBy,
                UpdatedBy = trailerSpec.UpdatedBy
            };

            return Results.Ok(responseDto);
        })
        .WithSummary("Get trailer specification by vehicle ID")
        .Produces<TrailerSpecificationResponseDto>()
        .ProducesProblem(404);

        // Update trailer specification
        group.MapPut("/{id:int}", async (int id, TrailerSpecificationCreateDto dto, AppDbContext context) =>
        {
            var trailerSpec = await context.TrailerSpecifications.FindAsync(id);
            if (trailerSpec == null)
            {
                return Results.NotFound($"Trailer specification {id} not found");
            }

            // Update properties
            trailerSpec.TrailerType = dto.TrailerType;
            trailerSpec.DeckLengthMm = dto.DeckLengthMm;
            trailerSpec.DeckWidthMm = dto.DeckWidthMm;
            trailerSpec.DeckHeightFromGroundMm = dto.DeckHeightFromGroundMm;
            trailerSpec.LoadingRampLengthMm = dto.LoadingRampLengthMm;
            trailerSpec.LoadingRampWidthMm = dto.LoadingRampWidthMm;
            trailerSpec.NumberOfAxles = dto.NumberOfAxles;
            trailerSpec.AxleSpacingMm = dto.AxleSpacingMm;
            trailerSpec.SuspensionType = dto.SuspensionType;
            trailerSpec.BrakeType = dto.BrakeType;
            trailerSpec.HasABS = dto.HasABS;
            trailerSpec.TyreSize = dto.TyreSize;
            trailerSpec.NumberOfTyres = dto.NumberOfTyres;
            trailerSpec.CouplingType = dto.CouplingType;
            trailerSpec.KingpinToRearAxleMm = dto.KingpinToRearAxleMm;
            trailerSpec.HasTieDownPoints = dto.HasTieDownPoints;
            trailerSpec.NumberOfTieDownPoints = dto.NumberOfTieDownPoints;
            trailerSpec.SideBoardType = dto.SideBoardType;
            trailerSpec.HasTarps = dto.HasTarps;
            trailerSpec.HasWinch = dto.HasWinch;
            trailerSpec.FloorMaterial = dto.FloorMaterial;
            trailerSpec.FloorThicknessMm = dto.FloorThicknessMm;
            trailerSpec.SpecialFeatures = dto.SpecialFeatures;
            trailerSpec.LoadingInstructions = dto.LoadingInstructions;
            trailerSpec.UpdatedAt = DateTime.UtcNow;
            trailerSpec.UpdatedBy = dto.CreatedBy; // Reusing CreatedBy for UpdatedBy

            await context.SaveChangesAsync();

            var responseDto = new TrailerSpecificationResponseDto
            {
                Id = trailerSpec.Id,
                VehicleId = trailerSpec.VehicleId,
                TrailerType = trailerSpec.TrailerType,
                DeckLengthMm = trailerSpec.DeckLengthMm,
                DeckWidthMm = trailerSpec.DeckWidthMm,
                DeckHeightFromGroundMm = trailerSpec.DeckHeightFromGroundMm,
                LoadingRampLengthMm = trailerSpec.LoadingRampLengthMm,
                LoadingRampWidthMm = trailerSpec.LoadingRampWidthMm,
                NumberOfAxles = trailerSpec.NumberOfAxles,
                AxleSpacingMm = trailerSpec.AxleSpacingMm,
                SuspensionType = trailerSpec.SuspensionType,
                BrakeType = trailerSpec.BrakeType,
                HasABS = trailerSpec.HasABS,
                TyreSize = trailerSpec.TyreSize,
                NumberOfTyres = trailerSpec.NumberOfTyres,
                CouplingType = trailerSpec.CouplingType,
                KingpinToRearAxleMm = trailerSpec.KingpinToRearAxleMm,
                HasTieDownPoints = trailerSpec.HasTieDownPoints,
                NumberOfTieDownPoints = trailerSpec.NumberOfTieDownPoints,
                SideBoardType = trailerSpec.SideBoardType,
                HasTarps = trailerSpec.HasTarps,
                HasWinch = trailerSpec.HasWinch,
                FloorMaterial = trailerSpec.FloorMaterial,
                FloorThicknessMm = trailerSpec.FloorThicknessMm,
                SpecialFeatures = trailerSpec.SpecialFeatures,
                LoadingInstructions = trailerSpec.LoadingInstructions,
                CreatedAt = trailerSpec.CreatedAt,
                UpdatedAt = trailerSpec.UpdatedAt,
                CreatedBy = trailerSpec.CreatedBy,
                UpdatedBy = trailerSpec.UpdatedBy
            };

            return Results.Ok(responseDto);
        })
        .WithSummary("Update trailer specification")
        .Produces<TrailerSpecificationResponseDto>()
        .ProducesValidationProblem()
        .ProducesProblem(404);

        // Delete trailer specification
        group.MapDelete("/{id:int}", async (int id, AppDbContext context) =>
        {
            var trailerSpec = await context.TrailerSpecifications.FindAsync(id);
            if (trailerSpec == null)
            {
                return Results.NotFound($"Trailer specification {id} not found");
            }

            context.TrailerSpecifications.Remove(trailerSpec);
            await context.SaveChangesAsync();

            return Results.NoContent();
        })
        .WithSummary("Delete trailer specification")
        .Produces(204)
        .ProducesProblem(404);

        // Get all trailer specifications
        group.MapGet("/", async (AppDbContext context) =>
        {
            var trailerSpecs = await context.TrailerSpecifications
                .Include(ts => ts.Vehicle)
                .Select(ts => new TrailerSpecificationResponseDto
                {
                    Id = ts.Id,
                    VehicleId = ts.VehicleId,
                    TrailerType = ts.TrailerType,
                    DeckLengthMm = ts.DeckLengthMm,
                    DeckWidthMm = ts.DeckWidthMm,
                    DeckHeightFromGroundMm = ts.DeckHeightFromGroundMm,
                    LoadingRampLengthMm = ts.LoadingRampLengthMm,
                    LoadingRampWidthMm = ts.LoadingRampWidthMm,
                    NumberOfAxles = ts.NumberOfAxles,
                    AxleSpacingMm = ts.AxleSpacingMm,
                    SuspensionType = ts.SuspensionType,
                    BrakeType = ts.BrakeType,
                    HasABS = ts.HasABS,
                    TyreSize = ts.TyreSize,
                    NumberOfTyres = ts.NumberOfTyres,
                    CouplingType = ts.CouplingType,
                    KingpinToRearAxleMm = ts.KingpinToRearAxleMm,
                    HasTieDownPoints = ts.HasTieDownPoints,
                    NumberOfTieDownPoints = ts.NumberOfTieDownPoints,
                    SideBoardType = ts.SideBoardType,
                    HasTarps = ts.HasTarps,
                    HasWinch = ts.HasWinch,
                    FloorMaterial = ts.FloorMaterial,
                    FloorThicknessMm = ts.FloorThicknessMm,
                    SpecialFeatures = ts.SpecialFeatures,
                    LoadingInstructions = ts.LoadingInstructions,
                    CreatedAt = ts.CreatedAt,
                    UpdatedAt = ts.UpdatedAt,
                    CreatedBy = ts.CreatedBy,
                    UpdatedBy = ts.UpdatedBy
                })
                .ToListAsync();

            return Results.Ok(trailerSpecs);
        })
        .WithSummary("Get all trailer specifications")
        .Produces<List<TrailerSpecificationResponseDto>>();
    }
}
