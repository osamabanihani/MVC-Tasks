namespace AcademyGraduation.Models;
public class SelectedOffer
{
    public int CategoryId { get; set; }
    public int OfferId { get; set; }
}
public class ReviewRow
{
    public string Category { get; set; } = "";
    public string Supplier { get; set; } = "";
    public string Offer { get; set; } = "";
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal Total { get; set; }
}
