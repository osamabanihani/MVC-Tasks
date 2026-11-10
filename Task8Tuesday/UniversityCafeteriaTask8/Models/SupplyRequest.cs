namespace UniversityCafeteriaTask8.Models;

public class SupplyRequest
{
    public int Id { get; set; }
    public DateTime RequestDate { get; set; } = DateTime.Now;
    public decimal TotalPrice { get; set; }
    public string Status { get; set; } = "Pending";
    public string UserId { get; set; } = "";
    public ApplicationUser? User { get; set; }
    public List<RequestItem> Items { get; set; } = new();
}
