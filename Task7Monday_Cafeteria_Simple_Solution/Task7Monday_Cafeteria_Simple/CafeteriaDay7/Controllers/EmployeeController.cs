using CafeteriaDay7.Data;
using CafeteriaDay7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaDay7.Controllers;

public class EmployeeController : Controller
{
    private readonly CafeteriaDbContext _db;
    public EmployeeController(CafeteriaDbContext db) => _db = db;

    // Show all records (including count of related records).
    public IActionResult Index()
    {
        var records = _db.Employees.Include(x => x.Orders).AsNoTracking().ToList();
        return View(records);
    }

    [HttpGet]
    public IActionResult Create() => View(new Employee());

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Employee model)
    {
        _db.Employees.Add(model);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var record = _db.Employees.Find(id);
        return record == null ? NotFound() : View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Employee model)
    {
        var record = _db.Employees.Find(model.Id);
        if (record == null) return NotFound();
        // Edit only regular fields. No changes to the Id.
        record.FullName = model.FullName;
        record.Phone = model.Phone;
        record.Email = model.Email;
        record.Position = model.Position;
        record.Salary = model.Salary;
        record.HireDate = model.HireDate;
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var record = _db.Employees.Include(x => x.Orders)
            .AsNoTracking().FirstOrDefault(x => x.Id == id);
        return record == null ? NotFound() : View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var record = _db.Employees.Find(id);
        if (record == null) return NotFound();

        // Prevent deleting a parent that still has related records.
        if (_db.Orders.Any(x => x.EmployeeId == id))
        {
            TempData["Error"] = "Cannot delete this employee because it has related orders.";
            return RedirectToAction(nameof(Index));
        }

        _db.Employees.Remove(record);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
