using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class ManufacturerEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/manufacturers")
                .WithTags("Manufacturers")
                .WithOpenApi();

            // GET /api/manufacturers - List all manufacturers
            group.MapGet("/", async (AppDbContext db) =>
            {
                var manufacturers = await db.Manufacturers
                    .Where(m => m.IsActive)
                    .Select(m => new ManufacturerDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Code = m.Code,
                        Address = m.Address,
                        ContactPhone = m.ContactPhone,
                        ContactEmail = m.ContactEmail,
                        CountryCode = m.CountryCode,
                        IsActive = m.IsActive,
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt
                    })
                    .ToListAsync();

                return Results.Ok(manufacturers);
            })
            .WithName("GetManufacturers")
            .WithSummary("Get all active manufacturers");

            // GET /api/manufacturers/{id} - Get manufacturer by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var manufacturer = await db.Manufacturers
                    .Where(m => m.Id == id)
                    .Select(m => new ManufacturerDto
                    {
                        Id = m.Id,
                        Name = m.Name,
                        Code = m.Code,
                        Address = m.Address,
                        ContactPhone = m.ContactPhone,
                        ContactEmail = m.ContactEmail,
                        CountryCode = m.CountryCode,
                        IsActive = m.IsActive,
                        CreatedAt = m.CreatedAt,
                        UpdatedAt = m.UpdatedAt
                    })
                    .FirstOrDefaultAsync();

                return manufacturer is not null ? Results.Ok(manufacturer) : Results.NotFound();
            })
            .WithName("GetManufacturerById")
            .WithSummary("Get manufacturer by ID");

            // POST /api/manufacturers - Create new manufacturer
            group.MapPost("/", async (AppDbContext db, CreateManufacturerDto createDto) =>
            {
                // Validate manufacturer name uniqueness
                if (await db.Manufacturers.AnyAsync(m => m.Name == createDto.Name && m.IsActive))
                {
                    return Results.BadRequest("Manufacturer name already exists");
                }

                var manufacturer = new Manufacturer
                {
                    Name = createDto.Name,
                    Code = createDto.Code,
                    Address = createDto.Address,
                    ContactPhone = createDto.ContactPhone,
                    ContactEmail = createDto.ContactEmail,
                    CountryCode = createDto.CountryCode,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.Manufacturers.Add(manufacturer);
                await db.SaveChangesAsync();

                var manufacturerDto = new ManufacturerDto
                {
                    Id = manufacturer.Id,
                    Name = manufacturer.Name,
                    Code = manufacturer.Code,
                    Address = manufacturer.Address,
                    ContactPhone = manufacturer.ContactPhone,
                    ContactEmail = manufacturer.ContactEmail,
                    CountryCode = manufacturer.CountryCode,
                    IsActive = manufacturer.IsActive,
                    CreatedAt = manufacturer.CreatedAt,
                    UpdatedAt = manufacturer.UpdatedAt
                };

                return Results.Created($"/api/manufacturers/{manufacturer.Id}", manufacturerDto);
            })
            .WithName("CreateManufacturer")
            .WithSummary("Create a new manufacturer");

            // PUT /api/manufacturers/{id} - Update manufacturer
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateManufacturerDto updateDto) =>
            {
                var manufacturer = await db.Manufacturers.FindAsync(id);
                if (manufacturer is null)
                {
                    return Results.NotFound();
                }

                // Check if name is changing and if new name already exists
                if (manufacturer.Name != updateDto.Name && 
                    await db.Manufacturers.AnyAsync(m => m.Name == updateDto.Name && m.Id != id && m.IsActive))
                {
                    return Results.BadRequest("Manufacturer name already exists");
                }

                manufacturer.Name = updateDto.Name;
                manufacturer.Code = updateDto.Code;
                manufacturer.Address = updateDto.Address;
                manufacturer.ContactPhone = updateDto.ContactPhone;
                manufacturer.ContactEmail = updateDto.ContactEmail;
                manufacturer.CountryCode = updateDto.CountryCode;
                manufacturer.UpdatedAt = DateTime.UtcNow;
                manufacturer.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateManufacturer")
            .WithSummary("Update an existing manufacturer");

            // DELETE /api/manufacturers/{id} - Soft delete manufacturer
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var manufacturer = await db.Manufacturers.FindAsync(id);
                if (manufacturer is null)
                {
                    return Results.NotFound();
                }

                // Check if manufacturer has vehicles
                if (await db.Vehicles.AnyAsync(v => v.ManufacturerId == id && v.IsActive))
                {
                    return Results.BadRequest("Cannot delete manufacturer with active vehicles");
                }

                manufacturer.IsActive = false;
                manufacturer.UpdatedAt = DateTime.UtcNow;
                manufacturer.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteManufacturer")
            .WithSummary("Soft delete a manufacturer");
        }
    }
}
