using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ErpInventory.Application.Common.Interfaces;
using ErpInventory.Application.Common.Models;
using MediatR;

namespace ErpInventory.Application.Products.Queries;

public class GetProductsListQueryHandler : IRequestHandler<GetProductsListQuery, PaginatedList<ProductDto>>
{
    private readonly IProductRepository _repository;

    public GetProductsListQueryHandler(IProductRepository repository)
    {
        _repository = repository;
    }

    public async Task<PaginatedList<ProductDto>> Handle(GetProductsListQuery request, CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 1 : request.PageSize;

        var (items, totalCount) = await _repository.GetPagedAsync(pageNumber, pageSize, cancellationToken);

        var dtos = items.Select(product => new ProductDto(
            product.Id,
            product.Sku,
            product.Name,
            product.UnitPrice,
            product.QuantityOnHand)).ToList();

        return new PaginatedList<ProductDto>(dtos, totalCount, pageNumber, pageSize);
    }
}
