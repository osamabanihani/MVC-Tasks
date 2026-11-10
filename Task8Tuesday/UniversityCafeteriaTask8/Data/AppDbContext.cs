using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UniversityCafeteriaTask8.Models;

namespace UniversityCafeteriaTask8.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<SupplyRequest> SupplyRequests => Set<SupplyRequest>();
    public DbSet<RequestItem> RequestItems => Set<RequestItem>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Creates the Identity tables.

        builder.Entity<SupplyRequest>()
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RequestItem>()
            .HasOne(i => i.SupplyRequest)
            .WithMany(r => r.Items)
            .HasForeignKey(i => i.SupplyRequestId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<RequestItem>()
            .HasOne(i => i.Product)
            .WithMany()
            .HasForeignKey(i => i.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Product>().Property(p => p.Price).HasPrecision(12, 2);
        builder.Entity<SupplyRequest>().Property(r => r.TotalPrice).HasPrecision(12, 2);
        builder.Entity<RequestItem>().Property(i => i.Price).HasPrecision(12, 2);
    }
}
