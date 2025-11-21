namespace Movies.Application.Database;

public class MigrationOptions
{
    public const string SectionName = "Migrations";
    
    public bool AutoMigrateOnStartup { get; set; } = true;
    public bool ValidateMigrationsOnStartup { get; set; } = true;
    public int TimeoutSeconds { get; set; } = 300;
    public string VersionTableSchema { get; set; } = "public";
    public string VersionTableName { get; set; } = "versioninfo";
}

