قالب
# KPI Template – Integration Feature

الهدف من هذا القالب هو تعريف **Definition of Done** لهذا النوع من الميزات.
استخدم هذه القائمة كمرجع للتحقق من أن الميزة نُفذت بجودة متسقة، وتشمل جميع الجوانب
اللازمة (Backend, Frontend, Docs, Testing) حسب ما ينطبق على هذا النوع.

هذا القالب خاص بالتكامل مع أنظمة خارجية (APIs, Webhooks, Sync Jobs).

## 1. معلومات عامة عن الميزة

- **FeatureId:** (مثال: FEAT-REPORTS-SALES)
- **FeatureName:** (مثال: Sales Reporting)
- **FeatureType:** (مثال: Reporting)
- **Primary Personas:** (مثال: Admin, Manager)
- **Related Requirements:** (IDs أو عناوين)
- **Spec File:** (مثال: specifications/18-features/feat-sales-reporting.md)

---

## 2. KPIs – API Contract

- [ ] توثيق Endpoint الخارجي (URL, Methods, Headers, Auth).
- [ ] توثيق Request/Response Formats.
- [ ] تعريف واضح لأخطاء الطرف الثالث المحتملة وكيفية التعامل معها.

## 3. KPIs – Implementation

- [ ] وجود طبقة Integration/Client واحدة مسؤولة عن الاتصال بالنظام الخارجي.
- [ ] عدم تكرار استدعاءات HTTP مبعثرة في الكود.
- [ ] التعامل مع Timeouts و Retries عند الحاجة.

## 4. KPIs – Security

- [ ] تخزين مفاتيح/API Keys في إعدادات آمنة (Secret Store).
- [ ] عدم تسريب المفاتيح في الـ Logs أو الـ UI.

## 5. KPIs – Testing

- [ ] Unit Tests لمنطق التجميع قبل/بعد الاتصال (mapping, validation).
- [ ] Integration Tests مع Mock Server أو Sandbox للطرف الثالث.
- [ ] (اختياري) E2E Test من منظور المستخدم إن كان للتكامل أثر ملموس في الواجهة.

## 6. Definition of Done

- [ ] التكامل موثوق (لا يتسبب في تعطل النظام عند حدوث خطأ في الطرف الثالث).
- [ ] سهل الصيانة (مكان واحد لتعديل العقد إذا تغيّر).
