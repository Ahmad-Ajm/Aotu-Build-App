قالب
# KPI Template – CRUD Full-Stack Feature
#
# الهدف
# هذا القالب يعرّف "تعريف الانتهاء" (Definition of Done) لأي ميزة CRUD نمطية
# بحيث تتضمن دائمًا:
# - Backend
# - Frontend
# - Documentation
# - Testing (Unit + Integration + E2E)
#
# يجب أن تمر كل ميزة جديدة بهذه القائمة قبل اعتبارها منتهية.

## 1. معلومات عامة عن الميزة

- **FeatureId:** (مثال: FEAT-CITIES)
- **FeatureName:** (مثال: City Management)
- **EntityName:** (مثال: City)
- **Primary Personas:** (مثال: Admin)
- **Related Requirements:** (IDs أو عناوين)
- **Spec File:** (مثال: specifications/18-features/feat-cities.md)

---

## 2. KPIs – Backend

يجب أن تتحقق العناصر التالية:

- [ ] تم إنشاء **Entity** رئيسية باسم متسق داخل طبقة الدومين.
- [ ] تم إضافة **DbSet / Repository** للكيان.
- [ ] تم إنشاء **Application Service / Manager** يحتوي على منطق CRUD الأساسي:
  - [ ] Create
  - [ ] Read (GetById, GetList مع Paging/Sorting)
  - [ ] Update
  - [ ] Delete (Soft/Hard حسب السياسة)
- [ ] تم تعريف **DTOs** اللازمة:
  - [ ] Create DTO
  - [ ] Update DTO
  - [ ] List / Detail DTO
- [ ] تم إنشاء **Controller / API Endpoints** تدعم CRUD.
- [ ] تم تطبيق **Authorization / Permissions** على كل Endpoint.
- [ ] تم تطبيق **Validation أساسية** على المدخلات (Required, Length, Ranges، إلخ).
- [ ] تم ربط الميزة بـ **FeatureId** (إن أمكن) في مكان مناسب (تعليق، Tag، أو Mapping).

### مقاييس (KPIs) كمية مقترحة للـ Backend

- عدد Endpoints المنشأة: `>= 4` (Create, Get, List, Update, Delete).
- عدد اختبارات Unit المرتبطة بالـ Service: `>= 3` (Success + Failure + Edge Case).
- زمن الاستجابة في بيئة التطوير لعمليات القراءة الأساسية ضمن حدود مقبولة (مثال: < 200ms في الحالات البسيطة).

---

## 3. KPIs – Frontend

يجب أن تتوفر العناصر التالية:

- [ ] صفحة **List** تعرض بيانات الكيان في جدول/قائمة.
- [ ] صفحة **Create/Edit Form** تحتوي على الحقول الأساسية.
- [ ] تم تعريف **Routes** مناسبة (مثال: `/cities`, `/cities/create`, `/cities/:id/edit`).
- [ ] تم إنشاء **Service** على الفرونت يستهلك Endpoints الـ API.
- [ ] تم تطبيق **Validation في الواجهة** متسقة مع الـ Backend (Required, Min/Max، إلخ).
- [ ] واجهة الاستخدام تتبع **Guidelines** العامة للتصميم (Buttons, Colors, Layout).
- [ ] معالجة أخطاء الاستجابة من الـ API (Errors / Validation Messages).

### مقاييس (KPIs) كمية مقترحة للـ Frontend

- وجود **مسار واحد على الأقل** لعرض القائمة ومسار واحد للنموذج.
- تغطية التفاعلات الأساسية:
  - [ ] إنشاء عنصر جديد وظهوره في القائمة.
  - [ ] تعديل عنصر موجود وانعكاسه في القائمة.
  - [ ] حذف عنصر واختفاؤه من القائمة.

---

## 4. KPIs – Documentation

يجب أن يتم تحديث/إنشاء الوثائق التالية:

- [ ] تحديث ملف **Feature Spec** لهذه الميزة (مثال: `feat-cities.md`):
  - [ ] User Story
  - [ ] Acceptance Criteria
  - [ ] Business Rules الأساسية
- [ ] تحديث **API Documentation** (OpenAPI/Swagger أو ملف Markdown):
  - [ ] جميع Endpoints الخاصة بالميزة موثقة.
  - [ ] نماذج الطلب/الاستجابة موثقة.
- [ ] إذا كانت هناك تغييرات على الـ UI، يتم تحديث أي **UI Flow / Wireframe** (إن وجد).

### KPIs مقترحة

- لا توجد Endpoints مستخدمة في الكود غير موثقة.
- كل Acceptance Criteria مذكورة في الـ spec لها تغطية اختبارية أو تحقق يدوي واضح.

---

## 5. KPIs – Testing (Unit + Integration + E2E)

### 5.1 Unit Tests

- [ ] توجد اختبارات Unit للمنطق الأساسي في الـ Service/Manager:
  - [ ] نجاح إنشاء كيان صحيح.
  - [ ] فشل الإنشاء عند بيانات غير صالحة.
  - [ ] سيناريو حافة (Edge Case) واحد على الأقل.

### 5.2 Integration Tests

- [ ] توجد اختبارات Integration لـ API CRUD الأساسية:
  - [ ] Create → GetById يتحقق من الحفظ.
  - [ ] Update → GetById يتحقق من التعديل.
  - [ ] Delete → GetById أو List يتحقق من الإزالة أو التعطيل.
- [ ] تستخدم بيئة Test مستقلة (In-memory DB أو Test DB).

### 5.3 E2E Tests

- [ ] يوجد سيناريو End-to-End واحد على الأقل:
  - [ ] المستخدم يفتح صفحة القائمة.
  - [ ] يضغط "إضافة جديد".
  - [ ] يملأ النموذج.
  - [ ] يضغط حفظ.
  - [ ] يرى العنصر الجديد في القائمة.
- [ ] يفضل وجود سيناريو E2E لتعديل/حذف أيضًا إن أمكن.

### KPIs كمية

- نسبة اختبارات Unit/Integration الناجحة = 100% في CI.
- وجود **E2E واحد على الأقل** لكل Feature CRUD أساسية.

---

## 6. خلاصة Definition of Done للميزة

تعتبر الميزة **مكتملة (Done)** عندما:

- [ ] تم تنفيذ جميع عناصر Backend الإلزامية.
- [ ] تم تنفيذ جميع عناصر Frontend الإلزامية.
- [ ] تم تحديث الوثائق (Feature Spec + API Docs).
- [ ] تم تنفيذ اختبارات Unit + Integration + E2E وفق ما سبق.
- [ ] نجحت جميع الاختبارات في بيئة CI/CD (إن وجدت).

أي عنصر غير مكتمل يجب أن يُذكر بوضوح كـ:

- `Deferred` (مؤجل) مع سبب واضح،
- أو `Out of Scope` إذا اتُّفق على استبعاده صراحة لهذه الميزة.
