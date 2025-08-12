using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using AVPermitSystemV2.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class VehicleEventEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/vehicle-events")
                .WithTags("Vehicle Events")
                .WithOpenApi();

            // GET /api/vehicle-events - List all vehicle events
            group.MapGet("/", async (AppDbContext db, int? vehicleId, EventType? eventType, DateTime? fromDate, DateTime? toDate) =>
            {
                var query = db.VehicleEvents
                    .Include(ve => ve.Vehicle)
                    .AsQueryable();

                if (vehicleId.HasValue)
                {
                    query = query.Where(ve => ve.VehicleId == vehicleId.Value);
                }

                if (eventType.HasValue)
                {
                    query = query.Where(ve => ve.EventType == eventType.Value);
                }

                if (fromDate.HasValue)
                {
                    query = query.Where(ve => ve.EventDate >= fromDate.Value);
                }

                if (toDate.HasValue)
                {
                    query = query.Where(ve => ve.EventDate <= toDate.Value);
                }

                var events = await query
                    .Select(ve => new VehicleEventDto
                    {
                        Id = ve.Id,
                        VehicleId = ve.VehicleId,
                        VehicleName = ve.Vehicle.Name,
                        VehicleVIN = ve.Vehicle.VIN,
                        EventType = ve.EventType,
                        EventTypeName = ve.EventType.ToString(),
                        Description = ve.Description,
                        EventDate = ve.EventDate,
                        Location = ve.Location,
                        RecordedBy = ve.RecordedBy,
                        Notes = ve.Notes,
                        AdditionalData = ve.AdditionalData,
                        CreatedAt = ve.CreatedAt,
                        UpdatedAt = ve.UpdatedAt,
                        CreatedBy = ve.CreatedBy,
                        UpdatedBy = ve.UpdatedBy
                    })
                    .OrderByDescending(ve => ve.EventDate)
                    .ToListAsync();

                return Results.Ok(events);
            })
            .WithName("GetVehicleEvents")
            .WithSummary("Get vehicle events with optional filtering");

            // GET /api/vehicle-events/{id} - Get vehicle event by ID
            group.MapGet("/{id}", async (AppDbContext db, int id) =>
            {
                var vehicleEvent = await db.VehicleEvents
                    .Include(ve => ve.Vehicle)
                    .Where(ve => ve.Id == id)
                    .Select(ve => new VehicleEventDto
                    {
                        Id = ve.Id,
                        VehicleId = ve.VehicleId,
                        VehicleName = ve.Vehicle.Name,
                        VehicleVIN = ve.Vehicle.VIN,
                        EventType = ve.EventType,
                        EventTypeName = ve.EventType.ToString(),
                        Description = ve.Description,
                        EventDate = ve.EventDate,
                        Location = ve.Location,
                        RecordedBy = ve.RecordedBy,
                        Notes = ve.Notes,
                        AdditionalData = ve.AdditionalData,
                        CreatedAt = ve.CreatedAt,
                        UpdatedAt = ve.UpdatedAt,
                        CreatedBy = ve.CreatedBy,
                        UpdatedBy = ve.UpdatedBy
                    })
                    .FirstOrDefaultAsync();

                return vehicleEvent is not null ? Results.Ok(vehicleEvent) : Results.NotFound();
            })
            .WithName("GetVehicleEventById")
            .WithSummary("Get vehicle event by ID");

            // POST /api/vehicle-events - Create new vehicle event
            group.MapPost("/", async (AppDbContext db, CreateVehicleEventDto createDto) =>
            {
                // Validate vehicle exists
                if (!await db.Vehicles.AnyAsync(v => v.Id == createDto.VehicleId && v.IsActive))
                {
                    return Results.BadRequest("Invalid vehicle ID");
                }

                var vehicleEvent = new VehicleEvent
                {
                    VehicleId = createDto.VehicleId,
                    EventType = createDto.EventType,
                    Description = createDto.Description,
                    EventDate = createDto.EventDate,
                    Location = createDto.Location,
                    RecordedBy = createDto.RecordedBy,
                    Notes = createDto.Notes,
                    AdditionalData = createDto.AdditionalData,
                    CreatedBy = "System" // TODO: Get from authenticated user
                };

                db.VehicleEvents.Add(vehicleEvent);
                await db.SaveChangesAsync();

                // Return the created event with related data
                var createdEvent = await db.VehicleEvents
                    .Include(ve => ve.Vehicle)
                    .Where(ve => ve.Id == vehicleEvent.Id)
                    .Select(ve => new VehicleEventDto
                    {
                        Id = ve.Id,
                        VehicleId = ve.VehicleId,
                        VehicleName = ve.Vehicle.Name,
                        VehicleVIN = ve.Vehicle.VIN,
                        EventType = ve.EventType,
                        EventTypeName = ve.EventType.ToString(),
                        Description = ve.Description,
                        EventDate = ve.EventDate,
                        Location = ve.Location,
                        RecordedBy = ve.RecordedBy,
                        Notes = ve.Notes,
                        AdditionalData = ve.AdditionalData,
                        CreatedAt = ve.CreatedAt,
                        UpdatedAt = ve.UpdatedAt,
                        CreatedBy = ve.CreatedBy,
                        UpdatedBy = ve.UpdatedBy
                    })
                    .FirstAsync();

                return Results.Created($"/api/vehicle-events/{vehicleEvent.Id}", createdEvent);
            })
            .WithName("CreateVehicleEvent")
            .WithSummary("Create a new vehicle event");

            // PUT /api/vehicle-events/{id} - Update vehicle event
            group.MapPut("/{id}", async (AppDbContext db, int id, CreateVehicleEventDto updateDto) =>
            {
                var vehicleEvent = await db.VehicleEvents.FindAsync(id);
                if (vehicleEvent is null)
                {
                    return Results.NotFound();
                }

                vehicleEvent.VehicleId = updateDto.VehicleId;
                vehicleEvent.EventType = updateDto.EventType;
                vehicleEvent.Description = updateDto.Description;
                vehicleEvent.EventDate = updateDto.EventDate;
                vehicleEvent.Location = updateDto.Location;
                vehicleEvent.RecordedBy = updateDto.RecordedBy;
                vehicleEvent.Notes = updateDto.Notes;
                vehicleEvent.AdditionalData = updateDto.AdditionalData;
                vehicleEvent.UpdatedAt = DateTime.UtcNow;
                vehicleEvent.UpdatedBy = "System"; // TODO: Get from authenticated user

                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("UpdateVehicleEvent")
            .WithSummary("Update an existing vehicle event");

            // DELETE /api/vehicle-events/{id} - Delete vehicle event
            group.MapDelete("/{id}", async (AppDbContext db, int id) =>
            {
                var vehicleEvent = await db.VehicleEvents.FindAsync(id);
                if (vehicleEvent is null)
                {
                    return Results.NotFound();
                }

                db.VehicleEvents.Remove(vehicleEvent);
                await db.SaveChangesAsync();
                return Results.NoContent();
            })
            .WithName("DeleteVehicleEvent")
            .WithSummary("Delete a vehicle event");

            // GET /api/vehicle-events/types - Get all event types
            group.MapGet("/types", () =>
            {
                var eventTypes = Enum.GetValues<EventType>()
                    .Select(et => new
                    {
                        Value = (int)et,
                        Name = et.ToString(),
                        DisplayName = et switch
                        {
                            EventType.VehicleRegistered => "Vehicle Registered",
                            EventType.OwnershipChanged => "Ownership Changed",
                            EventType.PermitApplied => "Permit Applied",
                            EventType.PermitApproved => "Permit Approved",
                            EventType.PermitRejected => "Permit Rejected",
                            EventType.InspectionCompleted => "Inspection Completed",
                            EventType.ViolationRecorded => "Violation Recorded",
                            EventType.MaintenancePerformed => "Maintenance Performed",
                            EventType.Other => "Other",
                            _ => et.ToString()
                        }
                    })
                    .ToList();

                return Results.Ok(eventTypes);
            })
            .WithName("GetEventTypes")
            .WithSummary("Get all available event types");

            // GET /api/vehicle-events/timeline/{vehicleId} - Get vehicle event timeline
            group.MapGet("/timeline/{vehicleId}", async (AppDbContext db, int vehicleId) =>
            {
                var vehicle = await db.Vehicles.FindAsync(vehicleId);
                if (vehicle is null)
                {
                    return Results.NotFound("Vehicle not found");
                }

                var timeline = await db.VehicleEvents
                    .Where(ve => ve.VehicleId == vehicleId)
                    .Select(ve => new VehicleEventDto
                    {
                        Id = ve.Id,
                        VehicleId = ve.VehicleId,
                        VehicleName = ve.Vehicle.Name,
                        VehicleVIN = ve.Vehicle.VIN,
                        EventType = ve.EventType,
                        EventTypeName = ve.EventType.ToString(),
                        Description = ve.Description,
                        EventDate = ve.EventDate,
                        Location = ve.Location,
                        RecordedBy = ve.RecordedBy,
                        Notes = ve.Notes,
                        AdditionalData = ve.AdditionalData,
                        CreatedAt = ve.CreatedAt,
                        UpdatedAt = ve.UpdatedAt,
                        CreatedBy = ve.CreatedBy,
                        UpdatedBy = ve.UpdatedBy
                    })
                    .OrderByDescending(ve => ve.EventDate)
                    .ToListAsync();

                return Results.Ok(timeline);
            })
            .WithName("GetVehicleTimeline")
            .WithSummary("Get complete event timeline for a vehicle");

            // GET /api/vehicle-events/summary/{vehicleId} - Get vehicle event summary
            group.MapGet("/summary/{vehicleId}", async (AppDbContext db, int vehicleId) =>
            {
                var vehicle = await db.Vehicles.FindAsync(vehicleId);
                if (vehicle is null)
                {
                    return Results.NotFound("Vehicle not found");
                }

                var summary = await db.VehicleEvents
                    .Where(ve => ve.VehicleId == vehicleId)
                    .GroupBy(ve => ve.EventType)
                    .Select(g => new
                    {
                        EventType = g.Key,
                        EventTypeName = g.Key.ToString(),
                        Count = g.Count(),
                        LastEventDate = g.Max(ve => ve.EventDate),
                        FirstEventDate = g.Min(ve => ve.EventDate)
                    })
                    .ToListAsync();

                var totalEvents = await db.VehicleEvents.CountAsync(ve => ve.VehicleId == vehicleId);
                var firstEvent = await db.VehicleEvents
                    .Where(ve => ve.VehicleId == vehicleId)
                    .OrderBy(ve => ve.EventDate)
                    .FirstOrDefaultAsync();
                var lastEvent = await db.VehicleEvents
                    .Where(ve => ve.VehicleId == vehicleId)
                    .OrderByDescending(ve => ve.EventDate)
                    .FirstOrDefaultAsync();

                var result = new
                {
                    VehicleId = vehicleId,
                    TotalEvents = totalEvents,
                    FirstEventDate = firstEvent?.EventDate,
                    LastEventDate = lastEvent?.EventDate,
                    EventsByType = summary
                };

                return Results.Ok(result);
            })
            .WithName("GetVehicleEventSummary")
            .WithSummary("Get event summary statistics for a vehicle");
        }
    }
}
