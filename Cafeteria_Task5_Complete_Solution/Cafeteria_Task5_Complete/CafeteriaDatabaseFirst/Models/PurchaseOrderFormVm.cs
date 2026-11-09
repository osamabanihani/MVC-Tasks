using System.ComponentModel.DataAnnotations;

namespace CafeteriaDatabaseFirst.Models;

public class PurchaseOrderFormVm
{
    [Range(1, int.MaxValue, ErrorMessage = "Choose a supplier.")]
    public int SupplierId { get; set; }

    public List<PurchaseOrderLineVm> Items { get; set; } = new() { new PurchaseOrderLineVm() };
}

public class PurchaseOrderLineVm
{
    [Range(1, int.MaxValue, ErrorMessage = "Choose a product.")]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be positive.")]
    public int Quantity { get; set; } = 1;
}
