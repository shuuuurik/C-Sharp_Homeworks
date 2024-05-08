namespace Domain;

public class ProductHistory
{
    public int Id { get; init; }
    public List<SaleRow> SalesRows { get; init; }
    public Dictionary<int, decimal> SeasonCoefs { get; init; }
    public decimal Ads { get; set; } = -1;
}