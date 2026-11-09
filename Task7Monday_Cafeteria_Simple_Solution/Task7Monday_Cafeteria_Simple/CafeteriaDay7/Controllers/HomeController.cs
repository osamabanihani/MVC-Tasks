using CafeteriaDay7.Data;
using Microsoft.AspNetCore.Mvc;

namespace CafeteriaDay7.Controllers;

public class HomeController : Controller
{
    private readonly CafeteriaDbContext _db;

    public HomeController(CafeteriaDbContext db) => _db = db;

    public IActionResult Index()
    {
        ViewBag.Categories = _db.Categories.Count();
        ViewBag.Suppliers = _db.Suppliers.Count();
        ViewBag.Products = _db.Products.Count();
        ViewBag.Employees = _db.Employees.Count();
        ViewBag.Orders = _db.Orders.Count();
        return View();
    }

    public IActionResult Error() => Content("An error occurred.");
}
