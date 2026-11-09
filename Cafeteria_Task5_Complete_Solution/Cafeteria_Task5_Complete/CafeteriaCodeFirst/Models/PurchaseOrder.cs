namespace CafeteriaCodeFirst.Models;

public class PurchaseOrder
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public ICollection<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new List<PurchaseOrderItem>();
}
