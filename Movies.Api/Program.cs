using Movies.Api.Configuration;
using Movies.Api.Endpoints;
using Movies.Api.Health;
using Movies.Api.Mapping;
using Movies.Api.Middleware;
using Movies.Application;
using Movies.Application.Database;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddValidatedConfiguration(config);
builder.Services.AddTelemetry(config, builder.Environment);
builder.Services.AddJwtAuthentication(config);
builder.Services.AddCustomAuthorization(config);
builder.Services.AddApiVersioning();
builder.Services.AddOutputCaching();
builder.Services.AddRateLimiting(config);
builder.Services.AddSecurityHeaders(config);
builder.Services.AddCorsConfiguration(config);
builder.Services.AddHealthChecks().AddCheck<DatabaseHealthCheck>(DatabaseHealthCheck.Name);
builder.Services.AddSwagger();
builder.Services.AddApplication();
builder.Services.AddDatabase(config["Database:ConnectionString"], config);

var app = builder.Build();

app.CreateApiVersionSet();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(x =>
    {
        foreach (var description in app.DescribeApiVersions())
        {
            x.SwaggerEndpoint(
                $"/swagger/{description.GroupName}/swagger.json",
                description.GroupName);
        }
    });
}

app.MapHealthChecks("_health");

if (app.Configuration.GetValue<bool>("Telemetry:Prometheus:Enabled", false))
{
    app.UseOpenTelemetryPrometheusScrapingEndpoint();
}

app.UseMiddleware<CorrelationIdMiddleware>();
app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseCors();
app.UseMiddleware<SecurityHeadersMiddleware>();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseOutputCache();
app.UseMiddleware<ValidationMappingMiddleware>();
app.MapApiEndpoints();

if (app.Environment.IsDevelopment())
{
    var dbInitializer = app.Services.GetRequiredService<DbInitializer>();
    await dbInitializer.InitializeAsync();
}
else
{
    var migrationOptions = app.Services.GetRequiredService<MigrationOptions>();
    if (migrationOptions.AutoMigrateOnStartup)
    {
        var migrationRunner = app.Services.GetRequiredService<IMigrationRunner>();
        
        if (migrationOptions.ValidateMigrationsOnStartup)
        {
            var hasPending = await migrationRunner.HasPendingMigrationsAsync();
            if (hasPending)
            {
                var info = await migrationRunner.GetMigrationInfoAsync();
                app.Logger.LogInformation(
                    "Found {Count} pending migration(s). Current version: {Version}",
                    info.PendingMigrations, info.CurrentVersion);
            }
        }
        
        await migrationRunner.MigrateAsync();
    }
}

app.Run();
