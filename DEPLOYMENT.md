# Deployment Guide

This guide covers deploying the Movies API to production environments.

## Prerequisites

- .NET 8 SDK (for local development)
- Docker and Docker Compose (for containerized deployment)
- PostgreSQL 16+ database
- Access to secrets management (Azure Key Vault, AWS Secrets Manager, or similar)

## Environment Variables

The following environment variables must be configured:

### Required

- `Database__ConnectionString` - PostgreSQL connection string
- `Jwt__Key` - JWT signing key (minimum 32 characters)
- `Jwt__Issuer` - JWT issuer identifier
- `Jwt__Audience` - JWT audience identifier
- `ApiKey` - API key for admin operations

### Optional

- `Telemetry__OtlpEndpoint` - OpenTelemetry collector endpoint (e.g., `http://otel-collector:4317`)
- `Telemetry__Prometheus__Enabled` - Enable Prometheus metrics endpoint (default: `true`)
- `ASPNETCORE_ENVIRONMENT` - Environment name (Production, Staging, etc.)
- `ASPNETCORE_URLS` - URLs to listen on (default: `http://+:8080`)

## Deployment Methods

### 1. Docker Compose (Recommended for Single Server)

1. **Create environment file**:
   ```bash
   cp .env.example .env.production
   # Edit .env.production with your production values
   ```

2. **Deploy**:
   ```bash
   docker-compose -f docker-compose.prod.yml --env-file .env.production up -d
   ```

3. **Verify deployment**:
   ```bash
   curl http://localhost:8080/_health
   ```

### 2. Docker (Standalone)

1. **Build image**:
   ```bash
   docker build -t movies-api:latest .
   ```

2. **Run container**:
   ```bash
   docker run -d \
     --name movies-api \
     -p 8080:8080 \
     -e Database__ConnectionString="Server=db;Database=movies_db;User ID=postgres;Password=..." \
     -e Jwt__Key="your-jwt-key" \
     -e Jwt__Issuer="https://your-issuer.com" \
     -e Jwt__Audience="https://your-audience.com" \
     -e ApiKey="your-api-key" \
     movies-api:latest
   ```

### 3. Azure App Service

1. **Create App Service**:
   ```bash
   az webapp create --resource-group myResourceGroup --plan myAppServicePlan --name movies-api --runtime "DOTNET|8.0"
   ```

2. **Configure Application Settings**:
   ```bash
   az webapp config appsettings set --resource-group myResourceGroup --name movies-api --settings \
     Database__ConnectionString="..." \
     Jwt__Key="..." \
     Jwt__Issuer="..." \
     Jwt__Audience="..." \
     ApiKey="..."
   ```

3. **Deploy**:
   ```bash
   dotnet publish -c Release
   cd Movies.Api/bin/Release/net8.0/publish
   zip -r deploy.zip .
   az webapp deployment source config-zip --resource-group myResourceGroup --name movies-api --src deploy.zip
   ```

### 4. Kubernetes

1. **Create secrets**:
   ```bash
   kubectl create secret generic movies-api-secrets \
     --from-literal=Database__ConnectionString="..." \
     --from-literal=Jwt__Key="..." \
     --from-literal=Jwt__Issuer="..." \
     --from-literal=Jwt__Audience="..." \
     --from-literal=ApiKey="..."
   ```

2. **Deploy**:
   ```bash
   kubectl apply -f k8s/
   ```

## Database Migrations

Migrations run automatically on application startup in Production mode. For manual migration:

```bash
dotnet run --project Movies.Application -- migrate
```

Or using Docker:

```bash
docker run --rm \
  -e Database__ConnectionString="..." \
  movies-api:latest \
  dotnet Movies.Application.dll migrate
```

## Health Checks

The application exposes health check endpoints:

- **Health Check**: `GET /_health`
- **Metrics** (if enabled): `GET /metrics`

Monitor these endpoints for deployment verification and load balancer health checks.

## Monitoring

### Prometheus Metrics

If `Telemetry__Prometheus__Enabled` is `true`, metrics are available at `/metrics`.

### OpenTelemetry

Configure `Telemetry__OtlpEndpoint` to send traces to an OpenTelemetry collector.

### Logging

Logs are written to stdout/stderr. Configure your container orchestration platform to collect these logs.

## Security Checklist

- [ ] All secrets stored in secure vault (not in code/config files)
- [ ] HTTPS enabled (use reverse proxy like nginx or Azure Application Gateway)
- [ ] CORS configured appropriately
- [ ] Rate limiting configured for your expected load
- [ ] Security headers enabled
- [ ] Database credentials rotated regularly
- [ ] JWT keys rotated regularly
- [ ] API keys rotated regularly

## Scaling

### Horizontal Scaling

The application is stateless and can be scaled horizontally:

```bash
docker-compose -f docker-compose.prod.yml up -d --scale movies-api=3
```

### Database Connection Pooling

Connection pooling is configured with:
- Min Pool Size: 0
- Max Pool Size: 100
- Connection Idle Lifetime: 300 seconds

Adjust these values based on your load and database capacity.

## Rollback Procedure

1. **Stop current deployment**:
   ```bash
   docker-compose -f docker-compose.prod.yml down
   ```

2. **Revert to previous image**:
   ```bash
   docker-compose -f docker-compose.prod.yml pull movies-api:previous-version
   docker-compose -f docker-compose.prod.yml up -d
   ```

3. **Verify rollback**:
   ```bash
   curl http://localhost:8080/_health
   ```

## Troubleshooting

### Application won't start

1. Check environment variables are set correctly
2. Verify database connectivity
3. Check application logs: `docker logs movies-api`

### Database connection errors

1. Verify connection string format
2. Check database is accessible from application
3. Verify database credentials
4. Check firewall rules

### Migration failures

1. Check database permissions
2. Verify migration history table exists
3. Review migration logs
4. Consider manual migration rollback if needed

## Support

For issues or questions, refer to the main README.md or contact the development team.

