using AVPermitSystemV2.Domain.Entities;
using AVPermitSystemV2.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.Application.Services;

public interface IReportingService
{
    Task<PermitStatisticsReport> GetPermitStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null);
    Task<VehicleActivityReport> GetVehicleActivityReportAsync(int vehicleId);
    Task<OwnerReport> GetOwnerReportAsync(int ownerId);
    Task<RouteUsageReport> GetRouteUsageReportAsync(int routeId);
    Task<byte[]> ExportPermitsToExcelAsync(DateTime? fromDate = null, DateTime? toDate = null);
}

public class ReportingService : IReportingService
{
    private readonly AppDbContext _context;

    public ReportingService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<PermitStatisticsReport> GetPermitStatisticsAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = _context.Permits.AsQueryable();

        if (fromDate.HasValue)
            query = query.Where(p => p.CreatedAt >= fromDate.Value);
        if (toDate.HasValue)
            query = query.Where(p => p.CreatedAt <= toDate.Value);

        var totalPermits = await query.CountAsync();
        var approvedPermits = await query.CountAsync(p => p.Status == PermitStatus.Approved);
        var rejectedPermits = await query.CountAsync(p => p.Status == PermitStatus.Rejected);
        var pendingPermits = await query.CountAsync(p => p.Status == PermitStatus.Submitted || p.Status == PermitStatus.UnderReview);
        var expiredPermits = await query.CountAsync(p => p.Status == PermitStatus.Expired);

        var permitsByType = await query
            .Include(p => p.PermitType)
            .GroupBy(p => p.PermitType.Name)
            .Select(g => new PermitTypeStatistic
            {
                PermitTypeName = g.Key,
                Count = g.Count(),
                ApprovedCount = g.Count(p => p.Status == PermitStatus.Approved),
                RejectedCount = g.Count(p => p.Status == PermitStatus.Rejected)
            })
            .ToListAsync();

        var permitsByMonth = await query
            .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
            .Select(g => new MonthlyStatistic
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToListAsync();

        return new PermitStatisticsReport
        {
            FromDate = fromDate,
            ToDate = toDate,
            TotalPermits = totalPermits,
            ApprovedPermits = approvedPermits,
            RejectedPermits = rejectedPermits,
            PendingPermits = pendingPermits,
            ExpiredPermits = expiredPermits,
            ApprovalRate = totalPermits > 0 ? (double)approvedPermits / totalPermits * 100 : 0,
            PermitsByType = permitsByType,
            PermitsByMonth = permitsByMonth,
            GeneratedAt = DateTime.UtcNow
        };
    }

    public async Task<VehicleActivityReport> GetVehicleActivityReportAsync(int vehicleId)
    {
        var vehicle = await _context.Vehicles
            .Include(v => v.Manufacturer)
            .Include(v => v.VehicleType)
            .FirstOrDefaultAsync(v => v.Id == vehicleId);

        if (vehicle == null)
            throw new ArgumentException("Vehicle not found", nameof(vehicleId));

        var permits = await _context.Permits
            .Include(p => p.PermitType)
            .Where(p => p.VehicleId == vehicleId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        var events = await _context.VehicleEvents
            .Include(ve => ve.EventType)
            .Where(ve => ve.VehicleId == vehicleId)
            .OrderByDescending(ve => ve.EventDate)
            .ToListAsync();

        var ownerships = await _context.VehicleOwnerships
            .Include(vo => vo.Owner)
            .Where(vo => vo.VehicleId == vehicleId)
            .OrderByDescending(vo => vo.StartDate)
            .ToListAsync();

        return new VehicleActivityReport
        {
            Vehicle = vehicle,
            TotalPermits = permits.Count,
            ActivePermits = permits.Count(p => p.Status == PermitStatus.Approved),
            RecentPermits = permits.Take(10).ToList(),
            RecentEvents = events.Take(10).ToList(),
            OwnershipHistory = ownerships,
            GeneratedAt = DateTime.UtcNow
        };
    }

    public async Task<OwnerReport> GetOwnerReportAsync(int ownerId)
    {
        var owner = await _context.Owners.FindAsync(ownerId);
        if (owner == null)
            throw new ArgumentException("Owner not found", nameof(ownerId));

        var vehicles = await _context.VehicleOwnerships
            .Include(vo => vo.Vehicle)
            .ThenInclude(v => v.Manufacturer)
            .Include(vo => vo.Vehicle)
            .ThenInclude(v => v.VehicleType)
            .Where(vo => vo.OwnerId == ownerId)
            .GroupBy(vo => vo.Vehicle)
            .Select(g => g.Key)
            .ToListAsync();

        var permits = await _context.Permits
            .Include(p => p.Vehicle)
            .Include(p => p.PermitType)
            .Where(p => vehicles.Select(v => v.Id).Contains(p.VehicleId))
            .ToListAsync();

        return new OwnerReport
        {
            Owner = owner,
            TotalVehicles = vehicles.Count,
            ActiveVehicles = vehicles.Count(v => v.IsActive),
            TotalPermits = permits.Count,
            ActivePermits = permits.Count(p => p.Status == PermitStatus.Approved),
            Vehicles = vehicles,
            RecentPermits = permits.OrderByDescending(p => p.CreatedAt).Take(10).ToList(),
            GeneratedAt = DateTime.UtcNow
        };
    }

    public async Task<RouteUsageReport> GetRouteUsageReportAsync(int routeId)
    {
        var route = await _context.Routes.FindAsync(routeId);
        if (route == null)
            throw new ArgumentException("Route not found", nameof(routeId));

        var permits = await _context.PermitRoutes
            .Include(pr => pr.Permit)
            .ThenInclude(p => p.Vehicle)
            .Include(pr => pr.Permit)
            .ThenInclude(p => p.PermitType)
            .Where(pr => pr.RouteId == routeId)
            .Select(pr => pr.Permit)
            .ToListAsync();

        var usageByMonth = permits
            .GroupBy(p => new { p.CreatedAt.Year, p.CreatedAt.Month })
            .Select(g => new MonthlyStatistic
            {
                Year = g.Key.Year,
                Month = g.Key.Month,
                Count = g.Count()
            })
            .OrderBy(m => m.Year)
            .ThenBy(m => m.Month)
            .ToList();

        return new RouteUsageReport
        {
            Route = route,
            TotalPermits = permits.Count,
            ActivePermits = permits.Count(p => p.Status == PermitStatus.Approved),
            UsageByMonth = usageByMonth,
            RecentPermits = permits.OrderByDescending(p => p.CreatedAt).Take(10).ToList(),
            GeneratedAt = DateTime.UtcNow
        };
    }

    public async Task<byte[]> ExportPermitsToExcelAsync(DateTime? fromDate = null, DateTime? toDate = null)
    {
        // This would require a library like EPPlus or ClosedXML
        // For now, returning empty array as placeholder
        await Task.CompletedTask;
        return Array.Empty<byte>();
    }
}

// Report DTOs
public class PermitStatisticsReport
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public int TotalPermits { get; set; }
    public int ApprovedPermits { get; set; }
    public int RejectedPermits { get; set; }
    public int PendingPermits { get; set; }
    public int ExpiredPermits { get; set; }
    public double ApprovalRate { get; set; }
    public List<PermitTypeStatistic> PermitsByType { get; set; } = new();
    public List<MonthlyStatistic> PermitsByMonth { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}

public class PermitTypeStatistic
{
    public string PermitTypeName { get; set; } = string.Empty;
    public int Count { get; set; }
    public int ApprovedCount { get; set; }
    public int RejectedCount { get; set; }
}

public class MonthlyStatistic
{
    public int Year { get; set; }
    public int Month { get; set; }
    public int Count { get; set; }
}

public class VehicleActivityReport
{
    public Vehicle Vehicle { get; set; } = null!;
    public int TotalPermits { get; set; }
    public int ActivePermits { get; set; }
    public List<Permit> RecentPermits { get; set; } = new();
    public List<VehicleEvent> RecentEvents { get; set; } = new();
    public List<VehicleOwnership> OwnershipHistory { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}

public class OwnerReport
{
    public Owner Owner { get; set; } = null!;
    public int TotalVehicles { get; set; }
    public int ActiveVehicles { get; set; }
    public int TotalPermits { get; set; }
    public int ActivePermits { get; set; }
    public List<Vehicle> Vehicles { get; set; } = new();
    public List<Permit> RecentPermits { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}

public class RouteUsageReport
{
    public Route Route { get; set; } = null!;
    public int TotalPermits { get; set; }
    public int ActivePermits { get; set; }
    public List<MonthlyStatistic> UsageByMonth { get; set; } = new();
    public List<Permit> RecentPermits { get; set; } = new();
    public DateTime GeneratedAt { get; set; }
}
