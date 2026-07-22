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
}