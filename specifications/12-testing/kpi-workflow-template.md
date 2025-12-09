قالب
# KPI Template – Workflow / Approval Feature

الهدف من هذا القالب هو تعريف **Definition of Done** لهذا النوع من الميزات.
استخدم هذه القائمة كمرجع للتحقق من أن الميزة نُفذت بجودة متسقة، وتشمل جميع الجوانب
اللازمة (Backend, Frontend, Docs, Testing) حسب ما ينطبق على هذا النوع.

هذا القالب خاص بميزات الموافقات وتعدد الحالات (Workflows, State Machines, Approvals).

## 1. معلومات عامة عن الميزة

- **FeatureId:** (مثال: FEAT-REPORTS-SALES)
- **FeatureName:** (مثال: Sales Reporting)
- **FeatureType:** (مثال: Reporting)
- **Primary Personas:** (مثال: Admin, Manager)
- **Related Requirements:** (IDs أو عناوين)
- **Spec File:** (مثال: specifications/18-features/feat-sales-reporting.md)

---

## 2. KPIs – Domain / States

- [ ] تعريف جميع الحالات الممكنة (States) بوضوح.
- [ ] تعريف جميع الانتقالات (Transitions) بين الحالات.
- [ ] تعريف من يملك صلاحية كل انتقال (Role/Persona).
- [ ] تعريف القواعد (Business Rules) لكل حالة/انتقال.

## 3. KPIs – Backend Logic

- [ ] تطبيق منطق الحالات (State Machine) بطريقة واضحة (حالة لكل عنصر).
- [ ] منع الانتقالات غير المسموحة (Invalid Transitions).
- [ ] إطلاق أحداث/إشعارات عند الحالات المهمة (Approved, Rejected...).

## 4. KPIs – Frontend / UX

- [ ] واجهة تظهر الحالة الحالية بوضوح.
- [ ] أزرار أو أفعال (Actions) لكل انتقال متاح للمستخدم الحالي.
- [ ] رسائل تأكيد عند العمليات الحرجة (Reject, Cancel...).

## 5. KPIs – Documentation

- [ ] مخطط حالات (State Diagram) أو جدول حالات/انتقالات.
- [ ] وصف نصي لكل حالة ومتى تُستخدم.

## 6. KPIs – Testing

- [ ] Unit Tests لكل انتقال مهم (Success + Invalid).
- [ ] Integration Tests لسير واحد على الأقل من البداية للنهاية.
- [ ] (اختياري) E2E Test سيناريو كامل Approval.

## 7. Definition of Done

- [ ] لا يمكن أن تكون الحالة في وضع غير معروف.
- [ ] جميع المسارات الحرجة في الـ workflow مغطاة باختبارات.
