using System.ComponentModel.DataAnnotations;
namespace CafeteriaDbFirst.Models;

public class Category
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [StringLength(500)] public string? Description { get; set; }
    public List<Product> Products { get; set; } = new();
}
public class Supplier
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [Required, EmailAddress, StringLength(150)] public string Email { get; set; } = "";
    [Required, StringLength(30)] public string Phone { get; set; } = "";
    public List<Product> Products { get; set; } = new();
    public List<PurchaseOrder> PurchaseOrders { get; set; } = new();
}
public class Product
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [Range(typeof(decimal), "0", "99999999")] public decimal Price { get; set; }
    [Range(0, int.MaxValue)] public int Quantity { get; set; }
    [Range(1, int.MaxValue)] public int CategoryId { get; set; }
    [Range(1, int.MaxValue)] public int SupplierId { get; set; }
    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }
    public List<PurchaseOrderItem> PurchaseOrderItems { get; set; } = new();
}
public class PurchaseOrder
{
    public int Id { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending";
    public int SupplierId { get; set; }
    public Supplier? Supplier { get; set; }
    public List<PurchaseOrderItem> Items { get; set; } = new();
}
public class PurchaseOrderItem
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public PurchaseOrder? PurchaseOrder { get; set; }
    public Product? Product { get; set; }
}
