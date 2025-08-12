using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class LoadEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/loads")
                .WithTags("Loads")
                .WithOpenApi();

            // GET /api/loads/{id} - Get load by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var load = await db.Loads
                    .Include(l => l.Dimensions)
                    .Include(l => l.Projections)
                    .Include(l => l.Permit)
                    .Where(l => l.Id == id)
                    .Select(l => new LoadDto
                    {
                        Id = l.Id,
                        PermitId = l.PermitId,
                        LoadType = l.LoadType,
                        LoadTypeName = l.LoadType.ToString(),
                        WeightKg = l.WeightKg,
                        Description = l.Description,
                        CargoType = l.CargoType,
                        IsIndivisible = l.IsIndivisible,
                        CreatedAt = l.CreatedAt,
                        UpdatedAt = l.UpdatedAt,
                        CreatedBy = l.CreatedBy,
                        UpdatedBy = l.UpdatedBy,
                        Dimensions = l.Dimensions.Select(d => new LoadDimensionDto
                        {
                            Id = d.Id,
                            LoadId = d.LoadId,
                            LengthMm = d.LengthMm,
                            WidthMm = d.WidthMm,
                            HeightMm = d.HeightMm,
                            Unit = d.Unit
                        }).ToList(),
                        Projections = l.Projections.Select(p => new LoadProjectionDto
                        {
                            Id = p.Id,
                            LoadId = p.LoadId,
                            Direction = p.Direction,
                            ProjectionMm = p.ProjectionMm,
                            Unit = p.Unit
                        }).ToList()
                    })
                    .FirstOrDefaultAsync();

                return load is not null ? Results.Ok(load) : Results.NotFound();
            })
            .WithName("GetLoadById")
            .WithSummary("Get load by ID");

            // PUT /api/loads/{id} - Update load
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateLoadDto updateDto) =>
            {
                var load = await db.Loads.FindAsync(id);
                if (load is null)
                {
                    return Results.NotFound();
                }

                load.LoadType = updateDto.LoadType;
                load.WeightKg = updateDto.WeightKg;
                load.Description = updateDto.Description;
                load.CargoType = updateDto.CargoType;
                load.IsIndivisible = updateDto.IsIndivisible;
                load.UpdatedAt = DateTime.UtcNow;
                load.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateLoad")
            .WithSummary("Update an existing load");

            // DELETE /api/loads/{id} - Delete load
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var load = await db.Loads
                    .Include(l => l.Dimensions)
                    .Include(l => l.Projections)
                    .FirstOrDefaultAsync(l => l.Id == id);
                
                if (load is null)
                {
                    return Results.NotFound();
                }

                // Remove dimensions and projections first
                if (load.Dimensions.Any())
                {
                    db.LoadDimensions.RemoveRange(load.Dimensions);
                }
                if (load.Projections.Any())
                {
                    db.LoadProjections.RemoveRange(load.Projections);
                }

                // Remove the load
                db.Loads.Remove(load);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteLoad")
            .WithSummary("Delete a load and all its dimensions and projections");

            // POST /api/loads/{id}/dimensions - Add dimensions to load
            group.MapPost("/{id}/dimensions", async (AppDbContext db, int id, CreateLoadDimensionDto dimensionDto) =>
            {
                var load = await db.Loads.FindAsync(id);
                if (load is null)
                {
                    return Results.NotFound("Load not found");
                }

                // Check if load already has dimensions
                if (await db.LoadDimensions.AnyAsync(ld => ld.LoadId == id))
                {
                    return Results.BadRequest("Load already has dimensions defined");
                }

                var dimension = new LoadDimension
                {
                    LoadId = id,
                    LengthMm = dimensionDto.LengthMm,
                    WidthMm = dimensionDto.WidthMm,
                    HeightMm = dimensionDto.HeightMm,
                    Unit = dimensionDto.Unit
                };

                db.LoadDimensions.Add(dimension);
                await db.SaveChangesAsync();

                var dimensionResponseDto = new LoadDimensionDto
                {
                    Id = dimension.Id,
                    LoadId = dimension.LoadId,
                    LengthMm = dimension.LengthMm,
                    WidthMm = dimension.WidthMm,
                    HeightMm = dimension.HeightMm,
                    Unit = dimension.Unit
                };

                return Results.Created($"/api/loads/{id}/dimensions", dimensionResponseDto);
            })
            .WithName("AddLoadDimensions")
            .WithSummary("Add dimensions to a load");

            // GET /api/loads/{id}/dimensions - Get load dimensions
            group.MapGet("/{id}/dimensions", async (AppDbContext db, int id) =>
            {
                var load = await db.Loads.FindAsync(id);
                if (load is null)
                {
                    return Results.NotFound("Load not found");
                }

                var dimensions = await db.LoadDimensions
                    .Where(ld => ld.LoadId == id)
                    .Select(ld => new LoadDimensionDto
                    {
                        Id = ld.Id,
                        LoadId = ld.LoadId,
                        LengthMm = ld.LengthMm,
                        WidthMm = ld.WidthMm,
                        HeightMm = ld.HeightMm,
                        Unit = ld.Unit
                    })
                    .FirstOrDefaultAsync();

                return dimensions is not null ? Results.Ok(dimensions) : Results.NotFound("Dimensions not found");
            })
            .WithName("GetLoadDimensions")
            .WithSummary("Get dimensions for a load");

            // POST /api/loads/{id}/projections - Add projection to load
            group.MapPost("/{id}/projections", async (AppDbContext db, int id, CreateLoadProjectionDto projectionDto) =>
            {
                var load = await db.Loads.FindAsync(id);
                if (load is null)
                {
                    return Results.NotFound("Load not found");
                }

                // Validate direction
                var validDirections = new[] { "Front", "Rear", "Left", "Right" };
                if (!validDirections.Contains(projectionDto.Direction, StringComparer.OrdinalIgnoreCase))
                {
                    return Results.BadRequest("Invalid direction. Must be Front, Rear, Left, or Right");
                }

                // Check if projection for this direction already exists
                if (await db.LoadProjections.AnyAsync(lp => lp.LoadId == id && 
                    lp.Direction.ToLower() == projectionDto.Direction.ToLower()))
                {
                    return Results.BadRequest($"Projection for {projectionDto.Direction} direction already exists");
                }

                var projection = new LoadProjection
                {
                    LoadId = id,
                    Direction = projectionDto.Direction,
                    ProjectionMm = projectionDto.ProjectionMm,
                    Unit = projectionDto.Unit
                };

                db.LoadProjections.Add(projection);
                await db.SaveChangesAsync();

                var projectionResponseDto = new LoadProjectionDto
                {
                    Id = projection.Id,
                    LoadId = projection.LoadId,
                    Direction = projection.Direction,
                    ProjectionMm = projection.ProjectionMm,
                    Unit = projection.Unit
                };

                return Results.Created($"/api/loads/{id}/projections/{projection.Id}", projectionResponseDto);
            })
            .WithName("AddLoadProjection")
            .WithSummary("Add a projection to a load");

            // GET /api/loads/{id}/projections - Get load projections
            group.MapGet("/{id}/projections", async (AppDbContext db, int id) =>
            {
                var load = await db.Loads.FindAsync(id);
                if (load is null)
                {
                    return Results.NotFound("Load not found");
                }

                var projections = await db.LoadProjections
                    .Where(lp => lp.LoadId == id)
                    .Select(lp => new LoadProjectionDto
                    {
                        Id = lp.Id,
                        LoadId = lp.LoadId,
                        Direction = lp.Direction,
                        ProjectionMm = lp.ProjectionMm,
                        Unit = lp.Unit
                    })
                    .ToListAsync();

                return Results.Ok(projections);
            })
            .WithName("GetLoadProjections")
            .WithSummary("Get all projections for a load");

            // PUT /api/loads/{loadId}/projections/{projectionId} - Update projection
            group.MapPut("/{loadId}/projections/{projectionId}", async (AppDbContext db, int loadId, int projectionId, CreateLoadProjectionDto updateDto) =>
            {
                var projection = await db.LoadProjections.FirstOrDefaultAsync(lp => lp.Id == projectionId && lp.LoadId == loadId);
                if (projection is null)
                {
                    return Results.NotFound("Projection not found");
                }

                // Validate direction
                var validDirections = new[] { "Front", "Rear", "Left", "Right" };
                if (!validDirections.Contains(updateDto.Direction, StringComparer.OrdinalIgnoreCase))
                {
                    return Results.BadRequest("Invalid direction. Must be Front, Rear, Left, or Right");
                }

                projection.Direction = updateDto.Direction;
                projection.ProjectionMm = updateDto.ProjectionMm;
                projection.Unit = updateDto.Unit;

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateLoadProjection")
            .WithSummary("Update a load projection");

            // DELETE /api/loads/{loadId}/projections/{projectionId} - Delete projection
            group.MapDelete("/{loadId}/projections/{projectionId}", async (AppDbContext db, int loadId, int projectionId) =>
            {
                var projection = await db.LoadProjections.FirstOrDefaultAsync(lp => lp.Id == projectionId && lp.LoadId == loadId);
                if (projection is null)
                {
                    return Results.NotFound("Projection not found");
                }

                db.LoadProjections.Remove(projection);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteLoadProjection")
            .WithSummary("Delete a load projection");
        }
    }
}
