using System.ComponentModel.DataAnnotations;

namespace CafeteriaDatabaseFirst.Models;

public class LoginVm
{
    [Required]
    public string Username { get; set; } = string.Empty;

    [Required, DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
