using ErpInventory.Application.Common.Interfaces;
using ErpInventory.Infrastructure.Persistence;
using ErpInventory.Infrastructure.Persistence.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<ErpInventoryDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(ErpInventory.Application.AssemblyReference).Assembly));

builder.Services.AddValidatorsFromAssembly(typeof(ErpInventory.Application.AssemblyReference).Assembly);

builder.Services.AddScoped<IProductRepository, ProductRepository>();

var app = builder.Build();

app.MapControllers();
app.Run();
