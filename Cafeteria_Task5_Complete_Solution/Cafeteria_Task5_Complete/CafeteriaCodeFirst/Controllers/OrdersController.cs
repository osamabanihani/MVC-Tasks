using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CafeteriaCodeFirst.Data;
using CafeteriaCodeFirst.Models;

namespace CafeteriaCodeFirst.Controllers;

[Authorize]
public class OrdersController : Controller
{
    private readonly CafeteriaDbContext _db;
    public OrdersController(CafeteriaDbContext db) => _db = db;

    public async Task<IActionResult> Index() => View(await _db.PurchaseOrders.AsNoTracking()
        .Include(o => o.Supplier).Include(o => o.PurchaseOrderItems)
        .OrderByDescending(o => o.OrderDate).ToListAsync());

    public async Task<IActionResult> Details(int id)
    {
        var order = await _db.PurchaseOrders.AsNoTracking()
            .Include(o => o.Supplier)
            .Include(o => o.PurchaseOrderItems).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        ViewBag.Total = order.PurchaseOrderItems.Sum(i => i.Quantity * i.UnitPrice);
        return View(order);
    }

    private async Task LoadListsAsync()
    {
        ViewBag.Suppliers = await _db.Suppliers.AsNoTracking().OrderBy(s => s.Name).ToListAsync();
        ViewBag.Products = await _db.Products.AsNoTracking().OrderBy(p => p.Name).ToListAsync();
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        await LoadListsAsync();
        return View(new PurchaseOrderFormVm());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(PurchaseOrderFormVm model)
    {
        if (!await _db.Suppliers.AnyAsync(s => s.Id == model.SupplierId))
            ModelState.AddModelError(nameof(model.SupplierId), "Select an existing supplier.");

        if (model.Items == null || model.Items.Count == 0)
        {
            ModelState.AddModelError("Items", "Add at least one product.");
            model.Items = new List<PurchaseOrderLineVm> { new() };
        }

        var ids = model.Items.Select(i => i.ProductId).ToList();
        if (ids.Distinct().Count() != ids.Count)
            ModelState.AddModelError("Items", "Each product can be added only once per order.");

        var products = await _db.Products.AsNoTracking()
            .Where(p => p.SupplierId == model.SupplierId && ids.Contains(p.Id))
            .ToDictionaryAsync(p => p.Id);

        if (products.Count != ids.Distinct().Count())
            ModelState.AddModelError("Items", "All selected products must belong to the chosen supplier.");

        if (!ModelState.IsValid)
        {
            await LoadListsAsync();
            return View(model);
        }

        var order = new PurchaseOrder
        {
            SupplierId = model.SupplierId,
            OrderDate = DateTime.UtcNow,
            Status = "Pending",
            PurchaseOrderItems = model.Items.Select(i => new PurchaseOrderItem
            {
                ProductId = i.ProductId,
                Quantity = i.Quantity,
                UnitPrice = products[i.ProductId].Price
            }).ToList()
        };
        _db.PurchaseOrders.Add(order);
        await _db.SaveChangesAsync(); // Stock is NOT changed for Pending orders.
        TempData["Notice"] = "Pending purchase order created successfully.";
        return RedirectToAction(nameof(Details), new { id = order.Id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Receive(int id)
    {
        // Serializable transaction helps prevent two simultaneous Receive requests
        // from increasing stock twice for the same order.
        await using var tx = await _db.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        var order = await _db.PurchaseOrders
            .Include(o => o.PurchaseOrderItems).ThenInclude(i => i.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();

        if (order.Status != "Pending")
        {
            TempData["Notice"] = "This order was already received.";
            return RedirectToAction(nameof(Details), new { id });
        }

        foreach (var item in order.PurchaseOrderItems)
        {
            if (item.Product == null) return BadRequest("Product not found.");
            item.Product.Quantity = checked(item.Product.Quantity + item.Quantity);
        }
        order.Status = "Received";
        await _db.SaveChangesAsync();
        await tx.CommitAsync();
        TempData["Notice"] = "Order received. Stock updated once.";
        return RedirectToAction(nameof(Details), new { id });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var order = await _db.PurchaseOrders.Include(o => o.PurchaseOrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);
        if (order == null) return NotFound();
        if (order.Status != "Pending")
        {
            TempData["Notice"] = "Received orders cannot be deleted.";
            return RedirectToAction(nameof(Index));
        }
        _db.PurchaseOrderItems.RemoveRange(order.PurchaseOrderItems);
        _db.PurchaseOrders.Remove(order);
        await _db.SaveChangesAsync();
        TempData["Notice"] = "Pending order deleted.";
        return RedirectToAction(nameof(Index));
    }
}
