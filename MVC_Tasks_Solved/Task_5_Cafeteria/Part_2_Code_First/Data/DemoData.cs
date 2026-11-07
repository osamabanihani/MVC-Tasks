using CafeteriaCodeFirst.Models;
namespace CafeteriaCodeFirst.Data;
public static class DemoData
{
    public static void Seed(CafeteriaDbContext db)
    {
        if (db.Categories.Any()) return;
        var category = new Category { Name = "Drinks", Description = "Cafeteria drinks" };
        var supplier = new Supplier { Name = "Demo Supplier", Email = "demo@example.com", Phone = "0790000000" };
        db.Products.AddRange(
            new Product { Name = "Water Bottle", Price = 0.5m, Quantity = 20, Category = category, Supplier = supplier },
            new Product { Name = "Orange Juice", Price = 1.25m, Quantity = 10, Category = category, Supplier = supplier }
        );
        db.SaveChanges();
    }
}
