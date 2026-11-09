using CafeteriaCodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaCodeFirst.Data;

public static class DemoDataSeeder
{
    public static async Task SeedAsync(CafeteriaDbContext db)
    {
        if (await db.Categories.AnyAsync() || await db.Suppliers.AnyAsync()) return;

        var drinks = new Category { Name = "Drinks", Description = "Coffee and beverages" };
        var food = new Category { Name = "Food", Description = "Meals and snacks" };
        var beverages = new Supplier { Name = "Beverages Supplier", Email = "beverages@example.com", Phone = "0790000001" };
        var foodSupplier = new Supplier { Name = "Food Supplier", Email = "food@example.com", Phone = "0790000002" };
        db.Products.AddRange(
            new Product { Name = "Coffee", Price = 2.50m, Quantity = 20, Category = drinks, Supplier = beverages },
            new Product { Name = "Juice", Price = 1.75m, Quantity = 15, Category = drinks, Supplier = beverages },
            new Product { Name = "Sandwich", Price = 3.25m, Quantity = 12, Category = food, Supplier = foodSupplier }
        );
        await db.SaveChangesAsync();
    }
}
