using Domain;
namespace DataAccess;

public class ProductHistoryRepository : IProductHistoryRepository
{
    private static readonly List<SaleRow> SalesHistory = new();
    private static readonly List<string[]> CoefsStrings = new();
    private static readonly List<ProductSeasonCoefs> ProductCoefs = new();
    
    public ProductHistoryRepository()
    {   
        var exePath = AppDomain.CurrentDomain.BaseDirectory;
        var salesHistoryPath = Path.Combine(exePath, "SalesHistory.txt");
        using var reader = new StreamReader(salesHistoryPath);
        reader.ReadLine(); // delete first line with column names
        
        while (!reader.EndOfStream)
        {
            var row = reader.ReadLine().Split(", ");
            SalesHistory.Add(new SaleRow {
                ProductId = int.Parse(row[0]),
                SaleDate = DateTime.Parse(row[1]),
                SalesCount = int.Parse(row[2]),
                StockCount = int.Parse(row[3]),
            });
        }
        
        var seasonCoefsPath = Path.Combine(exePath, "SeasonCoefs.txt");
        using var readerCoefs = new StreamReader(seasonCoefsPath);
        readerCoefs.ReadLine(); // delete first line with column names
        
        while (!readerCoefs.EndOfStream)
        {
            CoefsStrings.Add(readerCoefs.ReadLine().Split(", "));
        }

        var coefsByProduct = CoefsStrings.GroupBy(s => int.Parse(s[0]));
        foreach (var productCoefs in coefsByProduct)
        {
            ProductCoefs.Add(new ProductSeasonCoefs
            {
                ProductId = productCoefs.Key,
                SeasonCoefs = productCoefs
                    .Select(s => (int.Parse(s[1]), decimal.Parse(s[2].Replace('.', ','))))
                    .ToDictionary()
            });
        }
    }
        
    public ProductHistory Get(int productId)
    {
        var productHistory = new ProductHistory()
        {
            Id = productId,
            SalesRows = SalesHistory
                .Where(s => productId == s.ProductId)
                .ToList(),
            SeasonCoefs = ProductCoefs
                .First(s => productId == s.ProductId).SeasonCoefs
        };
        if (productHistory.SalesRows.Count == 0)
        {
            throw new NotSupportedException("No product with this id");
        }
        return productHistory;
    }
    
    private class ProductSeasonCoefs
    {
        public int ProductId { get; init; }
        public Dictionary<int, decimal> SeasonCoefs { get; init; }
    }
}