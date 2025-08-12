using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class AxleEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/axle-groups")
                .WithTags("Axles")
                .WithOpenApi();

            // POST /api/axle-groups/{id}/axles - Add axle to axle group
            group.MapPost("/{id}/axles", async (AppDbContext db, int id, CreateAxleDto axleDto) =>
            {
                var axleGroup = await db.AxleGroups.FindAsync(id);
                if (axleGroup is null)
                {
                    return Results.NotFound("Axle group not found");
                }

                // Check if position is already taken within this axle group
                if (await db.Axles.AnyAsync(a => a.AxleGroupId == id && a.Position == axleDto.Position))
                {
                    return Results.BadRequest("Position already occupied by another axle in this group");
                }

                var axle = new Axle
                {
                    AxleGroupId = id,
                    TyreCount = axleDto.TyreCount,
                    LoadKg = axleDto.LoadKg,
                    Position = axleDto.Position,
                    TyreSize = axleDto.TyreSize,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.Axles.Add(axle);
                await db.SaveChangesAsync();

                var axleResponseDto = new AxleDto
                {
                    Id = axle.Id,
                    AxleGroupId = axle.AxleGroupId,
                    TyreCount = axle.TyreCount,
                    LoadKg = axle.LoadKg,
                    Position = axle.Position,
                    TyreSize = axle.TyreSize,
                    CreatedAt = axle.CreatedAt,
                    UpdatedAt = axle.UpdatedAt,
                    CreatedBy = axle.CreatedBy,
                    UpdatedBy = axle.UpdatedBy
                };

                return Results.Created($"/api/axle-groups/{id}/axles/{axle.Id}", axleResponseDto);
            })
            .WithName("AddAxle")
            .WithSummary("Add an axle to an axle group");

            // GET /api/axle-groups/{id}/axles - Get axles for axle group
            group.MapGet("/{id}/axles", async (AppDbContext db, int id) =>
            {
                var axleGroup = await db.AxleGroups.FindAsync(id);
                if (axleGroup is null)
                {
                    return Results.NotFound("Axle group not found");
                }

                var axles = await db.Axles
                    .Where(a => a.AxleGroupId == id)
                    .Select(a => new AxleDto
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
                    })
                    .OrderBy(a => a.Position)
                    .ToListAsync();

                return Results.Ok(axles);
            })
            .WithName("GetAxleGroupAxles")
            .WithSummary("Get all axles for an axle group");

            // GET /api/axle-groups/{axleGroupId}/axles/{axleId} - Get specific axle
            group.MapGet("/{axleGroupId}/axles/{axleId}", async (AppDbContext db, int axleGroupId, int axleId) =>
            {
                var axle = await db.Axles
                    .Where(a => a.Id == axleId && a.AxleGroupId == axleGroupId)
                    .Select(a => new AxleDto
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
                    })
                    .FirstOrDefaultAsync();

                return axle is not null ? Results.Ok(axle) : Results.NotFound("Axle not found");
            })
            .WithName("GetAxleById")
            .WithSummary("Get a specific axle by ID");

            // PUT /api/axle-groups/{axleGroupId}/axles/{axleId} - Update axle
            group.MapPut("/{axleGroupId}/axles/{axleId}", async (AppDbContext db, int axleGroupId, int axleId, CreateAxleDto updateDto) =>
            {
                var axle = await db.Axles.FirstOrDefaultAsync(a => a.Id == axleId && a.AxleGroupId == axleGroupId);
                if (axle is null)
                {
                    return Results.NotFound("Axle not found");
                }

                // Check if position is changing and if new position is already taken
                if (axle.Position != updateDto.Position && 
                    await db.Axles.AnyAsync(a => a.AxleGroupId == axleGroupId && a.Position == updateDto.Position && a.Id != axleId))
                {
                    return Results.BadRequest("Position already occupied by another axle in this group");
                }

                axle.TyreCount = updateDto.TyreCount;
                axle.LoadKg = updateDto.LoadKg;
                axle.Position = updateDto.Position;
                axle.TyreSize = updateDto.TyreSize;
                axle.UpdatedAt = DateTime.UtcNow;
                axle.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateAxle")
            .WithSummary("Update an existing axle");

            // DELETE /api/axle-groups/{axleGroupId}/axles/{axleId} - Delete axle
            group.MapDelete("/{axleGroupId}/axles/{axleId}", async (AppDbContext db, int axleGroupId, int axleId) =>
            {
                var axle = await db.Axles.FirstOrDefaultAsync(a => a.Id == axleId && a.AxleGroupId == axleGroupId);
                if (axle is null)
                {
                    return Results.NotFound("Axle not found");
                }

                db.Axles.Remove(axle);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteAxle")
            .WithSummary("Delete an axle");

            // GET /api/axle-groups/{id} - Get axle group by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var axleGroup = await db.AxleGroups
                    .Include(ag => ag.Axles)
                    .Include(ag => ag.Component)
                    .ThenInclude(c => c.ComponentType)
                    .Where(ag => ag.Id == id)
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
                    .FirstOrDefaultAsync();

                return axleGroup is not null ? Results.Ok(axleGroup) : Results.NotFound("Axle group not found");
            })
            .WithName("GetAxleGroupById")
            .WithSummary("Get an axle group by ID");

            // PUT /api/axle-groups/{id} - Update axle group
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateAxleGroupDto updateDto) =>
            {
                var axleGroup = await db.AxleGroups.FindAsync(id);
                if (axleGroup is null)
                {
                    return Results.NotFound("Axle group not found");
                }

                // Check if position is changing and if new position is already taken
                if (axleGroup.Position != updateDto.Position && 
                    await db.AxleGroups.AnyAsync(ag => ag.ComponentId == axleGroup.ComponentId && ag.Position == updateDto.Position && ag.Id != id))
                {
                    return Results.BadRequest("Position already occupied by another axle group on this component");
                }

                axleGroup.Name = updateDto.Name;
                axleGroup.SpacingMm = updateDto.SpacingMm;
                axleGroup.UnladenMass = updateDto.UnladenMass;
                axleGroup.Position = updateDto.Position;
                axleGroup.UpdatedAt = DateTime.UtcNow;
                axleGroup.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateAxleGroup")
            .WithSummary("Update an existing axle group");

            // DELETE /api/axle-groups/{id} - Delete axle group
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var axleGroup = await db.AxleGroups.Include(ag => ag.Axles).FirstOrDefaultAsync(ag => ag.Id == id);
                if (axleGroup is null)
                {
                    return Results.NotFound("Axle group not found");
                }

                // Remove all axles first
                if (axleGroup.Axles.Any())
                {
                    db.Axles.RemoveRange(axleGroup.Axles);
                }

                // Remove the axle group
                db.AxleGroups.Remove(axleGroup);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteAxleGroup")
            .WithSummary("Delete an axle group and all its axles");
        }
    }
}
