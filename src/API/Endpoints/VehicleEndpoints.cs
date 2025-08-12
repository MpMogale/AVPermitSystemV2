
using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using FluentValidation;

namespace AVPermitSystemV2.API.Endpoints
{
    public class VehicleEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vehicles")
                .WithTags("Vehicles")
                .WithOpenApi();

            // GET /api/vehicles - List all vehicles
            group.MapGet("/", async (AppDbContext db) =>
            {
                var vehicles = await db.Vehicles
                    .Include(v => v.Manufacturer)
                    .Include(v => v.VehicleType)
                    .Include(v => v.VehicleCategory)
                    .Where(v => v.IsActive)
                    .Select(v => new VehicleDto
                    {
                        Id = v.Id,
                        VIN = v.VIN,
                        RegistrationNumber = v.RegistrationNumber,
                        Name = v.Name,
                        ManufacturerId = v.ManufacturerId,
                        ManufacturerName = v.Manufacturer.Name,
                        VehicleTypeId = v.VehicleTypeId,
                        VehicleTypeName = v.VehicleType.Name,
                        VehicleCategoryId = v.VehicleCategoryId,
                        VehicleCategoryName = v.VehicleCategory != null ? v.VehicleCategory.Name : null,
                        Model = v.Model,
                        YearOfManufacture = v.YearOfManufacture,
                        Color = v.Color,
                        GrossVehicleMass = v.GrossVehicleMass,
                        UnladenMass = v.UnladenMass,
                        LengthMm = v.LengthMm,
                        WidthMm = v.WidthMm,
                        HeightMm = v.HeightMm,
                        WheelbaseMm = v.WheelbaseMm,
                        FrontOverhangMm = v.FrontOverhangMm,
                        RearOverhangMm = v.RearOverhangMm,
                        IsActive = v.IsActive,
                        CreatedAt = v.CreatedAt,
                        UpdatedAt = v.UpdatedAt,
                        CreatedBy = v.CreatedBy,
                        UpdatedBy = v.UpdatedBy
                    })
                    .ToListAsync();

                return Results.Ok(vehicles);
            })
            .WithName("GetVehicles")
            .WithSummary("Get all active vehicles");

            // GET /api/vehicles/{id} - Get vehicle by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var vehicle = await db.Vehicles
                    .Include(v => v.Manufacturer)
                    .Include(v => v.VehicleType)
                    .Include(v => v.VehicleCategory)
                    .Where(v => v.Id == id)
                    .Select(v => new VehicleDto
                    {
                        Id = v.Id,
                        VIN = v.VIN,
                        RegistrationNumber = v.RegistrationNumber,
                        Name = v.Name,
                        ManufacturerId = v.ManufacturerId,
                        ManufacturerName = v.Manufacturer.Name,
                        VehicleTypeId = v.VehicleTypeId,
                        VehicleTypeName = v.VehicleType.Name,
                        VehicleCategoryId = v.VehicleCategoryId,
                        VehicleCategoryName = v.VehicleCategory != null ? v.VehicleCategory.Name : null,
                        Model = v.Model,
                        YearOfManufacture = v.YearOfManufacture,
                        Color = v.Color,
                        GrossVehicleMass = v.GrossVehicleMass,
                        UnladenMass = v.UnladenMass,
                        LengthMm = v.LengthMm,
                        WidthMm = v.WidthMm,
                        HeightMm = v.HeightMm,
                        WheelbaseMm = v.WheelbaseMm,
                        FrontOverhangMm = v.FrontOverhangMm,
                        RearOverhangMm = v.RearOverhangMm,
                        IsActive = v.IsActive,
                        CreatedAt = v.CreatedAt,
                        UpdatedAt = v.UpdatedAt,
                        CreatedBy = v.CreatedBy,
                        UpdatedBy = v.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                return vehicle is not null ? Results.Ok(vehicle) : Results.NotFound();
            })
            .WithName("GetVehicleById")
            .WithSummary("Get vehicle by ID");

            // POST /api/vehicles - Create new vehicle
            group.MapPost("/", async (AppDbContext db, CreateVehicleDto createDto) =>
            {
                // Validate VIN uniqueness
                if (await db.Vehicles.AnyAsync(v => v.VIN == createDto.VIN))
                {
                    return Results.BadRequest("VIN already exists in the system");
                }

                // Validate manufacturer exists
                if (!await db.Manufacturers.AnyAsync(m => m.Id == createDto.ManufacturerId && m.IsActive))
                {
                    return Results.BadRequest("Invalid manufacturer ID");
                }

                // Validate vehicle type exists
                if (!await db.VehicleTypes.AnyAsync(vt => vt.Id == createDto.VehicleTypeId && vt.IsActive))
                {
                    return Results.BadRequest("Invalid vehicle type ID");
                }

                // Validate vehicle category if provided
                if (createDto.VehicleCategoryId.HasValue && 
                    !await db.VehicleCategories.AnyAsync(vc => vc.Id == createDto.VehicleCategoryId && vc.IsActive))
                {
                    return Results.BadRequest("Invalid vehicle category ID");
                }

                var vehicle = new Vehicle
                {
                    VIN = createDto.VIN,
                    RegistrationNumber = createDto.RegistrationNumber,
                    Name = createDto.Name,
                    ManufacturerId = createDto.ManufacturerId,
                    VehicleTypeId = createDto.VehicleTypeId,
                    VehicleCategoryId = createDto.VehicleCategoryId,
                    Model = createDto.Model,
                    YearOfManufacture = createDto.YearOfManufacture,
                    Color = createDto.Color,
                    GrossVehicleMass = createDto.GrossVehicleMass,
                    UnladenMass = createDto.UnladenMass,
                    LengthMm = createDto.LengthMm,
                    WidthMm = createDto.WidthMm,
                    HeightMm = createDto.HeightMm,
                    WheelbaseMm = createDto.WheelbaseMm,
                    FrontOverhangMm = createDto.FrontOverhangMm,
                    RearOverhangMm = createDto.RearOverhangMm,
                    CreatedBy = "System", // TODO: Get from authenticated user
                    UpdatedBy = "System"
                };

                db.Vehicles.Add(vehicle);
                await db.SaveChangesAsync();

                // Return the created vehicle with related data
                var createdVehicle = await db.Vehicles
                    .Include(v => v.Manufacturer)
                    .Include(v => v.VehicleType)
                    .Include(v => v.VehicleCategory)
                    .Where(v => v.Id == vehicle.Id)
                    .Select(v => new VehicleDto
                    {
                        Id = v.Id,
                        VIN = v.VIN,
                        RegistrationNumber = v.RegistrationNumber,
                        Name = v.Name,
                        ManufacturerId = v.ManufacturerId,
                        ManufacturerName = v.Manufacturer.Name,
                        VehicleTypeId = v.VehicleTypeId,
                        VehicleTypeName = v.VehicleType.Name,
                        VehicleCategoryId = v.VehicleCategoryId,
                        VehicleCategoryName = v.VehicleCategory != null ? v.VehicleCategory.Name : null,
                        Model = v.Model,
                        YearOfManufacture = v.YearOfManufacture,
                        Color = v.Color,
                        GrossVehicleMass = v.GrossVehicleMass,
                        UnladenMass = v.UnladenMass,
                        LengthMm = v.LengthMm,
                        WidthMm = v.WidthMm,
                        HeightMm = v.HeightMm,
                        WheelbaseMm = v.WheelbaseMm,
                        FrontOverhangMm = v.FrontOverhangMm,
                        RearOverhangMm = v.RearOverhangMm,
                        IsActive = v.IsActive,
                        CreatedAt = v.CreatedAt,
                        UpdatedAt = v.UpdatedAt,
                        CreatedBy = v.CreatedBy,
                        UpdatedBy = v.UpdatedBy
                    })
                    .FirstAsync();

                return Results.Created($"/api/vehicles/{vehicle.Id}", createdVehicle);
            })
            .WithName("CreateVehicle")
            .WithSummary("Create a new vehicle");

            // PUT /api/vehicles/{id} - Update vehicle
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateVehicleDto updateDto) =>
            {
                var vehicle = await db.Vehicles.FindAsync(id);
                if (vehicle is null)
                {
                    return Results.NotFound();
                }

                // Check if VIN is changing and if new VIN already exists
                if (vehicle.VIN != updateDto.VIN && 
                    await db.Vehicles.AnyAsync(v => v.VIN == updateDto.VIN && v.Id != id))
                {
                    return Results.BadRequest("VIN already exists in the system");
                }

                // Update vehicle properties
                vehicle.VIN = updateDto.VIN;
                vehicle.RegistrationNumber = updateDto.RegistrationNumber;
                vehicle.Name = updateDto.Name;
                vehicle.ManufacturerId = updateDto.ManufacturerId;
                vehicle.VehicleTypeId = updateDto.VehicleTypeId;
                vehicle.VehicleCategoryId = updateDto.VehicleCategoryId;
                vehicle.Model = updateDto.Model;
                vehicle.YearOfManufacture = updateDto.YearOfManufacture;
                vehicle.Color = updateDto.Color;
                vehicle.GrossVehicleMass = updateDto.GrossVehicleMass;
                vehicle.UnladenMass = updateDto.UnladenMass;
                vehicle.LengthMm = updateDto.LengthMm;
                vehicle.WidthMm = updateDto.WidthMm;
                vehicle.HeightMm = updateDto.HeightMm;
                vehicle.WheelbaseMm = updateDto.WheelbaseMm;
                vehicle.FrontOverhangMm = updateDto.FrontOverhangMm;
                vehicle.RearOverhangMm = updateDto.RearOverhangMm;
                vehicle.UpdatedAt = DateTime.UtcNow;
                vehicle.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateVehicle")
            .WithSummary("Update an existing vehicle");

            // DELETE /api/vehicles/{id} - Soft delete vehicle
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var vehicle = await db.Vehicles.FindAsync(id);
                if (vehicle is null)
                {
                    return Results.NotFound();
                }

                vehicle.IsActive = false;
                vehicle.UpdatedAt = DateTime.UtcNow;
                vehicle.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteVehicle")
            .WithSummary("Soft delete a vehicle");

            // POST /api/vehicles/{id}/components - Add component to vehicle
            group.MapPost("/{id}/components", async (AppDbContext db, int id, CreateVehicleComponentDto componentDto) =>
            {
                var vehicle = await db.Vehicles.FindAsync(id);
                if (vehicle is null)
                {
                    return Results.NotFound("Vehicle not found");
                }

                // Validate component type exists
                if (!await db.ComponentTypes.AnyAsync(ct => ct.Id == componentDto.ComponentTypeId && ct.IsActive))
                {
                    return Results.BadRequest("Invalid component type ID");
                }

                // Check if position is already taken
                if (await db.VehicleComponents.AnyAsync(vc => vc.VehicleId == id && vc.Position == componentDto.Position))
                {
                    return Results.BadRequest("Position already occupied by another component");
                }

                var component = new VehicleComponent
                {
                    VehicleId = id,
                    ComponentTypeId = componentDto.ComponentTypeId,
                    RegistrationNumber = componentDto.RegistrationNumber,
                    SerialNumber = componentDto.SerialNumber,
                    ManufacturerName = componentDto.ManufacturerName,
                    Model = componentDto.Model,
                    YearOfManufacture = componentDto.YearOfManufacture,
                    Mass = componentDto.Mass,
                    Position = componentDto.Position,
                    CreatedBy = "System"
                };

                db.VehicleComponents.Add(component);
                await db.SaveChangesAsync();

                var createdComponent = await db.VehicleComponents
                    .Include(vc => vc.ComponentType)
                    .Where(vc => vc.Id == component.Id)
                    .Select(vc => new VehicleComponentDto
                    {
                        Id = vc.Id,
                        VehicleId = vc.VehicleId,
                        ComponentTypeId = vc.ComponentTypeId,
                        ComponentTypeName = vc.ComponentType.Name,
                        RegistrationNumber = vc.RegistrationNumber,
                        SerialNumber = vc.SerialNumber,
                        ManufacturerName = vc.ManufacturerName,
                        Model = vc.Model,
                        YearOfManufacture = vc.YearOfManufacture,
                        Mass = vc.Mass,
                        Position = vc.Position,
                        IsActive = vc.IsActive,
                        CreatedAt = vc.CreatedAt
                    })
                    .FirstAsync();

                return Results.Created($"/api/vehicles/{id}/components/{component.Id}", createdComponent);
            })
            .WithName("AddVehicleComponent")
            .WithSummary("Add a component to a vehicle");

            // GET /api/vehicles/{id}/components - Get vehicle components
            group.MapGet("/{id}/components", async (AppDbContext db, int id) =>
            {
                var vehicle = await db.Vehicles.FindAsync(id);
                if (vehicle is null)
                {
                    return Results.NotFound("Vehicle not found");
                }

                var components = await db.VehicleComponents
                    .Include(vc => vc.ComponentType)
                    .Include(vc => vc.Dimensions)
                    .Where(vc => vc.VehicleId == id && vc.IsActive)
                    .Select(vc => new VehicleComponentDto
                    {
                        Id = vc.Id,
                        VehicleId = vc.VehicleId,
                        ComponentTypeId = vc.ComponentTypeId,
                        ComponentTypeName = vc.ComponentType.Name,
                        RegistrationNumber = vc.RegistrationNumber,
                        SerialNumber = vc.SerialNumber,
                        ManufacturerName = vc.ManufacturerName,
                        Model = vc.Model,
                        YearOfManufacture = vc.YearOfManufacture,
                        Mass = vc.Mass,
                        Position = vc.Position,
                        IsActive = vc.IsActive,
                        CreatedAt = vc.CreatedAt,
                        Dimensions = vc.Dimensions != null ? new ComponentDimensionDto
                        {
                            Id = vc.Dimensions.Id,
                            ComponentId = vc.Dimensions.ComponentId,
                            LengthMm = vc.Dimensions.LengthMm,
                            WidthMm = vc.Dimensions.WidthMm,
                            HeightMm = vc.Dimensions.HeightMm,
                            FrontOverhangMm = vc.Dimensions.FrontOverhangMm,
                            RearOverhangMm = vc.Dimensions.RearOverhangMm,
                            LeftOverhangMm = vc.Dimensions.LeftOverhangMm,
                            RightOverhangMm = vc.Dimensions.RightOverhangMm,
                            Unit = vc.Dimensions.Unit,
                            CreatedAt = vc.Dimensions.CreatedAt
                        } : null
                    })
                    .OrderBy(vc => vc.Position)
                    .ToListAsync();

                return Results.Ok(components);
            })
            .WithName("GetVehicleComponents")
            .WithSummary("Get all components for a vehicle");
        }
    }
}
