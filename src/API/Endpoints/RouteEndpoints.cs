using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class RouteEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/routes")
                .WithTags("Routes")
                .WithOpenApi();

            // GET /api/routes - List all routes
            group.MapGet("/", async (AppDbContext db, bool activeOnly = true) =>
            {
                var query = db.Routes.AsQueryable();

                if (activeOnly)
                {
                    query = query.Where(r => r.IsActive);
                }

                var routes = await query
                    .Select(r => new RouteDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Origin = r.Origin,
                        Destination = r.Destination,
                        DistanceKm = r.DistanceKm,
                        Description = r.Description,
                        Waypoints = r.Waypoints,
                        IsActive = r.IsActive,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt,
                        CreatedBy = r.CreatedBy,
                        UpdatedBy = r.UpdatedBy
                    })
                    .OrderBy(r => r.Name)
                    .ToListAsync();

                return Results.Ok(routes);
            })
            .WithName("GetRoutes")
            .WithSummary("Get all routes");

            // GET /api/routes/{id} - Get route by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var route = await db.Routes
                    .Where(r => r.Id == id)
                    .Select(r => new RouteDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Origin = r.Origin,
                        Destination = r.Destination,
                        DistanceKm = r.DistanceKm,
                        Description = r.Description,
                        Waypoints = r.Waypoints,
                        IsActive = r.IsActive,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt,
                        CreatedBy = r.CreatedBy,
                        UpdatedBy = r.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                return route is not null ? Results.Ok(route) : Results.NotFound();
            })
            .WithName("GetRouteById")
            .WithSummary("Get route by ID");

            // POST /api/routes - Create new route
            group.MapPost("/", async (AppDbContext db, CreateRouteDto createDto) =>
            {
                var route = new Domain.Entities.Route
                {
                    Name = createDto.Name,
                    Origin = createDto.Origin,
                    Destination = createDto.Destination,
                    DistanceKm = createDto.DistanceKm,
                    Description = createDto.Description,
                    Waypoints = createDto.Waypoints,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.Routes.Add(route);
                await db.SaveChangesAsync();

                var routeDto = new RouteDto
                {
                    Id = route.Id,
                    Name = route.Name,
                    Origin = route.Origin,
                    Destination = route.Destination,
                    DistanceKm = route.DistanceKm,
                    Description = route.Description,
                    Waypoints = route.Waypoints,
                    IsActive = route.IsActive,
                    CreatedAt = route.CreatedAt,
                    UpdatedAt = route.UpdatedAt,
                    CreatedBy = route.CreatedBy,
                    UpdatedBy = route.UpdatedBy
                };

                return Results.Created($"/api/routes/{route.Id}", routeDto);
            })
            .WithName("CreateRoute")
            .WithSummary("Create a new route");

            // PUT /api/routes/{id} - Update route
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateRouteDto updateDto) =>
            {
                var route = await db.Routes.FindAsync(id);
                if (route is null)
                {
                    return Results.NotFound();
                }

                route.Name = updateDto.Name;
                route.Origin = updateDto.Origin;
                route.Destination = updateDto.Destination;
                route.DistanceKm = updateDto.DistanceKm;
                route.Description = updateDto.Description;
                route.Waypoints = updateDto.Waypoints;
                route.UpdatedAt = DateTime.UtcNow;
                route.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateRoute")
            .WithSummary("Update an existing route");

            // DELETE /api/routes/{id} - Soft delete route
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var route = await db.Routes.FindAsync(id);
                if (route is null)
                {
                    return Results.NotFound();
                }

                // Check if route is used in any active permits
                var hasActivePermits = await db.PermitRoutes
                    .Include(pr => pr.Permit)
                    .AnyAsync(pr => pr.RouteId == id && 
                             pr.Permit.Status != PermitStatus.Cancelled && 
                             pr.Permit.Status != PermitStatus.Expired);
                
                if (hasActivePermits)
                {
                    return Results.BadRequest("Cannot delete route that is used in active permits");
                }

                route.IsActive = false;
                route.UpdatedAt = DateTime.UtcNow;
                route.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteRoute")
            .WithSummary("Soft delete a route");

            // GET /api/routes/search - Search routes
            group.MapGet("/search", async (AppDbContext db, string? origin, string? destination, decimal? maxDistance) =>
            {
                var query = db.Routes.Where(r => r.IsActive);

                if (!string.IsNullOrEmpty(origin))
                {
                    query = query.Where(r => r.Origin.Contains(origin));
                }

                if (!string.IsNullOrEmpty(destination))
                {
                    query = query.Where(r => r.Destination.Contains(destination));
                }

                if (maxDistance.HasValue)
                {
                    query = query.Where(r => r.DistanceKm <= maxDistance.Value);
                }

                var routes = await query
                    .Select(r => new RouteDto
                    {
                        Id = r.Id,
                        Name = r.Name,
                        Origin = r.Origin,
                        Destination = r.Destination,
                        DistanceKm = r.DistanceKm,
                        Description = r.Description,
                        Waypoints = r.Waypoints,
                        IsActive = r.IsActive,
                        CreatedAt = r.CreatedAt,
                        UpdatedAt = r.UpdatedAt,
                        CreatedBy = r.CreatedBy,
                        UpdatedBy = r.UpdatedBy
                    })
                    .OrderBy(r => r.DistanceKm)
                    .ToListAsync();

                return Results.Ok(routes);
            })
            .WithName("SearchRoutes")
            .WithSummary("Search routes by origin, destination, or maximum distance");

            // GET /api/routes/{id}/permits - Get permits using this route
            group.MapGet("/{id}/permits", async (AppDbContext db, int id) =>
            {
                var route = await db.Routes.FindAsync(id);
                if (route is null)
                {
                    return Results.NotFound("Route not found");
                }

                var permitRoutes = await db.PermitRoutes
                    .Include(pr => pr.Permit)
                    .ThenInclude(p => p.Vehicle)
                    .Include(pr => pr.Permit)
                    .ThenInclude(p => p.PermitType)
                    .Where(pr => pr.RouteId == id)
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
                    .OrderByDescending(pr => pr.ScheduledDate)
                    .ToListAsync();

                return Results.Ok(permitRoutes);
            })
            .WithName("GetRoutePermits")
            .WithSummary("Get all permits using this route");
        }
    }
}
