using Microsoft.EntityFrameworkCore;
using CafeteriaCodeFirst.Models;
namespace CafeteriaCodeFirst.Data;

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
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
        });
        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Suppliers");
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
        });
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products", table => {
                table.HasCheckConstraint("CK_Products_Price", "[Price] >= 0");
                table.HasCheckConstraint("CK_Products_Quantity", "[Quantity] >= 0");
            });
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Price).HasColumnType("decimal(10,2)");
            entity.HasOne(x => x.Category).WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Supplier).WithMany(x => x.Products)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.ToTable("PurchaseOrders", table =>
                table.HasCheckConstraint("CK_Orders_Status", "[Status] IN ('Pending','Received')"));
            entity.Property(x => x.OrderDate).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired().HasDefaultValue("Pending");
            entity.HasOne(x => x.Supplier).WithMany(x => x.PurchaseOrders)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.Restrict);
        });
        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.ToTable("PurchaseOrderItems", table => {
                table.HasCheckConstraint("CK_Items_Quantity", "[Quantity] > 0");
                table.HasCheckConstraint("CK_Items_UnitPrice", "[UnitPrice] >= 0");
            });
            entity.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");
            entity.HasOne(x => x.PurchaseOrder).WithMany(x => x.Items)
                .HasForeignKey(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(x => x.Product).WithMany(x => x.PurchaseOrderItems)
                .HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.Restrict);
        });
    }
}
