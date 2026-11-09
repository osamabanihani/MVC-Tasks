using CafeteriaCodeFirst.Models;
using Microsoft.EntityFrameworkCore;

namespace CafeteriaCodeFirst.Migrations;

internal static class SnapshotModel
{
    internal static void Configure(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "8.0.11");
        modelBuilder.UseIdentityColumns();

        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.ToTable("Suppliers");
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.Phone).HasMaxLength(30).IsRequired();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products", t =>
            {
                t.HasCheckConstraint("CK_Products_Price", "[Price] >= 0");
                t.HasCheckConstraint("CK_Products_Quantity", "[Quantity] >= 0");
            });
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Price).HasColumnType("decimal(10,2)");
            entity.HasOne(x => x.Category).WithMany(x => x.Products)
                .HasForeignKey(x => x.CategoryId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Supplier).WithMany(x => x.Products)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.ToTable("PurchaseOrders", t =>
                t.HasCheckConstraint("CK_Orders_Status", "[Status] IN ('Pending','Received')"));
            entity.HasKey(x => x.Id);
            entity.Property(x => x.OrderDate).HasDefaultValueSql("SYSUTCDATETIME()");
            entity.Property(x => x.Status).HasMaxLength(20).IsRequired().HasDefaultValue("Pending");
            entity.HasOne(x => x.Supplier).WithMany(x => x.PurchaseOrders)
                .HasForeignKey(x => x.SupplierId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.ToTable("PurchaseOrderItems", t =>
            {
                t.HasCheckConstraint("CK_Items_Quantity", "[Quantity] > 0");
                t.HasCheckConstraint("CK_Items_UnitPrice", "[UnitPrice] >= 0");
            });
            entity.HasKey(x => x.Id);
            entity.Property(x => x.UnitPrice).HasColumnType("decimal(10,2)");
            entity.HasOne(x => x.PurchaseOrder).WithMany(x => x.PurchaseOrderItems)
                .HasForeignKey(x => x.PurchaseOrderId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.Product).WithMany(x => x.PurchaseOrderItems)
                .HasForeignKey(x => x.ProductId).OnDelete(DeleteBehavior.NoAction);
        });
    }
}
