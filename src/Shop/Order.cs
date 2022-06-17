namespace Shop;

public class Order
{
    public int Id { get; }
    
    public List<Product> Products { get; }

    public Order(int id, List<Product> products)
    {
        Id = id;
        Products = products;
    }
}

