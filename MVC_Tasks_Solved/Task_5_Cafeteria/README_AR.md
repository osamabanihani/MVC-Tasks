# Task 5 — University Cafeteria Inventory & Purchase Management

موجود **مشروعان مستقلان** لأن المهمة تطلب العمل بطريقتين مختلفتين وقاعدتين منفصلتين.

## المتطلبات
- Visual Studio 2022 يدعم .NET 8، مع SQL Server LocalDB أو SQL Server.
- Internet لاستعادة NuGet packages أول مرة (EF Core 8.0.11).
- ملف الإعدادات `appsettings.json` يحدد قاعدة البيانات، ويمكن تغيير السيرفر عند الحاجة.

## Part 1 — Database First (`CafeteriaDB`)
1. افتح `Part_1_Database_First/DatabaseFirst/create_CafeteriaDB.sql` في **SQL Server Management Studio** وشغّله لتنفيذ إنشاء قاعدة البيانات والجداول يدويًا.
2. افتح `Part_1_Database_First/CafeteriaDbFirst.csproj` في Visual Studio.
3. شغّل المشروع (أو `dotnet run`). **لا** يتم إنشاء الجداول آليًا في هذا الجزء، بل يعتمد على SQL script.

## Part 2 — Code First (`CafeteriaEmployeesDB`)
1. افتح `Part_2_Code_First/CafeteriaCodeFirst.csproj` في Visual Studio.
2. شغّل المشروع، وسيستخدم `Database.Migrate()` ملف Migration الموجود في مجلد `Migrations` لإنشاء الجداول في `CafeteriaEmployeesDB`.
3. أو نفذ `dotnet ef database update` من داخل مجلد المشروع عند توافر أداة dotnet-ef، ثم شغل التطبيق.

## الدخول التجريبي في المشروعين
- Username: `employee`
- Password: `1234`
- هذه بيانات تعليمية محفوظة في appsettings.json، وليست مصادقة آمنة للإنتاج؛ لا تُستخدم في تطبيق حقيقي.

## ما تم تنفيذه
- خمس جداول بنفس الأعمدة المطلوبة: Categories, Suppliers, Products, PurchaseOrders, PurchaseOrderItems.
- علاقات PK/FK وقيود CHECK وحماية المرجعية، وCRUD للأصناف والموردين والمنتجات.
- تسجيل دخول تجريبي وحماية صفحات الإدارة باستخدام `[Authorize]`.
- إنشاء Purchase Order بمنتجات متعددة من المورد نفسه وسعر وقت الطلب، وحساب مجموع البنود.
- الطلب الجديد `Pending`، وعند الضغط على `Mark Received` تتحول الحالة إلى `Received` وتُزاد كميات المخزون **مرة واحدة فقط**.
- إنشاء بيانات تجريبية بسيطة لتسهيل التجربة، ويمكن تغييرها من الواجهات.

**ملاحظة:** تحديث المخزون عند الاستلام هو قرار تطبيقي منطقي لأن الملف لم يحدد هل تتم زيادة المخزون عند إنشاء الطلب أو عند استلامه.
