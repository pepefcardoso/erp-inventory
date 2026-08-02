using ErpInventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ErpInventory.Infrastructure.Persistence;

public class ErpInventoryDbContext(DbContextOptions<ErpInventoryDbContext> options) : DbContext(options)
{
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<StockMovement> StockMovements => Set<StockMovement>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Product>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Sku).IsRequired().HasMaxLength(64);
            e.HasIndex(p => p.Sku).IsUnique();
            e.Property(p => p.Name).IsRequired().HasMaxLength(256);
            e.Property(p => p.UnitPrice).HasColumnType("numeric(18,2)");
        });

        builder.Entity<Warehouse>(e =>
        {
            e.HasKey(w => w.Id);
            e.Property(w => w.Name).IsRequired().HasMaxLength(256);
            e.Property(w => w.Location).IsRequired().HasMaxLength(256);
        });

        builder.Entity<StockMovement>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Type).HasConversion<string>().HasMaxLength(32);
            e.HasIndex(m => new { m.ProductId, m.OccurredAtUtc });
        });

        base.OnModelCreating(builder);
    }
}
