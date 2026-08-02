using MediatR;

namespace ErpInventory.Application.Products.Commands;

public record CreateProductCommand(string Sku, string Name, decimal UnitPrice) : IRequest<Guid>;
