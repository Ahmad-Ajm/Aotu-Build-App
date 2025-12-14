# Progress - FEAT-USER-REGISTRATION

## الحالة الحالية
- الميزة: In Progress
- الملخص: تم البدء في تنفيذ طبقة الـ Backend لتسجيل المستخدمين وتسجيل الدخول.

## ما تم إنجازه في هذا التشغيل
1. إضافة DTOs خاصة بالمصادقة:
   - `RegisterUserRequestDto`
   - `LoginRequestDto`
   - `AuthResponseDto`
   - المسار: `code/backend/CVSystem.Application.Contracts/Users/Dtos/UserDtos.cs`
2. إضافة واجهة لخدمة المستخدمين:
   - `IUserAppService`
   - المسار: `code/backend/CVSystem.Application.Contracts/Users/IUserAppService.cs`
3. إضافة تنفيذ مبدئي لخدمة المستخدمين لتسجيل المستخدم وتسجيل الدخول:
   - `UserAppService`
   - المسار: `code/backend/CVSystem.Application/Users/UserAppService.cs`
   - يعتمد على `CVSystemDbContext` والكيانين `User` و `UserProfile`.
4. إضافة متحكم للمصادقة مع نقاط النهاية:
   - `POST /api/auth/register`
   - `POST /api/auth/login`
   - المسار: `code/backend/src/Http/API/Controllers/AuthController.cs`
5. تحديث بعض إعدادات المشاريع/الموديول لضمان التوافق مع ABP و .NET 8:
   - تحديث `CVSystemApplicationModule` لضبط AutoMapper فقط.
   - تحديث مشروع `CVSystem.Application.Contracts` لاستخدام net8.0 ومرجع ABP Application.

## المهام مقابل خطة SpecKit

### من tasks.md
1. إعداد بيئة التطوير (Backend) - **جزئيًا**
   - تم ضبط مشروع Application.Contracts على net8.0 مع حزمة ABP.
   - باقي الإعدادات (مثل إعداد قاعدة البيانات على مستوى الـ solution بالكامل) كانت موجودة مسبقًا.
2. تصميم قاعدة البيانات - **مكتمل مسبقًا لهذه الميزة**
   - الجداول `Users` و `UserProfiles` معرفة مسبقًا عبر الكيانات و `CVSystemDbContext`.
3. تطوير خدمة المستخدمين - **تم تنفيذ جزء كبير**
   - إنشاء `UserAppService` مع:
     - دالة إنشاء المستخدم (RegisterAsync) مع تحقق أساسي من البريد والتأكد من تطابق كلمة المرور.
     - دالة تسجيل الدخول (LoginAsync) مع تحقق من البريد وكلمة المرور وتحديث `LastLoginDate`.
     - تشفير كلمات المرور باستخدام `SHA256` (يحتاج لاحقًا إلى تحسين لاستخدام BCrypt حسب المواصفات).
4. تطوير خدمة المصادقة - **تم بدء التنفيذ**
   - تم دمج منطق التسجيل وتسجيل الدخول داخل `UserAppService`.
   - لم يتم بعد تنفيذ JWT حقيقي أو إدارة جلسات متكاملة (موجود TODO في الكود).
5. تطوير المتحكمات (Backend) - **تم تنفيذ جزء**
   - إنشاء `AuthController` بنقاط النهاية:
     - `POST /api/auth/register`
     - `POST /api/auth/login`
   - لم يتم بعد إضافة نقاط `logout` أو `profile`.

### ما لم يُنفذ بعد (Backend)
- استخدام BCrypt بدل SHA256 لتشفير كلمات المرور (حسب NFR-USER-001).
- تطبيق قفل الحساب بعد 5 محاولات فاشلة داخل `LoginAsync` وربطها بحقل `FailedLoginAttempts` في الكيان `User`.
- إنشاء JWT حقيقي باستخدام إعدادات مصادقة (AuthService منفصل أو تكامل مع ABP Identity).
- تسجيل محاولات الدخول الفاشلة (Logging + تخزين في الحقول المناسبة).
- إضافة نقطة نهاية لعرض الملف الشخصي وتحديثه (Profile).

## خطة التشغيل/التشغيلات القادمة
1. تحسين أمان كلمات المرور:
   - إضافة مكتبة BCrypt إن لزم أو تطبيق بديل آمن متوافق مع القيود.
   - تعديل `UserAppService` لاستخدام الخوارزمية الجديدة وتحديث المستخدمين الجدد فقط.
2. إضافة منطق قفل الحساب:
   - تحديث `LoginAsync` لزيادة `FailedLoginAttempts`، وقفل الحساب بعد 5 محاولات.
   - إعادة تعيين `FailedLoginAttempts` عند تسجيل الدخول الناجح.
3. إنشاء خدمة مصادقة / JWT:
   - إضافة إعدادات JWT إلى مشروع الـ backend.
   - إنشاء خدمة بسيطة لتوليد التوكن وتضمين `userId` و `email`.
   - استبدال توليد التوكن المؤقت في `UserAppService` باستدعاء هذه الخدمة.
4. إضافة Profile API أساسية:
   - `GET /api/users/profile` لعرض بيانات المستخدم الحالي.
   - `PUT /api/users/profile` لتحديث البيانات الأساسية في `UserProfile`.
5. تشغيل CI بعد اكتمال دفعة التعديلات القادمة والتأكد من نجاح البناء.

## ملاحظات
- الكود الحالي يوفر نقطة بداية عملية للتكامل مع الواجهة الأمامية لاحقًا (Angular).
- التركيز في هذا التشغيل كان على توفير API أساسية تتماشى مع مواصفات `specify.md` لنقاط `/api/auth/register` و `/api/auth/login`.
- يجب مراجعة اعتبارات الأمن بالكامل بعد إضافة JWT وBCrypt لضمان مطابقة NFR-USER-001.
