namespace Domain;

public interface IProductHistoryRepository
{
    ProductHistory Get(int productId);
}