using System.Text.Json;
using AcademyGraduation.Data;
using AcademyGraduation.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AcademyGraduation.Controllers;
public class SupplyController : Controller
{
    private readonly GraduationDbContext _db;
    public SupplyController(GraduationDbContext db) => _db = db;
    private string GetMode(string? requested = null)
    {
        string mode = requested == "list" || requested == "keys" ? requested :
                      Request.Cookies["StorageMode"] == "list" ? "list" : "keys";
        Response.Cookies.Append("StorageMode", mode, new CookieOptions
        { HttpOnly = true, IsEssential = true, SameSite = SameSiteMode.Lax });
        return mode;
    }
    private List<SelectedOffer> ReadSelections(string mode)
    {
        // Phase 1: each category has its own session key.
        if (mode == "keys")
            return new[] { 1, 2, 3 }.Select(id => new SelectedOffer
                { CategoryId = id, OfferId = HttpContext.Session.GetInt32($"Offer_{id}") ?? 0 })
                .Where(s => s.OfferId > 0).ToList();

        // Phase 2: ONE list stored as JSON in ONE session key.
        string? json = HttpContext.Session.GetString("SelectedOffers");
        return json == null ? new List<SelectedOffer>() :
               JsonSerializer.Deserialize<List<SelectedOffer>>(json) ?? new();
    }
    public IActionResult Index(string? mode)
    {
        mode = GetMode(mode);
        ViewBag.Mode = mode;
        ViewBag.SelectedIds = ReadSelections(mode).Select(x => x.OfferId).ToList();
        return View(_db.Categories.Include(x => x.Suppliers)
            .ThenInclude(x => x.Offer).OrderBy(x => x.Id).ToList());
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Select(int categoryId, int offerId, string mode)
    {
        mode = GetMode(mode);
        bool valid = _db.Offers.Any(x => x.Id == offerId && x.Supplier.CategoryId == categoryId);
        if (!valid) { TempData["Error"] = "Choose a valid offer."; return RedirectToAction(nameof(Index), new { mode }); }
        if (mode == "keys")
        {
            HttpContext.Session.SetInt32($"Offer_{categoryId}", offerId);
        }
        else
        {
            var list = ReadSelections("list");
            list.RemoveAll(x => x.CategoryId == categoryId);
            list.Add(new SelectedOffer { CategoryId = categoryId, OfferId = offerId });
            HttpContext.Session.SetString("SelectedOffers", JsonSerializer.Serialize(list));
        }
        TempData["Message"] = "Offer saved.";
        return RedirectToAction(nameof(Index), new { mode });
    }
    public IActionResult Review(string? mode)
    {
        mode = GetMode(mode);
        var selections = ReadSelections(mode);
        var categories = _db.Categories.OrderBy(x => x.Id).Select(x => x.Id).ToList();
        // Exactly one selected offer per category, or redirect to the selection page.
        if (selections.Count != 3 || categories.Any(id => selections.Count(x => x.CategoryId == id) != 1))
        {
            TempData["Error"] = "Select one offer from EACH of the 3 categories before review.";
            return RedirectToAction(nameof(Index), new { mode });
        }
        var ids = selections.Select(s => s.OfferId).ToList();
        var offers = _db.Offers.Include(x => x.Supplier).ThenInclude(x => x.Category)
            .Where(x => ids.Contains(x.Id)).ToList();
        if (offers.Count != 3 || offers.Any(o => !selections.Any(s => s.CategoryId == o.Supplier.CategoryId && s.OfferId == o.Id)))
            return RedirectToAction(nameof(Index), new { mode });
        var rows = offers.OrderBy(o => o.Supplier.CategoryId).Select(o => new ReviewRow
        {
            Category = o.Supplier.Category.Name, Supplier = o.Supplier.Name, Offer = o.Name,
            Quantity = o.Quantity, UnitPrice = o.UnitPrice, Total = o.Quantity * o.UnitPrice
        }).ToList();
        decimal total = rows.Sum(x => x.Total);
        decimal discount = total > 500m ? .15m : total > 300m ? .10m : 0m;
        ViewBag.Total = total;
        ViewBag.DiscountPercent = discount * 100;
        ViewBag.DiscountAmount = total * discount;
        ViewBag.FinalTotal = total - (total * discount);
        ViewBag.Mode = mode;
        return View(rows);
    }
}
