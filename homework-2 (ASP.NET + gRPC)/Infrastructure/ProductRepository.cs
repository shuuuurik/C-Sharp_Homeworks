using System.Collections.Concurrent;
using Domain;

namespace Infrastructure;

public class ProductRepository : IProductRepository
{
    private ConcurrentDictionary<int, Product> _store;

    public ProductRepository()
    {
        _store = new ConcurrentDictionary<int, Product>();
    }

    public int Add(Product product)
    {
        var productId = Random.Shared.Next();
        product.Id = productId;
        _store.TryAdd(productId, product);
        return productId;
    }
    
    public Product Get(int productId)
    {
        // return _store[productId];
        if (_store.TryGetValue(productId, out var product))
        {
            return product;
        }
        throw new ArgumentException("No product with this id");
    }

    public Tuple<List<Product>, int> List(ProductType productType, DateTime? creationDate, long? warehouseId, OrderField field, int page, int size)
    {
        var filteredProducts = _store.Values
            .Where(p => (productType == ProductType.Common || p.Type == productType) && 
                          (creationDate is null || p.CreationDate.Date == creationDate.Value.Date) &&
                          (warehouseId is null || p.WarehouseId == warehouseId)).ToList();
        var filteredProductsCount = filteredProducts.Count;
        switch (field)
        {
            case OrderField.CreationDate:
                return new Tuple<List<Product>, int>(filteredProducts.OrderBy(p => p.CreationDate).Skip((page - 1) * size).Take(size).ToList(), filteredProductsCount);
            case OrderField.Type:
                return new Tuple<List<Product>, int>(filteredProducts.OrderBy(p => p.Type).Skip((page - 1) * size).Take(size).ToList(), filteredProductsCount);
            case OrderField.WarehouseId:
                return new Tuple<List<Product>, int>(filteredProducts.OrderBy(p => p.WarehouseId).Skip((page - 1) * size).Take(size).ToList(), filteredProductsCount);
            default:
                return new Tuple<List<Product>, int>(filteredProducts.Skip((page - 1) * size).Take(size).ToList(), filteredProductsCount);
        }
    }

    public Product UpdatePrice(int productId, double price)
    {
        if (_store.TryGetValue(productId, out var product))
        {
            product.Price = price;
            return product;
        }
        throw new ArgumentException("No product with this id");
    }
}