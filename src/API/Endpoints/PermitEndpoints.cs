using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class PermitEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/permits")
                .WithTags("Permits")
                .WithOpenApi();

            // GET /api/permits - List all permits
            group.MapGet("/", async (AppDbContext db, int? vehicleId, PermitStatus? status, bool activeOnly = false) =>
            {
                var query = db.Permits
                    .Include(p => p.Vehicle)
                    .Include(p => p.PermitType)
                    .AsQueryable();

                if (vehicleId.HasValue)
                {
                    query = query.Where(p => p.VehicleId == vehicleId.Value);
                }

                if (status.HasValue)
                {
                    query = query.Where(p => p.Status == status.Value);
                }

                if (activeOnly)
                {
                    query = query.Where(p => p.Status != PermitStatus.Cancelled && p.Status != PermitStatus.Expired);
                }

                var permits = await query
                    .Select(p => new PermitDto
                    {
                        Id = p.Id,
                        PermitNumber = p.PermitNumber,
                        VehicleId = p.VehicleId,
                        VehicleName = p.Vehicle.Name,
                        VehicleVIN = p.Vehicle.VIN,
                        PermitTypeId = p.PermitTypeId,
                        PermitTypeName = p.PermitType.Name,
                        PermitTypeCode = p.PermitType.Code,
                        Status = p.Status,
                        StatusName = p.Status.ToString(),
                        ApplicationDate = p.ApplicationDate,
                        ApprovalDate = p.ApprovalDate,
                        ValidFromDate = p.ValidFromDate,
                        ValidToDate = p.ValidToDate,
                        Purpose = p.Purpose,
                        Notes = p.Notes,
                        Fee = p.Fee,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        CreatedBy = p.CreatedBy,
                        UpdatedBy = p.UpdatedBy
                    })
                    .OrderByDescending(p => p.ApplicationDate)
                    .ToListAsync();

                return Results.Ok(permits);
            })
            .WithName("GetPermits")
            .WithSummary("Get permits with optional filtering");

            // GET /api/permits/{id} - Get permit by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var permit = await db.Permits
                    .Include(p => p.Vehicle)
                    .Include(p => p.PermitType)
                    .Where(p => p.Id == id)
                    .Select(p => new PermitDto
                    {
                        Id = p.Id,
                        PermitNumber = p.PermitNumber,
                        VehicleId = p.VehicleId,
                        VehicleName = p.Vehicle.Name,
                        VehicleVIN = p.Vehicle.VIN,
                        PermitTypeId = p.PermitTypeId,
                        PermitTypeName = p.PermitType.Name,
                        PermitTypeCode = p.PermitType.Code,
                        Status = p.Status,
                        StatusName = p.Status.ToString(),
                        ApplicationDate = p.ApplicationDate,
                        ApprovalDate = p.ApprovalDate,
                        ValidFromDate = p.ValidFromDate,
                        ValidToDate = p.ValidToDate,
                        Purpose = p.Purpose,
                        Notes = p.Notes,
                        Fee = p.Fee,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        CreatedBy = p.CreatedBy,
                        UpdatedBy = p.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                return permit is not null ? Results.Ok(permit) : Results.NotFound();
            })
            .WithName("GetPermitById")
            .WithSummary("Get permit by ID");

            // POST /api/permits - Create new permit
            group.MapPost("/", async (AppDbContext db, CreatePermitDto createDto) =>
            {
                // Validate vehicle exists
                if (!await db.Vehicles.AnyAsync(v => v.Id == createDto.VehicleId && v.IsActive))
                {
                    return Results.BadRequest("Invalid vehicle ID");
                }

                // Validate permit type exists
                var permitType = await db.PermitTypes.FirstOrDefaultAsync(pt => pt.Id == createDto.PermitTypeId && pt.IsActive);
                if (permitType is null)
                {
                    return Results.BadRequest("Invalid permit type ID");
                }

                // Validate dates
                if (createDto.ValidFromDate >= createDto.ValidToDate)
                {
                    return Results.BadRequest("Valid from date must be before valid to date");
                }

                // Generate permit number
                var permitNumber = await GeneratePermitNumber(db, permitType.Code);

                var permit = new Permit
                {
                    PermitNumber = permitNumber,
                    VehicleId = createDto.VehicleId,
                    PermitTypeId = createDto.PermitTypeId,
                    Status = PermitStatus.Draft,
                    ValidFromDate = createDto.ValidFromDate,
                    ValidToDate = createDto.ValidToDate,
                    Purpose = createDto.Purpose,
                    Notes = createDto.Notes,
                    Fee = permitType.Fee,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.Permits.Add(permit);
                await db.SaveChangesAsync();

                // Return the created permit with related data
                var createdPermit = await db.Permits
                    .Include(p => p.Vehicle)
                    .Include(p => p.PermitType)
                    .Where(p => p.Id == permit.Id)
                    .Select(p => new PermitDto
                    {
                        Id = p.Id,
                        PermitNumber = p.PermitNumber,
                        VehicleId = p.VehicleId,
                        VehicleName = p.Vehicle.Name,
                        VehicleVIN = p.Vehicle.VIN,
                        PermitTypeId = p.PermitTypeId,
                        PermitTypeName = p.PermitType.Name,
                        PermitTypeCode = p.PermitType.Code,
                        Status = p.Status,
                        StatusName = p.Status.ToString(),
                        ApplicationDate = p.ApplicationDate,
                        ApprovalDate = p.ApprovalDate,
                        ValidFromDate = p.ValidFromDate,
                        ValidToDate = p.ValidToDate,
                        Purpose = p.Purpose,
                        Notes = p.Notes,
                        Fee = p.Fee,
                        CreatedAt = p.CreatedAt,
                        UpdatedAt = p.UpdatedAt,
                        CreatedBy = p.CreatedBy,
                        UpdatedBy = p.UpdatedBy
                    })
                    .FirstAsync();

                return Results.Created($"/api/permits/{permit.Id}", createdPermit);
            })
            .WithName("CreatePermit")
            .WithSummary("Create a new permit");

            // PATCH /api/permits/{id}/status - Update permit status
            group.MapPatch("/{id}/status", async (AppDbContext db, int id, UpdatePermitStatusDto statusDto) =>
            {
                var permit = await db.Permits.FindAsync(id);
                if (permit is null)
                {
                    return Results.NotFound();
                }

                // Validate status transition
                if (!IsValidStatusTransition(permit.Status, statusDto.Status))
                {
                    return Results.BadRequest($"Invalid status transition from {permit.Status} to {statusDto.Status}");
                }

                permit.Status = statusDto.Status;
                if (!string.IsNullOrEmpty(statusDto.Notes))
                {
                    permit.Notes = statusDto.Notes;
                }

                // Set approval date if status is approved
                if (statusDto.Status == PermitStatus.Approved && !permit.ApprovalDate.HasValue)
                {
                    permit.ApprovalDate = DateTime.UtcNow;
                }

                permit.UpdatedAt = DateTime.UtcNow;
                permit.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdatePermitStatus")
            .WithSummary("Update permit status");

            // POST /api/permits/{id}/constraints - Add constraint to permit
            group.MapPost("/{id}/constraints", async (AppDbContext db, int id, CreatePermitConstraintDto constraintDto) =>
            {
                var permit = await db.Permits.FindAsync(id);
                if (permit is null)
                {
                    return Results.NotFound("Permit not found");
                }

                var constraint = new PermitConstraint
                {
                    PermitId = id,
                    ConstraintType = constraintDto.ConstraintType,
                    Description = constraintDto.Description,
                    Value = constraintDto.Value
                };

                db.PermitConstraints.Add(constraint);
                await db.SaveChangesAsync();

                var constraintResponseDto = new PermitConstraintDto
                {
                    Id = constraint.Id,
                    PermitId = constraint.PermitId,
                    ConstraintType = constraint.ConstraintType,
                    Description = constraint.Description,
                    Value = constraint.Value
                };

                return Results.Created($"/api/permits/{id}/constraints/{constraint.Id}", constraintResponseDto);
            })
            .WithName("AddPermitConstraint")
            .WithSummary("Add a constraint to a permit");

            // GET /api/permits/{id}/constraints - Get permit constraints
            group.MapGet("/{id}/constraints", async (AppDbContext db, int id) =>
            {
                var permit = await db.Permits.FindAsync(id);
                if (permit is null)
                {
                    return Results.NotFound("Permit not found");
                }

                var constraints = await db.PermitConstraints
                    .Where(pc => pc.PermitId == id)
                    .Select(pc => new PermitConstraintDto
                    {
                        Id = pc.Id,
                        PermitId = pc.PermitId,
                        ConstraintType = pc.ConstraintType,
                        Description = pc.Description,
                        Value = pc.Value
                    })
                    .ToListAsync();

                return Results.Ok(constraints);
            })
            .WithName("GetPermitConstraints")
            .WithSummary("Get all constraints for a permit");

            // GET /api/permits/types - Get all permit types
            group.MapGet("/types", async (AppDbContext db) =>
            {
                var permitTypes = await db.PermitTypes
                    .Where(pt => pt.IsActive)
                    .Select(pt => new PermitTypeDto
                    {
                        Id = pt.Id,
                        Name = pt.Name,
                        Code = pt.Code,
                        Description = pt.Description,
                        Fee = pt.Fee,
                        ValidityDays = pt.ValidityDays,
                        IsActive = pt.IsActive
                    })
                    .ToListAsync();

                return Results.Ok(permitTypes);
            })
            .WithName("GetPermitTypes")
            .WithSummary("Get all active permit types");

            // POST /api/permits/{id}/load - Add load to permit
            group.MapPost("/{id}/load", async (AppDbContext db, int id, CreateLoadDto loadDto) =>
            {
                var permit = await db.Permits.FindAsync(id);
                if (permit is null)
                {
                    return Results.NotFound("Permit not found");
                }

                // Check if permit already has a load
                if (await db.Loads.AnyAsync(l => l.PermitId == id))
                {
                    return Results.BadRequest("Permit already has a load defined");
                }

                var load = new Load
                {
                    PermitId = id,
                    LoadType = loadDto.LoadType,
                    WeightKg = loadDto.WeightKg,
                    Description = loadDto.Description,
                    CargoType = loadDto.CargoType,
                    IsIndivisible = loadDto.IsIndivisible,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.Loads.Add(load);
                await db.SaveChangesAsync();

                var loadResponseDto = new LoadDto
                {
                    Id = load.Id,
                    PermitId = load.PermitId,
                    LoadType = load.LoadType,
                    LoadTypeName = load.LoadType.ToString(),
                    WeightKg = load.WeightKg,
                    Description = load.Description,
                    CargoType = load.CargoType,
                    IsIndivisible = load.IsIndivisible,
                    CreatedAt = load.CreatedAt,
                    UpdatedAt = load.UpdatedAt,
                    CreatedBy = load.CreatedBy,
                    UpdatedBy = load.UpdatedBy
                };

                return Results.Created($"/api/permits/{id}/load", loadResponseDto);
            })
            .WithName("AddPermitLoad")
            .WithSummary("Add a load to a permit");

            // GET /api/permits/{id}/load - Get permit load
            group.MapGet("/{id}/load", async (AppDbContext db, int id) =>
            {
                var permit = await db.Permits.FindAsync(id);
                if (permit is null)
                {
                    return Results.NotFound("Permit not found");
                }

                var load = await db.Loads
                    .Include(l => l.Dimensions)
                    .Include(l => l.Projections)
                    .Where(l => l.PermitId == id)
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

                return load is not null ? Results.Ok(load) : Results.NotFound("Load not found");
            })
            .WithName("GetPermitLoad")
            .WithSummary("Get load for a permit");

            // POST /api/permits/{id}/routes - Add route to permit
            group.MapPost("/{id}/routes", async (AppDbContext db, int id, CreatePermitRouteDto routeDto) =>
            {
                var permit = await db.Permits.FindAsync(id);
                if (permit is null)
                {
                    return Results.NotFound("Permit not found");
                }

                // Validate route exists
                if (!await db.Routes.AnyAsync(r => r.Id == routeDto.RouteId && r.IsActive))
                {
                    return Results.BadRequest("Invalid route ID");
                }

                // Check if sequence is already taken for this permit
                if (await db.PermitRoutes.AnyAsync(pr => pr.PermitId == id && pr.Sequence == routeDto.Sequence))
                {
                    return Results.BadRequest("Sequence number already used for this permit");
                }

                var permitRoute = new PermitRoute
                {
                    PermitId = id,
                    RouteId = routeDto.RouteId,
                    Sequence = routeDto.Sequence,
                    ScheduledDate = routeDto.ScheduledDate,
                    Notes = routeDto.Notes
                };

                db.PermitRoutes.Add(permitRoute);
                await db.SaveChangesAsync();

                // Return the created permit route with related data
                var createdPermitRoute = await db.PermitRoutes
                    .Include(pr => pr.Route)
                    .Where(pr => pr.Id == permitRoute.Id)
                    .Select(pr => new PermitRouteDto
                    {
                        Id = pr.Id,
                        PermitId = pr.PermitId,
                        RouteId = pr.RouteId,
                        RouteName = pr.Route.Name,
                        RouteOrigin = pr.Route.Origin,
                        RouteDestination = pr.Route.Destination,
                        RouteDistanceKm = pr.Route.DistanceKm,
                        Sequence = pr.Sequence,
                        ScheduledDate = pr.ScheduledDate,
                        Notes = pr.Notes
                    })
                    .FirstAsync();

                return Results.Created($"/api/permits/{id}/routes/{permitRoute.Id}", createdPermitRoute);
            })
            .WithName("AddPermitRoute")
            .WithSummary("Add a route to a permit");

            // GET /api/permits/{id}/routes - Get permit routes
            group.MapGet("/{id}/routes", async (AppDbContext db, int id) =>
            {
                var permit = await db.Permits.FindAsync(id);
                if (permit is null)
                {
                    return Results.NotFound("Permit not found");
                }

                var routes = await db.PermitRoutes
                    .Include(pr => pr.Route)
                    .Where(pr => pr.PermitId == id)
                    .Select(pr => new PermitRouteDto
                    {
                        Id = pr.Id,
                        PermitId = pr.PermitId,
                        RouteId = pr.RouteId,
                        RouteName = pr.Route.Name,
                        RouteOrigin = pr.Route.Origin,
                        RouteDestination = pr.Route.Destination,
                        RouteDistanceKm = pr.Route.DistanceKm,
                        Sequence = pr.Sequence,
                        ScheduledDate = pr.ScheduledDate,
                        Notes = pr.Notes
                    })
                    .OrderBy(pr => pr.Sequence)
                    .ToListAsync();

                return Results.Ok(routes);
            })
            .WithName("GetPermitRoutes")
            .WithSummary("Get all routes for a permit");
        }

        private static async Task<string> GeneratePermitNumber(AppDbContext db, string permitTypeCode)
        {
            var year = DateTime.UtcNow.Year;
            var prefix = $"{permitTypeCode}{year}";
            
            var lastPermit = await db.Permits
                .Where(p => p.PermitNumber.StartsWith(prefix))
                .OrderByDescending(p => p.PermitNumber)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastPermit != null && lastPermit.PermitNumber.Length > prefix.Length)
            {
                var numberPart = lastPermit.PermitNumber.Substring(prefix.Length);
                if (int.TryParse(numberPart, out int lastNumber))
                {
                    nextNumber = lastNumber + 1;
                }
            }

            return $"{prefix}{nextNumber:D6}"; // Zero-padded 6 digits
        }

        private static bool IsValidStatusTransition(PermitStatus currentStatus, PermitStatus newStatus)
        {
            return currentStatus switch
            {
                PermitStatus.Draft => newStatus is PermitStatus.Submitted or PermitStatus.Cancelled,
                PermitStatus.Submitted => newStatus is PermitStatus.UnderReview or PermitStatus.Cancelled,
                PermitStatus.UnderReview => newStatus is PermitStatus.Approved or PermitStatus.Rejected,
                PermitStatus.Approved => newStatus is PermitStatus.Expired or PermitStatus.Cancelled,
                PermitStatus.Rejected => newStatus is PermitStatus.Draft,
                PermitStatus.Expired => false,
                PermitStatus.Cancelled => false,
                _ => false
            };
        }
    }
}
