using System.Data;
using Npgsql;

namespace Movies.Application.Database;

public interface IDbConnectionFactory
{
    Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
}

public class NpgSqlConnectionFactory : IDbConnectionFactory
{
    private readonly string? _connectionString;

    public NpgSqlConnectionFactory(string? connectionString)
    {
        _connectionString = connectionString;
    }


    public async Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default)
    {
        var connection = new NpgsqlConnection(_connectionString); // create the connection object
        await connection.OpenAsync(token); // open connection
        return connection; // return the connection instance
    }
}