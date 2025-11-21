using System.Data;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Movies.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class NpgSqlConnectionFactory : IDbConnectionFactory
{
    private readonly string? _connectionString;
    private readonly ILogger<NpgSqlConnectionFactory> _logger;

    public NpgSqlConnectionFactory(string? connectionString, ILogger<NpgSqlConnectionFactory> logger)
    {
        _connectionString = connectionString;
        _logger = logger;
    }

    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        if (string.IsNullOrWhiteSpace(_connectionString))
        {
            _logger.LogError("Database connection string is null or empty");
            throw new InvalidOperationException("Database connection string is not configured");
        }

        try
        {
            var connection = new NpgsqlConnection(_connectionString);
            await connection.OpenAsync(token);
            return connection;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to create database connection");
            throw;
        }
    }
}


