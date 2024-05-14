using Api;
using Api.Application;

namespace UnitTests;

public class ProductDtoBuilder
{
    private string? _name;
    private double? _price;
    private double? _weight;
    private ProductType? _type;
    private long? _warehouseId;

    public ProductDtoBuilder WithName(string name)
    {
        _name = name;
        return this;
    }
    
    public ProductDtoBuilder WithPrice(double price)
    {
        _price = price;
        return this;
    }
    
    public ProductDtoBuilder WithWeight(double weight)
    {
        _weight = weight;
        return this;
    }
    
    public ProductDtoBuilder WithType(ProductType type)
    {
        _type = type;
        return this;
    }
    
    public ProductDtoBuilder WithWarehouseId(long warehouseId)
    {
        _warehouseId = warehouseId;
        return this;
    }

    public ProductDto Build()
    {
        return new ProductDto
        (
            _name ?? nameof(_name),
            _price ?? Random.Shared.Next(),
            _weight ?? Random.Shared.Next(),
            _type ?? ProductType.Common,
            _warehouseId ?? Random.Shared.Next()
        );
    }
}