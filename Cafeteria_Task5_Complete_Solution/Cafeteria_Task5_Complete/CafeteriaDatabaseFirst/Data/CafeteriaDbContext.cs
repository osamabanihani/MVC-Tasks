using Microsoft.EntityFrameworkCore;
using CafeteriaDatabaseFirst.Models;

namespace CafeteriaDatabaseFirst.Data;

public class CafeteriaDbContext : DbContext
{
    public CafeteriaDbContext(DbContextOptions<CafeteriaDbContext> options) : base(options) { }

    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<PurchaseOrder> PurchaseOrders => Set<PurchaseOrder>();
    public DbSet<PurchaseOrderItem> PurchaseOrderItems => Set<PurchaseOrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Suppliers");
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(150).IsRequired();
            entity.Property(e => e.Phone).HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products", t =>
            {
                t.HasCheckConstraint("CK_Products_Price", "[Price] >= 0");
                t.HasCheckConstraint("CK_Products_Quantity", "[Quantity] >= 0");
            });
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
            entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            entity.HasOne(e => e.Category).WithMany(c => c.Products)
                .HasForeignKey(e => e.CategoryId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.Supplier).WithMany(s => s.Products)
                .HasForeignKey(e => e.SupplierId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.ToTable("PurchaseOrders", t =>
                t.HasCheckConstraint("CK_Orders_Status", "[Status] IN ('Pending','Received')"));
            entity.HasKey(e => e.Id);
            entity.Property(e => e.OrderDate).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(e => e.Status).HasMaxLength(20).IsRequired().HasDefaultValue("Pending");
            entity.HasOne(e => e.Supplier).WithMany(s => s.PurchaseOrders)
                .HasForeignKey(e => e.SupplierId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.ToTable("PurchaseOrderItems", t =>
            {
                t.HasCheckConstraint("CK_Items_Quantity", "[Quantity] > 0");
                t.HasCheckConstraint("CK_Items_UnitPrice", "[UnitPrice] >= 0");
            });
            entity.HasKey(e => e.Id);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(10,2)");
            entity.HasOne(e => e.PurchaseOrder).WithMany(o => o.PurchaseOrderItems)
                .HasForeignKey(e => e.PurchaseOrderId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.Product).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(e => e.ProductId).OnDelete(DeleteBehavior.NoAction);
        });
    }
}
