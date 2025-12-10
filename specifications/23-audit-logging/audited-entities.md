قالب
# Audit Logging

تسجيل الأنشطة (Audit Logging) لتتبع التغييرات في النظام.

---

## Auditing Strategy

### Auditing Levels

#### Full Auditing
- جميع العمليات: Create, Update, Delete
- يشمل: User, Time, Old Value, New Value
- للكيانات الحساسة والمهمة

#### Creation Auditing
- تسجيل من أنشأ ومتى فقط
- للكيانات العادية

#### Modification Auditing
- تسجيل التعديلات فقط
- للكيانات التي تتغير كثيراً

#### No Auditing
- للكيانات غير المهمة أو المؤقتة

---

## قالب Audited Entity

### [EntityName]

**Auditing Level**: Full / Creation / Modification / None

**Interfaces Implemented**:
```csharp
public class [EntityName] : 
    FullAuditedAggregateRoot<Guid>, // Full Auditing
    IMultiTenant                      // Multi-tenant support
{
    // Properties
}
```

**Audited Fields**:
| Field | Old Value Tracked | Reason |
|-------|-------------------|--------|
| [Field1] | Yes | [السبب] |
| [Field2] | Yes | [السبب] |
| [Field3] | No | غير حساس |

**Sensitive Data**:
- [قائمة الحقول الحساسة التي تحتاج Masking في Audit Log]

**Retention Period**: [المدة قبل الأرشفة/الحذف]

---

## قائمة Audited Entities

### Full Auditing (جميع العمليات)

#### User-related
- User
- UserRole
- UserPermission

#### Financial
- Invoice
- Payment
- Transaction

#### Critical Business Entities
- [Entity1]
- [Entity2]

### Creation Auditing Only

#### Log Entities
- [Entity1]
- [Entity2]

### No Auditing

#### Temporary/Cache Entities
- [Entity1]
- [Entity2]

---

## Audit Log Schema

```csharp
public class AuditLog
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string UserName { get; set; }
    public string TenantId { get; set; }
    public DateTime ExecutionTime { get; set; }
    public int ExecutionDuration { get; set; }
    public string ClientIpAddress { get; set; }
    public string BrowserInfo { get; set; }
    public string HttpMethod { get; set; }
    public string Url { get; set; }
    public string ServiceName { get; set; }
    public string MethodName { get; set; }
    public string Parameters { get; set; }
    public int? HttpStatusCode { get; set; }
    public string Exception { get; set; }
    public List<EntityChange> EntityChanges { get; set; }
}

public class EntityChange
{
    public Guid Id { get; set; }
    public DateTime ChangeTime { get; set; }
    public string ChangeType { get; set; } // Created, Updated, Deleted
    public string EntityId { get; set; }
    public string EntityTypeFullName { get; set; }
    public List<PropertyChange> PropertyChanges { get; set; }
}

public class PropertyChange
{
    public string PropertyName { get; set; }
    public string PropertyTypeFullName { get; set; }
    public string OriginalValue { get; set; }
    public string NewValue { get; set; }
}
```

---

## What to Audit

### User Actions
- Login/Logout
- Password Change
- Permission Changes
- Profile Updates

### Data Changes
- Create new records
- Update existing records
- Delete records (Soft/Hard)

### Administrative Actions
- Configuration Changes
- System Settings Updates
- User Management

### Security Events
- Failed Login Attempts
- Permission Denied
- Suspicious Activities

---

## Audit Trail UI

### Features
- Search by: User, Entity, Date Range, Action
- Filter by: Entity Type, Change Type
- View: Old Value vs New Value (Side-by-side)
- Export: Excel, PDF, CSV
- Timeline View: لرؤية تسلسل الأحداث

---

## Data Retention

| Category | Retention Period | Archive Strategy |
|----------|------------------|------------------|
| User Actions | 1 year | Move to Archive DB |
| Data Changes | 2 years | Move to Archive DB |
| Security Events | 5 years | Move to Cold Storage |
| System Logs | 3 months | Delete |

---

## Performance Considerations

### Async Logging
- تسجيل Audit Logs بشكل غير متزامن
- استخدام Queue لتجنب التأثير على الأداء

### Indexing
```sql
CREATE INDEX IX_AuditLogs_UserId ON AuditLogs(UserId);
CREATE INDEX IX_AuditLogs_ExecutionTime ON AuditLogs(ExecutionTime);
CREATE INDEX IX_AuditLogs_EntityTypeFullName ON EntityChanges(EntityTypeFullName);
```

### Partitioning
- تقسيم جدول Audit Logs حسب التاريخ (شهرياً)

---

## Compliance

### GDPR
- Right to Access: المستخدم يمكنه رؤية Audit Log الخاص به
- Right to Erasure: حذف Audit Logs عند حذف المستخدم

### SOC 2
- Logging جميع التغييرات على البيانات الحساسة
- Immutable Audit Logs (لا يمكن تعديلها)

---

## Sensitive Data Masking

بعض الحقول تُخفى (Masked) في Audit Log:

| Field Type | Masking Strategy |
|------------|------------------|
| Password | Never log |
| Credit Card | Show last 4 digits only |
| Email | ma***@example.com |
| Phone | +966 5X XXX XX56 |
| SSN | XXX-XX-1234 |

---

## Best Practices

1. **Selective Auditing**: ليس كل شيء يحتاج Audit
2. **Async Logging**: لا تؤثر على الأداء
3. **Data Masking**: للبيانات الحساسة
4. **Retention Policy**: حذف القديم بانتظام
5. **Monitoring**: مراقبة حجم Audit Logs

---

## ملاحظات التنفيذ

- ABP Framework يوفر Audit Logging جاهز
- استخدام IFullAuditedObject / IAuditedObject
- تسجيل تلقائي عند استخدام Repository Pattern
- استخدام Background Job لتنظيف البيانات القديمة

