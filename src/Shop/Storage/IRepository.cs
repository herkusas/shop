namespace Shop.Storage;

public interface IRepo
{
    Task SaveClient(Client client);

    Task<IEnumerable<Client>?> GetAllClients();

    Task<Client?> GetClientByName(string name);

    Task<int> DeleteClient(Client client);
    
    Task SaveProduct(Product client);

    Task<IEnumerable<Product>?> GetProducts();
    
    Task<Product?> GetProductByName(string name);
    
    Task<int> DeleteProduct(Product product);

    Task MakeOrder(Client client, List<Product> products);
}
