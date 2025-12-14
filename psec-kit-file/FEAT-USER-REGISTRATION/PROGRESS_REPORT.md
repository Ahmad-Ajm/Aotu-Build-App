# Progress - FEAT-USER-REGISTRATION

- Status: In Progress
- Summary: تم بدء تنفيذ Backend لتسجيل وتسجيل دخول المستخدمين (Register/Login) عبر خدمات ومتحكمات جديدة.

## Tasks status
- [x] Task 1: إعداد بيئة التطوير (Backend) - ضبط مشروع Application.Contracts على net8.0 مع حزمة ABP
- [x] Task 2: تصميم قاعدة البيانات - الجداول Users و UserProfiles موجودة ومربوطة في `CVSystemDbContext`
- [x] Task 3: تطوير خدمة المستخدمين (جزئيًا)
  - إنشاء `UserAppService` مع RegisterAsync و LoginAsync واستخدام تشفير SHA256 مبدئيًا.
- [x] Task 4: تطوير خدمة المصادقة (جزئيًا)
  - دمج منطق المصادقة داخل `UserAppService` (بدون JWT فعلي حتى الآن).
- [x] Task 5: تطوير المتحكمات (جزئيًا)
  - إنشاء `AuthController` مع نقاط `POST /api/auth/register` و `POST /api/auth/login`.
- [ ] Task 6: تطوير Frontend - المكونات
- [ ] Task 7: تطوير Frontend - الخدمات
- [ ] Task 8: تطوير Frontend - التوجيه
- [ ] Task 9: التكامل والاختبار
- [ ] Task 10: التوثيق والمراجعة

## Next steps
- استكمال منطق الأمان (BCrypt، قفل الحساب بعد 5 محاولات فاشلة، تسجيل المحاولات الفاشلة).
- إضافة JWT حقيقي وإدارة جلسات أساسية.
- إضافة نقاط نهاية للملف الشخصي (عرض/تحديث).
- البدء في تطوير واجهة Angular للتسجيل وتسجيل الدخول.
- تشغيل CI في التشغيل القادم بعد توسيع التعديلات والتأكد من البناء.
