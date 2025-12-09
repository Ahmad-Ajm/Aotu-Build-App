قالب
# Entity Template

قالب لتوثيق كيان جديد (Entity).

---

## [اسم الكيان]

**الوصف**: [وصف مختصر للكيان ودوره في النظام]

**Module**: [اسم الـ Module]

---

### Properties

| Property | Type | Required | Max Length | Default | Description |
|----------|------|----------|------------|---------|-------------|
| Id | Guid | Yes | - | Auto | المعرف الفريد |
| [Property1] | string | Yes | 200 | null | [الوصف] |
| [Property2] | int | No | - | 0 | [الوصف] |
| CreationTime | DateTime | Yes | - | Now | تاريخ الإنشاء (ABP) |
| CreatorId | Guid? | No | - | null | معرف المنشئ (ABP) |
| LastModificationTime | DateTime? | No | - | null | آخر تعديل (ABP) |
| LastModifierId | Guid? | No | - | null | معرف آخر معدل (ABP) |
| IsDeleted | bool | Yes | - | false | محذوف منطقياً (ABP) |
| DeletionTime | DateTime? | No | - | null | تاريخ الحذف |
| DeleterId | Guid? | No | - | null | معرف الحاذف |

---

### Navigation Properties

| Property | Related Entity | Relationship Type | Foreign Key | On Delete |
|----------|----------------|-------------------|-------------|-----------|
| [RelatedEntity] | [EntityName] | One-to-Many | [FKName] | Cascade |

---

### Interfaces Implemented

- [ ] IAggregateRoot (إذا كان Aggregate Root)
- [ ] IFullAuditedObject (Create + Update + Delete auditing)
- [ ] IAuditedObject (Create + Update auditing only)
- [ ] ICreationAuditedObject (Create auditing only)
- [ ] ISoftDelete (Soft delete support)
- [ ] IMultiTenant (Multi-tenancy support)
- [ ] IHasExtraProperties (Extra properties dictionary)

---

### Business Rules

1. [قاعدة عمل 1]
2. [قاعدة عمل 2]
3. [قاعدة عمل 3]

---

### Invariants

- [Invariant 1]: [الشرط الذي يجب أن يكون صحيحاً دائماً]
- [Invariant 2]: [مثال: StartDate < EndDate]

---

### Domain Events

- [ ] [EventName]Created: يُطلق عند إنشاء [الكيان]
- [ ] [EventName]Updated: يُطلق عند تحديث [الكيان]
- [ ] [EventName]Deleted: يُطلق عند حذف [الكيان]
- [ ] [EventName][CustomEvent]: [وصف الحدث]

---

### State Machine (إذا كان ذو حالات)

**States**:
- [State1]
- [State2]
- [State3]

**Transitions**:
| From | Action | To | Conditions |
|------|--------|----|------------|
| [State1] | [Action] | [State2] | [الشروط] |
| [State2] | [Action] | [State3] | [الشروط] |

---

### Database Schema

**Table Name**: `App[EntityName]s`

**Indexes**:
```sql
CREATE INDEX IX_[EntityName]_[Property] ON App[EntityName]s([Property]);
```

**Constraints**:
```sql
ALTER TABLE App[EntityName]s 
ADD CONSTRAINT CK_[EntityName]_[Rule] CHECK ([Condition]);
```

---

### API Endpoints

- [ ] GET /api/app/[entity-name]
- [ ] GET /api/app/[entity-name]/{id}
- [ ] POST /api/app/[entity-name]
- [ ] PUT /api/app/[entity-name]/{id}
- [ ] DELETE /api/app/[entity-name]/{id}

---

### Permissions

- `[Module].[Entity].Create`
- `[Module].[Entity].Edit`
- `[Module].[Entity].Delete`
- `[Module].[Entity].View`

---

### Notes

- [ملاحظة تنفيذية 1]
- [ملاحظة تنفيذية 2]

