using CafeteriaDay7.Data;
using CafeteriaDay7.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaDay7.Controllers;

public class OrderController : Controller
{
    private readonly CafeteriaDbContext _db;
    public OrderController(CafeteriaDbContext db) => _db = db;

    public IActionResult Index()
    {
        var records = _db.Orders.Include(o => o.Employee).AsNoTracking().ToList();
        return View(records);
    }

    private void LoadEmployees()
    {
        ViewBag.Employees = new SelectList(_db.Employees.ToList(), "Id", "FullName");
    }

    [HttpGet]
    public IActionResult Create()
    {
        LoadEmployees();
        return View(new Order());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Order model)
    {
        _db.Orders.Add(model);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var record = _db.Orders.Find(id);
        if (record == null) return NotFound();
        LoadEmployees();
        return View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(Order model)
    {
        var record = _db.Orders.Find(model.Id);
        if (record == null) return NotFound();

        record.CustomerName = model.CustomerName;
        record.OrderDate = model.OrderDate;
        record.TotalPrice = model.TotalPrice;
        record.Status = model.Status;
        record.EmployeeId = model.EmployeeId;
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Details(int id)
    {
        var record = _db.Orders.Include(o => o.Employee)
            .AsNoTracking().FirstOrDefault(o => o.Id == id);
        return record == null ? NotFound() : View(record);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        var record = _db.Orders.Find(id);
        if (record == null) return NotFound();
        _db.Orders.Remove(record);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
