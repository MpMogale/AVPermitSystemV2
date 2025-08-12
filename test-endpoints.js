// Test the manufacturers endpoint
const baseUrl = 'http://localhost:5053';

// Test data
const testManufacturer = {
  name: "Test Manufacturer",
  code: "TEST",
  address: "123 Test Street",
  contactPhone: "+1234567890",
  contactEmail: "test@example.com",
  countryCode: "US"
};

// Create a manufacturer
fetch(`${baseUrl}/api/manufacturers`, {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify(testManufacturer)
})
.then(response => response.json())
.then(data => {
  console.log('Created manufacturer:', data);
  return data.id;
})
.then(manufacturerId => {
  // Get the created manufacturer
  return fetch(`${baseUrl}/api/manufacturers/${manufacturerId}`);
})
.then(response => response.json())
.then(data => {
  console.log('Retrieved manufacturer:', data);
})
.catch(error => {
  console.error('Error:', error);
});

// Test vehicle creation
const testVehicle = {
  vin: "1HGBH41JXMN109186",
  registrationNumber: "TEST001",
  name: "Test Vehicle",
  manufacturerId: 1, // Volvo from seed data
  vehicleTypeId: 1,   // Truck from seed data
  vehicleCategoryId: 1, // Standard Commercial from seed data
  model: "FH16",
  yearOfManufacture: 2023,
  color: "Red",
  grossVehicleMass: 44000,
  unladenMass: 8000,
  lengthMm: 18500,
  widthMm: 2550,
  heightMm: 4000,
  wheelbaseMm: 6200,
  frontOverhangMm: 1200,
  rearOverhangMm: 3100
};

fetch(`${baseUrl}/api/vehicles`, {
  method: 'POST',
  headers: {
    'Content-Type': 'application/json',
  },
  body: JSON.stringify(testVehicle)
})
.then(response => response.json())
.then(data => {
  console.log('Created vehicle:', data);
})
.catch(error => {
  console.error('Error creating vehicle:', error);
});
