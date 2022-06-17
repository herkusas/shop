using System.Data;

namespace Shop.Storage.Postgre.DataLayer.Connection;

public interface IConnectionFactory
{
    Task<IDbConnection> Create();
}
