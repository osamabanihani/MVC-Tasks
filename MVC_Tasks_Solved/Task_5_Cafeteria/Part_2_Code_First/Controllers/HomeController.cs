using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CafeteriaCodeFirst.Data;
namespace CafeteriaCodeFirst.Controllers;
[Authorize]
public class HomeController : Controller
{
    private readonly CafeteriaDbContext _db;
    public HomeController(CafeteriaDbContext db) => _db = db;
    public IActionResult Index()
    {
        ViewBag.Categories = _db.Categories.Count();
        ViewBag.Suppliers = _db.Suppliers.Count();
        ViewBag.Products = _db.Products.Count();
        ViewBag.Orders = _db.PurchaseOrders.Count();
        return View();
    }
    [AllowAnonymous] public IActionResult Error() => Content("An error occurred. Check your SQL Server and database setup.");
}
