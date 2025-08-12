using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class VehicleOwnershipEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vehicle-ownerships")
                .WithTags("Vehicle Ownership")
                .WithOpenApi();

            // GET /api/vehicle-ownerships - List all vehicle ownerships
            group.MapGet("/", async (AppDbContext db, int? vehicleId, int? ownerId, bool activeOnly = false) =>
            {
                var query = db.VehicleOwnerships
                    .Include(vo => vo.Vehicle)
                    .Include(vo => vo.Owner)
                    .AsQueryable();

                if (vehicleId.HasValue)
                {
                    query = query.Where(vo => vo.VehicleId == vehicleId.Value);
                }

                if (ownerId.HasValue)
                {
                    query = query.Where(vo => vo.OwnerId == ownerId.Value);
                }

                if (activeOnly)
                {
                    query = query.Where(vo => !vo.EndDate.HasValue || vo.EndDate > DateTime.UtcNow);
                }

                var ownerships = await query
                    .Select(vo => new VehicleOwnershipDto
                    {
                        Id = vo.Id,
                        VehicleId = vo.VehicleId,
                        VehicleName = vo.Vehicle.Name,
                        VehicleVIN = vo.Vehicle.VIN,
                        OwnerId = vo.OwnerId,
                        OwnerName = vo.Owner.Name,
                        OwnerType = vo.Owner.OwnerType,
                        OwnerTypeName = vo.Owner.OwnerType.ToString(),
                        StartDate = vo.StartDate,
                        EndDate = vo.EndDate,
                        IsPrimaryOwner = vo.IsPrimaryOwner,
                        Notes = vo.Notes,
                        CreatedAt = vo.CreatedAt,
                        UpdatedAt = vo.UpdatedAt,
                        CreatedBy = vo.CreatedBy,
                        UpdatedBy = vo.UpdatedBy
                    })
                    .OrderByDescending(vo => vo.StartDate)
                    .ToListAsync();

                return Results.Ok(ownerships);
            })
            .WithName("GetVehicleOwnerships")
            .WithSummary("Get vehicle ownerships with optional filtering");

            // GET /api/vehicle-ownerships/{id} - Get vehicle ownership by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var ownership = await db.VehicleOwnerships
                    .Include(vo => vo.Vehicle)
                    .Include(vo => vo.Owner)
                    .Where(vo => vo.Id == id)
                    .Select(vo => new VehicleOwnershipDto
                    {
                        Id = vo.Id,
                        VehicleId = vo.VehicleId,
                        VehicleName = vo.Vehicle.Name,
                        VehicleVIN = vo.Vehicle.VIN,
                        OwnerId = vo.OwnerId,
                        OwnerName = vo.Owner.Name,
                        OwnerType = vo.Owner.OwnerType,
                        OwnerTypeName = vo.Owner.OwnerType.ToString(),
                        StartDate = vo.StartDate,
                        EndDate = vo.EndDate,
                        IsPrimaryOwner = vo.IsPrimaryOwner,
                        Notes = vo.Notes,
                        CreatedAt = vo.CreatedAt,
                        UpdatedAt = vo.UpdatedAt,
                        CreatedBy = vo.CreatedBy,
                        UpdatedBy = vo.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                return ownership is not null ? Results.Ok(ownership) : Results.NotFound();
            })
            .WithName("GetVehicleOwnershipById")
            .WithSummary("Get vehicle ownership by ID");

            // POST /api/vehicle-ownerships - Create new vehicle ownership
            group.MapPost("/", async (AppDbContext db, CreateVehicleOwnershipDto createDto) =>
            {
                // Validate vehicle exists
                if (!await db.Vehicles.AnyAsync(v => v.Id == createDto.VehicleId && v.IsActive))
                {
                    return Results.BadRequest("Invalid vehicle ID");
                }

                // Validate owner exists
                if (!await db.Owners.AnyAsync(o => o.Id == createDto.OwnerId && o.IsActive))
                {
                    return Results.BadRequest("Invalid owner ID");
                }

                // Validate date ranges
                if (createDto.EndDate.HasValue && createDto.EndDate <= createDto.StartDate)
                {
                    return Results.BadRequest("End date must be after start date");
                }

                // Check for overlapping date ranges for the same vehicle
                var hasOverlap = await db.VehicleOwnerships
                    .Where(vo => vo.VehicleId == createDto.VehicleId)
                    .AnyAsync(vo => 
                        (createDto.StartDate < (vo.EndDate ?? DateTime.MaxValue)) &&
                        ((createDto.EndDate ?? DateTime.MaxValue) > vo.StartDate));

                if (hasOverlap)
                {
                    return Results.BadRequest("Ownership period overlaps with existing ownership for this vehicle");
                }

                // If creating as primary owner, ensure no other active primary owner exists
                if (createDto.IsPrimaryOwner)
                {
                    var hasActivePrimaryOwner = await db.VehicleOwnerships
                        .AnyAsync(vo => vo.VehicleId == createDto.VehicleId && 
                                       vo.IsPrimaryOwner && 
                                       (!vo.EndDate.HasValue || vo.EndDate > createDto.StartDate));

                    if (hasActivePrimaryOwner)
                    {
                        return Results.BadRequest("Vehicle already has an active primary owner during this period");
                    }
                }

                var ownership = new VehicleOwnership
                {
                    VehicleId = createDto.VehicleId,
                    OwnerId = createDto.OwnerId,
                    StartDate = createDto.StartDate,
                    EndDate = createDto.EndDate,
                    IsPrimaryOwner = createDto.IsPrimaryOwner,
                    Notes = createDto.Notes,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.VehicleOwnerships.Add(ownership);
                await db.SaveChangesAsync();

                // Return the created ownership with related data
                var createdOwnership = await db.VehicleOwnerships
                    .Include(vo => vo.Vehicle)
                    .Include(vo => vo.Owner)
                    .Where(vo => vo.Id == ownership.Id)
                    .Select(vo => new VehicleOwnershipDto
                    {
                        Id = vo.Id,
                        VehicleId = vo.VehicleId,
                        VehicleName = vo.Vehicle.Name,
                        VehicleVIN = vo.Vehicle.VIN,
                        OwnerId = vo.OwnerId,
                        OwnerName = vo.Owner.Name,
                        OwnerType = vo.Owner.OwnerType,
                        OwnerTypeName = vo.Owner.OwnerType.ToString(),
                        StartDate = vo.StartDate,
                        EndDate = vo.EndDate,
                        IsPrimaryOwner = vo.IsPrimaryOwner,
                        Notes = vo.Notes,
                        CreatedAt = vo.CreatedAt,
                        UpdatedAt = vo.UpdatedAt,
                        CreatedBy = vo.CreatedBy,
                        UpdatedBy = vo.UpdatedBy
                    })
                    .FirstAsync();

                return Results.Created($"/api/vehicle-ownerships/{ownership.Id}", createdOwnership);
            })
            .WithName("CreateVehicleOwnership")
            .WithSummary("Create a new vehicle ownership");

            // PUT /api/vehicle-ownerships/{id} - Update vehicle ownership
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateVehicleOwnershipDto updateDto) =>
            {
                var ownership = await db.VehicleOwnerships.FindAsync(id);
                if (ownership is null)
                {
                    return Results.NotFound();
                }

                // Validate date ranges
                if (updateDto.EndDate.HasValue && updateDto.EndDate <= updateDto.StartDate)
                {
                    return Results.BadRequest("End date must be after start date");
                }

                // Check for overlapping date ranges (excluding current record)
                var hasOverlap = await db.VehicleOwnerships
                    .Where(vo => vo.VehicleId == updateDto.VehicleId && vo.Id != id)
                    .AnyAsync(vo => 
                        (updateDto.StartDate < (vo.EndDate ?? DateTime.MaxValue)) &&
                        ((updateDto.EndDate ?? DateTime.MaxValue) > vo.StartDate));

                if (hasOverlap)
                {
                    return Results.BadRequest("Ownership period overlaps with existing ownership for this vehicle");
                }

                ownership.VehicleId = updateDto.VehicleId;
                ownership.OwnerId = updateDto.OwnerId;
                ownership.StartDate = updateDto.StartDate;
                ownership.EndDate = updateDto.EndDate;
                ownership.IsPrimaryOwner = updateDto.IsPrimaryOwner;
                ownership.Notes = updateDto.Notes;
                ownership.UpdatedAt = DateTime.UtcNow;
                ownership.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateVehicleOwnership")
            .WithSummary("Update an existing vehicle ownership");

            // DELETE /api/vehicle-ownerships/{id} - Delete vehicle ownership
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var ownership = await db.VehicleOwnerships.FindAsync(id);
                if (ownership is null)
                {
                    return Results.NotFound();
                }

                db.VehicleOwnerships.Remove(ownership);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteVehicleOwnership")
            .WithSummary("Delete a vehicle ownership");

            // PATCH /api/vehicle-ownerships/{id}/end - End vehicle ownership
            group.MapPatch("/{id}/end", async (AppDbContext db, int id, DateTime? endDate = null) =>
            {
                var ownership = await db.VehicleOwnerships.FindAsync(id);
                if (ownership is null)
                {
                    return Results.NotFound();
                }

                if (ownership.EndDate.HasValue)
                {
                    return Results.BadRequest("Ownership has already ended");
                }

                var actualEndDate = endDate ?? DateTime.UtcNow;

                if (actualEndDate <= ownership.StartDate)
                {
                    return Results.BadRequest("End date must be after start date");
                }

                ownership.EndDate = actualEndDate;
                ownership.UpdatedAt = DateTime.UtcNow;
                ownership.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("EndVehicleOwnership")
            .WithSummary("End a vehicle ownership");
        }
    }
}
