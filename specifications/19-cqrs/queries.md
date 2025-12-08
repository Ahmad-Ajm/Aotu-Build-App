# Queries (CQRS)

الاستعلامات (Queries) هي طلبات لقراءة البيانات دون تغيير الحالة.

---

## مبادئ Queries

- Query يقرأ البيانات فقط (Read-Only)
- Query لا يُغير الحالة
- Query يمكن أن يُنفذ مرات متعددة بنفس النتيجة
- Query يمكن أن يُخزن مؤقتاً (Cacheable)

---

## قالب Query

### [QueryName]Query

**الوصف**: [وصف الاستعلام والغرض منه]

**Module**: [اسم الـ Module]

**Parameters**:
| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| [Param1] | [Type] | Yes/No | [الوصف] |
| [Param2] | [Type] | Yes/No | [الوصف] |

**Query DTO**:
```csharp
public class [QueryName]Query
{
    public [Type] [Param1] { get; set; }
    public [Type] [Param2] { get; set; }
    
    // Pagination
    public int SkipCount { get; set; }
    public int MaxResultCount { get; set; }
    
    // Sorting
    public string Sorting { get; set; }
}
```

**Handler**:
```csharp
public class [QueryName]QueryHandler : 
    IQueryHandler<[QueryName]Query, [ResultType]>
{
    public async Task<[ResultType]> Handle(
        [QueryName]Query query, 
        CancellationToken cancellationToken)
    {
        // Read data
        return result;
    }
}
```

**Result Type**:
```csharp
public class [ResultType]
{
    public [Type] [Property1] { get; set; }
    public [Type] [Property2] { get; set; }
}
```

**Caching**:
- **Enabled**: Yes / No
- **Duration**: [مدة الـ Cache]
- **Cache Key**: [pattern]

**Performance**:
- **Expected Response Time**: [< Xms]
- **Max Result Size**: [عدد السجلات]

---

## قائمة Queries

### [Module/Aggregate] Queries

#### List Queries
- Get[Entity]ListQuery
- GetFiltered[Entity]ListQuery
- Search[Entity]Query

#### Detail Queries
- Get[Entity]By IdQuery
- Get[Entity]DetailsQuery
- Get[Entity]WithRelationsQuery

#### Aggregate Queries
- Get[Entity]StatisticsQuery
- Get[Entity]SummaryQuery
- Get[Entity]CountQuery

#### Lookup Queries
- Get[Entity]LookupQuery (for dropdowns)
- Get[Entity]AutocompleteQuery

---

## Query Optimization

### Projection
استخدام Projection لجلب الحقول المطلوبة فقط:
```csharp
query.Select(x => new Dto {
    Id = x.Id,
    Name = x.Name
    // Only required fields
});
```

### Eager Loading
تحميل العلاقات مسبقاً إذا لزم الأمر:
```csharp
query.Include(x => x.RelatedEntity)
     .ThenInclude(x => x.NestedRelation);
```

### Indexes
التأكد من وجود Indexes على:
- Foreign Keys
- Fields المستخدمة في Filtering
- Fields المستخدمة في Sorting

---

## Pagination

### Standard Pagination
```csharp
public class PagedResult<T>
{
    public int TotalCount { get; set; }
    public List<T> Items { get; set; }
}
```

### Cursor-based Pagination
للقوائم الكبيرة جداً:
```csharp
public class CursorPagedResult<T>
{
    public string NextCursor { get; set; }
    public bool HasMore { get; set; }
    public List<T> Items { get; set; }
}
```

---

## Filtering & Sorting

### Dynamic Filtering
```csharp
public class FilterCriteria
{
    public string Field { get; set; }
    public string Operator { get; set; } // eq, ne, gt, lt, contains
    public string Value { get; set; }
}
```

### Dynamic Sorting
```csharp
// Example: "name ASC, createdDate DESC"
public string Sorting { get; set; }
```

---

## Read Models (إذا مُستخدم)

لتحسين أداء الاستعلامات المعقدة، يمكن إنشاء Read Models منفصلة.

### [ReadModelName]

**Purpose**: [الغرض من هذا Read Model]

**Source**: [من أين يُبنى]

**Update Frequency**: Real-time / Periodic / On-demand

**Schema**:
```csharp
public class [ReadModelName]
{
    // Denormalized data
}
```

---

## Caching Strategy

| Query Type | Cache Duration | Invalidation |
|------------|----------------|--------------|
| Static Data | 1 hour | Manual / On Update |
| Dynamic Data | 5 minutes | On Change Event |
| User-Specific | 1 minute | On User Action |

**Cache Keys Pattern**:
```
[Module]:[Entity]:[QueryType]:[Parameters]
```

---

## Best Practices

1. **تسمية**: Get[Entity], List[Entity], Search[Entity]
2. **Projection**: جلب الحقول المطلوبة فقط
3. **Pagination**: إجبارية للقوائم
4. **Caching**: للبيانات الثابتة أو البطيئة
5. **Async**: استخدام Async/Await دائماً

---

## ملاحظات التنفيذ

- استخدام MediatR أو ABP Application Services
- Query Handlers تُسجل تلقائياً في DI
- استخدام IQueryable للاستعلامات الديناميكية
- Redis أو Memory Cache للتخزين المؤقت

