قالب
# KPI Template – AI Feature

الهدف من هذا القالب هو تعريف **Definition of Done** لهذا النوع من الميزات.
استخدم هذه القائمة كمرجع للتحقق من أن الميزة نُفذت بجودة متسقة، وتشمل جميع الجوانب
اللازمة (Backend, Frontend, Docs, Testing) حسب ما ينطبق على هذا النوع.

هذا القالب خاص بميزات الذكاء الاصطناعي (Chat, RAG, Recommendations, Classifications...).

## 1. معلومات عامة عن الميزة

- **FeatureId:** (مثال: FEAT-REPORTS-SALES)
- **FeatureName:** (مثال: Sales Reporting)
- **FeatureType:** (مثال: Reporting)
- **Primary Personas:** (مثال: Admin, Manager)
- **Related Requirements:** (IDs أو عناوين)
- **Spec File:** (مثال: specifications/18-features/feat-sales-reporting.md)

---

## 2. KPIs – Problem & Data

- [ ] تعريف واضح للمشكلة التي يحلّها الـ AI (Use Case).
- [ ] تعريف نوع البيانات المستخدمة (نصوص، صور، سجلات...).
- [ ] تعريف مصدر الحقيقة (Ground Truth) إن وجد.

## 3. KPIs – Model & Pipeline

- [ ] تحديد النموذج المستخدم (Model name/version).
- [ ] توثيق الـ Pipeline (Preprocessing → Model → Postprocessing).
- [ ] سياسة الـ Prompts (Templates, System Prompts, Safety Prompts).
- [ ] وجود RAG/Vector DB عند الحاجة مع توثيق طريقة الاسترجاع.

## 4. KPIs – Quality & Evaluation

- [ ] تعريف معايير جودة (Accuracy, Relevance, Latency...).
- [ ] وجود طريقة اختبار (Manual review set أو Test set).
- [ ] مراقبة الأداء بمرور الوقت (Logs/metrics).

## 5. KPIs – Safety & Compliance

- [ ] الالتزام بسياسات المحتوى (Safety, Privacy).
- [ ] عدم تسريب بيانات حساسة إلى نموذج خارجي غير مصرح به.

## 6. Testing

- [ ] اختبارات وحدة للأجزاء غير النموذجية (pre/post processing).
- [ ] اختبارات تكامل لجزء الـ RAG/DB.
- [ ] سيناريوهات E2E تحاكي تفاعل المستخدم الحقيقي.

## 7. Definition of Done

- [ ] تعطي الميزة قيمة فعلية وواضحة للمستخدم.
- [ ] تم اختبارها على مجموعة حالات حقيقية أو شبه حقيقية.
