using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CafeteriaCodeFirst.Data;
using CafeteriaCodeFirst.Models;
namespace CafeteriaCodeFirst.Controllers;
[Authorize]
public class SuppliersController : Controller
{
    private readonly CafeteriaDbContext _db;
    public SuppliersController(CafeteriaDbContext db) => _db = db;
    public IActionResult Index() => View(_db.Suppliers.OrderBy(s => s.Name).ToList());
    public IActionResult Create() => View("Form", new Supplier());
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Supplier supplier)
    {
        if (!ModelState.IsValid) return View("Form", supplier);
        _db.Suppliers.Add(supplier);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Edit(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        return supplier == null ? NotFound() : View("Form", supplier);
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Supplier form)
    {
        if (id != form.Id) return BadRequest();
        if (!ModelState.IsValid) return View("Form", form);
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();
        supplier.Name = form.Name;
        supplier.Email = form.Email;
        supplier.Phone = form.Phone;
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var supplier = await _db.Suppliers.FindAsync(id);
        if (supplier == null) return NotFound();
        _db.Suppliers.Remove(supplier);
        try { await _db.SaveChangesAsync(); }
        catch (DbUpdateException) { TempData["Error"] = "Cannot delete a supplier used by products or orders."; }
        return RedirectToAction(nameof(Index));
    }
}
