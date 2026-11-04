using System.ComponentModel.DataAnnotations;

namespace UniversityCafeteriaSupply.Models;

public class SupplyItem
{
    [Required(ErrorMessage = "Product name is required")]
    public string ProductName { get; set; } = "";

    [Required]
    public string Category { get; set; } = "Food";

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero")]
    public int Quantity { get; set; } = 1;

    [Range(typeof(decimal), "0.01", "99999999", ErrorMessage = "Unit price must be greater than zero")]
    public decimal UnitPrice { get; set; } = 1;

    [Required]
    public string Priority { get; set; } = "Normal";

    public decimal Total => Quantity * UnitPrice;
}

public class SupplyRequestSummary
{
    public List<SupplyItem> Items { get; set; } = new();
    public string Priority { get; set; } = "Normal";
    public decimal Subtotal { get; set; }
    public decimal PreparationFee { get; set; }
    public decimal Discount { get; set; }
    public decimal FinalTotal { get; set; }
}

public class ConfirmedSupplyRequest : SupplyRequestSummary
{
    public int RequestId { get; set; }
    public string Status { get; set; } = "Confirmed";
}
