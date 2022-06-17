using Shop;
using Shop.Storage;
using Shop.Storage.Postgre.DataLayer;
using Shop.Storage.Postgre.DataLayer.Connection;
using Shop.Storage.Postgre.DataLayer.Default;
using Shop.Storage.Wrappers;

const string ConnectionString = "HOST=::1;PORT=5432;DATABASE=shop;Uid=postgres;Pwd=admin;";

var connectionFactory = new ConnectionFactory(ConnectionString);

IQueryExecutor queryExecutor = new QueryExecutor();

IRepo repo = new DefaultRepository(connectionFactory, queryExecutor);

var client = new Client("MeganFox");

await repo.SaveClient(client);

var product = new Product("Table", 10);

await repo.SaveProduct(product);

var products = await repo.GetProducts();

if (products != null)
{
    foreach (var p in products)
    {
        Console.WriteLine($"Name: {p.Name} Price: {p.Price}");
    }
}

//await clientStore.Delete(client);

//await repo.MakeOrder(client);
