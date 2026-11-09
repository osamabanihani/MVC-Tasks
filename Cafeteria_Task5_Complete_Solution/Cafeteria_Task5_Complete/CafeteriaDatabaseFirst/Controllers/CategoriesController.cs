using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeteriaDatabaseFirst.Data;
using CafeteriaDatabaseFirst.Models;

namespace CafeteriaDatabaseFirst.Controllers;

[Authorize]
public class CategoriesController : Controller
{
    private readonly CafeteriaDbContext _db;
    public CategoriesController(CafeteriaDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Categories.AsNoTracking().OrderBy(c => c.Name).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View("Form", new Category());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid) return View("Form", category);
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        return category == null ? NotFound() : View("Form", category);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category category)
    {
        if (id != category.Id) return BadRequest();
        if (!ModelState.IsValid) return View("Form", category);
        var existing = await _db.Categories.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Name = category.Name;
        existing.Description = category.Description;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();
        try
        {
            _db.Categories.Remove(category);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Notice"] = "Cannot delete a category that is used by products.";
        }
        return RedirectToAction(nameof(Index));
    }
}
