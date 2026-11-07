using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using CafeteriaDbFirst.Data;
using CafeteriaDbFirst.Models;
using CafeteriaDbFirst.ViewModels;
namespace CafeteriaDbFirst.Controllers;
[Authorize]
public class OrdersController : Controller
{
    private readonly CafeteriaDbContext _db;
    public OrdersController(CafeteriaDbContext db) => _db = db;
    public IActionResult Index() => View(_db.PurchaseOrders.Include(x => x.Supplier)
        .Include(x => x.Items).OrderByDescending(x => x.OrderDate).ToList());
    private async Task LoadLists()
    {
        ViewBag.Suppliers = await _db.Suppliers.OrderBy(s => s.Name).ToListAsync();
        ViewBag.Products = await _db.Products.OrderBy(p => p.Name).ToListAsync();
    }
    public async Task<IActionResult> Create()
    {
        await LoadLists();
        return View(new OrderFormVm { Items = new() { new(), new() } });
    }
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(OrderFormVm form)
    {
        var supplier = await _db.Suppliers.FindAsync(form.SupplierId);
        if (supplier == null) ModelState.AddModelError(nameof(form.SupplierId), "Choose a valid supplier.");
        var selected = form.Items.Where(x => x.ProductId > 0 || x.Quantity.GetValueOrDefault() > 0).ToList();
        if (selected.Count == 0) ModelState.AddModelError("", "Select at least one product.");
        var available = await _db.Products.Where(p => p.SupplierId == form.SupplierId)
            .ToDictionaryAsync(p => p.Id);
        foreach (var item in selected)
        {
            if (item.ProductId <= 0 || item.Quantity.GetValueOrDefault() <= 0 || !available.ContainsKey(item.ProductId))
                ModelState.AddModelError("", "Every item needs a valid product from this supplier and quantity greater than 0.");
        }
        if (!ModelState.IsValid)
        {
            await LoadLists();
            if (form.Items.Count == 0) form.Items.Add(new());
            return View(form);
        }
        var order = new PurchaseOrder
        {
            SupplierId = form.SupplierId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            Items = selected.Select(x => new PurchaseOrderItem
            {
                ProductId = x.ProductId,
                Quantity = x.Quantity.GetValueOrDefault(),
                UnitPrice = available[x.ProductId].Price // snapshot the price when ordering
            }).ToList()
        };
        _db.PurchaseOrders.Add(order);
        await _db.SaveChangesAsync();
        return RedirectToAction(nameof(Details), new { id = order.Id });
    }
    public async Task<IActionResult> Details(int id)
    {
        var order = await _db.PurchaseOrders.Include(x => x.Supplier)
            .Include(x => x.Items).ThenInclude(x => x.Product)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (order == null) return NotFound();
        ViewBag.Total = order.Items.Sum(x => x.Quantity * x.UnitPrice);
        return View(order);
    }
    // Receive supplies ONCE: receiving a purchase order increases inventory.
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Receive(int id)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var order = await _db.PurchaseOrders.Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (order == null) return NotFound();
        if (order.Status == "Pending")
        {
            foreach (var item in order.Items)
            {
                var product = await _db.Products.FindAsync(item.ProductId);
                if (product == null) return NotFound();
                product.Quantity += item.Quantity;
            }
            order.Status = "Received";
            await _db.SaveChangesAsync();
            await transaction.CommitAsync();
            TempData["Message"] = "Order received. Inventory updated.";
        }
        else
        {
            TempData["Error"] = "This order was already received; stock was not changed again.";
        }
        return RedirectToAction(nameof(Details), new { id });
    }
}
