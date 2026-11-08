using System.ComponentModel.DataAnnotations;

namespace UniversityCafeteria.Models;

public class Supplier
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Supplier name is required.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required.")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Enter a valid email address.")]
    public string Email { get; set; } = string.Empty;
}
