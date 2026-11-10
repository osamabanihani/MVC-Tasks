using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaTask8.Data;
using UniversityCafeteriaTask8.Models;

namespace UniversityCafeteriaTask8.Controllers;

[Authorize]
public class ProductController : Controller
{
    private readonly AppDbContext _db;

    public ProductController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index()
    {
        return View(await _db.Products.OrderBy(p => p.Id).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public IActionResult Create() => View();

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid) return View(product);

        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();
        return View(product);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        if (!ModelState.IsValid) return View(product);

        var oldProduct = await _db.Products.FindAsync(id);
        if (oldProduct == null) return NotFound();

        oldProduct.Name = product.Name;
        oldProduct.Description = product.Description;
        oldProduct.Price = product.Price;
        oldProduct.Quantity = product.Quantity;
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();

        if (await _db.RequestItems.AnyAsync(i => i.ProductId == id))
        {
            TempData["Error"] = "This product is in a supply request and cannot be deleted.";
            return RedirectToAction("Index");
        }

        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
