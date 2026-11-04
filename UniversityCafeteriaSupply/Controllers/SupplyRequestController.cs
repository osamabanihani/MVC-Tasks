using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using UniversityCafeteriaSupply.Models;

namespace UniversityCafeteriaSupply.Controllers;

public class SupplyRequestController : Controller
{
    private const string DraftKey = "SupplyDraft";
    private const string PriorityKey = "DraftPriority";
    private static readonly List<ConfirmedSupplyRequest> ConfirmedRequests = new();
    private static readonly object RequestLock = new();
    private static int _nextId = 1;

    private List<SupplyItem> GetDraft()
    {
        string? json = HttpContext.Session.GetString(DraftKey);
        return string.IsNullOrWhiteSpace(json)
            ? new List<SupplyItem>()
            : JsonSerializer.Deserialize<List<SupplyItem>>(json) ?? new List<SupplyItem>();
    }

    private void SaveDraft(List<SupplyItem> items) =>
        HttpContext.Session.SetString(DraftKey, JsonSerializer.Serialize(items));

    private static bool ValidPriority(string? priority) =>
        priority is "Normal" or "Urgent" or "Emergency";

    // All calculations happen in this controller.
    private SupplyRequestSummary Calculate(List<SupplyItem> items)
    {
        string priority = HttpContext.Session.GetString(PriorityKey) ?? "Normal";
        decimal subtotal = items.Sum(item => item.Total);
        decimal fee = priority switch
        {
            "Urgent" => 5m,
            "Emergency" => 10m,
            _ => 2m
        };
        decimal discount = subtotal > 200m ? subtotal * 0.15m
            : subtotal > 100m ? subtotal * 0.10m : 0m;

        return new SupplyRequestSummary
        {
            Items = items,
            Priority = priority,
            Subtotal = subtotal,
            PreparationFee = fee,
            Discount = discount,
            FinalTotal = subtotal + fee - discount
        };
    }

    [HttpGet]
    public IActionResult Index()
    {
        ViewBag.Items = GetDraft();
        ViewBag.PreferredPriority = Request.Cookies["PreferredPriority"] ?? "Normal";
        ViewBag.CurrentPriority = HttpContext.Session.GetString(PriorityKey) ?? "Normal";
        return View(new SupplyItem { Priority = ViewBag.PreferredPriority });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult AddItem(SupplyItem item)
    {
        item.ProductName = item.ProductName?.Trim() ?? "";
        if (!ValidPriority(item.Priority))
            ModelState.AddModelError(nameof(item.Priority), "Choose a valid priority.");
        if (!ModelState.IsValid)
        {
            ViewBag.Items = GetDraft();
            ViewBag.PreferredPriority = Request.Cookies["PreferredPriority"] ?? "Normal";
            ViewBag.CurrentPriority = HttpContext.Session.GetString(PriorityKey) ?? "Normal";
            return View("Index", item);
        }

        List<SupplyItem> items = GetDraft();
        items.Add(item);
        SaveDraft(items);
        // The latest selected item priority is used for the request preparation fee.
        HttpContext.Session.SetString(PriorityKey, item.Priority);
        TempData["Message"] = "Product added to draft.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Review() => View(Calculate(GetDraft()));

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ConfirmRequest()
    {
        List<SupplyItem> items = GetDraft();
        if (items.Count == 0)
        {
            TempData["Error"] = "You cannot confirm an empty supply request.";
            return RedirectToAction(nameof(Review));
        }

        SupplyRequestSummary summary = Calculate(items);
        lock (RequestLock)
        {
            ConfirmedRequests.Add(new ConfirmedSupplyRequest
            {
                RequestId = _nextId++,
                Items = summary.Items.ToList(),
                Priority = summary.Priority,
                Subtotal = summary.Subtotal,
                PreparationFee = summary.PreparationFee,
                Discount = summary.Discount,
                FinalTotal = summary.FinalTotal,
                Status = "Confirmed"
            });
        }
        HttpContext.Session.Remove(DraftKey);
        HttpContext.Session.Remove(PriorityKey);
        TempData["Message"] = "Your request was confirmed successfully.";
        return RedirectToAction(nameof(MyRequests));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ClearDraft()
    {
        HttpContext.Session.Remove(DraftKey);
        HttpContext.Session.Remove(PriorityKey);
        TempData["Message"] = "Draft cleared.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult SavePreference(string priority)
    {
        if (!ValidPriority(priority))
        {
            TempData["Error"] = "Please choose a valid priority.";
            return RedirectToAction(nameof(Index));
        }
        Response.Cookies.Append("PreferredPriority", priority, new CookieOptions
        {
            Expires = DateTimeOffset.UtcNow.AddDays(7),
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax
        });
        TempData["Message"] = $"Your preferred request priority is {priority}.";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult MyRequests()
    {
        lock (RequestLock)
        {
            return View(ConfirmedRequests.OrderByDescending(r => r.RequestId).ToList());
        }
    }

    [HttpGet]
    public IActionResult About() => View();
}
