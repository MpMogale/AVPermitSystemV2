using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.API.Endpoints
{
    public class DashboardEndpoints : IEndpoint
    {
        public void RegisterEndpoints(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/api/dashboard")
                .WithTags("Dashboard")
                .WithOpenApi();

            // GET /api/dashboard/statistics - Get system statistics
            group.MapGet("/statistics", async (AppDbContext db) =>
            {
                var stats = new
                {
                    Vehicles = new
                    {
                        Total = await db.Vehicles.CountAsync(v => v.IsActive),
                        ByType = await db.Vehicles
                            .Include(v => v.VehicleType)
                            .Where(v => v.IsActive)
                            .GroupBy(v => v.VehicleType.Name)
                            .Select(g => new { Type = g.Key, Count = g.Count() })
                            .ToListAsync(),
                        RecentlyRegistered = await db.Vehicles
                            .Where(v => v.IsActive && v.CreatedAt >= DateTime.UtcNow.AddDays(-30))
                            .CountAsync()
                    },
                    Owners = new
                    {
                        Total = await db.Owners.CountAsync(o => o.IsActive),
                        ByType = await db.Owners
                            .Where(o => o.IsActive)
                            .GroupBy(o => o.OwnerType)
                            .Select(g => new { Type = g.Key.ToString(), Count = g.Count() })
                            .ToListAsync()
                    },
                    Permits = new
                    {
                        Total = await db.Permits.CountAsync(),
                        ByStatus = await db.Permits
                            .GroupBy(p => p.Status)
                            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
                            .ToListAsync(),
                        ActivePermits = await db.Permits
                            .CountAsync(p => p.Status == PermitStatus.Approved && 
                                           DateTime.UtcNow >= p.ValidFromDate && 
                                           DateTime.UtcNow <= p.ValidToDate),
                        ExpiringThisWeek = await db.Permits
                            .CountAsync(p => p.Status == PermitStatus.Approved && 
                                           p.ValidToDate <= DateTime.UtcNow.AddDays(7) &&
                                           p.ValidToDate >= DateTime.UtcNow)
                    },
                    Components = new
                    {
                        Total = await db.VehicleComponents.CountAsync(vc => vc.IsActive),
                        ByType = await db.VehicleComponents
                            .Include(vc => vc.ComponentType)
                            .Where(vc => vc.IsActive)
                            .GroupBy(vc => vc.ComponentType.Name)
                            .Select(g => new { Type = g.Key, Count = g.Count() })
                            .ToListAsync()
                    },
                    Events = new
                    {
                        TotalEvents = await db.VehicleEvents.CountAsync(),
                        RecentEvents = await db.VehicleEvents
                            .Where(ve => ve.EventDate >= DateTime.UtcNow.AddDays(-7))
                            .CountAsync(),
                        EventsByType = await db.VehicleEvents
                            .Where(ve => ve.EventDate >= DateTime.UtcNow.AddDays(-30))
                            .GroupBy(ve => ve.EventType)
                            .Select(g => new { Type = g.Key.ToString(), Count = g.Count() })
                            .ToListAsync()
                    },
                    Routes = new
                    {
                        Total = await db.Routes.CountAsync(r => r.IsActive),
                        MostUsedRoutes = await db.PermitRoutes
                            .Include(pr => pr.Route)
                            .GroupBy(pr => pr.Route.Name)
                            .Select(g => new { RouteName = g.Key, UsageCount = g.Count() })
                            .OrderByDescending(x => x.UsageCount)
                            .Take(5)
                            .ToListAsync()
                    }
                };

                return Results.Ok(stats);
            })
            .WithName("GetDashboardStatistics")
            .WithSummary("Get comprehensive system statistics for dashboard");

            // GET /api/dashboard/recent-activity - Get recent system activity
            group.MapGet("/recent-activity", async (AppDbContext db, int limit = 20) =>
            {
                var recentEvents = await db.VehicleEvents
                    .Include(ve => ve.Vehicle)
                    .OrderByDescending(ve => ve.EventDate)
                    .Take(limit)
                    .Select(ve => new
                    {
                        Id = ve.Id,
                        EventType = ve.EventType.ToString(),
                        Description = ve.Description,
                        VehicleName = ve.Vehicle.Name,
                        VehicleVIN = ve.Vehicle.VIN,
                        EventDate = ve.EventDate,
                        Location = ve.Location,
                        RecordedBy = ve.RecordedBy
                    })
                    .ToListAsync();

                var recentPermits = await db.Permits
                    .Include(p => p.Vehicle)
                    .Include(p => p.PermitType)
                    .OrderByDescending(p => p.CreatedAt)
                    .Take(10)
                    .Select(p => new
                    {
                        Id = p.Id,
                        PermitNumber = p.PermitNumber,
                        VehicleName = p.Vehicle.Name,
                        PermitType = p.PermitType.Name,
                        Status = p.Status.ToString(),
                        ApplicationDate = p.ApplicationDate,
                        ValidFromDate = p.ValidFromDate,
                        ValidToDate = p.ValidToDate
                    })
                    .ToListAsync();

                var activity = new
                {
                    RecentEvents = recentEvents,
                    RecentPermits = recentPermits
                };

                return Results.Ok(activity);
            })
            .WithName("GetRecentActivity")
            .WithSummary("Get recent system activity");

            // GET /api/dashboard/alerts - Get system alerts
            group.MapGet("/alerts", async (AppDbContext db) =>
            {
                var now = DateTime.UtcNow;
                var weekFromNow = now.AddDays(7);
                var monthFromNow = now.AddDays(30);

                var alerts = new
                {
                    ExpiringPermits = await db.Permits
                        .Include(p => p.Vehicle)
                        .Where(p => p.Status == PermitStatus.Approved && 
                                   p.ValidToDate <= weekFromNow && 
                                   p.ValidToDate >= now)
                        .Select(p => new
                        {
                            PermitNumber = p.PermitNumber,
                            VehicleName = p.Vehicle.Name,
                            VehicleVIN = p.Vehicle.VIN,
                            ExpiryDate = p.ValidToDate,
                            DaysUntilExpiry = (p.ValidToDate - now).Days
                        })
                        .OrderBy(p => p.ExpiryDate)
                        .ToListAsync(),

                    PendingPermits = await db.Permits
                        .Include(p => p.Vehicle)
                        .Where(p => p.Status == PermitStatus.Submitted || p.Status == PermitStatus.UnderReview)
                        .CountAsync(),

                    VehiclesWithoutActivePermits = await db.Vehicles
                        .Where(v => v.IsActive && !v.Permits.Any(p => 
                            p.Status == PermitStatus.Approved && 
                            now >= p.ValidFromDate && 
                            now <= p.ValidToDate))
                        .CountAsync(),

                    RecentViolations = await db.VehicleEvents
                        .Include(ve => ve.Vehicle)
                        .Where(ve => ve.EventType == EventType.ViolationRecorded && 
                                    ve.EventDate >= now.AddDays(-30))
                        .Select(ve => new
                        {
                            VehicleName = ve.Vehicle.Name,
                            VehicleVIN = ve.Vehicle.VIN,
                            Description = ve.Description,
                            EventDate = ve.EventDate,
                            Location = ve.Location
                        })
                        .OrderByDescending(ve => ve.EventDate)
                        .ToListAsync()
                };

                return Results.Ok(alerts);
            })
            .WithName("GetSystemAlerts")
            .WithSummary("Get system alerts and warnings");

            // GET /api/dashboard/financial-summary - Get financial summary
            group.MapGet("/financial-summary", async (AppDbContext db) =>
            {
                var now = DateTime.UtcNow;
                var startOfMonth = new DateTime(now.Year, now.Month, 1);
                var startOfYear = new DateTime(now.Year, 1, 1);

                var financialSummary = new
                {
                    ThisMonth = new
                    {
                        PermitRevenue = await db.Permits
                            .Where(p => p.CreatedAt >= startOfMonth)
                            .SumAsync(p => p.Fee),
                        PermitCount = await db.Permits
                            .CountAsync(p => p.CreatedAt >= startOfMonth)
                    },
                    ThisYear = new
                    {
                        PermitRevenue = await db.Permits
                            .Where(p => p.CreatedAt >= startOfYear)
                            .SumAsync(p => p.Fee),
                        PermitCount = await db.Permits
                            .CountAsync(p => p.CreatedAt >= startOfYear)
                    },
                    RevenueByPermitType = await db.Permits
                        .Include(p => p.PermitType)
                        .Where(p => p.CreatedAt >= startOfYear)
                        .GroupBy(p => p.PermitType.Name)
                        .Select(g => new 
                        { 
                            PermitType = g.Key, 
                            Revenue = g.Sum(p => p.Fee),
                            Count = g.Count()
                        })
                        .ToListAsync(),
                    MonthlyTrend = await db.Permits
                        .Where(p => p.CreatedAt >= startOfYear)
                        .GroupBy(p => new { Year = p.CreatedAt.Year, Month = p.CreatedAt.Month })
                        .Select(g => new 
                        { 
                            Year = g.Key.Year,
                            Month = g.Key.Month,
                            Revenue = g.Sum(p => p.Fee),
                            Count = g.Count()
                        })
                        .OrderBy(g => g.Year).ThenBy(g => g.Month)
                        .ToListAsync()
                };

                return Results.Ok(financialSummary);
            })
            .WithName("GetFinancialSummary")
            .WithSummary("Get financial summary and revenue statistics");
        }
    }
}
