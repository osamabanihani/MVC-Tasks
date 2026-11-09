using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeteriaCodeFirst.Data;
using CafeteriaCodeFirst.Models;

namespace CafeteriaCodeFirst.Controllers;

[Authorize]
public class SuppliersController : Controller
{
    private readonly CafeteriaDbContext _db;
    public SuppliersController(CafeteriaDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.Suppliers.AsNoTracking().OrderBy(s => s.Name).ToListAsync());

    [HttpGet]
    public IActionResult Create() => View("Form", new Supplier());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        if (!ModelState.IsValid) return View("Form", supplier);
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        return supplier == null ? NotFound() : View("Form", supplier);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Supplier supplier)
    {
        if (id != supplier.Id) return BadRequest();
        if (!ModelState.IsValid) return View("Form", supplier);
        var existing = await _db.Suppliers.FindAsync(id);
        if (existing == null) return NotFound();
        existing.Name = supplier.Name;
        existing.Email = supplier.Email;
        existing.Phone = supplier.Phone;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();
        try
        {
            _db.Suppliers.Remove(supplier);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException)
        {
            TempData["Notice"] = "Cannot delete a supplier that is used by products or orders.";
        }
        return RedirectToAction(nameof(Index));
    }
}
