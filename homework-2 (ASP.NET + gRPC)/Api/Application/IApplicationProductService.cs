using Domain;

namespace Api.Application;

public interface IApplicationProductService
{
    int Add(ProductDto product);
    Product Get(int productId);
    Tuple<List<Product>, int> List(ProductType productType, DateTime? creationDate, long? warehouseId, ListProductRequest.Types.OrderField orderField, int page, int size);
    Product UpdatePrice(int productId, double price);
}