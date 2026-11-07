-- Task 5 / Part 1: DATABASE FIRST.
-- Open this script in SQL Server Management Studio and click Execute.
IF DB_ID(N'CafeteriaDB') IS NULL CREATE DATABASE CafeteriaDB;
GO
USE CafeteriaDB;
GO
IF OBJECT_ID('dbo.Categories','U') IS NULL
BEGIN
    CREATE TABLE dbo.Categories (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Description NVARCHAR(500) NULL
    );
END;
GO
IF OBJECT_ID('dbo.Suppliers','U') IS NULL
BEGIN
    CREATE TABLE dbo.Suppliers (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Email NVARCHAR(150) NOT NULL,
        Phone NVARCHAR(30) NOT NULL
    );
END;
GO
IF OBJECT_ID('dbo.Products','U') IS NULL
BEGIN
    CREATE TABLE dbo.Products (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        Name NVARCHAR(100) NOT NULL,
        Price DECIMAL(10,2) NOT NULL,
        Quantity INT NOT NULL,
        CategoryId INT NOT NULL,
        SupplierId INT NOT NULL,
        CONSTRAINT CK_Products_Price CHECK (Price >= 0),
        CONSTRAINT CK_Products_Quantity CHECK (Quantity >= 0),
        CONSTRAINT FK_Products_Categories_CategoryId FOREIGN KEY(CategoryId) REFERENCES dbo.Categories(Id),
        CONSTRAINT FK_Products_Suppliers_SupplierId FOREIGN KEY(SupplierId) REFERENCES dbo.Suppliers(Id)
    );
    CREATE INDEX IX_Products_CategoryId ON dbo.Products(CategoryId);
    CREATE INDEX IX_Products_SupplierId ON dbo.Products(SupplierId);
END;
GO
IF OBJECT_ID('dbo.PurchaseOrders','U') IS NULL
BEGIN
    CREATE TABLE dbo.PurchaseOrders (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        OrderDate DATETIME2 NOT NULL DEFAULT SYSUTCDATETIME(),
        Status NVARCHAR(20) NOT NULL DEFAULT N'Pending',
        SupplierId INT NOT NULL,
        CONSTRAINT CK_Orders_Status CHECK (Status IN (N'Pending',N'Received')),
        CONSTRAINT FK_PurchaseOrders_Suppliers_SupplierId FOREIGN KEY(SupplierId) REFERENCES dbo.Suppliers(Id)
    );
    CREATE INDEX IX_PurchaseOrders_SupplierId ON dbo.PurchaseOrders(SupplierId);
END;
GO
IF OBJECT_ID('dbo.PurchaseOrderItems','U') IS NULL
BEGIN
    CREATE TABLE dbo.PurchaseOrderItems (
        Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
        PurchaseOrderId INT NOT NULL,
        ProductId INT NOT NULL,
        Quantity INT NOT NULL,
        UnitPrice DECIMAL(10,2) NOT NULL,
        CONSTRAINT CK_Items_Quantity CHECK (Quantity > 0),
        CONSTRAINT CK_Items_UnitPrice CHECK (UnitPrice >= 0),
        CONSTRAINT FK_PurchaseOrderItems_PurchaseOrders_PurchaseOrderId FOREIGN KEY(PurchaseOrderId) REFERENCES dbo.PurchaseOrders(Id),
        CONSTRAINT FK_PurchaseOrderItems_Products_ProductId FOREIGN KEY(ProductId) REFERENCES dbo.Products(Id)
    );
    CREATE INDEX IX_PurchaseOrderItems_PurchaseOrderId ON dbo.PurchaseOrderItems(PurchaseOrderId);
    CREATE INDEX IX_PurchaseOrderItems_ProductId ON dbo.PurchaseOrderItems(ProductId);
END;
GO
-- Optional starting data for testing the UI.
IF NOT EXISTS (SELECT 1 FROM dbo.Categories)
BEGIN
    INSERT INTO dbo.Categories (Name, Description) VALUES (N'Drinks',N'Cafeteria drinks');
    INSERT INTO dbo.Suppliers (Name, Email, Phone) VALUES (N'Demo Supplier',N'demo@example.com',N'0790000000');
    INSERT INTO dbo.Products (Name,Price,Quantity,CategoryId,SupplierId)
    VALUES (N'Water Bottle',0.50,20,
        (SELECT TOP 1 Id FROM dbo.Categories ORDER BY Id),
        (SELECT TOP 1 Id FROM dbo.Suppliers ORDER BY Id));
    INSERT INTO dbo.Products (Name,Price,Quantity,CategoryId,SupplierId)
    VALUES (N'Orange Juice',1.25,10,
        (SELECT TOP 1 Id FROM dbo.Categories ORDER BY Id),
        (SELECT TOP 1 Id FROM dbo.Suppliers ORDER BY Id));
END;
GO
