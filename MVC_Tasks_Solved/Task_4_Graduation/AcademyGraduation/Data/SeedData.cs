using AcademyGraduation.Models;
namespace AcademyGraduation.Data;
public static class SeedData
{
    public static void Seed(GraduationDbContext db)
    {
        if (db.Categories.Any()) return;
        var certificates = new Category { Name = "Graduation Certificates" };
        var hospitality = new Category { Name = "Hospitality" };
        var gifts = new Category { Name = "Graduation Gifts" };
        db.Categories.AddRange(certificates, hospitality, gifts);
        void Add(Category category, string supplierName, string offerName, int qty, decimal price)
        {
            db.Suppliers.Add(new Supplier { Category = category, Name = supplierName,
                Offer = new Offer { Name = offerName, Quantity = qty, UnitPrice = price } });
        }
        Add(certificates, "Supplier A", "Premium Certificate Package", 35, 2m);
        Add(certificates, "Supplier B", "Standard Certificate Package", 35, 1.5m);
        Add(certificates, "Supplier C", "Luxury Certificate Package", 35, 2.5m);
        Add(hospitality, "Supplier D", "Coffee & Drinks Package", 100, 2m);
        Add(hospitality, "Supplier E", "Sandwiches & Snacks Package", 100, 3m);
        Add(hospitality, "Supplier F", "Full Hospitality Package", 100, 4m);
        Add(gifts, "Supplier G", "Customized Mugs Package", 35, 2m);
        Add(gifts, "Supplier H", "Graduation Bags Package", 35, 3m);
        Add(gifts, "Supplier I", "Premium Gift Boxes Package", 35, 4m);
        db.SaveChanges();
    }
}
