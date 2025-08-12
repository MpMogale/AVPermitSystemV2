
using AVPermitSystemV2.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AVPermitSystemV2.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // Vehicle Registry (Epic 1)
    public DbSet<Manufacturer> Manufacturers { get; set; } = null!;
    public DbSet<Vehicle> Vehicles { get; set; } = null!;
    public DbSet<VehicleType> VehicleTypes { get; set; } = null!;
    public DbSet<VehicleCategory> VehicleCategories { get; set; } = null!;
    public DbSet<ComponentType> ComponentTypes { get; set; } = null!;
    public DbSet<VehicleComponent> VehicleComponents { get; set; } = null!;
    public DbSet<ComponentDimension> ComponentDimensions { get; set; } = null!;

    // Axle Management (Epic 2)
    public DbSet<AxleGroup> AxleGroups { get; set; } = null!;
    public DbSet<Axle> Axles { get; set; } = null!;

    // Ownership (Epic 3)
    public DbSet<Owner> Owners { get; set; } = null!;
    public DbSet<VehicleOwnership> VehicleOwnerships { get; set; } = null!;

    // Permit Management (Epic 4)
    public DbSet<PermitType> PermitTypes { get; set; } = null!;
    public DbSet<Permit> Permits { get; set; } = null!;
    public DbSet<PermitConstraint> PermitConstraints { get; set; } = null!;

    // Load Management (Epic 5)
    public DbSet<Load> Loads { get; set; } = null!;
    public DbSet<LoadDimension> LoadDimensions { get; set; } = null!;
    public DbSet<LoadProjection> LoadProjections { get; set; } = null!;

    // Routing (Epic 6)
    public DbSet<Route> Routes { get; set; } = null!;
    public DbSet<PermitRoute> PermitRoutes { get; set; } = null!;

    // Event Tracking (Epic 7)
    public DbSet<VehicleEvent> VehicleEvents { get; set; } = null!;

    // Specification Modules (Epic 8)
    public DbSet<TruckSpecification> TruckSpecifications { get; set; } = null!;
    public DbSet<CraneSpecification> CraneSpecifications { get; set; } = null!;
    public DbSet<TrailerSpecification> TrailerSpecifications { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Vehicle Configuration
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(v => v.Id);
            entity.HasIndex(v => v.VIN).IsUnique();

            entity.HasOne(v => v.Manufacturer)
                .WithMany(m => m.Vehicles)
                .HasForeignKey(v => v.ManufacturerId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(v => v.VehicleType)
                .WithMany()
                .HasForeignKey(v => v.VehicleTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(v => v.VehicleCategory)
                .WithMany()
                .HasForeignKey(v => v.VehicleCategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Manufacturer Configuration
        modelBuilder.Entity<Manufacturer>(entity =>
        {
            entity.HasKey(m => m.Id);
            entity.HasIndex(m => m.Name).IsUnique();
        });

        // Vehicle Component Configuration
        modelBuilder.Entity<VehicleComponent>(entity =>
        {
            entity.HasKey(vc => vc.Id);
            entity.HasIndex(vc => new { vc.VehicleId, vc.Position }).IsUnique();

            entity.HasOne(vc => vc.Vehicle)
                .WithMany(v => v.Components)
                .HasForeignKey(vc => vc.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(vc => vc.ComponentType)
                .WithMany(ct => ct.VehicleComponents)
                .HasForeignKey(vc => vc.ComponentTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Component Dimension Configuration (One-to-One)
        modelBuilder.Entity<ComponentDimension>(entity =>
        {
            entity.HasKey(cd => cd.Id);
            entity.HasOne(cd => cd.Component)
                .WithOne(c => c.Dimensions)
                .HasForeignKey<ComponentDimension>(cd => cd.ComponentId);
        });

        // Axle Group Configuration
        modelBuilder.Entity<AxleGroup>(entity =>
        {
            entity.HasKey(ag => ag.Id);
            entity.HasOne(ag => ag.Component)
                .WithMany(c => c.AxleGroups)
                .HasForeignKey(ag => ag.ComponentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Axle Configuration
        modelBuilder.Entity<Axle>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasOne(a => a.AxleGroup)
                .WithMany(ag => ag.Axles)
                .HasForeignKey(a => a.AxleGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Vehicle Ownership Configuration
        modelBuilder.Entity<VehicleOwnership>(entity =>
        {
            entity.HasKey(vo => vo.Id);
            entity.HasOne(vo => vo.Vehicle)
                .WithMany(v => v.Ownerships)
                .HasForeignKey(vo => vo.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(vo => vo.Owner)
                .WithMany(o => o.VehicleOwnerships)
                .HasForeignKey(vo => vo.OwnerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Permit Configuration
        modelBuilder.Entity<Permit>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasIndex(p => p.PermitNumber).IsUnique();

            entity.HasOne(p => p.Vehicle)
                .WithMany(v => v.Permits)
                .HasForeignKey(p => p.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.PermitType)
                .WithMany(pt => pt.Permits)
                .HasForeignKey(p => p.PermitTypeId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Load Configuration (One-to-One with Permit)
        modelBuilder.Entity<Load>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasOne(l => l.Permit)
                .WithOne(p => p.Load)
                .HasForeignKey<Load>(l => l.PermitId);
        });

        // Route Configuration
        modelBuilder.Entity<PermitRoute>(entity =>
        {
            entity.HasKey(pr => pr.Id);
            entity.HasOne(pr => pr.Permit)
                .WithMany(p => p.Routes)
                .HasForeignKey(pr => pr.PermitId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(pr => pr.Route)
                .WithMany(r => r.PermitRoutes)
                .HasForeignKey(pr => pr.RouteId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Vehicle Event Configuration
        modelBuilder.Entity<VehicleEvent>(entity =>
        {
            entity.HasKey(ve => ve.Id);
            entity.HasOne(ve => ve.Vehicle)
                .WithMany(v => v.Events)
                .HasForeignKey(ve => ve.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Specification Module Configurations (Epic 8)
        modelBuilder.Entity<TruckSpecification>(entity =>
        {
            entity.HasKey(ts => ts.Id);
            entity.HasIndex(ts => ts.VehicleId).IsUnique(); // One truck spec per vehicle
            entity.HasOne(ts => ts.Vehicle)
                .WithOne()
                .HasForeignKey<TruckSpecification>(ts => ts.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<CraneSpecification>(entity =>
        {
            entity.HasKey(cs => cs.Id);
            entity.HasIndex(cs => cs.VehicleId).IsUnique(); // One crane spec per vehicle
            entity.HasOne(cs => cs.Vehicle)
                .WithOne()
                .HasForeignKey<CraneSpecification>(cs => cs.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<TrailerSpecification>(entity =>
        {
            entity.HasKey(ts => ts.Id);
            entity.HasIndex(ts => ts.VehicleId).IsUnique(); // One trailer spec per vehicle
            entity.HasOne(ts => ts.Vehicle)
                .WithOne()
                .HasForeignKey<TrailerSpecification>(ts => ts.VehicleId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed data
        SeedData(modelBuilder);
    }

    private void SeedData(ModelBuilder modelBuilder)
    {
        // Seed Manufacturers
        modelBuilder.Entity<Manufacturer>().HasData(
            new Manufacturer { Id = 1, Name = "Volvo Group", Code = "VOL", CountryCode = "SE", CreatedBy = "System", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Manufacturer { Id = 2, Name = "Mercedes-Benz", Code = "MB", CountryCode = "DE", CreatedBy = "System", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) },
            new Manufacturer { Id = 3, Name = "Scania", Code = "SCA", CountryCode = "SE", CreatedBy = "System", CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc) }
        );

        // Seed VehicleTypes
        modelBuilder.Entity<VehicleType>().HasData(
            new VehicleType { Id = 1, Name = "Truck", Description = "Commercial truck" },
            new VehicleType { Id = 2, Name = "Trailer", Description = "Trailer unit" },
            new VehicleType { Id = 3, Name = "Crane", Description = "Mobile crane" },
            new VehicleType { Id = 4, Name = "Semi-trailer", Description = "Semi-trailer unit" }
        );

        // Seed ComponentTypes
        modelBuilder.Entity<ComponentType>().HasData(
            new ComponentType { Id = 1, Name = "Prime Mover", Code = "PM", Description = "Main truck unit" },
            new ComponentType { Id = 2, Name = "Semi-trailer", Code = "ST", Description = "Semi-trailer" },
            new ComponentType { Id = 3, Name = "Dolly", Code = "DL", Description = "Converter dolly" },
            new ComponentType { Id = 4, Name = "Dog Trailer", Code = "DT", Description = "Dog trailer" }
        );

        // Seed PermitTypes
        modelBuilder.Entity<PermitType>().HasData(
            new PermitType { Id = 1, Name = "Standard", Code = "STD", Description = "Standard permit", Fee = 50.00m, ValidityDays = 30 },
            new PermitType { Id = 2, Name = "Abnormal Load", Code = "ABN", Description = "Abnormal load permit", Fee = 150.00m, ValidityDays = 7 },
            new PermitType { Id = 3, Name = "Annual", Code = "ANN", Description = "Annual permit", Fee = 500.00m, ValidityDays = 365 }
        );

        // Seed Vehicle Categories
        modelBuilder.Entity<VehicleCategory>().HasData(
            new VehicleCategory 
            { 
                Id = 1, 
                Name = "Standard Commercial", 
                Description = "Standard commercial vehicle limits",
                MaxLengthMm = 18500,
                MaxWidthMm = 2550,
                MaxHeightMm = 4300,
                MaxWeightKg = 56000
            },
            new VehicleCategory 
            { 
                Id = 2, 
                Name = "Abnormal Load", 
                Description = "Abnormal load vehicle category",
                MaxLengthMm = 50000,
                MaxWidthMm = 4500,
                MaxHeightMm = 6000,
                MaxWeightKg = 200000
            }
        );
    }
}
