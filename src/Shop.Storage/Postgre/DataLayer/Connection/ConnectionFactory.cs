using System.Data;
using Npgsql;

namespace Shop.Storage.Postgre.DataLayer.Connection;

public class ConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }
    
    public async Task<IDbConnection> Create()
    {
        var connection = new NpgsqlConnection(_connectionString);

        await connection.OpenAsync();

        return connection;
    }
}
