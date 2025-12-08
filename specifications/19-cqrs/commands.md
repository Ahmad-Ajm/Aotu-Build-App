# Commands (CQRS)

الأوامر (Commands) هي طلبات لتغيير حالة النظام.

---

## مبادئ Commands

- Command يُمثل نية (Intent) لفعل شيء
- Command يجب أن يكون Imperative (أمر)
- Command يُعالج بواسطة Handler واحد فقط
- Command قد ينجح أو يفشل

---

## قالب Command

### [CommandName]Command

**الوصف**: [وصف الأمر والغرض منه]

**Module**: [اسم الـ Module]

**Properties**:
| Property | Type | Required | Validation |
|----------|------|----------|------------|
| [Property1] | [Type] | Yes/No | [القاعدة] |
| [Property2] | [Type] | Yes/No | [القاعدة] |

**Command DTO**:
```csharp
public class [CommandName]Command
{
    public [Type] [Property1] { get; set; }
    public [Type] [Property2] { get; set; }
    
    // Validation
    public void Validate()
    {
        // Validation logic
    }
}
```

**Handler**:
```csharp
public class [CommandName]CommandHandler : 
    ICommandHandler<[CommandName]Command, [ResultType]>
{
    public async Task<[ResultType]> Handle(
        [CommandName]Command command, 
        CancellationToken cancellationToken)
    {
        // Implementation
        return result;
    }
}
```

**Business Rules Enforced**:
1. [قاعدة عمل 1]
2. [قاعدة عمل 2]

**Events Raised**:
- [EventName]: عند النجاح
- [EventName2]: عند حالة معينة

**Exceptions**:
| Exception | Scenario | HTTP Status |
|-----------|----------|-------------|
| [ExceptionName] | [السيناريو] | 400 / 409 / 500 |

---

## قائمة Commands

### [Module/Aggregate] Commands

#### Create Commands
- Create[Entity]Command
- Bulk Create[Entity]Command

#### Update Commands
- Update[Entity]Command
- Update[Entity]Status Command
- Partial Update[Entity]Command

#### Delete Commands
- Delete[Entity]Command
- Soft Delete[Entity]Command
- Bulk Delete[Entity]Command

#### Business Logic Commands
- [SpecificBusinessAction]Command
- [AnotherBusinessAction]Command

---

## Command Validation

### Validation Levels
1. **Syntax Validation**: Data Annotations
2. **Business Rules Validation**: في Handler
3. **Authorization**: Permissions

**Validation Pipeline**:
```
Request → Validate Input → Check Permissions → Execute Command → Raise Events
```

---

## Command Result

### Success Result
```csharp
public class CommandResult<T>
{
    public bool Success { get; set; }
    public T Data { get; set; }
    public string Message { get; set; }
}
```

### Failure Result
```csharp
public class CommandResult
{
    public bool Success { get; set; } // false
    public string ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public Dictionary<string, string[]> ValidationErrors { get; set; }
}
```

---

## Idempotency

Commands يجب أن تكون Idempotent حيثما أمكن.

**Strategies**:
- استخدام Idempotency Key في Header
- التحقق من الحالة قبل التنفيذ
- استخدام Unique Constraints في DB

---

## Best Practices

1. **تسمية**: استخدم أفعال أمر: Create, Update, Delete, Process
2. **حجم**: Command يجب أن يكون صغيراً ومركزاً
3. **Immutability**: Commands يجب أن تكون Immutable بعد الإنشاء
4. **Validation**: التحقق من جميع المدخلات
5. **Events**: إطلاق Events بعد النجاح

---

## ملاحظات التنفيذ

- استخدام MediatR أو ABP Application Services
- Command Handlers تُسجل تلقائياً في DI
- استخدام Unit of Work لضمان Consistency
- Logging لكل Command

