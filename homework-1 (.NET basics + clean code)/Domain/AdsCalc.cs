namespace Domain;

public class AdsCalc : IAdsCalc
{
    public decimal Calculate(ProductHistory productHistory)
    {
        if (productHistory.Ads != -1)
        {
            return productHistory.Ads;
        }
        var rowsWithProductInStock = productHistory.SalesRows.Where(s => s.StockCount != 0).ToArray();
        var totalSales = rowsWithProductInStock.Sum(s => s.SalesCount);
        var ads = Math.Round((decimal)totalSales / rowsWithProductInStock.Count(), 3);
        productHistory.Ads = ads;
        return ads;
    }
}