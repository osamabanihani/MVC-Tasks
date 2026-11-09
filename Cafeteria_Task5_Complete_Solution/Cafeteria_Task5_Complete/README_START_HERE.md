# Task 5 - University Cafeteria Inventory & Purchase Management

This solution follows **Task 5(2).docx** exactly for the two database names and the five tables:

| Approach | Project | SQL Server database | How tables are created |
| --- | --- | --- | --- |
| Database First | `CafeteriaDatabaseFirst` | `CafeteriaDB` | Run `DatabaseFirst/01_Create_CafeteriaDB.sql` in SSMS first. The EF Core context maps the existing tables. |
| Code First | `CafeteriaCodeFirst` | `CafeteriaEmployeesDB` | EF Core migration `InitialCreate` creates the tables. In Development the app automatically applies the migration on startup; you can also use `Update-Database`. |

**Five project tables in EACH database**: `Categories`, `Suppliers`, `Products`, `PurchaseOrders`, `PurchaseOrderItems`.

## Quick start (Visual Studio 2022 / .NET 8 + SQL Server)

1. Make sure SQL Server and SSMS are installed. If your SQL Server instance is named `SQLEXPRESS`, change `Server=.` to `Server=.\SQLEXPRESS` in **both** `appsettings.json` files.
2. Open `Cafeteria_Task5.sln` in Visual Studio 2022 and allow NuGet Restore.
3. **Database First:** in SSMS, connect using Windows Authentication and run the whole `CafeteriaDatabaseFirst/Database/01_Create_CafeteriaDB.sql` script. It creates `CafeteriaDB`, the five tables, constraints, FKs, indexes and sample rows. Then set `CafeteriaDatabaseFirst` as startup project and run it.
4. **Code First:** set `CafeteriaCodeFirst` as startup project and run it; in **Development** it runs `Database.MigrateAsync()` to create `CafeteriaEmployeesDB` and its five tables. To do it manually instead, open Package Manager Console, set default project `CafeteriaCodeFirst`, then run `Update-Database`.
5. On either app, open `/Account/Login`. **Development demo login:** `employee` / `Demo123!` (change this for any real usage).
6. Try Categories -> Suppliers -> Products -> Purchase Orders. An order is initially `Pending`, and the stock increases only once when you press `Mark Received` on its Details page.

## Database First reverse engineering

Database First means **SQL tables come first**. The delivered `CafeteriaDatabaseFirst/Data/CafeteriaDbContext.cs` and model classes already map the SQL schema and are ready to use. To see how scaffolding works from the same existing SQL tables, the corresponding Package Manager Console command is:

```powershell
Scaffold-DbContext "Server=.;Database=CafeteriaDB;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer -Context CafeteriaDbContext -OutputDir Models -ContextDir Data -NoOnConfiguring -Force
```

**Do not run `-Force` over your working project unless you intend to replace its mapped classes.** Scaffolding may use different navigation property names and overwrite the tailored model/context code. Use a new empty project to demonstrate fresh scaffolding, or back up the solution first.

## Code First migrations

- `CafeteriaCodeFirst/Models` defines the C# schema and its navigation properties.
- `CafeteriaCodeFirst/Data/CafeteriaDbContext.cs` configures names, decimal precision, five foreign keys and SQL constraints.
- `CafeteriaCodeFirst/Migrations/202611090001_InitialCreate.cs` contains the initial `Up()`/`Down()` database operations.
- For future model changes: `Add-Migration DescribeChange`, then `Update-Database` (select the **Code First** project as PMC default project).

## Functional pages

- Login and Logout (educational demo credentials; no sixth table)
- Dashboard with four summary counts
- Categories: List, Create, Edit, Delete
- Suppliers: List, Create, Edit, Delete
- Products: List, Details, Create, Edit, Delete and stock quantity
- Purchase Orders: List, Create with multiple item rows and supplier filtering, Details with totals, Receive (+stock exactly once), Delete pending order
- Database checks for nonnegative product prices and stock, positive order item quantities, and order status restricted to `Pending` or `Received`.

## Relationships (five foreign keys)

- `Categories (1) --> (many) Products`
- `Suppliers (1) --> (many) Products`
- `Suppliers (1) --> (many) PurchaseOrders`
- `PurchaseOrders (1) --> (many) PurchaseOrderItems`
- `Products (1) --> (many) PurchaseOrderItems`

`PurchaseOrderItems` is a join/detail table that stores the price at the time of the order. Order totals use `Quantity * UnitPrice`. Stock changes when the order moves from Pending to Received, never when the order is first created.

## Important notes

- The SQL Server account must have permission to create databases for Code First auto-migration (or use `Update-Database` with suitable privileges).
- The **demo login is for a local training assignment**, not a production authentication system. Production requires ASP.NET Core Identity (which adds Identity tables) or external organizational sign-in; no such tables are included because the assignment requires five application tables.
- We cannot run SQL Server or the .NET SDK in this delivery environment; code was checked for structure and consistency but was not executed against your local SQL Server. If you get a connection error, confirm the correct `Server` / SQL Server service / Windows access.
