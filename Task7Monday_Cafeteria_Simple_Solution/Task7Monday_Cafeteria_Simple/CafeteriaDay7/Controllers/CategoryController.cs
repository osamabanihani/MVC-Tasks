using CafeteriaDay7.Data;
using CafeteriaDay7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaDay7.Controllers;

public class CategoryController : Controller
{
    private readonly CafeteriaDbContext _db;
    public CategoryController(CafeteriaDbContext db) => _db = db;

    // Show all records (including count of related records).
    public IActionResult Index()
    {
        var records = _db.Categories.Include(x => x.Products).AsNoTracking().ToList();
        return View(records);
    }

    [HttpGet]
    public IActionResult Create() => View(new Category());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category model)
    {
        _db.Categories.Add(model);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var record = _db.Categories.Find(id);
        return record == null ? NotFound() : View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Category model)
    {
        var record = _db.Categories.Find(model.Id);
        if (record == null) return NotFound();
        // Edit only regular fields. No changes to the Id.
        record.Name = model.Name;
        record.Description = model.Description;
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var record = _db.Categories.Include(x => x.Products)
            .AsNoTracking().FirstOrDefault(x => x.Id == id);
        return record == null ? NotFound() : View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var record = _db.Categories.Find(id);
        if (record == null) return NotFound();

        // Prevent deleting a parent that still has related records.
        if (_db.Products.Any(x => x.CategoryId == id))
        {
            TempData["Error"] = "Cannot delete this category because it has related products.";
            return RedirectToAction(nameof(Index));
        }

        _db.Categories.Remove(record);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
