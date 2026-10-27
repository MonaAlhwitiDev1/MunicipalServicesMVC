# MunicipalServicesMVC

نظام تدريبي لإدارة الخدمات البلدية باستخدام ASP.NET Core MVC وWeb API وEntity Framework Core وSQL Server.

يدعم إدارة الإدارات والخدمات والمستخدمين والصلاحيات، وسلة الخدمات المحفوظة، وإرسال الطلبات وعرض كمياتها.

## التشغيل
- يتطلب .NET SDK متوافقًا مع net7.0 وSQL Server.
- اضبط ConnectionStrings:DefaultDatabase للموقع والـAPI باستخدام User Secrets أو متغير البيئة ConnectionStrings__DefaultDatabase.
- طبّق migrations الموجودة في مشروع Infrastructure على قاعدة بياناتك.
- مشروع الموقع: MunicipalServicesMVC/MunicipalServicesMVC.csproj
- مشروع API: MunicipalServicesMVC/MunicipalServicesMVC.API/MunicipalServicesMVC.API.csproj
- احتفظ بالمجلدين الرئيسيين بجانب بعضهما لضمان صحة مراجع المشاريع.
- بيانات الاتصال وكلمات المرور الفعلية غير مرفقة.
