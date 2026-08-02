using ErpInventory.Domain.Entities;

namespace ErpInventory.UnitTests.Domain;

public class ProductTests
{
    [Fact]
    public void RemoveStock_WhenQuantityExceedsOnHand_ThrowsInvalidOperationException()
    {
        var product = new Product("SKU-001", "Widget", 9.99m);
        product.ReceiveStock(5);

        Assert.Throws<InvalidOperationException>(() => product.RemoveStock(10));
    }
}
