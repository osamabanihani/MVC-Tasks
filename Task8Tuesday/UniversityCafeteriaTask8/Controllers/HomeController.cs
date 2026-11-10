using Microsoft.AspNetCore.Mvc;

namespace UniversityCafeteriaTask8.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();
    public IActionResult Error() => View();
}
