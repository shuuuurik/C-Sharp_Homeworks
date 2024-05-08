namespace Domain;

public class DemandCalc : IDemandCalc
{
    public int Calculate(int prediction, int quantityInStock)
    {
        var demand = prediction - quantityInStock;
        return demand > 0 ? demand : 0;
    }
}