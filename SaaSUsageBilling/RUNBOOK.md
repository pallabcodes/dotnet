# SaaSUsageBilling Runbook (Local)

## Prerequisites
- .NET 8 SDK
- Docker (for Postgres)

## Quick start
```bash
docker compose up -d        # start Postgres
dotnet build SaaSUsageBilling.sln
dotnet test SaaSUsageBilling.sln
cd SaaSUsageBilling/src/SaaSUsageBilling.Api
dotnet run --urls=http://localhost:5000
```

## Configuration
- Connection string: `appsettings.json` (`ConnectionStrings:BillingDatabase`). For Postgres set `UseInMemoryDatabase=false` and supply a Postgres connection string.
- JWT: update `Jwt:Key`, `Jwt:Issuer`, `Jwt:Audience` for non-dev use.
- Rate limiting: `IpRateLimiting` section in `appsettings.json`.

## Health
- Liveness: `/health/live`
- Readiness: `/health/ready`
- Basic status: `/health`

## Troubleshooting
- Enable detailed logs: set `Logging:LogLevel:Default` to `Debug`.
- Database migrations: ensure `dotnet-ef` is installed and run `dotnet ef migrations add <name>` from `src/SaaSUsageBilling.Api`.
- Idempotency/Outbox: review `OutboxProcessor` logs and `IdempotencyKeys` / `OutboxMessages` tables.

