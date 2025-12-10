قالب
# Domain Events

الأحداث على مستوى المجال (Domain Events) هي أحداث تُطلق عند حدوث تغيير مهم في الحالة داخل المجال.

---

## مفهوم Domain Events

Domain Events تُستخدم لـ:
- فصل الاهتمامات (Decoupling) بين الأجزاء المختلفة
- تنفيذ Side Effects بطريقة منظمة
- الحفاظ على Aggregate Boundaries
- تسهيل Event Sourcing (إذا كان مستخدماً)

---

## قالب Domain Event

### [EventName]Event

**الوصف**: [وصف الحدث ومتى يُطلق]

**Trigger**: [الإجراء الذي يُطلق هذا الحدث]

**Properties**:
| Property | Type | Description |
|----------|------|-------------|
| [Property1] | [Type] | [الوصف] |
| [Property2] | [Type] | [الوصف] |

**Event Data**:
```csharp
public class [EventName]Event
{
    public Guid Id { get; set; }
    public [Type] [Property] { get; set; }
    public DateTime OccurredOn { get; set; }
    // ... additional properties
}
```

**Handlers**:
| Handler | Purpose | Type |
|---------|---------|------|
| [HandlerName] | [الغرض] | Local/Distributed |

**Usage**:
```csharp
// Raising the event
await _distributedEventBus.PublishAsync(new [EventName]Event
{
    Id = entity.Id,
    // ... properties
});
```

---

## قائمة Domain Events

### [Module/Aggregate] Events

#### 1. [EventName]Created

**الوصف**: يُطلق عند إنشاء [كيان] جديد

**Properties**:
- Id: Guid
- [Properties...]

**Handlers**:
1. SendNotificationHandler: إرسال إشعار
2. UpdateStatisticsHandler: تحديث الإحصائيات
3. LogAuditHandler: تسجيل في Audit Log

#### 2. [EventName]Updated

**الوصف**: يُطلق عند تحديث [كيان]

**Properties**:
- Id: Guid
- OldValue: [Type]
- NewValue: [Type]
- ChangedBy: Guid

**Handlers**:
1. InvalidateCacheHandler: تحديث الـ Cache
2. NotifyWatchersHandler: إشعار المتابعين

#### 3. [EventName]Deleted

**الوصف**: يُطلق عند حذف [كيان]

**Properties**:
- Id: Guid
- DeletedBy: Guid
- DeletionReason: string

**Handlers**:
1. CleanupRelatedDataHandler: تنظيف البيانات المرتبطة
2. ArchiveHandler: أرشفة

#### 4. [EventName]StatusChanged

**الوصف**: يُطلق عند تغيير حالة [كيان]

**Properties**:
- Id: Guid
- OldStatus: [StatusEnum]
- NewStatus: [StatusEnum]
- ChangedBy: Guid
- Reason: string

**Handlers**:
1. WorkflowHandler: تشغيل سير العمل التالي
2. NotificationHandler: إشعار المعنيين

---

## Event Ordering

**ترتيب الأحداث**:
1. [Event1]
2. [Event2]
3. [Event3]

**ملاحظة**: بعض الأحداث يجب أن تُعالج بترتيب معين.

---

## Local vs Distributed Events

### Local Events
- تُستخدم داخل نفس التطبيق
- معالجة سريعة (In-Process)
- استخدام ABP Local Event Bus

### Distributed Events
- تُستخدم عبر Microservices
- معالجة غير متزامنة (Async)
- استخدام RabbitMQ / Kafka / Azure Service Bus

**متى نستخدم أيهما**:
| Scenario | Type |
|----------|------|
| تحديث Cache | Local |
| إرسال Email | Distributed |
| تحديث إحصائيات داخلية | Local |
| مزامنة بين خدمات | Distributed |

---

## Event Handlers

### قالب Event Handler

```csharp
public class [EventName]EventHandler : 
    ILocalEventHandler<[EventName]Event>,
    ITransientDependency
{
    public async Task HandleEventAsync([EventName]Event eventData)
    {
        // Handle the event
    }
}
```

### Best Practices للـ Handlers
1. **Idempotent**: يجب أن يكون Handler آمن عند التنفيذ أكثر من مرة
2. **Fast**: Handler يجب أن يكون سريعاً (< 100ms للـ Local Events)
3. **Error Handling**: معالجة الأخطاء بشكل صحيح
4. **Logging**: تسجيل ما يحدث
5. **Retry Logic**: إعادة المحاولة في حال فشل (للـ Distributed Events)

---

## Event Sourcing (إذا مُستخدَم)

### Event Store
[وصف كيفية تخزين الأحداث]

### Event Replay
[وصف كيفية إعادة تشغيل الأحداث]

### Snapshots
[وصف كيفية استخدام Snapshots لتحسين الأداء]

---

## Integration Events

الأحداث المستخدمة للتكامل مع أنظمة خارجية.

### [IntegrationEventName]

**الوصف**: [وصف الحدث]

**Format**:
```json
{
  "eventId": "...",
  "eventType": "...",
  "timestamp": "...",
  "data": {...}
}
```

**Target Systems**:
- [نظام 1]
- [نظام 2]

---

## Monitoring & Debugging

### Event Tracking
- تسجيل جميع الأحداث المُطلقة
- تتبع Handlers المنفذة
- قياس وقت المعالجة

### Common Issues
| Issue | Solution |
|-------|----------|
| Handler لا يُنفذ | تحقق من التسجيل في DI Container |
| Event يُنفذ مرتين | تحقق من Idempotency |
| بطء في المعالجة | استخدم Distributed Event بدلاً من Local |

---

## ملاحظات التنفيذ

- استخدام ABP Event Bus (Local & Distributed)
- التسجيل التلقائي للـ Handlers
- استخدام Outbox Pattern للـ Distributed Events لضمان Reliability
- Retry Policy: 3 محاولات مع Exponential Backoff

