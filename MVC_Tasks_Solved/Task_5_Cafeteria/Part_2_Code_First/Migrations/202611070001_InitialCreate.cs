using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using CafeteriaCodeFirst.Data;
namespace CafeteriaCodeFirst.Migrations;
[DbContext(typeof(CafeteriaDbContext))]
[Migration("202611070001_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Categories",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
            },
            constraints: table => table.PrimaryKey("PK_Categories", x => x.Id));
        migrationBuilder.CreateTable(
            name: "Suppliers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Suppliers", x => x.Id));
        migrationBuilder.CreateTable(
            name: "Products",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                Price = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                CategoryId = table.Column<int>(type: "int", nullable: false),
                SupplierId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Products", x => x.Id);
                table.CheckConstraint("CK_Products_Price", "[Price] >= 0");
                table.CheckConstraint("CK_Products_Quantity", "[Quantity] >= 0");
                table.ForeignKey("FK_Products_Categories_CategoryId", x => x.CategoryId,
                    "Categories", "Id", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_Products_Suppliers_SupplierId", x => x.SupplierId,
                    "Suppliers", "Id", onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.CreateTable(
            name: "PurchaseOrders",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false,
                    defaultValueSql: "SYSUTCDATETIME()"),
                Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false,
                    defaultValue: "Pending"),
                SupplierId = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseOrders", x => x.Id);
                table.CheckConstraint("CK_Orders_Status", "[Status] IN ('Pending','Received')");
                table.ForeignKey("FK_PurchaseOrders_Suppliers_SupplierId", x => x.SupplierId,
                    "Suppliers", "Id", onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.CreateTable(
            name: "PurchaseOrderItems",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false).Annotation("SqlServer:Identity", "1, 1"),
                PurchaseOrderId = table.Column<int>(type: "int", nullable: false),
                ProductId = table.Column<int>(type: "int", nullable: false),
                Quantity = table.Column<int>(type: "int", nullable: false),
                UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_PurchaseOrderItems", x => x.Id);
                table.CheckConstraint("CK_Items_Quantity", "[Quantity] > 0");
                table.CheckConstraint("CK_Items_UnitPrice", "[UnitPrice] >= 0");
                table.ForeignKey("FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId", x => x.PurchaseOrderId,
                    "PurchaseOrders", "Id", onDelete: ReferentialAction.Restrict);
                table.ForeignKey("FK_PurchaseOrderItems_Products_ProductId", x => x.ProductId,
                    "Products", "Id", onDelete: ReferentialAction.Restrict);
            });
        migrationBuilder.CreateIndex("IX_Products_CategoryId", "Products", "CategoryId");
        migrationBuilder.CreateIndex("IX_Products_SupplierId", "Products", "SupplierId");
        migrationBuilder.CreateIndex("IX_PurchaseOrders_SupplierId", "PurchaseOrders", "SupplierId");
        migrationBuilder.CreateIndex("IX_PurchaseOrderItems_PurchaseOrderId", "PurchaseOrderItems", "PurchaseOrderId");
        migrationBuilder.CreateIndex("IX_PurchaseOrderItems_ProductId", "PurchaseOrderItems", "ProductId");
    }
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "PurchaseOrderItems");
        migrationBuilder.DropTable(name: "Products");
        migrationBuilder.DropTable(name: "PurchaseOrders");
        migrationBuilder.DropTable(name: "Categories");
        migrationBuilder.DropTable(name: "Suppliers");
    }
    protected override void BuildTargetModel(ModelBuilder modelBuilder)
        => SnapshotModel.Configure(modelBuilder);
}
