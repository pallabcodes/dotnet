# .NET 8 DDD + Hexagonal/Clean Service Scaffold

Starting point for internal backend services. Clone, rename in one command, build on top:
add/remove/customize bounded contexts and infrastructure freely — the bones stay the same
whether the result is a monolith, a modular monolith, or distributed services.

## Quickstart
```bash
git clone <this-repo> your-service && cd your-service
./scripts/new-service.sh YourService
dotnet build YourService/YourService.sln && dotnet test YourService/YourService.sln
dotnet run --project YourService/src/YourService.Api   # Swagger at http://localhost:5080

# container (run from the repo root so the central props are in build context):
docker build -t your-service -f YourService/Dockerfile .
```

## Structure (dependency arrows point inward)
```
Shared/                       shared internal libs; every service consumes these
  Shared.Domain               zero deps: Entity, ValueObject, IDomainEvent/DomainEventBase
  Shared.Application          IUnitOfWork port + MediatR behaviors (validation, logging)

YourService/
  src/
    YourService.Domain          zero service deps: aggregates, value objects, domain events
    YourService.Application     ports (Abstractions/) + use cases (MediatR slices)
    YourService.Infrastructure  adapters: EF Core DbContext, repositories, UnitOfWork
    YourService.Api             composition root: versioned minimal APIs, middleware, OTel, health
  tests/
    YourService.UnitTests       domain invariants, validators, layer rules (architecture tests)
    YourService.IntegrationTests  real host via WebApplicationFactory + in-memory SQLite
```

## Rules
- Naming: all directories PascalCase per .NET convention (`YourService/`, `Shared/`).
  `YourService/` is what the clone rename touches; `Shared/` is internal shared libraries —
  every service consumes them and the rename never touches them. Forking `Shared/` is a
  deliberate platform decision, not part of the clone workflow.
- Add a bounded context by copying `YourService/Domain/Projects/` + one command/query + an EF
  configuration + an endpoints file. Contexts share nothing but `Shared.Domain` bases —
  that's the modular-monolith seam.
- Grow `Shared/` only when a second service needs the same piece — promote by moving code,
  not by pre-building speculative abstractions.
- `Shared/` publishes as versioned packages (`Shared.Domain`, `Shared.Application`; `dotnet pack Shared/Shared.sln`).
  Bump `<Version>` in the csproj on every change; services consume pinned versions from the internal
  NuGet feed so they don't silently inherit drift. Repo-convention sharing is only acceptable while
  the consumer count is small.
- Package versions live only in `Directory.Packages.props`; project files never pin versions.
- Layer rules are enforced by tests (`tests/YourService.UnitTests/Architecture/`).
- File budget: files stay well under ~200 lines; anything larger gets split.

## Deliberately excluded (promote when needed)
AuthN/AuthZ, rate limiting, outbox/inbox, idempotency, EF migrations (`EnsureCreated` is a dev
convenience; swap to migrations in production) and PostgreSQL/SqlServer (replace SQLite, then
mark `RowVersion` as `IsRowVersion()` for optimistic concurrency).

Observability is wired at boot: OpenTelemetry traces + metrics via OTLP; point
`OTEL_EXPORTER_OTLP_ENDPOINT` at your collector.
