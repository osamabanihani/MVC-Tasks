using Microsoft.AspNetCore.Mvc;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers;

public class SupplierController : Controller
{
    private readonly ApplicationDbContext _db;

    public SupplierController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /Supplier/Index
    public IActionResult Index()
    {
        return View(_db.Suppliers.ToList());
    }

    // GET: /Supplier/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Supplier/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Supplier supplier)
    {
        if (!ModelState.IsValid)
            return View(supplier);

        _db.Suppliers.Add(supplier);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}
