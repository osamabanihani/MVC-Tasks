using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CafeteriaDbFirst.Data;
using CafeteriaDbFirst.Models;
namespace CafeteriaDbFirst.Controllers;
[Authorize]
public class CategoriesController : Controller
{
    private readonly CafeteriaDbContext _db;
    public CategoriesController(CafeteriaDbContext db) => _db = db;
    public IActionResult Index() => View(_db.Categories.OrderBy(c => c.Name).ToList());
    public IActionResult Create() => View("Form", new Category());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Category category)
    {
        if (!ModelState.IsValid) return View("Form", category);
        _db.Categories.Add(category);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        return category == null ? NotFound() : View("Form", category);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Category form)
    {
        if (id != form.Id) return BadRequest();
        if (!ModelState.IsValid) return View("Form", form);
        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();
        category.Name = form.Name;
        category.Description = form.Description;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var category = await _db.Categories.FindAsync(id);
        if (category == null) return NotFound();
        _db.Categories.Remove(category);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { TempData["Error"] = "Cannot delete a category used by products."; }
        return RedirectToAction(nameof(Index));
    }
}
