using System.ComponentModel.DataAnnotations;

namespace UniversityCafeteriaTask8.Models;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = "";

    public string? Description { get; set; }

    [Range(0, 1000000)]
    public decimal Price { get; set; }

    [Range(0, 1000000)]
    public int Quantity { get; set; }
}
