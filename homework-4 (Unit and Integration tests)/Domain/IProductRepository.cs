namespace Domain;

public interface IProductRepository
{
    int Add(Product product);
    Product Get(int productId);
    Tuple<List<Product>, int> List(ProductType productType, DateTime? creationDate, long? warehouseId, OrderField field, int page, int size);
    Product UpdatePrice(int productId, double price);
}