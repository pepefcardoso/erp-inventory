namespace ErpInventory.Domain.Entities;

public class Product
{
    public Guid Id { get; private set; }
    public string Sku { get; private set; } = null!;
    public string Name { get; private set; } = null!;
    public decimal UnitPrice { get; private set; }
    public int QuantityOnHand { get; private set; }

    private Product() { }

    public Product(string sku, string name, decimal unitPrice)
    {
        if (string.IsNullOrWhiteSpace(sku))
            throw new ArgumentException("SKU is required.", nameof(sku));
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.", nameof(name));
        if (unitPrice < 0)
            throw new ArgumentException("Unit price cannot be negative.", nameof(unitPrice));

        Id = Guid.NewGuid();
        Sku = sku;
        Name = name;
        UnitPrice = unitPrice;
        QuantityOnHand = 0;
    }

    public void ReceiveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        QuantityOnHand += quantity;
    }

    public void RemoveStock(int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));
        if (quantity > QuantityOnHand)
            throw new InvalidOperationException("Cannot remove more stock than available.");
        QuantityOnHand -= quantity;
    }
}