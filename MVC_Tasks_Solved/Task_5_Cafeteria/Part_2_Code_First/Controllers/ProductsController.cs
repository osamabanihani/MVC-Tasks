using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CafeteriaCodeFirst.Data;
using CafeteriaCodeFirst.Models;
namespace CafeteriaCodeFirst.Controllers;
[Authorize]
public class ProductsController : Controller
{
    private readonly CafeteriaDbContext _db;
    public ProductsController(CafeteriaDbContext db) => _db = db;
    public IActionResult Index() => View(_db.Products.Include(p => p.Category)
        .Include(p => p.Supplier).OrderBy(p => p.Name).ToList());
    private void LoadDropdowns(Product p)
    {
        ViewBag.Categories = new SelectList(_db.Categories.OrderBy(x => x.Name).ToList(), "Id", "Name", p.CategoryId);
        ViewBag.Suppliers = new SelectList(_db.Suppliers.OrderBy(x => x.Name).ToList(), "Id", "Name", p.SupplierId);
    }
    public IActionResult Create()
    {
        var p = new Product(); LoadDropdowns(p); return View("Form", p);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Product product)
    {
        if (!ModelState.IsValid) { LoadDropdowns(product); return View("Form", product); }
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return NotFound();
        LoadDropdowns(p); return View("Form", p);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Product form)
    {
        if (id != form.Id) return BadRequest();
        if (!ModelState.IsValid) { LoadDropdowns(form); return View("Form", form); }
        var product = await _db.Products.FindAsync(id);
        if (product == null) return NotFound();
        product.Name = form.Name;
        product.Price = form.Price;
        product.Quantity = form.Quantity;
        product.CategoryId = form.CategoryId;
        product.SupplierId = form.SupplierId;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var p = await _db.Products.FindAsync(id);
        if (p == null) return NotFound();
        _db.Products.Remove(p);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { TempData["Error"] = "Cannot delete a product included in an order."; }
        return RedirectToAction(nameof(Index));
    }
}
