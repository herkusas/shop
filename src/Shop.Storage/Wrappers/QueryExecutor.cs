using System.Data;
using Dapper;

namespace Shop.Storage.Wrappers;

/// <summary>
/// for test purposes
/// </summary>
public class QueryExecutor : IQueryExecutor
{
    public async Task<IEnumerable<T>?> QueryAsync<T>(IDbConnection connection, string query, object? param = null)
    {
        return await connection.QueryAsync<T>(query, param);
    }
}
