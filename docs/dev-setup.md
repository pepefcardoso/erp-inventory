# Local Developer Setup

## Database connection (user-secrets)

The app reads its connection string from environment variables or user-secrets.
No real credentials are stored in source control.

To configure for local development:

```bash
cd src/ErpInventory.Api
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "Host=localhost;Port=5432;Database=erp_inventory;Username=postgres;Password=<your_password>"
```

For integration tests, export the env var before running:

```bash
export TEST_DB_CONNECTION="Host=localhost;Port=5432;Database=erp_inventory_test;Username=postgres;Password=<your_password>"
dotnet test tests/ErpInventory.IntegrationTests
```

## Docker Compose (local stack)

See T06 — credentials for the Docker Compose stack are handled via `.env` (not committed).
