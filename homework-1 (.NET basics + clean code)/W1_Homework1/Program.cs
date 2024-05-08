using DataAccess;
using Domain;

IProductHistoryRepository productHistoryRepository = new ProductHistoryRepository();
IAdsCalc adsCalc = new AdsCalc();
IPredictionCalc predictionCalc = new PredictionCalc();
IDemandCalc demandCalc = new DemandCalc();

while (true)
{
    try
    {
        var commandLine = Console.ReadLine().Split();
        var command = commandLine[0];

        if (commandLine.Length < 2)
        {
            throw new ArgumentException("Any command must consist of at least two arguments");
        }
        
        if (!int.TryParse(commandLine[1], out int id))
        {
            throw new ArgumentException("Second argument (id) must be an integer");
        }
        var productHistory = productHistoryRepository.Get(id);

        switch (command)
        {
            case "ads":
                if (commandLine.Length != 2)
                {
                    throw new ArgumentException(
                        $"Incorrect number of arguments: expected 2, but it was {commandLine.Length}");
                }
                
                var ads = adsCalc.Calculate(productHistory);
                Console.WriteLine($"Product {productHistory.Id} {Environment.NewLine}" +
                                  $"ADS {ads} {Environment.NewLine}");
                break;
            case "prediction":
                if (commandLine.Length != 3)
                {
                    throw new ArgumentException(
                        $"Incorrect number of arguments: expected 3, but it was {commandLine.Length}");
                }
                if (!int.TryParse(commandLine[2], out int days))
                {
                    throw new ArgumentException("Third argument (number of days) must be an integer");
                }
                
                ads = adsCalc.Calculate(productHistory);
                var prediction = predictionCalc.Calculate(productHistory, ads, days);
                Console.WriteLine($"Product {productHistory.Id} {Environment.NewLine}" +
                                  $"ADS {ads} {Environment.NewLine}" +
                                  $"Prediction for {days} days: {prediction} {Environment.NewLine}");
                break;
            case "demand":
                if (commandLine.Length != 3)
                {
                    throw new ArgumentException(
                        $"Incorrect number of arguments: expected 3, but it was {commandLine.Length}");
                }
                if (!int.TryParse(commandLine[2], out days))
                {
                    throw new ArgumentException("Third argument (number of days) must be an integer");
                }
                
                ads = adsCalc.Calculate(productHistory);
                prediction = predictionCalc.Calculate(productHistory, ads, days);

                var lastRow = productHistory.SalesRows.MaxBy(s => s.SaleDate);
                var quantityInStock = lastRow.StockCount - lastRow.SalesCount;
                var demand = demandCalc.Calculate(prediction, quantityInStock);

                Console.WriteLine($"Product {productHistory.Id} {Environment.NewLine}" +
                                  $"ADS {ads} {Environment.NewLine}" +
                                  $"Prediction for {days} days: {prediction} {Environment.NewLine}" +
                                  $"Quantity of the product in stock: {quantityInStock} {Environment.NewLine}" +
                                  $"Demand for {days} days: {demand} {Environment.NewLine}");
                break;
            default:
                throw new NotSupportedException($"Unknown command {Environment.NewLine}");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine(e.Message + Environment.NewLine);
    }
}