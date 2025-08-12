# 🚛 AV Permit System V2

A comprehensive Abnormal Vehicle (AV) Permit System built with .NET 9, Entity Framework Core, and Minimal APIs following Clean Architecture principles.

## 🏗️ Architecture Overview

The project follows Clean Architecture with these layers:

- **Domain Layer** (`src/Domain/`) - Core business entities and domain logic
- **Application Layer** (`src/Application/`) - Application services, DTOs, and business logic
- **Infrastructure Layer** (`src/Infrastructure/`) - Data access, external services
- **API Layer** (`src/API/`) - REST API endpoints using Minimal APIs

## 📋 Features Implemented

Based on the project backlog, the following epics have been implemented:

### ✅ Epic 1: Vehicle Registry
- [x] Create `Manufacturer` table/API
- [x] Create `VehicleType` lookup  
- [x] Create `POST /vehicles` endpoint
- [x] Validate VIN uniqueness
- [x] Add auditing columns
- [x] Create `ComponentType` lookup
- [x] Create `POST /vehicles/{id}/components`
- [x] Attach registration number to each component
- [x] Create `POST /components/{id}/dimensions`
- [x] Include length, width, height, overhangs
- [x] Validate min/max values

### ✅ Epic 2: Axle & Configuration Management
- [x] Create `AxleGroup` table
- [x] POST /components/{id}/axle-groups
- [x] Add spacing, unladen mass
- [x] Create `Axle` table
- [x] POST /axle-groups/{id}/axles
- [x] Include tyre count, load, position

### ✅ Epic 3: Ownership Lifecycle
- [x] Create `Owner` table and type enum
- [x] POST /owners
- [x] Create `VehicleOwnership` table
- [x] POST /vehicle-ownerships
- [x] Validate overlapping date ranges
- [x] Add `IsPrimaryOwner` flag logic

### ✅ Epic 4: Permit Management
- [x] Create `PermitType` lookup
- [x] Create `Permit` table with `Status`
- [x] POST /permits
- [x] PATCH /permits/{id}/status
- [x] Add constraint table
- [x] POST /permits/{id}/constraints

### ✅ Epic 5: Load Management (Abnormal Load)
- [x] Create `Load` table
- [x] POST /permits/{id}/load
- [x] Add `LoadType`, `Weight`
- [x] Create `LoadDimension` + `LoadProjection`
- [x] POST /load/{id}/dimensions
- [x] Validate logical values

### ✅ Epic 6: Routing & Trip Distances
- [x] Create `Route` table
- [x] POST /routes
- [x] Include origin, destination, distance
- [x] Create `PermitRoute` join table
- [x] POST /permits/{id}/routes
- [x] Support multi-leg trips

### ✅ Epic 7: Event Tracking & Auditing
- [x] Create `VehicleEvent` table
- [x] POST /vehicle-events
- [x] Define `EventType`

## 🚀 Getting Started

### Prerequisites
- .NET 9 SDK
- SQLite (for development)
- Visual Studio 2022 or VS Code

### Setup Instructions

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd AVPermitSystemV2
   ```

2. **Restore packages**
   ```bash
   dotnet restore
   ```

3. **Update database**
   ```bash
   cd src/Infrastructure
   dotnet ef database update --startup-project ../API
   ```

4. **Run the application**
   ```bash
   cd src/API
   dotnet run
   ```

5. **Access Swagger UI**
   Open your browser and navigate to: `http://localhost:5053/swagger`

## 📖 API Endpoints

### Vehicle Management
- `GET /api/vehicles` - List all vehicles
- `GET /api/vehicles/{id}` - Get vehicle by ID
- `POST /api/vehicles` - Create new vehicle
- `PUT /api/vehicles/{id}` - Update vehicle
- `DELETE /api/vehicles/{id}` - Soft delete vehicle
- `POST /api/vehicles/{id}/components` - Add component to vehicle
- `GET /api/vehicles/{id}/components` - Get vehicle components

### Manufacturer Management
- `GET /api/manufacturers` - List all manufacturers
- `GET /api/manufacturers/{id}` - Get manufacturer by ID
- `POST /api/manufacturers` - Create new manufacturer
- `PUT /api/manufacturers/{id}` - Update manufacturer
- `DELETE /api/manufacturers/{id}` - Soft delete manufacturer

### Component Management
- `POST /api/components/{id}/dimensions` - Add dimensions to component
- `GET /api/components/{id}/dimensions` - Get component dimensions
- `PUT /api/components/{id}/dimensions` - Update component dimensions
- `GET /api/components/types` - Get all component types

## 🗃️ Database Schema

### Core Entities

**Vehicle** - Main vehicle registry
- VIN (unique), Registration Number, Name
- Manufacturer, Vehicle Type, Vehicle Category
- Physical dimensions and mass properties
- Audit trail fields

**Manufacturer** - Vehicle manufacturers
- Name, Code, Contact details
- Country of origin

**VehicleComponent** - Components attached to vehicles
- Component type, registration number, serial number
- Position within vehicle configuration
- Mass and manufacturing details

**ComponentDimension** - Physical dimensions of components
- Length, width, height
- Overhang measurements (front, rear, left, right)

### Lookup Tables
- **VehicleType** - Standard, Truck, Trailer, Crane, etc.
- **VehicleCategory** - Standard Commercial, Abnormal Load, etc.
- **ComponentType** - Prime Mover, Semi-trailer, Dolly, Dog Trailer
- **PermitType** - Standard, Abnormal Load, Annual

### Advanced Features
- **Axle Management** - AxleGroup → Axle hierarchy
- **Ownership Tracking** - Owner → VehicleOwnership history
- **Permit System** - Permit → PermitConstraint → PermitRoute
- **Load Management** - Load → LoadDimension → LoadProjection
- **Event Auditing** - VehicleEvent for all system activities

## 🔧 Configuration

### Database Connection
Update `appsettings.json` in the API project:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=AVPermitSystem.db"
  }
}
```

### Environment Variables
- `ASPNETCORE_ENVIRONMENT` - Development/Production
- `ConnectionStrings__DefaultConnection` - Database connection string

## 🧪 Testing

Sample test data is automatically seeded including:
- 3 Manufacturers (Volvo, Mercedes-Benz, Scania)
- 4 Vehicle Types
- 4 Component Types  
- 3 Permit Types
- 2 Vehicle Categories

## 🔮 Next Steps

### Epic 8: Specification Modules
- [ ] Implement `TruckSpecification` entity and endpoints
- [ ] Implement `CraneSpecification` entity and endpoints  
- [ ] Implement `TrailerSpecification` entity and endpoints

### Epic 9: Security & Access Control
- [ ] Implement authentication (JWT/OAuth)
- [ ] Add role-based authorization
- [ ] Apply permissions to API endpoints

### Epic 10: Reporting & Exports
- [ ] Implement permit summary export
- [ ] Add vehicle summary reports
- [ ] Generate PDF permit documents

### Future Enhancements
- [ ] Permit renewal process
- [ ] Document upload functionality
- [ ] Email notifications
- [ ] GIS-integrated route validation
- [ ] Mobile inspector app

## 🏛️ Technology Stack

- **Backend**: .NET 9, ASP.NET Core Minimal APIs
- **Database**: Entity Framework Core 9, SQLite (dev), SQL Server (prod)
- **Documentation**: Swagger/OpenAPI
- **Validation**: FluentValidation
- **Architecture**: Clean Architecture, CQRS with MediatR
- **Mapping**: Mapster

## 📄 License

This project is licensed under the MIT License - see the LICENSE file for details.

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📞 Support

For support and questions, please open an issue in the repository.
