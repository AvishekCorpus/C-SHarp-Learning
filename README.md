# C-Sharp Shipment Learning

This repository now includes:

- `ShipmentLearning` (console app with parcel creation + in-memory list, plus optional persistence via Web API)
- `ShipmentLearning.Api` (ASP.NET Core Web API using EF Core SQLite to store `Parcel`)

## Setup

1. Start the API:
   - `cd "c:\\Users\\User\\Desktop\\C Sharp Learning\\ShipmentLearning\\ShipmentLearning.Api"`
   - `dotnet run`

2. Start the console app in another terminal:
   - `cd "c:\\Users\\User\\Desktop\\C Sharp Learning\\ShipmentLearning"`
   - `dotnet run`

The console app will try to persist parcel data to `https://localhost:7126/api/parcels` by default.

## API Endpoints

- GET `/api/parcels`
- GET `/api/parcels/{id}`
- POST `/api/parcels`
- PUT `/api/parcels/{id}`
- DELETE `/api/parcels/{id}`

## Database

- SQLite DB file: `ShipmentLearning.Api\shipments.db`
- EF migrations in: `ShipmentLearning.Api\ShipmentLearning.Api\Data\Migrations`

