using System.Data;

namespace Shop.Storage.Wrappers;

public interface IQueryExecutor
{
    Task<IEnumerable<T>?> QueryAsync<T>(IDbConnection connection, string query, object? param = null);
}
