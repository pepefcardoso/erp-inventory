namespace ErpInventory.Domain.Entities;

public enum StockMovementType { Inbound, Outbound }

public class StockMovement
{
    public Guid Id { get; private set; }
    public Guid ProductId { get; private set; }
    public Guid WarehouseId { get; private set; }
    public StockMovementType Type { get; private set; }
    public int Quantity { get; private set; }
    public DateTime OccurredAtUtc { get; private set; }

    private StockMovement() { }

    public StockMovement(Guid productId, Guid warehouseId, StockMovementType type, int quantity)
    {
        if (quantity <= 0)
            throw new ArgumentException("Quantity must be positive.", nameof(quantity));

        Id = Guid.NewGuid();
        ProductId = productId;
        WarehouseId = warehouseId;
        Type = type;
        Quantity = quantity;
        OccurredAtUtc = DateTime.UtcNow;
    }
}