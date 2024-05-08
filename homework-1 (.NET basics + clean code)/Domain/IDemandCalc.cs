namespace Domain;

public interface IDemandCalc
{
    int Calculate(int prediction, int quantityInStock);
}