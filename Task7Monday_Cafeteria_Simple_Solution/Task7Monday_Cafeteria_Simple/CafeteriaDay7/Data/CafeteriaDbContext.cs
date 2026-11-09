using CafeteriaDay7.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaDay7.Data;

public class CafeteriaDbContext : DbContext
{
    public CafeteriaDbContext(DbContextOptions<CafeteriaDbContext> options)
        : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Employee> Employees => Set<Employee>();
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().Property(p => p.Price).HasPrecision(10, 2);
        modelBuilder.Entity<Employee>().Property(e => e.Salary).HasPrecision(10, 2);
        modelBuilder.Entity<Order>().Property(o => o.TotalPrice).HasPrecision(10, 2);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category).WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Product>()
            .HasOne(p => p.Supplier).WithMany(s => s.Products)
            .HasForeignKey(p => p.SupplierId).OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Employee).WithMany(e => e.Orders)
            .HasForeignKey(o => o.EmployeeId).OnDelete(DeleteBehavior.Restrict);
    }
}
