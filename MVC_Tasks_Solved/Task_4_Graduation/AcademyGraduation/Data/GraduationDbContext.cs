using AcademyGraduation.Models;
using Microsoft.EntityFrameworkCore;
namespace AcademyGraduation.Data;
public class GraduationDbContext : DbContext
{
    public GraduationDbContext(DbContextOptions<GraduationDbContext> options) : base(options) { }
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Offer> Offers => Set<Offer>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().Property(x => x.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Supplier>().Property(x => x.Name).HasMaxLength(100).IsRequired();
        modelBuilder.Entity<Supplier>().HasOne(x => x.Category)
            .WithMany(x => x.Suppliers).HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Offer>().Property(x => x.Name).HasMaxLength(150).IsRequired();
        modelBuilder.Entity<Offer>().Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");
        modelBuilder.Entity<Offer>().HasIndex(x => x.SupplierId).IsUnique();
        modelBuilder.Entity<Offer>().HasOne(x => x.Supplier).WithOne(x => x.Offer)
            .HasForeignKey<Offer>(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
    }
}
