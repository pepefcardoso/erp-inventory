using System.Net;
using System.Net.Http.Json;
using ErpInventory.Application.Products.Commands;
using ErpInventory.Application.Products.Queries;

namespace ErpInventory.IntegrationTests;

public class ProductsControllerTests : IClassFixture<ProductsApiFactory>
{
    private readonly HttpClient _client;

    public ProductsControllerTests(ProductsApiFactory factory) => _client = factory.CreateClient();

    [Fact]
    public async Task CreateThenGetById_ReturnsCreatedProduct()
    {
        var sku = $"SKU-INT-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        var createResponse = await _client.PostAsJsonAsync("/api/products",
            new CreateProductCommand(sku, "Integration Widget", 19.99m));

        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var getResponse = await _client.GetAsync($"/api/products/{id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var dto = await getResponse.Content.ReadFromJsonAsync<ProductDto>();

        Assert.NotNull(dto);
        Assert.Equal(sku, dto!.Sku);
    }

    [Fact]
    public async Task GetProductsList_ReturnsPagedList()
    {
        var sku = $"SKU-INT-{Guid.NewGuid().ToString("N").Substring(0, 8)}";
        await _client.PostAsJsonAsync("/api/products",
            new CreateProductCommand(sku, "List Widget 1", 10.99m));

        var getResponse = await _client.GetAsync("/api/products?pageNumber=1&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var pagedList = await getResponse.Content.ReadFromJsonAsync<ErpInventory.Application.Common.Models.PaginatedList<ProductDto>>();
        Assert.NotNull(pagedList);
        Assert.True(pagedList!.Items.Count > 0);
        Assert.True(pagedList.TotalCount > 0);
        Assert.Equal(1, pagedList.PageNumber);
    }

    [Fact]
    public async Task GetProductsList_WhenOutOfRange_ReturnsEmptyList()
    {
        var getResponse = await _client.GetAsync("/api/products?pageNumber=9999&pageSize=10");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

        var pagedList = await getResponse.Content.ReadFromJsonAsync<ErpInventory.Application.Common.Models.PaginatedList<ProductDto>>();
        Assert.NotNull(pagedList);
        Assert.Empty(pagedList!.Items);
    }
}
