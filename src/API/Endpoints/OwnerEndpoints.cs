using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class OwnerEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/owners")
                .WithTags("Owners")
                .WithOpenApi();

            // GET /api/owners - List all owners
            group.MapGet("/", async (AppDbContext db) =>
            {
                var owners = await db.Owners
                    .Where(o => o.IsActive)
                    .Select(o => new OwnerDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        OwnerType = o.OwnerType,
                        OwnerTypeName = o.OwnerType.ToString(),
                        RegistrationNumber = o.RegistrationNumber,
                        Address = o.Address,
                        ContactPhone = o.ContactPhone,
                        ContactEmail = o.ContactEmail,
                        IsActive = o.IsActive,
                        CreatedAt = o.CreatedAt,
                        UpdatedAt = o.UpdatedAt,
                        CreatedBy = o.CreatedBy,
                        UpdatedBy = o.UpdatedBy
                    })
                    .ToListAsync();

                return Results.Ok(owners);
            })
            .WithName("GetOwners")
            .WithSummary("Get all active owners");

            // GET /api/owners/{id} - Get owner by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var owner = await db.Owners
                    .Where(o => o.Id == id)
                    .Select(o => new OwnerDto
                    {
                        Id = o.Id,
                        Name = o.Name,
                        OwnerType = o.OwnerType,
                        OwnerTypeName = o.OwnerType.ToString(),
                        RegistrationNumber = o.RegistrationNumber,
                        Address = o.Address,
                        ContactPhone = o.ContactPhone,
                        ContactEmail = o.ContactEmail,
                        IsActive = o.IsActive,
                        CreatedAt = o.CreatedAt,
                        UpdatedAt = o.UpdatedAt,
                        CreatedBy = o.CreatedBy,
                        UpdatedBy = o.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                return owner is not null ? Results.Ok(owner) : Results.NotFound();
            })
            .WithName("GetOwnerById")
            .WithSummary("Get owner by ID");

            // POST /api/owners - Create new owner
            group.MapPost("/", async (AppDbContext db, CreateOwnerDto createDto) =>
            {
                var owner = new Owner
                {
                    Name = createDto.Name,
                    OwnerType = createDto.OwnerType,
                    RegistrationNumber = createDto.RegistrationNumber,
                    Address = createDto.Address,
                    ContactPhone = createDto.ContactPhone,
                    ContactEmail = createDto.ContactEmail,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.Owners.Add(owner);
                await db.SaveChangesAsync();

                var ownerDto = new OwnerDto
                {
                    Id = owner.Id,
                    Name = owner.Name,
                    OwnerType = owner.OwnerType,
                    OwnerTypeName = owner.OwnerType.ToString(),
                    RegistrationNumber = owner.RegistrationNumber,
                    Address = owner.Address,
                    ContactPhone = owner.ContactPhone,
                    ContactEmail = owner.ContactEmail,
                    IsActive = owner.IsActive,
                    CreatedAt = owner.CreatedAt,
                    UpdatedAt = owner.UpdatedAt,
                    CreatedBy = owner.CreatedBy,
                    UpdatedBy = owner.UpdatedBy
                };

                return Results.Created($"/api/owners/{owner.Id}", ownerDto);
            })
            .WithName("CreateOwner")
            .WithSummary("Create a new owner");

            // PUT /api/owners/{id} - Update owner
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateOwnerDto updateDto) =>
            {
                var owner = await db.Owners.FindAsync(id);
                if (owner is null)
                {
                    return Results.NotFound();
                }

                owner.Name = updateDto.Name;
                owner.OwnerType = updateDto.OwnerType;
                owner.RegistrationNumber = updateDto.RegistrationNumber;
                owner.Address = updateDto.Address;
                owner.ContactPhone = updateDto.ContactPhone;
                owner.ContactEmail = updateDto.ContactEmail;
                owner.UpdatedAt = DateTime.UtcNow;
                owner.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateOwner")
            .WithSummary("Update an existing owner");

            // DELETE /api/owners/{id} - Soft delete owner
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var owner = await db.Owners.FindAsync(id);
                if (owner is null)
                {
                    return Results.NotFound();
                }

                // Check if owner has active ownerships
                var hasActiveOwnerships = await db.VehicleOwnerships
                    .AnyAsync(vo => vo.OwnerId == id && (!vo.EndDate.HasValue || vo.EndDate > DateTime.UtcNow));
                
                if (hasActiveOwnerships)
                {
                    return Results.BadRequest("Cannot delete owner with active vehicle ownerships");
                }

                owner.IsActive = false;
                owner.UpdatedAt = DateTime.UtcNow;
                owner.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteOwner")
            .WithSummary("Soft delete an owner");

            // GET /api/owners/{id}/vehicles - Get vehicles owned by owner
            group.MapGet("/{id}/vehicles", async (AppDbContext db, int id) =>
            {
                var owner = await db.Owners.FindAsync(id);
                if (owner is null)
                {
                    return Results.NotFound("Owner not found");
                }

                var ownerships = await db.VehicleOwnerships
                    .Include(vo => vo.Vehicle)
                    .ThenInclude(v => v.Manufacturer)
                    .Include(vo => vo.Vehicle)
                    .ThenInclude(v => v.VehicleType)
                    .Where(vo => vo.OwnerId == id)
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
            .WithName("GetOwnerVehicles")
            .WithSummary("Get all vehicles owned by an owner");
        }
    }
}
