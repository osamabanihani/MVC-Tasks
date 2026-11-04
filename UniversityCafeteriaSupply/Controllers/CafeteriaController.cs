using Microsoft.AspNetCore.Mvc;
namespace UniversityCafeteriaSupply.Controllers;

public class CafeteriaController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Products() => View();
    public IActionResult Suppliers() => View();
    public IActionResult SupplyStatus() => View();
    public IActionResult Error() => Content("Something went wrong. Please try again.");
}
