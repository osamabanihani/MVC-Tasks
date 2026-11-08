using Microsoft.AspNetCore.Mvc;
using UniversityCafeteria.Data;
using UniversityCafeteria.Models;

namespace UniversityCafeteria.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;

    public CategoryController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET: /Category/Index
    public IActionResult Index()
    {
        return View(_db.Categories.ToList());
    }

    // GET: /Category/Create
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    // POST: /Category/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Category category)
    {
        if (!ModelState.IsValid)
            return View(category);

        _db.Categories.Add(category);
        _db.SaveChanges();
        return RedirectToAction("Index");
    }
}
