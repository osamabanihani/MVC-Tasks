using System.ComponentModel.DataAnnotations;
namespace CafeteriaCodeFirst.ViewModels;
public class OrderFormVm
{
    [Range(1, int.MaxValue, ErrorMessage = "Select a supplier.")]
    public int SupplierId { get; set; }
    public List<OrderItemInput> Items { get; set; } = new();
}
public class OrderItemInput
{
    public int ProductId { get; set; }
    public int? Quantity { get; set; }
}
