namespace UniversityCafeteriaTask8.Models;

public class RequestItem
{
    public int Id { get; set; }
    public int SupplyRequestId { get; set; }
    public SupplyRequest? SupplyRequest { get; set; }
    public int ProductId { get; set; }
    public Product? Product { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}
