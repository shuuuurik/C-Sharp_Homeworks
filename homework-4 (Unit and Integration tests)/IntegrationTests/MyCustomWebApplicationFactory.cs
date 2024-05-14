using Api;
using AutoBogus;
using AutoBogus.Conventions;
using Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Moq;
using ProductType = Domain.ProductType;

namespace IntegrationTests;

public class MyCustomWebApplicationFactory<TEntryPoint> : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    public readonly Mock<IProductRepository> ProductRepositoryFake = new();
    
    public MyCustomWebApplicationFactory()
    {
        // ADD
        SetupAdd();
        
        // GET
        SetupGet();
        
        // LIST
        SetupList();
        
        // UPDATEPRICE
        SetupUpdatePrice();
    }
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.Replace(new ServiceDescriptor(typeof(IProductRepository), ProductRepositoryFake.Object));
        });
    }

    private void SetupAdd()
    {
        int expectedId = 1;
        
        ProductRepositoryFake
            .Setup(f => f.Add(It.IsAny<Product>()))
            .Returns(expectedId);
    }

    private void SetupGet()
    {
        int productId = 1;
        AutoFaker.Configure(f => f.WithConventions());
        var expectedProduct = new AutoFaker<Product>().Generate();
        
        ProductRepositoryFake
            .Setup(f => f.Get(productId))
            .Returns(expectedProduct);
    }

    private void SetupList()
    {
        var productType = Api.ProductType.Common;
        DateTime? creationDate = null;
        long? warehouseId = null;
        ListProductRequest.Types.OrderField orderField = ListProductRequest.Types.OrderField.Noorderfield;
        int page = 1;
        int size = 2;
        
        int filteredProductsCount = 10;

        AutoFaker.Configure(f => f.WithConventions());
        var productList = new AutoFaker<Product>().Generate(size);
        var expectedResponse = new Tuple<List<Product>, int>(productList, filteredProductsCount);
        
        ProductRepositoryFake
            .Setup(f => f.List((ProductType)productType, creationDate, warehouseId, (OrderField)orderField, page, size))
            .Returns(expectedResponse);
    }

    private void SetupUpdatePrice()
    {
        int productId = 1;
        double newPrice = 1000;
        AutoFaker.Configure(f => f.WithConventions());
        var expectedProduct = new AutoFaker<Product>().Generate();
        expectedProduct.Price = newPrice;
        
        ProductRepositoryFake
            .Setup(f => f.UpdatePrice(productId, newPrice))
            .Returns(expectedProduct);
    }
}