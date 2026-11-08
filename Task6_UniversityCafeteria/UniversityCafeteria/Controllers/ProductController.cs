using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers;

public class ProductController : Controller
{
    private readonly ApplicationDbContext _db;

    public ProductController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /Product/Index
    public IActionResult Index()
    {
        var products = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Supplier)
            .ToList();

        return View(products);
    }

    // GET: /Product/Create
    [HttpGet]
    public IActionResult Create()
    {
        LoadDropdowns();
        return View();
    }

    // POST: /Product/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if (!ModelState.IsValid)
        {
            LoadDropdowns();
            return View(product);
        }

        _db.Products.Add(product);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }

    // Fill the two dropdown lists on the product form.
    private void LoadDropdowns()
    {
        ViewBag.Categories = new SelectList(_db.Categories.OrderBy(c => c.Name).ToList(), "Id", "Name");
        ViewBag.Suppliers = new SelectList(_db.Suppliers.OrderBy(s => s.Name).ToList(), "Id", "Name");
    }
}
