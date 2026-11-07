using Microsoft.AspNetCore.Mvc;
namespace AcademyGraduation.Controllers;
public class HomeController : Controller
{
    public IActionResult About() => View();
    public IActionResult Error() => Content("An error occurred. Check the SQL Server connection and try again.");
}
