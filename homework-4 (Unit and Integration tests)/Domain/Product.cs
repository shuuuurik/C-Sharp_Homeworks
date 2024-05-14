namespace Domain;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public double Price { get; set; }
    public double Weight { get; set; }
    public ProductType Type { get; set; }
    public DateTime CreationDate { get; set; }
    public long WarehouseId { get; set; }
}