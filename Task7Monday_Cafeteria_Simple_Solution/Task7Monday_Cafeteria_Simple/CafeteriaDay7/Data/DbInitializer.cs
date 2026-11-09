using CafeteriaDay7.Models;

namespace CafeteriaDay7.Data;

public static class DbInitializer
{
    public static void Seed(CafeteriaDbContext db)
    {
        if (db.Categories.Any()) return;

        var drinks = new Category { Name = "Drinks", Description = "Hot and cold drinks" };
        var meals = new Category { Name = "Meals", Description = "Fresh cafeteria food" };
        db.Categories.AddRange(drinks, meals);

        var supplier1 = new Supplier
        {
            Name = "Campus Supplier", Phone = "0790000001",
            Email = "campus@example.com", Address = "Irbid"
        };
        var supplier2 = new Supplier
        {
            Name = "Fresh Food Co", Phone = "0790000002",
            Email = "fresh@example.com", Address = "Amman"
        };
        db.Suppliers.AddRange(supplier1, supplier2);

        var employee1 = new Employee
        {
            FullName = "Ahmad Ali", Phone = "0791111111", Email = "ahmad@example.com",
            Position = "Cashier", Salary = 400, HireDate = new DateTime(2026, 1, 10)
        };
        var employee2 = new Employee
        {
            FullName = "Sara Omar", Phone = "0792222222", Email = "sara@example.com",
            Position = "Supervisor", Salary = 550, HireDate = new DateTime(2026, 2, 15)
        };
        db.Employees.AddRange(employee1, employee2);
        db.SaveChanges();

        db.Products.AddRange(
            new Product { Name = "Coffee", Description = "Hot coffee", Price = 1.50m, Quantity = 15, CategoryId = drinks.Id, SupplierId = supplier1.Id },
            new Product { Name = "Juice", Description = "Orange juice", Price = 1.00m, Quantity = 3, CategoryId = drinks.Id, SupplierId = supplier1.Id },
            new Product { Name = "Sandwich", Description = "Cheese sandwich", Price = 2.50m, Quantity = 8, CategoryId = meals.Id, SupplierId = supplier2.Id }
        );

        db.Orders.AddRange(
            new Order { CustomerName = "Omar", OrderDate = DateTime.Today, TotalPrice = 4.50m, Status = "Pending", EmployeeId = employee1.Id },
            new Order { CustomerName = "Lina", OrderDate = DateTime.Today, TotalPrice = 7.00m, Status = "Completed", EmployeeId = employee1.Id }
        );

        db.SaveChanges();
    }
}
