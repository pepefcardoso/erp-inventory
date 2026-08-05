using ErpInventory.Application.Common.Interfaces;
using ErpInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ErpInventory.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ErpInventoryDbContext _context;

    public ProductRepository(ErpInventoryDbContext context) => _context = context;

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken) =>
        await _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public async Task AddAsync(Product product, CancellationToken cancellationToken)
    {
        await _context.Products.AddAsync(product, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<(IEnumerable<Product> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken)
    {
        var totalCount = await _context.Products.CountAsync(cancellationToken);

        if (totalCount == 0)
        {
            return (Enumerable.Empty<Product>(), 0);
        }

        var items = await _context.Products
            .AsNoTracking()
            .OrderBy(p => p.Id)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }
}
