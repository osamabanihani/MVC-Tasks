University Cafeteria — ASP.NET Core MVC Day 3

HOW TO OPEN:
1. Extract the ZIP file.
2. Open UniversityCafeteria.csproj in Visual Studio 2022 (ASP.NET and web development workload, .NET 8 SDK).
3. Run using Ctrl+F5, or in a terminal: dotnet run.
4. Open the displayed localhost URL (default route is Supply/Index).

PAGES:
/Supply/Index
/Supply/Products
/Supply/Suppliers
/Supply/CreateRequest
/Supply/About
/Supply/CheckQuantity?quantity=30

TASK IMPLEMENTATION:
- SupplyController with all required actions
- ViewData, ViewBag and foreach arrays
- GET CreateRequest, POST SubmitRequest using Request.Form
- Error message for empty employee name and invalid quantity/date
- TempData success message + RedirectToAction About
- Shared _MainLayout with five navigation links
- Bonus CheckQuantity action

This is a standalone project using in-memory values; no database or EF models.
Bootstrap is loaded from a CDN and requires internet access for styling.
The project targets .NET 8. No compilation was performed in this environment.
