using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeteriaDatabaseFirst.Data;

namespace CafeteriaDatabaseFirst.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly CafeteriaDbContext _db;
    public HomeController(CafeteriaDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        ViewBag.Categories = await _db.Categories.CountAsync();
        ViewBag.Suppliers = await _db.Suppliers.CountAsync();
        ViewBag.Products = await _db.Products.CountAsync();
        ViewBag.Orders = await _db.PurchaseOrders.CountAsync();
        return View();
    }

    [AllowAnonymous]
    public IActionResult Error() => View();
}
