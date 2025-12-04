# CleanMovies (Greenfield)

A from-scratch, CQRS-first, EF Core–backed API aimed at principal-engineer scrutiny.

## Projects
- `CleanMovies.Domain` – aggregates, value objects, domain events (no infrastructure).
- `CleanMovies.Application` – MediatR commands/queries, validators, behaviors, DTOs.
- `CleanMovies.Infrastructure` – EF Core DbContext, repositories, Unit of Work, DI.
- `CleanMovies.Api` – minimal API endpoints wired to MediatR; Swagger enabled.
- Tests: `CleanMovies.UnitTests`, `CleanMovies.IntegrationTests` (WebApplicationFactory + InMemory EF).

## Running
```bash
cd CleanMovies
# adjust ConnectionStrings:Default in src/CleanMovies.Api/appsettings.json (defaults to localdb)
dotnet restore
dotnet ef database update -p src/CleanMovies.Infrastructure -s src/CleanMovies.Api
dotnet run --project src/CleanMovies.Api
```
Swagger UI at `/swagger`.

Auth: mutating endpoints require a JWT Bearer token (default issuer/audience `cleanmovies`, key `replace-this-with-a-secure-long-secret-key`, role `editor`).

## Testing
```bash
cd CleanMovies
DOTNET_ENVIRONMENT=Testing dotnet test
```
Integration tests swap SQL Server for EF Core InMemory to stay hermetic.

## Design highlights
- True domain model with value objects (`MovieId`, `Slug`, `Genre`) and domain events.
- CQRS via MediatR; validation pipeline with FluentValidation.
- EF Core owned collections for `Genres` and `Ratings`; unique indexes on slug, title/year, rating uniqueness per user/movie; aggregate invariants in domain.
- JWT bearer auth with role-based policy (`Editor`) on mutating endpoints.
- Response caching via `ICacheService` abstraction; short-lived cache for reads.
- Correlation ID middleware + structured logging scopes; validation failures surfaced as RFC7807 ProblemDetails.
- Clear DI seams: repository + UnitOfWork abstractions; infra swaps in tests without touching domain/app layers.
