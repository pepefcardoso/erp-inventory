using ErpInventory.Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ErpInventory.IntegrationTests;

public class ProductsApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.RemoveAll<DbContextOptions<ErpInventoryDbContext>>();
            services.AddDbContext<ErpInventoryDbContext>(options =>
                options.UseNpgsql("Host=localhost;Port=5432;Database=erp_inventory_test;Username=postgres;Password=postgres"));

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ErpInventoryDbContext>();
            db.Database.Migrate();
        });
    }
}
