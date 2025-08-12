using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AVPermitSystemV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComponentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Manufacturers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ContactPhone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ContactEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    CountryCode = table.Column<string>(type: "TEXT", maxLength: 2, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Manufacturers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Owners",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    OwnerType = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Address = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    ContactPhone = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    ContactEmail = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PermitTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Code = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Fee = table.Column<decimal>(type: "TEXT", nullable: false),
                    ValidityDays = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Routes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Origin = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Destination = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    DistanceKm = table.Column<decimal>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Waypoints = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Routes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    MaxLengthMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxWidthMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxHeightMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    MaxWeightKg = table.Column<decimal>(type: "TEXT", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VehicleTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VIN = table.Column<string>(type: "TEXT", maxLength: 17, nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ManufacturerId = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    VehicleCategoryId = table.Column<int>(type: "INTEGER", nullable: true),
                    Model = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    YearOfManufacture = table.Column<int>(type: "INTEGER", nullable: true),
                    Color = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    GrossVehicleMass = table.Column<decimal>(type: "TEXT", nullable: true),
                    UnladenMass = table.Column<decimal>(type: "TEXT", nullable: true),
                    LengthMm = table.Column<int>(type: "INTEGER", nullable: false),
                    WidthMm = table.Column<int>(type: "INTEGER", nullable: false),
                    HeightMm = table.Column<int>(type: "INTEGER", nullable: false),
                    WheelbaseMm = table.Column<int>(type: "INTEGER", nullable: false),
                    FrontOverhangMm = table.Column<int>(type: "INTEGER", nullable: false),
                    RearOverhangMm = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vehicles_Manufacturers_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Manufacturers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleCategories_VehicleCategoryId",
                        column: x => x.VehicleCategoryId,
                        principalTable: "VehicleCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Vehicles_VehicleTypes_VehicleTypeId",
                        column: x => x.VehicleTypeId,
                        principalTable: "VehicleTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CraneSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    CraneType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MaxLiftingCapacityKg = table.Column<decimal>(type: "TEXT", nullable: true),
                    MaxReachM = table.Column<decimal>(type: "TEXT", nullable: true),
                    MaxLiftingHeightM = table.Column<decimal>(type: "TEXT", nullable: true),
                    BoomType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NumberOfAxles = table.Column<int>(type: "INTEGER", nullable: true),
                    CounterweightKg = table.Column<decimal>(type: "TEXT", nullable: true),
                    OutriggerType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    OutriggerExtensionM = table.Column<decimal>(type: "TEXT", nullable: true),
                    ControlType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    HasLoadMomentIndicator = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasAntiTwoBlocking = table.Column<bool>(type: "INTEGER", nullable: false),
                    CertificationStandard = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastInspectionDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CertificationExpiryDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    SafetyFeatures = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    OperationalLimitations = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraneSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CraneSpecifications_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Permits",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermitNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    PermitTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    ValidFromDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    ValidToDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Purpose = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Fee = table.Column<decimal>(type: "TEXT", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Permits_PermitTypes_PermitTypeId",
                        column: x => x.PermitTypeId,
                        principalTable: "PermitTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Permits_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TrailerSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    TrailerType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    DeckLengthMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    DeckWidthMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    DeckHeightFromGroundMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    LoadingRampLengthMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    LoadingRampWidthMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    NumberOfAxles = table.Column<int>(type: "INTEGER", nullable: true),
                    AxleSpacingMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    SuspensionType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    BrakeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    HasABS = table.Column<bool>(type: "INTEGER", nullable: false),
                    TyreSize = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NumberOfTyres = table.Column<int>(type: "INTEGER", nullable: true),
                    CouplingType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    KingpinToRearAxleMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    HasTieDownPoints = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumberOfTieDownPoints = table.Column<int>(type: "INTEGER", nullable: true),
                    SideBoardType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    HasTarps = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasWinch = table.Column<bool>(type: "INTEGER", nullable: false),
                    FloorMaterial = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FloorThicknessMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    SpecialFeatures = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    LoadingInstructions = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrailerSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TrailerSpecifications_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TruckSpecifications",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    EngineType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    EngineCapacityLitres = table.Column<decimal>(type: "TEXT", nullable: true),
                    FuelType = table.Column<string>(type: "TEXT", maxLength: 30, nullable: false),
                    PowerKw = table.Column<decimal>(type: "TEXT", nullable: true),
                    TorqueNm = table.Column<decimal>(type: "TEXT", nullable: true),
                    TransmissionType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    NumberOfGears = table.Column<int>(type: "INTEGER", nullable: true),
                    DriveConfiguration = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    FuelTankCapacityLitres = table.Column<decimal>(type: "TEXT", nullable: true),
                    EmissionStandard = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    MaxTowingCapacityKg = table.Column<decimal>(type: "TEXT", nullable: true),
                    BrakeType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    HasABS = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasESC = table.Column<bool>(type: "INTEGER", nullable: false),
                    HasRetarder = table.Column<bool>(type: "INTEGER", nullable: false),
                    CabType = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    AdditionalFeatures = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TruckSpecifications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TruckSpecifications_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleComponents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    ComponentTypeId = table.Column<int>(type: "INTEGER", nullable: false),
                    RegistrationNumber = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SerialNumber = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    ManufacturerName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Model = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    YearOfManufacture = table.Column<int>(type: "INTEGER", nullable: true),
                    Mass = table.Column<decimal>(type: "TEXT", nullable: true),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    IsActive = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleComponents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleComponents_ComponentTypes_ComponentTypeId",
                        column: x => x.ComponentTypeId,
                        principalTable: "ComponentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleComponents_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleEvents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    EventType = table.Column<int>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Location = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    RecordedBy = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    AdditionalData = table.Column<string>(type: "TEXT", maxLength: 1000, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleEvents_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VehicleOwnerships",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: false),
                    OwnerId = table.Column<int>(type: "INTEGER", nullable: false),
                    StartDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    EndDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    IsPrimaryOwner = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleOwnerships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleOwnerships_Owners_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "Owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VehicleOwnerships_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loads",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermitId = table.Column<int>(type: "INTEGER", nullable: false),
                    LoadType = table.Column<int>(type: "INTEGER", nullable: false),
                    WeightKg = table.Column<decimal>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false),
                    CargoType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    IsIndivisible = table.Column<bool>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loads", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Loads_Permits_PermitId",
                        column: x => x.PermitId,
                        principalTable: "Permits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermitConstraints",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermitId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConstraintType = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 500, nullable: false),
                    Value = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitConstraints", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermitConstraints_Permits_PermitId",
                        column: x => x.PermitId,
                        principalTable: "Permits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PermitRoutes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PermitId = table.Column<int>(type: "INTEGER", nullable: false),
                    RouteId = table.Column<int>(type: "INTEGER", nullable: false),
                    Sequence = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduledDate = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PermitRoutes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PermitRoutes_Permits_PermitId",
                        column: x => x.PermitId,
                        principalTable: "Permits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PermitRoutes_Routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "Routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AxleGroups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComponentId = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    SpacingMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    UnladenMass = table.Column<decimal>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AxleGroups", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AxleGroups_VehicleComponents_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "VehicleComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ComponentDimensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ComponentId = table.Column<int>(type: "INTEGER", nullable: false),
                    LengthMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    WidthMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    HeightMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    FrontOverhangMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    RearOverhangMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    LeftOverhangMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    RightOverhangMm = table.Column<decimal>(type: "TEXT", nullable: true),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentDimensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComponentDimensions_VehicleComponents_ComponentId",
                        column: x => x.ComponentId,
                        principalTable: "VehicleComponents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoadDimensions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoadId = table.Column<int>(type: "INTEGER", nullable: false),
                    LengthMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    WidthMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    HeightMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadDimensions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadDimensions_Loads_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Loads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LoadProjections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LoadId = table.Column<int>(type: "INTEGER", nullable: false),
                    Direction = table.Column<string>(type: "TEXT", maxLength: 20, nullable: false),
                    ProjectionMm = table.Column<decimal>(type: "TEXT", nullable: false),
                    Unit = table.Column<string>(type: "TEXT", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoadProjections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoadProjections_Loads_LoadId",
                        column: x => x.LoadId,
                        principalTable: "Loads",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Axles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AxleGroupId = table.Column<int>(type: "INTEGER", nullable: false),
                    TyreCount = table.Column<int>(type: "INTEGER", nullable: false),
                    LoadKg = table.Column<decimal>(type: "TEXT", nullable: false),
                    Position = table.Column<int>(type: "INTEGER", nullable: false),
                    TyreSize = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "TEXT", nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", nullable: false),
                    UpdatedBy = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Axles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Axles_AxleGroups_AxleGroupId",
                        column: x => x.AxleGroupId,
                        principalTable: "AxleGroups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "ComponentTypes",
                columns: new[] { "Id", "Code", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "PM", "Main truck unit", true, "Prime Mover" },
                    { 2, "ST", "Semi-trailer", true, "Semi-trailer" },
                    { 3, "DL", "Converter dolly", true, "Dolly" },
                    { 4, "DT", "Dog trailer", true, "Dog Trailer" }
                });

            migrationBuilder.InsertData(
                table: "Manufacturers",
                columns: new[] { "Id", "Address", "Code", "ContactEmail", "ContactPhone", "CountryCode", "CreatedAt", "CreatedBy", "IsActive", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "", "VOL", "", "", "SE", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", true, "Volvo Group", null, null },
                    { 2, "", "MB", "", "", "DE", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", true, "Mercedes-Benz", null, null },
                    { 3, "", "SCA", "", "", "SE", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "System", true, "Scania", null, null }
                });

            migrationBuilder.InsertData(
                table: "PermitTypes",
                columns: new[] { "Id", "Code", "Description", "Fee", "IsActive", "Name", "ValidityDays" },
                values: new object[,]
                {
                    { 1, "STD", "Standard permit", 50.00m, true, "Standard", 30 },
                    { 2, "ABN", "Abnormal load permit", 150.00m, true, "Abnormal Load", 7 },
                    { 3, "ANN", "Annual permit", 500.00m, true, "Annual", 365 }
                });

            migrationBuilder.InsertData(
                table: "VehicleCategories",
                columns: new[] { "Id", "Description", "IsActive", "MaxHeightMm", "MaxLengthMm", "MaxWeightKg", "MaxWidthMm", "Name" },
                values: new object[,]
                {
                    { 1, "Standard commercial vehicle limits", true, 4300m, 18500m, 56000m, 2550m, "Standard Commercial" },
                    { 2, "Abnormal load vehicle category", true, 6000m, 50000m, 200000m, 4500m, "Abnormal Load" }
                });

            migrationBuilder.InsertData(
                table: "VehicleTypes",
                columns: new[] { "Id", "Description", "IsActive", "Name" },
                values: new object[,]
                {
                    { 1, "Commercial truck", true, "Truck" },
                    { 2, "Trailer unit", true, "Trailer" },
                    { 3, "Mobile crane", true, "Crane" },
                    { 4, "Semi-trailer unit", true, "Semi-trailer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AxleGroups_ComponentId",
                table: "AxleGroups",
                column: "ComponentId");

            migrationBuilder.CreateIndex(
                name: "IX_Axles_AxleGroupId",
                table: "Axles",
                column: "AxleGroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ComponentDimensions_ComponentId",
                table: "ComponentDimensions",
                column: "ComponentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CraneSpecifications_VehicleId",
                table: "CraneSpecifications",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoadDimensions_LoadId",
                table: "LoadDimensions",
                column: "LoadId");

            migrationBuilder.CreateIndex(
                name: "IX_LoadProjections_LoadId",
                table: "LoadProjections",
                column: "LoadId");

            migrationBuilder.CreateIndex(
                name: "IX_Loads_PermitId",
                table: "Loads",
                column: "PermitId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Manufacturers_Name",
                table: "Manufacturers",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PermitConstraints_PermitId",
                table: "PermitConstraints",
                column: "PermitId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitRoutes_PermitId",
                table: "PermitRoutes",
                column: "PermitId");

            migrationBuilder.CreateIndex(
                name: "IX_PermitRoutes_RouteId",
                table: "PermitRoutes",
                column: "RouteId");

            migrationBuilder.CreateIndex(
                name: "IX_Permits_PermitNumber",
                table: "Permits",
                column: "PermitNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Permits_PermitTypeId",
                table: "Permits",
                column: "PermitTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Permits_VehicleId",
                table: "Permits",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_TrailerSpecifications_VehicleId",
                table: "TrailerSpecifications",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TruckSpecifications_VehicleId",
                table: "TruckSpecifications",
                column: "VehicleId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleComponents_ComponentTypeId",
                table: "VehicleComponents",
                column: "ComponentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleComponents_VehicleId_Position",
                table: "VehicleComponents",
                columns: new[] { "VehicleId", "Position" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_VehicleEvents_VehicleId",
                table: "VehicleEvents",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOwnerships_OwnerId",
                table: "VehicleOwnerships",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_VehicleOwnerships_VehicleId",
                table: "VehicleOwnerships",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_ManufacturerId",
                table: "Vehicles",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleCategoryId",
                table: "Vehicles",
                column: "VehicleCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VehicleTypeId",
                table: "Vehicles",
                column: "VehicleTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_VIN",
                table: "Vehicles",
                column: "VIN",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Axles");

            migrationBuilder.DropTable(
                name: "ComponentDimensions");

            migrationBuilder.DropTable(
                name: "CraneSpecifications");

            migrationBuilder.DropTable(
                name: "LoadDimensions");

            migrationBuilder.DropTable(
                name: "LoadProjections");

            migrationBuilder.DropTable(
                name: "PermitConstraints");

            migrationBuilder.DropTable(
                name: "PermitRoutes");

            migrationBuilder.DropTable(
                name: "TrailerSpecifications");

            migrationBuilder.DropTable(
                name: "TruckSpecifications");

            migrationBuilder.DropTable(
                name: "VehicleEvents");

            migrationBuilder.DropTable(
                name: "VehicleOwnerships");

            migrationBuilder.DropTable(
                name: "AxleGroups");

            migrationBuilder.DropTable(
                name: "Loads");

            migrationBuilder.DropTable(
                name: "Routes");

            migrationBuilder.DropTable(
                name: "Owners");

            migrationBuilder.DropTable(
                name: "VehicleComponents");

            migrationBuilder.DropTable(
                name: "Permits");

            migrationBuilder.DropTable(
                name: "ComponentTypes");

            migrationBuilder.DropTable(
                name: "PermitTypes");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropTable(
                name: "Manufacturers");

            migrationBuilder.DropTable(
                name: "VehicleCategories");

            migrationBuilder.DropTable(
                name: "VehicleTypes");
        }
    }
}
