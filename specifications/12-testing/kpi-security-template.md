قالب
# KPI Template – Security / Permissions Feature

الهدف من هذا القالب هو تعريف **Definition of Done** لهذا النوع من الميزات.
استخدم هذه القائمة كمرجع للتحقق من أن الميزة نُفذت بجودة متسقة، وتشمل جميع الجوانب
اللازمة (Backend, Frontend, Docs, Testing) حسب ما ينطبق على هذا النوع.

هذا القالب خاص بميزات الأمان والصلاحيات (Authentication, Authorization, Policies).

## 1. معلومات عامة عن الميزة

- **FeatureId:** (مثال: FEAT-REPORTS-SALES)
- **FeatureName:** (مثال: Sales Reporting)
- **FeatureType:** (مثال: Reporting)
- **Primary Personas:** (مثال: Admin, Manager)
- **Related Requirements:** (IDs أو عناوين)
- **Spec File:** (مثال: specifications/18-features/feat-sales-reporting.md)

---

## 2. KPIs – Authentication

- [ ] آلية تسجيل الدخول/الخروج موثقة وواضحة.
- [ ] حماية الجلسات (Sessions) من الاختطاف قدر الإمكان.

## 3. KPIs – Authorization / Permissions

- [ ] تعريف Roles/Permissions واضحة في المواصفات.
- [ ] تطبيق سياسات الصلاحيات على مستوى API و/أو UI.
- [ ] عدم وجود Endpoints حرجة بدون حماية.

## 4. KPIs – Data Protection

- [ ] عدم ظهور بيانات حساسة لمستخدمين غير مصرح لهم.
- [ ] إخفاء الحقول الحساسة في الـ UI إن لم تكن ضرورية.

## 5. KPIs – Testing

- [ ] Unit Tests لقواعد التفويض (Policies).
- [ ] Integration Tests لسيناريوهات السماح/المنع (Allowed/Denied).
- [ ] (اختياري) اختبارات أمان يدوية أو باستخدام أدوات.

## 6. Definition of Done

- [ ] تم التحقق من أن المسارات الحرجة محمية.
- [ ] لا يوجد تعارض واضح بين ما هو موثّق في Roles Matrix وما هو مطبق فعليًا.
