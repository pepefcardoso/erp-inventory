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
        var createResponse = await _client.PostAsJsonAsync("/api/products",
            new CreateProductCommand("SKU-INT-001", "Integration Widget", 19.99m));

        createResponse.EnsureSuccessStatusCode();
        var id = await createResponse.Content.ReadFromJsonAsync<Guid>();

        var getResponse = await _client.GetAsync($"/api/products/{id}");
        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
        var dto = await getResponse.Content.ReadFromJsonAsync<ProductDto>();

        Assert.NotNull(dto);
        Assert.Equal("SKU-INT-001", dto!.Sku);
    }
}
