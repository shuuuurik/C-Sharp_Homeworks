namespace Domain;

public class PredictionCalc : IPredictionCalc
{
    public int Calculate(ProductHistory productHistory, decimal ads, int days)
    {
        decimal prediction;
        var lastRow = productHistory.SalesRows.MaxBy(s => s.SaleDate);
        var lastPredictionDayDateTime = lastRow.SaleDate.AddDays(days);
        
        if (lastRow.SaleDate.Year == lastPredictionDayDateTime.Year && lastRow.SaleDate.Month == lastPredictionDayDateTime.Month)
        {
            prediction = ads * (lastPredictionDayDateTime.Day - lastRow.SaleDate.Day) *
                         productHistory.SeasonCoefs[lastRow.SaleDate.Month];
        }
        else
        {
            prediction =
                ads *
                (DateTime.DaysInMonth(lastRow.SaleDate.Year, lastRow.SaleDate.Month) - lastRow.SaleDate.Day) *
                productHistory.SeasonCoefs[lastRow.SaleDate.Month] +
                ads * lastPredictionDayDateTime.Day * productHistory.SeasonCoefs[lastPredictionDayDateTime.Month];
            for (var i = lastRow.SaleDate.AddMonths(1);
                 i.Month != lastPredictionDayDateTime.Month || i.Year != lastPredictionDayDateTime.Year;
                 i = i.AddMonths(1))
            {
                prediction += ads * DateTime.DaysInMonth(i.Year, i.Month) * productHistory.SeasonCoefs[i.Month];
            }
        }
        return (int)Math.Ceiling(prediction);
    }
}