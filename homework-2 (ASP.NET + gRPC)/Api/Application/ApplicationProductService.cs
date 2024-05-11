using Domain;
using Infrastructure;

namespace Api.Application;

public class ApplicationProductService : IApplicationProductService
{
    private IProductRepository _productRepository;

    public ApplicationProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public int Add(ProductDto productDto)
    {
        var product = new Product
        {
            Name = productDto.Name,
            Price = productDto.Price,
            Weight = productDto.Weight,
            Type = (Domain.ProductType)productDto.Type,
            CreationDate = DateTime.Now,
            WarehouseId = productDto.WarehouseId
        };
        return _productRepository.Add(product);
    }

    public Product Get(int productId)
    {
        return _productRepository.Get(productId);
    }

    public Tuple<List<Product>, int> List(ProductType productType, DateTime? creationDate, long? warehouseId, ListProductRequest.Types.OrderField orderField, int page, int size)
    {
        return _productRepository.List(
            (Domain.ProductType)productType,
            creationDate,
            warehouseId,
            (OrderField)orderField,
            page,
            size
        );
    }

    public Product UpdatePrice(int productId, double price)
    {
        return _productRepository.UpdatePrice(productId, price);
    }
}

public record ProductDto(string Name, double Price, double Weight, ProductType Type, long WarehouseId);