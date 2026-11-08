using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UniversityCafeteria.Models;

public class Product
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    public string Name { get; set; } = string.Empty;

    [Column(TypeName = "decimal(18,2)")]
    [Range(typeof(decimal), "0.01", "9999999.99", ErrorMessage = "Price must be greater than zero.")]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Quantity cannot be negative.")]
    public int Quantity { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Choose a category.")]
    public int CategoryId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Choose a supplier.")]
    public int SupplierId { get; set; }

    // Navigation properties allow us to show the category and supplier names.
    public Category? Category { get; set; }
    public Supplier? Supplier { get; set; }
}
