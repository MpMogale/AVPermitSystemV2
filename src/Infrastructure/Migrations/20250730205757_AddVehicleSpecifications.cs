using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AVPermitSystemV2.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleSpecifications : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 20, 57, 57, 210, DateTimeKind.Utc).AddTicks(3197));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 20, 57, 57, 210, DateTimeKind.Utc).AddTicks(5032));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 20, 57, 57, 210, DateTimeKind.Utc).AddTicks(5039));

            migrationBuilder.CreateIndex(
                name: "IX_CraneSpecifications_VehicleId",
                table: "CraneSpecifications",
                column: "VehicleId",
                unique: true);

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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CraneSpecifications");

            migrationBuilder.DropTable(
                name: "TrailerSpecifications");

            migrationBuilder.DropTable(
                name: "TruckSpecifications");

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 20, 5, 11, 174, DateTimeKind.Utc).AddTicks(3599));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 20, 5, 11, 174, DateTimeKind.Utc).AddTicks(4683));

            migrationBuilder.UpdateData(
                table: "Manufacturers",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 20, 5, 11, 174, DateTimeKind.Utc).AddTicks(4686));
        }
    }
}
