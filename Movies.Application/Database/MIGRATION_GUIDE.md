# Database Migration Guide

This guide covers how to create and manage database migrations using FluentMigrator.

## Creating a New Migration

### Migration Naming Convention

Migrations should follow this pattern:
- **Version**: `YYYYMMDDHHMMSS` (timestamp format)
- **Description**: Brief, descriptive name
- **File Name**: `{Version}_{Description}.cs`

Example: `20240115143000_AddMovieRatingIndex.cs`

### Step-by-Step

1. **Create a new migration class** in `Movies.Application/Database/Migrations/`:

```csharp
using FluentMigrator;

namespace Movies.Application.Database.Migrations;

[Migration(20240115143000, "Add index on movie ratings")]
public class AddMovieRatingIndex : Migration
{
    public override void Up()
    {
        Create.Index("idx_ratings_movieid")
            .OnTable("ratings")
            .OnColumn("movieid");
    }

    public override void Down()
    {
        Delete.Index("idx_ratings_movieid")
            .OnTable("ratings");
    }
}
```

2. **Build the project** to ensure the migration is compiled
3. **Test the migration** in development before deploying

## Migration Best Practices

### 1. Always Implement Down()

Every migration must have a `Down()` method for rollback capability:

```csharp
public override void Down()
{
    // Reverse all changes made in Up()
    Delete.Index("idx_ratings_movieid").OnTable("ratings");
}
```

### 2. Use Transactions

FluentMigrator runs each migration in a transaction automatically. If any part fails, the entire migration is rolled back.

### 3. Test Migrations

- Test `Up()` in development
- Test `Down()` to ensure rollback works
- Test on a copy of production data if possible

### 4. Avoid Data Loss

When modifying columns:
- Use `Alter.Column()` instead of dropping and recreating
- Migrate data before dropping columns
- Add new columns as nullable first, then populate, then make non-nullable

### 5. Index Creation

- Create indexes in separate statements for better error handling
- Consider index creation time for large tables
- Document why indexes are needed

### 6. Foreign Keys

- Always specify `OnDelete` behavior
- Use `Cascade` for dependent data
- Use `SetNull` or `Restrict` when appropriate

## Common Migration Patterns

### Adding a Column

```csharp
Alter.Table("movies")
    .AddColumn("description").AsString().Nullable();
```

### Modifying a Column

```csharp
Alter.Table("movies")
    .AlterColumn("title").AsString(500).NotNullable();
```

### Adding an Index

```csharp
Create.Index("idx_movies_title")
    .OnTable("movies")
    .OnColumn("title");
```

### Adding a Foreign Key

```csharp
Create.ForeignKey("fk_genres_movies")
    .FromTable("genres").ForeignColumn("movieid")
    .ToTable("movies").PrimaryColumn("id")
    .OnDelete(System.Data.Rule.Cascade);
```

### Data Migration

```csharp
Execute.Sql(@"
    UPDATE movies 
    SET description = 'No description available' 
    WHERE description IS NULL
");
```

## Running Migrations

### Automatic (Production)

Migrations run automatically on application startup in Production mode if `AutoMigrateOnStartup` is enabled.

### Manual (Development/Testing)

You can check migration status via the API endpoint:
```bash
GET /api/migrations/info
```

### Programmatic

```csharp
var migrationRunner = serviceProvider.GetRequiredService<IMigrationRunner>();
await migrationRunner.MigrateAsync();
```

## Migration Status

Check migration status:

```csharp
var info = await migrationRunner.GetMigrationInfoAsync();
Console.WriteLine($"Current version: {info.CurrentVersion}");
Console.WriteLine($"Pending migrations: {info.PendingMigrations}");
```

## Troubleshooting

### Migration Fails

1. Check application logs for detailed error messages
2. Verify database connection
3. Check if previous migrations completed successfully
4. Review migration SQL for syntax errors

### Version Conflicts

If you see version conflicts:
1. Check the `versioninfo` table in the database
2. Ensure all team members have the latest migrations
3. Never modify existing migrations - create new ones instead

### Rollback

To rollback a migration:
1. Create a new migration that reverses the changes
2. Or manually update the `versioninfo` table (not recommended)

## Configuration

Migration behavior can be configured in `appsettings.json`:

```json
{
  "Migrations": {
    "AutoMigrateOnStartup": true,
    "ValidateMigrationsOnStartup": true,
    "TimeoutSeconds": 300,
    "VersionTableSchema": "public",
    "VersionTableName": "versioninfo"
  }
}
```

## Version Table

FluentMigrator tracks applied migrations in the `versioninfo` table:
- `Version`: Migration version number
- `AppliedOn`: Timestamp when migration was applied
- `Description`: Migration description

Never manually modify this table unless absolutely necessary.

## Production Deployment

1. **Backup database** before running migrations
2. **Test migrations** on staging environment first
3. **Monitor logs** during migration execution
4. **Verify** migration success via `/api/migrations/info` endpoint
5. **Rollback plan** - know how to revert if needed

## Security

- Migration endpoint (`/api/migrations/info`) requires authorization
- Never expose migration execution endpoints publicly
- Use secure connection strings
- Limit migration execution to authorized personnel

