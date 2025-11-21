using Movies.Application.Database;

namespace Movies.Api.Endpoints.Migrations;

public static class MigrationInfoEndpoint
{
    public const string Name = "GetMigrationInfo";

    public static IEndpointRouteBuilder MapMigrationInfo(this IEndpointRouteBuilder app)
    {
        app.MapGet(
                "/api/migrations/info",
                async (IMigrationRunner migrationRunner, CancellationToken token) =>
                {
                    var info = await migrationRunner.GetMigrationInfoAsync(token);
                    var response = new MigrationInfoResponse
                    {
                        CurrentVersion = info.CurrentVersion,
                        TotalMigrations = info.TotalMigrations,
                        AppliedMigrations = info.AppliedMigrations,
                        PendingMigrations = info.PendingMigrations,
                        PendingMigrationDetails = info.PendingMigrationDetails.Select(m => new MigrationDetailsResponse
                        {
                            Version = m.Version,
                            Description = m.Description
                        }).ToList()
                    };
                    return TypedResults.Ok(response);
                })
            .WithName(Name)
            .Produces<MigrationInfoResponse>()
            .RequireAuthorization()
            .ExcludeFromDescription();

        return app;
    }
}

public class MigrationInfoResponse
{
    public long CurrentVersion { get; set; }
    public int TotalMigrations { get; set; }
    public int AppliedMigrations { get; set; }
    public int PendingMigrations { get; set; }
    public List<MigrationDetailsResponse> PendingMigrationDetails { get; set; } = new();
}

public class MigrationDetailsResponse
{
    public long Version { get; set; }
    public string Description { get; set; } = string.Empty;
}

