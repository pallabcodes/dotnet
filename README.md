# Movies API

A production-ready .NET 8 Web API for managing movies with authentication, authorization, and comprehensive features.

## Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

- **Movies.Api**: Web API layer with endpoints, authentication, and middleware
- **Movies.Application**: Business logic, services, repositories, and data access
- **Movies.Contracts**: Request/response DTOs and contracts
- **Movies.Api.Sdk**: Client SDK for consuming the API
- **Movies.Api.Sdk.Consumer**: Example consumer application
- **Identity.Api**: Identity service for JWT token generation

## Prerequisites

- .NET 8 SDK
- PostgreSQL database
- Visual Studio 2022, VS Code, or Rider

## Configuration

### Security Requirements

**CRITICAL**: Never commit secrets to source control. Use one of the following methods:

1. **User Secrets (Development)**
   ```bash
   dotnet user-secrets init
   dotnet user-secrets set "Database:ConnectionString" "your-connection-string"
   dotnet user-secrets set "Jwt:Key" "your-jwt-key"
   dotnet user-secrets set "Jwt:Issuer" "your-issuer"
   dotnet user-secrets set "Jwt:Audience" "your-audience"
   dotnet user-secrets set "ApiKey" "your-api-key"
   ```

2. **Environment Variables (Production)**
   ```bash
   export Database__ConnectionString="your-connection-string"
   export Jwt__Key="your-jwt-key"
   export Jwt__Issuer="your-issuer"
   export Jwt__Audience="your-audience"
   export ApiKey="your-api-key"
   ```

3. **Azure Key Vault (Recommended for Production)**
   - Configure Azure Key Vault provider in `Program.cs`
   - Store all secrets in Key Vault

### Configuration Structure

The `appsettings.json` file contains empty placeholders. Configure values using one of the methods above:

```json
{
  "Database": {
    "ConnectionString": ""
  },
  "Jwt": {
    "Key": "",
    "Issuer": "",
    "Audience": ""
  },
  "ApiKey": ""
}
```

## Database Setup

### Development

The database is automatically initialized in Development mode using `DbInitializer`. For production, use proper database migrations.

### Production

1. Create the database:
   ```sql
   CREATE DATABASE movies_db;
   ```

2. Run migrations (when implemented) or execute the schema manually from `DbInitializer.cs`

3. Configure connection pooling in `ApplicationServiceCollectionExtensions.cs`

## Running the Application

1. **Set up configuration** using User Secrets or environment variables
2. **Start PostgreSQL** database
3. **Run the API**:
   ```bash
   cd Movies.Api
   dotnet run
   ```
4. **Access Swagger UI**: `https://localhost:7280/swagger` (Development only)

## API Features

- **Authentication**: JWT Bearer token authentication
- **Authorization**: Role-based authorization with policies
- **API Versioning**: Supports multiple API versions
- **Output Caching**: Response caching for improved performance
- **Health Checks**: Database health monitoring
- **Validation**: FluentValidation for request validation
- **Structured Logging**: Comprehensive logging throughout

## Security Best Practices

1. **Never commit secrets** - Use secure configuration methods
2. **Use HTTPS** - Always in production
3. **Validate all inputs** - FluentValidation ensures data integrity
4. **Use parameterized queries** - All SQL queries use Dapper parameters
5. **Connection pooling** - Configured for optimal performance
6. **Health checks** - Monitor database connectivity

## Development Guidelines

### Code Quality

- All code follows C# naming conventions
- Async/await patterns used consistently
- Cancellation tokens properly implemented
- Dependency injection throughout
- Comprehensive error handling and logging

### Testing

**Note**: Test projects should be added for:
- Unit tests for services and repositories
- Integration tests for API endpoints
- End-to-end tests for critical flows

### Database Migrations

**Current**: Database initialization runs in Development mode only.

**Recommended**: Implement proper database migrations using:
- Entity Framework Core Migrations, or
- FluentMigrator, or
- Custom migration scripts

## Project Structure

```
Movies.Api/
├── Auth/              # Authentication and authorization
├── Configuration/     # Service configuration
├── Endpoints/         # API endpoints (Minimal APIs)
├── Health/           # Health check implementations
├── Mapping/          # Request/response mapping
└── Swagger/          # Swagger/OpenAPI configuration

Movies.Application/
├── Database/         # Database connection and initialization
├── Models/           # Domain models
├── Repositories/     # Data access layer
├── Services/         # Business logic
└── Validators/       # FluentValidation validators
```

## Production Deployment Checklist

- [ ] Configure all secrets via secure method (Key Vault, environment variables)
- [ ] Set up proper database migrations
- [ ] Configure connection pooling
- [ ] Enable HTTPS only
- [ ] Configure CORS if needed
- [ ] Set up monitoring and logging (Application Insights, etc.)
- [ ] Add rate limiting
- [ ] Configure health check endpoints
- [ ] Set up CI/CD pipeline
- [ ] Add comprehensive test coverage
- [ ] Review and configure security headers
- [ ] Set up distributed tracing

## License

[Specify your license here]

## Contributing

[Add contribution guidelines]

