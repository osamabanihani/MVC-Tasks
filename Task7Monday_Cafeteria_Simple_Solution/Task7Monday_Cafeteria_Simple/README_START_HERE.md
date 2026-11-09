# Task 7 Monday — University Cafeteria Management System

هذا هو الحل **البسيط والكامل** حسب ملف Task7Monday(1).docx.

## محتويات المشروع

- Visual Studio solution: `CafeteriaDay7.sln`
- ASP.NET Core MVC (.NET 8)
- Entity Framework Core + Microsoft SQL Server
- Models: `Category`, `Supplier`, `Product`, `Employee`, `Order`
- Controllers: `CategoryController`, `SupplierController`, `ProductController`, `EmployeeController`, `OrderController`
- لكل قسم: **Index / Create / Edit / Details / Delete** (الحذف من Index برسالة تأكيد)
- صفحات HTML Helpers: `Html.BeginForm`, `Html.TextBoxFor`, `Html.DropDownListFor`, `Html.ActionLink`, `Html.DisplayFor`
- الأعمدة الإضافية: Products Count في Category/Supplier، Low Stock للكمية الأقل من 5، Orders Count في Employee.
- Navigation يربط الأقسام الخمسة.

## تشغيل المشروع في Visual Studio 2022

1. فك ضغط ZIP وافتح `CafeteriaDay7.sln`.
2. تأكد من تثبيت **.NET 8 SDK** و **SQL Server**.
3. افتح `CafeteriaDay7/appsettings.json` وعدّل `Server=.` إذا سيرفر SQL عندك مختلف.
   - SQL Server default instance: `Server=.`
   - SQL Express: `Server=.\\SQLEXPRESS` (في JSON استخدم `\\` لباك سلاش واحدة)
   - LocalDB: `Server=(localdb)\\MSSQLLocalDB` (في JSON استخدم `\\` لباك سلاش واحدة)
4. اضغط **F5** أو **Ctrl+F5**.
5. سيستخدم المشروع `EnsureCreated()` لإنشاء قاعدة `CafeteriaDay7DB` وجداولها تلقائياً، ثم يضيف بيانات بسيطة للتجربة.

**مهم:** حساب Windows المستخدم مع `Trusted_Connection=True` لازم يكون عنده صلاحية إنشاء قاعدة بيانات. إذا ظهرت رسالة Login Failed أو Cannot Open Database، راجع Connection String والاتصال بـ SSMS.

## لفهم المطلوب

- `Models`: تمثل الجداول.
- `Data/CafeteriaDbContext.cs`: DbSets والعلاقات.
- `Data/DbInitializer.cs`: بيانات تجريبية للعرض.
- `Controllers`: أكواد CRUD.
- `Views`: الصفحات الأربعة لكل قسم.

## Code First & Migrations

هذا الحل يستخدم `EnsureCreated()` لسهولة التشغيل والتعلم، **ولا يحتوي على ملفات Migrations**. إذا طلب المدرب استخدامها، قبل التشغيل على قاعدة جديدة:

1. احذف/علّق سطر `db.Database.EnsureCreated();` في `Program.cs`.
2. نفّذ في Package Manager Console:

```powershell
Add-Migration InitialCreate
Update-Database
```

3. استخدم `db.Database.Migrate()` بدل `EnsureCreated()` إذا أردت تطبيق Migrations عند التشغيل، بعد إعدادها.

**لا تستخدم EnsureCreated على قاعدة تنوي إدارتها بالمهاجرات (Migrations)** لأنها لا تنشئ سجل المهاجرات.

## ملاحظات

- المهمة المطلوبة هي **Customer Orders** (الحقول: CustomerName, OrderDate, TotalPrice, Status, EmployeeId) وليست PurchaseOrderItems الخاصة بالمهمة السابقة.
- المشروع تدريبي، بدون Login/Authorization أو Form Validation إضافية؛ ملف المهمة لا يطلب إعداد المصادقة أو قواعد التحقق.
- عند وجود Products مرتبطة بتصنيف أو مورد لن يسمح بحذف الأب، وكذلك الموظف المرتبط بطلبات؛ حمايةً للعلاقات في قاعدة البيانات.
- يتطلب اتصال SQL Server محلياً. لم يتم تشغيل build في بيئة إعداد الملف لعدم توفر .NET SDK هنا.
