using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaTask8.Data;
using UniversityCafeteriaTask8.Models;
using UniversityCafeteriaTask8.ViewModels;

namespace UniversityCafeteriaTask8.Controllers;

[Authorize]
public class SupplyRequestController : Controller
{
    private readonly AppDbContext _db;
    private readonly UserManager<ApplicationUser> _users;

    public SupplyRequestController(AppDbContext db, UserManager<ApplicationUser> users)
    {
        _db = db;
        _users = users;
    }

    private bool IsAdmin => User.IsInRole("Admin") || User.IsInRole("SuperAdmin");
    private bool CanOpen(SupplyRequest request) => IsAdmin || request.UserId == _users.GetUserId(User);

    public async Task<IActionResult> Index()
    {
        var query = _db.SupplyRequests.Include(r => r.User).AsQueryable();

        // Normal users see their own requests only.
        if (!IsAdmin)
        {
            var userId = _users.GetUserId(User);
            query = query.Where(r => r.UserId == userId);
        }

        return View(await query.OrderByDescending(r => r.Id).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var request = await _db.SupplyRequests
            .Include(r => r.User)
            .Include(r => r.Items).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null) return NotFound();
        if (!CanOpen(request)) return Forbid();
        return View(request);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Products = await _db.Products.OrderBy(p => p.Name).ToListAsync();
        return View(new CreateRequestViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateRequestViewModel model)
    {
        // A request may have 1, 2, or 3 different products.
        var chosen = model.Items.Where(i => i.ProductId > 0).ToList();

        if (chosen.Count == 0 || chosen.Any(i => i.Quantity <= 0) ||
            model.Items.Any(i => i.ProductId == 0 && i.Quantity > 0))
        {
            ModelState.AddModelError("", "Choose at least one product with quantity greater than zero.");
        }

        if (chosen.GroupBy(i => i.ProductId).Any(g => g.Count() > 1))
            ModelState.AddModelError("", "Choose each product only once.");

        var request = new SupplyRequest
        {
            UserId = _users.GetUserId(User)!,
            RequestDate = DateTime.Now,
            Status = "Pending"
        };

        if (ModelState.IsValid)
        {
            foreach (var line in chosen)
            {
                var product = await _db.Products.FindAsync(line.ProductId);
                if (product == null)
                {
                    ModelState.AddModelError("", "One of the products was not found.");
                    break;
                }

                request.Items.Add(new RequestItem
                {
                    ProductId = product.Id,
                    Quantity = line.Quantity,
                    Price = product.Price
                });
                request.TotalPrice += line.Quantity * product.Price;
            }
        }

        if (!ModelState.IsValid)
        {
            ViewBag.Products = await _db.Products.OrderBy(p => p.Name).ToListAsync();
            return View(model);
        }

        _db.SupplyRequests.Add(request);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var request = await _db.SupplyRequests.Include(r => r.Items)
            .ThenInclude(i => i.Product).FirstOrDefaultAsync(r => r.Id == id);

        if (request == null) return NotFound();
        if (!CanOpen(request)) return Forbid();
        if (request.Status != "Pending")
        {
            TempData["Error"] = "Only pending requests can be edited.";
            return RedirectToAction("Index");
        }

        var model = new EditRequestViewModel
        {
            Id = request.Id,
            Items = request.Items.Select(i => new EditRequestLine
            {
                Id = i.Id,
                ProductName = i.Product?.Name,
                Quantity = i.Quantity
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditRequestViewModel model)
    {
        if (id != model.Id) return BadRequest();

        var request = await _db.SupplyRequests.Include(r => r.Items)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (request == null) return NotFound();
        if (!CanOpen(request)) return Forbid();
        if (request.Status != "Pending") return BadRequest("Only pending requests can be edited.");

        if (model.Items.Count != request.Items.Count ||
            model.Items.Any(i => i.Quantity <= 0) ||
            request.Items.Any(i => model.Items.Count(x => x.Id == i.Id) != 1))
        {
            TempData["Error"] = "Enter a valid quantity for each request item.";
            return RedirectToAction("Edit", new { id });
        }

        foreach (var item in request.Items)
            item.Quantity = model.Items.First(i => i.Id == item.Id).Quantity;

        request.TotalPrice = request.Items.Sum(i => i.Quantity * i.Price);
        await _db.SaveChangesAsync();
        return RedirectToAction("Details", new { id });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var request = await _db.SupplyRequests.FindAsync(id);
        if (request == null) return NotFound();
        if (!CanOpen(request)) return Forbid();

        if (request.Status != "Pending")
        {
            TempData["Error"] = "Only pending requests can be deleted.";
            return RedirectToAction("Index");
        }

        _db.SupplyRequests.Remove(request);
        await _db.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Approve(int id)
    {
        var request = await _db.SupplyRequests.FindAsync(id);
        if (request == null) return NotFound();
        if (request.Status == "Pending")
        {
            request.Status = "Approved";
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Details", new { id });
    }

    [HttpPost]
    [Authorize(Roles = "Admin,SuperAdmin")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Reject(int id)
    {
        var request = await _db.SupplyRequests.FindAsync(id);
        if (request == null) return NotFound();
        if (request.Status == "Pending")
        {
            request.Status = "Rejected";
            await _db.SaveChangesAsync();
        }
        return RedirectToAction("Details", new { id });
    }
}
