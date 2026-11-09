حل Task 5 – نظام كافتيريا الجامعة

افتح الملف Cafeteria_Task5.sln في Visual Studio 2022.

1) DATABASE FIRST
- افتح SSMS.
- شغّل SQL الموجود هنا:
  CafeteriaDatabaseFirst/Database/01_Create_CafeteriaDB.sql
- سوف ينشئ قاعدة البيانات CafeteriaDB وخمسة جداول وعلاقاتها.
- اختَر CafeteriaDatabaseFirst كمشروع تشغيل ثم Ctrl+F5.

2) CODE FIRST
- اختَر CafeteriaCodeFirst كمشروع تشغيل.
- شغّل المشروع؛ في وضع Development سيُطبّق Migration تلقائياً لإنشاء CafeteriaEmployeesDB والجداول الخمسة.
- أو افتح Package Manager Console واختَر CafeteriaCodeFirst ثم اكتب Update-Database.

مهم: إذا ظهر خطأ الاتصال بقاعدة البيانات، افتح appsettings.json في كل مشروع وغيّر Server=. إلى اسم السيرفر عندك، مثل Server=.\SQLEXPRESS (في JSON تُكتب الشرطة المائلة مرتين).

تسجيل الدخول التجريبي للمشروعين على جهازك:
Username: employee
Password: Demo123!
هذا تسجيل تجريبي للتدريب فقط وليس مناسباً للإنتاج.

التجربة: Categories -> Suppliers -> Products -> Purchase Orders -> New Purchase Order -> Details -> Mark Received.
أمر الشراء يبدأ Pending، والمخزون يزيد عند الاستلام Received فقط.

العلاقات الخمسة:
Category -> Products
Supplier -> Products
Supplier -> PurchaseOrders
PurchaseOrder -> PurchaseOrderItems
Product -> PurchaseOrderItems

الملف README_START_HERE.md فيه شرح إضافي باللغة الإنجليزية.
