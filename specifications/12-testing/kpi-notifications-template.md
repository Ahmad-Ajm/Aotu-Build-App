قالب
# KPI Template – Notifications Feature

الهدف من هذا القالب هو تعريف **Definition of Done** لهذا النوع من الميزات.
استخدم هذه القائمة كمرجع للتحقق من أن الميزة نُفذت بجودة متسقة، وتشمل جميع الجوانب
اللازمة (Backend, Frontend, Docs, Testing) حسب ما ينطبق على هذا النوع.

هذا القالب خاص بميزات الإشعارات (Email, SMS, Push, In-App).

## 1. معلومات عامة عن الميزة

- **FeatureId:** (مثال: FEAT-REPORTS-SALES)
- **FeatureName:** (مثال: Sales Reporting)
- **FeatureType:** (مثال: Reporting)
- **Primary Personas:** (مثال: Admin, Manager)
- **Related Requirements:** (IDs أو عناوين)
- **Spec File:** (مثال: specifications/18-features/feat-sales-reporting.md)

---

## 2. KPIs – Triggers & Rules

- [ ] تعريف جميع الأحداث التي تولّد إشعارات (Triggers).
- [ ] تعريف نوع الإشعار المطلوب لكل حدث (Email/SMS/Push/In-App).
- [ ] تعريف جمهور الإشعار (Recipient personas أو segments).

## 3. KPIs – Delivery Logic

- [ ] وجود طبقة إرسال موحدة (Notification Service).
- [ ] دعم قنوات متعددة عند الحاجة (Email + In-App، مثلاً).
- [ ] التعامل مع الأخطاء (Retries, Dead-letter queue إن كانت مطلوبة).

## 4. KPIs – Templates / Content

- [ ] قوالب نصية أو HTML للإشعارات المهمة.
- [ ] دعم اللغات المطلوبة (i18n) إن كان النظام متعدد اللغات.
- [ ] التحقق من أن المحتوى لا يحتوي معلومات حساسة غير ضرورية.

## 5. KPIs – Testing

- [ ] Unit Tests لمنطق اختيار القناة والمستلم.
- [ ] Integration Tests مع مزود الخدمة (أو Mock).
- [ ] (اختياري) E2E Test واحد على الأقل:
  - حدث معين → إشعار يصل للمستخدم (على الأقل في بيئة Staging).

## 6. Definition of Done

- [ ] جميع الأحداث المهمة في المواصفات لديها إشعار/قرار واضح (إرسال/عدم إرسال).
- [ ] لا يتم إرسال إشعارات متكررة بشكل مزعج أو غير منضبط.
