namespace ECommerce.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public decimal Price { get; private set; }
    public int Stock { get; private set; }
    public DateTime CreatedAt { get; private set; }

    private Product() { }

    public Product(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio.");

        if (price < 0)
            throw new ArgumentException("El precio no puede ser negativo.");

        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");

        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(string name, string description, decimal price, int stock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("El nombre es obligatorio.");

        if (price < 0)
            throw new ArgumentException("El precio no puede ser negativo.");

        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");

        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
    }
}