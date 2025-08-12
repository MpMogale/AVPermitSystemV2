using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class ComponentEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/components")
                .WithTags("Components")
                .WithOpenApi();

            // POST /api/components/{id}/dimensions - Add dimensions to component
            group.MapPost("/{id}/dimensions", async (AppDbContext db, int id, CreateComponentDimensionDto dimensionDto) =>
            {
                var component = await db.VehicleComponents.FindAsync(id);
                if (component is null)
                {
                    return Results.NotFound("Component not found");
                }

                // Check if component already has dimensions
                if (await db.ComponentDimensions.AnyAsync(cd => cd.ComponentId == id))
                {
                    return Results.BadRequest("Component already has dimensions defined");
                }

                var dimension = new ComponentDimension
                {
                    ComponentId = id,
                    LengthMm = dimensionDto.LengthMm,
                    WidthMm = dimensionDto.WidthMm,
                    HeightMm = dimensionDto.HeightMm,
                    FrontOverhangMm = dimensionDto.FrontOverhangMm,
                    RearOverhangMm = dimensionDto.RearOverhangMm,
                    LeftOverhangMm = dimensionDto.LeftOverhangMm,
                    RightOverhangMm = dimensionDto.RightOverhangMm,
                    Unit = dimensionDto.Unit,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.ComponentDimensions.Add(dimension);
                await db.SaveChangesAsync();

                var dimensionResponseDto = new ComponentDimensionDto
                {
                    Id = dimension.Id,
                    ComponentId = dimension.ComponentId,
                    LengthMm = dimension.LengthMm,
                    WidthMm = dimension.WidthMm,
                    HeightMm = dimension.HeightMm,
                    FrontOverhangMm = dimension.FrontOverhangMm,
                    RearOverhangMm = dimension.RearOverhangMm,
                    LeftOverhangMm = dimension.LeftOverhangMm,
                    RightOverhangMm = dimension.RightOverhangMm,
                    Unit = dimension.Unit,
                    CreatedAt = dimension.CreatedAt
                };

                return Results.Created($"/api/components/{id}/dimensions", dimensionResponseDto);
            })
            .WithName("AddComponentDimensions")
            .WithSummary("Add dimensions to a component");

            // GET /api/components/{id}/dimensions - Get component dimensions
            group.MapGet("/{id}/dimensions", async (AppDbContext db, int id) =>
            {
                var component = await db.VehicleComponents.FindAsync(id);
                if (component is null)
                {
                    return Results.NotFound("Component not found");
                }

                var dimensions = await db.ComponentDimensions
                    .Where(cd => cd.ComponentId == id)
                    .Select(cd => new ComponentDimensionDto
                    {
                        Id = cd.Id,
                        ComponentId = cd.ComponentId,
                        LengthMm = cd.LengthMm,
                        WidthMm = cd.WidthMm,
                        HeightMm = cd.HeightMm,
                        FrontOverhangMm = cd.FrontOverhangMm,
                        RearOverhangMm = cd.RearOverhangMm,
                        LeftOverhangMm = cd.LeftOverhangMm,
                        RightOverhangMm = cd.RightOverhangMm,
                        Unit = cd.Unit,
                        CreatedAt = cd.CreatedAt
                    })
                    .FirstOrDefaultAsync();

                return dimensions is not null ? Results.Ok(dimensions) : Results.NotFound("Dimensions not found");
            })
            .WithName("GetComponentDimensions")
            .WithSummary("Get dimensions for a component");

            // PUT /api/components/{id}/dimensions - Update component dimensions
            group.MapPut("/{id}/dimensions", async (AppDbContext db, int id, CreateComponentDimensionDto updateDto) =>
            {
                var component = await db.VehicleComponents.FindAsync(id);
                if (component is null)
                {
                    return Results.NotFound("Component not found");
                }

                var dimensions = await db.ComponentDimensions.FirstOrDefaultAsync(cd => cd.ComponentId == id);
                if (dimensions is null)
                {
                    return Results.NotFound("Dimensions not found");
                }

                dimensions.LengthMm = updateDto.LengthMm;
                dimensions.WidthMm = updateDto.WidthMm;
                dimensions.HeightMm = updateDto.HeightMm;
                dimensions.FrontOverhangMm = updateDto.FrontOverhangMm;
                dimensions.RearOverhangMm = updateDto.RearOverhangMm;
                dimensions.LeftOverhangMm = updateDto.LeftOverhangMm;
                dimensions.RightOverhangMm = updateDto.RightOverhangMm;
                dimensions.Unit = updateDto.Unit;
                dimensions.UpdatedAt = DateTime.UtcNow;
                dimensions.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateComponentDimensions")
            .WithSummary("Update dimensions for a component");

            // GET /api/components/types - Get all component types
            group.MapGet("/types", async (AppDbContext db) =>
            {
                var componentTypes = await db.ComponentTypes
                    .Where(ct => ct.IsActive)
                    .Select(ct => new
                    {
                        ct.Id,
                        ct.Name,
                        ct.Code,
                        ct.Description,
                        ct.IsActive
                    })
                    .ToListAsync();

                return Results.Ok(componentTypes);
            })
            .WithName("GetComponentTypes")
            .WithSummary("Get all active component types");

            // POST /api/components/{id}/axle-groups - Add axle group to component
            group.MapPost("/{id}/axle-groups", async (AppDbContext db, int id, CreateAxleGroupDto axleGroupDto) =>
            {
                var component = await db.VehicleComponents.FindAsync(id);
                if (component is null)
                {
                    return Results.NotFound("Component not found");
                }

                // Check if position is already taken
                if (await db.AxleGroups.AnyAsync(ag => ag.ComponentId == id && ag.Position == axleGroupDto.Position))
                {
                    return Results.BadRequest("Position already occupied by another axle group");
                }

                var axleGroup = new AxleGroup
                {
                    ComponentId = id,
                    Name = axleGroupDto.Name,
                    SpacingMm = axleGroupDto.SpacingMm,
                    UnladenMass = axleGroupDto.UnladenMass,
                    Position = axleGroupDto.Position,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.AxleGroups.Add(axleGroup);
                await db.SaveChangesAsync();

                var axleGroupResponseDto = new AxleGroupDto
                {
                    Id = axleGroup.Id,
                    ComponentId = axleGroup.ComponentId,
                    Name = axleGroup.Name,
                    SpacingMm = axleGroup.SpacingMm,
                    UnladenMass = axleGroup.UnladenMass,
                    Position = axleGroup.Position,
                    CreatedAt = axleGroup.CreatedAt,
                    UpdatedAt = axleGroup.UpdatedAt,
                    CreatedBy = axleGroup.CreatedBy,
                    UpdatedBy = axleGroup.UpdatedBy
                };

                return Results.Created($"/api/components/{id}/axle-groups/{axleGroup.Id}", axleGroupResponseDto);
            })
            .WithName("AddAxleGroup")
            .WithSummary("Add an axle group to a component");

            // GET /api/components/{id}/axle-groups - Get axle groups for component
            group.MapGet("/{id}/axle-groups", async (AppDbContext db, int id) =>
            {
                var component = await db.VehicleComponents.FindAsync(id);
                if (component is null)
                {
                    return Results.NotFound("Component not found");
                }

                var axleGroups = await db.AxleGroups
                    .Include(ag => ag.Axles)
                    .Where(ag => ag.ComponentId == id)
                    .Select(ag => new AxleGroupDto
                    {
                        Id = ag.Id,
                        ComponentId = ag.ComponentId,
                        Name = ag.Name,
                        SpacingMm = ag.SpacingMm,
                        UnladenMass = ag.UnladenMass,
                        Position = ag.Position,
                        CreatedAt = ag.CreatedAt,
                        UpdatedAt = ag.UpdatedAt,
                        CreatedBy = ag.CreatedBy,
                        UpdatedBy = ag.UpdatedBy,
                        Axles = ag.Axles.Select(a => new AxleDto
                        {
                            Id = a.Id,
                            AxleGroupId = a.AxleGroupId,
                            TyreCount = a.TyreCount,
                            LoadKg = a.LoadKg,
                            Position = a.Position,
                            TyreSize = a.TyreSize,
                            CreatedAt = a.CreatedAt,
                            UpdatedAt = a.UpdatedAt,
                            CreatedBy = a.CreatedBy,
                            UpdatedBy = a.UpdatedBy
                        }).OrderBy(a => a.Position).ToList()
                    })
                    .OrderBy(ag => ag.Position)
                    .ToListAsync();

                return Results.Ok(axleGroups);
            })
            .WithName("GetComponentAxleGroups")
            .WithSummary("Get all axle groups for a component");
        }
    }
}
