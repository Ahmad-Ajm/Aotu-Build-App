قالب
# Background Jobs

المهام الخلفية (Background Jobs) التي تُنفذ بشكل غير متزامن.

---

## قالب Background Job

### [JobName]Job

**الوصف**: [وصف المهمة والغرض منها]

**Type**: Recurring / One-time / Delayed

**Schedule** (للـ Recurring):
- **Cron Expression**: `[cron expression]`
- **Frequency**: [مثل: كل ساعة، يومياً، أسبوعياً]
- **Next Run**: [الوقت التالي]

**Execution Time**: [< X minutes]

**Priority**: High / Medium / Low

**Arguments**:
| Argument | Type | Description |
|----------|------|-------------|
| [Arg1] | [Type] | [الوصف] |

**Implementation**:
```csharp
public class [JobName]Job : AsyncBackgroundJob<[JobArgs]>, ITransientDependency
{
    public override async Task ExecuteAsync([JobArgs] args)
    {
        // Job logic
    }
}
```

**Retry Policy**:
- **Max Retries**: [عدد]
- **Backoff Strategy**: Linear / Exponential
- **Delay Between Retries**: [المدة]

**Error Handling**:
- [كيف تُعالج الأخطاء]
- [متى تُعتبر failed]

**Monitoring**:
- [Metrics to track]
- [Alerts]

---

## قائمة Background Jobs

### Data Processing Jobs
| Job Name | Schedule | Duration | Description |
|----------|----------|----------|-------------|
| [Job1] | Hourly | ~5 min | [الوصف] |
| [Job2] | Daily 02:00 | ~30 min | [الوصف] |

### Maintenance Jobs
| Job Name | Schedule | Duration | Description |
|----------|----------|----------|-------------|
| CleanupTempFiles | Daily 03:00 | ~2 min | حذف الملفات المؤقتة |
| PurgeOldLogs | Weekly Sun 04:00 | ~10 min | حذف السجلات القديمة |

### Notification Jobs
| Job Name | Schedule | Duration | Description |
|----------|----------|----------|-------------|
| SendPendingEmails | Every 5 min | ~1 min | إرسال الإيميلات المعلقة |
| ProcessPushNotifications | Every 2 min | ~30 sec | إرسال الإشعارات |

### Synchronization Jobs
| Job Name | Schedule | Duration | Description |
|----------|----------|----------|-------------|
| SyncWith[ExternalSystem] | Every 15 min | ~3 min | مزامنة مع نظام خارجي |

### Report Generation Jobs
| Job Name | Schedule | Duration | Description |
|----------|----------|----------|-------------|
| GenerateDailyReport | Daily 08:00 | ~15 min | تقرير يومي |
| GenerateMonthlyReport | Monthly 1st 09:00 | ~1 hour | تقرير شهري |

---

## Job Queue Configuration

| Queue Name | Priority | Concurrency | Timeout |
|------------|----------|-------------|---------|
| default | Medium | 10 | 5 min |
| high-priority | High | 20 | 10 min |
| low-priority | Low | 5 | 30 min |

---

## Scheduling

### Cron Expressions
```
* * * * *  → Every minute
0 * * * *  → Every hour
0 0 * * *  → Daily at midnight
0 2 * * *  → Daily at 2 AM
0 0 * * 0  → Weekly on Sunday
0 0 1 * *  → Monthly on 1st
```

### Dynamic Scheduling
بعض المهام تُجدول ديناميكياً بناءً على:
- أحداث النظام
- طلبات المستخدمين
- شروط معينة

---

## Hangfire Configuration (إذا مُستخدَم)

```csharp
services.AddHangfire(config =>
{
    config.UseSqlServerStorage(connectionString);
    config.UseFilter(new AutomaticRetryAttribute { 
        Attempts = 3 
    });
});

services.AddHangfireServer(options =>
{
    options.WorkerCount = 20;
    options.Queues = new[] { "high-priority", "default", "low-priority" };
});
```

---

## Best Practices

1. **Idempotency**: Job يجب أن يكون آمن عند التنفيذ أكثر من مرة
2. **Logging**: تسجيل البداية والنهاية والأخطاء
3. **Timeouts**: تحديد Timeout لكل Job
4. **Monitoring**: مراقبة التنفيذ والأداء
5. **Error Handling**: معالجة الأخطاء وإعادة المحاولة

---

## ملاحظات التنفيذ

- استخدام Hangfire أو ABP Background Jobs
- استخدام Queue System (RabbitMQ / Azure Service Bus) للمهام الكبيرة
- تسجيل Jobs في Dependency Injection
- استخدام Database Storage لضمان عدم فقد Jobs

