using System.Net;
using System.Net.Http.Json;
using Api;
using Api.Application;
using FluentAssertions;
using UnitTests;

namespace IntegrationTests;

public class ProductServiceTests : IClassFixture<MyCustomWebApplicationFactory<Startup>>
{
    private readonly MyCustomWebApplicationFactory<Startup> _webApplicationFactory;
    
    public ProductServiceTests(MyCustomWebApplicationFactory<Startup> webApplicationFactory)
    {
        _webApplicationFactory = webApplicationFactory;
    }
    
    [Fact]
    public async Task Add_AddCorrectProduct_ShouldReturnProductId()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(100).WithWeight(1).Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.OK 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Add_AddProductWithEmptyName_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(100).WithWeight(1).WithName("").Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Add_AddProductWithLongName_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(100).WithWeight(1).WithName(new string('a', 31)).Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Add_AddProductWithNegativePrice_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(-1).WithWeight(1).Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Add_AddProductWithBigPrice_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(1000000001).WithWeight(1).Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Add_AddProductWithNegativeWeight_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(100).WithWeight(-1).Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Add_AddProductWithBigWeight_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(100).WithWeight(1001).Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Add_AddProductWithNegativeWarehouseId_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(
            new ProductDtoBuilder().WithPrice(100).WithWeight(1).WithWarehouseId(-1).Build());
        
        // Act
        var response = await client.PostAsync("/v1/product/add", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Post
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Get_ProductExistInService_ShouldReturnProduct()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/get/1");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.OK 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Get_ProductNotExistInService_ShouldReturn400StatusCode()
    {
        // Arrange
        // Переопределяем Mock из конструктора MyCustomWebApplicationFactory
        int productId = 1;
        string expectedExceptionMessage = "No product with this id";
        _webApplicationFactory.ProductRepositoryFake
            .Setup(f => f.Get(productId))
            .Throws(new ArgumentException(expectedExceptionMessage));
        
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/get/1");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Get_ProductWithNegativeId_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/get/-1");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task Get_ProductWithBigId_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/get/10000000000");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task List_ShouldReturnAllProducts()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/list?page=1&size=2");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.OK 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "?page=1&size=2")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task List_FilterWithNegativeWarehouseId_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/list?warehouseId=-1&page=1&size=2");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "?warehouseId=-1&page=1&size=2")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task List_FilterWithNegativePage_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/list?page=-1&size=2");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "?page=-1&size=2")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task List_FilterWithNegativeSize_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/list?page=1&size=-1");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "?page=1&size=-1")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task List_FilterWithBigSize_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        
        // Act
        var response = await client.GetAsync("/v1/product/list?page=1&size=11");
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Get
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "?page=1&size=11")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task UpdatePrice_ProductExistInService_ShouldUpdatePriceAndReturnProduct()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(new PutBody(1000));
        
        // Act
        var response = await client.PutAsync("/v1/product/update/1", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.OK 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Put
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task UpdatePrice_ProductNotExistInService_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(new PutBody(1000));
        
        // Переопределяем Mock из конструктора MyCustomWebApplicationFactory
        int productId = 1;
        double newPrice = 1000;
        string expectedExceptionMessage = "No product with this id";

        _webApplicationFactory.ProductRepositoryFake
            .Setup(f => f.UpdatePrice(productId, newPrice))
            .Throws(new ArgumentException(expectedExceptionMessage));
        
        // Act
        var response = await client.PutAsync("/v1/product/update/1", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Put
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task UpdatePrice_ProductWithNegativeId_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(new PutBody(1000));
        
        // Act
        var response = await client.PutAsync("/v1/product/update/-1", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Put
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task UpdatePrice_ProductWithNegativePrice_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(new PutBody(-1000));
        
        // Act
        var response = await client.PutAsync("/v1/product/update/1", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Put
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }
    
    [Fact]
    public async Task UpdatePrice_ProductWithBigPrice_ShouldReturn400StatusCode()
    {
        // Arrange
        var client = _webApplicationFactory.CreateClient();
        HttpContent content = JsonContent.Create(new PutBody(1000000001));
        
        // Act
        var response = await client.PutAsync("/v1/product/update/1", content);
        var result = response.Content.ReadAsStringAsync();
        
        // Assert
        response.Should().Match(r => r.StatusCode == HttpStatusCode.BadRequest 
                                     && r.Version == HttpVersion.Version11
                                     && r.RequestMessage.Method == HttpMethod.Put
                                     && r.RequestMessage.RequestUri.Scheme == "http"
                                     && r.RequestMessage.RequestUri.Query == "")
            .And.NotBeNull(result.Result);
    }

    private record PutBody(double Price);
}