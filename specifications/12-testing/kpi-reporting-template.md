قالب
# KPI Template – Reporting / Analytics Feature

الهدف من هذا القالب هو تعريف **Definition of Done** لهذا النوع من الميزات.
استخدم هذه القائمة كمرجع للتحقق من أن الميزة نُفذت بجودة متسقة، وتشمل جميع الجوانب
اللازمة (Backend, Frontend, Docs, Testing) حسب ما ينطبق على هذا النوع.

هذا القالب خاص بميزات التقارير واللوحات (Reports, Dashboards, Analytics).

## 1. معلومات عامة عن الميزة

- **FeatureId:** (مثال: FEAT-REPORTS-SALES)
- **FeatureName:** (مثال: Sales Reporting)
- **FeatureType:** (مثال: Reporting)
- **Primary Personas:** (مثال: Admin, Manager)
- **Related Requirements:** (IDs أو عناوين)
- **Spec File:** (مثال: specifications/18-features/feat-sales-reporting.md)

---

## 2. KPIs – Backend / Data

- [ ] تعريف واضح لمصادر البيانات (Entities, Views, External Sources).
- [ ] Queries أو Aggregations مكتوبة بشكل واضح (مثال: LINQ, SQL, Stored Procedures).
- [ ] دعم فلاتر التاريخ (من/إلى) إن كانت ذات معنى.
- [ ] دعم Pagination أو Limit لمنع تحميل بيانات ضخمة دفعة واحدة.
- [ ] المؤشرات/الحقول الأساسية للتقرير موثقة بوضوح (KPIs / Measures).

## 3. KPIs – Frontend / Visualization

- [ ] شاشة تقرير أو Dashboard تعرض البيانات المطلوبة.
- [ ] عناصر عرض مناسبة (Table / Chart / Cards).
- [ ] فلاتر واضحة (Date, Category, Status...) مع Labels مفهومة.
- [ ] توضيح وحدات الأرقام (Currency, Percentage, Count...).
- [ ] أداء مقبول عند تغيير الفلاتر (لا تنتظر > 3–5 ثوانٍ في الحالات الشائعة).

## 4. KPIs – Documentation

- [ ] وصف واضح لما يقدمه التقرير (ماذا يعني كل رقم؟).
- [ ] تعريف لكل KPI/Metric مستخدم في التقرير.
- [ ] أمثلة على سيناريوهات استخدام التقرير (من سيستخدمه ولماذا؟).

## 5. KPIs – Testing

- [ ] Unit Tests لجزء التجميع المنطقي (Calculations, Aggregations).
- [ ] Integration Tests للتأكد من أن الفلاتر تعمل كما هو متوقع.
- [ ] (اختياري) Snapshot Tests أو Screenshots مرجعية عند الحاجة.

## 6. Definition of Done

الميزة مكتملة عندما:
- [ ] يتمكن المستخدم المستهدف من استخدام التقرير لتحقيق القرار المطلوب.
- [ ] النتائج متسقة مع بيانات النظام الفعلية ضمن هامش خطأ مقبول.
- [ ] الأداء وتجربة المستخدم مقبولان.
