قالب
# نموذج المجال (Domain Model)

هذا الملف يصف نموذج المجال الكامل للمشروع وفق مبادئ Domain-Driven Design (DDD).

---

## الكيانات (Entities)

قائمة بجميع كيانات المجال مع تفاصيلها.

### قالب وصف الكيان

#### [اسم الكيان]

**الوصف**: [وصف مختصر للكيان ودوره في النظام]

**Properties**:
| Property | Type | Required | Max Length | Description |
|----------|------|----------|------------|-------------|
| Id | Guid/int | Yes | - | المعرف الفريد |
| [Property1] | [Type] | Yes/No | [Length] | [الوصف] |
| [Property2] | [Type] | Yes/No | [Length] | [الوصف] |
| CreationTime | DateTime | Yes | - | تاريخ الإنشاء (ABP) |
| CreatorId | Guid? | No | - | معرف المنشئ (ABP) |
| LastModificationTime | DateTime? | No | - | آخر تعديل (ABP) |
| LastModifierId | Guid? | No | - | معرف آخر معدل (ABP) |
| IsDeleted | bool | Yes | - | محذوف منطقياً (ABP Soft Delete) |
| DeletionTime | DateTime? | No | - | تاريخ الحذف |
| DeleterId | Guid? | No | - | معرف الحاذف |

**Navigation Properties**:
- [RelatedEntity]: [Relationship Type] - [وصف العلاقة]
  - Type: One-to-Many / Many-to-One / Many-to-Many / One-to-One
  - Foreign Key: [PropertyName]
  - On Delete: Cascade / Restrict / Set Null

**Interfaces Implemented**:
- IAggregateRoot (إذا كان Aggregate Root)
- IFullAuditedObject / IAuditedObject / ICreationAuditedObject
- ISoftDelete
- IMultiTenant (إذا كان multi-tenant)
- IHasExtraProperties (للـ Extra Properties في ABP)

**Business Rules**:
1. [قاعدة عمل 1]
2. [قاعدة عمل 2]
3. [قاعدة عمل 3]

**Invariants** (القواعد الثابتة):
- [Invariant 1]: [الشرط الذي يجب أن يكون صحيحاً دائماً]
- [Invariant 2]: [مثال: StartDate < EndDate]

**Domain Events**:
- [EventName]: يُطلق عند [الحدث]
- [EventName2]: يُطلق عند [الحدث]

---

## القيم (Value Objects)

القيم هي كائنات غير قابلة للتغيير (Immutable) وتُعرّف بقيمتها وليس بمعرف فريد.

### قالب وصف Value Object

#### [اسم Value Object]

**الوصف**: [وصف الغرض من هذا Value Object]

**Properties**:
| Property | Type | Required | Validation |
|----------|------|----------|------------|
| [Property1] | [Type] | Yes/No | [القاعدة] |
| [Property2] | [Type] | Yes/No | [القاعدة] |

**Validation Rules**:
1. [قاعدة التحقق 1]
2. [قاعدة التحقق 2]

**Factory Methods**:
- `Create([params])`: [وصف]
- `From([params])`: [وصف]

**Equality**:
- يُقارن بناءً على: [Properties المستخدمة في المقارنة]

---

## التجميعات (Aggregates)

التجميعات تمثل حدود الاتساق (Consistency Boundaries) في النموذج.

### قالب وصف Aggregate

#### [اسم Aggregate]

**Aggregate Root**: [اسم الكيان الجذر]

**Entities المتضمنة**:
1. [Entity1] (Root)
2. [Entity2] (Child)
3. [Entity3] (Child)

**Value Objects المستخدمة**:
- [ValueObject1]
- [ValueObject2]

**Consistency Boundary**:
[وصف حدود الاتساق - أي التغييرات التي يجب أن تحدث معاً كوحدة واحدة]

**Aggregate Rules**:
1. يجب الوصول إلى [Child Entities] فقط عبر [Root Entity]
2. [قاعدة 2]
3. [قاعدة 3]

**External References**:
[الـ Aggregates الأخرى التي يُشار إليها بالمعرف فقط]

---

## قواعد العمل (Business Rules)

قائمة شاملة بقواعد العمل في المجال.

### قواعد عامة

| ID | القاعدة | الموقع | الأولوية |
|----|---------|--------|----------|
| BR-001 | [وصف القاعدة] | [Entity/ValueObject] | High/Medium/Low |
| BR-002 | [وصف القاعدة] | [Entity/ValueObject] | High/Medium/Low |

### قواعد الحسابات

| ID | العملية الحسابية | الصيغة | الملاحظات |
|----|------------------|--------|-----------|
| CALC-001 | [اسم الحساب] | [الصيغة الرياضية] | [ملاحظات] |

### قواعد الحالة (State Transitions)

#### [اسم الكيان] - State Machine

```
[State1] → [Action] → [State2]
[State2] → [Action] → [State3]
[State3] → [Action] → [State1] (if applicable)
```

**Allowed Transitions**:
| From State | Action | To State | Conditions |
|------------|--------|----------|------------|
| [State1] | [Action] | [State2] | [الشروط] |
| [State2] | [Action] | [State3] | [الشروط] |

---

## العلاقات (Relationships)

### Entity Relationship Summary

| Parent Entity | Child Entity | Type | Cascade | Notes |
|---------------|--------------|------|---------|-------|
| [Entity1] | [Entity2] | One-to-Many | Yes/No | [ملاحظات] |
| [Entity3] | [Entity4] | Many-to-Many | No | [ملاحظات] |

### Many-to-Many Junction Tables

| Table Name | Entity A | Entity B | Extra Properties |
|------------|----------|----------|------------------|
| [JunctionTable] | [EntityA] | [EntityB] | [Properties] |

---

## Domain Services

الخدمات التي تحتوي على منطق عمل لا ينتمي لكيان واحد.

### [اسم Domain Service]

**الغرض**: [وصف الغرض من هذه الخدمة]

**Methods**:
| Method | Input | Output | Description |
|--------|-------|--------|-------------|
| [MethodName] | [Params] | [ReturnType] | [الوصف] |

**Business Logic**:
[وصف المنطق الذي تنفذه هذه الخدمة]

---

## Specifications Pattern

مواصفات إعادة استخدام معايير الاستعلام.

### [اسم Specification]

**الغرض**: [وصف الغرض]

**Criteria**:
```
[الشروط المنطقية]
```

**Usage**:
```
var spec = new [SpecificationName]([params]);
var results = repository.Where(spec);
```

---

## المجالات الفرعية (Subdomains)

تقسيم المجال إلى مجالات فرعية.

### Core Domain
[المجال الأساسي - الميزة التنافسية الرئيسية]

### Supporting Subdomains
1. [مجال مساند 1]
2. [مجال مساند 2]

### Generic Subdomains
1. [مجال عام 1]
2. [مجال عام 2]

---

## Bounded Contexts

حدود السياق في النظام.

### [اسم Bounded Context]

**الكيانات المتضمنة**:
- [Entity1]
- [Entity2]

**Ubiquitous Language**:
[المصطلحات الخاصة بهذا السياق]

**Integration Points**:
[نقاط التكامل مع Bounded Contexts الأخرى]

---

## ملاحظات التنفيذ

- استخدام ABP Framework للـ Auditing و Soft Delete تلقائياً
- جميع الكيانات تُورث من `Entity<TKey>` أو `AggregateRoot<TKey>`
- استخدام Repository Pattern للوصول للبيانات
- تطبيق Domain Events باستخدام ABP Local Events أو Distributed Events

---

## Diagrams

### Entity Relationship Diagram (ERD)
[رابط أو تضمين ERD]

### Domain Model Class Diagram
[رابط أو تضمين Class Diagram]

### State Machine Diagrams
[رابط أو تضمين State Diagrams للكيانات ذات الحالات]
