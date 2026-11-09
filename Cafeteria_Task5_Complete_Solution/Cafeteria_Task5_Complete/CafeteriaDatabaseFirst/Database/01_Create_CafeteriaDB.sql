-- DATABASE FIRST: execute the entire script in SSMS, connected to the correct SQL Server instance.
USE master;
GO
IF DB_ID(N'CafeteriaDB') IS NULL
    CREATE DATABASE [CafeteriaDB];
GO
USE [CafeteriaDB];
GO
IF OBJECT_ID(N'dbo.Categories', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Categories PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL
    );
END;
GO
IF OBJECT_ID(N'dbo.Suppliers', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Suppliers (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Suppliers PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NOT NULL,
        Phone NVARCHAR(30) NOT NULL
    );
END;
GO
IF OBJECT_ID(N'dbo.Products', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.Products (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Products PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        Quantity INT NOT NULL,
        CategoryId INT NOT NULL,
        SupplierId INT NOT NULL,
        CONSTRAINT CK_Products_Price CHECK (Price >= 0),
        CONSTRAINT CK_Products_Quantity CHECK (Quantity >= 0),
        CONSTRAINT FK_Products_Categories_CategoryId FOREIGN KEY (CategoryId) REFERENCES dbo.Categories(Id),
        CONSTRAINT FK_Products_Suppliers_SupplierId FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers(Id)
    );
    CREATE INDEX IX_Products_CategoryId ON dbo.Products(CategoryId);
    CREATE INDEX IX_Products_SupplierId ON dbo.Products(SupplierId);
END;
GO
IF OBJECT_ID(N'dbo.PurchaseOrders', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PurchaseOrders (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PurchaseOrders PRIMARY KEY,
        OrderDate DATETIME2 NOT NULL CONSTRAINT DF_PurchaseOrders_OrderDate DEFAULT SYSUTCDATETIME(),
        Status NVARCHAR(20) NOT NULL CONSTRAINT DF_PurchaseOrders_Status DEFAULT N'Pending',
        SupplierId INT NOT NULL,
        CONSTRAINT CK_Orders_Status CHECK (Status IN (N'Pending', N'Received')),
        CONSTRAINT FK_PurchaseOrders_Suppliers_SupplierId FOREIGN KEY (SupplierId) REFERENCES dbo.Suppliers(Id)
    );
    CREATE INDEX IX_PurchaseOrders_SupplierId ON dbo.PurchaseOrders(SupplierId);
END;
GO
IF OBJECT_ID(N'dbo.PurchaseOrderItems', N'U') IS NULL
BEGIN
    CREATE TABLE dbo.PurchaseOrderItems (
        Id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PurchaseOrderItems PRIMARY KEY,
        PurchaseOrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        CONSTRAINT CK_Items_Quantity CHECK (Quantity > 0),
        CONSTRAINT CK_Items_UnitPrice CHECK (UnitPrice >= 0),
        CONSTRAINT FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId FOREIGN KEY (PurchaseOrderId) REFERENCES dbo.PurchaseOrders(Id),
        CONSTRAINT FK_PurchaseOrderItems_Products_ProductId FOREIGN KEY (ProductId) REFERENCES dbo.Products(Id)
    );
    CREATE INDEX IX_PurchaseOrderItems_PurchaseOrderId ON dbo.PurchaseOrderItems(PurchaseOrderId);
    CREATE INDEX IX_PurchaseOrderItems_ProductId ON dbo.PurchaseOrderItems(ProductId);
END;
GO
-- Sample records (run this script more than once without duplicating samples)
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE Name = N'Drinks')
    INSERT INTO dbo.Categories(Name, Description) VALUES (N'Drinks', N'Coffee, juice and beverages');
IF NOT EXISTS (SELECT 1 FROM dbo.Categories WHERE Name = N'Food')
    INSERT INTO dbo.Categories(Name, Description) VALUES (N'Food', N'Meals and snacks');
IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers WHERE Email = N'beverages@example.com')
    INSERT INTO dbo.Suppliers(Name, Email, Phone) VALUES (N'Beverages Supplier', N'beverages@example.com', N'0790000001');
IF NOT EXISTS (SELECT 1 FROM dbo.Suppliers WHERE Email = N'food@example.com')
    INSERT INTO dbo.Suppliers(Name, Email, Phone) VALUES (N'Food Supplier', N'food@example.com', N'0790000002');
GO
DECLARE @Drinks INT = (SELECT TOP (1) Id FROM dbo.Categories WHERE Name = N'Drinks');
DECLARE @Food INT = (SELECT TOP (1) Id FROM dbo.Categories WHERE Name = N'Food');
DECLARE @DrinkSupplier INT = (SELECT TOP (1) Id FROM dbo.Suppliers WHERE Email = N'beverages@example.com');
DECLARE @FoodSupplier INT = (SELECT TOP (1) Id FROM dbo.Suppliers WHERE Email = N'food@example.com');
IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE Name=N'Coffee' AND SupplierId=@DrinkSupplier)
    INSERT INTO dbo.Products(Name, Price, Quantity, CategoryId, SupplierId) VALUES (N'Coffee', 2.50, 20, @Drinks, @DrinkSupplier);
IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE Name=N'Juice' AND SupplierId=@DrinkSupplier)
    INSERT INTO dbo.Products(Name, Price, Quantity, CategoryId, SupplierId) VALUES (N'Juice', 1.75, 15, @Drinks, @DrinkSupplier);
IF NOT EXISTS(SELECT 1 FROM dbo.Products WHERE Name=N'Sandwich' AND SupplierId=@FoodSupplier)
    INSERT INTO dbo.Products(Name, Price, Quantity, CategoryId, SupplierId) VALUES (N'Sandwich', 3.25, 12, @Food, @FoodSupplier);
GO
SELECT 'Categories' AS TableName, COUNT(*) AS RowsCount FROM dbo.Categories
UNION ALL SELECT 'Suppliers', COUNT(*) FROM dbo.Suppliers
UNION ALL SELECT 'Products', COUNT(*) FROM dbo.Products
UNION ALL SELECT 'PurchaseOrders', COUNT(*) FROM dbo.PurchaseOrders
UNION ALL SELECT 'PurchaseOrderItems', COUNT(*) FROM dbo.PurchaseOrderItems;
