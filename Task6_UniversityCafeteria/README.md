# Task 6 — University Cafeteria Management System

Simple ASP.NET Core MVC project using Entity Framework Core and SQL Server.

## Features

- **Categories:** View and create categories (Id, Name, Description).
- **Suppliers:** View and create suppliers (Id, Name, Phone, Email).
- **Products:** View and create products (Id, Name, Price, Quantity, CategoryId, SupplierId).
- **Code First:** `ApplicationDbContext` with Categories, Suppliers, Products.
- **Views:** HTML tables for Index pages, HTML Helpers for Create forms.

## Running the project

1. Open `UniversityCafeteria.sln` in Visual Studio 2022.
2. Make sure .NET 8 SDK and SQL Server Express LocalDB are installed.
3. Restore NuGet packages, then start the project with Ctrl+F5.
4. Add a Category, then a Supplier, then a Product.

The connection string is configured in `UniversityCafeteria/appsettings.json` for `(localdb)\\MSSQLLocalDB` and database `UniversityCafeteriaDB`. Adjust it if your SQL Server instance is different.

## How it works

- **Models:** Define the shape of data saved in SQL Server.
- **Controllers:** `Index()` shows records; `Create()` GET displays a form; `Create()` POST validates, adds and saves the entry.
- **Views:** Show records in HTML tables and create records using `Html.BeginForm`, `Html.TextBoxFor` and `Html.DropDownListFor`.
- **DbContext:** Contains the three `DbSet` properties.

For this simple assignment the database is created on first run using `db.Database.EnsureCreated()` in `Program.cs`. This is not a migration-based setup; do not mix it with EF Core Migrations on the same database.

## شرح مختصر

المشروع نظام كافتيريا بسيط بـ ASP.NET Core MVC. يحتوي على 3 Models و3 Controllers و6 Views، ويربط البيانات مع SQL Server عبر Entity Framework Core. استخدمنا HTML Helpers للنماذج و`Add()` و`SaveChanges()` لحفظ البيانات. المنتج مرتبط بالفئة والمورد باستخدام `CategoryId` و`SupplierId`.
