<!-- README.md -->
# ERP Inventory API

A lightweight backend service for tracking product stock levels and warehouse movements.

## Problem

Small inventory operations often outgrow spreadsheets before they can justify a full ERP suite. This service provides the core primitives — products, warehouses, stock movements — as a clean, testable API that other systems (storefronts, POS, reporting tools) can build on top of.

## Tech Stack

| Layer          | Technology                          |
|----------------|--------------------------------------|
| Runtime        | .NET 10                              |
| API            | ASP.NET Core Web API                 |
| CQRS / Mediator| MediatR                              |
| Validation     | FluentValidation                     |
| Persistence    | EF Core + Npgsql                     |
| Database       | PostgreSQL 16                        |
| Testing        | xUnit, coverlet, WebApplicationFactory |
| Containerization | Docker, Docker Compose             |

## Architecture

Clean Architecture with CQRS. Dependencies point inward; `Infrastructure` implements interfaces defined by `Application`.

```
ErpInventory.Api            → Controllers, HTTP concerns
       │  (MediatR requests)
       ▼
ErpInventory.Application    → Commands/Queries, Validators, Interfaces
       │  (implements)                     ▲
       ▼                                    │
ErpInventory.Domain          ErpInventory.Infrastructure
(Entities, invariants)  ←──  (EF Core, PostgreSQL, Repositories)
```

## Key Features

**Product management**
- Create product (SKU, name, unit price) — enforced via `CreateProductCommandValidator`
- Retrieve product by ID
- Domain-level stock adjustment (`ReceiveStock`, `RemoveStock`) — modeled on `Product`, not yet exposed via API

**Warehouse**
- `Warehouse` entity (name, location) — modeled, no endpoints yet

**Stock movements**
- `StockMovement` entity (Inbound/Outbound, quantity, timestamp) — modeled, no endpoints yet

## Project Structure

```
src/
├── ErpInventory.Api/            # Controllers, Program.cs, appsettings
├── ErpInventory.Application/    # Commands, Queries, Validators, DTOs
│   └── Products/
│       ├── Commands/            # CreateProductCommand + handler + validator
│       └── Queries/             # GetProductByIdQuery + handler + DTO
├── ErpInventory.Domain/         # Entities: Product, Warehouse, StockMovement
└── ErpInventory.Infrastructure/
    └── Persistence/
        ├── ErpInventoryDbContext.cs
        ├── Migrations/
        └── Repositories/        # ProductRepository

tests/
├── ErpInventory.UnitTests/      # Domain logic tests
└── ErpInventory.IntegrationTests/ # Full API + Postgres via WebApplicationFactory
```

## Getting Started

**Prerequisites:** .NET 10 SDK, Docker (for Postgres), PostgreSQL 16 (if running outside Docker).

```bash
git clone <repo-url>
cd erp-inventory
dotnet restore
```

**Configure local secrets** (no credentials are committed):

```bash
cd src/ErpInventory.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=erp_inventory;Username=postgres;Password=<your_password>"
cd ../..
```

**Run with Docker Compose** (API + Postgres):

```bash
cp .env.example .env
docker compose up --build
```

**Run locally against Docker's Postgres:**

```bash
dotnet run --project src/ErpInventory.Api
```

## Environment Variables

| Variable                              | Description                                  | Example / Required            |
|----------------------------------------|-----------------------------------------------|--------------------------------|
| `POSTGRES_USER`                        | Postgres superuser for the compose stack      | `postgres` (required)          |
| `POSTGRES_PASSWORD`                    | Postgres password for the compose stack       | required, no default           |
| `POSTGRES_DB`                          | Database name created on first container start| `erp_inventory` (required)     |
| `ConnectionStrings__DefaultConnection` | EF Core connection string (API host)          | set via user-secrets locally   |
| `TEST_DB_CONNECTION`                   | Connection string for integration tests       | `Host=localhost;Port=5432;Database=erp_inventory_test;Username=postgres;Password=` |

## API Reference

| Method | Route                | Auth | Description                     |
|--------|-----------------------|------|----------------------------------|
| POST   | `/api/products`       | None | Create a product                 |
| GET    | `/api/products/{id}`  | None | Get a product by ID              |

No authentication/authorization is currently configured in `Program.cs`.

## Testing

```bash
# Unit tests (Domain logic, no DB required)
dotnet test tests/ErpInventory.UnitTests

# Integration tests (requires a reachable Postgres, e.g. via docker compose up postgres)
export TEST_DB_CONNECTION="Host=localhost;Port=5432;Database=erp_inventory_test;Username=postgres;Password=<your_password>"
dotnet test tests/ErpInventory.IntegrationTests
```

Coverage collection is available via `coverlet.collector` but no threshold is currently enforced:

```bash
dotnet test --collect:"XPlat Code Coverage"
```

## Deployment

Multi-stage Docker build (`Dockerfile`): SDK image restores/publishes, runtime image (`aspnet:10.0`) serves on port `8080`.

```bash
docker compose up --build -d
```

`Dockerfile.test` is provided for running the containerized build in test/CI contexts; it currently mirrors `Dockerfile` and should be diverged (e.g. to run `dotnet test`) if used in a pipeline.

## Non-Negotiable Rules

No `AGENTS.md` / `GEMINI.md` / `conventions.md` exists in this repo yet. Add one and reference it here if hard constraints emerge (e.g. domain invariants, migration policy).

## License

MIT — see [LICENSE](./LICENSE).
```
