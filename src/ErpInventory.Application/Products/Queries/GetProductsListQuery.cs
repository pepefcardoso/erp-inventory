using ErpInventory.Application.Common.Models;
using MediatR;

namespace ErpInventory.Application.Products.Queries;

public record GetProductsListQuery(int PageNumber = 1, int PageSize = 10) : IRequest<PaginatedList<ProductDto>>;
