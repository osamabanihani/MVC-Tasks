using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaTask8.Models;

namespace UniversityCafeteriaTask8.Data;

public static class SeedData
{
    public static async Task InitializeAsync(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // Quick classroom setup: create database automatically (no migrations required).
        await db.Database.EnsureCreatedAsync();

        foreach (var name in new[] { "User", "Admin", "SuperAdmin" })
        {
            if (!await roleManager.RoleExistsAsync(name))
                await roleManager.CreateAsync(new IdentityRole(name));
        }

        if (!await db.Products.AnyAsync())
        {
            db.Products.AddRange(
                new Product { Name = "Coffee", Description = "Coffee packets", Price = 3, Quantity = 50 },
                new Product { Name = "Milk", Description = "Milk cartons", Price = 2, Quantity = 40 },
                new Product { Name = "Juice", Description = "Juice bottles", Price = 1.5m, Quantity = 70 },
                new Product { Name = "Sandwich", Description = "Sandwiches", Price = 2.5m, Quantity = 30 },
                new Product { Name = "Water", Description = "Water bottles", Price = 0.5m, Quantity = 100 },
                new Product { Name = "Packaging", Description = "Packaging materials", Price = 1, Quantity = 60 }
            );
            await db.SaveChangesAsync();
        }

        // Demo account exists ONLY when running in Development mode.
        if (app.Environment.IsDevelopment())
        {
            var email = app.Configuration["DemoSuperAdmin:Email"];
            var password = app.Configuration["DemoSuperAdmin:Password"];

            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(password))
            {
                var admin = await userManager.FindByEmailAsync(email);
                if (admin == null)
                {
                    admin = new ApplicationUser { UserName = email, Email = email, FullName = "Demo Super Admin" };
                    var result = await userManager.CreateAsync(admin, password);
                    if (!result.Succeeded)
                        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                if (!await userManager.IsInRoleAsync(admin, "SuperAdmin"))
                    await userManager.AddToRoleAsync(admin, "SuperAdmin");
            }
        }
    }
}
