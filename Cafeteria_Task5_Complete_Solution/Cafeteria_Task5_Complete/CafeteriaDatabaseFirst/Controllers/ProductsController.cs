using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeteriaDatabaseFirst.Data;
using CafeteriaDatabaseFirst.Models;

namespace CafeteriaDatabaseFirst.Controllers;

[Authorize]
public class ProductsController : Controller
{
    private readonly CafeteriaDbContext _db;
    public ProductsController(CafeteriaDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Products.AsNoTracking()
        .Include(p => p.Category).Include(p => p.Supplier)
        .OrderBy(p => p.Name).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products.AsNoTracking().Include(p => p.Category)
            .Include(p => p.Supplier).FirstOrDefaultAsync(p => p.Id == id);
        return product == null ? NotFound() : View(product);
    }

    private async Task LoadListsAsync()
    {
        ViewBag.Categories = new SelectList(await _db.Categories.OrderBy(c => c.Name).ToListAsync(), "Id", "Name");
        ViewBag.Suppliers = new SelectList(await _db.Suppliers.OrderBy(s => s.Name).ToListAsync(), "Id", "Name");
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadListsAsync();
        return View("Form", new Product());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        // Navigation fields are populated by EF Core, not submitted by the form.
        ModelState.Remove(nameof(Product.Category));
        ModelState.Remove(nameof(Product.Supplier));
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return View("Form", product);
        }
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();
        await LoadListsAsync();
        return View("Form", product);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product product)
    {
        if (id != product.Id) return BadRequest();
        ModelState.Remove(nameof(Product.Category));
        ModelState.Remove(nameof(Product.Supplier));
        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return View("Form", product);
        }
        var existing = await _db.Products.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Name = product.Name;
        existing.Price = product.Price;
        existing.Quantity = product.Quantity;
        existing.CategoryId = product.CategoryId;
        existing.SupplierId = product.SupplierId;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();
        try
        {
            _db.Products.Remove(product);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Notice"] = "Cannot delete a product that appears in a purchase order.";
        }
        return RedirectToAction(nameof(Index));
    }
}
