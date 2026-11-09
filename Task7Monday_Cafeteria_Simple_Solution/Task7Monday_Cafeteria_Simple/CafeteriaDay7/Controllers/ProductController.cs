using CafeteriaDay7.Data;
using CafeteriaDay7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaDay7.Controllers;

public class ProductController : Controller
{
    private readonly CafeteriaDbContext _db;
    public ProductController(CafeteriaDbContext db) => _db = db;

    public IActionResult Index()
    {
        var records = _db.Products.Include(p => p.Category).Include(p => p.Supplier)
            .AsNoTracking().ToList();
        return View(records);
    }

    private void LoadLists()
    {
        ViewBag.Categories = new SelectList(_db.Categories.ToList(), "Id", "Name");
        ViewBag.Suppliers = new SelectList(_db.Suppliers.ToList(), "Id", "Name");
    }

    [HttpGet]
    public IActionResult Create()
    {
        LoadLists();
        return View(new Product());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product model)
    {
        _db.Products.Add(model);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var record = _db.Products.Find(id);
        if (record == null) return NotFound();
        LoadLists();
        return View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Product model)
    {
        var record = _db.Products.Find(model.Id);
        if (record == null) return NotFound();

        record.Name = model.Name;
        record.Description = model.Description;
        record.Price = model.Price;
        record.Quantity = model.Quantity;
        record.CategoryId = model.CategoryId;
        record.SupplierId = model.SupplierId;
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var record = _db.Products.Include(p => p.Category).Include(p => p.Supplier)
            .AsNoTracking().FirstOrDefault(p => p.Id == id);
        return record == null ? NotFound() : View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var record = _db.Products.Find(id);
        if (record == null) return NotFound();
        _db.Products.Remove(record);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
