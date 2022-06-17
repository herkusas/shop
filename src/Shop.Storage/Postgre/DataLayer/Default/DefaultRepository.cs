using System.Data;
using Dapper;
using Shop.Exceptions;
using Shop.Storage.Postgre.DataLayer.Connection;
using Shop.Storage.Wrappers;

namespace Shop.Storage.Postgre.DataLayer.Default;

public class DefaultRepository : IRepo
{
    private readonly IConnectionFactory _connectionFactory;
    private readonly IQueryExecutor _queryExecutor;

    public DefaultRepository(IConnectionFactory connectionFactory, IQueryExecutor queryExecutor)
    {
        _connectionFactory = connectionFactory;
        _queryExecutor = queryExecutor;
    }
    
    public async Task SaveProduct(Product product)
    {
        var productId = await ProductExist(product.Name);

        if (productId != null)
        {
            await UpdateProduct(product);
        }
        else
        {
            await InsertProduct(product);
        }
    }
    
    public async Task<IEnumerable<Product>?> GetProducts()
    {
        const string Query = @"SELECT * FROM products";
        
        var cnn = await _connectionFactory.Create();

        var productRecords =  await _queryExecutor.QueryAsync<ProductRecord>(cnn, Query);

        return productRecords?.Select(productRecord => productRecord.Map());
    }

    public async Task<Product?> GetProductByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var products = await GetProducts();

        return products?.SingleOrDefault(x => x.Name.Equals(name));
    }

    public async Task<int> DeleteProduct(Product product)
    {
        const string Query = @"DELETE FROM products WHERE name = @name";

        var cnn = await _connectionFactory.Create();

        return await cnn.ExecuteAsync(Query, new { name = product.Name });
    }

    public async Task MakeOrder(Client client, List<Product> products)
    {
        var clientId = await ExistClient(client.Name);
        if (clientId != null)
        {
            var cnn = await _connectionFactory.Create();
            using var transaction = cnn.BeginTransaction();
            try
            {
                var orderId = await CreateOrder(cnn, transaction, clientId.Value);
                transaction.Commit();
                cnn.Close();
            }
            catch
            {
                transaction.Rollback();
                cnn.Close();
                throw;
            }
        }
        else
        {
            throw new NoSuchClientException($"{client.Name} does not exist");
        }
    }


    public async Task SaveClient(Client client)
    {
        var id = await ExistClient(client.Name);

        if (id != null)
        {
            //Update could be implemented if there would be more properties
            await Task.CompletedTask;
        }
        else
        {
            await InsertClient(client);
        }
    }

    public async Task<IEnumerable<Client>?> GetAllClients()
    {
        const string Query = @"SELECT * FROM clients";

        var cnn = await _connectionFactory.Create();

        var clientRecords = await _queryExecutor.QueryAsync<ClientRecord>(cnn, Query);

        return clientRecords?.Select(clientRecord => clientRecord.Map());
    }

    public async Task<Client?> GetClientByName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        var clients = await GetAllClients();

        return clients?.SingleOrDefault(x => x.Name.Equals(name));
    }

    public async Task<int> DeleteClient(Client client)
    {
        const string Query = @"DELETE FROM clients WHERE name = @name";

        var cnn = await _connectionFactory.Create();

        return await cnn.ExecuteAsync(Query, new { name = client.Name });
    }
    
    private static Task AddProductsToAnOrder(IDbConnection connection, IDbTransaction transaction, int orderId)
    {
        throw new NotImplementedException();
    }


    private static Task<int> CreateOrder(IDbConnection connection, IDbTransaction transaction, int clientId)
    {
        return connection.ExecuteScalarAsync<int>(
            @"INSERT INTO orders(client_id) VALUES(@clientId) RETURNING id",
            new { clientId },
            transaction);
    }


    private async Task<int?> ExistClient(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        const string Query = @"SELECT id FROM clients WHERE name = @name";

        var cnn = await _connectionFactory.Create();

        return await cnn.ExecuteScalarAsync<int?>(Query, new { name });
    }
    
    private async Task<int?> ProductExist(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        const string Query = @"SELECT id FROM products WHERE name = @name";

        var cnn = await _connectionFactory.Create();

        return await cnn.ExecuteScalarAsync<int?>(Query, new { name });
    }
    private async Task InsertClient(Client client)
    {
        var cnn = await _connectionFactory.Create();
        //While inserting client is operation with just one table you don't need this, thinking about future here
        using var transaction = cnn.BeginTransaction();
        try
        {
            await InsertClient(cnn, transaction, client);
            transaction.Commit();
            cnn.Close();
        }
        catch
        {
            transaction.Rollback();
            cnn.Close();
            throw;
        }
    }

    private static Task InsertClient(IDbConnection connection, IDbTransaction transaction,
        Client client)
    {
        return connection.ExecuteAsync(
            @"INSERT INTO clients(name) VALUES(@name)",
            new { name = client.Name },
            transaction);
    }
    
    private async Task InsertProduct(Product product)
    {
        var cnn = await _connectionFactory.Create();
        //While inserting product is operation with just one table you don't need this, thinking about future here
        using var transaction = cnn.BeginTransaction();
        try
        {
            await InsertProduct(cnn, transaction, product);
            transaction.Commit();
            cnn.Close();
        }
        catch
        {
            transaction.Rollback();
            cnn.Close();
            throw;
        }
    }
    
    private async Task UpdateProduct(Product product)
    {
        var cnn = await _connectionFactory.Create();
        //While inserting product is operation with just one table you don't need this, thinking about future here
        using var transaction = cnn.BeginTransaction();
        try
        {
            await UpdateProduct(cnn, transaction, product);
            transaction.Commit();
            cnn.Close();
        }
        catch
        {
            transaction.Rollback();
            cnn.Close();
            throw;
        }
    }

    private static Task InsertProduct(IDbConnection connection, IDbTransaction transaction,
        Product product)
    {
        return connection.ExecuteAsync(
            @"INSERT INTO products(name, price) VALUES(@name, @price)",
            new {name = product.Name, price = product.Price},
            transaction);
    }
    
    private static Task UpdateProduct(IDbConnection connection, IDbTransaction transaction,
        Product product)
    {
        return connection.ExecuteAsync(
            @"UPDATE products SET price = @price WHERE name = @name",
            new {name = product.Name, price = product.Price},
            transaction);
    }

}

internal sealed class ClientRecord
{
    public string Name { get; init; } = null!;

    public Client Map()
    {
        return new Client(Name);
    }
}

// ReSharper disable once ClassNeverInstantiated.Global
internal sealed class ProductRecord
{
    private string Name { get; } = null!;
    
    // ReSharper disable once UnassignedGetOnlyAutoProperty
    private decimal Price { get; }

    public Product Map()
    {
        return new Product(Name,Price);
    }
}
