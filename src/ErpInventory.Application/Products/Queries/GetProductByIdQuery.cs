using MediatR;

namespace ErpInventory.Application.Products.Queries;

public record GetProductByIdQuery(Guid Id) : IRequest<ProductDto?>;

public record ProductDto(Guid Id, string Sku, string Name, decimal UnitPrice, int QuantityOnHand);