using Api;
using Api.Application;
using AutoBogus;
using AutoBogus.Conventions;
using Bogus;
using Domain;
using Moq;
using ProductType = Domain.ProductType;

namespace UnitTests;

public class ProductServiceTests
{
    private Mock<IProductRepository> _productRepositoryFake = new(MockBehavior.Strict);
    
    private readonly IApplicationProductService _productService;

    public ProductServiceTests()
    {
        _productService = new ApplicationProductService(_productRepositoryFake.Object);
    }

    [Fact]
    public void Add_ShouldReturnProductId()
    {
        // Arrange
        int expectedId = 1;
        var productDto = new ProductDtoBuilder().Build();

        _productRepositoryFake
            .Setup(f => f.Add(It.IsAny<Product>()))
            .Returns(expectedId);
        
        // Act
        var actualId = _productService.Add(productDto);
        
        // Assert
        Assert.Equal(expectedId, actualId);
        _productRepositoryFake.Verify(f => f.Add(It.IsAny<Product>()), Times.Once);
    }
    
    [Fact]
    public void Get_ProductExistInRepository_ShouldReturnProductFromRepository()
    {
        // Arrange
        int productId = 1;
        
        // AutoFixture
        // var expectedProduct = new Fixture().Create<Product>();
        
        // Bogus
        var expectedProduct = new Faker<Product>()
            .RuleFor(f => f.Id, f => f.IndexFaker)
            .RuleFor(f => f.Name, f => f.Commerce.ProductName())
            .RuleFor(f => f.Price, f => double.Parse(f.Commerce.Price()))
            .RuleFor(f => f.Weight, f => f.IndexFaker)
            .RuleFor(f => f.Type, f => ProductType.Common)
            .RuleFor(f => f.CreationDate, f => DateTime.Now)
            .RuleFor(f => f.WarehouseId, f => f.IndexFaker)
            .Generate();
        
        // AutoBogus
        // AutoFaker.Configure(f => f.WithConventions());
        // var expectedProduct = new AutoFaker<Product>().Generate();

        _productRepositoryFake
            .Setup(f => f.Get(productId))
            .Returns(expectedProduct);
        
        // Act
        var actualProduct = _productService.Get(productId);
        
        // Assert
        Assert.NotNull(actualProduct);
        Assert.Equal(expectedProduct, actualProduct);
        _productRepositoryFake.Verify(f => f.Get(productId), Times.Once);
    }
    
    [Fact]
    public void Get_ProductNotExistInRepository_ShouldThrowArgumentException()
    {
        // Arrange
        int productId = 1;
        string expectedExceptionMessage = "No product with this id";

        _productRepositoryFake
            .Setup(f => f.Get(productId))
            .Throws(new ArgumentException(expectedExceptionMessage));
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() => _productService.Get(productId));
        
        // Assert
        Assert.Equal(ex.Message, expectedExceptionMessage);
        _productRepositoryFake.Verify(f => f.Get(productId), Times.Once);
    }
    
    [Fact]
    public void List_ProductsForFilterExistInRepository_ShouldReturnFilteredProductsListFromRepository()
    {
        // Arrange
        var productType = Api.ProductType.Common;
        DateTime? creationDate = null;
        long? warehouseId = null;
        ListProductRequest.Types.OrderField orderField = ListProductRequest.Types.OrderField.Noorderfield;
        int page = 1;
        int size = 2;
        
        int filteredProductsCount = 10;
        // AutoBogus
        AutoFaker.Configure(f => f.WithConventions());
        var productList = new AutoFaker<Product>().Generate(size);
        var expectedResponse = new Tuple<List<Product>, int>(productList, filteredProductsCount);

        _productRepositoryFake
            .Setup(f => f.List((ProductType)productType, creationDate, warehouseId, (OrderField)orderField, page, size))
            .Returns(expectedResponse);
        
        // Act
        var actualResponse = _productService.List(productType, creationDate, warehouseId, orderField, page, size);
        
        // Assert
        Assert.NotNull(actualResponse);
        Assert.NotEmpty(actualResponse.Item1);
        Assert.Equal(expectedResponse, actualResponse);
        _productRepositoryFake.Verify(f => f.List((ProductType)productType, creationDate, warehouseId, (OrderField)orderField, page, size), Times.Once);
    }
    
    [Fact]
    public void UpdatePrice_ProductExistInRepository_ShouldUpdatePriceAndReturnProductFromRepository()
    {
        // Arrange
        int productId = 1;
        double newPrice = 1000;
        
        // AutoBogus
        AutoFaker.Configure(f => f.WithConventions());
        var expectedProduct = new AutoFaker<Product>().Generate();
        expectedProduct.Price = newPrice;
            
        _productRepositoryFake
            .Setup(f => f.UpdatePrice(productId, newPrice))
            .Returns(expectedProduct);
        
        // Act
        var actualProduct = _productService.UpdatePrice(productId, newPrice);
        
        // Assert
        Assert.NotNull(actualProduct);
        Assert.Equal(expectedProduct, actualProduct);
        _productRepositoryFake.Verify(f => f.UpdatePrice(productId, newPrice), Times.Once);
    }
    
    [Fact]
    public void UpdatePrice_ProductNotExistInRepository_ShouldThrowArgumentException()
    {
        // Arrange
        int productId = 1;
        double newPrice = 1000;
        string expectedExceptionMessage = "No product with this id";

        _productRepositoryFake
            .Setup(f => f.UpdatePrice(productId, newPrice))
            .Throws(new ArgumentException(expectedExceptionMessage));
        
        // Act
        var ex = Assert.Throws<ArgumentException>(() => _productService.UpdatePrice(productId, newPrice));
        
        // Assert
        Assert.Equal(ex.Message, expectedExceptionMessage);
        _productRepositoryFake.Verify(f => f.UpdatePrice(productId, newPrice), Times.Once);
    }
}