namespace CafeteriaDay7.Models;

public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public DateTime OrderDate { get; set; } = DateTime.Today;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }
}
