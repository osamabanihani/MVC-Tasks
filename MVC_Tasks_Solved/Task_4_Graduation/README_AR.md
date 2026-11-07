# Task 4 — Graduation Ceremony Supply Management

مشروع ASP.NET Core MVC بسيط يحقق المراحل الثلاث داخل نفس التطبيق.

## التشغيل
1. افتح `AcademyGraduation/AcademyGraduation.csproj` في Visual Studio 2022 مع .NET 8 وSQL Server LocalDB.
2. إذا كنت تستخدم SQL Server عادي غيّر `ConnectionStrings:DefaultConnection` في `appsettings.json`.
3. شغّل المشروع. يتم إنشاء قاعدة البيانات `AcademyGraduationPreparationDB` تلقائيًا باستخدام EF Core `EnsureCreated()` وإضافة بيانات الموردين والعروض التسعة.
4. من صفحة Create Supply Request اختَر **Phase 1** أو **Phase 2**، ثم اختر عرضًا لكل فئة واضغط Review.

## كيف يعمل؟
- Phase 1: الاختيار يُحفظ في مفاتيح Session مختلفة: `Offer_1`, `Offer_2`, `Offer_3`.
- Phase 2: الاختيارات تحفظ في `List<SelectedOffer>` متسلسلة JSON في المفتاح الواحد `SelectedOffers`.
- Phase 3: `Category` → `Supplier` → `Offer` علاقات EF Core. كل مورد له عرض واحد، ولا يتغير أسلوب Session.
- Cookie باسم `StorageMode` تحفظ وضع Session المختار.
- الخصم: إجمالي أكبر من 500 = 15%، أكبر من 300 = 10%، وإلا 0%. كل الحسابات في Controller.

مهم: الاختيار يدوي بالكامل، ولا يمكن فتح Review إلا عند اكتمال الاختيارات الثلاثة.
