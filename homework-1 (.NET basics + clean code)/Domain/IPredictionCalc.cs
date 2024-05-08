namespace Domain;

public interface IPredictionCalc
{
    int Calculate(ProductHistory productHistory, decimal ads, int days);
}