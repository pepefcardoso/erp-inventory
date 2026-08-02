using ErpInventory.Application.Common.Interfaces;
using ErpInventory.Domain.Entities;
using MediatR;

namespace ErpInventory.Application.Products.Commands;

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
{
    private readonly IProductRepository _repository;

    public CreateProductCommandHandler(IProductRepository repository) => _repository = repository;

    public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = new Product(request.Sku, request.Name, request.UnitPrice);
        await _repository.AddAsync(product, cancellationToken);
        return product.Id;
    }
}
