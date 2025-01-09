using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using WatchDog;
using HealthChecks.UI.Client;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers
builder.Services.AddControllers();

// Add Swagger for API Documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Getting Started API",
        Version = "v1",
        Description = "API for demonstrating health checks, API versioning, and more in .NET 8"
    });
    options.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "Getting Started API - Version 2",
        Version = "v2"
    });
});

// Add Health Checks
builder.Services.AddHealthChecks()
    .AddCheck<RandomHealthCheck>("Random Health Check", failureStatus: HealthStatus.Degraded);

// Configure HealthChecks UI
builder.Services.AddHealthChecksUI(options =>
{
    options.SetEvaluationTimeInSeconds(15); // Time in seconds for UI to poll Health Check status
    options.MaximumHistoryEntriesPerEndpoint(50); // History entries to store per endpoint
    options.AddHealthCheckEndpoint("Health Checks", "/health"); // Map health checks
}).AddInMemoryStorage(); // Use in-memory storage for HealthChecks UI

// Configure API Versioning
builder.Services.AddApiVersioning(opts =>
{
    opts.AssumeDefaultVersionWhenUnspecified = true;
    opts.DefaultApiVersion = new ApiVersion(2, 0);
    opts.ReportApiVersions = true;
});

// Configure API Explorer for Versioning
builder.Services.AddVersionedApiExplorer(opts =>
{
    opts.GroupNameFormat = "'v'VVV";
    opts.SubstituteApiVersionInUrl = true;
});

// Add WatchDog for Monitoring
builder.Services.AddWatchDogServices();

// Build the app
var app = builder.Build();

// Configure Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Configure Health Check Endpoints
app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

// Configure Health Checks UI
app.MapHealthChecksUI(options =>
{
    options.UIPath = "/health-ui"; // Path to access the Health Checks UI
});

// Configure WatchDog Monitoring
app.UseWatchDogExceptionLogger();
app.UseWatchDog(config =>
{
    config.WatchPageUsername = "admin";
    config.WatchPagePassword = "password";
});

// Run the app
app.Run();
